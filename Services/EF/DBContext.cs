using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Models;

namespace Services
{
    //[DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class DBContext : EFDataContext
    {
        public int department_id = 0;
        public DBContext()
            : base("name=DBContext")
        {
            this.Database.Initialize(false);
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            string product = "";
            modelBuilder.Entity<UserInfo>().ToTable(product+"UserInfo");
            modelBuilder.Entity<QRCode>().ToTable(product+"QRCode");

            modelBuilder.Entity<md_seminar_meeting_main>().ToTable(product+"md_seminar_meeting_main");
            modelBuilder.Entity<md_seminar_rep>().ToTable(product+"md_seminar_rep");
            modelBuilder.Entity<md_seminar_geo>().ToTable(product + "md_seminar_geo");
            modelBuilder.Entity<md_seminar_meeting_detail>().ToTable(product + "md_seminar_meeting_detail");
            modelBuilder.Entity<md_seminar_vote>().ToTable(product + "md_seminar_vote");
            modelBuilder.Entity<md_seminar_survey>().ToTable(product + "md_seminar_survey");
            modelBuilder.Entity<td_seminar_meeting_accept>().ToTable(product + "td_seminar_meeting_accept");
            modelBuilder.Entity<td_seminar_vote_result>().ToTable(product + "td_seminar_vote_result");
            modelBuilder.Entity<td_seminar_question>().ToTable(product + "td_seminar_question");
            modelBuilder.Entity<td_seminar_survey_result>().ToTable(product + "td_seminar_survey_result");
            modelBuilder.Entity<Openweixin>().ToTable(product + "openweixin");
            modelBuilder.Entity<Publicnumber>().ToTable(product + "publicnumber");
            modelBuilder.Entity<sfe_register>().ToTable( "sfe_register");
            modelBuilder.Entity<click_log>().ToTable(product + "click_log");
            modelBuilder.Entity<data_click_log>().ToTable(product + "data_click_log");
            modelBuilder.Entity<SendEmail>().ToTable(product + "send_email_log");
            modelBuilder.Entity<behavior_record>().ToTable(product + "behavior_record");
            modelBuilder.Entity<messagelog>().ToTable(product + "messagelog");
            modelBuilder.Entity<MeetingSign>().ToTable(product + "MeetingSign");
            modelBuilder.Entity<travel_information>().ToTable(product + "travel_information");
            modelBuilder.Entity<table_input>().ToTable(product + "table_input");
            modelBuilder.Entity<agenda>().ToTable(product + "agenda");
            modelBuilder.Entity<file_information>().ToTable(product + "file_information");
            modelBuilder.Entity<sfe_push>().ToTable(product + "sfe_push");
        }
        #region 字段
        public DbSet<MeetingSign> MeetingSign { get; set; }
        public DbSet<Openweixin> Openweixin { get; set; }
        public DbSet<messagelog> messagelog { get; set; }
        public DbSet<Publicnumber> Publicnumber { get; set; }
        public DbSet<UserInfo> UserInfo { get; set; }
        public DbSet<QRCode> QRCode { get; set; }

        public DbSet<md_seminar_meeting_main> md_seminar_meeting_main { get; set; }
        public DbSet<md_seminar_rep> md_seminar_rep { get; set; }
        public DbSet<md_seminar_geo> md_seminar_geo { get; set; }
        public DbSet<md_seminar_meeting_detail> md_seminar_meeting_detail { get; set; }
        public DbSet<md_seminar_vote> md_seminar_vote { get; set; }
        public DbSet<md_seminar_survey> md_seminar_survey { get; set; }
        public DbSet<td_seminar_meeting_accept> td_seminar_meeting_accept { get; set; }
        public DbSet<td_seminar_vote_result> td_seminar_vote_result { get; set; }
        public DbSet<td_seminar_question> td_seminar_question { get; set; }
        public DbSet<td_seminar_survey_result> td_seminar_survey_result { get; set; }
        public DbSet<sfe_register> sfe_register { get; set; }
        public DbSet<click_log> click_log { get; set; }
        public DbSet<data_click_log> data_click_log { get; set; }
        public DbSet<SendEmail> SendEmail { get; set; }
        public DbSet<behavior_record> behavior_record { get; set; }
        public DbSet<travel_information> travel_information { get; set; }
        public DbSet<table_input> table_input { get; set; }

        public DbSet<agenda> agenda { get; set; }

        public DbSet<file_information> file_information { get; set; }

        public DbSet<sfe_push> sfe_push { get; set; }
        #endregion
        ~DBContext()
        {
            //base.Commit();
        }
    }
}
