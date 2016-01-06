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
            return parsePackage(packageDependencies[0]);            
        }

        private string parsePackage (string packageDependency)
        {
            int indexPackageDependencyDelimiter = packageDependency.IndexOf(PACKAGE_DEPENDENCY_DELIMITER);
            return packageDependency.Substring(0, indexPackageDependencyDelimiter);
        }
    }
}