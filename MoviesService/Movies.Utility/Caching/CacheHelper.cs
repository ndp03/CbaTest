using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Utility.Caching
{
    public static class CacheHelper
    {
        public static string GetCacheKey(string declaringTypeName, string methodName, params object[] values)
        {
            return String.Format("{0}_{1}_{2}", declaringTypeName, methodName, String.Join("_", values));
        }

        public static string GetCacheKey(MethodBase methodBase, params object[] values)
        {
            return GetCacheKey(methodBase.DeclaringType != null ? methodBase.DeclaringType.FullName : null, methodBase.Name, values);
        }
    }
}
