using System;
using System.Collections.Generic;

namespace PackageDependencies
{
    public class PackageDependency
    { 
        public string GetInstallListFromDependencies(string[] packageDependencyPairs)
        {
            var parsedPackageDependencyPair = new ParsedPackageDependencyPair();
            var packageDependencyTree = new PackageDependencyTree();

            for (int i = 0; i < packageDependencyPairs.Length; i++)
            {
                parsedPackageDependencyPair = Parse.ParsePackageDependencPair(packageDependencyPairs[i]);

                storeParsedPackageDependencyPair(packageDependencyTree, parsedPackageDependencyPair);
            }

            return PackageDependencyTree.ExtractPackageTreeInstallList(packageDependencyTree);
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

        private void storeParsedPackageDependencyPair
            (PackageDependencyTree tree, ParsedPackageDependencyPair parsedPackageDependencyPair)
        {
            PackageDependencyBranch branchMainPackage = 
                PackageDependencyTree.GetBranchInTree(tree, parsedPackageDependencyPair.MainPackage);
            PackageDependencyBranch branchNeededPackage;

            // If there is no needed package and main package is not in tree, add main package in new branch
            if (parsedPackageDependencyPair.NeededPackage == null)
            {
                if (branchMainPackage == null)
                {
                    branchMainPackage = new PackageDependencyBranch();
                    PackageDependencyBranch.AppendPackage(branchMainPackage, parsedPackageDependencyPair.MainPackage);
                    PackageDependencyTree.AppendBranch(tree, branchMainPackage);
                }
            }

            // Needed package exists
            else
            {
                branchNeededPackage = 
                    PackageDependencyTree.GetBranchInTree(tree, parsedPackageDependencyPair.NeededPackage);

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
                            PackageDependencyTree.RemoveBranch(tree, branchNeededPackage);
                            PackageDependencyTree.RemoveBranch(tree, branchNeededPackage);
                            PackageDependencyTree.InsertBranch(tree, branchNeededPackage);
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
                        PackageDependencyTree.AppendBranch(tree, branchMainPackage);
                    }
                }
            }
        }
    }
}