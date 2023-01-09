using System.ComponentModel.DataAnnotations.Schema;

namespace MBRS_API.Models
{
    public class TypeBike
    {
        [Column("type_bike_id")]
        public int typeBikeID { get; set; }
        [Column("type_bike_code")]
        public string typeBikeCode { get; set; }
        [Column("type_bike_name")]
        public string typeBikeName { get; set; }
        [Column("price")]
        public float price { get; set; }
        [Column("color")]
        public string color { get; set; }
        [Column("number_of_seat")]
        public int numberOfSeat { get; set; }
        [Column("list_image_id")]
        public int? listImageID { get; set; }
        [Column("total_bike")]
        public int totalBike { get; set; }
        [Column("image_base64")]
        public string? imageBase64 { get; set; }
    }
}
