using System;
using Humanizer;
using Humanizer.Configuration;
using Humanizer.DateTimeHumanizeStrategy;
using static System.Console;

WriteLine(".:: Humanizer Samples ::.");

// Configurator.DateTimeHumanizeStrategy = new PrecisionDateTimeHumanizeStrategy(precision: .75);
// Configurator.DateTimeOffsetHumanizeStrategy = new PrecisionDateTimeOffsetHumanizeStrategy(precision: .75); // configure when humanizing DateTimeOffset

WriteLine("PascalCaseInputStringIsTurnedIntoSentence".Humanize());
WriteLine("Underscored_input_string_is_turned_into_sentence".Humanize());
WriteLine("Underscored_input_String_is_turned_INTO_sentence".Humanize());
WriteLine("HUMANIZER".Transform(To.LowerCase, To.TitleCase));
WriteLine("Lon".Truncate(6));
WriteLine("Long text to truncate".Truncate(6, ""));
WriteLine("Long text to truncate".Truncate(6));
WriteLine("Long text to truncate".Truncate(6, "---"));
WriteLine("Long text to truncate".Truncate(6, "..."));
WriteLine(DateTime.UtcNow.AddDays(-1).Humanize());
WriteLine(DateTime.UtcNow.AddHours(-1).Humanize());
WriteLine(DateTime.UtcNow.Humanize());
WriteLine(DateTime.UtcNow.AddMinutes(10).Humanize());
WriteLine(DateTime.UtcNow.AddMinutes(-10).Humanize());
WriteLine(DateTime.UtcNow.AddMinutes(60).Humanize());

WriteLine(DateTime.UtcNow.AddSeconds(45).Humanize());
WriteLine(DateTime.UtcNow.AddSeconds(-45).Humanize());
WriteLine(DateTime.UtcNow.AddSeconds(46).Humanize());
WriteLine(DateTime.UtcNow.AddSeconds(-46).Humanize());
WriteLine(new DateTime(2021, 06, 01).Humanize());
WriteLine(new DateTime(2021, 02, 01).Humanize());
WriteLine(DateTime.UtcNow.ToOrdinalWords());

WriteLine("homem".Pluralize());
WriteLine("homens".Pluralize());
WriteLine("mulher".Pluralize());
WriteLine("colher".Pluralize());
WriteLine("carro".Pluralize());

WriteLine("carro".ToQuantity(1));
WriteLine("carro".ToQuantity(2));
WriteLine($"Ordinalize: {1.Ordinalize()}");
WriteLine($"Ordinalize: {2.Ordinalize()}");
WriteLine($"Ordinalize: {3.Ordinalize()}");
WriteLine($"Ordinalize: {4.Ordinalize()}");
WriteLine($"ToOrdinalWords: {1.ToOrdinalWords()}");
WriteLine($"ToOrdinalWords: {2.ToOrdinalWords()}");
WriteLine($"ToOrdinalWords: {3.ToOrdinalWords()}");
WriteLine($"ToOrdinalWords: {4.ToOrdinalWords()}");

WriteLine("fáBio Monteiro_naspolini".Titleize());
WriteLine("fáBio Monteiro_naspolini".Pascalize());
WriteLine("fáBio Monteiro_naspolini".Camelize());
WriteLine("fáBio Monteiro_naspolini".Dasherize());
WriteLine("fáBio Monteiro_naspolini".Hyphenate());
WriteLine("fáBio Monteiro_naspolini".Kebaberize());

WriteLine(1.ToWords());
WriteLine(2.ToWords());
WriteLine(225.ToWords());
WriteLine(2252.ToWords());
WriteLine(2252.ToWords(GrammaticalGender.Feminine));

WriteLine(1d.ToMetric());
WriteLine(1230d.ToMetric());
WriteLine(0.1.ToMetric());

var fileSize = 50;
var fileSize2 = 1200;
WriteLine(fileSize.Bytes().Humanize());
WriteLine(fileSize.Kilobytes().Humanize());
WriteLine(fileSize.Megabytes().Humanize());
WriteLine(fileSize.Gigabytes().Humanize());

WriteLine(fileSize2.Bytes().Humanize());
WriteLine(fileSize2.Bytes().Humanize("KB"));
WriteLine(fileSize2.Bytes().Humanize("MB"));
WriteLine(fileSize2.Kilobytes().Humanize());
WriteLine(fileSize2.Megabytes().Humanize());
WriteLine(fileSize2.Gigabytes().Humanize());

WriteLine(0d.ToHeadingArrow());
WriteLine(90d.ToHeadingArrow());
WriteLine(180d.ToHeadingArrow());
WriteLine(270d.ToHeadingArrow());
