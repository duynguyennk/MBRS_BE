using System.ComponentModel.DataAnnotations.Schema;

namespace MBRS_API.Models
{
    public class FeedbackRoom
    {
        [Column("order_id")]
        public int orderID { get; set; }
        [Column("number_rating_conveniences")]
        public int numberRatingConveniences { get; set; }
        [Column("number_rating_Interior")]
        public int numberRatingInterior { get; set; }
        [Column("number_rating_employee")]
        public int numberRatingEmployee { get; set; }
        [Column("number_rating_service")]
        public int numberRatingService { get; set; }
        [Column("number_rating_hygiene")]
        public int numberRatingHygiene { get; set; }
        [Column("number_rating_view")]
        public int numberRatingView { get; set; }
        [Column("content_rating")]
        public string contentRating { get; set; }
    }
}
