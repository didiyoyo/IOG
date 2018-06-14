using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    public class travel_information
    {
        public int id { get; set; }

        public int mid { get; set; }

        public string openid { get; set; }

        public string doctorname { get; set; }

        public string isn_name { get; set; }

        public string department { get; set; }

        public string pro_title { get; set; }

        public string sex { get; set; }

        public string phone { get; set; }

        public string transportation { get; set; }

        public string Flight { get; set; }

        public string province { get; set; }

        public string city { get; set; }

        public string setout { get; set; }

        public DateTime? departuretime { get; set; }

        public DateTime? arrivaltime { get; set; }

        public string destination { get; set; }

        public string arriveprovince { get; set; }

        public string arrivecity { get; set; }

        public string remarks { get; set; }

        public string return_transportation { get; set; }
        public string return_Flight { get; set; }

        public string return_province { get; set; }

        public string return_city { get; set; }

        public string return_setout { get; set; }

        public DateTime? return_departuretime { get; set; }

        public DateTime? return_arrivaltime { get; set; }

        public string return_destination { get; set; }

        public string return_arriveprovince { get; set; }

        public string return_arrivecity { get; set; }

        public string return_remarks { get; set; }

        public string hotelname { get; set; }

        public DateTime? date_stay { get; set; }

        public DateTime? departure_date { get; set; }

        public string addres { get; set; }
    }
}
