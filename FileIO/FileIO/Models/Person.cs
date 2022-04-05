using System;
using System.Collections.Generic;
using System.Text;

namespace FileIO.Models
{
    class Person
    {
        private static int _idCounter;
        public Guid Guid { get; private set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        static Person()
        {
            _idCounter = 0;
        }
        public Person()
        {

        }
        private Person(string plug)
        {
            Id = ++_idCounter;
            Guid = Guid.NewGuid();
        }
        public Person(string name, string surname):this("")
        {
            Name = name;
            Surname = surname;
        }
        public override string ToString()
        {
            return $"Person {Id}:\nName: {Name}, Surname: {Surname}\n";
        }
    }
}
