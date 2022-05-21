using System;
using System.Collections.Generic;
using System.Text;


namespace DotIniWorking.Models
{
    public class Block // Собственно сами блоки, состоящие из списков свойств и названия блока
    {
        public string Name { get; set; }
        //public string Comments     { get; set; }//Обыно не критично но для реализации комментариев +- необходимо
        public List<Property> Propertyes { get; set; }
        public Block(string name) //string lines
        {
            Name = name;//в теории можно изменить на title, но мне кажется они должны различаться
            //Lines = lines;
            Propertyes = new List<Property>();
        }
    }
}
