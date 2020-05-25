using System.Collections.Generic;
using System.Linq;

namespace NFile.Util
{
    public static class PathUtils
    {
        public const char Separator = '\\';

        public static string Normalize(string path)
        {
            var platformIgnore = path.Replace('/', Separator);
            var stripped = platformIgnore.Trim(Separator);
            var lower = stripped.ToLower();
            return lower;
        }

        public static string GetName(string path)
        {
            var normal = Normalize(path);
            return normal.Split(Separator).LastOrDefault();
        }

        public static string Platformize(string path)
        {
            return path.Replace(Separator, System.IO.Path.DirectorySeparatorChar);
        }

        public static IEnumerable<string> Split(string path)
        {
            return path.Split(Separator);
        }

        public static string Combine(params string[] pieces)
        {
            var treated = pieces.Where(p => !string.IsNullOrWhiteSpace(p));
            return string.Join(Separator, treated);
        }
    }
}
