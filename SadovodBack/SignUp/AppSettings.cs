using System;

namespace SadovodBack
{
    public class AppSettings
    {
        public string Secret { get; set; }

        public AppSettings()
        {
            Secret = DateTime.Now.ToString();
        }
    }
}