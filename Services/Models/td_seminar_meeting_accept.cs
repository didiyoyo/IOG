using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    public partial class td_seminar_meeting_accept
    {
        public int Id { get; set; }
        public string OPenID { get; set; }
        public int MId { get; set; }
        public string Email { get; set; }
        public int State { get; set; }
        public DateTime? CreateOn { get; set; }
    }
}
