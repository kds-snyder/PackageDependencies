using System;
using System.Collections.Generic;

namespace PackageDependencies
{
    public class PackageDependency
    {
        private const string PACKAGE_INSTALL_LIST_DELIMITER = ", ";

        private List<PackageDependencyBranch> _packageDependencyTree;

        private PackageDependencyBranch _packageDependencyBranch;

        public PackageDependency()
        {
            _packageDependencyTree = new List<PackageDependencyBranch>();
            _packageDependencyBranch = new PackageDependencyBranch();
            _packageDependencyTree.Add(_packageDependencyBranch);
        }

        public string GetInstallListFromDependencies(string[] packageDependencyPairs)
        {
            var parsedPackageDependencyPair = new ParsedPackageDependencyPair();

            for (int i = 0; i < packageDependencyPairs.Length; i++)
            {
                parsedPackageDependencyPair = Parse.ParsePackageDependencPair(packageDependencyPairs[i]);

                storeParsedPackageDependencyPair(parsedPackageDependencyPair);

                checkForDependencyCycle(parsedPackageDependencyPair);

            }

            return extractPackageInstallList();

        }

        private void checkForDependencyCycle(ParsedPackageDependencyPair parsedPackageDependencyPair)
        {
            int indexMainPackage = 0;
            int indexNeededPackage = 0;

            if (parsedPackageDependencyPair.NeededPackage != null)
            {
                indexNeededPackage =
                 _packageDependencyBranch.Packages.IndexOf(parsedPackageDependencyPair.NeededPackage);
            }

            indexMainPackage =
                _packageDependencyBranch.Packages.IndexOf(parsedPackageDependencyPair.MainPackage);

            if (indexNeededPackage > indexMainPackage)
            {
                throw new Exception("The input package dependencies cause a dependency cycle");
            }
        }

        private string extractPackageInstallList()
        {
            return string.Join(PACKAGE_INSTALL_LIST_DELIMITER, _packageDependencyBranch.Packages.ToArray());
        }       

        private void storeParsedPackageDependencyPair(ParsedPackageDependencyPair parsedPackageDependencyPair)
        {
            if (parsedPackageDependencyPair.NeededPackage != null)
            {
                if (!_packageDependencyBranch.Packages.Contains(parsedPackageDependencyPair.NeededPackage))
                {
                    _packageDependencyBranch.Packages.Insert(0, parsedPackageDependencyPair.NeededPackage);
                }
            }

            if (!_packageDependencyBranch.Packages.Contains(parsedPackageDependencyPair.MainPackage))
            {
                _packageDependencyBranch.Packages.Add(parsedPackageDependencyPair.MainPackage);
            }

        }
    }
}