using System.ComponentModel.DataAnnotations.Schema;

namespace MBRS_API.Models
{
    public class Floor
    {
        [Column("floor_id")]
        public int floorID { get; set; }
        [Column("floor_code")]
        public string floorCode { get; set; }
        [Column("number_floor")]
        public int numberFloor { get; set; }
        [Column("floor_name")]
        public string floorName { get; set; }

    }
}
