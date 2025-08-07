using System;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;

namespace quartz_playground;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine(".:: Quartz Scheduler ::.");
        InicializarQuartzAsync().Wait();
    }

    public static async Task InicializarQuartzAsync()
    {
        var factory = new StdSchedulerFactory();
        var scheduler = await factory.GetScheduler();
        await scheduler.Start();

        // Jobs
        var job = JobBuilder.Create<MeuJob>()
            .WithIdentity("job1", "group1")
            .DisallowConcurrentExecution() // Impedir que seja executada uma nova instância caso a anterior ainda estiver em execução
            .Build();

        var job2 = JobBuilder.Create<MeuJob2>()
            .WithIdentity("job2", "group1")
            .Build();

        // Gatilhos
        var trigger_a_cada_2_segundos_após_inicialização = TriggerBuilder.Create()
            .WithIdentity("trigger1", "group1")
            .StartNow()
            .WithSimpleSchedule(x => x
                .WithIntervalInSeconds(2)
                .RepeatForever())
            .Build();

        var trigger_cron_expression_a_cada_3_segundos = TriggerBuilder.Create()
            .WithIdentity("trigger2", "group1")
            .WithCronSchedule("0/3 * * * * ?", x => x
                .InTimeZone(TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")))
            .Build();

        // Registrando atividades
        await scheduler.ScheduleJob(job, trigger_a_cada_2_segundos_após_inicialização);
        await scheduler.ScheduleJob(job2, trigger_cron_expression_a_cada_3_segundos);

        // Manter console application em execução
        while (true)
            await Task.Delay(TimeSpan.FromSeconds(1));
        // await scheduler.Shutdown();
    }
}

public class MeuJob : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        await Console.Out.WriteLineAsync($"Job a cada 2 segundos {DateTime.Now}, mas com 10 segundos de tempo de processamento!");
        await Task.Delay(10000);
    }
}

public class MeuJob2 : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        await Console.Out.WriteLineAsync($"Job a cada 3 segundos {DateTime.Now}!");
    }
}