using System;
using System.Collections.Generic;
using System.Text;

namespace DotIniWorking.Parser.Interfaces
{
    interface IIniParser
    {
        public string[] Show();
        public void Read();
        public void Set();
        public string Get_as_string(int i,int j);
        public int Get_as_int(int i, int j);
        public double Get_as_double(int i, int j);
        public void Rename(string to_what, int where_block, int where_property = -1);
        public string[] Showcurrent();
        public void Save(string where);
        public void Add(int what, string name, string value = "", int where = -1);
        public void Delete(int what, int block_number, int property_number);//what - можно было использовать bool
    }
}
