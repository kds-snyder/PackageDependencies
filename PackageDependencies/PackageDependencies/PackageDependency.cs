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
            string neededPackage;

            string mainPackage = parsePackage(packageDependencies[0], out neededPackage);

            if (neededPackage != null)
            {
                return neededPackage + ", " + mainPackage;
            }
            else
            {
                return mainPackage;
            }                  
        }

        private string parsePackage (string packageDependency, out string neededPackage)
        {
            int indexPackageDependencyDelimiter = packageDependency.IndexOf(PACKAGE_DEPENDENCY_DELIMITER);
            if (indexPackageDependencyDelimiter + 2 < packageDependency.Length)
            {
                neededPackage = packageDependency.Substring(indexPackageDependencyDelimiter + 2);
            }
            else
            {
                neededPackage = null;
            }
            return packageDependency.Substring(0, indexPackageDependencyDelimiter);
        }
    }
}