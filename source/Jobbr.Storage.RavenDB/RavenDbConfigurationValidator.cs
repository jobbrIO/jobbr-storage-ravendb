using System;
using Jobbr.ComponentModel.Registration;

namespace Jobbr.Storage.RavenDB
{
    public class RavenDbConfigurationValidator : IConfigurationValidator
    {
        public Type ConfigurationType { get; set; } = typeof(JobbrRavenDbConfiguration);

        public bool Validate(object toValidate)
        {
            var configuration = (JobbrRavenDbConfiguration)toValidate;

            if (string.IsNullOrWhiteSpace(configuration.Url))
            {
                throw new InvalidOperationException("Please specify an Url in your RavenDB configuration");
            }

            if (string.IsNullOrWhiteSpace(configuration.Database))
            {
                throw new InvalidOperationException("Please specify a Database in your RavenDB configuration.");
            }

            return true;
        }
    }
}