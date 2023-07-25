using System;
using DasMulli.DataBuilderGenerator;
using DataBuilderGeneratorPlayground;
using static System.Console;

WriteLine(".:: Data Builder Generator ::.");

var pessoa1 = new Person("Primeiro", "Ultimo");

var pessoa2 = new PersonBuilder()
    .WithFirstName("Primeiro")
    .WithMiddleNames("Meio")
    .WithLastName("Ultimo")
    .Build();

var pessoa3 = new PersonBuilder(pessoa2)
    .WithoutMiddleNames()
    .Build();

WriteLine($"Pessoa 1: {pessoa1.GetFullName()}");
WriteLine($"Pessoa 2: {pessoa2.GetFullName()}");
WriteLine($"Pessoa 3: {pessoa3.GetFullName()}");

namespace DataBuilderGeneratorPlayground
{
    [GenerateDataBuilder]
    public class Person
    {
        public string FirstName { get; set; }
        public string? MiddleNames { get; set; } = "Initial value";
        public string LastName { get; set; }

        public Person(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public string GetFullName() => $"{FirstName} {MiddleNames} {LastName}";
    }
}
