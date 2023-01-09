using System.ComponentModel.DataAnnotations.Schema;

namespace MBRS_API.Models.ViewModels
{
    public class FoodViewModel
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
        [Column("type_food_name")]
        public string typeFoodName { get; set; }
        [Column("image")]
        public string image { get; set; }
    }
}
