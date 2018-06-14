using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    public class table_input
    {
        public int id { get; set; }

        public string mcode { get; set; }

        public string networkid { get; set; }

        public string WXPlatForm { get; set; }

        public string doc_name { get; set; }
        public string ins_name { get; set; }

        public string Pro_title { get; set; }
        public string department { get; set; }

        public string user_name { get; set; }
        public string statusCode { get; set; }

        public string IsInvitation { get; set; }
        public string IsAppointments { get; set; }

        public string IsSign { get; set; }
        public string Invitation_code { get; set; }

        public string browse { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? IsSelected { get; set; }

        public string ModelType { get; set; }

        /// <summary>
        /// 参会时间
        /// </summary>
        public DateTime? meetingtime { get; set; }

        /// <summary>
        /// 参会方式
        /// </summary>
        public string participation { get; set; }
    }
}
