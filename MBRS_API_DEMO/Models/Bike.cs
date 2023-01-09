using System.ComponentModel.DataAnnotations.Schema;

namespace MBRS_API.Models
{
    public class Bike
    {
        [Column("bike_id")]
        public int bikeID { get; set; }
        [Column("type_bike_id")]
        public int typeBikeID { get; set; }
        [Column("bike_code")]
        public string bikeCode { get; set; }
        [Column("bike_name")]
        public string bikeName { get; set; }
    }
}
