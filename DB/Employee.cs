using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    [Serializable]
    class Employee
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int ID { get; set; }
        public Employee()
        {
        }
        public Employee(string firstName, string lastName,int id)
        {
            FirstName = firstName;
            LastName = lastName;
            ID = id;
        }
        public override string ToString()
        {
            string str = ID + " " + FirstName + " " + LastName + " ";
            return str;
        }
    }
}
