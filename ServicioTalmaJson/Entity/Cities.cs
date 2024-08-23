using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioTalmaJson.Entity
{
    public class Cities
    {
        public int id { get; set; }
        public string name { get; set; }
        public List<Weather> weather { get; set; } = new List<Weather>();
        public Main main { get; set; }
    }
}
