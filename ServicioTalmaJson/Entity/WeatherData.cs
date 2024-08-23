using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioTalmaJson.Entity
{
    public class WeatherData
    {
        public int id { get; set; }
        public string name { get; set; }
        public Coord coord { get; set; }
        public Main main { get; set; }
        public int dt { get; set; }
        public Wind wind { get; set; }
        public Clouds clouds { get; set; }
        public List<Weather> weather { get; set; }
    }

    public class Coord
    {
        public double lon { get; set; }
        public double lat { get; set; }
    }

    public class Wind
    {
        public double speed { get; set; }
        public int deg { get; set; }
    }
    public class Clouds
    {
        public int all { get; set; }
    }
}
