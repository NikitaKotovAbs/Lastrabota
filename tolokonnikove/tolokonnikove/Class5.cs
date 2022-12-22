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
    class Warehouse
    {
       
        public static void GetProducts()
        {
            string[] files = Directory.GetFiles(@"Data\Products\");
            int n = 0;
            var table = new ConsoleTable("Магазин", "Склад", "Категория", "Товар", "Кол-во", "Годен до", "Цена");
            List<string> categ = new List<string>();
            List<string[][]> dat = new List<string[][]>();

           
            foreach (string filename in files)
            {
                n++;
                string[] finded = Data.Explode("\\", filename);
                finded = Data.Explode(".", finded[finded.Length - 1]);
                string[,] prod = Data.GetProduct(finded[0]);
                string[] prodDat = new string[7];
                string cat = "";
                for (int i = 0; i < 7; i++)
                {
                    prodDat[i] = prod[i, 1];

                    if (prod[i, 0] == "category")
                    {
                        cat = prodDat[i];
                        if (!categ.Contains(cat)) 
                        {
                            categ.Add(cat); 
                        }
                    }
                }
                string[][] res = new string[][] { new string[] { n.ToString() }, prodDat };
                dat.Add(res);

            }
            
            for (int i = 0; i < categ.Count; i++)
            {
                
                for (int j = 0; j < dat.Count; j++)
                {
                    string[] str = dat[j][1];
                    if (str[2] == categ[i]) 
                    {
                        if (DateTime.Now > DateTime.Parse(str[5])) 
                        {
                            str[6] = (Convert.ToDouble(str[6]) * 0.5).ToString() + " (-50%)";
                        }
                        table.AddRow(str);
                    }
                }
            }
            table.Write();
        }
        
        public static void UpdateProductByParam(string type, string product)
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
                    
                    if (prod[i, 0] == type)
                    {
                        
                        bool access = true;
                        while (access)
                        {
                            Console.Write("Введите новый " + prod[i, 0] + ": ");
                            dat[i] = Console.ReadLine();
                            if (Data.CheckString(dat[i]))
                            {
                                if (prod[i, 0] == "count")
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
                    else
                    {
                        dat[i] = prod[i, 1];
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
            else
            {
                Console.WriteLine("Товар не найден");
            }
        }
    }
}
