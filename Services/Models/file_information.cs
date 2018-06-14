using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    public class file_information
    {
        public int id { get; set; }

        /// <summary>
        /// 文件编号
        /// </summary>
        public string filenumber { get; set; }

        /// <summary>
        /// 文件内容
        /// </summary>
        public string content { get; set; }
    }
}
