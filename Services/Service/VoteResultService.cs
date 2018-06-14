using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service
{
    public partial class VoteResultService
    {
        public int AddVoteResult(td_seminar_vote_result vr)
        {
            using (DBContext db = new DBContext())
            {
                if(db.md_seminar_vote.Any(x=>x.vid==vr.vid && x.vstate==2)){
                    return -2;
                }
                if (db.md_seminar_vote.Any(x => x.vid == vr.vid && x.vstate == 0))
                {
                    return -3;
                }
                if (db.td_seminar_vote_result.Any(x => x.uid == vr.uid && x.vid == vr.vid))
                { 
                    return -1;
                }
                
                if (vr != null)
                {
                    string[] ans = vr.vresult.Split('^');
                    for (int i = 0; i < ans.Length; i++)
                    {
                        if (ans[i].Trim() != "")
                        {
                            vr.vresult = ans[i];
                            db.td_seminar_vote_result.Add(vr);
                            db.Commit();
                        }
                    }
                    return vr.vrid;
                }
                else
                {
                    return 0;
                }  
            }

        }

        public List<td_seminar_vote_result> GetVoteResultList(int vid)
        {
            using (DBContext db = new DBContext())
            {
               var list= db.td_seminar_vote_result.Where(x=>x.vid==vid).ToList();
               return list;
            } 
        }

        public bool IsVote(string uid,int id) {
            using (DBContext db = new DBContext())
            {
                return db.td_seminar_vote_result.Any(x => x.uid ==uid && x.vid ==id);
            } 
        }
    }
}
