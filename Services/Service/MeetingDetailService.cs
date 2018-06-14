using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service
{
    public partial class MeetingDetailService
    {
        public List<md_seminar_meeting_detail> GetMeetingDataByMid(int mid)
        {
            using (DBContext db = new DBContext())
            {
                var list = db.md_seminar_meeting_detail.AsNoTracking().Where(x => x.mid == mid).ToList();
                return list;
            }
        }

        public bool GetIsData(int mid)
        {
            using (DBContext db = new DBContext())
            {
                return db.md_seminar_meeting_detail.Any(x=>x.mid==mid);
            }
        }

        public void SendEmail(SendEmail se)
        {
            using (DBContext db = new DBContext())
            {
                db.SendEmail.Add(se);
                db.Commit();
            }
        }
    }
}
