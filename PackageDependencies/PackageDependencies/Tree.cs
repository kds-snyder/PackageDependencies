using System.Collections.Generic;

namespace PackageDependencies
{
    public class Tree
    {
        private const string TREE_LIST_DELIMITER = ", ";

        private List<Branch> _branches;

        public Tree()
        {
            _branches = new List<Branch>();
        }

        public static void Append(Tree tree, Branch branch)
        {
            tree._branches.Add(branch);
        }

        public static string ExtractTreeDelimitedList(Tree tree)
        {
            string result = "";

            //foreach (var branch in _branches)
            foreach (var branch in tree._branches)
            {
                result += Branch.ExtractBranchDelimitedList(branch, TREE_LIST_DELIMITER)
                                                                    + TREE_LIST_DELIMITER;
            }

            if (result.Length > TREE_LIST_DELIMITER.Length)
            {
                result = result.Substring(0, result.Length - 2);
            }
            return result;
        }

        public static Branch GetBranch(Tree tree, string package)
        {
            foreach (var branch in tree._branches)
            {
                if (Branch.IsInBranch(branch, package))
                {
                    return branch;
                }
            }
            return null;
        }

        public static int GetIndex(Tree tree, Branch branch)
        {
            return tree._branches.IndexOf(branch);
        }

        public static void Insert(Tree tree, Branch branch)
        {
            tree._branches.Insert(0, branch);
        }

        public static void Remove(Tree tree, Branch branch)
        {
            tree._branches.Remove(branch);
        }
    }
}
