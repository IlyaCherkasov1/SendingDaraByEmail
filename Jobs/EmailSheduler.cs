using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SendingDataByEmail.Jobs
{
    public class EmailSheduler
    {
        public static async void Start(string jobName, string timeStringMoment, int periodicitySecond)
        {

            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();

            await scheduler.Start();

            IJobDetail job = JobBuilder.Create<EmailSender>().
                WithIdentity(jobName, null).Build();

            ITrigger trigger = CreateTrigger(timeStringMoment, periodicitySecond, job);

            var jobKey = new JobKey(jobName);
            bool isExist = await scheduler.CheckExists(jobKey);
            if (isExist)
            {
                await scheduler.UnscheduleJob(trigger.Key);

                IJobDetail job1 = JobBuilder.Create<EmailSender>().
            WithIdentity(jobName, null).Build();

                 trigger = CreateTrigger(timeStringMoment, periodicitySecond, job);
            }

            await scheduler.ScheduleJob(job, trigger);
        }

        private static ITrigger CreateTrigger(string timeStringMoment, int periodicitySecond, IJobDetail job)
        {
            return TriggerBuilder.Create()
                .ForJob(job)
                .WithIdentity(job.Key.Name + "trigger")
                .StartAt(DateTimeOffset.Parse(timeStringMoment))
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(periodicitySecond)
                    .RepeatForever())
                .Build();
        }

        public static async void Delete(string jobName)
        {
            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();

            await scheduler.Start();
            await scheduler.DeleteJob(new JobKey(jobName));
        }
    }
}

