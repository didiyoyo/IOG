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
    public class QuestionController : Controller
    {
        QuestionService qs = new QuestionService();
        //
        // GET: /Question/

        public ActionResult Index(int id)
        {
            @ViewBag.Mid = id;
            return View();
        }

         public ActionResult AddQuestion(td_seminar_question q)
        { 
              try
            {
                if (Session["openid"] == null)
                {
                    return Json(new { success = false, msg = "参数错误" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    q.uid = Session["openid"].ToString();
                    q.datetime =DateTime.Parse( DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    var i =qs.AddQuestion(q);
                    return Json(new { success = i == 0 ? false : true, msg = i == -1 ? "提交失败" : "提交成功" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {

                throw;
            }
             
            
        }

    }
}
