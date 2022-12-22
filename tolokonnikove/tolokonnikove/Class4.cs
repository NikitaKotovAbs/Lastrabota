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
    class HR
    {
        
        public static void GetUsers(string status)
        {
            string[] files = Directory.GetFiles(@"Data\Users\");
            int n = 0;

            
            var table = new ConsoleTable("Логин", "Пароль", "Почта", "Статус", "ФИО", "Образование", "Опыт", "Должность", "Место работы", "ЗП");

            
            foreach (string filename in files)
            {
                n++;
                string[] finded = Data.Explode("\\", filename);
                finded = Data.Explode(".", finded[finded.Length - 1]);
                string[,] user = Data.GetUser(finded[0]);
                string[] userDat = new string[10];
                for (int i = 0; i < 10; i++)
                {
                    if (status == "hr" && i == 1) 
                    {
                        userDat[i] = new string('*', user[i, 1].Length);
                    }
                    else
                    {
                        userDat[i] = user[i, 1];
                    }
                }
                table.AddRow(userDat);
            }
            table.Write();
        }
        
        public static void ChangeUserByType(string login, string type)
        {
            if (Data.CheckExistFile(login, "Users"))
            {
                string[,] user = Data.GetUser(login);
                string[] dat = new string[10];

                Console.WriteLine(new string('_', 50));

                
                if (type == "delete")
                {
                    dat[3] = "buyer"; 
                    for (int i = 0; i < 4; i++)
                    {
                        dat[i] = user[i, 1];
                    }
                    for (int i = 4; i < 9; i++)
                    {
                        dat[i] = "-"; 
                    }
                    dat[9] = "0.00"; 

                    
                    Admin.UpdateUser(dat[0], dat[1], dat[2], dat[3], dat[4], dat[5], dat[6], dat[7], dat[8], dat[9]);

                    Console.WriteLine("Пользователь уволен");
                }
                else 
                {
                    Console.WriteLine("Текущие данные:\r\n");

                    for (int i = 0; i < 10; i++)
                    {
                        if (i == 1) 
                        {
                            Console.WriteLine(user[i, 0] + " : " + new string('*', user[i, 1].Length));
                        }
                        else
                        {
                            Console.WriteLine(user[i, 0] + " : " + user[i, 1]);
                        }
                    }

                    for (int i = 0; i < 4; i++)
                    {
                        dat[i] = user[i, 1];
                    }

                    
                    Console.WriteLine("\r\n");
                    for (int i = 4; i < 10; i++)
                    {
                        bool access = true;
                        while (access)
                        {
                            Console.Write("Введите новый " + user[i, 0] + ": ");
                            dat[i] = Console.ReadLine();
                            if (Data.CheckString(dat[i]))
                            {
                                if (user[i, 0] == "pay")
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
                        Admin.UpdateUser(dat[0], dat[1], dat[2], dat[3], dat[4], dat[5], dat[6], dat[7], dat[8], dat[9]);
                    }
                    else
                    {
                        Console.WriteLine("Операция отменена");
                    }
                }
            }
            else
            {
                Console.WriteLine("Пользователь не найден");
            }
        }
    }
    internal class Class4
    {
    }
}
