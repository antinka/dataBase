using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    interface ISerializable
    {
        List<Employee> Read();
        void Write(List<Employee> employees);
    }
}
