using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PackageDependencies.Test
{
    [TestClass]
    public class PackageDependencyTests
    {
        [TestMethod]
        public void SinglePackageReturnsPackage()
        {
            // Arrange
            var packageDependency = new PackageDependency();

            // Act   
            string installList = packageDependency.GetInstallListFromDependencies(new string[] { "NLog: " });

            // Assert
            Assert.AreEqual(installList, "NLog");
        }

        [TestMethod]
        public void TwoDependentPairsReturnsCorrectOrder()
        {
            // Arrange
            var packageDependency = new PackageDependency();

            // Act  
            string installList = packageDependency.GetInstallListFromDependencies
                (new string[] { "NLog.Config: NLog", "NLog: " });

            // Assert
            Assert.AreEqual(installList, "NLog, NLog.Config");
        }
    }
}
