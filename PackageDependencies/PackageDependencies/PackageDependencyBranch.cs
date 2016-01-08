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
    }
}
