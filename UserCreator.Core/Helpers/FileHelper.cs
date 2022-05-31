using System.Linq;

namespace UserCreator.Core.Helpers
{
    public static class FileHelper
    {
        public static string GetFileName(string fileName)
        {
            return fileName.Split("\\").Last().Split(".").Last();
        }
    }
}