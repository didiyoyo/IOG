using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Models;
using Services.Tools.WeiXin;

namespace Services.Service
{
    public class PublicnumberService
    {
        DBContext db = new DBContext();
        public void Insert(Publicnumber Publicnumber)
        {
            db.Publicnumber.Add(Publicnumber);
            db.SaveChanges();
        }
        public void Delete(Publicnumber Publicnumber)
        {
            db.Publicnumber.Remove(Publicnumber);
            db.SaveChanges();
        }
        public void Update(Publicnumber Publicnumber)
        {
            db.Entry<Publicnumber>(Publicnumber).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }
        public List<Publicnumber> Select()
        {
            List<Publicnumber> list = db.Publicnumber.ToList();
            return list;
        }

        public void AddbyCode(string auth_code)
        {
            OpenweixinService service = new OpenweixinService();
            Services.Tools.WeiXin.OpenWeiXinTools.Authorization_Info info = OpenWeiXinTools.GetAuthorizer_access_token(service.getAccessToken(), auth_code);
            if (info != null)
            {
                if (db.Publicnumber.Any(x => x.AppID == info.authorizer_appid))
                {
                    Publicnumber number = db.Publicnumber.FirstOrDefault(x => x.AppID == info.authorizer_appid);
                    number.access_token = info.authorizer_access_token;
                    number.refresh_token = info.authorizer_refresh_token;
                    number.refreshdate = DateTime.Now;
                    this.Update(number);
                }
                else
                {
                    Publicnumber number = new Publicnumber();
                    number.AppID = info.authorizer_appid;
                    number.access_token = info.authorizer_access_token;
                    number.refresh_token = info.authorizer_refresh_token;
                    number.refreshdate = DateTime.Now;
                    this.Insert(number);
                }
            }
        }
        public string GetAccessToken(string AppID)
        {
            if(db.Publicnumber.Any(x=>x.AppID==AppID))
            {
                Publicnumber number = db.Publicnumber.FirstOrDefault(x => x.AppID == AppID);
                if (number.refreshdate < DateTime.Now.AddMinutes(20))
                {
                    OpenweixinService service = new OpenweixinService();
                    Services.Tools.WeiXin.OpenWeiXinTools.Authorization_Info info = OpenWeiXinTools.Getauthorizer_refresh_token(service.getAccessToken(), number.refresh_token, AppID);
                    number.access_token = info.authorizer_access_token;
                    number.refresh_token = info.authorizer_refresh_token;
                    number.refreshdate = DateTime.Now.AddSeconds(info.expires_in);
                    this.Update(number);
                    return number.access_token;
                }
                else
                {
                    return number.access_token;
                }
            }
            return "";
        }
    }
}