using FluentBuilder;

namespace FluentBuilderPlayground;

[AutoGenerateBuilder]
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

[AutoGenerateBuilder]
public class User
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime? Date { get; set; }

    [FluentBuilderIgnore] // Add this attribute to ignore this property when generating a FluentBuilder.
    public int Age { get; set; }

    public int Answer { get; set; } = 42; // When a default value is set, this value is also set as default in the FluentBuilder.
}