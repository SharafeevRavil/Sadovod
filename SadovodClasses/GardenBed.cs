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

            WaterDate = DateTime.Now;
            WeedDate = DateTime.Now;
            PileUpDate = DateTime.Now;
            FertilizeDate = DateTime.Now;

            WaterPeriod = 1;
            WeedPeriod = 14;
            PileUpPeriod = 14;
            FertilizePeriod = 14;
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

        //Полив
        public DateTime WaterDate
        {
            get; set;
        }
        public int WaterPeriod
        {
            get; set;
        }
        //Прополка
        public DateTime WeedDate
        {
            get; set;
        }
        public int WeedPeriod
        {
            get; set;
        }
        //Окучивание
        public DateTime PileUpDate
        {
            get; set;
        }
        public int PileUpPeriod
        {
            get; set;
        }
        //Удобрение
        public DateTime FertilizeDate
        {
            get; set;
        }
        public int FertilizePeriod
        {
            get; set;
        }
    }
}