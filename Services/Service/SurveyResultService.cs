using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service
{
    public partial class SurveyResultService
    {
        public int AddVoteResult(List<td_seminar_survey_result> sr)
        {
            try
            {
                using (DBContext db = new DBContext())
                {
                    foreach (var item in sr)
                    {
                        if (db.td_seminar_survey_result.Any(x => x.uid ==item.uid && x.sid == item.sid))
                        {
                            return -1;
                        }else
                        {
                            string[] ans = item.sresult.Split('^');
                            for (int i = 0; i < ans.Length; i++)
                            {
                                if (ans[i].Trim() != "")
                                {
                                    item.sresult = ans[i];
                                    db.td_seminar_survey_result.Add(item);
                                    db.Commit();
                                }
                            }
                           
                        }
                    }
                }
                return 1;
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
            

        }
    }
}
