using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    public class data_DocData
    {
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
        /// 负责医生认证数量
        /// </summary>
        public int? AcceptedDocCount { get; set; }
    }
}
