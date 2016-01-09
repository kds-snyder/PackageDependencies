using System;
using System.Collections.Generic;

namespace PackageDependencies
{
    public class PackageDependency
    { 
        public string GetInstallListFromDependencies(string[] packageDependencyPairs)
        {
            var parsedPackageDependencyPair = new ParsedPackageDependencyPair();
            var packageDependencyTree = new Tree();

            for (int i = 0; i < packageDependencyPairs.Length; i++)
            {
                parsedPackageDependencyPair = Parse.ParsePackageDependencPair(packageDependencyPairs[i]);

                storeParsedPackageDependencyPair(packageDependencyTree, parsedPackageDependencyPair);
            }

            return Tree.ExtractTreeDelimitedList(packageDependencyTree);
        }

        private void checkForDependencyCycle(Branch branch,
                                ParsedPackageDependencyPair parsedPackageDependencyPair)
        {
            int indexMainPackage =
                Branch.GetIndex(branch, parsedPackageDependencyPair.MainPackage);

            int indexNeededPackage = 0;

            if (parsedPackageDependencyPair.NeededPackage != null)
            {
                indexNeededPackage =
                 Branch.GetIndex(branch, parsedPackageDependencyPair.NeededPackage);
            }            

            if (indexNeededPackage > indexMainPackage)
            {
                throw new Exception("The input package dependencies cause a dependency cycle");
            }
        }        

        private void storeParsedPackageDependencyPair
            (Tree tree, ParsedPackageDependencyPair parsedPackageDependencyPair)
        {
            Branch branchMainPackage = 
                Tree.GetBranch(tree, parsedPackageDependencyPair.MainPackage);
            Branch branchNeededPackage;

            // If there is no needed package and main package is not in tree, add main package in new branch
            if (parsedPackageDependencyPair.NeededPackage == null)
            {
                if (branchMainPackage == null)
                {
                    branchMainPackage = new Branch();
                    Branch.Append(branchMainPackage, parsedPackageDependencyPair.MainPackage);
                    Tree.Append(tree, branchMainPackage);
                }
            }

            // Needed package exists
            else
            {
                branchNeededPackage = 
                    Tree.GetBranch(tree, parsedPackageDependencyPair.NeededPackage);

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
                            Tree.Remove(tree, branchNeededPackage);                            
                            Tree.Insert(tree, branchNeededPackage);
                        }
                    }
                    // Main package is not in tree; add it to needed package branch
                    else
                    {
                        Branch.Append(branchNeededPackage, parsedPackageDependencyPair.MainPackage);
                    }
                }

                // Needed package is not in tree:
                //  If main package is in tree, insert needed package in main package branch
                //  If main package is not in tree, add needed and main packages in new branch
                else
                {
                    if (branchMainPackage != null)
                    {
                        Branch.Insert(branchMainPackage, parsedPackageDependencyPair.NeededPackage);
                    }
                    else
                    {
                        branchMainPackage = new Branch();
                        Branch.Append(branchMainPackage, parsedPackageDependencyPair.NeededPackage);
                        Branch.Append(branchMainPackage, parsedPackageDependencyPair.MainPackage);
                        Tree.Append(tree, branchMainPackage);
                    }
                }
            }
        }
    }
}