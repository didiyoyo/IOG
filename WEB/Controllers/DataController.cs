using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Services;
using Services.Models;
using Services.Service;
using Services.Thirdparty;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace WEB.Controllers
{
    public class DataController : Controller
    {
        //
        // GET: /Data/ 
        public ActionResult GetMeetingInfo()
        {
            try
            {
                DBContext db = new DBContext();
                string sql = string.Format(@"select smm.mcode as mid, smm.mtitle,smm.msite,smm.mbegintime,smm.mendtime,
                                                (
                                                SELECT COUNT(1) from (
                                                SELECT distinct OPenID,MId from click_log where modelName='查看邀请函'
                                                ) as c where c.mid=smm.mid group by mid 
                                                ) as InvitedDocCount ,
                                                (select count(1) from td_seminar_meeting_accept sma where sma.MId=smm.mid  group by sma.MId) as JoinDocCount ,
                                                (SELECT count(1) from click_log where modelName='会议详情' and mid=smm.mid group by mid) as MeetingInfoCount ,
                                                (SELECT count(1) from click_log where modelName='在线观看' and mid=smm.mid group by mid) as LiveCount ,
                                                (
                                                SELECT COUNT(1) from (
                                                SELECT distinct OPenID,MId from click_log where modelName='在线观看'
                                                ) as l where l.mid=smm.mid group by mid 
                                                ) as LiveByUserCount,
                                                ( 
                                                SELECT COUNT(1) from (
                                                select distinct uid,mid from md_seminar_vote  sv 
                                                RIGHT JOIN td_seminar_vote_result svr on sv.vid=svr.vid 
                                                ) as vote where mid=smm.mid GROUP BY mid
                                                ) as VoteCount,
                                                ( 
                                                SELECT COUNT(1) from (
                                                select distinct uid,mid from md_seminar_survey ss 
                                                RIGHT JOIN td_seminar_survey_result ssr on ss.sid=ssr.sid
                                                ) as survey where mid=smm.mid GROUP BY mid
                                                ) as SurveyCount,
                                                (
                                                SELECT COUNT(1) from (
                                                SELECT  distinct uid,mid from td_seminar_question 
                                                ) as question where question.mid=smm.mid  group by question.mid
                                                ) as QuestionCount,
                                                (
                                                SELECT COUNT(1) from (
                                                SELECT DISTINCT OPenID,mid from send_email_log 
                                                ) as sendemail where sendemail.mid=smm.mid group by sendemail.mid
                                                ) as SendEailCount
                                                from md_seminar_meeting_main smm");
                var dm = db.Database.SqlQuery<data_meetinginfo>(sql);
                IsoDateTimeConverter dtConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
                return Content(JsonConvert.SerializeObject(new { status = true, data = dm }, dtConverter)); 
            }
            catch (Exception e)
            {
                return Json(new { status = false, errmsg = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }


        public ActionResult GetDocData()
        {
            try
            {
                DBContext db = new DBContext();
                string sql = string.Format(@"SELECT max(du.area) as area,max(dsm_name) as dsm_name,max(du.user_code) as user_code,max(du.user_name) as user_name,COUNT(1) as DocCount,COUNT(doctorcode) as AcceptedDocCount from dsm_user du
                                            left JOIN sfe_register sr on du.user_code=sr.CYCLE_PLAN_VOD_R_TERRITORY_VOD__C
                                            left join userinfo u on sr.doc_code=u.doctorcode  and u.statuscode='Accepted'
                                            GROUP BY du.user_code");
                var dm = db.Database.SqlQuery<data_DocData>(sql);
                IsoDateTimeConverter dtConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" }; 
                return Content(JsonConvert.SerializeObject(new { status = true, data = dm }, dtConverter));
            }
            catch (Exception e)
            {
                return Json(new { status = false, errmsg = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }


        public ActionResult GetInvitedDocData()
        {
            try
            {
                DBContext db = new DBContext();
                string sql = string.Format(@"SELECT *,
                                            (SELECT COUNT(1) from sfe_register sr where sr.CYCLE_PLAN_VOD_R_TERRITORY_VOD__C= mduser.user_code) as DocCount,
                                            (
                                            SELECT COUNT(1) from (
                                            select distinct clog.openid,clog.mid from click_log clog
                                            JOIN userinfo u on clog.openid=u.openid and clog.modelname='查看邀请函'
                                            join sfe_register sr on sr.doc_code=u.doctorcode
                                            ) cl
                                            where cl.mid=mduser.mid
                                            ) as InvitedDocCount,
                                            (
                                            select COUNT(1) from td_seminar_meeting_accept sma
                                            JOIN userinfo u on sma.openid=u.openid
                                            join sfe_register sr on sr.doc_code=u.doctorcode 
                                            where sma.mid=mduser.mid
                                            ) as JoinDocCount
                                            from (
                                            select  smm.mcode as mid,smm.mtitle,smm.msite,smm.mbegintime,smm.mendtime,mus.user_code,dus.area,dus.dsm_name,dus.user_name
                                            from md_seminar_meeting_main smm
                                            join meeting_user mus on mus.mid=smm.mid
                                            join dsm_user dus on dus.user_code=mus.user_code
                                            ) as mduser");
                var dm = db.Database.SqlQuery<data_InvitedDocData>(sql);
                IsoDateTimeConverter dtConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
                return Content(JsonConvert.SerializeObject(new { status = true, data = dm }, dtConverter));
            }
            catch (Exception e)
            {
                return Json(new { status = false, errmsg = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }


        public ActionResult GetVoteInfo(int pageIndex ,int pageSize) 
        {
            try
            { 
                DBContext db = new DBContext();
                string num=string.Format(@"SELECT  COUNT(1)
                                            from td_seminar_vote_result vr 
                                            JOIN md_seminar_vote v on vr.vid=v.vid
                                            join md_seminar_meeting_main m on m.mid=v.mid
                                            join userinfo u on vr.uid=u.openid
                                            LEFT JOIN (SELECT distinct doc_name ,  doc_code from  sfe_register) sfe on sfe.doc_code=u.doctorCode ");
                string sql = string.Format(@"SELECT SQL_CALC_FOUND_ROWS  m.mcode as mid, m.mtitle,m.msite,m.mbegintime,m.mendtime,sfe.doc_code,sfe.doc_name,v.vid,
                                            v.vtopic as title,v.vanswer as answer,v.vtype as type,vr.vresult as result, vr.vdatetime as createTime
                                            from td_seminar_vote_result vr 
                                            JOIN md_seminar_vote v on vr.vid=v.vid
                                            join md_seminar_meeting_main m on m.mid=v.mid
                                            join userinfo u on vr.uid=u.openid
                                            LEFT JOIN (SELECT distinct doc_name ,  doc_code from  sfe_register) sfe on sfe.doc_code=u.doctorCode
                                            LIMIT {0},{1};
                                            SELECT FOUND_ROWS();", (pageIndex-1)*pageSize,pageSize);
                var count = db.Database.SqlQuery<int>(num).FirstOrDefault();
                var v = db.Database.SqlQuery<data_vote>(sql);
                IsoDateTimeConverter dtConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
                var pages=count%pageSize==0?count/pageSize:count/pageSize+1;
                return Content(JsonConvert.SerializeObject(new { status = true, data = v, totalPages = pages, totalItems = count }, dtConverter));
            }
            catch (Exception e)
            {
                 return Json(new { status = false, errmsg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult GetSurveyInfo(int pageIndex, int pageSize)
        {
            try
            {
                DBContext db = new DBContext();
                string num = string.Format(@"SELECT count(1) 
                                            from td_seminar_survey_result sr 
                                            JOIN md_seminar_survey s on sr.sid=s.sid
                                            join md_seminar_meeting_main m on m.mid=s.mid
                                            join userinfo u on sr.uid=u.openid
                                            LEFT JOIN (SELECT distinct doc_name ,  doc_code from  sfe_register) sfe on sfe.doc_code=u.doctorCode");
                string sql = string.Format(@"SELECT m.mcode as mid, m.mtitle,m.msite,m.mbegintime,m.mendtime,sfe.doc_code,sfe.doc_name,s.sid,
                                            s.stitle as title,s.sanswer as answer,s.stype as type,sr.sresult as result,sr.sdatatiem as createTime
                                            from td_seminar_survey_result sr 
                                            JOIN md_seminar_survey s on sr.sid=s.sid
                                            join md_seminar_meeting_main m on m.mid=s.mid
                                            join userinfo u on sr.uid=u.openid
                                            LEFT JOIN (SELECT distinct doc_name ,  doc_code from  sfe_register) sfe on sfe.doc_code=u.doctorCode
                                            ORDER BY m.mid
                                            LIMIT {0},{1};", (pageIndex - 1) * pageSize, pageSize);
                var count = db.Database.SqlQuery<int>(num).FirstOrDefault();
                var s = db.Database.SqlQuery<data_survey>(sql);
                IsoDateTimeConverter dtConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
                var pages = count % pageSize == 0 ? count / pageSize : count / pageSize + 1;
                return Content(JsonConvert.SerializeObject(new { status = true, data = s, totalPages = pages, totalItems = count }, dtConverter));
            }
            catch (Exception e)
            {
                return Json(new { status = false, errmsg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult GetQuestionInfo(int pageIndex, int pageSize)
        {
            try
            {
                DBContext db = new DBContext();
                string num = string.Format(@"SELECT count(1) 
                                            from td_seminar_question q
                                            join md_seminar_meeting_main m on m.mid=q.mid
                                            join userinfo u on q.uid=u.openid
                                            LEFT JOIN (SELECT distinct doc_name ,  doc_code from  sfe_register) sfe on sfe.doc_code=u.doctorCode");
                string sql = string.Format(@"SELECT m.mcode, m.mtitle,m.msite,m.mbegintime,m.mendtime,sfe.doc_code,sfe.doc_name, q.content as question, q.datetime as createTime
                                            from td_seminar_question q
                                            join md_seminar_meeting_main m on m.mid=q.mid
                                            join userinfo u on q.uid=u.openid
                                            LEFT JOIN (SELECT distinct doc_name ,  doc_code from  sfe_register) sfe on sfe.doc_code=u.doctorCode
                                            ORDER BY m.mid
                                            LIMIT {0},{1};", (pageIndex - 1) * pageSize, pageSize);
                var count = db.Database.SqlQuery<int>(num).FirstOrDefault();
                var s = db.Database.SqlQuery<data_question>(sql);
                IsoDateTimeConverter dtConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
                var pages = count % pageSize == 0 ? count / pageSize : count / pageSize + 1;
                return Content(JsonConvert.SerializeObject(new { status = true, data = s, totalPages = pages, totalItems = count }, dtConverter));
            }
            catch (Exception e)
            {
                return Json(new { status = false, errmsg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult GetLiveInfo(int pageIndex, int pageSize)
        {
            try
            {
                DBContext db = new DBContext();
                string num = string.Format(@"SELECT count(1) 
                                             from click_log cl
                                            join md_seminar_meeting_main m on m.mid=cl.mid and cl.modelName='在线观看'
                                            join userinfo u on cl.openid=u.openid
                                            LEFT JOIN (SELECT distinct doc_name ,  doc_code from  sfe_register) sfe on sfe.doc_code=u.doctorCode");
                string sql = string.Format(@"SELECT m.mcode, m.mtitle,m.msite,m.mbegintime,m.mendtime,sfe.doc_code,sfe.doc_name, cl.createTime
                                            from click_log cl
                                            join md_seminar_meeting_main m on m.mid=cl.mid and cl.modelName='在线观看'
                                            join userinfo u on cl.openid=u.openid
                                            LEFT JOIN (SELECT distinct doc_name ,  doc_code from  sfe_register) sfe on sfe.doc_code=u.doctorCode
                                            ORDER BY m.mid 
                                            LIMIT {0},{1};", (pageIndex - 1) * pageSize, pageSize);
                var count = db.Database.SqlQuery<int>(num).FirstOrDefault();
                var s = db.Database.SqlQuery<data_live>(sql);
                IsoDateTimeConverter dtConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
                var pages = count % pageSize == 0 ? count / pageSize : count / pageSize + 1;
                return Content(JsonConvert.SerializeObject(new { status = true, data = s, totalPages = pages, totalItems = count }, dtConverter));
            }
            catch (Exception e)
            {
                return Json(new { status = false, errmsg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult GetSendEmailInfo(int pageIndex, int pageSize)
        {
            try
            {
                DBContext db = new DBContext();
                string num = string.Format(@"SELECT count(1) 
                                            from send_email_log el
                                            join md_seminar_meeting_main m on m.mid=el.mid  
                                            join userinfo u on el.openid=u.openid
                                            LEFT JOIN (SELECT distinct doc_name ,  doc_code from  sfe_register) sfe on sfe.doc_code=u.doctorCode");
                string sql = string.Format(@"SELECT m.mcode, m.mtitle,m.msite,m.mbegintime,m.mendtime,sfe.doc_code,sfe.doc_name,u.email,el.ToEmaill as to_email,el.`name`, el.`status`, el.createTime
                                            from send_email_log el
                                            join md_seminar_meeting_main m on m.mid=el.mid  
                                            join userinfo u on el.openid=u.openid
                                            LEFT JOIN (SELECT distinct doc_name ,  doc_code from  sfe_register) sfe on sfe.doc_code=u.doctorCode
                                            ORDER BY m.mid
                                            LIMIT {0},{1};", (pageIndex - 1) * pageSize, pageSize);
                var count = db.Database.SqlQuery<int>(num).FirstOrDefault();
                var s = db.Database.SqlQuery<data_send_email>(sql);
                IsoDateTimeConverter dtConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
                var pages = count % pageSize == 0 ? count / pageSize : count / pageSize + 1;
                return Content(JsonConvert.SerializeObject(new { status = true, data = s, totalPages = pages, totalItems = count }, dtConverter));
            }
            catch (Exception e)
            {
                return Json(new { status = false, errmsg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult GetClickDataInfo(int pageIndex, int pageSize)
        {
            try
            {
                DBContext db = new DBContext();
                string num = string.Format(@"SELECT count(1) 
                                            from data_click_log dcl
                                            left JOIN md_seminar_meeting_detail md on md.did=dcl.did
                                            join md_seminar_meeting_main m on m.mid=dcl.mid  
                                            join userinfo u on dcl.openid=u.openid
                                            LEFT JOIN (SELECT distinct doc_name ,  doc_code from  sfe_register) sfe on sfe.doc_code=u.doctorCode");
                string sql = string.Format(@"SELECT m.mcode, m.mtitle,m.msite,m.mbegintime,m.mendtime,sfe.doc_code,sfe.doc_name,md.dname as name, dcl.createTime
                                            from data_click_log dcl
                                            left JOIN md_seminar_meeting_detail md on md.did=dcl.did
                                            join md_seminar_meeting_main m on m.mid=dcl.mid  
                                            join userinfo u on dcl.openid=u.openid
                                            LEFT JOIN (SELECT distinct doc_name ,  doc_code from  sfe_register) sfe on sfe.doc_code=u.doctorCode
                                            ORDER BY m.mid
                                            LIMIT {0},{1};", (pageIndex - 1) * pageSize, pageSize);
                var count = db.Database.SqlQuery<int>(num).FirstOrDefault();
                var s = db.Database.SqlQuery<data_click_data>(sql);
                IsoDateTimeConverter dtConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
                var pages = count % pageSize == 0 ? count / pageSize : count / pageSize + 1;
                return Content(JsonConvert.SerializeObject(new { status = true, data = s, totalPages = pages, totalItems = count }, dtConverter));
            }
            catch (Exception e)
            {
                return Json(new { status = false, errmsg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetClickInfo(int pageIndex, int pageSize)
        {
            try
            {
                DBContext db = new DBContext();
                string num = string.Format(@"SELECT count(1)  from behavior_record br 
                                            left join userinfo u on br.openid=u.openid
                                            left JOIN (SELECT distinct doc_name ,  doc_code from  sfe_register) sfe on sfe.doc_code=u.doctorCode ");
                string sql = string.Format(@"SELECT sfe.doc_code,sfe.doc_name,br.* from behavior_record br 
                                            left join userinfo u on br.openid=u.openid
                                            left JOIN (SELECT distinct doc_name ,  doc_code from  sfe_register) sfe on sfe.doc_code=u.doctorCode 
                                            LIMIT {0},{1};", (pageIndex - 1) * pageSize, pageSize);
                var count = db.Database.SqlQuery<int>(num).FirstOrDefault();
                var s = db.Database.SqlQuery<behavior_record>(sql);
                IsoDateTimeConverter dtConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
                var pages = count % pageSize == 0 ? count / pageSize : count / pageSize + 1;
                return Content(JsonConvert.SerializeObject(new { status = true, data = s, totalPages = pages, totalItems = count }, dtConverter));
            }
            catch (Exception e)
            {
                return Json(new { status = false, errmsg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult Sign(int id)
        {
            object obj = new object();
            try
            {
                string openid = Session["openid"].ToString();
                using (DBContext db = new DBContext())
                {
                    if (db.MeetingSign.Any(x => x.meetingid == id && x.openid == openid))
                    {
                        throw new Exception("您已签到！");
                    }
                    else
                    {
                        var meeting = db.md_seminar_meeting_main.FirstOrDefault(m => m.mid == id);
                        UserInfoService userInfoService = new UserInfoService();
                        var userInfo = userInfoService.SelectByOpenid(openid);
                        var meetingSign = new MeetingSign
                        {
                            createTime = DateTime.Now,
                            meetingid = id,
                            openid = openid
                        };
                        string[] s1 = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
                        Random rd = new Random();
                        string randomInvitationCode = s1[rd.Next(0, s1.Length)] + s1[rd.Next(0, s1.Length)] + rd.Next(100000, 1000000);
                        int count = 0;
                        while (db.MeetingSign.FirstOrDefault(m => m.randominvitationcode == randomInvitationCode&&m.meetingid==id) != null)
                        {
                            randomInvitationCode = s1[rd.Next(0, s1.Length)] + s1[rd.Next(0, s1.Length)] + rd.Next(100000, 1000000);
                            count++;
                            if (count > 20)
                                break;
                        }
                        meetingSign.randominvitationcode = randomInvitationCode;
                        db.MeetingSign.Add(meetingSign);
                        var workId = db.sfe_register.FirstOrDefault(r => r.doc_code == userInfo.doctorCode)?.NETWORK_EXTERNAL_ID_BMS_CN__C;
                        var tableInputList = db.table_input.Where(t => t.mcode == meeting.mcode && t.networkid == workId).ToList();
                        if (tableInputList.Count() > 0)
                        {
                            foreach (var tableInput in tableInputList)
                            {
                                tableInput.IsSign = "是";
                                tableInput.CreateTime = DateTime.Now;
                            }
                        }
                        db.Commit();
                        obj = new { success = true };
                    }
                }
            }
            catch (Exception e)
            {
                obj = new { success = false, message = e.Message };
            }
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ChangeEmail(string email)
        {
            object obj = new object();
            try
            {
                string openid = Session["openid"].ToString();
                ThirdpartyService thirdpartyService = new ThirdpartyService();
                thirdpartyService.ChangeEmail(openid, email);
                UserInfoService userInfoService = new UserInfoService();
                userInfoService.UpdateByOpenid(openid);
                obj = new { success = true };
            }
            catch (Exception e)
            {
                obj = new { success = false, message = e.Message };
            }
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public void UpdateUserInfo()
        {
            DateTime start = DateTime.Now;
            DateTime end = DateTime.Now;
            try
            {
                UserInfoService userInfoService = new UserInfoService();
                List<string> list = userInfoService.Select().Select(x => x.openid).ToList();
                //List<string> list = db.UserInfo.Select(x => x.openid).ToList();
                Thread[] thread = new Thread[20];
                for (int i = 0; i < thread.Length; i++)
                {
                    thread[i] = new Thread(() =>
                    {
                        while (list.Count > 0)
                        {
                            string openid = "";
                            lock (userInfoService)
                            {
                                if (list.Count > 0)
                                {
                                    openid = list[0];
                                    list.Remove(openid);
                                    userInfoService.UpdateByOpenid(openid);
                                }
                            }
                        }
                    }) { IsBackground = true };
                    thread[i].Start();
                }
                while (thread.Any(x => x.ThreadState != ThreadState.Stopped))
                {
                    Thread.Sleep(500);
                }
                end = DateTime.Now;
                Response.Write("更新成功，耗时：" + (end - start).TotalSeconds+"秒！");
            }
            catch (Exception ex)
            {
                end = DateTime.Now;
                Response.Write("更新失败，耗时：" + (end - start).TotalSeconds + "秒！");
            }
        }
    }
}
