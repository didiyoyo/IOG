using log4net;
using Services.Enums;
using Services.Models;
using Services.Tools.WeiXin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Services.Service
{
    /// <summary>
    /// 会议主表
    /// </summary>
    public partial class mdSeminarMeetingMainService
    {
        private static object obj = new object();
        public md_seminar_meeting_main GetMeetingById(int id)
        {
            using (DBContext db = new DBContext())
            {
                var smm = db.md_seminar_meeting_main.AsNoTracking().Where(x => x.mid == id).SingleOrDefault();
                return smm;
            }
        }

        public List<md_seminar_meeting_main> GetMeetingByOpenID(string openId)
        {
            using (DBContext db = new DBContext())
            {
                var ids = db.td_seminar_meeting_accept.Where(x => x.OPenID == openId && x.State == 1).Select(x => x.MId).ToArray();
                var list = db.md_seminar_meeting_main.Where(x => ids.Contains(x.mid)).OrderBy(x => x.mbegintime).ToList();
                return list;
            }
        }
        private readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public void UpdateSFE()
        {
            try
            {
                string res = HttpWebResponseUtility.CreateGetHttpResponse(System.Configuration.ConfigurationManager.ConnectionStrings["updateurl"].ConnectionString, 300000, null, null);
                log.Info(res);
            }
            catch (Exception ex)
            {
                log.Info(ex);
            }
        }
        public void Task()
        {
            try
            {
                DateTime now = DateTime.Now;
                DateTime end15 = DateTime.Now.AddMinutes(15);
                DateTime end = DateTime.Now.AddDays(1);
                using (DBContext db = new DBContext())
                {
                    foreach (var meeting in db.md_seminar_meeting_main.Where(x => x.mbegintime > now && x.mbegintime < end && x.meetingmode != (int)MeetingModeTypeEnum.LargeUnderLine).ToList())
                    {
                        if (meeting.mbegintime < end15)
                        {
                            string key = meeting.mbegintime.Value.ToString() + "10";
                            if (db.messagelog.Any(x => x.meetingid == meeting.mid && x.key == key))
                            {
                                continue;
                            }
                            db.messagelog.Add(new messagelog()
                            {
                                meetingid = meeting.mid,
                                key = key
                            });
                        }
                        else
                        {
                            string key = meeting.mbegintime.Value.ToString() + "24";
                            if (db.messagelog.Any(x => x.meetingid == meeting.mid && x.key == key))
                            {
                                continue;
                            }
                            db.messagelog.Add(new messagelog()
                            {
                                meetingid = meeting.mid,
                                key = key
                            });
                        }

                        foreach (var accept in db.td_seminar_meeting_accept.Where(x => x.MId == meeting.mid).ToList())
                        {
                            string AppID = System.Configuration.ConfigurationManager.ConnectionStrings["weixin.AppID"].ConnectionString;
                            if (db.Publicnumber.Any(x => x.AppID == AppID))
                            {
                                Publicnumber number = db.Publicnumber.FirstOrDefault(x => x.AppID == AppID);
                                string accessToken = number.access_token;
                                int? res = WeiXinTool.sendTemplateMessage(AppID, accessToken, new
                                {
                                    touser = accept.OPenID,
                                    template_id = System.Configuration.ConfigurationManager.ConnectionStrings["tmpKey"].ConnectionString,
                                    url = System.Configuration.ConfigurationManager.ConnectionStrings["Host"].ConnectionString + "/Authorization/QR/0",
                                    data = new
                                    {
                                        first = new
                                        {
                                            value = "",
                                            color = ""
                                        },
                                        keyword1 = new
                                        {
                                            value = meeting.mtitle,
                                            color = "#173177"
                                        },
                                        keyword2 = new
                                        {
                                            value = meeting.mbegintime.Value.ToString("yyyy-MM-dd HH:mm") + "至" + meeting.mendtime.Value.ToString("yyyy-MM-dd HH:mm"),
                                            color = "#173177"
                                        },
                                        keyword3 = new
                                        {
                                            value = meeting.msite,
                                            color = "#173177"
                                        },
                                        keyword4 = new
                                        {
                                            value = "",
                                            color = "#173177"
                                        },
                                        remark = new
                                        {
                                            value = "会议即将召开，点击进入我的会议列表",
                                            color = "#173177"
                                        },
                                    }
                                });
                            }
                        }
                        db.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("推送消息错误");
                log.Error(ex);
            }
            try
            {
                using (DBContext db = new DBContext())
                {
                    log.Info("开启调研");
                    string AppID = System.Configuration.ConfigurationManager.ConnectionStrings["weixin.AppID"].ConnectionString;
                    if (db.Publicnumber.Any(x => x.AppID == AppID))
                    {
                        Publicnumber number = db.Publicnumber.FirstOrDefault(x => x.AppID == AppID);
                        string accessToken = number.access_token;
                        DateTime start = DateTime.Now.AddDays(-1);
                        //DateTime end = start.AddMinutes(30);&& x.mendtime < end
                        foreach (var meeting in db.md_seminar_meeting_main.Where(x => x.mendtime < start  && db.md_seminar_survey.Any(s => s.mid == x.mid && s.sstate == 0)).ToList())
                        {
                            log.Info("开启调研"+meeting.mid);
                            foreach (var survey in db.md_seminar_survey.Where(s => s.mid == meeting.mid && s.sstate == 0))
                            {
                                survey.sstate = 1;
                            }
                            db.Commit();
                            foreach (var m in db.td_seminar_meeting_accept.Where(x => x.MId == meeting.mid).ToList())
                            {
                                try
                                {

                                    //发送消息
                                    var res = WeiXinTool.sendTemplateMessage(AppID, accessToken, new
                                    {
                                        touser = m.OPenID,
                                        template_id = System.Configuration.ConfigurationManager.ConnectionStrings["SurveytmpKey"].ConnectionString,
                                        url = System.Configuration.ConfigurationManager.ConnectionStrings["Host"].ConnectionString + "Authorization/SurveyData/" + meeting.mid,
                                        data = new
                                        {
                                            first = new
                                            {
                                                value = "您好，您关注的会议有资料更新",
                                                color = "#173177"
                                            },
                                            keyword1 = new
                                            {
                                                value = "IO在握",
                                                color = "#173177"
                                            },
                                            keyword2 = new
                                            {
                                                value = meeting.mtitle,
                                                color = "#173177"
                                            },
                                            keyword3 = new
                                            {
                                                value = meeting.mbegintime.Value.ToString("yyyy-MM-dd HH:mm") + "至" + meeting.mendtime.Value.ToString("yyyy-MM-dd HH:mm"),
                                                color = "#173177"
                                            },
                                            keyword4 = new
                                            {
                                                value = meeting.msite,
                                                color = "#173177"
                                            },
                                            remark = new
                                            {
                                                value = "您参加的会议有资料更新，请点击查看会议详情。",
                                                color = "#173177"
                                            }
                                        }
                                    });
                                }
                                catch (Exception ex)
                                {
                                    log.Info("开启调研-发送模板消息失败");
                                    log.Error(ex);
                                }
                            }
                           
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Info("开启调研-异常");
                log.Error(ex);
            }
            //try
            //{
            //    DateTime start = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 11:56:00"));
            //    DateTime end = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 12:12:00"));
            //    if (DateTime.Now > start && DateTime.Now < end)
            //    {

            //        start = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 23:59:59"));
            //        end = start.AddDays(1);
            //        using (DBContext db = new DBContext())
            //        {
            //            foreach (var meeting in db.md_seminar_meeting_main.Where(x => x.mbegintime > start && x.mbegintime < end).ToList())
            //            {
            //                foreach (var accept in db.td_seminar_meeting_accept.Where(x => x.MId == meeting.mid).ToList())
            //                {
            //                    string AppID = System.Configuration.ConfigurationManager.ConnectionStrings["weixin.AppID"].ConnectionString;
            //                    if (db.Publicnumber.Any(x => x.AppID == AppID))
            //                    {
            //                        Publicnumber number = db.Publicnumber.FirstOrDefault(x => x.AppID == AppID);
            //                        string accessToken = number.access_token;
            //                        int? res = WeiXinTool.sendTemplateMessage(AppID, accessToken, new
            //                        {
            //                            touser = accept.OPenID,
            //                            template_id = System.Configuration.ConfigurationManager.ConnectionStrings["tmpKey"].ConnectionString,
            //                            url = "http://vmeeting.bms.com.cn/QR/0",
            //                            data = new
            //                            {
            //                                first = new
            //                                {
            //                                    value = "",
            //                                    color = ""
            //                                },
            //                                keyword1 = new
            //                                {
            //                                    value = meeting.mtitle,
            //                                    color = "#173177"
            //                                },
            //                                keyword2 = new
            //                                {
            //                                    value = meeting.mbegintime.Value.ToString("yyyy-MM-dd HH:mm") + "至" + meeting.mendtime.Value.ToString("yyyy-MM-dd HH:mm"),
            //                                    color = "#173177"
            //                                },
            //                                keyword3 = new
            //                                {
            //                                    value = meeting.msite,
            //                                    color = "#173177"
            //                                },
            //                                keyword4 = new
            //                                {
            //                                    value = "",
            //                                    color = ""
            //                                },
            //                                remark = new
            //                                {
            //                                    value = "会议即将召开，点击进入我的会议列表",
            //                                    color = "#173177"
            //                                },
            //                            }
            //                        });
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            //catch(Exception ex)
            //{

            //}
        }

        public void PushMessageToDoctor()
        {
            lock (obj)
            {
                try
                {
                    var templateId = System.Configuration.ConfigurationManager.ConnectionStrings["MeetingInvitationTemplateId"].ConnectionString;
                    DateTime now = DateTime.Now;
                    //DateTime end24 = now.AddHours(24);
                    //DateTime end72 = now.AddHours(72);
                    DateTime end24 = now.AddMinutes(24 * 60);
                    DateTime end48 = now.AddMinutes(48 * 60);
                    using (DBContext db = new DBContext())
                    {
                        string AppID = System.Configuration.ConfigurationManager.ConnectionStrings["weixin.AppID"].ConnectionString;
                        if (db.Publicnumber.Any(x => x.AppID == AppID))
                        {
                            Publicnumber number = db.Publicnumber.FirstOrDefault(x => x.AppID == AppID);
                            string accessToken = number.access_token;
                            var meetingList = db.md_seminar_meeting_main.Where(x => x.mbegintime < end48 && (!x.pushCount.HasValue || x.pushCount.Value != 2) && x.meetingmode == (int)MeetingModeTypeEnum.LargeUnderLine).ToList();
                            foreach (var meeting in meetingList)
                            {
                                if (string.IsNullOrEmpty(meeting.mhyrc_type))
                                {
                                    continue;
                                }
                                if (!string.IsNullOrEmpty(meeting.createtime) && meeting.mbegintime < Convert.ToDateTime(meeting.createtime).AddHours(24))
                                {
                                    continue;
                                }
                                var doctorList = db.table_input.Where(t => t.mcode == meeting.mcode && t.ModelType == "邀请文件").ToList();
                                var networkIdList = doctorList.Select(d => d.networkid).ToList();
                                var doctorCodeList = db.sfe_register.Where(s => networkIdList.Contains(s.NETWORK_EXTERNAL_ID_BMS_CN__C) && s.doc_code != null).Select(s => s.doc_code).ToList();
                                var openIdList = db.UserInfo.Where(u => doctorCodeList.Contains(u.doctorCode)).Select(u => u.openid).ToList();
                                if (meeting.mbegintime > end24 && meeting.mbegintime < end48 && (!meeting.pushCount.HasValue || meeting.pushCount.Value == 0))
                                {
                                    foreach (var openid in openIdList)
                                    {
                                        //发送消息
                                        var res = WeiXinTool.sendTemplateMessage(AppID, accessToken, new
                                        {
                                            touser = openid,
                                            template_id = templateId,
                                            url = System.Configuration.ConfigurationManager.ConnectionStrings["qrHost"].ConnectionString + "WeiXin/MyMeetingSchedule/" + meeting.mid,
                                            data = new
                                            {
                                                //first = new
                                                //{
                                                //    value = "",
                                                //    color = ""
                                                //},
                                                keyword1 = new
                                                {
                                                    value = meeting.mtitle,
                                                    color = "#173177"
                                                },
                                                keyword2 = new
                                                {
                                                    value = (meeting.mbegintime)?.ToString("yyyy年MM月dd日 HH:mm"),
                                                    color = "#173177"
                                                },
                                                keyword3 = new
                                                {
                                                    value = meeting.msite,
                                                    color = "#173177"
                                                },
                                                keyword4 = new
                                                {
                                                    value = "邀请您参加会议，请点击进入查看会议邀请函。",
                                                    color = "#173177"
                                                }
                                                //remark = new
                                                //{
                                                //    value = "邀请您参加会议，请点击进入查看会议邀请。",
                                                //    color = "#173177"
                                                //}
                                            }
                                        });
                                    }
                                    meeting.pushCount = 1;
                                }
                                else if (meeting.mbegintime < end24 && meeting.mbegintime > now)
                                {
                                    if (!meeting.pushCount.HasValue || meeting.pushCount.Value == 0)
                                    {
                                        foreach (var openid in openIdList)
                                        {
                                            //发送消息
                                            var res = WeiXinTool.sendTemplateMessage(AppID, accessToken, new
                                            {
                                                touser = openid,
                                                template_id = templateId,
                                                url = System.Configuration.ConfigurationManager.ConnectionStrings["qrHost"].ConnectionString + "WeiXin/MyMeetingSchedule/" + meeting.mid,
                                                data = new
                                                {
                                                    //first = new
                                                    //{
                                                    //    value = "",
                                                    //    color = ""
                                                    //},
                                                    keyword1 = new
                                                    {
                                                        value = meeting.mtitle,
                                                        color = "#173177"
                                                    },
                                                    keyword2 = new
                                                    {
                                                        value = (meeting.mbegintime)?.ToString("yyyy年MM月dd日 HH:mm"),
                                                        color = "#173177"
                                                    },
                                                    keyword3 = new
                                                    {
                                                        value = meeting.msite,
                                                        color = "#173177"
                                                    },
                                                    keyword4 = new
                                                    {
                                                        value = "邀请您参加会议，请点击进入查看会议邀请函。",
                                                        color = "#173177"
                                                    }
                                                    //remark = new
                                                    //{
                                                    //    value = "邀请您参加会议，请点击进入查看会议邀请。",
                                                    //    color = "#173177"
                                                    //}
                                                }
                                            });
                                        }
                                        meeting.pushCount = 2;
                                    }
                                    else if (meeting.pushCount == 1)
                                    {
                                        foreach (var openid in openIdList)
                                        {
                                            //发送消息
                                            var res = WeiXinTool.sendTemplateMessage(AppID, accessToken, new
                                            {
                                                touser = openid,
                                                template_id = templateId,
                                                url = System.Configuration.ConfigurationManager.ConnectionStrings["qrHost"].ConnectionString + "WeiXin/MyMeetingSchedule/" + meeting.mid,
                                                data = new
                                                {
                                                    //first = new
                                                    //{
                                                    //    value = "",
                                                    //    color = ""
                                                    //},
                                                    keyword1 = new
                                                    {
                                                        value = meeting.mtitle,
                                                        color = "#173177"
                                                    },
                                                    keyword2 = new
                                                    {
                                                        value = (meeting.mbegintime)?.ToString("yyyy年MM月dd日 HH:mm"),
                                                        color = "#173177"
                                                    },
                                                    keyword3 = new
                                                    {
                                                        value = meeting.msite,
                                                        color = "#173177"
                                                    },
                                                    keyword4 = new
                                                    {
                                                        value = "邀请您参加会议，请点击进入查看会议邀请函。",
                                                        color = "#173177"
                                                    }
                                                    //remark = new
                                                    //{
                                                    //    value = "邀请您参加会议，请点击进入查看会议邀请。",
                                                    //    color = "#173177"
                                                    //}
                                                }
                                            });
                                        }
                                        meeting.pushCount = 2;
                                    }
                                }
                            }
                            db.Commit();
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}