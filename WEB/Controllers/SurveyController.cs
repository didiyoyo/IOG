﻿using Newtonsoft.Json;
using Services;
using Services.Models;
using Services.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WEB.Filters;

namespace WEB.Controllers
{

    [PermissionsAttribute(true)]
    public class SurveyController : Controller
    {
        SurveyService ss = new SurveyService();
        SurveyResultService srs = new SurveyResultService();
        MeetingDetailService mds = new MeetingDetailService();
        mdSeminarMeetingMainService smm = new mdSeminarMeetingMainService();
        //
        // GET: /Survey/

        public ActionResult Index(int id)
        {
            ViewBag.id = id;

            string openid = Session["openid"].ToString();
            var meeting = smm.GetMeetingById(id);
            var list = ss.GetSurveyByMid(id);
            int datacount = mds.GetMeetingDataByMid(id).Count;
            ViewBag.datacount = datacount;
            ViewBag.SurvayTitle = meeting.survaytitle;
            //return View("Index_old",list);
            return View(list);
        }

        public ActionResult CheckSurvey(int id)
        { 
            return Json(new { success = true, msg = ss.CheckSurvey(id).ToString().ToLower() }, JsonRequestBehavior.AllowGet); 
        }

        public ActionResult Next(int mid, int index)
        {
            var list = ss.GetSurveyByMid(mid);
            list = list.Skip(index).Take(1).ToList();
            return Json(new { success = true, data = list });
        }

        public ActionResult AddSurvey(string JsonStr)
        {
             
            try
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                List<td_seminar_survey_result> vr = jss.Deserialize<List<td_seminar_survey_result>>(JsonStr);
                if (Session["openid"] == null)
                {
                    return Json(new { success = false, msg = "参数错误" }, JsonRequestBehavior.AllowGet);
                }
                foreach (var item in vr)
                { 
                    item.uid = Session["openid"].ToString();
                    item.sdatatiem = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")); 
                }

                var i = srs.AddVoteResult(vr);
                return Json(new { success = i == -1 ? false : true, msg = i == -1 ? "不可以重复提交" : "已提交成功，谢谢您的宝贵意见！" }, JsonRequestBehavior.AllowGet); 
            }
            catch (Exception)
            {
                return Json(new { success =false ,  msg="提交失败" }, JsonRequestBehavior.AllowGet); 
                throw;
            }

        }

        /// <summary>
        /// 已经调研提醒
        /// </summary>
        /// <returns></returns>
        public ActionResult AlreadySurveyPrompt()
        {
            return View();
        }
    }
}
