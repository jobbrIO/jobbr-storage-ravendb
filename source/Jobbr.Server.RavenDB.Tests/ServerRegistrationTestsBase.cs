using Jobbr.ComponentModel.Registration;
using Jobbr.Server.Builder;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jobbr.Server.RavenDB.Tests
{
    [TestClass]
    public class ServerRegistrationTestsBase : RavenDbIntegrationTestBase
    {
        [TestMethod]
        public void RegisteredAsComponent_JobbrIsStarted_ProviderHasCorrectType()
        {
            GivenRavenDb();
            var builder = new JobbrBuilder();
            builder.Register<IJobbrComponent>(typeof(ExposeStorageProvider));

            builder.AddRavenDbStorage(config =>
            {
                config.Database = Store.DefaultDatabase;
                config.Url = Store.Url;
            });

            builder.Create();

            Assert.AreEqual(typeof(RavenDbStorageProvider), ExposeStorageProvider.Instance.JobStorageProvider.GetType());
        }

        [TestMethod]
        public void RegisteredAsComponent_WithBasicConfiguration_DoesStart()
        {
            GivenRavenDb();

            var builder = new JobbrBuilder();
            builder.Register<IJobbrComponent>(typeof(ExposeStorageProvider));

            builder.AddRavenDbStorage(config =>
            {
                config.Database = Store.DefaultDatabase;
                config.Url = Store.Url;
            });

            using (var server = builder.Create())
            {
                server.Start();

                Assert.AreEqual(JobbrState.Running, server.State, "Server should be possible to start with a proper configuration.");
            }
        }
    }
}
