using System.IO;

namespace TestProject1
{
    public static class Utility
    {
        public static string SolutionPath()
        {
            var currentDir = System.IO.Directory.GetCurrentDirectory();
            var solutionDir = string.Empty;
            
            while(true)
            {
                solutionDir = currentDir;

                if (Path.GetFileName(solutionDir) == "UniverlForumClient")
                {
                    break;
                }

                solutionDir = Directory.GetParent(solutionDir).FullName;
            }

            return solutionDir;
        }
    }
}