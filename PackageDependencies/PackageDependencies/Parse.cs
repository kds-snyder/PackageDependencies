namespace PackageDependencies
{
    public class Parse
    {
        private const char PACKAGE_DEPENDENCY_DELIMITER = ':';
        private const int PACKAGE_DEPENDENCY_OFFSET = 2;
       
        public static ParsedPackageDependencyPair ParsePackageDependencPair(string packageDependencyPair)
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
