using System;
using System.Collections.Generic;
using System.Text;
using DotIniWorking.Models;

namespace DotIniWorking.Parser.Implemetations
{
    class IniParser
    {
        public string File_path { get; set; }
        public List<Block> Blocks { get; set; } //List<Block> blocks = new List<Block>(); //коллекция блоков, содержащихся в файле
        public IniParser(string file_path)
        {
            File_path = file_path;
            Blocks = new List<Block>();

        }

        public string[] Show() 
        {
            try
            {
                List<string> str = new List<string>();
                string File_line;
                System.IO.StreamReader file = new System.IO.StreamReader(File_path);
                while ((File_line = file.ReadLine()) != null)
                {
                    str.Add(File_line);
                }
                file.Close();
                return str.ToArray();
            }
            catch
            {
                throw new Exception("Файл не найден, возможно он перемещён или удалён");
            }
        }
        public void Read() //Path - путь к файлу
        {
            
            int block_id = -1, property_id = -1;
            int a, b;// 2 переменные используемые при поиске названий блоков
            int pos;// позиция запятой для удаления комментария, и для поиска = в строке для получения свойств // Писать комментарий в отдельную переменную?


            string File_line;
            try
            {
                System.IO.StreamReader file = new System.IO.StreamReader(File_path);
                while ((File_line = file.ReadLine()) != null) //цикл чтения файла (построчно) //Не знал что в цикле можно ТАК присваивать значения
                {
                    pos = File_line.IndexOf(';');
                    if (pos != -1) // очистка комментария
                    {
                        File_line = File_line.Substring(0, pos);
                    }
                    //удаление пробелов, если такие сушествуют
                    //Работает немного не верно
                    //while ((pos = File_line.IndexOf(" ")) != -1)
                    //{
                    //    File_line = File_line.Remove(pos, 1);
                    //    //Console.WriteLine(File_line);
                    //    //Console.WriteLine(pos);
                    //}

                    a = File_line.IndexOf('[');
                    b = File_line.IndexOf(']');
                    if ((a != -1) & (b != -1) & (a < b))
                    {
                        block_id += 1;
                        Blocks.Add(new Block(File_line.Substring(a + 1, b - 1))); //new block = block(File_line.Substring(a, b), new List<property>())
                                                                                  //Console.WriteLine("There is a block");
                        property_id = -1;
                    }
                    pos = File_line.IndexOf('='); //повторное использоваение pos возможно надо пофиксить но позже
                    if (pos != -1)
                    {

                        property_id += 1;
                        Blocks[block_id].Propertyes.Add(new Property()); //new block = block(File_line.Substring(a, b), new List<property>())
                        Blocks[block_id].Propertyes[property_id].Title = File_line.Substring(0, pos - 1).Trim();
                        Blocks[block_id].Propertyes[property_id].Value = File_line.Substring(pos + 1, (File_line.Length - pos - 1)).Trim();
                    }
                    //Console.WriteLine(File_line);
                }

                file.Close();
            }
            catch
            {
                throw new Exception("Файл не найден");
            }


        }
        public void Set(int i, int j, string value_to_set)
        {
            Blocks[i].Propertyes[j].Value = value_to_set;
        }
        public string Get_as_string(int block_id, int property_id)
        {
            return Blocks[block_id].Propertyes[property_id].Value;
        }
        public int Get_as_int(int block_id, int property_id)
        {
            try
            {
                double intermediate = Convert.ToDouble(Blocks[block_id].Propertyes[property_id].Value.Replace(".", ","));//Да, костили но как иначе не понятно
                int result = Convert.ToInt32(intermediate);
                return result;
            }
            catch
            {
                throw new Exception("Не получается конвертировать данную строку в тип int");
            }
        }
        public double Get_as_double(int block_id, int property_id)
        {
            try
            {
                double result = Convert.ToDouble(Blocks[block_id].Propertyes[property_id].Value.Replace(".", ","));
                return result;
            }
            catch
            {
                throw new Exception("Не получается конвертировать данную строку в тип double");
            }
        }
        public void Rename(string to_what, int where_block, int where_property= -1)
        {
            try
            {
                if (where_property == -1) //если параметр не указан
                {
                    Blocks[where_block].Name = to_what;
                }
                else
                {
                    Blocks[where_block].Propertyes[where_property].Title = to_what;
                }
            }
            catch
            {
                throw new Exception("Переменная с таким индексом не найдена");
            }

        }
        public string[] ShowCurrent()
        {
            List<string> current_file = new List<string>();
            foreach(Block bl in Blocks)
            {
                current_file.Add($"[{bl.Name}]");
                foreach (Property pr in bl.Propertyes)
                {
                    current_file.Add($"{pr.Title} = {pr.Value}");
                }
            }
            return current_file.ToArray();
        }
        public void Save(string where)
        {
            try
            {
                System.IO.StreamWriter sw = new System.IO.StreamWriter(where, false);
                foreach (Block bl in Blocks)
                {
                    sw.WriteLine($"[{bl.Name}]");
                    foreach (Property pr in bl.Propertyes)
                    {
                        sw.WriteLine($"{pr.Title} = {pr.Value}");
                    }
                }
                sw.Close();
            }
            catch
            {
                throw new Exception("Ошибка записи, возможно файл занят другим процессом");
            }

        }
        public void Add(int what,string name, string value = "",int where = -1)//what - можно было использовать bool
        {
            int a;
            if (what == 1)//блок
            {
                Blocks.Add(new Block(name));
            }
            if (what == 2)//параметр
            {
                Blocks[where].Propertyes.Add(new Property());
                a = Blocks[where].Propertyes.Count-1;
                Blocks[where].Propertyes[a].Title=name;
                Blocks[where].Propertyes[a].Value = value;
            }

        }
        public void Delete(int what, int block_number, int property_number = -1)//what - можно было использовать bool
        {
            try
            {
                
                if (what == 1)//блок
                {
                    Blocks.RemoveAt(block_number);
                }
                if (what == 2)//параметр
                {
                    Blocks[block_number].Propertyes.RemoveAt(property_number);
                }
            }
            catch
            {
                throw new Exception("Ошибка, такая запись не найдена");
            }

        }
    }
}
