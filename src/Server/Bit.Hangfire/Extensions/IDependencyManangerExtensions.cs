﻿using System;
using Bit.Hangfire.Implementations;
using Bit.Hangfire.Middlewares.JobScheduler;
using Bit.Hangfire.Middlewares.JobScheduler.Implementations;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.Logging;

namespace Bit.Core.Contracts
{
    public static class IDependencyManangerExtensions
    {
        public static IDependencyManager RegisterBackgroundJobWorkerUsingDefaultConfiguration<TJobSchedulerBackendConfiguration>(this IDependencyManager dependencyManager)
            where TJobSchedulerBackendConfiguration : class, IAppEvents
        {
            if (dependencyManager == null)
                throw new ArgumentNullException(nameof(dependencyManager));

            dependencyManager.Register<ILogProvider, DefaultBackgroundJobWorkerLogProvider>();
            dependencyManager.Register<IDashboardAuthorizationFilter, DefaultJobsDashboardAuthorizationFilter>(lifeCycle: DependencyLifeCycle.SingleInstance, overwriteExciting: false);
            dependencyManager.RegisterOwinMiddleware<JobSchedulerMiddlewareConfiguration>();
            dependencyManager.RegisterAppEvents<TJobSchedulerBackendConfiguration>();
            dependencyManager.Register<IBackgroundJobWorker, DefaultBackgroundJobWorker>(lifeCycle: DependencyLifeCycle.SingleInstance);
            dependencyManager.Register<JobActivator, AutofacJobActivator>(lifeCycle: DependencyLifeCycle.SingleInstance);

            return dependencyManager;
        }
    }
}
