using System;
using System.Collections.Generic;

namespace SadovodClasses
{
	public class GardenBed : GardenBedCare								//Класс "грядка"
	{
		public class PlantType											//Класс "растения"
		{
			private string TypeName;									//Название растения
			private string SortName;									//Сорт растения

			public PlantType(string TypeName, string SortName)
			{
				this.TypeName = TypeName;
				this.SortName = SortName;
			}

		}

		public enum GardenBedType										//Перечесление, для указания где расположена грядка		
		{
			Alfresco,													//Под открытым небом
			Hotbed,														//В парнике
			Greenhouse													//В теплице
		}
		private List<PlantType> plants;									//Список раcтений на грядке
		private string notes;											//Заметки о растениях
		protected GardenBedType BedType;

		public GardenBed(GardenBedType BedType = GardenBedType.Alfresco)
		{
			plants = new List<PlantType>();
			this.BedType = BedType;
		}

		public void AddPlant(string TypeName, string SortName)          //Добавление новых растений в грядку
		{																//Параметры - названия типа и сорта расстения
			var plantToAdd = new PlantType(TypeName, SortName);
			plants.Add(plantToAdd);
		}

		public void RemovePlant(int numberOfPlant)						//Убрать n-ое растение из грядки
		{
			plants.RemoveAt(numberOfPlant - 1);
		}

		public void AddNote(string notesToMake)						   //Сделать заметку о грядке
		{
			notes += notesToMake;
		}

		public void DeleteNote()                            //Очистить заметки о грядке
		{
			notes = null;
		}
	}
}