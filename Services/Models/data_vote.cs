using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    public class data_vote
    {
        public string mid { get; set; }
        public string mtitle { get; set; }
        public string msite { get; set; }
        public DateTime? mbegintime { get; set; }
        public DateTime? mendtime { get; set; }
        public string doc_code { get; set; }
        public string doc_name { get; set; }
        public int vid { get; set; }
        public string title { get; set; }
        public string answer { get; set; }
        public string type { get; set; }
        public string result { get; set; }
        public DateTime? createTime { get; set; }
    }
}
