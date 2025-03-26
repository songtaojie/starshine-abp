using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.Abp.Swashbuckle
{
    internal static class ObjectExtensions
    {
        /// <summary>
        /// 首字母小写
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        internal static string ToLowerCamelCase(this string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return str;

            return string.Concat(str.First().ToString().ToLower(), str.AsSpan(1));
        }

        /// <summary>
        /// 首字母大写
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        internal static string ToUpperCamelCase(this string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return str;

            return string.Concat(str.First().ToString().ToUpper(), str.AsSpan(1));
        }

        /// <summary>
        /// 将 DateTime 转换成 DateTimeOffset
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        internal static DateTimeOffset ConvertToDateTimeOffset(this DateTime dateTime)
        {
            return DateTime.SpecifyKind(dateTime, DateTimeKind.Local);
        }

        /// <summary>
        /// 将 DateTimeOffset 转换成本地 DateTime
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        internal static DateTime ConvertToDateTime(this DateTimeOffset dateTime)
        {
            if (dateTime.Offset.Equals(TimeSpan.Zero))
                return dateTime.UtcDateTime;
            if (dateTime.Offset.Equals(TimeZoneInfo.Local.GetUtcOffset(dateTime.DateTime)))
                return dateTime.ToLocalTime().DateTime;
            else
                return dateTime.DateTime;
        }

        /// <summary>
        /// 将一个对象转换为指定类型
        /// </summary>
        /// <param name="obj">待转换的对象</param>
        /// <param name="type">目标类型</param>
        /// <returns>转换后的对象</returns>
        internal static object? ChangeType(this object? obj, Type type)
        {
            if (type == null) return obj;
            if (type == typeof(string)) return obj?.ToString();
            if (type == typeof(Guid) && obj != null &&Guid.TryParse(obj.ToString(),out Guid result)) return result;
            if (type == typeof(bool) && obj != null && obj is not bool)
            {
                var objStr = obj.ToString()?.ToLower();
                if (objStr == "1" || objStr == "true" || objStr == "yes" || objStr == "on") return true;
                return false;
            }
            if (obj == null) return type.IsValueType ? Activator.CreateInstance(type) : null;
            var underlyingType = Nullable.GetUnderlyingType(type);
            var objString = obj.ToString();
            if (type.IsAssignableFrom(obj.GetType())) return obj;
            else if ((underlyingType ?? type).IsEnum)
            {
                if (underlyingType != null && string.IsNullOrWhiteSpace(objString)) return null;
                else if(string.IsNullOrEmpty(objString)) return null;
                else return Enum.Parse(underlyingType ?? type, objString);
            }
            // 处理DateTime -> DateTimeOffset 类型
            else if (obj.GetType().Equals(typeof(DateTime)) && (underlyingType ?? type).Equals(typeof(DateTimeOffset)))
            {
                return ((DateTime)obj).ConvertToDateTimeOffset();
            }
            // 处理 DateTimeOffset -> DateTime 类型
            else if (obj.GetType().Equals(typeof(DateTimeOffset)) && (underlyingType ?? type).Equals(typeof(DateTime)))
            {
                return ((DateTimeOffset)obj).ConvertToDateTime();
            }
            else if (typeof(IConvertible).IsAssignableFrom(underlyingType ?? type))
            {
                try
                {
                    return Convert.ChangeType(obj, underlyingType ?? type, null);
                }
                catch
                {
                    return underlyingType == null ? Activator.CreateInstance(type) : null;
                }
            }
            else
            {
                var converter = TypeDescriptor.GetConverter(type);
                if (converter.CanConvertFrom(obj.GetType())) return converter.ConvertFrom(obj);

                var constructor = type.GetConstructor(Type.EmptyTypes);
                if (constructor != null)
                {
                    var o = constructor.Invoke(null);
                    var propertys = type.GetProperties();
                    var oldType = obj.GetType();

                    foreach (var property in propertys)
                    {
                        var p = oldType.GetProperty(property.Name);
                        if (property.CanWrite && p != null && p.CanRead)
                        {
                            property.SetValue(o, ChangeType(p.GetValue(obj, null), property.PropertyType), null);
                        }
                    }
                    return o;
                }
            }
            return obj;
        }

    }
}
