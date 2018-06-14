using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Models;

namespace Services.Service
{
    public class QRCodeService
    {
        DBContext db = new DBContext();
        public void Insert(QRCode QRCode)
        {
            db.QRCode.Add(QRCode);
                db.SaveChanges();
        }
        public void Delete(QRCode QRCode)
        {
            db.QRCode.Remove(QRCode);
            db.SaveChanges();
        }
        public void Update(QRCode QRCode)
        {
            db.Entry<QRCode>(QRCode).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }
        public List<QRCode> Select()
        {
            List<QRCode> list = db.QRCode.ToList();
            return list;
        }
        public QRCode Select(int id)
        {
            return db.QRCode.FirstOrDefault(x => x.id == id);
        }
    }
}