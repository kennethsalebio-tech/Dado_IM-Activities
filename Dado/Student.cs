using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD
{
    public class Student
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Course { get; set; }
        public int Year { get; set; }
        public string CreatedAt { get; set; }
        public string FullName => FirstName + " " + LastName;
    }

}
