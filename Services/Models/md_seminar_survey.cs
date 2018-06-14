using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    public partial class md_seminar_survey
    {
        [Key]
        public int sid { get; set; }
        public int mid { get; set; }
        [Display(Name = "题目")] 
        public string stitle { get; set; }

        public string smaxtitle { get; set; }

        [Display(Name = "答案")]
        public string sanswer { get; set; }
        [Display(Name = "状态")]
        public int sstate { get; set; }
        [Display(Name = "创建时间")]
        public string sdatatiem { get; set; }
        [Display(Name = "类型")]
        public int stype { get; set; }
    }
}
