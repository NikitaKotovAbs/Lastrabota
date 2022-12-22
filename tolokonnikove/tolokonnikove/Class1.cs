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
    class Data
    {
        
        public static string[] Explode(string separator, string source)
        {
            
            return source.Split(new string[] { separator }, StringSplitOptions.None);
        }
        
        public static bool CheckExistFile(string file, string dir)
        {
            try
            {
                
                using (StreamReader sr = new StreamReader(@"Data\" + dir + "\\" + file + ".dat"))
                {
                    sr.Close();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        
        public static string[] ReadFileByString(string file, string dir)
        {
            string dat = "";

            
            using (StreamReader sr = new StreamReader(@"Data\" + dir + "\\" + file + ".dat"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    dat = dat + line;
                }
                sr.Close();
            }
            return Data.Explode(";", dat);
        }
        
        public static string[,] GetUser(string login)
        {
            string[] user = ReadFileByString(login, "Users"); 
            string[] userData = Data.Explode("]", user[0]);  
            string[,] data = new string[userData.Length, 2];
            for (int i = 0; i < userData.Length - 1; i++)
            {
                string[] exp = Explode("[", userData[i]); 
                data[i, 0] = exp[0];
                data[i, 1] = exp[1];
            }
            return data;
        }
        
        public static void GetHelp(string status)
        {
            if (status == "buyer")
            {
                Console.WriteLine("\r\nСписок команд для покупателя\r\n" + new string('_', 50));

                var table = new ConsoleTable("Команда", "Параметры", "Описание"); 

                
                table.AddRow(new string[] { "get_products", "", "Вывести список товаров" });
                table.AddRow(new string[] { "check_chart", "", "Просмотр корзины" });
                table.AddRow(new string[] { "add_chart", "product count", "Добавление товара в корзину" });
                table.AddRow(new string[] { "change_chart", "product count", "Изменение кол-ва товара в корзине" });
                table.AddRow(new string[] { "delete_chart", "product", "Удаление товара из корзины" });

                
                table.Write();

                
            }
            else if (status == "admin")
            {
                Console.WriteLine("\r\nСписок команд для администратора\r\n" + new string('_', 50));

                var table = new ConsoleTable("Команда", "Параметры", "Описание");

                table.AddRow(new string[] { "delete_user", "login", "Удалить пользовтеля" });
                table.AddRow(new string[] { "change_user", "login", "Изменить пользователя" });
                table.AddRow(new string[] { "add_user", "", "Добавить пользователя" });
                table.AddRow(new string[] { "add_product", "", "Добавить товар" });
                table.AddRow(new string[] { "change_product", "product", "Изменить товар" });
                table.AddRow(new string[] { "delete_product", "product", "Удалить товар" });
                table.AddRow(new string[] { "add_chart", "product count login", "Добавление товара в корзину пользователя" });
                table.AddRow(new string[] { "change_chart", "product count login", "Изменение кол-ва товара в корзине пользователя" });
                table.AddRow(new string[] { "delete_chart", "product login", "Удаление товара из корзины пользователя" });

                table.Write();
            }
            else if (status == "hr")
            {
                Console.WriteLine("\r\nСписок команд для HR\r\n" + new string('_', 50));

                var table = new ConsoleTable("Команда", "Параметры", "Описание");

                table.AddRow(new string[] { "get_users", "", "Вывести список пользователей" });
                table.AddRow(new string[] { "add_worker", "login", "Нанять пользователя" });
                table.AddRow(new string[] { "delete_worker", "login", "Уволить пользователя" });
                table.AddRow(new string[] { "change_worker", "login", "Изменить пользователя" });

                table.Write();
            }
            else if (status == "warehouse")
            {
                Console.WriteLine("\r\nСписок команд для кладовщика\r\n" + new string('_', 50));

                var table = new ConsoleTable("Команда", "Параметры", "Описание");

                table.AddRow(new string[] { "get_products", "", "Вывести список товаров" });
                table.AddRow(new string[] { "update_product_count", "product", "Изменить количество товара" });
                table.AddRow(new string[] { "unready_product", "product", "Изменить срок годности товара" });
                table.AddRow(new string[] { "transfer_product", "product", "Изменить склад товара" });

                table.Write();
            }
            else if (status == "cassa")
            {
                Console.WriteLine("\r\nСписок команд для кассира\r\n" + new string('_', 50));

                var table = new ConsoleTable("Команда", "Параметры", "Описание");

                table.AddRow(new string[] { "get_chart_list", "", "Просмотр списка корзин" });
                table.AddRow(new string[] { "check_chart", "login", "Просмотр корзины" });
                table.AddRow(new string[] { "complete_order", "login", "Оформление заказа" });

                table.Write();
            }
            else if (status == "finance")
            {
                Console.WriteLine("\r\nСписок команд для бухгалтера\r\n" + new string('_', 50));

                var table = new ConsoleTable("Команда", "Параметры", "Описание");

                table.AddRow(new string[] { "get_order_list", "", "Просмотр списка заказов" });
                table.AddRow(new string[] { "get_order", "period", "Просмотр информации по заказам за период (period - day, month, quoter, year, all)" });
                table.AddRow(new string[] { "check_order", "order", "Просмотр информации по заказу" });
                table.AddRow(new string[] { "send_payment", "", "Выдача зарплат за месяц" });
                table.AddRow(new string[] { "get_payment", "period", "Просмотр информации по зарплатам за период (period - month, quoter, year, all)" });
                table.AddRow(new string[] { "get_budget", "period", "Просмотр информации по бюджету за период (period - month, quoter, year, all)" });

                table.Write();
            }
        }
        
        public static bool CheckString(string str)
        {
            
            if (str != "")
            {
                int err = 0;
                char[] bad = new char[] { ';', '[', ']' };
                for (int i = 0; i < str.Length; i++)
                {
                    if (bad.Contains(str[i]))
                    {
                        err++;
                    }
                }
                if (err > 0)
                {
                    string ch = "";
                    for (int i = 0; i < bad.Length; i++)
                    {
                        ch += " " + bad[i];
                    }
                    Console.WriteLine("Найдены запрещенные символы, исключите следующие символы: " + ch);
                    return false;
                }
                return true;
            }
            else
            {
                Console.WriteLine("Пустая строка недопустима");
                return false;
            }
        }
       
        public static bool CheckPass(string str)
        {
            if (str.Length >= 8)
            {
                char[] ch = str.ToCharArray();
                var count = ch.Where((n) => n >= '0' && n <= '9').Count(); 
                if (count >= 3)
                {
                    Regex regex = new Regex("([A-Z])|([А-Я])"); 
                    if (regex.Matches(str).Count >= 3)
                    {
                        int err = 0;
                        string c = str;
                        for (int i = 0; i < ch.Length; i++)
                        {
                            if ((c[i] >= 'a' && c[i] <= 'z') || (c[i] >= 'A' && c[i] <= 'Z'))
                            { }
                            else if (c[i] >= '0' && c[i] <= '9')
                            { }
                            else if (c[i] != ' ' && c[i] != '\n')
                            {
                                err++;
                            }
                        }
                        if (err >= 2)
                        {
                            return true;
                        }
                        else
                        {
                            Console.WriteLine("Пароль должен содержать минмум 2 спец символа");
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Пароль должен содержать минмум 3 заглавных символа");
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Пароль должен содержать минмум 3 цифры");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Минимальная длинна пароля 8 символов");
                return false;
            }
        }
        
        public static bool CheckMail(string str)
        {
            bool finded = false;

            
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == '@')
                {
                    finded = true;
                }
            }

            if (finded)
            {
                return true;
            }
            else
            {
                Console.WriteLine("Электронная почта должна содержать символ @");
                return false;
            }
        }
        
        public static bool CheckStatus(string str)
        {
            string[] status = new string[] { "buyer", "admin", "hr", "warehouse", "cassa", "finance" };
            string n_status = "";
            bool finded = false;

            
            for (int i = 0; i < status.Length; i++)
            {
                n_status += " " + status[i];
            }

            
            for (int i = 0; i < status.Length; i++)
            {
                if (status[i] == str)
                {
                    finded = true;
                }
            }

            if (finded)
            {
                return true;
            }
            else
            {
                Console.WriteLine("Введен не поддерживаемый статус");
                Console.WriteLine("Введите один из доступных статусов: " + n_status);
                return false;
            }
        }
        
        public static string[,] GetProduct(string product)
        {
            
            string[] prod = ReadFileByString(product, "Products");
            string[] prodData = Data.Explode("]", prod[0]);
            string[,] data = new string[prodData.Length, 2];
            for (int i = 0; i < prodData.Length - 1; i++)
            {
                string[] exp = Explode("[", prodData[i]);
                data[i, 0] = exp[0];
                data[i, 1] = exp[1];
            }
            return data;
        }
        
        public static string[][] GetChart(string login)
        {
            
            string[] chartList = ReadFileByString(login, "Charts");
            string[][] data = new string[chartList.Length][];
            for (int i = 0; i < chartList.Length - 1; i++)
            {
                
                string[] chartData = Data.Explode("]", chartList[i]);
                string[] chartRes = new string[chartData.Length];
                for (int j = 0; j < chartData.Length - 1; j++)
                {
                    string[] exp = Explode("[", chartData[j]);
                    chartRes[j] = exp[1];
                }
                data[i] = chartRes;
            }
            return data;
        }
        
        public static bool CheckInt(string integer)
        {
            try
            {
                
                int num = Convert.ToInt32(integer);
                return true;
            }
            catch
            {
                return false;
            }
        }
        
        public static bool SendMail(string text, string mailName)
        {
            
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com"); 
                mail.From = new MailAddress("zhilcove@gmail.com"); 
                mail.To.Add(mailName); 
                mail.Subject = "Order"; 

                
                mail.Body += " <html>";
                mail.Body += "<body>";
                mail.Body += text;
                mail.Body += "</body>";
                mail.Body += "</html>";

                mail.IsBodyHtml = true; 

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("zhilcove@gmail.com", "1806326!"); 
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail); 

                return true;
            }
            catch
            {
                return false;
            }
        }
        
        public static string[][] GetOrder(string login)
        {
            
            string[] chartList = ReadFileByString(login, "Orders");
            string[][] data = new string[chartList.Length][];
            for (int i = 0; i < chartList.Length - 1; i++)
            {
                string[] chartData = Data.Explode("]", chartList[i]);
                string[] chartRes = new string[chartData.Length];
                for (int j = 0; j < chartData.Length - 1; j++)
                {
                    string[] exp = Explode("[", chartData[j]);
                    chartRes[j] = exp[1];
                }
                data[i] = chartRes;
            }
            return data;
        }
        
        public static List<string> GetFilesByPeriod(string period, string dir, bool shortname = false)
        {
            
            List<string> res = new List<string>();

            
            string[] files = Directory.GetFiles("Data\\" + dir + "\\");


            foreach (string filename in files)
            {
                string[] finded = Data.Explode("\\", filename);
                finded = Data.Explode(".", finded[finded.Length - 1]); 

                string[] Fulldate;

                
                if (shortname)
                {
                    Fulldate = new string[1] { finded[0] };
                }
                else 
                {
                    Fulldate = Data.Explode("_", finded[0]);
                }

                string[] date = Data.Explode("-", Fulldate[0]); 
                string Nowdate = DateTime.Now.ToString();
                string[] exp = Nowdate.Split(new string[] { " " }, StringSplitOptions.None); 
                string[] nowDate = Data.Explode(".", exp[0]); 

                
                if (period == "day") 
                {
                    if (date[0] == nowDate[0] && date[1] == nowDate[1] && date[2] == nowDate[2])
                    {
                        res.Add(finded[0]);
                    }
                }
                else if (period == "month")
                {
                    if (date[1] == nowDate[1] && date[2] == nowDate[2])
                    {
                        res.Add(finded[0]);
                    }
                }
                else if (period == "qouter")
                {
                    
                    int quoterMin = 0;
                    int quoterMax = 0;

                    if (Convert.ToInt32(nowDate[2]) >= 1)
                    {
                        quoterMax = 3;
                        quoterMin = 1;
                    }
                    else if (Convert.ToInt32(nowDate[2]) >= 4)
                    {
                        quoterMax = 6;
                        quoterMin = 4;
                    }
                    else if (Convert.ToInt32(nowDate[2]) >= 7)
                    {
                        quoterMax = 9;
                        quoterMin = 7;
                    }
                    else if (Convert.ToInt32(nowDate[2]) >= 10)
                    {
                        quoterMax = 12;
                        quoterMin = 10;
                    }

                    if ((Convert.ToInt32(date[1]) >= quoterMin || Convert.ToInt32(date[1]) <= quoterMax) && date[2] == nowDate[2])
                    {
                        res.Add(finded[0]);
                    }
                }
                else if (period == "year")
                {
                    if (date[2] == nowDate[2])
                    {
                        res.Add(finded[0]);
                    }
                }
                else if (period == "all")
                {
                    res.Add(finded[0]);
                }
            }

            return res;
        }
       
        public static string[][] GetPayment(string login)
        {
            
            string[] chartList = ReadFileByString(login, "Payments");
            string[][] data = new string[chartList.Length][];
            for (int i = 0; i < chartList.Length - 1; i++)
            {
                string[] chartData = Data.Explode("]", chartList[i]);
                string[] chartRes = new string[chartData.Length];
                for (int j = 0; j < chartData.Length - 1; j++)
                {
                    string[] exp = Explode("[", chartData[j]);
                    chartRes[j] = exp[1];
                }
                data[i] = chartRes;
            }
            return data;
        }
    }
}
