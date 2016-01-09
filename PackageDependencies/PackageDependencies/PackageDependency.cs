using System;
using System.Collections.Generic;

namespace PackageDependencies
{
    public class PackageDependency
    {
        private const string PACKAGE_INSTALL_LIST_DELIMITER = ", ";

        private List<PackageDependencyBranch> _packageDependencyTree;

        public PackageDependency()
        {
            _packageDependencyTree = new List<PackageDependencyBranch>();
        }

        public string GetInstallListFromDependencies(string[] packageDependencyPairs)
        {
            var parsedPackageDependencyPair = new ParsedPackageDependencyPair();

            for (int i = 0; i < packageDependencyPairs.Length; i++)
            {
                parsedPackageDependencyPair = Parse.ParsePackageDependencPair(packageDependencyPairs[i]);

                storeParsedPackageDependencyPair(parsedPackageDependencyPair);

            }

            return extractPackageTreeInstallList();
        }

        private void checkForDependencyCycle(PackageDependencyBranch branch,
                                ParsedPackageDependencyPair parsedPackageDependencyPair)
        {
            int indexMainPackage =
                PackageDependencyBranch.GetPackageIndex(branch, parsedPackageDependencyPair.MainPackage);

            int indexNeededPackage = 0;

            if (parsedPackageDependencyPair.NeededPackage != null)
            {
                indexNeededPackage =
                 PackageDependencyBranch.GetPackageIndex(branch, parsedPackageDependencyPair.NeededPackage);
            }            

            if (indexNeededPackage > indexMainPackage)
            {
                throw new Exception("The input package dependencies cause a dependency cycle");
            }
        }

        private string extractPackageTreeInstallList()
        {
            string result = "";

            foreach (var branch in _packageDependencyTree)
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

        public PackageDependencyBranch getBranchInTree(string package)
        {
            foreach (var branch in _packageDependencyTree)
            {
                if (PackageDependencyBranch.IsInBranch(branch, package))
                {
                    return branch;
                }
            }
            return null;
        }

        private void storeParsedPackageDependencyPair(ParsedPackageDependencyPair parsedPackageDependencyPair)
        {
            PackageDependencyBranch branchMainPackage = getBranchInTree(parsedPackageDependencyPair.MainPackage);
            PackageDependencyBranch branchNeededPackage;

            // If there is no needed package and main package is not in tree, add main package in new branch
            if (parsedPackageDependencyPair.NeededPackage == null)
            {
                if (branchMainPackage == null)
                {
                    branchMainPackage = new PackageDependencyBranch();
                    PackageDependencyBranch.AppendPackage(branchMainPackage, parsedPackageDependencyPair.MainPackage);
                    _packageDependencyTree.Add(branchMainPackage);
                }
            }

            // Needed package exists
            else
            {
                branchNeededPackage = getBranchInTree(parsedPackageDependencyPair.NeededPackage);

                // If needed package and main package are in tree:
                //  If in same branch, check for dependency cycle
                //  If in different branches, move branch of needed package to before branch of main package
                if (branchNeededPackage != null)
                {
                    if (branchMainPackage != null)
                    {
                        if (branchMainPackage == branchNeededPackage)
                        {
                            checkForDependencyCycle(branchMainPackage, parsedPackageDependencyPair);
                        }
                        else
                        {
                            _packageDependencyTree.Remove(branchNeededPackage);
                            _packageDependencyTree.Insert(0, branchNeededPackage);
                        }
                    }
                    // Main package is not in tree; add it to needed package branch
                    else
                    {
                        PackageDependencyBranch.AppendPackage
                            (branchNeededPackage, parsedPackageDependencyPair.MainPackage);
                    }
                }

                // Needed package is not in tree:
                //  If main package is in tree, insert needed package in main package branch
                //  If main package is not in tree, add needed and main packages in new branch
                else
                {
                    if (branchMainPackage != null)
                    {
                        PackageDependencyBranch.InsertPackage
                            (branchMainPackage, parsedPackageDependencyPair.NeededPackage);
                    }
                    else
                    {
                        branchMainPackage = new PackageDependencyBranch();
                        PackageDependencyBranch.AppendPackage
                            (branchMainPackage, parsedPackageDependencyPair.NeededPackage);
                        PackageDependencyBranch.AppendPackage
                            (branchMainPackage, parsedPackageDependencyPair.MainPackage);
                        _packageDependencyTree.Add(branchMainPackage);
                    }
                }
            }
        }
    }
}