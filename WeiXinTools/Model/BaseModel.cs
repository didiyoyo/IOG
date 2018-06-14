using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiXinTools.Model
{
    /// <summary>
    /// 基础类，包含错误信息字段
    /// </summary>
    public class BaseModel
    {
        public int? errcode { get; set; }
        public string errmsg { get; set; }
    }
}
