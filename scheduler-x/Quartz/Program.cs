using System.Threading.Tasks;
using System;
using Quartz;
using Quartz.Impl;

namespace Quartz_Sample
{
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
                .Build();

            var job2 = JobBuilder.Create<MeuJob2>()
                .WithIdentity("job2", "group1")
                .Build();

            // Gatilhos
            var trigger_a_cada_2_segundos = TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(2)
                    .RepeatForever())
                .Build();

            var trigger_a_cada_minuto = TriggerBuilder.Create()
                .WithIdentity("trigger2", "group1")
                .WithCronSchedule("0 0/1 * * * ?", x => x
                    .InTimeZone(TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")))
                .Build();

            // Registrando atividades
            await scheduler.ScheduleJob(job, trigger_a_cada_2_segundos);
            await scheduler.ScheduleJob(job2, trigger_a_cada_minuto);

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
            await Console.Out.WriteLineAsync($"Job recorrente {DateTime.Now}!");
        }
    }

    public class MeuJob2 : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            await Console.Out.WriteLineAsync($"Job agendado {DateTime.Now}!");
        }
    }
}
