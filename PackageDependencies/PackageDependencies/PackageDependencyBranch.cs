using System.Collections.Generic;

namespace PackageDependencies
{
    public class PackageDependencyBranch
    {
        public List<string> Packages;

        public PackageDependencyBranch()
        {
            Packages = new List<string>();
        }

        public static void AppendPackage(PackageDependencyBranch branch, string package)
        {
            branch.Packages.Add(package);
        }

        public static string ExtractPackageBranchInstallList(PackageDependencyBranch branch, string delimiter)
        {
            return string.Join(delimiter, branch.Packages.ToArray());                                                                    
        }

        public static int GetPackageIndex(PackageDependencyBranch branch, string package)
        {
            return branch.Packages.IndexOf(package);
        }

        public static void InsertPackage(PackageDependencyBranch branch, string package)
        {
            branch.Packages.Insert(0, package);
        }

        public static bool IsInBranch(PackageDependencyBranch branch, string package)
        {
            return branch.Packages.Contains(package);
        }
    }
}
