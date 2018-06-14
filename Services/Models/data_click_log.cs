using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    public class data_click_log
    {
        [Key]
        public int id { get; set; }
        public int mid { get; set; }
        public string openid { get; set; }
        public int did { get; set; }
        public DateTime? createTime { get; set; }
    }
}
