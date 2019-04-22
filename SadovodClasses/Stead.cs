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

        public string Name
        {
            get; set;
        }

        public Stead(string name)
        {
            Name = name;
            gardenBeds = new List<GardenBed>();
        }

        //Добавить грядку на участок
        public void AddBed(GardenBed gardenBedToToAdd)
        {
            gardenBeds.Add(gardenBedToToAdd);
            BedsChanged.Invoke(this, new EventArgs());
        }

        //Событие изменения коллекции грядок
        public event EventHandler BedsChanged;

        //Убрать n-ую грядку из участка
        public bool RemoveBed(int numberOfBed)
        {
            if (numberOfBed >= gardenBeds.Count || numberOfBed < 0)
            {
                return false;
            }
            gardenBeds.RemoveAt(numberOfBed - 1);
            return true;
            BedsChanged.Invoke(this, new EventArgs());
        }
        //Убрать грядку
        public bool RemoveBed(GardenBed bed)
        {
            return gardenBeds.Remove(bed);
            BedsChanged.Invoke(this, new EventArgs());
        }
    }
}