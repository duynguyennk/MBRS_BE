using System.ComponentModel.DataAnnotations.Schema;

namespace MBRS_API.Models.ViewModels
{
    public class TypeRoomViewModel
    {
        [Column("type_room_id")]
        public int typeRoomID { get; set; }
        [Column("type_room_name")]
        public string typeRoomName { get; set; }
        [Column("number_of_bed")]
        public int numberOfBed { get; set; }
        [Column("number_of_adult")]
        public int numberOfAdult { get; set; }
        [Column("number_of_child")]
        public int numberOfChild { get; set; }
        [Column("number_of_view")]
        public int numberOfView { get; set; }
        [Column("number_of_bath_room")]
        public int numberOfBathRoom { get; set; }
        [Column("price")]
        public float price { get; set; }
        [Column("list_image_id")]
        public int listImageID { get; set; }
        [Column("list_utilities_id")]
        public int listUtilitiesID { get; set; }
        [Column("total_room")]
        public int totalRoom { get; set; }
        [Column("first_image_base64")]
        public string? firstImageBase64 { get; set; }
        [Column("second_image_base64")]
        public string? secondImageBase64 { get; set; }
        [Column("third_image_base64")]
        public string? thirdImageBase64 { get; set; }
        [Column("fourth_image_base64")]
        public string? fourthImageBase64 { get; set; }
        [Column("fifth_image_base64")]
        public string? fifthImageBase64 { get; set; }
    }
}
