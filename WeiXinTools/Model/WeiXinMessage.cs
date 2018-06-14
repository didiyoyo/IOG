using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiXinTools.Model
{
    public class WeiXinMessage
    {
        /// <summary>
        /// 本公众账号
        /// </summary>
        public string ToUserName { get; set; }
        /// <summary>
        /// 用户账号
        /// </summary>
        public string FromUserName { get; set; }
        /// <summary>
        /// 发送时间戳
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 发送的文本内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 消息的类型
        /// </summary>
        public string MsgType { get; set; }
        /// <summary>
        /// 事件名称
        /// </summary>
        public string EventName { get; set; }
        public string Recognition { get; set; }
        public string MediaId { get; set; }
        public string EventKey { get; set; }
    }
}
