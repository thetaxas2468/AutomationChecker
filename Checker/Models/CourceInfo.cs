using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checker.Models
{
    public class Course_info
    {
        public string Course_id { get; set; }
        public string Course_name { get; set; }
        public string Course_year { get; set; }
        public string Course_language { get; set; }
        public List<string> Course_students { get; set; }=new List<string>();

        public override string ToString()
        {
            string temp = "";
            foreach(var item in Course_students)
            {
                temp += item;
                temp += "|";
            }
            return "CourseId: " + Course_id + ",CourseName: " + Course_name + ",CourseYear: " + Course_year + ",CourseLanguage: " + Course_language + ",Students: " + temp;
        }
    }
}
