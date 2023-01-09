using System.ComponentModel.DataAnnotations.Schema;

namespace MBRS_API.Models.ViewModels
{
    public class CustomerViewModel
    {
        [Column("customer_id")]
        public int customerID { get; set; }

        [Column("full_name")]
        public string fullName { get; set; }

        [Column("phone_number")]
        public string phoneNumber { get; set; }

        [Column("day_of_birth")]
        public DateTime dateOfBirth { get; set; }

        [Column("identify_number")]
        public string identifyNumber { get; set; }

        [Column("account_id")]
        public int accountID { get; set; }

        [Column("user_name")]
        public string? userName { get; set; }

        [Column("department_id")]
        public int departmentID { get; set; }

        [Column("email")]
        public string email { get; set; }

        [Column("role")]
        public string? role { get; set; }

        [Column("Total")]
        public int Total { get; set; }

        public string? password { get; set; }

        public string dateOfBirthConvert
        {
            get
            {
                return dateOfBirth.ToString("dd/MM/yyyy");
            }
        }
        public string dateOfBirthConvertForReceoptionist
        {
            get
            {
                return dateOfBirth.ToString("yyyy-MM-dd");
            }
        }
    }
}
