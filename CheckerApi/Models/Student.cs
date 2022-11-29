using System.ComponentModel.DataAnnotations.Schema;

namespace CheckerApi.Models
{
    public class Student
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string StudentId { get; set; }
        public string AvgGrade { get; set; }
        public string CourseName { get; set; }
        public string CourseYear { get; set; }

    }
}
