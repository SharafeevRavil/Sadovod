using System;
using System.Collections.Generic;
using System.Text;

namespace SadovodClasses
{
    public class PlantType
    {
        //Название растения
        public string TypeName
        {
            get; set;
        }
        //Сорт растения
        public string SortName
        {
            get; set;
        }

        public PlantType(string typeName, string sortName)
        {
            TypeName = typeName;
            SortName = sortName;
        }
    }
}
