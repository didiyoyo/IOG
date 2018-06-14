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
    public class VoteController : Controller
    {
        mdSeminarMeetingMainService smm = new mdSeminarMeetingMainService();
        VoteService vs = new VoteService();
        VoteResultService vr = new VoteResultService();
        //
        // GET: /Vote/

        public ActionResult Index(int id)
        {
            var meeting = smm.GetMeetingById(id);
            var list = vs.GetVoteByMid(id);
            @ViewBag.mTitle = meeting.mtitle;
            return View(list);
        }

        public ActionResult CheckVote(int id)
        {
            return Json(new { success = true, msg = vs.CheckVote(id).ToString().ToLower() }, JsonRequestBehavior.AllowGet); 
        }

        public ActionResult Vote(int id) {
            var vote = vs.GetVoteByid(id);
            return View(vote);
        }

        public ActionResult VoteById(int id)
        {
            var vote = vs.GetVoteByid(id);
            return Json(new { success = true, data = vote }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult IsVote(int id)
        {
            if (Session["openid"] == null)
            {
                return Json(new { success = false, msg = "参数错误" }, JsonRequestBehavior.AllowGet);
            }
            else
            { 
                return Json(new { success = true, data = vr.IsVote(Session["openid"].ToString(), id).ToString().ToLower() }, JsonRequestBehavior.AllowGet); 

            }
        }

        public ActionResult AddVote(td_seminar_vote_result _vr)
        {
            try
            {
                if (Session["openid"] == null)
                {
                    return Json(new { success = false, msg = "参数错误" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    _vr.uid = Session["openid"].ToString();
                    _vr.vdatetime =DateTime.Parse( DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    var i = vr.AddVoteResult(_vr);
                    if(i==-1){
                        return Json(new { success =false, msg = "请不要重复提交" }, JsonRequestBehavior.AllowGet);
                    }else if(i==-2){
                        return Json(new { success = true, msg = "提交失败，该题投票已结束。页面跳转中，请稍等" }, JsonRequestBehavior.AllowGet);
                    }
                    else if (i == -3)
                    {
                        return Json(new { success = false, msg = "提交失败，该题投票已关闭" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = true, msg = "提交成功。页面跳转中，请稍等" }, JsonRequestBehavior.AllowGet);
                    }
                    
                }
            }
            catch (Exception)
            {

                throw;
            }

            
        }

        public ActionResult VoteChart(int id)
        { 
            md_seminar_vote v = vs.GetVoteByid(id);
            return View(v);

        }

        public ActionResult GetVoteResultList(int vid)
        {
            List<td_seminar_vote_result> VoteResults = vr.GetVoteResultList(vid);
            var vrs = VoteResults.OrderBy(x => x.vresult).GroupBy(x => x.vresult).Select(x => new { count = x.Count(), result = x.FirstOrDefault().vresult });
            return Json(new { success = true, Count = VoteResults.Count(), data = vrs }, JsonRequestBehavior.AllowGet);

            //return View();
        }
    }
}
