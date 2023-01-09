using System.ComponentModel.DataAnnotations.Schema;

namespace MBRS_API.Models
{
    public class CarAirport
    {
        [Column("car_airport_id")]
        public int carAirportID { get; set; }
        [Column("car_airport_name")]
        public string carAirportName { get; set; }
        [Column("car_airport_code")]
        public string carAirportCode { get; set; }
        [Column("type_car_airport_id")]
        public int typeCarAirportID { get; set; }
        [Column("identify_car_number")]
        public string identifyCarNumber { get; set; }
        [Column("color")]
        public string? color { get; set; }
        [Column("image")]
        public string? image { get; set; }
    }
}
