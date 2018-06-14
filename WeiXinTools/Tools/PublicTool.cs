using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WeiXinTools.Model;

namespace WeiXinTools
{
    public class PublicTool
    {
        public static bool CheckSignature(string signature, string timestamp, string nonce, string Token)
        {
            
            string[] ArrTmp = { Token, timestamp, nonce };
            Array.Sort(ArrTmp);     //字典排序  
            string tmpStr = string.Join("", ArrTmp);
            tmpStr = SHA1(tmpStr);
            tmpStr = tmpStr.ToLower();
            if (tmpStr == signature)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static string SHA1(string text)
        {
            byte[] cleanBytes = Encoding.Default.GetBytes(text);
            byte[] hashedBytes = System.Security.Cryptography.SHA1.Create().ComputeHash(cleanBytes);
            return BitConverter.ToString(hashedBytes).Replace("-", "");
        }
        public static WeiXinMessage getMessage(string data)
        {
            WeiXinMessage wx = new WeiXinMessage();
            if (data != null)
            {
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(data);
                wx.ToUserName = xml.SelectSingleNode("xml").SelectSingleNode("ToUserName").InnerText;
                wx.FromUserName = xml.SelectSingleNode("xml").SelectSingleNode("FromUserName").InnerText;
                wx.MsgType = xml.SelectSingleNode("xml").SelectSingleNode("MsgType").InnerText;
                if (wx.MsgType.Trim() == "text")
                {
                    wx.Content = xml.SelectSingleNode("xml").SelectSingleNode("Content").InnerText;
                }
                if (wx.MsgType.Trim() == "event")
                {
                    wx.EventName = xml.SelectSingleNode("xml").SelectSingleNode("Event").InnerText;
                    wx.EventKey = xml.SelectSingleNode("xml").SelectSingleNode("EventKey").InnerText;
                }
                if (wx.MsgType.Trim() == "voice")
                {
                    wx.Recognition = xml.SelectSingleNode("xml").SelectSingleNode("Recognition").InnerText;
                }
                if (wx.MsgType.Trim() == "image")
                {
                    wx.MediaId = xml.SelectSingleNode("xml").SelectSingleNode("MediaId").InnerText;
                }
            }


            return wx;

        }
    }
}
