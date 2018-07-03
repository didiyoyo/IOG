using log4net;
using Services;
using Services.Enums;
using Services.Models;
using Services.Service;
using Services.Tools.WeiXin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using WEB.Filters;

namespace WEB.Controllers
{
    public class AuthorizationController : Controller
    {
        private readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        string key = System.Configuration.ConfigurationManager.ConnectionStrings["open.weixin.Key"].ConnectionString;
        //
        // GET: /Authorization/

        public ActionResult Index()
        {
            StreamReader sr = new StreamReader(Request.GetBufferedInputStream());
            string res = sr.ReadToEnd();
            log.Info(res);
            OpenweixinService service = new OpenweixinService();
            service.refresh_token(res);
            return Content("success");
        }
        public ActionResult CallBack(string auth_code)
        {
            PublicnumberService service = new PublicnumberService();
            service.AddbyCode(auth_code);
            log.Info(Request.RawUrl);
            return Content("<h1>授权成功！</h1>");
        }
        [PermissionsAttribute(false)]
        public void MeetingData(int id)
        {
            if (Session["openid"] == null)
            {
                Response.Redirect(OpenWeiXinTools.getWebAuthUrl(System.Configuration.ConfigurationManager.ConnectionStrings["weixin.AppID"].ConnectionString, Request.Url.ToString(), ""));
                return;
            }
            else
            {
                Response.Redirect("/IO/Meeting/DataList/"+id);
            }
        }
        [PermissionsAttribute(false)]
        public void SurveyData(int id)
        {
            if (Session["openid"] == null)
            {
                Response.Redirect(OpenWeiXinTools.getWebAuthUrl(System.Configuration.ConfigurationManager.ConnectionStrings["weixin.AppID"].ConnectionString, Request.Url.ToString(), ""));
                return;
            }
            else
            {
                Response.Redirect("/IO/Survey/Index?id=" + id);
            }
        }
        [PermissionsAttribute(false)]
        public void MyMeeting(string id)
        {
            if (Session["openid"] == null)
            {
                Response.Redirect(OpenWeiXinTools.getWebAuthUrl(System.Configuration.ConfigurationManager.ConnectionStrings["weixin.AppID"].ConnectionString, Request.Url.ToString(), ""));
                return;
            }
            else
            {
                string url = System.Configuration.ConfigurationManager.ConnectionStrings["url"].ConnectionString;

                UserInfoService userInfoService = new UserInfoService();
                mdSeminarMeetingMainService mdseminarMeetingMainService = new mdSeminarMeetingMainService();
                UserInfo userInfo = new UserInfo();
                try
                {
                    userInfo = userInfoService.SelectByOpenid(Session["openid"].ToString());
                }
                catch (Exception ex)
                {
                    log.Info("获取用户信息失败！");
                    log.Error(ex);
                    Response.Redirect(url + "/portal/wechat/login");
                    Response.End();
                    return;
                }
                if (userInfo == null)//未获取到用户信息
                {
                    Response.Redirect("/IO/WeiXin/Error/获取用户信息错误！");
                    Response.End();
                    return;
                }
                if (string.IsNullOrWhiteSpace(userInfo.statusCode))
                {
                    Response.Redirect(url + "/portal/wechat/login");
                    Response.End();
                }
                else if (userInfo.statusCode.Equals("Accepted") || userInfo.statusCode.Equals("Undetermined") || userInfo.statusCode.Equals("Rejected"))//已认证跳转到我的会议页面
                {
                    Response.Redirect("/IO/Meeting/Index?id=" + id);
                    Response.End();
                }
                else//未认证跳转到登录页面
                {
                    Response.Redirect(url + "/portal/wechat/login");
                    Response.End();
                }
            }
        }

