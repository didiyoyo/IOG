using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    public partial class md_seminar_meeting_detail
    {
        [Key]
        public int did { get; set; }
        public int mid { get; set; }
        public string dname { get; set; }
        public string dcontent { get; set; }
        public string dformat { get; set; }
        public DateTime? createTime { get; set; }
        
    }
}
