using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniversalForumClient.Core
{
    public static class UriManager
    {
        private static string ForumNamePlaceHolder = "ForumName";
        private static string ForumPagePlaceHolder = "ForumPage";

        private static string Forum = $"forums/{ForumNamePlaceHolder}/page-{ForumPagePlaceHolder}";

        public static string ForumUri(string forumName, int forumPage)
        {
            return Forum.Replace(ForumNamePlaceHolder, forumName)
                        .Replace(ForumPagePlaceHolder, forumPage.ToString());
        }
    }
}
