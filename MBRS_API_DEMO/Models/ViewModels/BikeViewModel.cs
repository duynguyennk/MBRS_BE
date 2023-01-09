using System.ComponentModel.DataAnnotations.Schema;

namespace MBRS_API.Models.ViewModels
{
    public class BikeViewModel
    {
        [Column("bike_id")]
        public int bikeID { get; set; }
        [Column("type_bike_id")]
        public int typeBikeID { get; set; }
        [Column("bike_code")]
        public string bikeCode { get; set; }
        [Column("bike_name")]
        public string bikeName { get; set; }
        [Column("type_bike_name")]
        public string typeBikeName { get; set; }
        [Column("price")]
        public float price { get; set; }
        [Column("number_of_seat")]
        public int numberOfSeat { get; set; }
        [Column("color")]
        public string color { get; set; }
    }
}
