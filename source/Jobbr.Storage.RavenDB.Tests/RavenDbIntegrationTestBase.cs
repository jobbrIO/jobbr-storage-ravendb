using Raven.Client.Document;
using Raven.Database.Config;
using Raven.Tests.Helpers;

namespace Jobbr.Storage.RavenDB.Tests
{
    public abstract class RavenDbIntegrationTestBase : RavenTestBase
    {
        protected DocumentStore Store;
        protected RavenDbStorageProvider StorageProvider;

        protected void GivenRavenDb()
        {
            // ReSharper disable once ExplicitCallerInfoArgument
            this.Store = this.NewRemoteDocumentStore(databaseName: "JobbrTests");
        }

        protected override void ModifyConfiguration(InMemoryRavenConfiguration configuration)
        {
            base.ModifyConfiguration(configuration);

            configuration.Storage.Voron.AllowOn32Bits = true;
        }

        protected void GivenStorageProvider()
        {
            this.StorageProvider = new RavenDbStorageProvider(new JobbrRavenDbConfiguration
            {
                Database = this.Store.DefaultDatabase,
                Url = this.Store.Url
            });
        }
    }
}