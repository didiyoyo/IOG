﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    public class data_send_email
    {
        public string mid { get; set; }
        public string mtitle { get; set; }
        public string msite { get; set; }
        public DateTime? mbegintime { get; set; }
        public DateTime? mendtime { get; set; }
        public string doc_code { get; set; }
        public string doc_name { get; set; }
        public string email { get; set; }
        public string to_email { get; set; }
        public string name { get; set; }
        public string status { get; set; }
        public DateTime? createTime { get; set; }
    }
}
