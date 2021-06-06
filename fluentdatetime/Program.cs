using System;
using System.Globalization;
using System.Threading;
using FluentDate;
using FluentDateTime;
using static System.Console;

var culture = new CultureInfo("pt-BR", false);
culture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
culture.DateTimeFormat.LongTimePattern = "HH:mm:ss.fff";
Thread.CurrentThread.CurrentCulture = culture;
Console.WriteLine(DateTime.Now);

var date = new DateTime(2021, 05, 10, 10, 15, 27, 555);

WriteLine(".:: FluentDateTime Samples ::.");
WriteLine($"Data atual (Now)    : {DateTime.Now}");
WriteLine($"Data base para teste: {date}");
WriteLine();

WriteLine(".:: Navegação de período aterando a hora para 00:00:00 ou 23:59:59.999 ::.");
WriteLine($"BeginningOfDay      : {date.BeginningOfDay()}");
WriteLine($"EndOfDay            : {date.EndOfDay()}");
WriteLine($"BeginningOfWeek     : {date.BeginningOfWeek()}");
WriteLine($"EndOfWeek           : {date.EndOfWeek()}");
WriteLine($"BeginningOfMonth    : {date.BeginningOfMonth()}");
WriteLine($"EndOfMonth          : {date.EndOfMonth()}");
WriteLine($"BeginningOfQuarter  : {date.BeginningOfQuarter()}");
WriteLine($"EndOfQuarter        : {date.EndOfQuarter()}");
WriteLine($"BeginningOfYear     : {date.BeginningOfYear()}");
WriteLine($"EndOfYear           : {date.EndOfYear()}");
WriteLine();

WriteLine(".:: Inicio / Fim de período preservando a hora atual ::.");
WriteLine($"PreviousDay         : {date.PreviousDay()}");
WriteLine($"NextDay             : {date.NextDay()}");
WriteLine($"Previous Sunday     : {date.Previous(DayOfWeek.Sunday)}");
WriteLine($"Next Sunday         : {date.Next(DayOfWeek.Sunday)}");
WriteLine();

WriteLine($"Add 1 BusinessDays  : {date.AddBusinessDays(1)}");
WriteLine($"Add 10 BusinessDays : {date.AddBusinessDays(10)}");
WriteLine();

WriteLine($"FirstDayOfWeek      : {date.FirstDayOfWeek()}");
WriteLine($"LastDayOfWeek       : {date.LastDayOfWeek()}");
WriteLine($"FirstDayOfMonth     : {date.FirstDayOfMonth()}");
WriteLine($"LastDayOfMonth      : {date.LastDayOfMonth()}");
WriteLine($"FirstDayOfQuarter   : {date.FirstDayOfQuarter()}");
WriteLine($"LastDayOfQuarter    : {date.LastDayOfQuarter()}");
WriteLine($"FirstDayOfYear      : {date.FirstDayOfYear()}");
WriteLine($"LastDayOfYear       : {date.LastDayOfYear()}");
WriteLine();

WriteLine(".:: Navegação relativa ::.");
WriteLine($"Ago - 2 dias atrás do momento atual     : {2.Days().Ago()}");
WriteLine($"Ago - 2 dias atrás de 'date' fluent     : {2.Days().Ago(date)}");
WriteLine($"Ago - 2 dias atrás de 'date' expressão  : {date - 2.Days()}");

WriteLine($"Since - 2 dias após 'date' fluent       : {2.Days().Since(date)}");
WriteLine($"Since - 2 dias após 'date' expressão    : {date + 2.Days()}");
WriteLine("");

WriteLine(".:: Outros ::.");
WriteLine($"SetTime                 : {date.SetTime(10, 0, 0, 0)}");
WriteLine($"At (Igual SetTime)      : {date.At(10, 0, 0, 0)}");
WriteLine($"SetDay                  : {date.SetDay(6)}");
WriteLine($"SetMonth                : {date.SetMonth(1)}");
WriteLine($"Midnight (Meia noite)   : {date.Midnight()}");
WriteLine($"Noon (Meio dia)         : {date.Noon()}");
WriteLine($"IsInFuture              : {date.IsInFuture()}");
WriteLine($"IsInPast                : {date.IsInPast()}");
WriteLine($"SameDay(DateTime.Now)   : {date.SameDay(DateTime.Now)}");
WriteLine($"SameMonth(DateTime.Now) : {date.SameMonth(DateTime.Now)}");
WriteLine($"SameYear(DateTime.Now)  : {date.SameYear(DateTime.Now)}");
WriteLine("");
