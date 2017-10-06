using System.Reflection;
using Jobbr.DevSupport.ReferencedVersionAsserter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jobbr.Storage.RavenDB.Tests
{
    [TestClass]
    public class PackagingTests
    {
        private readonly bool isPre = Assembly.GetExecutingAssembly().GetInformalVersion().Contains("-");

        [TestMethod]
        public void Feature_NuSpec_IsCompilant()
        {
            var asserter = new Asserter(Asserter.ResolvePackagesConfig("Jobbr.Storage.RavenDB"), Asserter.ResolveRootFile("Jobbr.Storage.RavenDB.nuspec"));

            asserter.Add(new PackageExistsInBothRule("Jobbr.ComponentModel.Registration"));
            asserter.Add(new PackageExistsInBothRule("Jobbr.ComponentModel.JobStorage"));

            asserter.Add(new AllowNonBreakingChangesRule("RavenDb.Client*"));

            asserter.Add(new VersionIsIncludedInRange("Jobbr.ComponentModel.*"));

            asserter.Add(new NoMajorChangesInNuSpec("Jobbr.*"));
            asserter.Add(new NoMajorChangesInNuSpec("RavenDb.Client*"));

            var result = asserter.Validate();

            Assert.IsTrue(result.IsSuccessful, result.Message);
        }
    }
}
