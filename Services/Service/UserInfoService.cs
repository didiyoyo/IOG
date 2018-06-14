using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Models;
using Services.Thirdparty;
using System.Threading;

namespace Services.Service
{
    public class UserInfoService
    {
        DBContext db = new DBContext();
        public void Insert(UserInfo UserInfo)
        {
            db.UserInfo.Add(UserInfo);
            db.SaveChanges();
        }
        public void Update(UserInfo UserInfo)
        {
            db.Entry<UserInfo>(UserInfo).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }
        public void Delete(UserInfo UserInfo)
        {
            db.UserInfo.Remove(UserInfo);
            db.SaveChanges();
        }
        public List<UserInfo> Select()
        {
            List<UserInfo> list = db.UserInfo.ToList();
            return list;
        }
        public UserInfo Select(string Name)
        {
            return db.UserInfo.First(x => x.name == Name);
        }
        public void UpdateByOpenid(string openid)
        {
            ThirdpartyService service = new ThirdpartyService();
            UserInfo newinfo = service.getUserInfo(openid);
            if (db.UserInfo.Any(x => x.openid == openid))
            {
                if (newinfo != null)
                {
                    UserInfo userInfo = db.UserInfo.FirstOrDefault(x => x.openid == openid);
                    if (userInfo._status == newinfo._status &&
                    userInfo.academicTitle == newinfo.academicTitle &&
                    userInfo.administrativeTitle == newinfo.administrativeTitle &&
                    userInfo.avatar == newinfo.avatar &&
                    userInfo.city == newinfo.city &&
                    userInfo.department == newinfo.department &&
                    userInfo.doctorLicense == newinfo.doctorLicense &&
                    userInfo.doctorMDId == newinfo.doctorMDId &&
                    userInfo.email == newinfo.email &&
                    userInfo.gender == newinfo.gender &&
                    userInfo.hospitalAddress == newinfo.hospitalAddress &&
                    userInfo.hospitalName == newinfo.hospitalName &&
                    userInfo.mark == newinfo.mark &&
                    userInfo.mobile == newinfo.mobile &&
                    userInfo.name == newinfo.name &&
                    userInfo.nickname == newinfo.nickname &&
                    userInfo.postalCode == newinfo.postalCode &&
                    userInfo.professionalTitle == newinfo.professionalTitle &&
                    userInfo.province == newinfo.province &&
                    userInfo.serialNumber == newinfo.serialNumber &&
                    userInfo.uuid == newinfo.uuid &&
                    userInfo.doctorCode == newinfo.doctorCode &&
                    userInfo.statusCode == newinfo.statusCode &&
                    userInfo.hospitalCode == newinfo.hospitalCode &&
                    userInfo.departmentCode == newinfo.departmentCode &&
                    userInfo.therapeuticArea == newinfo.therapeuticArea &&
                    userInfo.wechatNickname == newinfo.wechatNickname &&
                    userInfo.subscribeTime == newinfo.subscribeTime &&
                    userInfo.username == newinfo.username)
                    {
                        return;
                    }
                    userInfo._status = newinfo._status;
                    userInfo.subscribeTime = newinfo.subscribeTime;
                    userInfo.academicTitle = newinfo.academicTitle;
                    userInfo.administrativeTitle = newinfo.administrativeTitle;
                    userInfo.avatar = newinfo.avatar;
                    userInfo.city = newinfo.city;
                    userInfo.department = newinfo.department;
                    userInfo.doctorLicense = newinfo.doctorLicense;
                    userInfo.doctorMDId = newinfo.doctorMDId;
                    userInfo.email = newinfo.email;
                    userInfo.gender = newinfo.gender;
                    userInfo.hospitalAddress = newinfo.hospitalAddress;
                    userInfo.hospitalName = newinfo.hospitalName;
                    userInfo.mark = newinfo.mark;
                    userInfo.mobile = newinfo.mobile;
                    userInfo.name = newinfo.name;
                    userInfo.nickname = newinfo.nickname;
                    userInfo.postalCode = newinfo.postalCode;
                    userInfo.professionalTitle = newinfo.professionalTitle;
                    userInfo.province = newinfo.province;
                    userInfo.serialNumber = newinfo.serialNumber;
                    userInfo.uuid = newinfo.uuid;
                    userInfo.doctorCode = newinfo.doctorCode;
                    userInfo.statusCode = newinfo.statusCode;
                    userInfo.hospitalCode = newinfo.hospitalCode;
                    userInfo.departmentCode = newinfo.departmentCode;
                    userInfo.therapeuticArea = newinfo.therapeuticArea;
                    userInfo.wechatNickname = newinfo.wechatNickname;
                    userInfo.username = newinfo.username;
                    this.Update(userInfo);
                }
            }
            else
            {
                if (newinfo == null)
                {
                    newinfo = new UserInfo();
                }
                newinfo.openid = openid;
                this.Insert(newinfo);
            }
            db.SaveChanges();
        }
        public UserInfo SelectByOpenid(string openid)
        {
            UpdateByOpenid(openid);
            db.Commit();
            return db.UserInfo.FirstOrDefault(x => x.openid == openid);
        }


        public string GetDocName(string openid)
        {
            string doc_name = "";
            var user = db.UserInfo.FirstOrDefault(x => x.openid == openid);
            if (user != null)
            {
                var doc = db.sfe_register.FirstOrDefault(x => x.doc_code == user.doctorCode);
                if (doc != null)
                {
                    doc_name = doc.doc_name;
                }
            }
            return doc_name;
        }

        public sfe_register GetDocInfo(string openid)
        {
            var user = db.UserInfo.FirstOrDefault(x => x.openid == openid);
            var doc = db.sfe_register.FirstOrDefault(x => x.doc_code == user.doctorCode);
            return doc;
        }
        public void UpdateUserInfo()
        {
            
        }

    }
}