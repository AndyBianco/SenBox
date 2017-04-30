using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using Quartz;
using Quartz.Impl;
using SendboxBot.Models;

namespace SendboxBot
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            try {
                // Grab the Scheduler instance from the Factory and start it off
                var scheduler = StdSchedulerFactory.GetDefaultScheduler();
                scheduler.Start();

                // define the job and tie it to our ConstantCheckJob class
                var job = JobBuilder.Create<ConstantCheckJob>()
                    .WithIdentity("job1", "group1")
                    .Build();

                // Trigger the job to run now, and then repeat every 10 seconds
                var trigger = TriggerBuilder.Create()
                    .WithIdentity("trigger1", "group1")
                    .StartNow()
                    .WithSimpleSchedule(x => x
                        .WithIntervalInSeconds(350)
                        .RepeatForever())
                    .Build();

                // Tell quartz to schedule the job using our trigger
                scheduler.ScheduleJob(job, trigger);
            } catch (SchedulerException e) {
                System.Diagnostics.Debug.WriteLine(e);
            }
        }
    }
}
