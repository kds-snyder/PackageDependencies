using System.Collections.Generic;

namespace PackageDependencies
{
    public class PackageDependency
    {
        public PackageDependency()
        {
        }

        public string GetInstallListFromDependencies(string[] packageDependencyPairs)
        {
            List<string> packageDependencyList = new List<string>();

            var parsedPackageDependencyPair = new ParsedPackageDependencyPair();

            for (int i = 0; i < packageDependencyPairs.Length; i++)
            {
                parsedPackageDependencyPair = Parse.Instance.ParsePackageDependencPair(packageDependencyPairs[i]);

                if (parsedPackageDependencyPair.NeededPackage != null)
                {
                    if (!packageDependencyList.Contains(parsedPackageDependencyPair.NeededPackage))
                    {
                        packageDependencyList.Add(parsedPackageDependencyPair.NeededPackage);
                    }                    
                }

                if (!packageDependencyList.Contains(parsedPackageDependencyPair.MainPackage))
                {
                    packageDependencyList.Add(parsedPackageDependencyPair.MainPackage);
                }
                    
            }

            return string.Join(", ", packageDependencyList.ToArray());

        }
    }
}