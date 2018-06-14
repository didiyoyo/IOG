using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    public class SendEmail
    {
        public int id { get; set; }
        public string openid { get; set; }
        public string ToEmaill { get; set; }
        public int mid { get; set; }
        public string name { get; set; }
        public string status { get; set; }
        public DateTime? createTime { get; set; }
    }
}
