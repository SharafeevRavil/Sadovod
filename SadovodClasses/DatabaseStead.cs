﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace SadovodClasses
{
    public class DatabaseStead
    {
        public int Id { get; set; }
        public Stead Stead { get; set; }
        public int GardenerID { get; set; }
    }
}
