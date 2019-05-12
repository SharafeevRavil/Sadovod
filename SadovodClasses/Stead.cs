using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

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
        [JsonConstructor]
        public Stead(List<GardenBed> gardenBeds, string name)
        {
            Name = name;
            this.gardenBeds = gardenBeds;
        }

        //Добавить грядку на участок
        public void AddBed(GardenBed gardenBedToToAdd)
        {
            gardenBeds.Add(gardenBedToToAdd);
            InvokeBedsChanged();
        }

        //Событие изменения коллекции грядок
        public event EventHandler BedsChanged;
        //Инвокер события
        public void InvokeBedsChanged()
        {
            BedsChanged?.Invoke(this, new EventArgs());
        }

        //Убрать n-ую грядку из участка
        public bool RemoveBed(int numberOfBed)
        {
            if (numberOfBed >= gardenBeds.Count || numberOfBed < 0)
            {
                return false;
            }
            gardenBeds.RemoveAt(numberOfBed);
            InvokeBedsChanged();
            return true;
        }
        //Убрать грядку
        public bool RemoveBed(GardenBed bed)
        {
            var answer = gardenBeds.Remove(bed);
            InvokeBedsChanged();
            return answer;
        }
    }
}