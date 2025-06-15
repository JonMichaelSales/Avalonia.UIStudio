using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avalonia.UIStudio.Appearance.Services
{
    public static class TypeHelpers
    {


        public static bool IsListOfStrings(Type type)
        {
            if (type == typeof(string))
                return false;

            if (typeof(IEnumerable<string>).IsAssignableFrom(type))
                return true;

            if (type.IsGenericType &&
                type.GetGenericTypeDefinition() == typeof(List<>) &&
                type.GetGenericArguments()[0] == typeof(string))
                return true;

            if (type.IsArray && type.GetElementType() == typeof(string))
                return true;

            return false;
        }

        public static bool IsListOfObjects(Type type)
        {
            if (type == typeof(string))
                return false;

            if (typeof(IEnumerable).IsAssignableFrom(type) &&
                type != typeof(string))
            {
                var elementType = GetElementType(type);
                return elementType != null &&
                       !elementType.IsPrimitive &&
                       elementType != typeof(string) &&
                       !elementType.IsEnum;
            }

            return false;
        }

        public static Type? GetElementType(Type type)
        {
            if (type.IsArray)
                return type.GetElementType();

            if (type.IsGenericType && typeof(IEnumerable<>).IsAssignableFrom(type.GetGenericTypeDefinition()))
                return type.GetGenericArguments()[0];

            foreach (var iface in type.GetInterfaces())
            {
                if (iface.IsGenericType && iface.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                    return iface.GetGenericArguments()[0];
            }

            return null;
        }
    }
}
