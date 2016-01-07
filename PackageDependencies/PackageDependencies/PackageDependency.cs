using System.Collections.Generic;

namespace PackageDependencies
{
    public class PackageDependency
    {
        private const string PACKAGE_INSTALL_LIST_DELIMITER = ", ";

        private List<string> _packageDependencyList;

        public PackageDependency()
        {
            _packageDependencyList = new List<string>();
        }

        public string GetInstallListFromDependencies(string[] packageDependencyPairs)
        {           
            var parsedPackageDependencyPair = new ParsedPackageDependencyPair();

            for (int i = 0; i < packageDependencyPairs.Length; i++)
            {
                parsedPackageDependencyPair = Parse.ParsePackageDependencPair(packageDependencyPairs[i]);

                storeParsedPackageDependencyPair(parsedPackageDependencyPair);
            }

            return extractPackageInstallList();

        }

        private string extractPackageInstallList()
        {
            return string.Join(PACKAGE_INSTALL_LIST_DELIMITER, _packageDependencyList.ToArray());
        }

        private void storeParsedPackageDependencyPair(ParsedPackageDependencyPair parsedPackageDependencyPair)
        {
            if (parsedPackageDependencyPair.NeededPackage != null)
            {
                if (!_packageDependencyList.Contains(parsedPackageDependencyPair.NeededPackage))
                {
                    _packageDependencyList.Insert(0, parsedPackageDependencyPair.NeededPackage);
                }               
            }

            if (!_packageDependencyList.Contains(parsedPackageDependencyPair.MainPackage))
            {
                _packageDependencyList.Add(parsedPackageDependencyPair.MainPackage);
            }              
        }
    }
}