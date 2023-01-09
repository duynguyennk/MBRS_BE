using System.ComponentModel.DataAnnotations.Schema;

namespace MBRS_API.Models
{
    public class FeedbackService
    {
        [Column("order_id")]
        public int orderID { get; set; }
        [Column("rating_number")]
        public int ratingNumber { get; set; }
        [Column("content_rating")]
        public string contentRating { get; set; }
        public int selectedOption { get; set; }
    }
}
