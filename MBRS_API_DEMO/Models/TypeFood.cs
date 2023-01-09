using System.ComponentModel.DataAnnotations.Schema;

namespace MBRS_API.Models
{
    public class TypeFood
    {
        [Column("type_food_id")]
        public int typeFoodID { get; set; }
        [Column("type_food_name")]
        public string typeFoodName { get; set; }
        [Column("type_food_code")]
        public string typeFoodCode { get; set; }
    }
}
