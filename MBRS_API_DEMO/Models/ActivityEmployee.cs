using System.ComponentModel.DataAnnotations.Schema;

namespace MBRS_API.Models
{
    public class ActivityEmployee
    {
        [Column("activity_employee_id")]
        public int activityEmployeeID { get; set; }
        [Column("content_activity")]
        public string contentActivity { get; set; }
        [Column("employee_id")]
        public int employeeID { get; set; }
        [Column("full_name")]
        public string fullName { get; set; }
        [Column("date_time")]
        public DateTime dateTime { get; set; }

        public string convertDate
        {
            get
            {
                return dateTime.ToString("dd/MM/yyyy HH:mm");
            }
        }
    }
}
