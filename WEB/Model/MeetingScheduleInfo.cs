using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEB.Model
{
    public class MeetingScheduleInfo
    {
        public DateTime MarkTime { get; set; }

        public List<agenda> AgendaList { get; set; }
    }
}