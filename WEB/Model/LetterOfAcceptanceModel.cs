using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEB.Model
{
    /// <summary>
    /// 接受函文本信息
    /// </summary>
    public class LetterOfAcceptanceModel
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
    }
}