using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Services.Models
{
    public class QRCode
    {
        public int? id { get; set; }
        public int? meetingid { get; set; }
        public string doctor_code { get; set; }
        public int? type { get; set; }
        public int? expireSeconds { get; set; }
        public DateTime? expirationtime { get; set; }
        
        public string qr_image { get; set; }
        public DateTime? createtime { get; set; }
        public int? status { get; set; }
    }
}