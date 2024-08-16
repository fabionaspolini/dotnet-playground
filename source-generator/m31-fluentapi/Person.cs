using M31.FluentApi.Attributes;
//var person = Create

[FluentApi]
public class Person
{
    [FluentMember(0)]
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