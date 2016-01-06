namespace PackageDependencies
{
    public class PackageDependency
    {
        private const char PACKAGE_DEPENDENCY_DELIMITER = ':';
        private const int PACKAGE_DEPENDENCY_OFFSET = 2;

        public PackageDependency()
        {
        }

        public string GetInstallListFromDependencies(string[] packageDependencyPairs)
        {         
            var parsedPackageDependencyPair = parsePackageDependencPair(packageDependencyPairs[0]);

            if (parsedPackageDependencyPair.NeededPackage != null)
            {
                return parsedPackageDependencyPair.NeededPackage + ", " + parsedPackageDependencyPair.MainPackage;
            }
            else
            {
                return parsedPackageDependencyPair.MainPackage;
            }                  
        }

        private ParsedPackageDependencyPair parsePackageDependencPair(string packageDependencyPair)
        {
            var parsedPackageDependencyPair = new ParsedPackageDependencyPair();

            int indexPackageDependencyDelimiter = packageDependencyPair.IndexOf(PACKAGE_DEPENDENCY_DELIMITER);

            parsedPackageDependencyPair.MainPackage = packageDependencyPair.Substring(0, indexPackageDependencyDelimiter);
            if (indexPackageDependencyDelimiter + PACKAGE_DEPENDENCY_OFFSET < packageDependencyPair.Length)
            {
                parsedPackageDependencyPair.NeededPackage =
                    packageDependencyPair.Substring(indexPackageDependencyDelimiter + PACKAGE_DEPENDENCY_OFFSET);
            }

            return parsedPackageDependencyPair;
        }
    }
}