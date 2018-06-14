using log4net;
using Services;
using Services.Models;
using Services.Service;
using Services.Tools.WeiXin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.SessionState;

namespace WEB
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication, IReadOnlySessionState
    {
        protected void Application_Start()
        {
            //读取日志  如果使用log4net,应用程序一开始的时候，都要进行初始化配置
            log4net.Config.XmlConfigurator.Configure();
            Database.SetInitializer<DBContext>(new Initializer());
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
        }
        void Session_Start(object sender, EventArgs e)
        {
            Session.Timeout = 120;
        }
        protected void Application_Error(object sender, EventArgs e)
        {
            ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

            Exception exception = Server.GetLastError();
            log.Error(Request.UserHostAddress + ":" + Request.Url.ToString());
            log.Error(exception);//.Message);
            //Response.Write(exception.Message);
            // Response.Clear();
            if (Response.StatusCode == 404)
            {
                Server.ClearError();
                Response.Redirect("/WeiXin/Error/找不到页面！");
                Response.End();
            }
            else
            {
                //Server.ClearError();
                //Response.Redirect("/WeiXin/Error/数据错误！");
                //Response.End();
            }
            //Response.Write(Request.RawUrl);
        }
        private readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static Regex regqr = new Regex("^/IO/QR/(?<url>([0-9]{1,}))", RegexOptions.Compiled);
        private static Regex regcall = new Regex("^/IO/callback/(?<url>([\\s\\S]{1,}))", RegexOptions.Compiled);
        //private static Regex reg = new Regex("^/AuthorizationUrl\\?(?<url>([0-9]{1,}))", RegexOptions.Compiled);
        void Application_BeginRequest(object sender, EventArgs e)
        {
            log.Info("Request.RawUrl:"+Request.RawUrl);
            Match mccall = regcall.Match(Request.RawUrl);
            if (mccall.Groups["url"].Success)
            {
                Response.Write("success");
                Response.End();
                return;
            }
            Match mc = regqr.Match(Request.RawUrl);
            if (mc.Groups["url"].Success)
            {
                Response.Redirect(Request.Url.ToString().Replace("QR","Authorization/QR"));
                Response.End();
                return;
            }
            //Match mccall = regcall.Match(Request.RawUrl);
            //if (mccall.Groups["url"].Success)
            //{
            //    Response.Write("success");
            //    Response.End();
            //    return;
            //}
            //Match mc = regqr.Match(Request.RawUrl);
            //if (mc.Groups["url"].Success)
            //{
            //    if (Request["code"] == null)
            //    {
            //        //跳转到授权页面
            //        Response.Redirect(OpenWeiXinTools.getWebAuthUrl(System.Configuration.ConfigurationManager.ConnectionStrings["weixin.AppID"].ConnectionString, Request.Url.ToString()
            //            //"http%3a%2f%2fopen.weixin.ecache.com.cn%2fQR%2f1"
            //            , ""));
            //        Response.End();
            //    }
            //    else
            //    {
            //        //if (Request["appid"] == System.Configuration.ConfigurationManager.ConnectionStrings["weixin.AppID"].ConnectionString)
            //        //{
            //        //    OpenweixinService service=new OpenweixinService ();
            //        //    string openid = OpenWeiXinTools.getOpenIDByCode(Request["appid"], Request["code"], service.getAccessToken());
            //        //    Session["openid"] = openid;
            //        //    log.Info(openid);
            //        //}
            //        //Response.Redirect("/Meeting/Index");
            //        //Response.End();
            //    }
            //}
        }
        void Application_AcquireRequestState(object sender, EventArgs e)
        {

            //Match mc = regqr.Match(Request.RawUrl);
            //string url = System.Configuration.ConfigurationManager.ConnectionStrings["url"].ConnectionString;
            //if (mc.Groups["url"].Success)
            //{
            //    if (Request["code"] == null)
            //    {
            //        //跳转到授权页面
            //        Response.Redirect(OpenWeiXinTools.getWebAuthUrl(System.Configuration.ConfigurationManager.ConnectionStrings["weixin.AppID"].ConnectionString, Request.Url.ToString()
            //            //"http%3a%2f%2fopen.weixin.ecache.com.cn%2fQR%2f" + mc.Groups["url"].Value
            //            , ""));
            //        Response.End();
            //    }
            //    else
            //    {
            //        if (Request["appid"] == System.Configuration.ConfigurationManager.ConnectionStrings["weixin.AppID"].ConnectionString)
            //        {
            //            OpenweixinService service = new OpenweixinService();
            //            string openid = OpenWeiXinTools.getOpenIDByCode(Request["code"], Request["appid"], service.getAccessToken());
            //            Session["openid"] = openid;
            //            //根据openid去更新数据
            //            UserInfoService userInfoService = new UserInfoService();
            //            userInfoService.UpdateByOpenid(openid);
            //            log.Info(openid);
            //            UserInfo userInfo = userInfoService.SelectByOpenid(openid);
            //            log.Info(mc.Groups["url"].Value);
            //            int qrid = Convert.ToInt32(mc.Groups["url"].Value);
            //            QRCodeService qrCodeService = new QRCodeService();
            //            QRCode qrCode = qrCodeService.Select(qrid);
            //            if (qrCode != null)
            //            {
                            
            //                if (qrCode.type != 2)
            //                {
            //                    if (userInfo == null)
            //                    {
            //                        Response.Redirect(url + "/portal/wechat/login");
            //                        Response.End();
            //                        return;
            //                    }
            //                    if (string.IsNullOrEmpty(userInfo.statusCode))
            //                    {
            //                        Response.Redirect(url + "/portal/wechat/login");
            //                        Response.End();
            //                        return;
            //                    }
            //                    if (userInfo.statusCode.Equals("Undetermined"))
            //                    {
            //                        Response.Redirect(url + "/portal/wechat/login");
            //                        Response.End();
            //                        return;
            //                    }
            //                    if (userInfo.statusCode.Equals("Registed"))
            //                    {
            //                        //Response.Redirect("/WeiXin/Error/此资源只有认证用户有权限查看，请先认证！");
            //                        Response.Redirect("/Notice/NOPermission");
            //                        Response.End();
            //                        return;
            //                    }
            //                    if (!userInfo.statusCode.Equals("Accepted"))
            //                    {
            //                        Response.Redirect(url + "/portal/wechat/login");
            //                        Response.End();
            //                        return;
            //                    }
            //                    else
            //                    {
            //                        if (qrCode.type == 3)
            //                        {
            //                            //Response.Redirect("/WeiXin/Error/认证成功！");
            //                            Response.Redirect("/Notice/AuthenticationSuccess");
            //                            Response.End();
            //                            return;
            //                        }
            //                    }
            //                }
            //                mdSeminarMeetingMainService mdseminarMeetingMainService = new mdSeminarMeetingMainService();
            //                md_seminar_meeting_main meeting = mdseminarMeetingMainService.GetMeetingById(qrCode.meetingid.Value);
            //                if (meeting == null)
            //                {
            //                    Response.Redirect("/WeiXin/Error/会议不存在！");
            //                    Response.End();
            //                    return;
            //                }

            //                //根据qrid去更新会议
            //                //根据二维码类型去跳转页面
            //                switch (qrCode.type)
            //                {
            //                    case 1://会议邀请函
            //                        Response.Redirect("/Meeting/Invitation/" + qrCode.meetingid);
            //                        Response.End();
            //                        break;
            //                    case 2://会议互动
            //                        //添加会议
            //                        if (meeting.mendtime < DateTime.Now)
            //                        {
            //                            Response.Redirect("/WeiXin/Error/会议已过期！");
            //                            Response.End();
            //                            return;
            //                        }
            //                        td_seminar_meeting_accept accept = new td_seminar_meeting_accept();
            //                        accept.MId = qrCode.meetingid.Value;
            //                        accept.OPenID = openid;
            //                        accept.State = 1;
            //                        accept.CreateOn = DateTime.Now;
            //                        tdSeminarMeetingAcceptService acceptService = new tdSeminarMeetingAcceptService();
            //                        acceptService.Insert(accept);
            //                        Response.Redirect("/Meeting/Interaction/" + qrCode.meetingid);
            //                        Response.End();
            //                        break;
            //                    case 3://医生认证

            //                        break;
            //                    case 4://医生会议邀请函
            //                        Response.Redirect("/Meeting/Invitation/" + qrCode.meetingid);
            //                        Response.End();
            //                        break;
            //                }

            //            }
            //            else
            //            {
            //                if (userInfo == null)
            //                {
            //                    Response.Redirect(url + "/portal/wechat/login");
            //                    Response.End();
            //                    return;
            //                }
            //                if (string.IsNullOrEmpty(userInfo.statusCode))
            //                {
            //                    Response.Redirect(url + "/portal/wechat/login");
            //                    Response.End();
            //                    return;
            //                }
            //                if (userInfo.statusCode.Equals("Undetermined"))
            //                {
            //                    Response.Redirect(url + "/portal/wechat/login");
            //                    Response.End();
            //                    return;
            //                }
            //                if (userInfo.statusCode.Equals("Registed"))
            //                {
            //                    //Response.Redirect("/WeiXin/Error/此资源只有认证用户有权限查看，请先认证！");
            //                    Response.Redirect("/Notice/NOPermission");
            //                    Response.End();
            //                    return;
            //                }
            //                if (!userInfo.statusCode.Equals("Accepted"))
            //                {
            //                    Response.Redirect(url+"/portal/wechat/login");
            //                    Response.End();
            //                    return;
            //                }
            //                Response.Redirect("/Meeting/Index");
            //                Response.End();
            //            }
            //        }
            //        //Response.Write(Request["appid"] + "_____" + Request["code"]);
            //        //Response.End();
            //    }
            //}
        }
    }
}