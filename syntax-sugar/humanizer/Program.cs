using System;
using Humanizer;
using static System.Console;

WriteLine(".:: Humanizer Samples ::.");

// Configurator.DateTimeHumanizeStrategy = new PrecisionDateTimeHumanizeStrategy(precision: .75);
// Configurator.DateTimeOffsetHumanizeStrategy = new PrecisionDateTimeOffsetHumanizeStrategy(precision: .75); // configure when humanizing DateTimeOffset

WriteLine($"string.Humanize: " + "PascalCaseInputStringIsTurnedIntoSentence".Humanize());
WriteLine($"string.Humanize: " + "Underscored_input_string_is_turned_into_sentence".Humanize());
WriteLine($"string.Humanize: " + "Underscored_input_String_is_turned_INTO_sentence".Humanize());
WriteLine();

WriteLine($"Truncate: " + "Lon".Truncate(6));
WriteLine($"Truncate: " + "Long text to truncate".Truncate(6, ""));
WriteLine($"Truncate: " + "Long text to truncate".Truncate(6));
WriteLine($"Truncate: " + "Long text to truncate".Truncate(6, "---"));
WriteLine($"Truncate: " + "Long text to truncate".Truncate(6, "..."));
WriteLine();

WriteLine($"DateTime.Humanize: {DateTime.UtcNow.AddDays(-1).Humanize()}");
WriteLine($"DateTime.Humanize: {DateTime.UtcNow.AddHours(-1).Humanize()}");
WriteLine($"DateTime.Humanize: {DateTime.UtcNow.Humanize()}");
WriteLine($"DateTime.Humanize: {DateTime.UtcNow.AddMinutes(10).Humanize()}");
WriteLine($"DateTime.Humanize: {DateTime.UtcNow.AddMinutes(-10).Humanize()}");
WriteLine($"DateTime.Humanize: {DateTime.UtcNow.AddMinutes(60).Humanize()}");
WriteLine($"DateTime.Humanize: {DateTime.UtcNow.AddSeconds(45).Humanize()}");
WriteLine($"DateTime.Humanize: {DateTime.UtcNow.AddSeconds(-45).Humanize()}");
WriteLine($"DateTime.Humanize: {DateTime.UtcNow.AddSeconds(46).Humanize()}");
WriteLine($"DateTime.Humanize: {DateTime.UtcNow.AddSeconds(-46).Humanize()}");
WriteLine($"DateTime.Humanize: {new DateTime(2021, 06, 01).Humanize()}");
WriteLine($"DateTime.Humanize: {new DateTime(2021, 02, 01).Humanize()}");
WriteLine();

WriteLine($"Pluralize: " + "homem".Pluralize());
WriteLine($"Pluralize: " + "homens".Pluralize());
WriteLine($"Pluralize: " + "mulher".Pluralize());
WriteLine($"Pluralize: " + "colher".Pluralize());
WriteLine($"Pluralize: " + "carro".Pluralize());

WriteLine($"ToQuantity: " + "carro".ToQuantity(1));
WriteLine($"ToQuantity: " + "carro".ToQuantity(2));

WriteLine($"Ordinalize: {1.Ordinalize()}");
WriteLine($"Ordinalize: {2.Ordinalize()}");
WriteLine($"Ordinalize: {3.Ordinalize()}");
WriteLine($"Ordinalize: {4.Ordinalize()}");
WriteLine();

WriteLine($"ToOrdinalWords: {1.ToOrdinalWords()}");
WriteLine($"ToOrdinalWords: {2.ToOrdinalWords()}");
WriteLine($"ToOrdinalWords: {3.ToOrdinalWords()}");
WriteLine($"ToOrdinalWords: {4.ToOrdinalWords()}");
WriteLine();

WriteLine($"Transform   : " + "HUMANIZER".Transform(To.LowerCase, To.TitleCase));
WriteLine($"Titleize    : " + "primeira Segunda_terceira".Titleize());
WriteLine($"Pascalize   : " + "primeira Segunda_terceira".Pascalize());
WriteLine($"Camelize    : " + "primeira Segunda_terceira".Camelize());
WriteLine($"Dasherize   : " + "primeira Segunda_terceira".Dasherize());
WriteLine($"Hyphenate   : " + "primeira Segunda_terceira".Hyphenate());
WriteLine($"Kebaberize  : " + "primeira Segunda_terceira".Kebaberize());
WriteLine();

WriteLine($"ToWords: {1.ToWords()}");
WriteLine($"ToWords: {2.ToWords()}");
WriteLine($"ToWords: {225.ToWords()}");
WriteLine($"ToWords: {2252.ToWords()}");
WriteLine($"ToWords: {2252.ToWords(GrammaticalGender.Feminine)}");
WriteLine();

WriteLine($"ToMetric: {1d.ToMetric()}");
WriteLine($"ToMetric: {1230d.ToMetric()}");
WriteLine($"ToMetric: {0.1.ToMetric()}");
WriteLine();

var fileSize = 50;
var fileSize2 = 1200;
WriteLine($"fileSize: {fileSize.Bytes().Humanize()}");
WriteLine($"fileSize: {fileSize.Kilobytes().Humanize()}");
WriteLine($"fileSize: {fileSize.Megabytes().Humanize()}");
WriteLine($"fileSize: {fileSize.Gigabytes().Humanize()}");

WriteLine($"fileSize2: {fileSize2.Bytes().Humanize()}");
WriteLine($"fileSize2: {fileSize2.Bytes().Humanize("KB")}");
WriteLine($"fileSize2: {fileSize2.Bytes().Humanize("MB")}");
WriteLine($"fileSize2: {fileSize2.Kilobytes().Humanize()}");
WriteLine($"fileSize2: {fileSize2.Megabytes().Humanize()}");
WriteLine($"fileSize2: {fileSize2.Gigabytes().Humanize()}");
WriteLine();

WriteLine($"ToHeadingArrow: {0d.ToHeadingArrow()}");
WriteLine($"ToHeadingArrow: {90d.ToHeadingArrow()}");
WriteLine($"ToHeadingArrow: {180d.ToHeadingArrow()}");
WriteLine($"ToHeadingArrow: {270d.ToHeadingArrow()}");
WriteLine();
