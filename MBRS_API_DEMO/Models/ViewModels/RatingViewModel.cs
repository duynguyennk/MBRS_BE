using System.ComponentModel.DataAnnotations.Schema;

namespace MBRS_API.Models.ViewModels
{
    public class RatingViewModel
    {
        [Column("rating_id")]
        public int ratingID { get; set; }
        [Column("type_room_id")]
        public int typeRoomID { get; set; }
        [Column("customer_id")]
        public int customerID { get; set; }
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
        [Column("full_name")]
        public string fullName { get; set; }
        [Column("date_time")]
        public string dateTime { get; set; }
        [Column("content_rating")]
        public string contentRating { get; set; }
        public double totalRating
        {
            get { return Math.Round((double)(numberRatingConveniences+ numberRatingInterior+ numberRatingEmployee+ numberRatingService+ numberRatingHygiene+ numberRatingView)/6,1); }
        }
    }
}
