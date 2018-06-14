using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    public class click_log
    {
        [Key]
        public int id { get; set; }
        public int mid { get; set; }
        public string openid { get; set; }
        public string modelName { get; set; }
        public DateTime? createTime { get; set; }
    }
}
