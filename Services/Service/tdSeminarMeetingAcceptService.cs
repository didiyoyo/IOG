using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service
{
    /// <summary>
    /// 会议和医生关联表
    /// </summary>
    public partial class tdSeminarMeetingAcceptService
    {
        public int Insert(td_seminar_meeting_accept sma)
        {
            using (DBContext db = new DBContext())
            {
                DateTime start = DateTime.Now;
                if(!db.md_seminar_meeting_main.Any(x=>x.mid==sma.MId&&x.mendtime>start))
                {
                    return -2;
                }
                if(db.td_seminar_meeting_accept.Any(x=>x.MId==sma.MId && x.OPenID==sma.OPenID)){
                    //已经确认参加过了
                    return -1;
                }
                else
                {
                    db.td_seminar_meeting_accept.Add(sma);
                    db.Commit();
                    return sma.Id;
                }
            }
        }
    }
}
