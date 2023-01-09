using System.ComponentModel.DataAnnotations.Schema;

namespace MBRS_API.Models
{
    public class TypeCarAirport
    {
        [Column("type_car_airport_id")]
        public int typeCarAirportID { get; set; }
        [Column("type_car_airport_code")]
        public string typeCarAirportCode { get; set; }
        [Column("type_car_airport_name")]
        public string typeCarAirportName { get; set; }
        [Column("price")]
        public float price { get; set; }
        [Column("number_of_seat")]
        public int numberOfSeat { get; set; }
        [Column("color")]
        public string color { get; set; }
    }
}
