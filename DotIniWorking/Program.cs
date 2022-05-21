using System;
using System.Collections.Generic;
using DotIniWorking.Models;
using DotIniWorking.Parser.Implemetations;

namespace DotIniWorking
{
    //Как сказал Влад можно через регулярку всё сделать, но я не умею...

    /*Пару слов о комментариях
     * В ini файле могут быть комментариии, но программой обрабатываться они не будут,
     * ибо я предпологаю что комментарии будут писаться в файл от руки.
     * В случае сохранения/замены файла комментарии теряются.
     * Могу обработать так, что-бы не терялись но сейчас не считаю нужным.
    */
    
    class Program
    {
        static void Main(string[] args)
        {
            int a, b;//переменные для передачи используемые в диалоге с пользователем для нахожденя блока/параметра
            int pos; //
            string result, path, ending; //path Путь к файлу , 
            //result - результат выполнения в первой части или результат "общения" пользователя с программой во второй
            //ending - расширение файла
            IniParser pars ; //Объект, с помощью которого осуществляется обработка файла
            int Val_type;// тип переменной указываемой в get
            int i ; //нумератор используемый в некоторых процедурах
            string val;//значение задаваемое параметру (set/add)
            string where;//Куда будет осуществлятся сохранение
            string what;//что переименовываем
            string name;
            

            //string check;
            //check = "120000000,0";
            //Console.WriteLine(Convert.ToDecimal(check));

            do
            {
                Console.WriteLine("Введите путь к целевому файлу");

                path = System.Console.ReadLine();//INI_File.ini
                pos = path.LastIndexOf(".")+1;
                //Console.WriteLine(pos);
                ending = path[pos..];
                //подготовка данных к проверке

                if (System.IO.File.Exists(path))
                {
                    if (ending == "ini")
                    {
                        result = "Success";
                    }
                    else
                    {
                        Console.WriteLine("Файл имеет неверный формат");
                        Console.WriteLine("Попробуйте снова");
                        result = "Fail";
                    }
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Файл не найден");
                    Console.WriteLine("Попробуйте снова");
                    Console.WriteLine();
                    result = "Fail";
                }
            } while (result != "Success");
            pars = new IniParser(path); //pars - новый объект класса IniParser
            pars.Read();
            Console.Clear();
            do
            {
                Console.WriteLine("Введите команду (введите help для получения справки по функциям)");
                /* 
                * help - справка по методам
                * show - выводит содержимое файла
                * get - будет выводить заданный параметр
                * set - будет устанавливать значение заданному параметру 
                * exit - осуществляет выход из программы.
                */
                result = Console.ReadLine();
                Console.WriteLine();
                switch (result)
                {
                    case "help":
                        Console.WriteLine("Команды (регистр учитывается):");
                        Console.WriteLine(" * help - справка по методам;");//good
                        
                        Console.WriteLine(" * show - выводит содержимое файла;");//good
                        Console.WriteLine(" * showcurrent - выводит текущие изменения в файле;");//good

                        Console.WriteLine(" * add - добавляет блок/параметр в блок;");//good
                        Console.WriteLine(" * get - выводит заданный параметр;");//good
                        Console.WriteLine(" * set - устанавливает значение заданному параметру; ");//good
                        Console.WriteLine(" * delete - удаляет блок/параметр ;");

                        Console.WriteLine(" * rename - данной коммандой можно переименовать ;");//good
                        
                        Console.WriteLine(" * save - сохраняет изменения в файл; ");//good
                        
                        Console.WriteLine(" * clear - очищает консоль; ");//good
                        Console.WriteLine(" * exit - осуществляет выход из программы. ");//good
                        
                        Console.WriteLine("");
                        
                        break;
                    case "show":
                        foreach (string stri in pars.Show())
                        {
                            Console.WriteLine(stri);//good
                        }
                        Console.WriteLine();
                        break;
                    case "showcurrent":
                        foreach (string stri in pars.ShowCurrent())
                        {
                            Console.WriteLine(stri);//good
                        }
                        Console.WriteLine();
                        break;
                    case "get":
                        Console.WriteLine("Введите номер блока из которого вы хотите получить значение:");
                        i = 1;
                        foreach (Block bl in pars.Blocks)
                        {
                            Console.Write($" {i++}.");
                            Console.WriteLine(bl.Name);
                        }
                        do
                        {
                            a = Convert.ToInt32(Console.ReadLine()) - 1;//-1 Т.к нумерация с 0, а программа выдаёт номера в удобном для чтения виде
                            if (a >= i)
                            {
                                Console.WriteLine($"Ввод неверен");//{i}
                            }
                        }
                        while (a >= i);

                        i = 1;
                        Console.WriteLine();
                        Console.WriteLine("Введите номер параметра значение которого вы хотите получить :");
                        foreach (Property prop in pars.Blocks[a].Propertyes)
                        {
                            Console.Write($" {i++}.");
                            Console.WriteLine(prop.Title);
                        }

                        do
                        {
                            b = Convert.ToInt32(Console.ReadLine()) - 1;//-1 Т.к нумерация с 0, а программа выдаёт номера в удобном для чтения виде
                            if (b >= i)
                            {
                                Console.WriteLine("Ввод неверен");
                            }
                        }
                        while (b >= i);

                        Console.WriteLine("В каком виде вы хотите получить значение");
                        Console.WriteLine(" 1.String");
                        Console.WriteLine(" 2.Int");
                        Console.WriteLine(" 3.Double");
                        do
                        {
                            Val_type = Convert.ToInt32(Console.ReadLine());
                            if (!((Val_type == 1) | (Val_type == 2)| (Val_type == 3)))
                            {
                                Console.WriteLine("Ввод неверен");
                            }
                        } while (!((Val_type == 1) | (Val_type == 2) | (Val_type == 3)));
                        try
                        {
                            switch (Val_type)
                            {
                                case 1:
                                    Console.WriteLine(pars.Get_as_string(a, b));
                                    break;
                                case 2:
                                    Console.WriteLine(pars.Get_as_int(a, b));
                                    break;
                                case 3:
                                    Console.WriteLine(pars.Get_as_double(a, b));
                                    break;
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                        break;
                    case "set":
                        i = 1;
                        Console.WriteLine("Введите номер блока в котором вы хотите изменить значение:");
                        foreach (Block bl in pars.Blocks)
                        {
                            Console.Write($" {i++}.");
                            Console.WriteLine(bl.Name);
                        }
                        do
                        {
                            a = Convert.ToInt32(Console.ReadLine()) - 1;//-1 Т.к нумерация с 0, а программа выдаёт номера в удобном для чтения виде
                            if (a >= i)
                            {
                                Console.WriteLine("Ввод неверен");
                            }
                        }
                        while (a >= i);

                        i = 1;
                        Console.WriteLine();
                        Console.WriteLine("Введите номер параметра значение которого вы хотите изменить :");
                        foreach (Property prop in pars.Blocks[a].Propertyes)
                        {
                            Console.Write($" {i++}.");
                            Console.WriteLine(prop.Title);
                        }

                        do
                        {
                            b = Convert.ToInt32(Console.ReadLine()) - 1;//-1 Т.к нумерация с 0, а программа выдаёт номера в удобном для чтения виде
                            if (b >= i)
                            {
                                Console.WriteLine("Ввод неверен");
                            }
                        }
                        while (b >= i);
                        Console.WriteLine($"Текущее значение равно: {pars.Blocks[a].Propertyes[b].Value}");
                        Console.WriteLine("Введите значение которое присваивается данной переменной");
                        val = Console.ReadLine();
                        pars.Set(a,b,val);
                        try
                        {
                            pars.Set(a, b, val);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                        break;
                    case "save":
                        Console.WriteLine("Введите путь к файлу в который запишуться данные");
                        Console.WriteLine("Нажмите Enter, для использования пути, указанного при запуске программы");
                        where = Console.ReadLine();

                        if (where == "")
                        {
                            where = path;
                        }
                        if (System.IO.File.Exists(where))
                        {
                            Console.WriteLine();
                            Console.WriteLine("Вы уверены?");
                            Console.WriteLine("Данный файл будет перезаписан.");
                            Console.WriteLine("Если ответ положительный введите + , иначе -.");
                            if (Console.ReadLine()== "-")
                            {
                                break;
                            }
                        }
                        pars.Save(where);
                        try
                        {
                            pars.Save(where);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                        //Console.WriteLine("проверка");
                        break;
                    case "exit":
                        result = "Exit"; //good
                        break;
                    case "clear":
                        Console.Clear(); //good
                        break;
                    default:
                        Console.WriteLine("Неизвестная команда");
                        break;
                    case "rename":
                        a = -1;
                        b = -1;
                        Console.WriteLine("Что вы хотите переименовать? (введите номер)");
                        Console.WriteLine(" 1.Блок");
                        Console.WriteLine(" 2.Параметр");
                        do
                        {
                            what = Console.ReadLine();
                        } while (!((what =="1")| (what == "2")));

                        i = 1;
                        Console.WriteLine("Введите номер блока в котором вы хотите изменить значение:");
                        foreach (Block bl in pars.Blocks)
                        {
                            Console.Write($" {i++}.");
                            Console.WriteLine(bl.Name);
                        }
                        do
                        {
                            a = Convert.ToInt32(Console.ReadLine()) - 1;//-1 Т.к нумерация с 0, а программа выдаёт номера в удобном для чтения виде
                            if (a >= i)
                            {
                                Console.WriteLine("Ввод неверен");
                            }
                        }
                        while (a >= i);
                        if (what == "2")
                        {
                            i = 1;
                            Console.WriteLine();
                            Console.WriteLine("Введите номер параметра значение которого вы хотите изменить :");
                            foreach (Property prop in pars.Blocks[a].Propertyes)
                            {
                                Console.Write($" {i++}.");
                                Console.WriteLine(prop.Title);
                            }

                            do
                            {
                                b = Convert.ToInt32(Console.ReadLine()) - 1;//-1 Т.к нумерация с 0, а программа выдаёт номера в удобном для чтения виде
                                if (b >= i)
                                {
                                    Console.WriteLine("Ввод неверен");
                                }
                            }
                            while (b >= i);
                        }

                        Console.WriteLine("Введите новое название");
                        val = Console.ReadLine();
                        try
                        {
                            if (b != -1)
                            {
                                pars.Rename(val, a, b);
                            }
                            else
                            {
                                pars.Rename(val, a);
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }

                        break;
                    case "add":
                        Console.WriteLine("Что вы хотите добавить? (введите номер)");
                        Console.WriteLine(" 1.Блок");
                        Console.WriteLine(" 2.Параметр");
                        do
                        {
                            what = Console.ReadLine();
                            if (!((what == "1") | (what == "2")))
                            {
                                Console.WriteLine("Ввод неверен");
                            }
                        } while (!((what == "1") | (what == "2")));
                        if (what == "1")
                        {
                            Console.WriteLine("Введите название блока");
                            name = Console.ReadLine();
                            pars.Add(Convert.ToInt32(what), name);
                        }
                        else if (what == "2")
                        {
                            i = 1;
                            Console.WriteLine("Введите номер блока в котором вы хотите добавить значение:");
                            foreach (Block bl in pars.Blocks)
                            {
                                Console.Write($" {i++}.");
                                Console.WriteLine(bl.Name);
                            }
                            do
                            {
                                a = Convert.ToInt32(Console.ReadLine()) - 1;//-1 Т.к нумерация с 0, а программа выдаёт номера в удобном для чтения виде
                                if (a >= i)
                                {
                                    Console.WriteLine("Ввод неверен");
                                }
                            } while (a >= i);
                            Console.WriteLine("Введите название параметра");
                            name = Console.ReadLine();
                            Console.WriteLine("Введите значение параметра");
                            val = Console.ReadLine();
                            try
                            {
                                pars.Add(Convert.ToInt32(what), name, val, a);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                            }
                            
                        }
                        break;
                    case "delete":
                        i = 1;
                        a = -1;
                        b = -1;
                        Console.WriteLine("Что вы хотите удалить? (введите номер)");
                        Console.WriteLine(" 1.Блок");
                        Console.WriteLine(" 2.Параметр");
                        do
                        {
                            what = Console.ReadLine();
                            if (!((what == "1") | (what == "2")))
                            {
                                Console.WriteLine("Ввод неверен");
                            }
                        } while (!((what == "1") | (what == "2")));
                        if (what == "1")
                        {
                            Console.WriteLine("Введите номер удаляемого блока");
                            foreach (Block bl in pars.Blocks)
                            {
                                Console.Write($" {i++}.");
                                Console.WriteLine(bl.Name);
                            }
                            do
                            {
                                a = Convert.ToInt32(Console.ReadLine()) - 1;//-1 Т.к нумерация с 0, а программа выдаёт номера в удобном для чтения виде
                                if (a >= i)
                                {
                                    Console.WriteLine("Ввод неверен");
                                }
                            }
                            while (a >= i);
                            try
                            {
                                pars.Delete(Convert.ToInt32(what), a);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                            }
                        }
                       

                        if (what == "2")
                        {
                            Console.WriteLine("Введите номер удаляемого блока");
                            foreach (Block bl in pars.Blocks)
                            {
                                Console.Write($" {i++}.");
                                Console.WriteLine(bl.Name);
                            }
                            do
                            {
                                a = Convert.ToInt32(Console.ReadLine()) - 1;//-1 Т.к нумерация с 0, а программа выдаёт номера в удобном для чтения виде
                                if (a >= i)
                                {
                                    Console.WriteLine("Ввод неверен");
                                }
                            }
                            while (a >= i);
                            Console.WriteLine();
                            Console.WriteLine("Введите номер удаляемого блока");
                            foreach (Property prop in pars.Blocks[a].Propertyes)
                            {
                                Console.Write($" {i++}.");
                                Console.WriteLine(prop.Title);
                            }
                            do
                            {
                                b = Convert.ToInt32(Console.ReadLine()) - 1;//-1 Т.к нумерация с 0, а программа выдаёт номера в удобном для чтения виде
                                if (b >= i)
                                {
                                    Console.WriteLine("Ввод неверен");
                                }
                            }
                            while (b >= i);
                            try
                            {
                                pars.Delete(Convert.ToInt32(what), a, b);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                            }
                            
                        }
                        
                        break;
                }

            } while (result != "Exit");
            
        }
    }
}
