using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service
{
    public class ClickLogService
    {
        public int AddClickLog(click_log cl)
        {
            using (DBContext db = new DBContext())
            {
                string Time = Convert.ToDateTime(cl.createTime).ToString("yyyy-MM-dd HH:mm");
                if (db.click_log.Any(x =>x.createTime.ToString().Contains(Time)&& x.mid==cl.mid&& x.modelName==cl.modelName && x.openid==cl.openid)) {
                    return 0;
                }
                db.click_log.Add(cl);
                db.Commit();
                return cl.id;
            }
        }

        /// <summary>
        /// 添加资料查看记录
        /// </summary>
        /// <param name="dcl"></param>
        /// <returns></returns>
        public int AddDataClickLog(data_click_log dcl)
        {
            using (DBContext db = new DBContext())
            {
                string Time=Convert.ToDateTime(dcl.createTime).ToString("yyyy-MM-dd HH:mm");
                if (db.data_click_log.Any(x =>x.createTime.ToString().Contains(Time) && x.did==dcl.did && x.mid==dcl.mid && x.openid==dcl.openid)) {
                    return 0;
                }
                db.data_click_log.Add(dcl);
                db.Commit();
                return dcl.id;
            }
        }

        /// <summary>
        /// 添加行为记录
        /// </summary>
        /// <param name="br"></param>
        /// <returns></returns>
        public int AddBehavior_Record(behavior_record br)
        {
            using (DBContext db = new DBContext())
            {

                db.behavior_record.Add(br);
                db.Commit();
                return br.id;
            }
        }
    }
}
