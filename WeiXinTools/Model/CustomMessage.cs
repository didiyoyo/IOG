using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiXinTools
{
    public class CustomMessage
    {
        public string touser { get; set; }
        /// <summary>
        /// text,image,voice,video，music，news，mpnews
        /// </summary>
        public string msgtype { get; set; }
        public News news { get; set; }
        public Mpnews mpnews { get; set; }
        public Music music { get; set; }
        public Video video { get; set; }

        public Mpnews voice { get; set; }

        public Mpnews image { get; set; }
        public Text text { get; set; }
    }
    public class Text{
        public string content { get; set; }
    }
    public class News
    {
        public Article[] articles { get; set; }
    }

    public class Article
    {
        public string title { get; set; }
        public string description { get; set; }
        public string url { get; set; }
        public string picurl { get; set; }
    }

    public class Mpnews
    {
        public string media_id { get; set; }
    }

    public class Music
    {
        public string title { get; set; }
        public string description { get; set; }
        public string musicurl { get; set; }
        public string hqmusicurl { get; set; }
        public string thumb_media_id { get; set; }
    }
    
public class Video
{
public string media_id { get; set; }
public string thumb_media_id { get; set; }
public string title { get; set; }
public string description { get; set; }
}

}