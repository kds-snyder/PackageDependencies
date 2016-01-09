using System.Collections.Generic;

namespace PackageDependencies
{
    public class Branch
    {
        private List<string> _leaves;

        public Branch()
        {
            _leaves = new List<string>();
        }

        public static void Append(Branch branch, string leaf)
        {
            branch._leaves.Add(leaf);
        }

        public static string ExtractBranchDelimitedList(Branch branch, string delimiter)
        {
            return string.Join(delimiter, branch._leaves.ToArray());                                                                    
        }

        public static int GetIndex(Branch branch, string leaf)
        {
            return branch._leaves.IndexOf(leaf);
        }

        public static void Insert(Branch branch, string leaf)
        {
            branch._leaves.Insert(0, leaf);
        }

        public static bool IsInBranch(Branch branch, string leaf)
        {
            return branch._leaves.Contains(leaf);
        }
    }
}
