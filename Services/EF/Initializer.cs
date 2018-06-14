using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class Initializer : System.Data.Entity.CreateDatabaseIfNotExists<DBContext>
    {
        protected override void Seed(DBContext _context)
        {
            context = _context;




            context.Commit();
        }
        DBContext context;


    }
}
