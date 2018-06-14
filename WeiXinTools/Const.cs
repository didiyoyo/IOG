using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiXinTools
{
    public static class Const
    {
        public const string access_token = "https://api.weixin.qq.com/cgi-bin/token";
        public const string menu_create = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token=";
        public const string menu_delete = "https://api.weixin.qq.com/cgi-bin/menu/delete?access_token=";
        public const string user_info = "https://api.weixin.qq.com/cgi-bin/user/info?access_token=";
        public const string access_token_code = "https://api.weixin.qq.com/sns/oauth2/access_token";
        public const string weixin_authorize = "https://open.weixin.qq.com/connect/oauth2/authorize";
        public const string custom_message = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=";
        public const string web_user_info = "https://api.weixin.qq.com/sns/userinfo";
        public const string qrcode = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=";
        public const string uploadmedia = "http://file.api.weixin.qq.com/cgi-bin/media/upload?access_token=";
        public const string template_message = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=";
    }
}
