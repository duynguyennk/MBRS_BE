using System.ComponentModel.DataAnnotations.Schema;

namespace MBRS_API.Models
{
    public class TypeRoom
    {
        [Column("type_room_id")]
        public int typeRoomID { get; set; }
        [Column("type_room_code")]
        public string typeRoomCode { get; set; }
        [Column("type_room_name")]
        public string typeRoomName { get; set; }
        [Column("number_of_child")]
        public int numberOfChild { get; set; }
        [Column("number_of_bed")]
        public int numberOfBed { get; set; }
        [Column("number_of_bedroom")]
        public int numberOfBedroom { get; set; }
        [Column("number_of_adult")]
        public int numberOfAdult { get; set; }
        [Column("number_of_view")]
        public int numberOfView { get; set; }
        [Column("number_of_adding_bed")]
        public int numberOfAddingBed { get; set; }
        [Column("number_of_bath_room")]
        public int numberOfBathRoom { get; set; }
        [Column("price")]
        public float price { get; set; }
        [Column("content_Introduce_Room")]
        public string contentIntroduceRoom { get; set; }
        [Column("room_square")]
        public float roomSquare { get; set; }
        [Column("list_image_id")]
        public int listImageID { get; set; }
        [Column("list_utilities_id")]
        public string? listUtilitiesID { get; set; }

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

        [Column("safety_box")]
        public Boolean? safetyBox { get; set; }
        [Column("dryer")]
        public Boolean? dryer { get; set; }
        [Column("wifi")]
        public Boolean? wifi { get; set; }
        [Column("iron")]
        public Boolean? iron { get; set; }
        [Column("tivi")]
        public Boolean? tivi { get; set; }
        [Column("fridge")]
        public Boolean? fridge { get; set; }
        [Column("heater_bath")]
        public Boolean? heaterBath { get; set; }
        [Column("bathtub")]
        public Boolean? bathTub { get; set; }
        [Column("air_condition")]
        public Boolean? airCondition { get; set; }
        [Column("heater_room")]
        public Boolean? heaterRoom { get; set; }
        [Column("dinner_table")]
        public Boolean? dinnerTable { get; set; }
    }
}


