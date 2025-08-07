namespace mapster_playground;

public class Person
{
    public Person(Guid id, string name, DateTime birthDate)
    {
        Id = id;
        Name = name;
        BirthDate = birthDate;
    }

    public Guid Id { get; }
    public string Name { get; }
    public DateTime BirthDate { get; }
}