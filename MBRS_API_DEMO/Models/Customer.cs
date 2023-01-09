using System.ComponentModel.DataAnnotations.Schema;

namespace MBRS_API.Models
{
    public class Customer
    {
        [Column("customer_id")]
        public int customerID { get; set; }

        [Column("full_name")]
        public string fullName { get; set; }

        [Column("phone_number")]
        public string phoneNumber { get; set; }

        [Column("day_of_birth")]
        public string dateOfBirth { get; set; }

        [Column("identify_number")]
        public string identifyNumber { get; set; }

        [Column("account_id")]
        public int accountID { get; set; }

        [Column("user_name")]
        public string userName { get; set; }

        [Column("email")]
        public string email { get; set; }

        [Column("department_name")]
        public string departmentName { get; set; }

    }
}
