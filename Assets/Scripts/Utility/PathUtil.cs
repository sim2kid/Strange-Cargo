using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public static class PathUtil
    {
        public static string SanitizePath(string s)
        {
            return s.Replace('/', '\\');
        }
        public static string ForwardSlashPath(string s)
        {
            return s.Replace('\\', '/');
        }
    }
}