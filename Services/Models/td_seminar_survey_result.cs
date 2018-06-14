using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    public partial class td_seminar_survey_result
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public int srid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string uid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int sid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string sresult { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? sdatatiem { get; set; }
    }
}
