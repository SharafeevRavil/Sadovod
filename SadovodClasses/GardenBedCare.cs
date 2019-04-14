using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace GardenLibrary
{
	public class GardenBedCare								//Класс "действия по уходу за грядкой"
	{
		//Была ли грядка полита, прополота, окучена, удобрена
		private bool isPoured, isWeeded, isPiledUp, isFertilized;
		//Время последнего полива, прополки. окучивания, удобрения
		private DateTime dateWhenPoured, dateWhenWeeded, dateWhenPiledUp, dateWhenFertilized;

		public void ToPour()                                //Полить грядку, зафиксировать дату и время полива
		{
			isPoured = true;
			dateWhenPoured = DateTime.Now;
		}

		public void ToWeed()                               //Прополоть грядку, зафиксировать дату и время прополки
		{
			isWeeded = true;
			dateWhenWeeded = DateTime.Now;
		}

		public void ToPileUp()                            //Окучить грядку, зафиксировать дату и время окучивания
		{
			isPiledUp = true;
			dateWhenPiledUp = DateTime.Now;
		}

		public void ToFertilize()                        //Удобрить грядку, зафиксировать дату и время удобрения
		{
			isFertilized = true;
			dateWhenFertilized = DateTime.Now;
		}
	}
}