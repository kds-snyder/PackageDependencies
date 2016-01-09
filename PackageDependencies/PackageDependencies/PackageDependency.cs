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

            return extractPackageInstallList();

        }

        private void checkForDependencyCycle(PackageDependencyBranch branch,
                                ParsedPackageDependencyPair parsedPackageDependencyPair)
        {
            int indexMainPackage = 0;
            int indexNeededPackage = 0;

            if (parsedPackageDependencyPair.NeededPackage != null)
            {
                indexNeededPackage =
                 branch.Packages.IndexOf(parsedPackageDependencyPair.NeededPackage);
            }

            indexMainPackage =
                branch.Packages.IndexOf(parsedPackageDependencyPair.MainPackage);

            if (indexNeededPackage > indexMainPackage)
            {
                throw new Exception("The input package dependencies cause a dependency cycle");
            }
        }

        private string extractPackageInstallList()
        {
            string result = "";

            foreach (var branch in _packageDependencyTree)
            {
                result += string.Join(PACKAGE_INSTALL_LIST_DELIMITER, branch.Packages.ToArray())
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
                if (branch.Packages.Contains(package))
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
                    branchMainPackage.Packages.Add(parsedPackageDependencyPair.MainPackage);
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
                        branchNeededPackage.Packages.Add(parsedPackageDependencyPair.MainPackage);
                    }
                }

                // Needed package is not in tree:
                //  If main package is in tree, insert needed package in main package branch
                //  If main package is not in tree, add needed and main packages in new branch
                else
                {
                    if (branchMainPackage != null)
                    {
                        branchMainPackage.Packages.Insert(0, parsedPackageDependencyPair.NeededPackage);
                    }
                    else
                    {
                        branchMainPackage = new PackageDependencyBranch();
                        branchMainPackage.Packages.Add(parsedPackageDependencyPair.NeededPackage);
                        branchMainPackage.Packages.Add(parsedPackageDependencyPair.MainPackage);
                        _packageDependencyTree.Add(branchMainPackage);
                    }
                }
            }
        }
    }
}