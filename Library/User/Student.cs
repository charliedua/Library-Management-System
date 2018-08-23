using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library
{
    public class Student : User
    {
        public Student(string name, string identifier) : base(name, identifier)
        {
        }
    }
}