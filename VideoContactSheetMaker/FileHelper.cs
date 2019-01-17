using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace VideoContactSheetMaker
{
    static class FileHelper
    {
        public static readonly string[] VideoExtensions = { "mp4", "wmv", "avi", "mkv", "flv", "mov", "mpg", "mpeg", "m4v", "asf", "f4v", "webm", "divx", "m2t", "m2ts", "vob" };
        public static List<string> GetFilesRecursive(string initial, bool recursive)
        {
            try
            {
                var files = Directory.EnumerateFiles(initial).Where(f => VideoExtensions.Any(x => f.EndsWith(x, StringComparison.OrdinalIgnoreCase)));
                if (recursive)
                    files = files.Concat(Directory.EnumerateDirectories(initial)
                        .SelectMany(d => GetFilesRecursive(d, recursive)));
                return files.ToList();
            }
            catch
            {
                return new List<string>();
            }
        }
    }
}
