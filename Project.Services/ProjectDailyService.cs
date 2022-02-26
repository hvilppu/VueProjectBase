using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using Quartz.Logging;
using Quartz.Spi;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace project.Services
{
    /// <summary>
    /// https://www.quartz-scheduler.net/documentation/index.html
    /// </summary>
    public class ProjectDailyService
    {
        readonly IServiceProvider _serviceProvider;
        public ProjectDailyService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Run()
        {      
            RunProgramRunExample().GetAwaiter().GetResult();
        }

        private async Task RunProgramRunExample()
        {
            try
            {
                // Grab the Scheduler instance from the Factory
                NameValueCollection props = new NameValueCollection
                {
                    { "quartz.serializer.type", "binary" }
                };
                StdSchedulerFactory factory = new StdSchedulerFactory(props);
                IScheduler scheduler = await factory.GetScheduler();
                var jobFactory = new JobFactory(_serviceProvider);
                scheduler.JobFactory = jobFactory;
                // and start it off
                await scheduler.Start();

                IJobDetail job = JobBuilder.Create<ProjectDailyJob>()
                    .WithIdentity("ProjectDailyJob", "ProjectGroup")
                    .Build();

               //// Trigger the job to run now, and then repeat every 100 seconds
               //ITrigger trigger = TriggerBuilder.Create()
               //    .WithIdentity("PoolsTrigger", "PoolsGroup")
               //    .StartNow()
               //    .WithSimpleSchedule(x => x
               //        .WithIntervalInSeconds(100)
               //        .RepeatForever())
               //    .Build();

                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity("ProjectTrigger", "ProjectGroup")
                    .StartAt(DateBuilder.TomorrowAt(0, 0, 0)) //Always next day
                    .WithSimpleSchedule(x => x
                        .WithIntervalInHours(24) //Day after that... forever 
                        .RepeatForever())
                    .Build();

                // Tell quartz to schedule the job using our trigger
                await scheduler.ScheduleJob(job, trigger);
                //scheduler.ListenerManager.AddJobListener(new PoolsJobListener(), KeyMatcher<JobKey>.KeyEquals(new JobKey("PoolsDailyJob", "PoolsGroup")));
                //await scheduler.Shutdown();
            }
            catch (SchedulerException se)
            {
                Console.WriteLine(se);
            }
        }
    }

    public class PoolsJobListener : IJobListener
    {
        public string Name => "ProjectJobListener";

        public Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            return null;
        }

        public Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            return null;
        }

        public Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException, CancellationToken cancellationToken = default)
        {
            return null;
        }
    }

    class JobFactory : IJobFactory
    {
        protected readonly IServiceProvider _serviceProvider;

        protected readonly ConcurrentDictionary<IJob, IServiceScope> _scopes = new ConcurrentDictionary<IJob, IServiceScope>();

        public JobFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IJob GetNewJob()
        {
            return (_serviceProvider.GetService(typeof(ProjectDailyJob)) as IJob);
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            return _serviceProvider.GetService(typeof(ProjectDailyJob)) as IJob;
        }

        public void ReturnJob(IJob job)
        {
            // i couldn't find a way to release services with your preferred DI, 
            // its up to you to google such things

            var disposable = job as IDisposable;
            disposable?.Dispose();
        }
    }
}
