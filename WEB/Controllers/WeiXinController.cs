using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEB.Filters;
using Services.Service;
using Services.Models;
using Services.Tools;
using Services.Tools.WeiXin;
using Services.Thirdparty;
using System.Threading;
using Services;

namespace WEB.Controllers
{
    public class WeiXinController : Controller
    {
        //
        // GET: /WeiXin/
        public ActionResult Index()
        {
            OpenweixinService service = new OpenweixinService();

            ViewBag.redirect_uri = OpenWeiXinTools.getOpenWeiXinURL(OpenWeiXinTools.GetPreAuthCode(service.getAccessToken()), System.Configuration.ConfigurationManager.ConnectionStrings["Host"].ConnectionString + "/Authorization/CallBack");
            return View();
        }
        public ActionResult OpenAuthorization()
        {
            OpenweixinService service = new OpenweixinService();
            ViewBag.redirect_uri = OpenWeiXinTools.getOpenWeiXinURL(OpenWeiXinTools.GetPreAuthCode(service.getAccessToken()), System.Configuration.ConfigurationManager.ConnectionStrings["Host"].ConnectionString + "/Authorization/CallBack");
            return View();
        }
        public ActionResult Error(string id)
        {
            ViewBag.message = id;
            return View();
        }
        public void MyMeeting(string id)
        {
            if(id.Equals("open"))
            {
                Response.Redirect("/IO/Authorization/MyMeeting/2");
            }
            else if (id.Equals("notopen"))
            {
                Response.Redirect("/IO/Authorization/MyMeeting/1");
            }
            else
            {
                Response.Redirect("/IO/Authorization/MyMeeting/1");
            }
            //Response.Redirect("/IO/Authorization/QR/0");
        }

        /// <summary>
        /// 我的会议行程
        /// </summary>
        /// <param name="mid"></param>
        public void MyMeetingSchedule(int id)
        {
            Response.Redirect("/IO/Authorization/MyMeetingSchedule/"+id);
        }

        /// <summary>
        /// 医生行程
        /// </summary>
        /// <param name="id"></param>
        public void MyDoctorSchedule(int id)
        {
            Response.Redirect("/IO/Authorization/MyDoctorSchedule/" + id);
        }

        public void MeetingSurvey(int id)
        {
            Response.Redirect("/IO/Authorization/MeetingSurvey/" + id);
        }

        public ActionResult GetWeiXinInfo(string id)
        {
            OpenweixinService service = new OpenweixinService();
            string res = OpenWeiXinTools.getWeiXinInfo(id, service.getAccessToken());
            return Content(res);
        }
        public ActionResult GetAccesstoken()
        {
            PublicnumberService publicnumberService = new PublicnumberService();
            string token=publicnumberService.GetAccessToken(System.Configuration.ConfigurationManager.ConnectionStrings["weixin.AppID"].ConnectionString);
            return Content(token);
        }
        public void Test(string id)
        {
            UserInfoService userInfoService = new UserInfoService();
            UserInfo userInfo = new UserInfo();
            try
            {
                userInfo = userInfoService.SelectByOpenid(id);
            }
            catch (Exception ex)
            {
                return;
            }
        }
    }
}
