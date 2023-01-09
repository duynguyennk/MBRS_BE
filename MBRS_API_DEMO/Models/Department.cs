using System.ComponentModel.DataAnnotations.Schema;

namespace MBRS_API.Models
{
    public class Department
    {
        [Column("department_id")]
        public int departmentID { get; set; }

        [Column("department_code")]
        public string departmentCode { get; set; }

        [Column("department_name")]
        public string departmentName { get; set; }
    }
}
