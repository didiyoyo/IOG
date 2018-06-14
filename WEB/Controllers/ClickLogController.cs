using Services.Models;
using Services.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEB.Filters;

namespace WEB.Controllers
{
    [PermissionsAttribute(true)]
    public class ClickLogController : Controller
    {
        //
        // GET: /Click_Log/

        public ActionResult Click_Log(click_log cl)
        {
            try
            {
                string openid= Session["openid"].ToString();
                cl.openid = openid;
                cl.createTime = DateTime.Now;
                new ClickLogService().AddClickLog(cl);
                return Json(new { success = true, msg = "添加成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, msg = "添加失败" }, JsonRequestBehavior.AllowGet);
                throw;
            }  
        }


        public ActionResult Data_Click_Log(data_click_log dcl)
        {
            try
            {
                string openid = Session["openid"].ToString();
                dcl.openid = openid;
                dcl.createTime = DateTime.Now;
                new ClickLogService().AddDataClickLog(dcl);
                return Json(new { success = true, msg = "添加成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, msg = "添加失败" }, JsonRequestBehavior.AllowGet);
                throw;
            }
        }

        //public ActionResult Behavior_Record(string url, string sdate, string edate, string ip, string agent, string user, string operate, string mark, string field_id,string  field_valuestring)
        public ActionResult Behavior_Record(behavior_record br)
        {
            try
            {
                string openid = Session["openid"].ToString();
                br.openid = openid;
                br.createTime = DateTime.Now;
                new ClickLogService().AddBehavior_Record(br);
                return Json(new { success = true, msg = "添加成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, msg = "添加失败" }, JsonRequestBehavior.AllowGet);
                throw;
            }
        }
    }
}
