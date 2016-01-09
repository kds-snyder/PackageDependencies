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

        private void handleMainAndNeededPackagesAreInDifferentBranches
            (Tree tree, Branch branchMainPackage, Branch branchNeededPackage,
                    ParsedPackageDependencyPair parsedPackageDependencyPair)
        {
            if (Tree.GetIndex(tree, branchNeededPackage) > Tree.GetIndex(tree, branchMainPackage))
            {
                Tree.Remove(tree, branchNeededPackage);
                Tree.Insert(tree, branchNeededPackage);
            }
        }

        private void handleMainPackageIsInTree(Tree tree, Branch branchMainPackage, Branch branchNeededPackage, 
            ParsedPackageDependencyPair parsedPackageDependencyPair)
        {
            if (parsedPackageDependencyPair.NeededPackage == null)
            {
                return;
            }

            if (branchNeededPackage == null)
            { 
                Branch.Insert(branchMainPackage, parsedPackageDependencyPair.NeededPackage);
            }
            else if (branchMainPackage == branchNeededPackage)
            {
                checkForDependencyCycle(branchMainPackage, parsedPackageDependencyPair);
            }
            else
            {
                handleMainAndNeededPackagesAreInDifferentBranches(tree,branchMainPackage, branchNeededPackage,
                        parsedPackageDependencyPair);
            }                        
        }

        private void handleMainPackageIsNotInTree(Tree tree, Branch branchMainPackage, Branch branchNeededPackage,
            ParsedPackageDependencyPair parsedPackageDependencyPair)
        {
            if (parsedPackageDependencyPair.NeededPackage == null || 
                branchNeededPackage == null)
            {
                addPackageBranch(tree, parsedPackageDependencyPair);
            }
        }

        private void storeParsedPackageDependencyPair
            (Tree tree, ParsedPackageDependencyPair parsedPackageDependencyPair)
        {
            Branch branchMainPackage =
                Tree.GetBranch(tree, parsedPackageDependencyPair.MainPackage);
            Branch branchNeededPackage =
                parsedPackageDependencyPair.NeededPackage == null ?
                null : Tree.GetBranch(tree, parsedPackageDependencyPair.NeededPackage);

            if (branchMainPackage != null)
            {
                handleMainPackageIsInTree(tree, branchMainPackage, branchNeededPackage, parsedPackageDependencyPair);                
            }
            else
            {
                handleMainPackageIsNotInTree(tree, branchMainPackage, branchNeededPackage, parsedPackageDependencyPair);
            } 
        }
    }
}