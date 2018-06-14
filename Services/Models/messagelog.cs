using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    public class messagelog
    {
        [Key]
        public int id { get; set; }
        public string key { get; set; }
        public int meetingid { get; set; }
                           
    }
}
