using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Services.Models
{
    public class Publicnumber
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
         public string access_token { get; set; }
         /// <summary>
         /// 
         /// </summary>
         public string refresh_token { get; set; }
         /// <summary>
         /// 
         /// </summary>
         public DateTime? refreshdate { get; set; }
    }
}