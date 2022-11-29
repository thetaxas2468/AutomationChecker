using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checker.Models
{
    public class Student
    {
        public int Id { get; set; }
        public int Grade { get; set; }
        public Course_info CourseInfo { get; set; }

        public string HomeworkId { get; set; }

        public string HomeworkName { get; set; }

        public override string ToString()
        {
            return "Id : " + Id + "|Grade : "+Grade+"| HomeworkId : "+HomeworkId+"|Homeworkname : "+HomeworkName;
        }
    }
}
