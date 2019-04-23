using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace SadovodClasses
{
    public class Stead
    {
        //Поле грядок на участке
        private List<GardenBed> gardenBeds;
        public ReadOnlyCollection<GardenBed> GardenBeds
        {
            get => new ReadOnlyCollection<GardenBed>(gardenBeds);
        }

        public Stead()
        {
            gardenBeds = new List<GardenBed>();
        }

        //Добавить грядку на участок
        public void AddBed(GardenBed gardenBedToToAdd)
        {
            gardenBeds.Add(gardenBedToToAdd);
        }
        //Убрать n-ую грядку из участка
        public bool RemoveBed(int numberOfBed)
        {
            if (numberOfBed >= gardenBeds.Count || numberOfBed < 0)
            {
                return false;
            }
            gardenBeds.RemoveAt(numberOfBed - 1);
            return true;
        }
        //Убрать грядку
        public bool RemoveBed(GardenBed bed)
        {
            return gardenBeds.Remove(bed);
        }
    }
}