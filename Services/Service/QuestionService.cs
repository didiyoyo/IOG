using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service
{
     public partial class QuestionService
    {
         public int AddQuestion(td_seminar_question q) {
             using (DBContext db = new DBContext())
             {
                 db.td_seminar_question.Add(q);
                 db.Commit();
                 return q.id;
             }
         }
    }
}
