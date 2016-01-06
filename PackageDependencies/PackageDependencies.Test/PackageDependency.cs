using System;

namespace PackageDependencies.Test
{
    internal class PackageDependency
    {
        public PackageDependency()
        {
        }

        internal string GetInstallListFromDependencies(string[] v)
        {
            int indexPackageDependencyDelimiter = v[0].IndexOf(":");
            return v[0].Substring(0, indexPackageDependencyDelimiter);
        }
    }
}