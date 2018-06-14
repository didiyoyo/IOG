using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    public class agenda
    {
        public int id { get; set; }

        public DateTime? date { get; set; }

        public string time { get; set; }

        public string schedule { get; set; }

        public string name { get; set; }

        public int? mid { get; set; }
    }
}
