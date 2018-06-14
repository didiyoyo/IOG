using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiXinTools
{

    public class MenuModal
    {
        public List<Button> button { get; set; }
    }

    public class Button
    {
        public string name { get; set; }
        public List<Button> sub_button { get; set; }
        public string type { get; set; }
        public string key { get; set; }
        public string media_id { get; set; }
        public string url { get; set; }
    }
}