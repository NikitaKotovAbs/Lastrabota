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
    class Admin
    {
        
        public static void UpdateUser(string login, string pass, string mail, string status, string fio = "-", string education = "-", string exp = "-", string special = "-", string work = "-", string pay = "0.00")
        {
            
            string writePath = @"Data\Users\" + login + ".dat";
            using (StreamWriter sw = new StreamWriter(writePath))
            {
                sw.WriteLine("login[" + login + "]");
                sw.WriteLine("pass[" + pass + "]");
                sw.WriteLine("mail[" + mail + "]");
                sw.WriteLine("status[" + status + "]");
                sw.WriteLine("fio[" + fio + "]");
                sw.WriteLine("education[" + education + "]");
                sw.WriteLine("exp[" + exp + "]");
                sw.WriteLine("special[" + special + "]");
                sw.WriteLine("work[" + work + "]");
                sw.WriteLine("pay[" + pay + "];");
                sw.Close();
            }
        }
        
        public static void DeleteUser(string login)
        {
            
            if (Data.CheckExistFile(login, "Users"))
            {
                
                File.Delete("Data\\Users\\" + login + ".dat");
                Console.WriteLine("Пользователь удален");
            }
            else
            {
                Console.WriteLine("Пользователь не найден");
            }
        }
        
        public static void ChangeUser(string login)
        {
            if (Data.CheckExistFile(login, "Users"))
            {
                
                string[,] user = Data.GetUser(login);
                string[] dat = new string[10];

                Console.WriteLine(new string('_', 50));
                Console.WriteLine("Если пользователь - не сотрудник, значения фио и далее указывайте как '-', зп как 0.00");
                Console.WriteLine("Текущие данные:\r\n");

                
                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine(user[i, 0] + " : " + user[i, 1]);
                }
                Console.WriteLine("\r\n");
                for (int i = 0; i < 10; i++)
                {
                    
                    bool access = true;
                    while (access)
                    {
                        Console.Write("Введите новый " + user[i, 0] + ": ");
                        dat[i] = Console.ReadLine();
                        if (Data.CheckString(dat[i]))
                        {
                            if (user[i, 0] == "login")
                            {
                                dat[i] = user[i, 1];
                                access = false;
                                Console.WriteLine("Логин изменить нельзя");
                            }
                            else if (user[i, 0] == "pass")
                            {
                                if (Data.CheckPass(dat[i]))
                                {
                                    access = false;
                                }
                            }
                            else if (user[i, 0] == "mail")
                            {
                                if (Data.CheckMail(dat[i]))
                                {
                                    access = false;
                                }
                            }
                            else if (user[i, 0] == "status")
                            {
                                if (Data.CheckStatus(dat[i]))
                                {
                                    access = false;
                                }
                            }
                            else if (user[i, 0] == "pay")
                            {
                                try
                                {
                                    double n = Convert.ToDouble(dat[i]);
                                    access = false;
                                }
                                catch
                                {
                                    Console.WriteLine("ЗП должно быть дробным числом (5.00, 23.50 ...)");
                                }
                            }
                            else
                            {
                                access = false;
                            }
                        }
                    }
                }
                Console.Write("Перепроверьте данные, для подтверждения операции введите y (eng) , для отмены любой другой символ: ");
                string s = Console.ReadLine();
                if (s == "y")
                {
                   
                    UpdateUser(dat[0], dat[1], dat[2], dat[3], dat[4], dat[5], dat[6], dat[7], dat[8], dat[9]);
                }
                else
                {
                    Console.WriteLine("Операция отменена");
                }
            }
            else
            {
                Console.WriteLine("Пользователь не найден");
            }
        }
        
        public static void AddUser()
        {
            
            string[] dat = new string[10];
            string[] title = new string[] { "login", "pass", "mail", "status", "fio", "education", "exp", "special", "work", "pay" };

            Console.WriteLine(new string('_', 50));
            Console.WriteLine("Если пользователь - не сотрудник, значения фио и далее указывайте как '-', зп как 0.00");
            Console.WriteLine("Добавление пользователя:\r\n");
            Console.WriteLine("\r\n");
            for (int i = 0; i < 10; i++)
            {
                
                bool access = true;
                while (access)
                {
                    Console.Write("Введите " + title[i] + ": ");
                    dat[i] = Console.ReadLine();
                    if (Data.CheckString(dat[i]))
                    {
                        if (title[i] == "login")
                        {
                            if (!Data.CheckExistFile(dat[i], "Users"))
                            {
                                access = false;
                            }
                            else
                            {
                                Console.WriteLine("Логин занят");
                            }
                        }
                        else if (title[i] == "pass")
                        {
                            if (Data.CheckPass(dat[i]))
                            {
                                access = false;
                            }
                        }
                        else if (title[i] == "mail")
                        {
                            if (Data.CheckMail(dat[i]))
                            {
                                access = false;
                            }
                        }
                        else if (title[i] == "status")
                        {
                            if (Data.CheckStatus(dat[i]))
                            {
                                access = false;
                            }
                        }
                        else if (title[i] == "pay")
                        {
                            try
                            {
                                double n = Convert.ToDouble(dat[i]);
                                access = false;
                            }
                            catch
                            {
                                Console.WriteLine("ЗП должно быть дробным числом (5.00, 23.50 ...)");
                            }
                        }
                        else
                        {
                            access = false;
                        }
                    }
                }
            }
            Console.Write("Перепроверьте данные, для подтверждения операции введите y (eng) , для отмены любой другой символ: ");
            string s = Console.ReadLine();
            if (s == "y")
            {
                
                UpdateUser(dat[0], dat[1], dat[2], dat[3], dat[4], dat[5], dat[6], dat[7], dat[8], dat[9]);
            }
            else
            {
                Console.WriteLine("Операция отменена");
            }
        }
        
        public static void UpdateProduct(string shop, string warehouse, string category, string product, string count, string ready, string price)
        {
            
            string writePath = @"Data\Products\" + product + ".dat";
            using (StreamWriter sw = new StreamWriter(writePath))
            {
                sw.WriteLine("shop[" + shop + "]");
                sw.WriteLine("warehouse[" + warehouse + "]");
                sw.WriteLine("category[" + category + "]");
                sw.WriteLine("product[" + product + "]");
                sw.WriteLine("count[" + count + "]");
                sw.WriteLine("ready[" + ready + "]");
                sw.WriteLine("price[" + price + "];");
                sw.Close();
            }
        }
        
        public static void AddProduct()
        {
            
            string[] dat = new string[7];
            string[] title = new string[] { "shop", "warehouse", "category", "product", "count", "ready", "price" };

            Console.WriteLine(new string('_', 50));
            Console.WriteLine("Добавление товара:\r\n");
            for (int i = 0; i < dat.Length; i++)
            {
                bool access = true;
                while (access)
                {
                    Console.Write("Введите " + title[i] + ": ");
                    dat[i] = Console.ReadLine();
                    if (Data.CheckString(dat[i]))
                    {
                        if (title[i] == "product")
                        {
                            if (!Data.CheckExistFile(dat[i], "Products"))
                            {
                                access = false;
                            }
                            else
                            {
                                Console.WriteLine("Товар уже существует");
                            }
                        }
                        else if (title[i] == "count")
                        {
                            try
                            {
                                int n = Convert.ToInt32(dat[i]);
                                access = false;
                            }
                            catch
                            {
                                Console.WriteLine("Количество должно быть целочисленным");
                            }
                        }
                        else if (title[i] == "price")
                        {
                            try
                            {
                                double n = Convert.ToDouble(dat[i]);
                                access = false;
                            }
                            catch
                            {
                                Console.WriteLine("Цена должна быть дробным числом (5,00 , 23,50 ...)");
                            }
                        }
                        else if (title[i] == "ready")
                        {
                            DateTime dt;
                            bool parse = DateTime.TryParse(dat[i], out dt); 
                            if (parse)
                            {
                                access = false;
                            }
                            else
                            {
                                Console.WriteLine("Срок годности должен быть в виде даты (25.12.2020)");
                            }
                        }
                        else
                        {
                            access = false;
                        }
                    }
                }
            }
            Console.Write("Перепроверьте данные, для подтверждения операции введите y (eng) , для отмены любой другой символ: ");
            string s = Console.ReadLine();
            if (s == "y")
            {
                Admin.UpdateProduct(dat[0], dat[1], dat[2], dat[3], dat[4], dat[5], dat[6]);
            }
            else
            {
                Console.WriteLine("Операция отменена");
            }
        }
        
        public static void ChangeProduct(string product)
        {
            if (Data.CheckExistFile(product, "Products"))
            {
                
                string[,] prod = Data.GetProduct(product);
                string[] dat = new string[7];

                Console.WriteLine(new string('_', 50));
                Console.WriteLine("Текущие данные:\r\n");

                for (int i = 0; i < dat.Length; i++)
                {
                    Console.WriteLine(prod[i, 0] + " : " + prod[i, 1]);
                }
                Console.WriteLine("\r\n");
                for (int i = 0; i < dat.Length; i++)
                {
                    bool access = true;
                    while (access)
                    {
                        Console.Write("Введите новый " + prod[i, 0] + ": ");
                        dat[i] = Console.ReadLine();
                        if (Data.CheckString(dat[i]))
                        {
                            if (prod[i, 0] == "product")
                            {
                                dat[i] = prod[i, 1];
                                Console.WriteLine("Имя товара изменить нельзя");
                                access = false;
                            }
                            else if (prod[i, 0] == "count")
                            {
                                try
                                {
                                    int n = Convert.ToInt32(dat[i]);
                                    access = false;
                                }
                                catch
                                {
                                    Console.WriteLine("Количество должно быть целочисленным");
                                }
                            }
                            else if (prod[i, 0] == "price")
                            {
                                try
                                {
                                    double n = Convert.ToDouble(dat[i]);
                                    access = false;
                                }
                                catch
                                {
                                    Console.WriteLine("Цена должна быть дробным числом (5,00  23,50 ...)");
                                }
                            }
                            else if (prod[i, 0] == "ready")
                            {
                                DateTime dt;
                                bool parse = DateTime.TryParse(dat[i], out dt);
                                if (parse)
                                {
                                    access = false;
                                }
                                else
                                {
                                    Console.WriteLine("Срок годности должен быть в виде даты (25.12.2020)");
                                }
                            }
                            else
                            {
                                access = false;
                            }
                        }
                    }
                }
                Console.Write("Перепроверьте данные, для подтверждения операции введите y (eng) , для отмены любой другой символ: ");
                string s = Console.ReadLine();
                if (s == "y")
                {
                    UpdateProduct(dat[0], dat[1], dat[2], dat[3], dat[4], dat[5], dat[6]);
                }
                else
                {
                    Console.WriteLine("Операция отменена");
                }
            }
            else
            {
                Console.WriteLine("Товар не найден");
            }
        }
        
        public static void DeleteProduct(string product)
        {
            
            if (Data.CheckExistFile(product, "Products"))
            {
                File.Delete("Data\\Products\\" + product + ".dat");
                Console.WriteLine("Товар удален");
            }
            else
            {
                Console.WriteLine("Товар не найден");
            }
        }
    }
}
