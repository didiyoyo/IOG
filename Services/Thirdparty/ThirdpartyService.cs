using log4net;
using Newtonsoft.Json;
using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Services.Thirdparty
{
    public class ThirdpartyService
    {
        private readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public UserInfo getUserInfo(string openid)
        {
            var obj = new { timestamp = (DateTime.Now - DateTime.Parse("1970-01-01 00:00:00")).TotalMilliseconds, openId = openid };
            requestModel req = new requestModel()
            {
                pId = System.Configuration.ConfigurationManager.ConnectionStrings["pid"].ConnectionString,
                encryptContent = AESHelper.AESEncrypt(JsonConvert.SerializeObject(obj))
            };
            try
            {
                string res = HttpWebResponseUtility.CreatePostDataResponse(System.Configuration.ConfigurationManager.ConnectionStrings["url"].ConnectionString +
                    System.Configuration.ConfigurationManager.ConnectionStrings["userInfo"].ConnectionString, JsonConvert.SerializeObject(req));
                log.Debug("获取用户信息：" + res);
                responseModel response = JsonConvert.DeserializeObject<responseModel>(res);
                if (response.status.Equals("success"))
                {
                    log.Debug("解析用户信息：" + AESHelper.AESDecrypt(response.encryptionBody));
                    UserInfo userInfo = JsonConvert.DeserializeObject<UserInfo>(AESHelper.AESDecrypt(response.encryptionBody));
                    return userInfo;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public string getQR(QRModel qr)
        {
            qr.timestamp = (DateTime.Now - DateTime.Parse("1970-01-01 00:00:00")).TotalMilliseconds;
            requestModel req = new requestModel()
            {
                pId = System.Configuration.ConfigurationManager.ConnectionStrings["pid"].ConnectionString,
                encryptContent = AESHelper.AESEncrypt(JsonConvert.SerializeObject(qr))
            };
            try
            {
                string res = HttpWebResponseUtility.CreatePostDataResponse(System.Configuration.ConfigurationManager.ConnectionStrings["url"].ConnectionString +
                    System.Configuration.ConfigurationManager.ConnectionStrings["getqr"].ConnectionString, JsonConvert.SerializeObject(req));
                responseModel response = JsonConvert.DeserializeObject<responseModel>(res);
                if (response.status.Equals("success"))
                {
                    return response.qr_image;
                }
                else
                {
                    throw new Exception(response.message);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public string GetSMSCode(string phone)
        {
            object obj = new
            {

                timestamp = (DateTime.Now - DateTime.Parse("1970-01-01 00:00:00")).TotalMilliseconds,
                mobile = phone
            };
            requestModel req = new requestModel()
            {
                pId = System.Configuration.ConfigurationManager.ConnectionStrings["pid"].ConnectionString,
                encryptContent = AESHelper.AESEncrypt(JsonConvert.SerializeObject(obj))
            };
            try
            {
                string res = HttpWebResponseUtility.CreatePostDataResponse(System.Configuration.ConfigurationManager.ConnectionStrings["url"].ConnectionString +
                    System.Configuration.ConfigurationManager.ConnectionStrings["getsmscode"].ConnectionString, JsonConvert.SerializeObject(req));
                responseModel response = JsonConvert.DeserializeObject<responseModel>(res);
                if (response.status.Equals("success"))
                {
                    AESHelper.AESDecrypt(response.encryptionBody);
                    SMSModel smsmodel = JsonConvert.DeserializeObject<SMSModel>(AESHelper.AESDecrypt(response.encryptionBody));
                    return smsmodel.sms_code;
                }
                else
                {
                    throw new Exception(response.message);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string UserRegister(UserRegisterModel userRegisterModel)
        {
            userRegisterModel.timestamp = (DateTime.Now - DateTime.Parse("1970-01-01 00:00:00")).TotalMilliseconds;
            requestModel req = new requestModel()
            {
                pId = System.Configuration.ConfigurationManager.ConnectionStrings["pid"].ConnectionString,
                encryptContent = AESHelper.AESEncrypt(JsonConvert.SerializeObject(userRegisterModel))
            };
            string res = HttpWebResponseUtility.CreatePostDataResponse(System.Configuration.ConfigurationManager.ConnectionStrings["url"].ConnectionString +
                     System.Configuration.ConfigurationManager.ConnectionStrings["register"].ConnectionString, JsonConvert.SerializeObject(req));
            responseModel response = JsonConvert.DeserializeObject<responseModel>(res);
            if (response.status.Equals("success"))
            {
                return response.qr_image;
            }
            else
            {
                throw new Exception(response.message);
            }
        }

        public bool ChangeEmail(string openId, string email)
        {
            object obj = new
            {

                timestamp = (DateTime.Now - DateTime.Parse("1970-01-01 00:00:00")).TotalMilliseconds,
                email,
                openId
            };
            requestModel req = new requestModel()
            {
                pId = System.Configuration.ConfigurationManager.ConnectionStrings["pid"].ConnectionString,
                encryptContent = AESHelper.AESEncrypt(JsonConvert.SerializeObject(obj))
            };
            try
            {
                string res = HttpWebResponseUtility.CreatePostDataResponse(System.Configuration.ConfigurationManager.ConnectionStrings["url"].ConnectionString +
                    System.Configuration.ConfigurationManager.ConnectionStrings["changeemail"].ConnectionString, JsonConvert.SerializeObject(req));
                responseModel response = JsonConvert.DeserializeObject<responseModel>(res);
                if (response.status.Equals("success"))
                {
                    return true;
                }
                else
                {
                    throw new Exception(response.message);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public class SMSModel
        {
            public string sms_code { get; set; }
        }
        public class requestModel
        {
            public string pId { get; set; }
            public string encryptContent { get; set; }
        }
        public class responseModel
        {
            public double access_time { get; set; }
            public int code { get; set; }
            public string status { get; set; }
            public string encryptionBody { get; set; }
            public string message { get; set; }
            public string qr_image { get; set; }

        }

        public class QRModel
        {
            public double timestamp { get; set; }
            /// <summary>
            /// 二维码用途
            /// </summary>
            public string title { get; set; }
            /// <summary>
            /// 二维码描述
            /// </summary>
            public string description { get; set; }
            /// <summary>
            /// 文本
            /// </summary>
            public string text { get; set; }
            /// <summary>
            /// text，image,voice,video,nusic,news
            /// </summary>
            public string mediaType { get; set; }
            /// <summary>
            /// 选值为0或者1,0表示永久二维码,1表示临时二维码
            /// </summary>
            public int type { get; set; }
            /// <summary>
            /// 医生编码
            /// </summary>
            public string drCode { get; set; }
            /// <summary>
            /// 活动编码
            /// </summary>
            public string eventCode { get; set; }
            /// <summary>
            /// 该临时二维码有效时间，以秒为单位。 最大不超过2592000（即30天），此字段如果不填，则默认有效期为30秒。
            /// </summary>
            public int expireSeconds { get; set; }
            /// <summary>
            /// 图文消息的标题
            /// </summary>
            public string newsTitle { get; set; }
            /// <summary>
            /// 图文消息的描述
            /// </summary>
            public string newsDesc { get; set; }
            /// <summary>
            /// 图文消息封包图片
            /// </summary>
            public string picUrl { get; set; }
            /// <summary>
            /// 点击图文消息之后跳转的链接
            /// </summary>
            public string url { get; set; }
            /// <summary>
            /// 参数值为：none（不需要权限） login（需要登录）, certification（需要实名认证）
            /// </summary>
            public string authorityCode { get; set; }
        }
        
        public class UserRegisterModel
        {
            public double timestamp { get; set; }
            public string eventMark { get; set; }
            public string openid { get; set; }
            public string mobile { get; set; }
            public string smsCode { get; set; }
            public string nickname { get; set; }
            public int gender { get; set; }
            public string province { get; set; }
            public string city { get; set; }
            public string hospital { get; set; }
            public string department { get; set; }
            public string email { get; set; }
            public string professionalTitle { get; set; }
        }

    }
}