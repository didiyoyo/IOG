using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Services.Tools
{
    public class MailTools
    {
        private SmtpClient smtp = new SmtpClient();
        /// <summary>
        /// 邮件地址
        /// </summary>
        public string Host { get; set; }
        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// 是否需要验证身份
        /// </summary>
        public bool UseDefaultCredentials { get; set; }
        /// <summary>
        /// 是否允许ssl
        /// </summary>
        public bool EnableSsl { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string userName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string passWord { get; set; }
        /// <summary>
        /// 发件人地址
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// 发件人昵称
        /// </summary>
        public string displayName { get; set; }
        /// <summary>
        /// 邮件回复地址
        /// </summary>
        public string ReplyToAddress { get; set; }
        /// <summary>
        /// 邮件回复时昵称
        /// </summary>
        public string ReplyToDisplayName { get; set; }
        /// <summary>
        /// 收件人，多个用","分隔
        /// </summary>
        public string SendAddresses { get; set; }
        /// <summary>
        /// 邮件主题
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// 正文是否是HTML
        /// </summary>
        public bool IsBodyHtml { get; set; }
        /// <summary>
        /// 邮件正文
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// 抄送者，多个用","分隔
        /// </summary>
        public string CC { get; set; }
        /// <summary>
        /// 密送者，多个用","分隔
        /// </summary>
        public string Bcc { get; set; }
        /// <summary>
        /// 用|分隔
        /// </summary>
        public string AttachmentsFileNames { get; set; }
        public Dictionary <string,Stream> AttachmentsStreams { get; set; }
        public MailTools() {
            this.Port = 25;
            this.UseDefaultCredentials = true;
            this.EnableSsl = true;
            this.IsBodyHtml = false;
        }
        /// <summary>
        /// 邮件类
        /// </summary>
        /// <param name="Host">服务器地址</param>
        /// <param name="userName">用户名</param>
        /// <param name="passWord">密码</param>
        /// <param name="SendAddresses">收件人</param>
        /// <param name="Subject">主题</param>
        /// <param name="Body">内容</param>
        /// <param name="AttachmentsFileNames">附件</param>
        public MailTools(string Host, string userName, string passWord, string SendAddresses, string Subject, string Body, string AttachmentsFileNames, Dictionary<string, Stream> AttachmentsStreams = null) 
        {
            this.Port = 25;
            this.UseDefaultCredentials = true;
            this.EnableSsl = true;
            this.IsBodyHtml = false;

            this.Host=Host;
            this.userName=userName;
            this.passWord=passWord;
            this.SendAddresses=SendAddresses;
            this.Subject=Subject;
            this.Body = Body;
            this.AttachmentsFileNames = AttachmentsFileNames;
            this.AttachmentsStreams = AttachmentsStreams;
        }
        /// <summary>
        /// 发送邮件
        /// </summary>
        public void Send()
        {
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network; 
            smtp.Host = this.Host;
            smtp.Port = this.Port;
            smtp.UseDefaultCredentials = this.UseDefaultCredentials;
            smtp.EnableSsl = this.EnableSsl;
            smtp.Credentials = new NetworkCredential(this.userName,this.passWord);
            MailMessage mm = new MailMessage();
            mm.Priority = MailPriority.High;
            if (!string.IsNullOrEmpty(this.address))
            {
                mm.From = new MailAddress(this.address, this.displayName, Encoding.UTF8);
            }
            else
            {
                mm.From = new MailAddress(this.userName, this.userName, Encoding.UTF8);
            }
            if (!string.IsNullOrEmpty(this.ReplyToAddress))
            {
                mm.ReplyTo = new MailAddress(this.ReplyToAddress, this.ReplyToDisplayName, Encoding.UTF8);
            }
            if (!string.IsNullOrEmpty(CC))
            {
                mm.CC.Add(CC);
            }
            if (!string.IsNullOrEmpty(Bcc))
            {
                mm.Bcc.Add(Bcc);
            }
            mm.To.Add(this.SendAddresses);
            mm.Subject = Subject; 
            mm.SubjectEncoding = Encoding.UTF8;
            mm.IsBodyHtml = this.IsBodyHtml;
            mm.BodyEncoding = Encoding.UTF8;
            mm.Body = this.Body;
            if (!string.IsNullOrEmpty(this.AttachmentsFileNames))
            {
                foreach (string fileName in this.AttachmentsFileNames.Split('|'))
                {
                    if (File.Exists(fileName))
                    {
                        mm.Attachments.Add(new Attachment(fileName));
                    }
                }
            }
            if (this.AttachmentsStreams != null)
            {
                foreach (var item in this.AttachmentsStreams)
                {
                    if (item.Value != null)
                    {
                        mm.Attachments.Add(new Attachment(item.Value, item.Key));
                    }
                }
            }
            smtp.Send(mm); 
        }
    }
}
