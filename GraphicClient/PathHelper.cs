using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicClient
{
    internal static class PathHelper
    {
        private static readonly char[] _pathSeparators = { '/', '\\' };
        internal static string GetParent(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }
            var index = path.LastIndexOfAny(_pathSeparators);
            return index == -1 ? @"\" : path.Substring(0, index);
        }
    }
}
