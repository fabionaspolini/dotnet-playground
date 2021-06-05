using System;
using FluentDate;
using FluentDateTime;

Console.WriteLine(".:: FluentDateTime Sample ::.");
Console.WriteLine(DateTime.Now.PreviousDay());
Console.WriteLine(DateTime.Now.BeginningOfDay());
Console.WriteLine(DateTime.Now.EndOfDay());

Console.WriteLine(DateTime.Now.BeginningOfDay().Previous(DayOfWeek.Sunday));
Console.WriteLine(DateTime.Now.EndOfDay().Previous(DayOfWeek.Sunday));

Console.WriteLine(DateTime.Now.AddBusinessDays(1));
Console.WriteLine(DateTime.Now.AddBusinessDays(10));

Console.WriteLine(DateTime.Now.LastDayOfWeek());
Console.WriteLine(DateTime.Now.LastDayOfWeek().Next(DayOfWeek.Sunday).EndOfDay());

Console.WriteLine("BeginningOfWeek: " + DateTime.Now.BeginningOfWeek());
Console.WriteLine("EndOfWeek: " + DateTime.Now.EndOfWeek());

Console.WriteLine(DateTime.Now.StartOfWeek());