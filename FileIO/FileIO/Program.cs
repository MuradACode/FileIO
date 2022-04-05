using FileIO.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace FileIO
{
    class Program
    {
        static void Main(string[] args)
        {
            StringBuilder rootPath = new StringBuilder(Directory.GetCurrentDirectory());
            string rootDir = rootPath.Remove(42, 23).ToString();
            Menu();
        }
        public static void Menu()
        {
        Menu:
            Console.WriteLine("\n----------------------------------------\nChoose the command:\n1: See all people\n2: Create a person\n3: Remove person\n0: Exit\n----------------------------------------");
            string command = GetStringInput("command");
            switch (command)
            {
                case "1":
                    GetAllPeople();
                    goto Menu;
                case "2":
                    CreatePerson();
                    goto Menu;
                case "3":
                    Thread thread = new Thread(RemovePerson);
                    thread.Start();
                    goto Menu;
                case "0":
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("Invalid input try again!");
                    goto Menu;
            }
        }
        public static void GetAllPeople()
        {
            Console.Clear();
            StringBuilder rootPath = new StringBuilder(Directory.GetCurrentDirectory());
            string rootDir = rootPath.Remove(42, 23).ToString();
            IEnumerable<string> peopleJson = Read(rootDir);
            List<Person> people = new List<Person>();
            foreach (var itemJson in peopleJson)
            {
                Person person = JsonSerializer.Deserialize<Person>(itemJson);
                people.Add(person);
            }
            foreach (Person person in people)
            {
                Console.WriteLine(person.ToString());
            }
            Console.WriteLine("\n----------------------------------------\n");
        }
        public static string GetStringInput(string output)
        {
        TryAgain:
            Console.Write($"Enter the {output}: ");
            string input = Console.ReadLine();
            if (String.IsNullOrEmpty(input))
            {
                Console.Clear();
                Console.WriteLine("Input can't be empty!, Try again");
                goto TryAgain;
            }
            return input;
        }
        public static void CreatePerson()
        {
            Console.Clear();
            string name = GetStringInput("name").Trim();
            string surname = GetStringInput("surname").Trim();
            Person person = new Person(name, surname);
            string jsonPerson = JsonSerializer.Serialize(person);
            StringBuilder rootPath = new StringBuilder(Directory.GetCurrentDirectory());
            string rootDir = rootPath.Remove(42, 23).ToString();
            Write(rootDir, jsonPerson);
            Console.WriteLine("----------------------------------------\nPerson created successfully\n----------------------------------------");
        }
        public static void RemovePerson()
        {
            Console.Clear();
            StringBuilder rootPath = new StringBuilder(Directory.GetCurrentDirectory());
            string rootDir = rootPath.Remove(42, 23).ToString();
            IEnumerable<string> peopleJson = Read(rootDir);
            List<Person> people = new List<Person>();
            foreach (var itemJson in peopleJson)
            {
                Person person = JsonSerializer.Deserialize<Person>(itemJson);
                people.Add(person);
            }
            string deletingPerson = GetStringInput("person who you want to delete");
            people.Remove(people.Find(n => n.Name == deletingPerson.Trim()));
            string contentDir = Path.Combine(rootDir, "Content");
            string fileDir = Path.Combine(contentDir, "people.txt");
            File.Create(fileDir);
            foreach (Person person in people)
            {
                string jsonPerson = JsonSerializer.Serialize(person);
                using (var textWriter = new StreamWriter(fileDir, true))
                {
                    textWriter.WriteLine(jsonPerson);
                }
            }
            Console.WriteLine("----------------------------------------\nPerson deleted successfully\n----------------------------------------");
        }
        public static string CreateFile(string rootDir)
        {
            string contentDir = Path.Combine(rootDir, "Content");

            if (!Directory.Exists(contentDir))
            {
                Directory.CreateDirectory(contentDir);
            }
            string fileDir = Path.Combine(contentDir, "people.txt");

            if (!File.Exists(fileDir))
            {
                File.Create(fileDir);
            }
            return fileDir;
        }
        public static void Write(string rootDir, string text)
        {
            string fileDir = CreateFile(rootDir);
            using (var textWriter = new StreamWriter(fileDir, true))
            {
                textWriter.WriteLine(text);
            }
        }
        public static IEnumerable<string> Read(string rootDir)
        {
            string fileDir = CreateFile(rootDir);
            IEnumerable<string> lines = File.ReadLines(fileDir);
            return lines;
        }
    }
}
