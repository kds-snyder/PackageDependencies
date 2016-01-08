using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

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
        public void TwoPackagesOneDependencyReturnsCorrectOrder()
        {
            // Arrange
            var packageDependency = new PackageDependency();

            // Act  
            string installList = packageDependency.GetInstallListFromDependencies
                (new string[] { "NLog.Config: NLog", "NLog: " });

            // Assert
            Assert.AreEqual(installList, "NLog, NLog.Config");
        }

        [TestMethod]
        public void ThreePackagesOneDependencyReturnsCorrectOrder()
        {
            // Arrange
            var packageDependency = new PackageDependency();

            // Act  
            string installList = packageDependency.GetInstallListFromDependencies
                (new string[] {"NLog.Config: NLog", "EntityFramework: ", "NLog: "});

            // Assert
            Assert.AreEqual(installList, "NLog, NLog.Config, EntityFramework");
        }

        [TestMethod]
        public void ThreePackagesTwoDependenciesReturnsCorrectOrder()
        {
            // Arrange
            var packageDependency = new PackageDependency();

            // Act  
            string installList = packageDependency.GetInstallListFromDependencies
                (new string[] { "NLog.Web: NLog.Test", "NLog.Test: NLog.HTTP","NLog.HTTP: ", "NLog.Web: " });

            // Assert
            Assert.AreEqual(installList, "NLog.HTTP, NLog.Test, NLog.Web");
        }

        [TestMethod]
        public void FourPackagesThreeDependenciesMixedOrderReturnsCorrectOrder()
        {
            // Arrange
            var packageDependency = new PackageDependency();

            // Act  
            string installList = packageDependency.GetInstallListFromDependencies(new string[]
                                    { "NLog.HTTP: NLog.Test", "NLog.Config: ",
                                        "NLog.Test: NLog.Web", "NLog.Web: NLog.Config"});            

            // Assert
            Assert.AreEqual(installList, "NLog.Config, NLog.Web, NLog.Test, NLog.HTTP");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception),
         "The input package dependencies cause a dependency cycle")]
        public void TwoPackagesDependencyCycleThrowsException()
        {
            // Arrange
            var packageDependency = new PackageDependency();

            // Act  
            string installList = packageDependency.GetInstallListFromDependencies(new string[]
                                    { "NLog.Config: NLog.Test", "NLog.Test: NLog.Config" });

        }       
    }
    
}
