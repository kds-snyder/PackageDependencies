namespace PackageDependencies
{
    public class PackageDependency
    {
        private const char PACKAGE_DEPENDENCY_DELIMITER = ':';

        public PackageDependency()
        {
        }

        public string GetInstallListFromDependencies(string[] packageDependencies)
        {         
            var parsedPackageDependencyPair = parsePackage(packageDependencies[0]);

            if (parsedPackageDependencyPair.NeededPackage != null)
            {
                return parsedPackageDependencyPair.NeededPackage + ", " + parsedPackageDependencyPair.MainPackage;
            }
            else
            {
                return parsedPackageDependencyPair.MainPackage;
            }                  
        }

        private ParsedPackageDependencyPair parsePackage (string packageDependency)
        {
            var parsedPackageDependencyPair = new ParsedPackageDependencyPair();

            int indexPackageDependencyDelimiter = packageDependency.IndexOf(PACKAGE_DEPENDENCY_DELIMITER);

            parsedPackageDependencyPair.MainPackage = packageDependency.Substring(0, indexPackageDependencyDelimiter);
            if (indexPackageDependencyDelimiter + 2 < packageDependency.Length)
            {
                parsedPackageDependencyPair.NeededPackage = 
                    packageDependency.Substring(indexPackageDependencyDelimiter + 2);
            }

            return parsedPackageDependencyPair;
        }
    }
}