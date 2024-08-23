using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioTalmaJson.Entity
{
    public class ProcessedData
    {
        public Weather weather { get; set; }// = new WeatherResponse();
        public List<Cities> cities { get; set; } = new List<Cities>();
    }
}