        [PermissionsAttribute(false)]
        public void MyMeetingSchedule(int id)
        {
            if (Session["openid"] == null)
            {
                Response.Redirect(OpenWeiXinTools.getWebAuthUrl(System.Configuration.ConfigurationManager.ConnectionStrings["weixin.AppID"].ConnectionString, Request.Url.ToString(), ""));
                return;
            }
            else
            {
                string url = System.Configuration.ConfigurationManager.ConnectionStrings["url"].ConnectionString;

                UserInfoService userInfoService = new UserInfoService();
                UserInfo userInfo = new UserInfo();
                try
                {
                    userInfo = userInfoService.SelectByOpenid(Session["openid"].ToString());
                }
                catch (Exception ex)
                {
                    log.Info("获取用户信息失败！");
                    log.Error(ex);
                    Response.Redirect(url + "/portal/wechat/login");
                    Response.End();
                    return;
                }
                if (userInfo == null)//未获取到用户信息
                {
                    Response.Redirect("/IO/WeiXin/Error/获取用户信息错误！");
                    Response.End();
                    return;
                }
                if (string.IsNullOrWhiteSpace(userInfo.statusCode))
                {
                    Response.Redirect(url + "/portal/wechat/login");
                    Response.End();
                }
                if (userInfo.statusCode.Equals("Accepted"))//已认证跳转到我的会议页面
                {
                    Response.Redirect("/IO/Meeting/MeetingSchedule?id=" + id);
                    Response.End();
                }
                else if (userInfo.statusCode.Equals("Undetermined"))
                {
                    Response.Redirect("/IO/Meeting/MeetingSchedule?id=" + id);
                    Response.End();
                }
                else if (userInfo.statusCode.Equals("Registed"))//已注册的如果有会议可以进我的会议列表
                {
                    Response.Redirect("/IO/Meeting/MeetingSchedule?id=" + id);
                    Response.End();
                }
                else//未认证跳转到登录页面
                {
                    Response.Redirect(url + "/portal/wechat/login");
                    Response.End();
                }
            }
        }

        [PermissionsAttribute(false)]
        public void MyDoctorSchedule(int id)
        {
            if (Session["openid"] == null)
            {
                Response.Redirect(OpenWeiXinTools.getWebAuthUrl(System.Configuration.ConfigurationManager.ConnectionStrings["weixin.AppID"].ConnectionString, Request.Url.ToString(), ""));
                return;
            }
            else
            {
                string url = System.Configuration.ConfigurationManager.ConnectionStrings["url"].ConnectionString;

                UserInfoService userInfoService = new UserInfoService();
                mdSeminarMeetingMainService mdseminarMeetingMainService = new mdSeminarMeetingMainService();
                UserInfo userInfo = new UserInfo();
                try
                {
                    userInfo = userInfoService.SelectByOpenid(Session["openid"].ToString());
                }
                catch (Exception ex)
                {
                    log.Info("获取用户信息失败！");
                    log.Error(ex);
                    Response.Redirect(url + "/portal/wechat/login");
                    Response.End();
                    return;
                }
                if (userInfo == null)//未获取到用户信息
                {
                    Response.Redirect("/IO/WeiXin/Error/获取用户信息错误！");
                    Response.End();
                    return;
                }
                if (string.IsNullOrWhiteSpace(userInfo.statusCode))
                {
                    Response.Redirect(url + "/portal/wechat/login");
                    Response.End();
                }
                else if (userInfo.statusCode.Equals("Accepted") || userInfo.statusCode.Equals("Undetermined") || userInfo.statusCode.Equals("Rejected"))
                {
                    Response.Redirect("/IO/Meeting/DoctorSchedule?mid=" + id);
                    Response.End();
                }
                else//未认证跳转到登录页面
                {
                    Response.Redirect(url + "/portal/wechat/login");
                    Response.End();
                }
            }
        }

