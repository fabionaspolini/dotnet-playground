using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangFeatures_Sample
{
    static class Tuplas
    {
        public static void Execute()
        {
            var dic = new Dictionary<(string topic, string queue), int>();
            dic.Add((topic: "teste", queue: "1"), 1);
            dic.Add((topic: "teste", queue: "2"), 2);
            dic.Add((topic: "teste", queue: "3"), 3);
            dic.Add((topic: "teste2", queue: "1"), 21);
            dic.Add((topic: "teste2", queue: "2"), 22);
            dic.Add((topic: "teste2", queue: "3"), 23);

            if (dic.TryGetValue((topic: "teste", queue: "2"), out var teste))
                Console.WriteLine("1. Existe");

            if (!dic.TryGetValue((topic: "teste2", queue: "155"), out var teste2))
                Console.WriteLine("2. Não existe");

            Console.WriteLine($"Teste: {teste}");
            Console.WriteLine($"Teste2: {teste2}");
        }
    }
}
