using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace Services.Tools.WeiXin
{
    public class OpenWeiXinTools
    {
        public static string AppSecret = System.Configuration.ConfigurationManager.ConnectionStrings["open.weixin.AppSecret"].ConnectionString;
        public static string AppID = System.Configuration.ConfigurationManager.ConnectionStrings["open.weixin.AppID"].ConnectionString;
        public static string key = System.Configuration.ConfigurationManager.ConnectionStrings["open.weixin.Key"].ConnectionString;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static Regex reg = new Regex("\\<xml\\>[\\s\\S]{0,}\\</xml\\>", RegexOptions.Compiled);
        /// <summary>
        /// 获取推送的ComponentVerifyTicket
        /// </summary>
        /// <param name="message">推送的XML消息</param>
        /// <returns></returns>
        public static string GetComponentVerifyTicket(string message)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode root;
            doc.LoadXml(message);
            root = doc.FirstChild;
            string sEncryptMsg = root["Encrypt"].InnerText;
            string str = AESHelper.AESDecrypt(key, sEncryptMsg);
            Match mc = reg.Match(str);
            if (mc.Success)
            {
                doc.LoadXml(mc.Value);
                root = doc.FirstChild;
                log.Info(root["ComponentVerifyTicket"].InnerText);
                return root["ComponentVerifyTicket"].InnerText;
                //return mc.Value;
            }
            return "";
        }
        /// <summary>
        /// 获取接口调用凭据Component_access_token
        /// </summary>
        /// <param name="componentVerifyTicket">推送的ComponentVerifyTicket</param>
        /// <returns></returns>
        public static string GetComponent_access_token(string componentVerifyTicket)
        {
            var data = new { component_appid = AppID, component_appsecret = AppSecret, component_verify_ticket = componentVerifyTicket };
            string res = HttpWebResponseUtility.CreatePostDataResponse("https://api.weixin.qq.com/cgi-bin/component/api_component_token", JsonConvert.SerializeObject(data));
            log.Info(res);
            ResModel resModel = JsonConvert.DeserializeObject<ResModel>(res);
            if (resModel.errcode.HasValue)
            {
                return "";
            }
            else
            {
                return resModel.component_access_token;
            }
        }
        /// <summary>
        /// 获取公众号预授权码
        /// </summary>
        /// <param name="component_access_token">接口调用凭据</param>
        /// <returns></returns>
        public static string GetPreAuthCode(string component_access_token)
        {
            var data = new { component_appid = AppID };
            string res = HttpWebResponseUtility.CreatePostDataResponse("https://api.weixin.qq.com/cgi-bin/component/api_create_preauthcode?component_access_token=" + component_access_token, JsonConvert.SerializeObject(data));
            ResModel resModel = JsonConvert.DeserializeObject<ResModel>(res);
            if (resModel.errcode.HasValue)
            {
                return "";
            }
            else
            {
                return resModel.pre_auth_code;
            }
        }
        /// <summary>
        /// 获取微信公众号授权URL
        /// </summary>
        /// <param name="pre_auth_code"></param>
        /// <param name="redirect_uri"></param>
        /// <returns></returns>
        public static string getOpenWeiXinURL(string pre_auth_code, string redirect_uri)
        {
            return "https://mp.weixin.qq.com/cgi-bin/componentloginpage?component_appid="+AppID+"&pre_auth_code="+pre_auth_code+"&redirect_uri=" + redirect_uri;
        }
        /// <summary>
        /// 获取第三方公众号接口调用凭据
        /// </summary>
        /// <param name="component_access_token"></param>
        /// <param name="authorization_code"></param>
        /// <returns></returns>
        public static Authorization_Info GetAuthorizer_access_token(string component_access_token, string authorization_code)
        {
            var data = new { component_appid = AppID, authorization_code = authorization_code };
            string res = HttpWebResponseUtility.CreatePostDataResponse("https://api.weixin.qq.com/cgi-bin/component/api_query_auth?component_access_token=" + component_access_token, JsonConvert.SerializeObject(data));
            ResModel resModel = JsonConvert.DeserializeObject<ResModel>(res);
            if (resModel.errcode.HasValue)
            {
                return null;
            }
            else
            {
                return resModel.authorization_info;
            }
        }
        /// <summary>
        /// 刷新第三方公众号的接口调用凭据
        /// </summary>
        /// <param name="component_access_token">接口调用凭据</param>
        /// <param name="authorizer_refresh_token">第三方公众号的刷新凭据</param>
        /// <param name="authorizer_appid">第三方APPID</param>
        /// <returns></returns>
        public static Authorization_Info Getauthorizer_refresh_token(string component_access_token, string authorizer_refresh_token,string authorizer_appid)
        {
            var data = new { component_appid = AppID, authorizer_appid = authorizer_appid, authorizer_refresh_token = authorizer_refresh_token };
            string res = HttpWebResponseUtility.CreatePostDataResponse("https://api.weixin.qq.com/cgi-bin/component/api_authorizer_token?component_access_token=" + component_access_token, JsonConvert.SerializeObject(data));
            log.Info(res);
            Authorization_Info authorization_Info = JsonConvert.DeserializeObject<Authorization_Info>(res);
            if (authorization_Info.errcode.HasValue)
            {
                return null;
            }
            else
            {
                return authorization_Info;
            }
        }
        /// <summary>
        /// 获取第三方公众号用户网页授权URL
        /// </summary>
        /// <param name="appid">第三方公众号的APPID</param>
        /// <param name="redirect_uri">回调URL</param>
        /// <param name="state">带参state</param>
        /// <returns></returns>
        public static string getWebAuthUrl(string appid,string redirect_uri,string state)
        {
            return "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + appid + "&redirect_uri=" + redirect_uri + "&response_type=code&scope=snsapi_base&state="+state+"&component_appid=" + AppID + "#wechat_redirect";
        }
        /// <summary>
        /// 根据网页授权Code获取用户openid
        /// </summary>
        /// <param name="code">code</param>
        /// <param name="appid">公众号的APPID</param>
        /// <param name="component_access_token">接口调用凭据</param>
        /// <returns></returns>
        public static string getOpenIDByCode(string code,string appid,string component_access_token)
        {
            string res = HttpWebResponseUtility.CreateGetHttpResponse("https://api.weixin.qq.com/sns/oauth2/component/access_token?appid="+appid+"&code="+code+"&grant_type=authorization_code&component_appid="+AppID+"&component_access_token=" + component_access_token, 5000, null, null);
            log.Info(res);
            ResModel resModel = JsonConvert.DeserializeObject<ResModel>(res);
            if (resModel.errcode.HasValue)
            {
                return null;
            }
            else
            {
                return resModel.openid;
            }
        }
        public static string getWeiXinInfo(string appid,string component_access_token)
        {
            var data = new { component_appid = AppID, authorizer_appid = appid};
            return HttpWebResponseUtility.CreatePostDataResponse("https://api.weixin.qq.com/cgi-bin/component/api_get_authorizer_info?component_access_token=" + component_access_token, JsonConvert.SerializeObject(data));
        }



        private class ResModel
        {
            public string component_access_token { get; set; }
            public int expires_in { get; set; }
            public int? errcode { get; set; }
            public string errmsg { get; set; }
            public string pre_auth_code { get; set; }
            public Authorization_Info authorization_info { get; set; }
            public string access_token { get; set; }
            public string refresh_token { get; set; }
            public string openid { get; set; }
            public string scope { get; set; }
        }
        public class Authorization_Info
        {
            public string authorizer_appid { get; set; }
            public string authorizer_access_token { get; set; }
            public int expires_in { get; set; }
            public string authorizer_refresh_token { get; set; }
            public Func_Info[] func_info { get; set; }
            public int? errcode { get; set; }
            public string errmsg { get; set; }
        }

        public class Func_Info
        {
            public Funcscope_Category funcscope_category { get; set; }
        }

        public class Funcscope_Category
        {
            public int id { get; set; }
        }

    }

}
