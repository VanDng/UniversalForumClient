using System.IO;

namespace TestProject1
{
    public static class Utility
    {
        public static string SolutionPath()
        {
            var currentDir = System.IO.Directory.GetCurrentDirectory();
            var solutionDir = currentDir;
            
            while(true)
            {
                if (Path.GetFileName(solutionDir) == "UniversalForumClient")
                {
                    break;
                }

                solutionDir = Directory.GetParent(solutionDir).FullName;
            }

            return solutionDir;
        }
    }
}