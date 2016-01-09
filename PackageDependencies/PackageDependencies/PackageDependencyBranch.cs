using System.Collections.Generic;

namespace PackageDependencies
{
    public class PackageDependencyBranch
    {
        private List<string> _packages;

        public PackageDependencyBranch()
        {
            _packages = new List<string>();
        }

        public static void AppendPackage(PackageDependencyBranch branch, string package)
        {
            branch._packages.Add(package);
        }

        public static string ExtractPackageBranchInstallList(PackageDependencyBranch branch, string delimiter)
        {
            return string.Join(delimiter, branch._packages.ToArray());                                                                    
        }

        public static int GetPackageIndex(PackageDependencyBranch branch, string package)
        {
            return branch._packages.IndexOf(package);
        }

        public static void InsertPackage(PackageDependencyBranch branch, string package)
        {
            branch._packages.Insert(0, package);
        }

        public static bool IsInBranch(PackageDependencyBranch branch, string package)
        {
            return branch._packages.Contains(package);
        }
    }
}
