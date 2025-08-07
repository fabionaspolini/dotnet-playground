using System.Globalization;
using Maestria.Extensions.FluentCast;
using static System.Console;

WriteLine(".:: Maestria Fluent Cast ::.");

// Possibilidade de configuração de cultura padrão para número e data
Maestria.Extensions.FluentCast.GlobalSettings.Configure(cfg => cfg
    .NumberCulture(CultureInfo.InvariantCulture)
    .DateTimeCulture(CultureInfo.GetCultureInfo("pt-BR")));

// Métodos de conversão de tipos primitivos inline:
// - ToBoolean()
// - ToByteArray()
// - ToDateTime()
// - ToDecimal()
// - ToDouble()
// - ToEnum<T>()
// - ToFloat()
// - ToGuid()
// - ToInt16()
// - ToInt32()
// - ToInt64()
// - ToString()
// - ToTimeSpan()
// Todas estas rotinas não são safe, ou seja, no caso de exception é abortado o processo.
// Mas para todas existe o overload com o sufixo Safe() para chamadas que não não disparam exceções.
// Os métodos Safes permitem a recepção do valor padrão em caso de falha, quando não há este agumente o resultado pe um Nullable<T> com valor null, quando há o argumento, o resultado já é o tipo primitivo direto.


WriteLine("153".ToInt32());
WriteLine("abcd".ToInt32Safe().ToStringSafe() ?? "null"); // Retorna null
WriteLine("abcd".ToInt32Safe(-1)); // Retorna -1

WriteLine("21/06/2021".ToDateTime()); // Cultura padrão de data pt-BR
WriteLine("2021-06-22".ToDateTime(CultureInfo.InvariantCulture)); // Mas há suporte para informar outra cultura

WriteLine("39:15:10".ToTimeSpan());
WriteLine("158a4c56-f353-4e3c-9afd-2654caf6d2ef".ToGuid().GetType().FullName);

WriteLine(((object) null).ToStringSafe() ?? "Não ocorrerá exceção!");