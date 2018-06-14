using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Services.Models
{
    public class Openweixin
    {
         [Key]

         /// <summary>
         /// 
         /// </summary>
         public int? id { get; set; }
         /// <summary>
         /// 
         /// </summary>
         public string AppID { get; set; }
         /// <summary>
         /// 
         /// </summary>
         public string AppSecret { get; set; }
         /// <summary>
         /// 
         /// </summary>
         public string Token { get; set; }
         /// <summary>
         /// 
         /// </summary>
         public string Key { get; set; }
         /// <summary>
         /// 
         /// </summary>
         public string component_access_token { get; set; }
         /// <summary>
         /// 
         /// </summary>
         public DateTime? component_access_tokendate { get; set; }
    }
}