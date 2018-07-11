using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    public class sfe_push
    {
        [Key]
        public int id { get; set; }

        /// <summary>
        /// openid
        /// </summary>
        public string openid { get; set; }

        /// <summary>
        /// 推送时间
        /// </summary>
        public DateTime? confiPushTime { get; set; }

        /// <summary>
        /// 会议id
        /// </summary>
        public int? mid { get; set; }

        /// <summary>
        /// 推送类型
        /// </summary>
        public string pushType { get; set; }

        /// <summary>
        /// 是否推送
        /// </summary>
        public bool? ispush { get; set; }

        /// <summary>
        /// networkid
        /// </summary>
        public string networkid { get; set; }
    }
}
