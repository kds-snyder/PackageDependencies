﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        public void FourPackagesTwoDependenciesMixedOrderReturnsCorrectOrder()
        {
            // Arrange
            var packageDependency = new PackageDependency();

            // Act  
            string installList = packageDependency.GetInstallListFromDependencies(new string[]
                                    { "NLog.Config: ", "NLog.Test: NLog.Web", "EntityFramework: ",
                                         "NLog.Web: NLog.Config"});

            // Assert
            Assert.AreEqual(installList, "NLog.Config, NLog.Web, NLog.Test, EntityFramework");
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
        public void FivePackagesNoDependenciesReturnsCorrectOrder()
        {
            // Arrange
            var packageDependency = new PackageDependency();

            // Act  
            string installList = packageDependency.GetInstallListFromDependencies(new string[]
                                    { "NLog: ", "EntityFramework: ", "AspNet.WebAPI: ", "Owin: ", "Ice: "});

            // Assert
            Assert.AreEqual(installList, "NLog, EntityFramework, AspNet.WebAPI, Owin, Ice");
        }

        [TestMethod]
        public void FivePackagesSomeDependenciesReturnsCorrectOrder()
        {
            // Arrange
            var packageDependency = new PackageDependency();

            // Act  
            string installList = packageDependency.GetInstallListFromDependencies(new string[]
                                    {"Owin: ", "NLog.Config: NLog", "Owin.Identity: Owin", "NLog: ",
                                     "NLog.Web: NLog.Config"});

            // Assert
            Assert.AreEqual(installList, "Owin, Owin.Identity, NLog, NLog.Config, NLog.Web");
        }

        [TestMethod]
        public void SixPackagesFourDependenciesMixedOrderReturnsCorrectOrde()
        {
            // Arrange
            var packageDependency = new PackageDependency();

            // Act  
            string installList = packageDependency.GetInstallListFromDependencies(new string[]
                                    {"KittenService: ", "Leetmeme: Cyberportal", "Cyberportal: Ice",
                                                "CamelCaser: KittenService","Fraudstream: Leetmeme", "Ice: "});

            // Assert
            Assert.AreEqual(installList, "KittenService, CamelCaser, Ice, Cyberportal, Leetmeme, Fraudstream");
        }

        [TestMethod]
        public void SeveralPackagesSeveralDependenciesMixedOrderReturnsCorrectOrde()
        {
            // Arrange
            var packageDependency = new PackageDependency();

            // Act  
            string installList = packageDependency.GetInstallListFromDependencies(new string[]
                                    {"Owin: ", "EntityFramework: ", "Identity.EntityFramework: EntityFramework",
                                     "Owin.Identity: Owin","AspNet.WebAPI: AspNet.HTTP", "WebClient: ",
                                     "AspNet.HTTP: WebClient", "NLog.HTTP: NLog.Web", "NLog.Config: ",
                                     "NLog.Test: NLog.HTTP", "NLog.Web: NLog.Config"});

            // Assert
            Assert.AreEqual(installList,
             "NLog.Config, WebClient, Owin, Owin.Identity, EntityFramework, Identity.EntityFramework, AspNet.HTTP, AspNet.WebAPI, NLog.Web, NLog.HTTP, NLog.Test");
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

        [TestMethod]
        [ExpectedException(typeof(Exception),
         "The input package dependencies cause a dependency cycle")]
        public void FourPackagesDependencyCycleThrowsException()
        {
            // Arrange
            var packageDependency = new PackageDependency();

            // Act  
            string installList = packageDependency.GetInstallListFromDependencies(new string[]
                                    { "NLog.Config: NLog.Test", "NLog.Test: NLog.Web" ,
                                        "NLog.Web: NLog.HTTP", "NLog.HTTP: NLog.Config" });

        }

        [TestMethod]
        [ExpectedException(typeof(Exception),
             "The input package dependencies cause a dependency cycle")]
        public void SeveralPackagesDependencyCycle()
        {
            // Arrange
            var packageDependency = new PackageDependency();

            // Act  
            string installList = packageDependency.GetInstallListFromDependencies(new string[]
                                    { "KittenService: ", "Leetmeme: Cyberportal", "Cyberportal: Ice",
                                "CamelCaser: KittenService","Fraudstream: ","Ice: Leetmeme" });
        }

        [TestMethod]
        [ExpectedException(typeof(Exception),
         "The input package dependencies are not in the correct format")]
        public void InputFormatIncorrectThrowsException()
        {
            // Arrange
            var packageDependency = new PackageDependency();

            // Act  
            string installList = packageDependency.GetInstallListFromDependencies(new string[]
                                    { "NLog.Config", "NLog.Test: NLog.Config" });
        }
    }
    
}
