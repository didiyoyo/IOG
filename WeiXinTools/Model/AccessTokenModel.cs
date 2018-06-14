using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiXinTools.Model
{
    public class AccessTokenModel:BaseModel
    {
        public string access_token { get; set; }
        public string expires_in { get; set; }
    }
}
