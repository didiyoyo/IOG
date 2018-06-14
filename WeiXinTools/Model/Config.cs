using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiXinTools
{
    /// <summary>
    /// 配置文件
    /// </summary>
    public class Config
    {
        public string AppID { get; set; }
        public string AppSecret { get; set; }
        public string Token { get; set; }
        public string accessToken { get; set; }
        public DateTime accessTokenDate { get; set; }
        public string EncodingAESKey { get; set; }
        public string EncryptionType { get; set; }
    }
}
