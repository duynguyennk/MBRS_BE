using System.ComponentModel.DataAnnotations.Schema;

namespace MBRS_API.Models
{
    public class Food
    {
        [Column("food_id")]
        public int foodID { get; set; }
        [Column("food_name")]
        public string foodName { get; set; }
        [Column("food_code")]
        public string foodCode { get; set; }
        [Column("price")]
        public float price { get; set; }
        [Column("quantity")]
        public int quantity { get; set; }
        [Column("type_food_id")]
        public int typeFoodID { get; set; }
        [Column("image_base64")]
        public string? imageBase64 { get; set; }
    }
}
