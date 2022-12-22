using ConsoleTables;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace tolokonnikove
{
    class Program
    {
        static void Main(string[] args)
        {
            
            string login = "";
            string status = "";
            string statusName = "";
            bool work = true;
            bool log_in = true;

            Console.WriteLine(" Вас приветсвует компания \"ВСЁ ЧТО НАДО\"\n Для получение доступа к функционалу программы войдите в свой профиль.\n Если у вас нет своего профиля, то зарегестрируйтесь");
            Console.WriteLine("  \n\n\t\t\t\t\t\t\t\t\t*Примечание*\n\t\t\t\t\t\t\tВ программе используется функционнал ввода.\n\t\t\t\t\t\tПоэтому для работы с функционалом используйте команды ввода");
            Console.WriteLine(" \nЕсли вам необходимо авторизироваться напишите log, если зарегестрироваться напишите reg");
            
            while (log_in)
            {
                string res = Console.ReadLine();
                
                if (res == "log")
                {
                    Console.Write("Введите логин и пароль через пробел: ");
                    string[] dat = Data.Explode(" ", Console.ReadLine());

                    
                    if(dat.Length >= 2)
                    {
                        
                        if (Data.CheckExistFile(dat[0], "Users"))
                        {
                            string[,] Finduser = Data.GetUser(dat[0]); 
                            if (dat[0] == Finduser[0, 1]) 
                            {
                                if (dat[1] == Finduser[1, 1]) 
                                {
                                    log_in = false;
                                    login = Finduser[0, 1];
                                    status = Finduser[3, 1];
                                    switch (Finduser[3, 1]) 
                                    {
                                        case "buyer": statusName = "покупатель"; break;
                                        case "admin": statusName = "администратор"; break;
                                        case "hr": statusName = "HR"; break;
                                        case "warehouse": statusName = "кладовщик"; break;
                                        case "cassa": statusName = "кассир-продавец"; break;
                                        case "finance": statusName = "бухгалтер"; break;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Пароль не верный");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Пользователь не найден");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Пользователь не найден");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Не все параметры переданы") ;
                    }
                }
                else if(res == "reg") 
                {
                    Console.Write("Введите логин, пароль и email через пробел: ");
                    string[] dat = Data.Explode(" ", Console.ReadLine());
                    if(dat.Length >= 3) 
                    {
                        if(Data.CheckString(dat[0]) && Data.CheckString(dat[1]) && Data.CheckString(dat[2])) 
                        {
                            if (!Data.CheckExistFile(dat[0], "Users")) 
                            {
                                if(Data.CheckPass(dat[1])) 
                                {
                                    if(Data.CheckMail(dat[2])) 
                                    {
                                        Admin.UpdateUser(dat[0], dat[1], dat[2], "buyer"); 
                                        login = dat[0];
                                        status = "buyer";
                                        statusName = "покупатель";
                                        log_in = false;
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("Логин занят");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Не все параметры указаны");
                    }
                }
                else
                {
                    Console.WriteLine("Команда не опознана");
                }
            }

            Console.Clear();
            Console.WriteLine("Здравствуйте " + statusName + " " + login);

            
            while (work)
            {
                string comand = "";
                Console.WriteLine("\r\nВведите команду. Узнать список доступных команд можно введя команду help.");
                comand = Console.ReadLine();

                
                string[] dat = Data.Explode(" ", comand);

               
                if (dat[0] == "help")
                {
                    Console.WriteLine("Команды указанны в следующем порядке - Команда Аргумент1 Аргумент2 АргументN - описание");
                    Console.WriteLine("\r\nСписок общих команд\r\n" + new string('_', 50));
                    var table = new ConsoleTable("Команда", "Параметры", "Описание");

                    table.AddRow(new string[] { "help", "", "Вывести список доступных команд" });
                    table.AddRow(new string[] { "exit", "", "Выход" });
                    table.AddRow(new string[] { "clear", "", "Очистить консоль" });

                    table.Write();

                    if (status == "admin")
                    {
                        string[] usersN = new string[] { "buyer", "admin",  "hr", "warehouse", "cassa", "finance" };
                        for(int i = 0; i < usersN.Length;i++)
                        {
                            Data.GetHelp(usersN[i]);
                        }
                    }
                    else
                    {
                        Data.GetHelp(status);
                    }
                }
                else if(dat[0] == "exit")
                {
                    work = false;
                }
                else if(dat[0] == "clear")
                {
                    Console.Clear();
                }
                else if(dat[0] == "delete_user")
                {
                    if (dat.Length >= 2)
                    {
                        if (status == "admin")
                        {
                            Admin.DeleteUser(dat[1]);
                        }
                        else
                        {
                            Console.WriteLine("Доступ заблокирован");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Не все параметры указаны");
                    }
                }
                else if(dat[0] == "change_user")
                {
                    if (dat.Length >= 2)
                    {
                        if (status == "admin")
                        {
                            Admin.ChangeUser(dat[1]);
                        }
                        else
                        {
                            Console.WriteLine("Доступ заблокирован");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Не все параметры указаны");
                    }
                }
                else if(dat[0] == "add_user")
                {
                    if (status == "admin")
                    {
                        Admin.AddUser();
                    }
                    else
                    {
                        Console.WriteLine("Доступ заблокирован");
                    }
                }
                else if(dat[0] == "add_product")
                {
                    if (status == "admin")
                    {
                        Admin.AddProduct();
                    }
                    else
                    {
                        Console.WriteLine("Доступ заблокирован");
                    }
                }
                else if(dat[0] == "change_product")
                {
                    if (dat.Length >= 2)
                    {
                        if (status == "admin")
                        {
                            Admin.ChangeProduct(dat[1]);
                        }
                        else
                        {
                            Console.WriteLine("Доступ заблокирован");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Не все параметры указаны");
                    }
                }
                else if(dat[0] == "delete_product")
                {
                    if(dat.Length >= 2)
                    {
                        if (status == "admin")
                        {
                            Admin.DeleteProduct(dat[1]);
                        }
                        else
                        {
                            Console.WriteLine("Доступ заблокирован");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Не все параметры указаны");
                    }
                }
                else if(dat[0] == "get_products")
                {
                    if (status == "admin" || status == "warehouse" || status == "buyer")
                    {
                        Warehouse.GetProducts();
                    }
                    else
                    {
                        Console.WriteLine("Доступ заблокирован");
                    }
                }
                else if(dat[0] == "update_product_count")
                {
                    if (dat.Length >= 2)
                    {
                        if (status == "admin" || status == "warehouse")
                        {
                            Warehouse.UpdateProductByParam("count", dat[1]);
                        }
                        else
                        {
                            Console.WriteLine("Доступ заблокирован");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Не все параметры указаны");
                    }
                }
                else if(dat[0] == "unready_product")
                {
                    if (dat.Length >= 2)
                    {
                        if (status == "admin" || status == "warehouse")
                        {
                            Warehouse.UpdateProductByParam("ready",dat[1]);
                        }
                        else
                        {
                            Console.WriteLine("Доступ заблокирован");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Не все параметры указаны");
                    }
                }
                else if(dat[0] == "transfer_product")
                {
                    if (dat.Length >= 2)
                    {
                        if (status == "admin" || status == "warehouse")
                        {
                            Warehouse.UpdateProductByParam("warehouse", dat[1]);
                        }
                        else
                        {
                            Console.WriteLine("Доступ заблокирован");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Не все параметры указаны");
                    }
                }
                else if(dat[0] == "get_users")
                {
                    if (status == "admin" || status == "hr")
                    {
                        HR.GetUsers(status);
                    }
                    else
                    {
                        Console.WriteLine("Доступ заблокирован");
                    }
                }
                else if(dat[0] == "add_worker")
                {
                    if (dat.Length >= 2)
                    {
                        if (status == "admin" || status == "hr")
                        {
                            HR.ChangeUserByType(dat[1],"add");
                        }
                        else
                        {
                            Console.WriteLine("Доступ заблокирован");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Не все параметры указаны");
                    }
                }
                else if(dat[0] == "delete_worker")
                {
                    if (dat.Length >= 2)
                    {
                        if (status == "admin" || status == "hr")
                        {
                            HR.ChangeUserByType(dat[1], "delete");
                        }
                        else
                        {
                            Console.WriteLine("Доступ заблокирован");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Не все параметры указаны");
                    }
                }
                else if(dat[0] == "change_worker")
                {
                    if (dat.Length >= 2)
                    {
                        if (status == "admin" || status == "hr")
                        {
                            HR.ChangeUserByType(dat[1], "change");
                        }
                        else
                        {
                            Console.WriteLine("Доступ заблокирован");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Не все параметры указаны");
                    }
                }
                else if(dat[0] == "check_chart")
                {
                    if (status == "admin" || status == "cassa")
                    {
                        if (dat.Length >= 2)
                        {
                            Buyer.CheckChart(dat[1]);
                        }
                        else
                        {
                            Console.WriteLine("Не все параметры указаны");
                        }
                    }
                    else if(status == "buyer")
                    {
                        Buyer.CheckChart(login);
                    }
                    else
                    {
                        Console.WriteLine("Доступ заблокирован");
                    }
                }
                else if(dat[0] == "add_chart")
                {
                    if (status == "admin" || status == "buyer")
                    {
                        if (dat.Length >= 3)
                        {
                            if(status == "admin")
                            {
                                if (dat.Length >= 4)
                                {
                                    Buyer.AddChart(dat[3], dat[1], dat[2]);
                                }
                                else
                                {
                                    Console.WriteLine("Не указан логин");
                                }
                            }
                            else if(status == "buyer")
                            {
                                Buyer.AddChart(login, dat[1], dat[2]);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Не все параметры указаны");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Доступ заблокирован");
                    }
                }
                else if(dat[0] == "change_chart")
                {
                    if (status == "admin" || status == "buyer")
                    {
                        if (dat.Length >= 3)
                        {
                            if (status == "admin")
                            {
                                if (dat.Length >= 4)
                                {
                                    Buyer.ChangeChart(dat[3], dat[1], dat[2]);
                                }
                                else
                                {
                                    Console.WriteLine("Не указан логин");
                                }
                            }
                            else if (status == "buyer")
                            {
                                Buyer.ChangeChart(login, dat[1], dat[2]);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Не все параметры указаны");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Доступ заблокирован");
                    }
                }
                else if(dat[0] == "delete_chart")
                {
                    if (status == "admin" || status == "buyer")
                    {
                        if (dat.Length >= 2)
                        {
                            if (status == "admin")
                            {
                                if (dat.Length >= 3)
                                {
                                    Buyer.DeleteChart(dat[2], dat[1]);
                                }
                                else
                                {
                                    Console.WriteLine("Не указан логин");
                                }
                            }
                            else if (status == "buyer")
                            {
                                Buyer.DeleteChart(login, dat[1]);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Не все параметры указаны");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Доступ заблокирован");
                    }
                }
                else if(dat[0] == "get_chart_list")
                {
                    if (status == "admin" || status == "cassa")
                    {
                        Cassa.GetChartList();
                    }
                    else
                    {
                        Console.WriteLine("Доступ заблокирован");
                    }
                }
                else if(dat[0] == "complete_order")
                {
                    if (status == "admin" || status == "cassa")
                    {
                        if (dat.Length >= 2)
                        {
                            Cassa.CompleteOrder(dat[1]);
                        }
                        else
                        {
                            Console.WriteLine("Не все параметры указаны");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Доступ заблокирован");
                    }
                }
                else if(dat[0] == "get_order_list")
                {
                    if (status == "admin" || status == "finance")
                    {
                        Finance.GetOrderList();
                    }
                    else
                    {
                        Console.WriteLine("Доступ заблокирован");
                    }
                }
                else if(dat[0] == "get_order")
                {
                    if (status == "admin" || status == "finance")
                    {
                        if (dat.Length >= 2)
                        {
                            Finance.GetOrderByPeriod(dat[1]);
                        }
                        else
                        {
                            Console.WriteLine("Не указан период");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Доступ заблокирован");
                    }
                }
                else if(dat[0] == "check_order")
                {
                    if (status == "admin" || status == "finance")
                    {
                        if (dat.Length >= 2)
                        {
                            Finance.CheckOrder(dat[1]);
                        }
                        else
                        {
                            Console.WriteLine("Не все параметры указаны");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Доступ заблокирован");
                    }
                }
                else if(dat[0] == "send_payment")
                {
                    if (status == "admin" || status == "finance")
                    {
                        Finance.SendPayments();
                    }
                    else
                    {
                        Console.WriteLine("Доступ заблокирован");
                    }
                }
                else if(dat[0] == "get_payment")
                {
                    if (status == "admin" || status == "finance")
                    {
                        if (dat.Length >= 2)
                        {
                            Finance.GetPaymentsByPeriod(dat[1]);
                        }
                        else
                        {
                            Console.WriteLine("Не указан период");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Доступ заблокирован");
                    }
                }
                else if(dat[0] == "get_budget")
                {
                    if (status == "admin" || status == "finance")
                    {
                        if (dat.Length >= 2)
                        {
                            Finance.GetBudgetByPeriod(dat[1]);
                        }
                        else
                        {
                            Console.WriteLine("Не указан период");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Доступ заблокирован");
                    }
                }
                else
                {
                    Console.WriteLine("Команда не распознана");
                }
            }
        }
    }
}
