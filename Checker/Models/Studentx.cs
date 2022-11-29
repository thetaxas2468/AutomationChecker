using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checker.Models
{
    public class Studentx
    {
        public string studentId { get; set; }
        public string avgGrade { get; set; }

        public string courseName { get; set; }

        public string courseYear { get; set; }

        public Studentx(string studentIda,string avgGradea,string courseNamea,string courseYeara)
        {
            studentId = studentIda;
            avgGrade = avgGradea;
            courseName = courseNamea;
            courseYear = courseYeara;
        }
    }
}
