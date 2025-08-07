using System;
using Polly;
using static System.Console;

namespace polly_playground;

public static class Cenario1
{
    private static int _contador = 0;
    public static void Execute()
    {
        WriteLine("=====> Cenário 1 <=====");
        // Cenário: Realizamos uma consulta de notas fiscais num sistema externo que é instável, hora da erro, hora da certo.
        // No lugar de abortar o processo de nosso usuário, vamos tentar consultar 5 vezes a nota e a cada falha vamos colocar um delay em potência de 2.
        // Falha 1: Aguarda 2 segundos
        // Falha 2: Aguarda 4 segundos
        // Falha 3: Aguarda 8 segundos
        // Falha 4: Aguarda 16 segundos
        // Falha 5: Aguarda 32 segundos
        // Falha 6: Aborta e apresenta erro pro usuário

        // Criar uma política:
        // - Estamos aplicando para todos os tipos de erro ao filtrar a classe "Exception", mas poderiam ser filtrados apenas HttpException por exemplo
        // - WaitAndRetry:
        //      - Indicado para tentar 5 vezes
        //      - sleepDurationProvider: Indica o tempo para aguardar antes da próxima exceção
        //      - onRetry: logamos detalhes da falha.
        var policy = Policy
            .Handle<Exception>()
            .WaitAndRetry(5,
                sleepDurationProvider: tentativa =>
                {
                    var delay = TimeSpan.FromSeconds(Math.Pow(2, tentativa));
                    Console.WriteLine($"Tentativa {tentativa} falhou. Tentar novamente em {delay.TotalSeconds} segundos.");
                    return delay;
                },
                onRetry: (exception, delay) => Console.WriteLine($"Erro ao consultar valor da nota fiscal [{exception.GetType().Name}]: {exception.Message}"));

        try
        {
            // Para usar a politica, simplesmente chamamos o método "Execute" passando por argumento o delegate para executar nossa ação
            var valorNf = policy.Execute(() => ConsultarValorNotaFiscal());
            WriteLine();
            WriteLine($"Valor da nota fiscal: {valorNf}");
        }
        catch (Exception e)
        {
            // Se após as 5 tentativas ainda assim ocorrer erro, o Polly irá disparar a exception e você deve trata-la.
            WriteLine("Não foi possível consultar o valor da nota fiscal: " + e.Message);
        }
    }

    private static decimal ConsultarValorNotaFiscal()
    {
        _contador++;
        if (_contador <= 3)
            throw new Exception("Erro simulado");
        if (_contador <= 5)
            throw new MyCustomException("Erro simulado com exceção personalizada");
        return 150;
    }
}