using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Services.Models
{
    public class sfe_register
    {
        [Key]

        [Column("id")]
        /// <summary>
        /// 
        /// </summary>
        public string id { get; set; }
        [Column("BU")]
        /// <summary>
        /// 
        /// </summary>
        public string BU { get; set; }
        [Column("territory_code")]
        /// <summary>
        /// 
        /// </summary>
        public string territory_code { get; set; }
        [Column("CYCLE_PLAN_VOD_R_TERRITORY_VOD__C")]
        /// <summary>
        /// 
        /// </summary>
        public string CYCLE_PLAN_VOD_R_TERRITORY_VOD__C { get; set; }
        [Column("user_code")]
        /// <summary>
        /// 
        /// </summary>
        public string user_code { get; set; }
        [Column("user_name")]
        /// <summary>
        /// 
        /// </summary>
        public string user_name { get; set; }
        [Column("PRIMARY_PARENT_VOD_R_BP_ID_BMS_CORE_C")]
        /// <summary>
        /// 
        /// </summary>
        public string PRIMARY_PARENT_VOD_R_BP_ID_BMS_CORE_C { get; set; }
        [Column("Team_code")]
        /// <summary>
        /// 
        /// </summary>
        public string Team_code { get; set; }
        [Column("Product_ID")]
        /// <summary>
        /// 
        /// </summary>
        public string Product_ID { get; set; }
        [Column("ins_code")]
        /// <summary>
        /// 
        /// </summary>
        public string ins_code { get; set; }
        [Column("ins_name")]
        /// <summary>
        /// 
        /// </summary>
        public string ins_name { get; set; }
        [Column("doc_code")]
        /// <summary>
        /// 
        /// </summary>
        public string doc_code { get; set; }
        [Column("doc_name")]
        /// <summary>
        /// 
        /// </summary>
        public string doc_name { get; set; }
        [Column("Sex_code")]
        /// <summary>
        /// 
        /// </summary>
        public string Sex_code { get; set; }
        [Column("CYCLE_PLAN_ACCOUNT_VOD__R.DEPARTMENT_BMS_CN__C")]
        /// <summary>
        /// 
        /// </summary>
        public string CYCLE_PLAN_ACCOUNT_VOD__R_DEPARTMENT_BMS_CN__C { get; set; }
        [Column("department")]
        /// <summary>
        /// 
        /// </summary>
        public string department { get; set; }
        [Column("Admin_title")]
        /// <summary>
        /// 
        /// </summary>
        public string Admin_title { get; set; }
        [Column("CYCLE_PLAN_ACCOUNT_VOD__R.PROFESSIONAL_TITLE_BMS_CN__C")]
        /// <summary>
        /// 
        /// </summary>
        public string CYCLE_PLAN_ACCOUNT_VOD__R_PROFESSIONAL_TITLE_BMS_CN__C { get; set; }
        [Column("Pro_title")]
        /// <summary>
        /// 
        /// </summary>
        public string Pro_title { get; set; }
        [Column("Academic_title")]
        /// <summary>
        /// 
        /// </summary>
        public string Academic_title { get; set; }
        [Column("Decision of Beds Index")]
        /// <summary>
        /// 
        /// </summary>
        public string Decision_of_Beds_Index { get; set; }
        [Column("Weekly Out-patients")]
        /// <summary>
        /// 
        /// </summary>
        public string Weekly_Out_patients { get; set; }
        [Column("Weekly HBV Out-patients")]
        /// <summary>
        /// 
        /// </summary>
        public string Weekly_HBV_Out_patients { get; set; }
        [Column("3 Months Average ETV/ARV")]
        /// <summary>
        /// 
        /// </summary>
        public string T3_Months_Average_ETV_ARV { get; set; }
        [Column("HBV Beds")]
        /// <summary>
        /// 
        /// </summary>
        public string HBV_Beds { get; set; }
        [Column("3 Months Average HP/ETV")]
        /// <summary>
        /// 
        /// </summary>
        public string T3_Months_Average_HP_ETV { get; set; }
        [Column("3 Months Average Lamivudine/ARV")]
        /// <summary>
        /// 
        /// </summary>
        public string T3_Months_Average_Lamivudine_ARV { get; set; }
        [Column("CML Patients")]
        /// <summary>
        /// 
        /// </summary>
        public string CML_Patients { get; set; }
        [Column("SUB_SPECIALTY_HTN")]
        /// <summary>
        /// 
        /// </summary>
        public string SUB_SPECIALTY_HTN { get; set; }
        [Column("CML_FIRST_TREATED_PATIENTS")]
        /// <summary>
        /// 
        /// </summary>
        public string CML_FIRST_TREATED_PATIENTS { get; set; }
        [Column("CML_RE_TREATED_PATIENTS")]
        /// <summary>
        /// 
        /// </summary>
        public string CML_RE_TREATED_PATIENTS { get; set; }
        [Column("Type2 Diabetes Patients")]
        /// <summary>
        /// 
        /// </summary>
        public string Type2_Diabetes_Patients { get; set; }
        [Column("Monthly Out-patients Days")]
        /// <summary>
        /// 
        /// </summary>
        public string Monthly_Out_patients_Days { get; set; }
        [Column("INDICATION_TAXOL")]
        /// <summary>
        /// 
        /// </summary>
        public string INDICATION_TAXOL { get; set; }
        [Column("Beds(Eliquis)")]
        /// <summary>
        /// 
        /// </summary>
        public string Beds_Eliquis_ { get; set; }
        [Column("Monthly Operations")]
        /// <summary>
        /// 
        /// </summary>
        public string Monthly_Operations { get; set; }
        [Column("Category")]
        /// <summary>
        /// 
        /// </summary>
        public string Category { get; set; }
        [Column("New_class")]
        /// <summary>
        /// 
        /// </summary>
        public string New_class { get; set; }
        [Column("ALGORITHM_PRODUCTS_BMS_CN__C")]
        /// <summary>
        /// 
        /// </summary>
        public string ALGORITHM_PRODUCTS_BMS_CN__C { get; set; }
        [Column("Region")]
        /// <summary>
        /// 
        /// </summary>
        public string Region { get; set; }
        [Column("ETV_Classification")]
        /// <summary>
        /// 
        /// </summary>
        public string ETV_Classification { get; set; }
        [Column("ANTICOAGULATION_DECISION")]
        /// <summary>
        /// 
        /// </summary>
        public string ANTICOAGULATION_DECISION { get; set; }
        [Column("PROVINCE_BMS_CN__C")]
        /// <summary>
        /// 
        /// </summary>
        public string PROVINCE_BMS_CN__C { get; set; }
        [Column("CITY_BMS_CN__C")]
        /// <summary>
        /// 
        /// </summary>
        public string CITY_BMS_CN__C { get; set; }

        public string NETWORK_EXTERNAL_ID_BMS_CN__C { get; set; }
    }
}