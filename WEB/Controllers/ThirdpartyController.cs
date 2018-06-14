using Services;
using Services.Models;
using Services.Service;
using Services.Tools.WeiXin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEB.Filters;

namespace WEB.Controllers
{
    public class ThirdpartyController : Controller
    {
        [HttpPost]
        public ActionResult GetMeetingInfo(RequestModel request)
        {
            object obj = new object();
            try
            {
                QRCodeService qrCodeService = new QRCodeService();
                QRCode qrCode = qrCodeService.Select(request.id);
                mdSeminarMeetingMainService service = new mdSeminarMeetingMainService();
                var meeting = service.GetMeetingById(qrCode.meetingid.Value);
                var data = new
                {
                    title = meeting.mtitle,
                    starttime = meeting.mbegintime.Value.ToString("yyyy-MM-dd HH:mm"),
                    endtime = meeting.mendtime.Value.ToString("yyyy-MM-dd HH:mm"),
                    address = meeting.msite,
                    meeting.InvitationCode
                };
                obj = new { success = true, data };
            }
            catch(Exception ex)
            {
                obj = new { success = false, message = "参数错误！" };
            }
            return Json(obj,JsonRequestBehavior.AllowGet);
        }
        [PermissionsAttribute(false)]
        public void SignSuccess(int id)
        {
            if (Session["openid"] == null)
            {
                Response.Redirect(OpenWeiXinTools.getWebAuthUrl(System.Configuration.ConfigurationManager.ConnectionStrings["weixin.AppID"].ConnectionString, Request.Url.ToString(), ""));
                return;
            }
            try
            {
                QRCodeService qrCodeService = new QRCodeService();
                QRCode qrCode = qrCodeService.Select(id);
                string openid = Session["openid"].ToString();
                using (DBContext db = new DBContext())
                {
                    if (db.MeetingSign.Any(x => x.meetingid == qrCode.meetingid && x.openid == openid))
                    {
                        
                    }
                    else
                    {
                        db.MeetingSign.Add(new MeetingSign()
                        {
                            createTime = DateTime.Now,
                            meetingid = qrCode.meetingid.Value,
                            openid = openid
                        });
                        db.Commit();
                    }
                    
                }
                Response.Redirect("/IO/Sign/Success/" + qrCode.meetingid);
                Response.End();
            }
            catch (Exception ex)
            {
                Response.Redirect("/IO/Sign/Index/"+id);
                Response.End();
            }
        }
        public class RequestModel {
            public int id { get; set; }
        }

    }
}
