using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service
{
    public partial class SurveyService
    {
        public List<md_seminar_survey> GetSurveyByMid(int mid) {
            using (DBContext db = new DBContext())
            {
                var list = db.md_seminar_survey.AsNoTracking().Where(x => x.mid == mid && x.sstate==1).OrderBy(x => x.sid).ToList();
                return list;
            }
        }

        public bool CheckSurvey(int id)
        {
            using (DBContext db = new DBContext())
            {
                return db.md_seminar_survey.Any(x => x.mid == id && x.sstate==1);
            }
        }
    }
}
