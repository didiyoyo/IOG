using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    public class data_InvitedDocData
    {
        public string mid { get; set; }
        public string mtitle { get; set; }
        public string msite { get; set; }
        public DateTime? mbegintime { get; set; }
        public DateTime? mendtime { get; set; }
        /// <summary>
        /// 区域
        /// </summary>
        public string area { get; set; }
        /// <summary>
        /// DSM姓名
        /// </summary>
        public string dsm_name { get; set; }
        /// <summary>
        /// 代表code
        /// </summary>
        public string user_code { get; set; }
        /// <summary>
        /// 代表姓名
        /// </summary>
        public string user_name { get; set; }
        /// <summary>
        /// 负责医生数量
        /// </summary>
        public int? DocCount { get; set; }
        /// <summary>
        /// 邀约医生数量
        /// </summary>
        public int? InvitedDocCount { get; set; }
        /// <summary>
        /// 医生预约参会数量
        /// </summary>
        public int? JoinDocCount { get; set; } 
       
    }
}
