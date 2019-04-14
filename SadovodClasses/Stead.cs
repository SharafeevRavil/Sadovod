using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SadovodClasses
{
	public class Stead											//Класс "участок"
	{
		private List<GardenBed> gardenBeds;						//Поле грядок на участке

		public Stead()
		{
			gardenBeds = new List<GardenBed>();
		}

		public void AddBed(GardenBed gardenBedToToAdd)			//Добавить грядку на участок		
		{
			gardenBeds.Add(gardenBedToToAdd);
		}

		public void RemoveBed(int numberOfBed)					//Убрать n-ую грядку из участка
		{
			gardenBeds.RemoveAt(numberOfBed - 1);
		}
	}
}