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
    class Cassa
    {
        
        public static void GetChartList()
        {
            string[] files = Directory.GetFiles(@"Data\Charts\");
            int n = 0;
            var table = new ConsoleTable("Пользователь");

            foreach (string filename in files)
            {
                n++;
                string[] finded = Data.Explode("\\", filename);
                finded = Data.Explode(".", finded[finded.Length - 1]); 
                table.AddRow(finded[0]);
            }
            table.Write();
        }
        
        public static void CompleteOrder(string order)
        {
            
            if (Data.CheckExistFile(order, "Charts"))
            {
                string[][] data = Data.GetChart(order);
                string[] price = new string[data.Length - 1];
                string[] price_one = new string[data.Length - 1]; 
                bool access = true;

                
                for (int i = 0; i < data.Length - 1; i++)
                {
                    string[,] prod = Data.GetProduct(data[i][0]);

                   
                    for (int j = 0; j < prod.Length / 2 - 1; j++)
                    {
                        if (prod[j, 0] == "count")
                        {
                            if (Convert.ToInt32(prod[j, 1]) < Convert.ToInt32(data[i][1]))
                            {
                                Console.WriteLine("Недостаточно " + (Convert.ToInt32(data[i][1]) - Convert.ToInt32(prod[j, 1])) + " ед. продукции " + data[i][0]);
                                access = false;
                            }
                        }
                        else if (prod[j, 0] == "price") 
                        {
                            price[i] = (Convert.ToDouble(prod[j, 1]) * Convert.ToInt32(data[i][1])).ToString();
                            price_one[i] = prod[j, 1];
                            if (DateTime.Now > DateTime.Parse(prod[5, 1]))
                            {
                                price[i] = (Convert.ToDouble(price[i]) * 0.5).ToString();
                                price_one[i] = (Convert.ToDouble(price[i]) * 0.5).ToString() + " (-50%)";
                            }
                        }
                    }

                }

                
                if (access)
                {
                    string date = "";
                    string time = DateTime.Now.ToString();

                    
                    for (int i = 0; i < time.Length; i++)
                    {
                        if (time[i] == ' ') { date += "_"; }
                        else if (time[i] == '.') { date += "-"; }
                        else if (time[i] == ':') { date += "-"; }
                        else { date += time[i]; }
                    }

                    string[,] user = Data.GetUser(order); 

                    
                    string text = "Thanks. Your order:<br>";

                    for (int i = 0; i < data.Length - 1; i++)
                    {
                        text += "<hr>";
                        text += "Product - " + data[i][0] + "<br>";
                        text += "Count - " + data[i][1] + "<br>";
                        text += "Summary cost - " + price[i] + "<br>";
                        text += "Cost per unit - " + price_one[i] + "<br>";
                    }

                    text += "<hr>";
                    text += "<br>Date - " + DateTime.Now + "<br>";

                    
                    if (Data.SendMail(text, user[2, 1]))
                    {
                        string writePath = @"Data\Orders\" + date + ".dat";
                        using (StreamWriter sw = new StreamWriter(writePath))
                        {
                            for (int i = 0; i < data.Length - 1; i++)
                            {
                                sw.WriteLine("user[" + order + "]");
                                sw.WriteLine("product[" + data[i][0] + "]");
                                sw.WriteLine("count[" + data[i][1] + "]");
                                sw.WriteLine("price[" + price[i] + "]");
                                sw.WriteLine("price_one[" + price_one[i] + "]");
                                sw.WriteLine("date[" + date + "];");
                            }
                            sw.Close();
                        }

                        
                        for (int j = 0; j < data.Length - 1; j++)
                        {
                            string[,] prod = Data.GetProduct(data[j][0]);
                            string[] dat = new string[7];

                            for (int i = 0; i < dat.Length; i++)
                            {
                                if (prod[i, 0] == "count")
                                {
                                    dat[i] = (Convert.ToInt32(prod[i, 1]) - Convert.ToInt32(data[j][1])).ToString();
                                }
                                else
                                {
                                    dat[i] = prod[i, 1];
                                }
                            }
                            Admin.UpdateProduct(dat[0], dat[1], dat[2], dat[3], dat[4], dat[5], dat[6]); 
                        }
                        File.Delete("Data\\Charts\\" + order + ".dat"); 
                        Console.WriteLine("Заказ оформлен");
                    }
                    else
                    {
                        Console.WriteLine("Не получилось отправить электронное сообщение");
                    }
                }
            }
            else
            {
                Console.WriteLine("Заказ не найден");
            }
        }
    }
}
