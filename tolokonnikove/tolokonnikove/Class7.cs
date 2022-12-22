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
    class Finance
    {
        
        public static void GetOrderByPeriod(string period)
        {
            Console.Clear();
            List<string> data = Data.GetFilesByPeriod(period, "Orders"); 
            if (data.Count > 0)
            {
                double res = 0;
                
                var table = new ConsoleTable("Пользователь", "Товар", "Количество", "Цена", "Дата");

                
                for (int j = 0; j < data.Count; j++)
                {
                    string[][] dat = Data.GetOrder(data[j]); 

                    
                    for (int i = 0; i < dat.Length - 1; i++)
                    {
                        table.AddRow(new string[] { dat[i][0], dat[i][1], dat[i][2], dat[i][3], dat[i][4] });
                        res += Convert.ToDouble(dat[i][3]);
                    }
                }
                table.Write();
                Console.WriteLine("Общий доход: " + res);
            }
            else
            {
                Console.WriteLine("Записей за период не найдено");
            }
        }
        
        public static void CheckOrder(string order)
        {
            if (Data.CheckExistFile(order, "Orders"))
            {
                string[][] data = Data.GetOrder(order); 

                var table = new ConsoleTable("Пользователь", "Товар", "Количество", "Цена", "Дата");

                
                for (int i = 0; i < data.Length - 1; i++)
                {
                    table.AddRow(new string[] { data[i][0], data[i][1], data[i][2], data[i][3], data[i][4] });
                }

                table.Write();
            }
            else
            {
                Console.WriteLine("Заказ не найден");
            }
        }
        
        public static void GetOrderList()
        {
            string[] files = Directory.GetFiles(@"Data\Orders\");
            int n = 0;
            var table = new ConsoleTable("Заказ");

            
            foreach (string filename in files)
            {
                n++;
                string[] finded = Data.Explode("\\", filename);
                finded = Data.Explode(".", finded[finded.Length - 1]);
                table.AddRow(finded[0]);
            }
            table.Write();
        }
       
        public static void SendPayments()
        {
            string[] files = Directory.GetFiles(@"Data\Users\");

            string date = "";
            string[] Ntime = Data.Explode(" ", DateTime.Now.ToString());
            string time = Ntime[0];

            
            for (int i = 0; i < time.Length; i++)
            {
                if (time[i] == '.') { date += "-"; }
                else { date += time[i]; }
            }

           
            if (!Data.CheckExistFile(date, "Payments"))
            {
                string writePath = @"Data\Payments\" + date + ".dat"; 
                using (StreamWriter sw = new StreamWriter(writePath))
                {
                    
                    foreach (string filename in files)
                    {
                        string[] finded = Data.Explode("\\", filename);
                        finded = Data.Explode(".", finded[finded.Length - 1]);
                        string[,] user = Data.GetUser(finded[0]); 
                        string[] userDat = new string[10];
                        if (user[9, 1] != "") 
                        {
                            if (Data.CheckInt(user[9, 1])) 
                            {
                                sw.WriteLine("user[" + user[0, 1] + "]");
                                sw.WriteLine("payment[" + user[9, 1] + "];");
                            }
                        }
                    }
                    sw.Close();
                }
                Console.WriteLine("Зарплаты выплачены");
            }
            else
            {
                Console.WriteLine("В этом месяце зарпалы уже выплачивались");
            }
        }
        
        public static void GetPaymentsByPeriod(string period)
        {
            Console.Clear();
            List<string> data = Data.GetFilesByPeriod(period, "Payments");
            if (data.Count > 0)
            {
                double res = 0;
                var table = new ConsoleTable("Пользователь", "Зарплата", "Дата");

                
                for (int j = 0; j < data.Count; j++)
                {
                    string[][] dat = Data.GetPayment(data[j]);

                    
                    for (int i = 0; i < dat.Length - 1; i++)
                    {
                        table.AddRow(new string[] { dat[i][0], dat[i][1], data[j] });
                        res += Convert.ToDouble(dat[i][1]);
                    }
                }
                table.Write();
                Console.WriteLine("Всего выплачено: " + res);
            }
            else
            {
                Console.WriteLine("Записей за период не найдено");
            }
        }
        
        public static void GetBudgetByPeriod(string period)
        {
            Console.Clear();

            double outcome = 0;
            double income = 0;

            Console.WriteLine("Продажи за период: \r\n" + new string('_', 50) + "\r\n");

            
            List<string> data = Data.GetFilesByPeriod(period, "Orders");
            if (data.Count > 0)
            {
                var table = new ConsoleTable("Пользователь", "Товар", "Количество", "Цена", "Дата");

                for (int j = 0; j < data.Count; j++)
                {
                    string[][] dat = Data.GetOrder(data[j]);

                    for (int i = 0; i < dat.Length - 1; i++)
                    {
                        table.AddRow(new string[] { dat[i][0], dat[i][1], dat[i][2], dat[i][3], dat[i][4] });
                        income += Convert.ToDouble(dat[i][3]);
                    }
                }
                table.Write();
                income = income * 0.9398;
                Console.WriteLine("Общий доход учетом налогов: " + income);
            }
            else
            {
                Console.WriteLine("Записей за период не найдено");
            }

            data.Clear();

            Console.WriteLine("\r\n\r\nВыплаты ЗП за период: \r\n" + new string('_', 50) + "\r\n");

            
            data = Data.GetFilesByPeriod(period, "Payments");
            if (data.Count > 0)
            {
                var table = new ConsoleTable("Пользователь", "Зарплата", "Дата");

                for (int j = 0; j < data.Count; j++)
                {
                    string[][] dat = Data.GetPayment(data[j]);

                    for (int i = 0; i < dat.Length - 1; i++)
                    {
                        table.AddRow(new string[] { dat[i][0], dat[i][1], data[j] });
                        outcome += Convert.ToDouble(dat[i][1]);
                    }
                }
                table.Write();
                Console.WriteLine("Всего выплачено: " + outcome);
            }
            else
            {
                Console.WriteLine("Записей за период не найдено");
            }
            
            Console.WriteLine("Состояние бюджета за период: " + (income - outcome));
        }
    }
}
