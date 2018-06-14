using Services;
using Services.Enums;
using Services.Models;
using Services.Service;
using Services.Thirdparty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEB.Filters;
using static Services.Thirdparty.ThirdpartyService;

namespace WEB.Controllers
{
    [PermissionsAttribute(true)]
    public class SignController : Controller
    {
        //
        // GET: /Sign/
        string url = System.Configuration.ConfigurationManager.ConnectionStrings["url"].ConnectionString;
        public ActionResult MeetingSign(int id)
        {
            ViewBag.url = url;
            string openid = Session["openid"].ToString();
            mdSeminarMeetingMainService service = new mdSeminarMeetingMainService();
            var meeting = service.GetMeetingById(id);
            UserInfoService userInfoService = new UserInfoService();
            var userInfo = userInfoService.SelectByOpenid(openid);
            ViewBag.meeting = meeting;
            ViewBag.userInfo = userInfo;
            return View();
        }
        public ActionResult MeetingRegister(int id)
        {
            ViewBag.url = url;
            string openid = Session["openid"].ToString();
            mdSeminarMeetingMainService service = new mdSeminarMeetingMainService();
            var meeting = service.GetMeetingById(id);
            ViewBag.meeting = meeting;
            return View();
        }
        public ActionResult Success(int id)
        {
            mdSeminarMeetingMainService service = new mdSeminarMeetingMainService();
            var meeting = service.GetMeetingById(id);
            ViewBag.Meeting = meeting;
            string openid = Session["openid"].ToString();
            UserInfoService userInfoService = new UserInfoService();
            var userInfo = userInfoService.SelectByOpenid(openid);
            ViewBag.UserInfo = userInfo;
            MeetingSignService ms = new MeetingSignService();
            ViewBag.MeetingSign = ms.GetMeetingSignInfo(id, openid);
            ViewBag.url = url;
            ViewBag.id = id;
            using (DBContext db = new DBContext())
            {
                var workId = db.sfe_register.FirstOrDefault(r => r.doc_code == userInfo.doctorCode)?.NETWORK_EXTERNAL_ID_BMS_CN__C;
                var inportInfo = db.table_input.FirstOrDefault(t => t.networkid == workId && t.mcode == meeting.mcode && t.ModelType == "邀请文件");
                ViewBag.IsImport = inportInfo != null;
                if (userInfo.statusCode == "Accepted")
                {
                    var sfe = db.sfe_register.FirstOrDefault(s => s.doc_code == userInfo.doctorCode);
                    ViewBag.Sfe = sfe;
                }
            }
            return View();
        }
        public ActionResult Index(int id)
        {
            ViewBag.url = url;
            try
            {
                string openid = Session["openid"].ToString();
                UserInfoService userInfoService = new UserInfoService();
                var userInfo = userInfoService.SelectByOpenid(openid);
                QRCodeService qrCodeService = new QRCodeService();
                QRCode qrCode = qrCodeService.Select(id);
                using (DBContext db = new DBContext())
                {

                    if(db.MeetingSign.Any(x=>x.meetingid== qrCode.meetingid && x.openid== openid))
                    {
                        return RedirectToAction("Success", new { id=qrCode.meetingid });
                    }
                    else
                    {
                        if(userInfo.statusCode == "Accepted"|| userInfo.statusCode == "Registed"|| userInfo.statusCode== "Undetermined")
                        {
                            var meeting = db.md_seminar_meeting_main.AsNoTracking().Where(x => x.mid == qrCode.meetingid.Value).SingleOrDefault();
                            var workId = db.sfe_register.FirstOrDefault(r => r.doc_code == userInfo.doctorCode)?.NETWORK_EXTERNAL_ID_BMS_CN__C;
                            var inportInfo = db.table_input.FirstOrDefault(t => t.networkid == workId && t.mcode == meeting.mcode && t.ModelType == "邀请文件");
                            //非后台导入的大型线下会议直接跳转到签到成功页面
                            if (meeting.meetingmode == (int)MeetingModeTypeEnum.LargeUnderLine && inportInfo == null)
                            {
                                var meetingSign = new MeetingSign
                                {
                                    createTime = DateTime.Now,
                                    meetingid = meeting.mid,
                                    openid = openid
                                };
                                string[] s1 = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
                                Random rd = new Random();
                                string randomInvitationCode = s1[rd.Next(0, s1.Length)] + s1[rd.Next(0, s1.Length)] + rd.Next(100000, 1000000);
                                int count = 0;
                                while (db.MeetingSign.FirstOrDefault(m => m.randominvitationcode == randomInvitationCode && m.meetingid == meeting.mid) != null)
                                {
                                    randomInvitationCode = s1[rd.Next(0, s1.Length)] + s1[rd.Next(0, s1.Length)] + rd.Next(100000, 1000000);
                                    count++;
                                    if (count > 20)
                                        break;
                                }
                                meetingSign.randominvitationcode = randomInvitationCode;
                                db.MeetingSign.Add(meetingSign);
                                db.Commit();
                                return RedirectToAction("Success", new { id = qrCode.meetingid });
                            }
                            else
                            {
                                return RedirectToAction("MeetingSign", new { id = qrCode.meetingid });
                            }
                        }
                        else
                        {
                            string url = System.Configuration.ConfigurationManager.ConnectionStrings["signregister"].ConnectionString;
                            url = string.Format(url, id);//.IndexOf("?") > 0 ? url + "&id=" + id : url + "?id=" + id;
                            return Redirect(url);
                            //return RedirectToAction("MeetingRegister", new { id });
                        }
                    }
                }
                    
            }
            catch(Exception ex)
            {
                return RedirectToAction("Success", new { id });
            }
        }
        public ActionResult GetSMSCode(string phone)
        {
            ThirdpartyService service = new ThirdpartyService();
            object obj = new object();
            try
            {
                service.GetSMSCode(phone);
                obj = new { success = true};
            }
            catch(Exception ex)
            {
                obj = new { success = false, message = ex.Message };
            }
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UserRegister(UserRegisterModel userRegisterModel)
        {
            ThirdpartyService service = new ThirdpartyService();
            object obj = new object();
            try
            {
                service.UserRegister(userRegisterModel);
                obj = new { success = true };
            }
            catch (Exception ex)
            {
                obj = new { success = false, message = ex.Message };
            }
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        
    }
}