        [PermissionsAttribute(false)]
        public void MeetingSurvey(int id)
        {
            if (Session["openid"] == null)
            {
                Response.Redirect(OpenWeiXinTools.getWebAuthUrl(System.Configuration.ConfigurationManager.ConnectionStrings["weixin.AppID"].ConnectionString, Request.Url.ToString(), ""));
                return;
            }
            else
            {
                string url = System.Configuration.ConfigurationManager.ConnectionStrings["url"].ConnectionString;

                UserInfoService userInfoService = new UserInfoService();
                mdSeminarMeetingMainService mdseminarMeetingMainService = new mdSeminarMeetingMainService();
                UserInfo userInfo = new UserInfo();
                try
                {
                    userInfo = userInfoService.SelectByOpenid(Session["openid"].ToString());
                }
                catch (Exception ex)
                {
                    log.Info("获取用户信息失败！");
                    log.Error(ex);
                    Response.Redirect(url + "/portal/wechat/login");
                    Response.End();
                    return;
                }
                if (userInfo == null)//未获取到用户信息
                {
                    Response.Redirect("/IO/WeiXin/Error/获取用户信息错误！");
                    Response.End();
                    return;
                }
                if (string.IsNullOrWhiteSpace(userInfo.statusCode))
                {
                    Response.Redirect(url + "/portal/wechat/login");
                    Response.End();
                }
                else if (userInfo.statusCode.Equals("Accepted") || userInfo.statusCode.Equals("Undetermined") || userInfo.statusCode.Equals("Rejected"))
                {
                    using (DBContext db = new DBContext())
                    {
                        var survayIdList = db.md_seminar_survey.Where(s => s.mid == id).Select(s=>s.sid).ToList();
                        var session = Session["openid"].ToString();
                        var survayResultList = db.td_seminar_survey_result.FirstOrDefault(sr => survayIdList.Contains(sr.sid) && sr.uid == session);
                        if (survayResultList != null)
                        {
                            Response.Redirect("/IO/Survey/AlreadySurveyPrompt");
                            Response.End();
                        }
                        else
                        {
                            Response.Redirect("/IO/Survey/Index?id=" + id);
                            Response.End();
                        }
                    }
                }
                else//未认证跳转到登录页面
                {
                    Response.Redirect(url + "/portal/wechat/login");
                    Response.End();
                }
            }
        }

