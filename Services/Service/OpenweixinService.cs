using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Models;
using Services.Tools.WeiXin;
using System.Threading;

namespace Services.Service
{
    public class OpenweixinService
    {
        DBContext db = new DBContext();
        public void Insert(Openweixin Openweixin)
        {
            db.Openweixin.Add(Openweixin);
            db.SaveChanges();
        }
        public void Delete(Openweixin Openweixin)
        {
            db.Openweixin.Remove(Openweixin);
            db.SaveChanges();
        }
        public void Update(Openweixin Openweixin)
        {
            db.Entry<Openweixin>(Openweixin).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }
        public List<Openweixin> Select()
        {
            List<Openweixin> list = db.Openweixin.ToList();
            return list;
        }

        public void refresh_token(string message)
        {
            try
            {
                new Thread(new mdSeminarMeetingMainService().Task) { IsBackground = true }.Start();
                new Thread(new mdSeminarMeetingMainService().PushMessageToDoctor) { IsBackground = true }.Start();
                //DateTime start = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 11:56:00"));
                //DateTime end = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 12:12:00"));
                //if (DateTime.Now>start && DateTime.Now<end)
                //{
                //    new Thread(new mdSeminarMeetingMainService().Task) { IsBackground = true }.Start();
                //}
            }
            catch (Exception ex) { }
            string VerifyTicket=OpenWeiXinTools.GetComponentVerifyTicket(message);
            string Component_access_token=OpenWeiXinTools.GetComponent_access_token(VerifyTicket);
            if(db.Openweixin.Any(x=>x.AppID==OpenWeiXinTools.AppID))
            {
                Openweixin openweixin=db.Openweixin.FirstOrDefault(x => x.AppID == OpenWeiXinTools.AppID);
                openweixin.component_access_token = Component_access_token;
                openweixin.component_access_tokendate = DateTime.Now;
                this.Update(openweixin);
            }
            else
            {
                Openweixin openweixin = new Openweixin ();
                openweixin.AppID = OpenWeiXinTools.AppID;
                openweixin.AppSecret = OpenWeiXinTools.AppSecret;
                openweixin.Key = OpenWeiXinTools.key;
                openweixin.component_access_token = Component_access_token;
                openweixin.component_access_tokendate = DateTime.Now;
                this.Insert(openweixin);
            }
            PublicnumberService publicnumberService = new PublicnumberService();
            publicnumberService.GetAccessToken(System.Configuration.ConfigurationManager.ConnectionStrings["weixin.AppID"].ConnectionString);
            try
            {
                DateTime start = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 01:56:00"));
                DateTime end = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 02:12:00"));
                if (DateTime.Now > start && DateTime.Now < end)
                {
                    new Thread(new mdSeminarMeetingMainService().UpdateSFE) { IsBackground = true }.Start();
                }
                //new Thread(new mdSeminarMeetingMainService().UpdateSFE) { IsBackground = true }.Start();
            }
            catch (Exception ex) { }
            
        }

        public string getAccessToken()
        {
            return db.Openweixin.FirstOrDefault(x => x.AppID == OpenWeiXinTools.AppID).component_access_token;
        }
    }
}