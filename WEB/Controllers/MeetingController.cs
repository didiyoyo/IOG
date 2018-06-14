using Aspose.Slides;
using Services;
using Services.Enums;
using Services.Models;
using Services.Service;
using Services.Tools;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using WEB.Filters;
using WEB.Model;

namespace WEB.Controllers
{
    [PermissionsAttribute(true)]
    public class MeetingController : Controller
    {
        //
        // GET: /Meeting/
        mdSeminarMeetingMainService smm = new mdSeminarMeetingMainService();
        tdSeminarMeetingAcceptService sma = new tdSeminarMeetingAcceptService();
        MeetingDetailService mds = new MeetingDetailService();
        SurveyService ss = new SurveyService();
        VoteService vs = new VoteService();
        DoctorScheduleService ds = new DoctorScheduleService();
        public ActionResult Index(int id=1)
        {
            try
            {
                string url = System.Configuration.ConfigurationManager.ConnectionStrings["url"].ConnectionString;
                ViewBag.url = url;
                ViewBag.index = 0;
                var OpenID = Session["openid"].ToString();
                List<md_seminar_meeting_main> list = smm.GetMeetingByOpenID(OpenID);
                DateTime start = DateTime.Now;
                ViewBag.NOTOpen = list.Where(x => x.mendtime > start).Select(x => new DoctorScheduleInfo { IsDisplayDoctorSchedule = ds.IsExistDoctorSchedule(x, OpenID), Meeting = x }).ToList();
                ViewBag.Open = list.Where(x => x.mendtime <= start).Select(x => new DoctorScheduleInfo { IsDisplayDoctorSchedule = ds.IsExistDoctorSchedule(x, OpenID), Meeting = x }).ToList();
                ViewBag.tag = id==1?1:2;
                return View();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult MeetingInfo(int id)
        {
            string openid = Session["openid"].ToString();
            var meeting = smm.GetMeetingById(id);
            bool IsData = mds.GetIsData(id);
            var user = new UserInfoService().SelectByOpenid(openid);

            using (var context=new DBContext())
            {
                if (meeting.mhyrc_type == "2")
                {
                    var meetingScheduleList = context.agenda.Where(ms => ms.mid == id).OrderBy(m => m.date).ToList();
                    var meetingScheduleInfo = meetingScheduleList.GroupBy(m => new { date = m.date }, (Station, StationGroup) => new MeetingScheduleInfo
                    {
                        MarkTime = Station.date.Value,
                        AgendaList = StationGroup.ToList()
                    }).ToList();
                    ViewBag.MeetingScheduleInfo = meetingScheduleInfo;
                }
            }
            
            if (user != null)
            {
                if (meeting.liveURL != null)
                {
                    var doc = new UserInfoService().GetDocInfo(openid);
                    if (meeting.liveURL.IndexOf('?') < 0)
                    {
                        if (doc != null)
                        {
                            meeting.liveURL = meeting.liveURL + "?username=" + doc.doc_code + "&realName=" + doc.doc_name + "&mobile=" + doc.doc_code + "&mid=" + meeting.mcode;
                        }
                        else
                        {
                            if (user.doctorCode != null)
                            {
                                meeting.liveURL = meeting.liveURL + "?username=" + user.doctorCode + "&realName=" + user.nickname + "&mobile=" + user.doctorCode + "&mid=" + meeting.mcode;
                            }
                            else
                            {
                                meeting.liveURL = meeting.liveURL + "?username=" + user.openid + "&realName=" + user.nickname + "&mobile=" + user.openid + "&mid=" + meeting.mcode;
                            }
                        }

                    }
                    else
                    {
                        if (doc != null)
                        {
                            meeting.liveURL = meeting.liveURL + "&username=" + doc.doc_code + "&realName=" + doc.doc_name + "&mobile=" + doc.doc_code + "&mid=" + meeting.mcode;
                        }
                        else
                        {
                            if (user.doctorCode != null)
                            {
                                meeting.liveURL = meeting.liveURL + "&username=" + user.doctorCode + "&realName=" + user.nickname + "&mobile=" + user.doctorCode + "&mid=" + meeting.mcode;
                            }
                            else
                            {
                                meeting.liveURL = meeting.liveURL + "&username=" + user.openid + "&realName=" + user.nickname + "&mobile=" + user.openid + "&mid=" + meeting.mcode;

                            }
                        }

                    }
                }
            }
            else
            {
                meeting.liveURL = meeting.liveURL + "?username=&realName=&mobile=&mid=";
            }
            @ViewBag.IsData = IsData.ToString().ToLower();
            if (meeting.meetingmode == (int)MeetingModeTypeEnum.LargeUnderLine && meeting.mhyrc_type == "2")
            {
                return View("~/Views/Meeting/MeetingInfoToLargeUnderLine.cshtml", meeting);
            }
            else
            {
                return View(meeting);
            }
        }

        public ActionResult MeetingSchedule(int id)
        {
            ViewBag.mid = id;
            var meeting = smm.GetMeetingById(id);
            UserInfoService userInfoService = new UserInfoService();
            var userInfo = userInfoService.SelectByOpenid(Session["openid"].ToString());
            ViewBag.Userinfo = userInfo;
            using (var context = new DBContext())
            {
                if (meeting.mhyrc_type == "2")
                {
                    var meetingScheduleList = context.agenda.Where(ms => ms.mid == id).OrderBy(m => m.date).ToList();
                    var meetingScheduleInfo = meetingScheduleList.GroupBy(m => new { date = m.date }, (Station, StationGroup) => new MeetingScheduleInfo
                    {
                        MarkTime = Station.date.Value,
                        AgendaList = StationGroup.ToList()
                    }).ToList();
                    ViewBag.MeetingScheduleInfo = meetingScheduleInfo;
                }
                var workId = context.sfe_register.FirstOrDefault(r => r.doc_code == userInfo.doctorCode)?.NETWORK_EXTERNAL_ID_BMS_CN__C;
                var tableInputList = context.table_input.Where(t => t.mcode == meeting.mcode && t.networkid == workId).ToList();
                ViewBag.IsImport = tableInputList.Where(t => t.ModelType == "邀请文件").Count() > 0;
                if (tableInputList.Count() > 0 && tableInputList[0].IsInvitation != "是")
                {
                    foreach (var tableInput in tableInputList)
                    {
                        tableInput.IsInvitation = "是";
                    }
                    context.SaveChanges();
                }

                if (meeting.meetingmode == (int)MeetingModeTypeEnum.LargeUnderLine)
                {
                    var openid = Session["openid"].ToString();
                    var accept = context.td_seminar_meeting_accept.FirstOrDefault(a => a.OPenID == openid && a.MId == meeting.mid);
                    ViewBag.IsAccept = accept != null;
                }
            }
            if (meeting.meetingmode == (int)MeetingModeTypeEnum.LargeUnderLine && meeting.mhyrc_type == "2")
            {
                return View("~/Views/Meeting/MeetingScheduleToLargeUnderLine.cshtml", meeting);
            }
            else
            {
                return View(meeting);
            }
        }
        public ActionResult MeetingCode(int id)
        {
            var meeting = smm.GetMeetingById(id);
            return View(meeting);
        }

        #region 医生行程 20180428吴迪
        /// <summary>
        /// 20180428WD
        /// 通过会议Id和用户Id查询行程信息
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        public ActionResult DoctorSchedule(int mid)
        {
            var meeting = smm.GetMeetingById(mid);
            UserInfoService userInfoService = new UserInfoService();
            var userInfo = userInfoService.SelectByOpenid(Session["openid"].ToString());
            using (var context=new DBContext())
            {
                var workId = context.sfe_register.FirstOrDefault(r => r.doc_code == userInfo.doctorCode)?.NETWORK_EXTERNAL_ID_BMS_CN__C;
                var tableInputList = context.table_input.Where(t => t.mcode == meeting.mcode && t.networkid == workId).ToList();
                if (tableInputList.Count() > 0 && tableInputList[0].browse != "是")
                {
                    foreach (var tableInput in tableInputList)
                    {
                        tableInput.browse = "是";
                    }
                    context.SaveChanges();
                }
            }
            var doctorSchedule = ds.GetDoctorSchedule(mid, Session["openid"].ToString());
            ViewBag.UserInfo = userInfo;
            ViewBag.Meeting = meeting;
            ViewBag.DoctorSchedule = doctorSchedule;
            return View();
        }
        #endregion

        #region 邀请函---会议日程

        public ActionResult Invitation(int id)
        {
            //var OpenID = Session["openid"].ToString();
            if (Session["openid"] == null)
            {
                return Json(new { success = false, msg = "参数错误" }, JsonRequestBehavior.AllowGet);
            } 
            //var user = new UserInfoService().SelectByOpenid(OpenID);
            //var docname = "";
            //if (user != null)
            //{
            //    var doc = new UserInfoService().GetDocInfo(OpenID);
            //    if (doc != null) 
            //        docname = doc.doc_name; 
            //    else 
            //        docname = user.nickname; 
            //}
            //@ViewBag.Doc_Name = docname;
            var meeting = smm.GetMeetingById(id);
            return View(meeting);
        }

        public ActionResult doctorInfo(int id)
        {
            var meeting = smm.GetMeetingById(id);
            UserInfoService userInfoService = new UserInfoService();
            var userInfo = userInfoService.SelectByOpenid(Session["openid"].ToString());
            return View(meeting);
        }

        public JsonResult GetDoctorInfo(int id)
        {
            var meeting = smm.GetMeetingById(id);
            if (Convert.ToBoolean(System.Web.Configuration.WebConfigurationManager.ConnectionStrings["IsTestFilePreView"].ToString()))
            {
                return Json(System.Web.Configuration.WebConfigurationManager.ConnectionStrings["FilePreViewTest"] + meeting.doctorimg);
            }
            else
            {
                return Json(System.Web.Configuration.WebConfigurationManager.ConnectionStrings["FilePreView"] + meeting.doctorimg);
            }
            //return Json(System.Web.Configuration.WebConfigurationManager.ConnectionStrings["DataPath"] + meeting.doctorimg);
            //UserInfoService userInfoService = new UserInfoService();
            //var userInfo = userInfoService.SelectByOpenid(Session["openid"].ToString());

            //if (meeting.doctortype == 3)
            //{
            //    return Json(System.Web.Configuration.WebConfigurationManager.ConnectionStrings["DataPath"] + meeting.doctorimg);
            //}
            //if (meeting.doctortype == 4 || meeting.doctortype == 5)
            //{
            //    string path = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["DataPath"] + meeting.doctorimg;
            //    string extension = meeting.doctorimg.Split('.')[1];
            //    var fileName= ReadRemoteFile(path, userInfo.doctorCode, extension);
            //    string route = Request.Url.Scheme + "://" + Request.Url.Host + "/IO/Files/" + userInfo.doctorCode + "/" + fileName + ".pdf";
            //    switch (meeting.doctortype)
            //    {
            //        case 4:
            //            Aspose.Words.Document doc = new Aspose.Words.Document(Server.MapPath("~/Files/" + userInfo.doctorCode + "/" + fileName + "." + extension));
            //            doc.Save(Server.MapPath("~/Files/") + userInfo.doctorCode + "/" + fileName + ".pdf", Aspose.Words.SaveFormat.Pdf);
            //            //ViewBag.FileRoute= Request.Url.Scheme + "://" + Request.Url.Host + "/IO/Files/" + userInfo.doctorCode + "/" + userInfo.doctorCode + ".pdf";
            //            //Response.ContentType = "application/pdf";
            //            //Response.Redirect(route);
            //            break;
            //        case 5:
            //            Presentation pres = new Presentation(Server.MapPath("~/Files/" + userInfo.doctorCode + "/" + fileName + "." + extension));
            //            pres.Save(Server.MapPath("~/Files/") + userInfo.doctorCode + "/" + fileName + ".pdf", Aspose.Slides.Export.SaveFormat.Pdf);
            //            //ViewBag.FileRoute = Request.Url.Scheme + "://" + Request.Url.Host  + "/IO/Files/" + userInfo.doctorCode + "/" + userInfo.doctorCode + ".pdf";
            //            //Response.Redirect(route);
            //            break;
            //        default:
            //            break;
            //    }
            //    return Json(route);
            //}
            //else
            //{
            //    return null;
            //}
        }

        //private string ReadRemoteFile(string c,string doctorCode,string extension)
        //{
        //    //if (!Directory.Exists(Server.MapPath("~/Files/") + doctorCode))
        //    //{
        //    //    Directory.CreateDirectory(Server.MapPath("~/Files/") + doctorCode);
        //    //}
        //    //DirectoryInfo dir = new DirectoryInfo(Server.MapPath("~/Files/") + doctorCode);
        //    //FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //返回目录中所有文件和子目录
        //    //foreach (FileSystemInfo i in fileinfo)
        //    //{
        //    //    if (i is DirectoryInfo)            //判断是否文件夹
        //    //    {
        //    //        DirectoryInfo subdir = new DirectoryInfo(i.FullName);
        //    //        subdir.Delete(true);          //删除子目录和文件
        //    //    }
        //    //    else
        //    //    {
        //    //        System.IO.File.Delete(i.FullName);      //删除指定文件
        //    //    }
        //    //}

        //    //HttpWebRequest req = WebRequest.Create(path) as HttpWebRequest;
        //    //req.Accept = @"text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
        //    ////req.Referer = oldurl;
        //    //req.UserAgent = @" Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/33.0.1750.154 Safari/537.36";
        //    //req.ContentType = "application/octet-stream";
        //    //HttpWebResponse response = req.GetResponse() as HttpWebResponse;
        //    //Stream stream = response.GetResponseStream();
        //    //string fileName = Guid.NewGuid().ToString();
        //    //FileStream fs = new FileStream(Server.MapPath("~/Files/"+doctorCode+"/"+ fileName + "."+ extension), FileMode.Create);
        //    //try
        //    //{
        //    //    long length = response.ContentLength;
        //    //    int i = 0;
        //    //    do
        //    //    {
        //    //        byte[] buffer = new byte[1024];
        //    //        i = stream.Read(buffer, 0, 1024);
        //    //        fs.Write(buffer, 0, i);
        //    //    } while (i > 0);
        //    //}
        //    //finally
        //    //{
        //    //    fs.Close();
        //    //}
        //    //return fileName;
        //}

        public ActionResult JoinMeeting(td_seminar_meeting_accept _sma)
        {
            
            try
            {
                if (Session["openid"] == null)
                {
                    return Json(new { success = false, msg = "参数错误" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    mdSeminarMeetingMainService service = new mdSeminarMeetingMainService();
                    var meeting = service.GetMeetingById(_sma.MId);
                    UserInfoService userInfoService = new UserInfoService();
                    var userInfo = userInfoService.SelectByOpenid(Session["openid"].ToString());
                    _sma.OPenID = Session["openid"].ToString();
                    _sma.CreateOn = DateTime.Now;
                    var i = sma.Insert(_sma);
                    if(i>0)
                    {
                        using (var context =new DBContext())
                        {
                            var workId = context.sfe_register.FirstOrDefault(r => r.doc_code == userInfo.doctorCode)?.NETWORK_EXTERNAL_ID_BMS_CN__C;
                            var tableInputList = context.table_input.Where(t => t.mcode == meeting.mcode && t.networkid == workId).ToList();
                            if (tableInputList.Count() > 0)
                            {
                                foreach (var tableInput in tableInputList)
                                {
                                    tableInput.IsAppointments = "是";
                                    tableInput.meetingtime = DateTime.Now;
                                    tableInput.participation = "微信确认";
                                }
                                context.SaveChanges();
                            }
                        }
                        return Json(new { success =true,msg= "预约成功，可以从“我的会议”查看。页面跳转中，请稍等" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        string msg = "会议已过期。页面跳转中，请稍等";
                        if(i==-1)
                        {
                            msg = "您已预约成功，请勿重复点击。页面跳转中，请稍等";
                        }
                        return Json(new { success = false, msg = msg }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
#endregion

        /// <summary>
        /// 会议互动
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Interaction(int id)
        {
            var meeting = smm.GetMeetingById(id);
            bool checkvote = false;
            bool checksurvey = false;
            if (meeting.vote)
            {
                checkvote = vs.CheckVote(id);
            }
            if (meeting.survey)
            {
                checksurvey = ss.CheckSurvey(id);
            }
            @ViewBag.checkvote = checkvote;
            @ViewBag.checksurvey = checksurvey;
            return View(meeting);
        }

        public ActionResult DataList(int id)
        {
            string hosturl = System.Configuration.ConfigurationManager.ConnectionStrings["url"].ConnectionString;
            ViewBag.hosturl = hosturl;
            var OpenID = Session["openid"].ToString();
            var user = new UserInfoService().SelectByOpenid(OpenID);
            var meeting = smm.GetMeetingById(id);
            var list = mds.GetMeetingDataByMid(id);
            ViewBag.mTitle = meeting.mtitle;
            ViewBag.mid = id;
            ViewBag.Email = user.email;
            return View(list);
        }
        
        /// <summary>
        /// 获取接受函
        /// </summary>
        /// <returns></returns>
        public ViewResult GetLetterOfAcceptance()
        {
            using (var context=new DBContext())
            {
                var letterStr = context.file_information.FirstOrDefault(f => f.filenumber == "LetterOfAcceptance").content;
                var letterInfo = new LetterOfAcceptanceModel
                {
                    Title = letterStr.Split('&')[0],
                    Content = letterStr.Split('&')[1]
                };
                ViewBag.letterInfo = letterInfo;
            }
                return View("~/Views/Meeting/LetterOfAcceptance.cshtml");
        }

        //public ActionResult SendEmail(int mid, string Name,string MeetingName,string Title, string toUserEmail, string EmailHtml)
        //{

        //    byte[] outputb = Convert.FromBase64String(EmailHtml);
        //    EmailHtml = Encoding.UTF8.GetString(outputb);

        //    var OpenID = Session["openid"].ToString();
        //    var user = new UserInfoService().SelectByOpenid(OpenID);
        //    var docname = "";
        //    if (user != null) {
        //        var doc = new UserInfoService().GetDocInfo(OpenID);
        //        if (doc != null)
        //        {
        //            docname = doc.doc_name;
        //        }
        //        else
        //        {
        //            docname = user.nickname; 
        //        }
        //    }

        //    try
        //    {
        //        string path = Server.MapPath("~/") + "cookie.dat";
        //        string cookie = "";
        //        string name = "PubAuth1";
        //        FileInfo fi = new FileInfo(path);
        //        if (!System.IO.File.Exists(path) || fi.LastWriteTime.AddHours(7) < DateTime.Now)
        //        {
        //            //登陆
        //            string url = "https://ebm.cheetahmail.com/api/login1?name=bms@api&cleartext=bms@05)(";
        //            string cookieHeader = "";
        //            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
        //            myHttpWebRequest.CookieContainer = new CookieContainer();
        //            myHttpWebRequest.CookieContainer.SetCookies(new Uri(url), cookieHeader);
        //            HttpWebResponse myresponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
        //            string cookies = myresponse.Headers["Set-Cookie"];//获取验证码页面的Cookies
        //            cookie = GetCookieValue(cookies);
        //            //保存cookie到本地
        //            FileStream fs = new FileStream(path, FileMode.OpenOrCreate);
        //            //获得字节数组
        //            byte[] data = System.Text.Encoding.Default.GetBytes(cookie);
        //            //开始写入
        //            fs.Write(data, 0, data.Length);
        //            //清空缓冲区、关闭流
        //            fs.Flush();
        //            fs.Close();
        //            fi.LastWriteTime = DateTime.Now;
        //        }
        //        else
        //        {
        //            //读取本地cookie
        //            using (FileStream fsRead = new FileStream(path, FileMode.Open))
        //            {
        //                int fsLen = (int)fsRead.Length;
        //                byte[] heByte = new byte[fsLen];
        //                int r = fsRead.Read(heByte, 0, heByte.Length);
        //                string myStr = System.Text.Encoding.UTF8.GetString(heByte);
        //                cookie = myStr;
        //            }
        //        }
        //        //POST
        //        //CookieCollection cc = new CookieCollection();
        //        //Cookie c = new Cookie(name, cookie, "/", "ebm.cheetahmail.com");
        //        //cc.Add(c);
        //        //Dictionary<string, string> dt = new Dictionary<string, string>();
        //        //dt.Add("email", toUserEmail);
        //        //dt.Add("eid", "305459");
        //        //dt.Add("MEETING_TITLE", Title);
        //        //dt.Add("MEETING_NAME", MeetingName);
        //        //dt.Add("DOC_NAME", docname);
        //        //dt.Add("DATA", EmailHtml);
        //        //string temp = HttpWebResponseUtility.CreatePostHttpResponse("https://ebm.cheetahmail.com/api/ebmtrigger1", dt, 6000, "", System.Text.Encoding.UTF8, cc);


        //        string temp = SendEmailAPI(name, cookie, toUserEmail, "305459", Title, MeetingName, docname,
        //         EmailHtml);
        //        if(temp.ToUpper().Contains("OK")){
        //            SendEmail se = new Services.Models.SendEmail()
        //            {
        //                createTime = DateTime.Now,
        //                name = Name,
        //                mid = mid,
        //                openid = OpenID,
        //                status = "true",
        //                ToEmaill = toUserEmail
        //            };
        //            mds.SendEmail(se);
        //            return Json(new { success = true, msg = "发送成功" }, JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            SendEmail se = new Services.Models.SendEmail()
        //            {
        //                createTime = DateTime.Now,
        //                name = Name,
        //                mid = mid,
        //                openid = OpenID,
        //                status = "false",
        //                ToEmaill = toUserEmail
        //            };
        //            mds.SendEmail(se);
        //            return Json(new { success = false, msg = "发送失败" }, JsonRequestBehavior.AllowGet);
        //        }


        //    }
        //    catch (Exception e)
        //    {
        //        SendEmail se = new Services.Models.SendEmail()
        //        {
        //            createTime = DateTime.Now,
        //            name = Name,
        //            mid = mid,
        //            openid = OpenID,
        //            status = "false",
        //            ToEmaill = toUserEmail
        //        };
        //        mds.SendEmail(se);
        //        return Json(new { success = false, msg = "发送失败"}, JsonRequestBehavior.AllowGet);
        //        throw;
        //    }

        //}

        //private string GetCookieValue(string cookie)
        //{
        //    Regex regex = new Regex("=.*?;");
        //    Match value = regex.Match(cookie);
        //    string cookieValue = value.Groups[0].Value;
        //    return cookieValue.Substring(1, cookieValue.Length - 2);
        //}

        //private string GetCookieName(string cookie)
        //{
        //    Regex regex = new Regex("sulcmiswebpac.*?");
        //    Match value = regex.Match(cookie);
        //    return value.Groups[0].Value;
        //}

        //private string SendEmailAPI(string name, string cookie, string email, string eid, string title, string meetingName,string DocName, string data)
        //{
        //    //string param = "{email:'" + email + "',eid:" + eid + ",title:'" + title + "',}";
        //    //byte[] byteArray = Encoding.UTF8.GetBytes("json=" + param);
        //    string url = string.Format(@"https://ebm.cheetahmail.com/api/ebmtrigger1?email={0}&eid={1}&MEETING_TITLE={2}&MEETING_NAME={3}&DATA={4}", email, eid, title, meetingName, data);
        //    // "https://ebm.cheetahmail.com/ebm/ebmtrigger1?eid=305459&email="+email+"&MEETING_TITLE="+title+"&MEETING_NAME="+meetingName+"&DATA=" + data
        //    CookieCollection cookies = new CookieCollection();
        //    cookies.Add(new Cookie(name, cookie));
        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://ebm.cheetahmail.com/ebm/ebmtrigger1?eid=305459&email=" + email + "&SUBJECT_LINE=" + title + "&MEETING_NAME=" + meetingName + "&DOC_NAME=" + DocName + "&DATA=" + data + "");
        //    request.Method = "GET";
        //    request.Headers.Add("Cookie", name + "=" + cookie);
        //    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        //    Stream stream = response.GetResponseStream();
        //    StreamReader reader = new StreamReader(stream, Encoding.UTF8);
        //    return reader.ReadToEnd();
        //}
        public ActionResult SendEmail(int mid, string Name, string MeetingName, string Title, string toUserEmail, string EmailHtml)
        {
            byte[] outputb = Convert.FromBase64String(EmailHtml);
            EmailHtml = Encoding.UTF8.GetString(outputb);

            var OpenID = Session["openid"].ToString();
            var user = new UserInfoService().SelectByOpenid(OpenID);
            var docname = "";
            if (user != null)
            {
                var doc = new UserInfoService().GetDocInfo(OpenID);
                if (doc != null)
                {
                    docname = doc.doc_name;
                }
                else
                {
                    docname = user.nickname;
                }
            }
            bool result = SendEmailAPI(toUserEmail, Title, MeetingName, docname, EmailHtml);
            if (result)
            {
                SendEmail se = new Services.Models.SendEmail()
                {
                    createTime = DateTime.Now,
                    name = Name,
                    mid = mid,
                    openid = OpenID,
                    status = "true",
                    ToEmaill = toUserEmail
                };
                mds.SendEmail(se);
                return Json(new { success = true, msg = "发送成功" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                SendEmail se = new Services.Models.SendEmail()
                {
                    createTime = DateTime.Now,
                    name = Name,
                    mid = mid,
                    openid = OpenID,
                    status = "false",
                    ToEmaill = toUserEmail
                };
                mds.SendEmail(se);
                return Json(new { success = false, msg = "发送失败" }, JsonRequestBehavior.AllowGet);
            }
        }
        private bool SendEmailAPI( string email, string title, string meetingName, string DocName, string data)
        {
            string Body = "<img src=\"http://vmeeting.bms.com.cn/IO/Content/images/ioemail2.png\"/>" +
                "<p>尊敬的" + DocName + "医生：</p>" +
                "<p>    感谢您参加" + meetingName + "，该会议资料下载地址如下：</p>" +
                data +
                "<p>    此邮件的信息只发送给医药专业人员，不应转发给其他任何人。</p>" +
                "<p>    ©️2018,Bristol-Myers Squibb 保留所有权利。</p>";
            string Host = ConfigurationManager.ConnectionStrings["EmailHost"].ConnectionString;
            string userName = ConfigurationManager.ConnectionStrings["EmailUserName"].ConnectionString;
            string passWord = ConfigurationManager.ConnectionStrings["EmailPassWord"].ConnectionString;
            int Port = Convert.ToInt32(ConfigurationManager.ConnectionStrings["EmailPort"].ConnectionString);
            bool EnableSsl = Convert.ToBoolean(ConfigurationManager.ConnectionStrings["EmailEnableSsl"].ConnectionString);
            MailTools mailTools = new MailTools(Host,userName,passWord,email,title,Body,null,null)
            {
                Port=Port,
                EnableSsl= EnableSsl,
                IsBodyHtml=true
            };
            try
            {
                mailTools.Send();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

    }
}
