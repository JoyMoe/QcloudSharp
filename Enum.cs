using System;
using System.Reflection;

namespace QcloudSharp
{
    [Obsolete("QcloudSharp.Enum is deprecated, please use QcloudSharp.Enums.",true)]
    public static class Enum : Enums
    {
        public static string ToEndpoint(Endpoint en)
        {
            return en.ToDescription();
        }

        public static string ToRegion(Region en)
        {
            return en.ToDescription();
        }
    }
}
