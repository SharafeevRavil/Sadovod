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
            get;
        }
        //Сорт растения
        public string SortName
        {
            get;
        }

        public PlantType(string typeName, string sortName)
        {
            TypeName = typeName;
            SortName = sortName;
        }
    }
}
