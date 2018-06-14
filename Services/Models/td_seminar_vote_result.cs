using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    public partial class td_seminar_vote_result
    {
        [Key]

        /// <summary>
        /// 
        /// </summary>
        public int vrid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string uid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? vid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string vresult { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? vdatetime { get; set; }
    }
}
