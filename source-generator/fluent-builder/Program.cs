using System;
using FluentBuilder;
using FluentBuilderPlayground;

//var user = new FluentBuilderPlayground.UserBuilder()
//    .WithFirstName("Test")
//    .WithLastName("User")
//    .Build();
//Console.WriteLine($"{user.FirstName} {user.LastName}");

var person = new PersonBuilder()
    .WithFirstName("Fábio")
    .WithLastName("Naspolini")
    .Build(true);
Console.WriteLine(person.GetFullName());