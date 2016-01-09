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

        private void addPackageBranch(Tree tree, ParsedPackageDependencyPair parsedPackageDependencyPair)
        {
            var branch = new Branch();
            if (parsedPackageDependencyPair.NeededPackage != null)
            {
                Branch.Append(branch, parsedPackageDependencyPair.NeededPackage);
            }
            Branch.Append(branch, parsedPackageDependencyPair.MainPackage);
            Tree.Append(tree, branch);
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

            // If there is no needed package and main package is not in tree, add main package in new branch
            if (parsedPackageDependencyPair.NeededPackage == null)
            {
                if (branchMainPackage == null)
                {
                    addPackageBranch(tree, parsedPackageDependencyPair);
                }
                return;
            }

            // Needed package exists in parsed package pair
            Branch branchNeededPackage =
                Tree.GetBranch(tree, parsedPackageDependencyPair.NeededPackage);

            // If main package is not in tree:
            //  If needed package is not in tree, add branch with packages
            //  If needed package is in tree, add main package to branch of needed package
            if (branchMainPackage == null)
            {
                if (branchNeededPackage == null)
                {
                    addPackageBranch(tree, parsedPackageDependencyPair);
                }
                else
                {
                    Branch.Append(branchNeededPackage, parsedPackageDependencyPair.MainPackage);
                }
                return;
            }

            // Main package is in tree:
            //  If in same branch, check for dependency cycle
            //  If in different branches:    
            //      if needed package is not in tree, insert needed package in main branch           
            //      if needed package is in tree, move branch of needed package to before branch of main package

            if (branchMainPackage == branchNeededPackage)
            {
                checkForDependencyCycle(branchMainPackage, parsedPackageDependencyPair);
            }
            else if (branchNeededPackage == null)
            {
                Branch.Insert(branchMainPackage, parsedPackageDependencyPair.NeededPackage);
            }

            else
            {
                Tree.Remove(tree, branchNeededPackage);
                Tree.Insert(tree, branchNeededPackage);
            }
        }
    }
}