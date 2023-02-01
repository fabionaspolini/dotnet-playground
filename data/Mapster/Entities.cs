using System;

namespace Mapster_Sample;

public class Person
{
    public Person(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
}