using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoublyLinkedListApp
{
    class Person
    {
        public Person(string lastname, uint height, DateTime birthdate)
        {
            LastName = lastname;
            Height = height;
            BirthDate = birthdate;
        }
        public string LastName { get; set; }
        public uint Height { get; set; }
        public DateTime BirthDate { get; set; }
        public Person Prev { get; set; }
        public Person Next { get; set; }

        public void Output()
        {
            Console.Write(LastName + " ");
            Console.Write(Height + " ");
            Console.WriteLine(BirthDate.Day + "." + BirthDate.Month + "." + BirthDate.Year);
        }
    }
}
