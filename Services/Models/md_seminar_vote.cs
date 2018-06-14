using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    public partial class md_seminar_vote
    {
        [Key]
        public int vid { get; set; } 
        public int mid { get; set; }
        [Display(Name = "题目")]
        public string vtopic { get; set; }
        [Display(Name = "答案")]
        public string vanswer { get; set; }
        [Display(Name = "状态")]
        public int vstate { get; set; }
        [Display(Name = "创建时间")]
        public string vdatetime { get; set; }
        [Display(Name = "类型")]
        public int vtype { get; set; }
    }
}
