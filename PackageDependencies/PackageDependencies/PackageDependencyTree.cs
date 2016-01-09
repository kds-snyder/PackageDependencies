using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageDependencies
{
    public class PackageDependencyTree
    {
        private const string PACKAGE_INSTALL_LIST_DELIMITER = ", ";

        private List<PackageDependencyBranch> _packageDependencyTree;

        public PackageDependencyTree()
        {
            _packageDependencyTree = new List<PackageDependencyBranch>();
        }

        public static void AppendBranch(PackageDependencyTree tree, PackageDependencyBranch branch)
        {
            tree._packageDependencyTree.Add(branch);
        }

        public static string ExtractPackageTreeInstallList(PackageDependencyTree tree)
        {
            string result = "";

            //foreach (var branch in _packageDependencyTree)
            foreach (var branch in tree._packageDependencyTree)
            {
                result += PackageDependencyBranch.ExtractPackageBranchInstallList(branch, PACKAGE_INSTALL_LIST_DELIMITER)
                                                                    + PACKAGE_INSTALL_LIST_DELIMITER;
            }

            if (result.Length > PACKAGE_INSTALL_LIST_DELIMITER.Length)
            {
                result = result.Substring(0, result.Length - 2);
            }
            return result;
        }

        public static PackageDependencyBranch GetBranchInTree(PackageDependencyTree tree, string package)
        {
            foreach (var branch in tree._packageDependencyTree)
            {
                if (PackageDependencyBranch.IsInBranch(branch, package))
                {
                    return branch;
                }
            }
            return null;
        }

        public static void InsertBranch(PackageDependencyTree tree, PackageDependencyBranch branch)
        {
            tree._packageDependencyTree.Insert(0, branch);
        }

        public static void RemoveBranch(PackageDependencyTree tree, PackageDependencyBranch branch)
        {
            tree._packageDependencyTree.Remove(branch);
        }
    }
}
