namespace PackageDependencies
{
    public class PackageDependency
    {    
        public PackageDependency()
        {
        }

        public string GetInstallListFromDependencies(string[] packageDependencyPairs)
        {         
            var parsedPackageDependencyPair = Parse.Instance.ParsePackageDependencPair(packageDependencyPairs[0]);

            if (parsedPackageDependencyPair.NeededPackage != null)
            {
                return parsedPackageDependencyPair.NeededPackage + ", " + parsedPackageDependencyPair.MainPackage;
            }
            else
            {
                return parsedPackageDependencyPair.MainPackage;
            }                  
        }
       
    }
}