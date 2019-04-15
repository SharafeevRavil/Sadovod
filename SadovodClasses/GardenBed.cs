using System;
using System.Collections.Generic;

namespace SadovodClasses
{
    public class GardenBed
    {
        //Раcтение на грядке
        public PlantType Plant
        {
            get;
        }

        //Заметки о растениях
        public List<string> Notes
        {
            get;
        }

        //Тип грядки
        public GardenBedType BedType
        {
            get; set;
        }

        public GardenBed(PlantType plantType, GardenBedType bedType = GardenBedType.Alfresco)
        {
            Plant = plantType;
            BedType = bedType;
            Notes = new List<string>();
        }

        //Сделать заметку о грядке
        public void AddNote(string notesToMake)
        {
            Notes.Add(notesToMake);
        }
        //Удалить заметку о грядке
        public bool DeleteNote(int index)
        {
            if (index >= Notes.Count || index < 0)
            {
                return false;
            }
            Notes.RemoveAt(index);
            return true;
        }
        public bool DeleteNote(string note)
        {
            return Notes.Remove(note);
        }
    }
}