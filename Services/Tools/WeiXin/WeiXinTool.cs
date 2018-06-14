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

namespace Services.Tools.WeiXin
{
    /// <summary>
    /// 微信操作类，WeiXinTool.getWeiXinTool(),获取实例
    /// </summary>
    public class WeiXinTool
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="AppID"></param>
        /// <param name="accessToken"></param>
        /// <param name="obj"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static int? sendTemplateMessage(string AppID, string accessToken, object obj)
        {
            BaseModel templateMessageModel = new BaseModel();
            string res = "";
            try
            {
                res = HttpWebResponseUtility.CreatePostDataResponse(Const.template_message + accessToken, JsonConvert.SerializeObject(obj));
                log.Info(res);
                templateMessageModel = JsonConvert.DeserializeObject<BaseModel>(res);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            return templateMessageModel.msgid;
        }
        public static void SendCustomMessage(object message, string accessToken)
        {
            string res = HttpWebResponseUtility.CreatePostDataResponse(Const.custom_message + accessToken, JsonConvert.SerializeObject(message));
            log.Info("发送客服消息返回：" + res);
        }
        public class BaseModel
        {
            public int? errcode { get; set; }
            public string errmsg { get; set; }
            public int? msgid { get; set; }
        }
    }
}