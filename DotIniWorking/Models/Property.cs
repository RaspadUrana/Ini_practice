using System;
using System.Collections.Generic;
using System.Text;

namespace DotIniWorking.Models
{
    public class Property //В каждом блоке есть некоторые свойства, так вот это они
    {//а не проще ли было сделоть param(s)?
        public string Title { get; set; }
        public string Value { get; set; }
        //public string Value_type { get; set; } 
        //public Property(string Title,string Value) 
    }
}
