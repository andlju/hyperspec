using System;

namespace Hyperspec
{
    public static class ReflectionHelpers
    {
        public static bool IsNullableType(this Type type)
        {
            return type.IsGenericType && (type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)));
        }

        public static Type GetTypeOfNullable(this Type type)
        {
            return type.GetGenericArguments()[0];
        }
    }
}