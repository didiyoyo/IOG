using log4net;
using Services.Models;
using Services.Service;
using Services.Tools;
using Services.Tools.WeiXin;
using System;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Reflection;
using System.Threading;
using System.Web.Mvc;

namespace WEB.Filters
{
    //[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class PermissionsAttribute : ActionFilterAttribute
    {
        private readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        bool WeiXinVerification = true;
        /// <summary>
        /// 权限验证属性
        /// </summary>
        /// <param name="WeiXinVerification">是否验证微信访问权限</param>
        /// <param name="AdminVerification">是否验证后台访问权限</param>
        public PermissionsAttribute(bool WeiXinVerification = true)
        {
            this.WeiXinVerification = WeiXinVerification;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request["appid"] == ConfigurationManager.ConnectionStrings["weixin.AppID"].ConnectionString)
            {
                OpenweixinService service = new OpenweixinService();
                string openid = OpenWeiXinTools.getOpenIDByCode(filterContext.HttpContext.Request["code"], filterContext.HttpContext.Request["appid"], service.getAccessToken());
                filterContext.HttpContext.Session["openid"] = openid;
                //UserInfoService userInfoService = new UserInfoService();
                //userInfoService.UpdateByOpenid(openid);
            }
#if DEBUG
            string openID = "o9UKvwwuAHNg7nsO8JSLyz00tQ_4";
            filterContext.HttpContext.Session["openid"] = openID;
#endif
            if (WeiXinVerification)
            {
                if (filterContext.HttpContext.Session["openid"] == null)
                {
                    if (filterContext.HttpContext.Request.HttpMethod.ToUpper().Equals("GET"))
                    {
                        filterContext.HttpContext.Response.Write("<h1>非法访问。</h1>");
                        filterContext.HttpContext.Response.End();
                    }
                    else
                    {
                        filterContext.HttpContext.Response.Write("{\"success\":false,\"message\":\"用户授权失败，请退出后重试！\"}");
                        filterContext.HttpContext.Response.End();
                    }

                }
            }

        }
    }
}
