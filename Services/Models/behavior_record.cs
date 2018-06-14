using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    public class behavior_record
    {
        [Key]
        public int id { get; set; }
        public string url { get; set; }
        public DateTime? sdate { get; set; }
        public DateTime? edate { get; set; }
        public string ip { get; set; }
        public string agent { get; set; }
        public string openid { get; set; }
        public string operate { get; set; }
        public string mark { get; set; }
        public string field_id { get; set; }
        public string field_value { get; set; }
        public DateTime? createTime { get; set; }

        [NotMapped]
        public string doc_code { get; set; }
        [NotMapped]
        public string doc_name { get; set; }

                           
    }
}
