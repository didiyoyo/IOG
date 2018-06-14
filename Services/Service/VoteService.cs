using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service
{
    public partial class VoteService
    {
        public List<md_seminar_vote> GetVoteByMid(int mid)
        {
            using (DBContext db = new DBContext())
            {
                var list = db.md_seminar_vote.AsNoTracking().Where(x => x.mid == mid ).ToList();
                return list;
            }
        }

        public md_seminar_vote GetVoteByid(int id)
        {
            using (DBContext db = new DBContext())
            {
                var vote = db.md_seminar_vote.AsNoTracking().Where(x => x.vid == id).SingleOrDefault(); 
                return vote;
            }
        }

        public bool CheckVote(int id)
        {
            using (DBContext db = new DBContext())
            { 
                return db.md_seminar_vote.Any(x=>x.mid==id );
            }
        }
    }
}
