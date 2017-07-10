﻿using System;
using Jobbr.ComponentModel.JobStorage;
using Jobbr.ComponentModel.Registration;

namespace Jobbr.Storage.RavenDB
{
    public static class JobbrBuilderExtensions
    {
        public static void AddRavenDbStorage(this IJobbrBuilder builder, Action<JobbrRavenDbConfiguration> config)
        {
            var ravenConfiguration = new JobbrRavenDbConfiguration();

            config(ravenConfiguration);

            builder.Add<JobbrRavenDbConfiguration>(ravenConfiguration);

            builder.Register<IJobStorageProvider>(typeof(RavenDbStorageProvider));
            builder.Register<IConfigurationValidator>(typeof(RavenDbConfigurationValidator));
        }
    }
}