using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checker.Models
{
    public class Rules
    {
        public string File { get; set; } 
        public int Points { get; set; }

        public int Rulenumber { get; set; }
        public string Expression { get; set; }

        public override string ToString()
        {
            return "File: " + File + ", Points: " + Points + ", Rulesnumber: " + Rulenumber + ",Expression : " + Expression;
        }
    }
}
