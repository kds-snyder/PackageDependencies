using System;

namespace PackageDependencies.Test
{
    public class PackageDependency
    {
        public PackageDependency()
        {
        }

        public string GetInstallListFromDependencies(string[] v)
        {
            int indexPackageDependencyDelimiter = v[0].IndexOf(":");
            return v[0].Substring(0, indexPackageDependencyDelimiter);
        }
    }
}