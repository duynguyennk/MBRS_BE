using System.ComponentModel.DataAnnotations.Schema;

namespace MBRS_API.Models.ViewModels
{
    public class CarAirportViewModel
    {
        [Column("car_airport_id")]
        public int carAirportID { get; set; }
        [Column("car_airport_code")]
        public string carAirportCode { get; set; }
        [Column("type_car_airport_name")]
        public string typeCarAirportName { get; set; }
        [Column("car_airport_name")]
        public string carAirportName { get; set; }
        [Column("type_car_airport_id")]
        public int typeCarAirportID { get; set; }
        [Column("identify_car_number")]
        public string identifyCarNumber { get; set; }
        [Column("number_of_seat")]
        public string numberOfSeat { get; set; }
        [Column("color")]
        public string color { get; set; }
        [Column("image")]
        public string image { get; set; }
        [Column("price")]
        public float price { get; set; }
    }
}
