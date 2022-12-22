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
    class Buyer
    {
        
        public static void UpdateChart(string login, string data)
        {
            
            string writePath = @"Data\Charts\" + login + ".dat";
            string[] exp = Data.Explode("]", data);
            using (StreamWriter sw = new StreamWriter(writePath))
            {
                for (int i = 0; i < exp.Length - 1; i = i + 2)
                {
                    sw.WriteLine(exp[i] + "]");
                    sw.WriteLine(exp[i + 1] + "];");
                }
                sw.Close();
            }
            Console.WriteLine("Корзина обновлена");
        }
        
        public static void CheckChart(string login)
        {
            
            if (Data.CheckExistFile(login, "Charts"))
            {
                
                string[][] data = Data.GetChart(login);

                
                var table = new ConsoleTable("Товар", "Количество");

                for (int i = 0; i < data.Length - 1; i++)
                {
                    table.AddRow(new string[] { data[i][0], data[i][1] });
                }

                table.Write();
            }
            else
            {
                Console.WriteLine("Корзины еще нет");
            }
        }
        
        public static void AddChart(string login, string product, string count)
        {
            
            bool access = true;
            string res = "";

            if (Data.CheckExistFile(product, "Products"))
            {
                if (Data.CheckInt(count))
                {
                    if (Data.CheckExistFile(login, "Charts"))
                    {
                        
                        string[][] data = Data.GetChart(login);
                        
                        for (int i = 0; i < data.Length - 1; i++)
                        {
                            
                            if (product == data[i][0])
                            {
                                ChangeChart(login, product, count, true);
                                access = false;
                            }
                            res += "product[" + data[i][0] + "]";
                            res += "count[" + data[i][1] + "]";
                        }
                    }
                    else
                    {
                        string[][] data = new string[1][];
                    }

                    
                    if (access)
                    {
                        res += "product[" + product + "]";
                        res += "count[" + count + "]";
                        UpdateChart(login, res);
                    }
                }
                else
                {
                    Console.WriteLine("Количество должно быть целочисленным");
                }
            }
            else
            {
                Console.WriteLine("Указанного товара не существует");
            }
        }
        
        public static void ChangeChart(string login, string product, string count, bool plus = false)
        {
            
            if (Data.CheckExistFile(product, "Products"))
            {
                if (Data.CheckInt(count))
                {
                    if (Data.CheckExistFile(login, "Charts"))
                    {
                        string res = "";
                        string[][] data = Data.GetChart(login);
                        for (int i = 0; i < data.Length - 1; i++)
                        {
                            if (product == data[i][0])
                            {
                                
                                if (plus)
                                {
                                    res += "product[" + data[i][0] + "]";
                                    res += "count[" + (Convert.ToInt32(data[i][1]) + Convert.ToInt32(count)).ToString() + "]";
                                }
                                else 
                                {
                                    res += "product[" + data[i][0] + "]";
                                    res += "count[" + count + "]";
                                }
                            }
                            else 
                            {
                                res += "product[" + data[i][0] + "]";
                                res += "count[" + data[i][1] + "]";
                            }
                        }
                        UpdateChart(login, res); 
                    }
                }
                else
                {
                    Console.WriteLine("Количество должно быть целочисленным");
                }
            }
            else
            {
                Console.WriteLine("Указанного товара не существует");
            }
        }
        
        public static void DeleteChart(string login, string product)
        {
            
            if (Data.CheckExistFile(product, "Products"))
            {
                if (Data.CheckExistFile(login, "Charts"))
                {
                    string res = "";
                    string[][] data = Data.GetChart(login);
                    for (int i = 0; i < data.Length - 1; i++)
                    {
                        if (product != data[i][0]) 
                        {
                            res += "product[" + data[i][0] + "]";
                            res += "count[" + data[i][1] + "]";
                        }
                    }
                    if (res == "") 
                    {
                        File.Delete("Data\\Charts\\" + login + ".dat");
                        Console.WriteLine("Корзина удалена");
                    }
                    else
                    {
                        UpdateChart(login, res); 
                    }
                }
            }
            else
            {
                Console.WriteLine("Указанного товара не существует");
            }
        }
    }
}
