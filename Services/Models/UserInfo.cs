using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Services.Models
{
    public class UserInfo
    {
        public int? id { get; set; }
        public string openid { get; set; }
        public string serialNumber { get; set; }
        public string academicTitle { get; set; }
        public string administrativeTitle { get; set; }
        public string doctorMDId { get; set; }
        public string email { get; set; }
        public int? gender { get; set; }
        public string doctorLicense { get; set; }
        public string hospitalAddress { get; set; }
        public string hospitalName { get; set; }
        public string mobile { get; set; }
        public string postalCode { get; set; }
        public string professionalTitle { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public string department { get; set; }
        public string nickname { get; set; }
        public string avatar { get; set; }
        public string uuid { get; set; }
        public string name { get; set; }
        public int? _status { get; set; }
        public string mark { get; set; }
        public string doctorCode { get; set; }
        /// <summary>
        /// Accepted已认证
        /// </summary>
        public string statusCode { get; set; }
        public string hospitalCode { get; set; }
        public string departmentCode { get; set; }
        public string therapeuticArea { get; set; }
        
        public string wechatNickname { get; set; }
        public string username { get; set; }
        public int? subscribeTime { get; set; }
        public int? isInterior { get; set; }
    }

}