using System.ComponentModel.DataAnnotations.Schema;

namespace MBRS_API.Models.ViewModels
{
    public class RatingPercentViewModel
    {
        [Column("total_number_rating_conveniences")]
        public int totalNumberRatingConveniences { get; set; }
        [Column("total_number_rating_interior")]
        public int totalNumberRatingInterior { get; set; }
        [Column("total_number_rating_employee")]
        public int totalNumberRatingEmployee { get; set; }
        [Column("total_number_rating_service")]
        public int totalNumberRatingService { get; set; }
        [Column("total_number_rating_hygiene")]
        public int totalNumberRatingHygiene { get; set; }
        [Column("total_number_rating_view")]
        public int totalNumberRatingView { get; set; }
        [Column("total_rating")]
        public int totalRating { get; set; }

        public double averageNumberRatingConveniences
        {
            get
            {
                if (totalNumberRatingConveniences != 0 && totalRating != 0)
                {
                    return Math.Round((double)(totalNumberRatingConveniences / totalRating), 1);
                }
                else
                {
                    return 0;
                }
            }
        }
        public double averageNumberRatingInterior
        {
            get
            {
                if (totalNumberRatingInterior != 0 && totalRating != 0)
                {
                    return Math.Round((double)(totalNumberRatingInterior / totalRating), 1);
                }
                else
                {
                    return 0;
                }
            }
        }
        public double averageNumberRatingEmployee
        {
            get
            {
                if (totalNumberRatingEmployee != 0 && totalRating != 0)
                {
                    return Math.Round((double)(totalNumberRatingEmployee / totalRating), 1);
                }
                else
                {
                    return 0;
                }
            }
        }
        public double averageNumberRatingService
        {
            get
            {
                if (totalNumberRatingService != 0 && totalRating != 0)
                {
                    return Math.Round((double)(totalNumberRatingService / totalRating), 1);
                }
                else
                {
                    return 0;
                }
            }
        }
        public double averageNumberRatingHygiene
        {
            get
            {
                if (totalNumberRatingHygiene != 0 && totalRating != 0)
                {
                    return Math.Round((double)(totalNumberRatingHygiene / totalRating), 1);
                }
                else
                {
                    return 0;
                }
            }
        }
        public double averageNumberRatingView
        {
            get
            {
                if (totalNumberRatingView != 0 && totalRating != 0)
                {
                    return Math.Round((double)(totalNumberRatingView / totalRating), 1);
                }
                else
                {
                    return 0;
                }
            }
        }


        public double percentNumberRatingConveniences
        {
            get
            {
                if (totalNumberRatingConveniences != 0 && totalRating != 0)
                {
                    return Math.Round((((double)(totalNumberRatingConveniences / totalRating) / 5) * 100), 1);
                }
                else
                {
                    return 0;
                }
            }
        }
        public double percentNumberRatingInterior
        {
            get
            {
                if (totalNumberRatingInterior != 0 && totalRating != 0)
                {
                    return Math.Round((((double)(totalNumberRatingInterior / totalRating) / 5) * 100), 1);
                }
                else
                {
                    return 0;
                }
            }
        }
        public double percentNumberRatingEmployee
        {
            get
            {
                if (totalNumberRatingEmployee != 0 && totalRating != 0)
                {
                    return Math.Round((((double)(totalNumberRatingEmployee / totalRating) / 5) * 100), 1);
                }
                else
                {
                    return 0;
                }
            }
        }
        public double percentNumberRatingService
        {
            get
            {
                if (totalNumberRatingService != 0 && totalRating != 0)
                {
                    return Math.Round((((double)(totalNumberRatingService / totalRating) / 5) * 100), 1);
                }
                else
                {
                    return 0;
                }
            }
        }
        public double percentNumberRatingHygiene
        {
            get
            {
                if (totalNumberRatingHygiene != 0 && totalRating != 0)
                {
                    return Math.Round((((double)(totalNumberRatingHygiene / totalRating) / 5) * 100), 1);
                }
                else
                {
                    return 0;
                }
            }
        }
        public double percentNumberRatingView
        {
            get
            {
                if (totalNumberRatingView != 0 && totalRating != 0)
                {
                    return Math.Round((((double)(totalNumberRatingView / totalRating) / 5) * 100), 1);
                }
                else
                {
                    return 0;
                }
            }
        }

    }
}
