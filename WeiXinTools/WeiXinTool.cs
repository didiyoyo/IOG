using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using WeiXinTools.Model;

namespace WeiXinTools
{
    /// <summary>
    /// 微信操作类，WeiXinTool.getWeiXinTool(),获取实例
    /// </summary>
    public class WeiXinTool
    {
        private readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        Dictionary<string, Config> configList = new Dictionary<string, Config>();

        #region 单例获取类的对象
        private static WeiXinTool tool = null;
        private static object obj = new object();
        /// <summary>
        /// 初始化变量和环境
        /// </summary>
        private WeiXinTool(string AppID, string AppSecret, string Token)
        {
            if (configList.ContainsKey(AppID))
            {
                Config config = configList[AppID];
                config.AppID = AppID;
                config.AppSecret = AppSecret;
                config.Token = Token;
                config = updateAccessToken(config);
            }
            else
            {
                Config config = new Config();
                config.AppID = AppID;
                config.AppSecret = AppSecret;
                config.Token = Token;
                config = updateAccessToken(config, true);
                configList.Add(AppID, config);
            }
        }
        /// <summary>
        /// 获取类的实例
        /// </summary>
        /// <returns></returns>
        public static WeiXinTool getWeiXinTool(string AppID, string AppSecret, string Token)
        {
            lock (obj)
            {
                if (tool == null)
                {
                    tool = new WeiXinTool(AppID, AppSecret, Token);
                }
                return tool;
            }
        }
        #endregion
        /// <summary>
        /// 更新AccessToken
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private Config updateAccessToken(Config config, bool force = false)
        {
            lock (obj)
            {
                if (force || config.accessTokenDate == null || config.accessTokenDate < DateTime.Now.AddMinutes(-100))
                {
                    string res = HttpWebResponseUtility.CreateGetHttpResponse(Const.access_token + String.Format("?grant_type=client_credential&appid={0}&secret={1}", config.AppID, config.AppSecret), 5000, null, null);
                    AccessTokenModel accessTokenModel = JsonConvert.DeserializeObject<AccessTokenModel>(res);
                    if (!accessTokenModel.errcode.HasValue)
                    {
                        config.accessToken = accessTokenModel.access_token;
                        config.accessTokenDate = DateTime.Now;
                    }
                }
            }
            return config;
        }
        /// <summary>
        /// 根据APPID获取Config信息
        /// </summary>
        /// <param name="AppID"></param>
        /// <returns></returns>
        public Config getConfig(string AppID, bool force = false)
        {
            if (AppID.Equals(""))
            {
                return updateAccessToken(configList.First().Value, force);
            }
            else
            {
                if (configList.ContainsKey(AppID))
                {
                    return updateAccessToken(configList[AppID], force);
                }
                else
                {
                    throw new Exception("未找到AppID：" + AppID + ",在使用时请先初始化！");
                }
            }
        }
        public bool CheckSuccess(int? errorCode)
        {
            if ((!errorCode.HasValue) || errorCode == 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 检查签名
        /// </summary>
        /// <param name="signature"></param>
        /// <param name="timestamp"></param>
        /// <param name="nonce"></param>
        /// <returns></returns>
        public bool CheckSignature(string signature, string timestamp, string nonce,string AppID="")
        {
            Config config = getConfig(AppID);
            return PublicTool.CheckSignature(signature, timestamp, nonce, config.Token);
        }
        /// <summary>
        /// 创建菜单
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public string CreateMenu(MenuModal menu, string AppID = "", int count = 3)
        {
            Config config = getConfig(AppID);
            string res = "";
            do
            {
                try
                {
                    res = HttpWebResponseUtility.CreatePostDataResponse(Const.menu_create + config.accessToken, JsonConvert.SerializeObject(menu));
                    count = 0;
                }
                catch (Exception ex)
                {
                    config = getConfig(AppID, true);
                }
            }
            while (count-- > 0);
            return res;
        }
        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <returns></returns>
        public string DeleteMenu(string AppID = "", int count = 3)
        {
            Config config = getConfig(AppID);
            string res = "";
            do
            {
                try
                {
                    res = HttpWebResponseUtility.CreateGetHttpResponse(Const.menu_delete + config.accessToken, 5000, null, null);
                    count = 0;
                }
                catch (Exception ex)
                {
                    config = getConfig(AppID, true);
                }
            }
            while (count-- > 0);
            return res;
        }
        /// <summary>
        /// 解析消息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public WeiXinMessage getMessage(string data)
        {
            try
            {
                return PublicTool.getMessage(data);
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// 通过openID获取用户信息
        /// </summary>
        /// <param name="openID"></param>
        /// <returns></returns>
        public UserInfo getUserInfo(string openID, string AppID = "", int count = 3)
        {
            Config config = getConfig(AppID);
            UserInfo userInfo = new UserInfo();
            do
            {
                try
                {
                    string res = "";
                    res = HttpWebResponseUtility.CreateGetHttpResponse(string.Format(Const.user_info + "{0}&openid={1}&lang=zh_CN", config.accessToken, openID), 5000, null, null);
                    userInfo = JsonConvert.DeserializeObject<UserInfo>(res);
                    if (CheckSuccess(userInfo.errcode))
                    {
                        count = 0;
                    }
                }
                catch (Exception ex)
                {
                    config = getConfig(AppID, true);
                    log.Info(ex.Message);
                }
            } while (count-- > 0);
            return userInfo;
        }

        /// <summary>
        /// 通过code来获取openID
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public AnsapiUserInfo getOpenIDByCode(string code, string AppID = "", int count = 3)
        {
            log.Info("Code:" + code);
            Config config = getConfig(AppID);
            AnsapiUserInfo ansapiUserInfo = new AnsapiUserInfo();
            do
            {
                try
                {
                    string res = "";
                    res = HttpWebResponseUtility.CreateGetHttpResponse(string.Format(Const.access_token_code + "?appid={0}&secret={1}&code={2}&grant_type=authorization_code", config.AppID, config.AppSecret, code), 5000, null, null);
                    ansapiUserInfo = JsonConvert.DeserializeObject<AnsapiUserInfo>(res);
                    if (CheckSuccess(ansapiUserInfo.errcode))
                    {
                        count = 0;
                    }
                }
                catch (Exception ex)
                {
                    log.Info(ex.Message);
                    log.Info(ex);
                }
                count--;
            } while (count > 0);
            return ansapiUserInfo;
        }
        /// <summary>
        /// 构造微信网页认证的URL
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string getWeiXinAuthorzeURL(string url, string AppID = "")
        {
            Config config = getConfig(AppID);
            return string.Format(Const.weixin_authorize + "?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_base&state=STATE&connect_redirect=1#wechat_redirect", config.AppID, url);
        }
        /// <summary>
        /// 发送客服消息
        /// </summary>
        /// <param name="message"></param>
        public BaseModel sendCustomMessage(CustomMessage message, string AppID = "", int count = 3)
        {
            Config config = getConfig(AppID);
            BaseModel baseModel = new BaseModel();
            do
            {
                string res = "";
                res = HttpWebResponseUtility.CreatePostDataResponse(Const.custom_message + config.accessToken, JsonConvert.SerializeObject(message));
                log.Info("发送客服消息返回：" + res);
                try
                {
                    baseModel = JsonConvert.DeserializeObject<BaseModel>(res);
                    if (CheckSuccess(baseModel.errcode))
                    {
                        count = 0;
                    }
                }
                catch (Exception)
                {
                    config = getConfig(AppID, true);
                }
            } while (count-- > 0);
            return baseModel;
        }
        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="message"></param>
        public ResTemplateMessageModel sendTemplateMessage(object obj, string AppID = "", int count = 3)
        {
            Config config = getConfig(AppID);
            ResTemplateMessageModel templateMessageModel = new ResTemplateMessageModel();
            string res = "";
            do
            {
                try
                {
                    res = HttpWebResponseUtility.CreatePostDataResponse(Const.template_message + config.accessToken, JsonConvert.SerializeObject(obj));
                    templateMessageModel = JsonConvert.DeserializeObject<ResTemplateMessageModel>(res);
                    if (CheckSuccess(templateMessageModel.errcode))
                    {
                        count = 0;
                    }
                }
                catch (Exception)
                {
                    config = getConfig(AppID, true);
                }

            } while (count-- > 0);
            return templateMessageModel;
        }
        /// <summary>
        /// 获取二维码
        /// </summary>
        /// <param name="scene_id"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public ResQRcode GetQRcode(int scene_id, string AppID = "", int count = 3)
        {
            Config config = getConfig(AppID);
            ResQRcode qr = new ResQRcode();
            string str = "{\"expire_seconds\": 2592000, \"action_name\": \"QR_SCENE\", \"action_info\": {\"scene\": {\"scene_id\": " + scene_id + "}}}";
            string res = "";
            do
            {
                try
                {
                    res = HttpWebResponseUtility.CreatePostDataResponse(Const.qrcode + config.accessToken, str);
                    qr = JsonConvert.DeserializeObject<ResQRcode>(res);
                    if (CheckSuccess(qr.errcode))
                    {
                        count = 0;
                    }
                }
                catch (Exception)
                {
                    config = getConfig(AppID, true);
                }

            } while (count-- > 0);
            return qr;
        }
        /// <summary>
        /// 上传临时文件
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public ResUploadMedia UploadMedia(string Type, string filepath, string AppID = "", int count = 3)
        {
            Config config = getConfig(AppID);
            ResUploadMedia ru = new ResUploadMedia();
            string result = "";
            string wxurl = Const.uploadmedia + config.accessToken + "&type=" + Type;
            do
            {
                try
                {
                    WebClient myWebClient = new WebClient();
                    myWebClient.Credentials = CredentialCache.DefaultCredentials;
                    byte[] responseArray = myWebClient.UploadFile(wxurl, "POST", filepath);
                    result = System.Text.Encoding.Default.GetString(responseArray, 0, responseArray.Length);
                    ru = JsonConvert.DeserializeObject<ResUploadMedia>(result);
                    if (CheckSuccess(ru.errcode))
                    {
                        count = 0;
                    }
                }
                catch (Exception)
                {
                    config = getConfig(AppID, true);
                }

            } while (count-- > 0);
            return ru;
        }
        public string getAccessTokenstr(string AppID = "", bool force = false) 
        {
            Config config = getConfig(AppID, force);
            return config.accessToken;
        }
    }
}