        /// <summary>
        /// 二维码页面，所有入口
        /// </summary>
        /// <param name="id"></param>
        [PermissionsAttribute(false)]
        public void QR(int id)
        {
            if (Session["openid"] == null)
            {
                Response.Redirect(OpenWeiXinTools.getWebAuthUrl(System.Configuration.ConfigurationManager.ConnectionStrings["weixin.AppID"].ConnectionString, Request.Url.ToString(), ""));
                return;
            }
            else
            {
                string url = System.Configuration.ConfigurationManager.ConnectionStrings["url"].ConnectionString;

                UserInfoService userInfoService = new UserInfoService();
                mdSeminarMeetingMainService mdseminarMeetingMainService = new mdSeminarMeetingMainService();
                UserInfo userInfo = new UserInfo();
                try
                {
                    userInfo = userInfoService.SelectByOpenid(Session["openid"].ToString());
                }
                catch (Exception ex)
                {
                    log.Info("获取用户信息失败！");
                    log.Error(ex);
                    Response.Redirect(url + "/portal/wechat/login");
                    Response.End();
                    return;
                }
                QRCodeService qrCodeService = new QRCodeService();
                QRCode qrCode = qrCodeService.Select(id);


                if (userInfo == null)//未获取到用户信息
                {
                    Response.Redirect("/IO/WeiXin/Error/获取用户信息错误！");
                    Response.End();
                    return;
                }
                if (qrCode == null)
                {
                    if (string.IsNullOrWhiteSpace(userInfo.statusCode))
                    {
                        Response.Redirect(url + "/portal/wechat/login");
                        Response.End();
                    }
                    else if (userInfo.statusCode.Equals("Accepted") || userInfo.statusCode.Equals("Undetermined") || userInfo.statusCode.Equals("Rejected"))//已认证跳转到我的会议页面
                    {
                        Response.Redirect("/IO/Meeting/Index");
                        Response.End();
                    }
                    else//未认证跳转到登录页面
                    {
                        Response.Redirect(url + "/portal/wechat/login");
                        Response.End();
                    }
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(userInfo.statusCode)&&qrCode.type!=2)
                    {
                        Response.Redirect(url + "/portal/wechat/login");
                        Response.End();
                    }
                    else
                    {
                        md_seminar_meeting_main meeting = mdseminarMeetingMainService.GetMeetingById(qrCode.meetingid.Value);
                        if (qrCode.type == 5)
                        {
                            Response.Redirect("/IO/Sign/Index?id=" + id);
                            Response.End();
                            return;
                        }
                        else if (qrCode.type == 3)//医生认证
                        {
                            if (userInfo.statusCode.Equals("Accepted"))//已认证跳转到认证成功页面
                            {
                                Response.Redirect("/IO/Notice/AuthenticationSuccess");
                                Response.End();
                            }
                            else//未认证跳转到登录页面
                            {
                                Response.Redirect(url + "/portal/wechat/login");
                                Response.End();
                            }
                        }
                        else
                        {
                            if (meeting == null|| meeting.Type==1)//会议不存在
                            {
                                //if (userInfo.statusCode.Equals("Accepted"))//已认证跳转到我的会议页面
                                //{
                                //    Response.Redirect("/IO/Meeting/Index");
                                //    Response.End();
                                //}
                                //else//未认证跳转到登录页面
                                //{
                                //    Response.Redirect(url + "/portal/wechat/login");
                                //    Response.End();
                                //}
                                Response.Redirect("/IO/Error/Index");
                                Response.End();
                                return;
                            }
                            if (qrCode.type == 2)//会议互动，只要没有过期都可以进去
                            {
                                if (meeting.mendtime < DateTime.Now)
                                {
                                    Response.Redirect("/IO/WeiXin/Error/会议已过期！");
                                    Response.End();
                                    return;
                                }
                                else
                                {
                                    Response.Redirect("/IO/Vote/Index/" + qrCode.meetingid);
                                    Response.End();
                                }
                            }
                            else//会议邀请函或医生会议邀请函
                            {
                                //认证的用户跳转到会议邀请函，已注册的跳转到会议邀请码页面
                                if (userInfo.statusCode.Equals("Accepted"))//认证的用户跳转到会议邀请函
                                {
                                    Response.Redirect("/IO/Meeting/MeetingSchedule/" + qrCode.meetingid);
                                    Response.End();
                                }
                                else if (userInfo.statusCode.Equals("Rejected") || userInfo.statusCode.Equals("Undetermined"))//已注册的跳转到会议邀请码页面
                                {
                                    if (meeting.meetingmode == (int)MeetingModeTypeEnum.LargeUnderLine && userInfo.statusCode.Equals("Undetermined") && (qrCode.type == 4 || qrCode.type == 1))
                                    {
                                        Response.Redirect("/IO/Meeting/MeetingSchedule/" + qrCode.meetingid);
                                        Response.End();
                                    }
                                    else
                                    {
                                        if (userInfo.statusCode.Equals("Undetermined") && (qrCode.type == 4 || qrCode.type == 1))
                                        {
                                            using (var context = new DBContext())
                                            {
                                                if (!string.IsNullOrEmpty(userInfo.doctorCode))
                                                {
                                                    var sfe = context.sfe_register.FirstOrDefault(s => s.doc_code == userInfo.doctorCode);
                                                    if (sfe != null)
                                                    {
                                                        Response.Redirect("/IO/Meeting/MeetingSchedule/" + qrCode.meetingid);
                                                        Response.End();
                                                    }
                                                    else
                                                    {
                                                        Response.Redirect("/IO/Meeting/MeetingCode/" + qrCode.meetingid);
                                                        //Response.Redirect("/Notice/NOPermission");
                                                        Response.End();
                                                    }
                                                }
                                                else
                                                {
                                                    Response.Redirect("/IO/Meeting/MeetingCode/" + qrCode.meetingid);
                                                    //Response.Redirect("/Notice/NOPermission");
                                                    Response.End();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            Response.Redirect("/IO/Meeting/MeetingCode/" + qrCode.meetingid);
                                            //Response.Redirect("/Notice/NOPermission");
                                            Response.End();
                                        }
                                    }
                                }
                                else//未认证跳转到登录页面
                                {
                                    Response.Redirect(url + "/portal/wechat/login");
                                    Response.End();
                                }

                            }
                        }
                    }
                }
            }
        }
    }
}
