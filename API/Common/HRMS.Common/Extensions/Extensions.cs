using HRMS.Common.Enumeration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.IO;

namespace HRMS.Common.Extensions
{
    public static class Extensions
    {
        #region GetEnumDescription
        /// <summary>
        /// Gets Description of enumeration
        /// </summary>
        /// <param name="input"></param>
        /// <returns>String of enumeration</returns>
        public static string GetEnumDescription(this Enum input)
        {
            string output = null;
            Type type = input.GetType();
    
            FieldInfo fi = type.GetField(input.ToString());
            var attrs = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attrs.Length > 0)
            {
                output = attrs[0].Description;
            }

            return output;
        }
        #endregion

        #region TrimLowerCase()
        /// <summary>
        /// Gets lower case trimmed string
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Lower case trimmed string</returns>
        public static string TrimLowerCase(this string input)
        {
            return input.ToLower().Trim();
        }
        #endregion

        #region TrimUpperCase()
        /// <summary>
        /// Gets upper case trimmed string
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Upper case trimmed string</returns>
        public static string TrimUpperCase(this string input)
        {
            return input.ToUpper().Trim();
        }
        #endregion

        #region ToHashEntries
        /// <summary>
        /// Convert the object to hash entities
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static HashEntry[] ToHashEntries(this object obj)
        {
            PropertyInfo[] properties = obj.GetType().GetProperties();
            return properties
                .Where(x => x.GetValue(obj) != null) // <-- PREVENT NullReferenceException
                .Select(property => new HashEntry(property.Name, property.GetValue(obj)
                .ToString())).ToArray();
        }

        #endregion

        #region ConvertHashToEntity
        /// <summary>
        /// Convert Hash Entities to Object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="hashEntries"></param>
        /// <returns></returns>
        public static T ConvertHashToEntity<T>(this HashEntry[] hashEntries)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            var obj = Activator.CreateInstance(typeof(T));
            foreach (var property in properties)
            {
                HashEntry entry = hashEntries.FirstOrDefault(g => g.Name.ToString().Equals(property.Name));
                var targetType = IsNullableType(property.PropertyType) ? Nullable.GetUnderlyingType(property.PropertyType) : property.PropertyType;
                if (entry.Equals(new HashEntry())) continue;
                property.SetValue(obj, Convert.ChangeType(entry.Value.ToString(), targetType));
            }
            return (T)obj;
        }

        private static bool IsNullableType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
        }

        #endregion

        #region GetmimeType
        /// <summary>
        /// Get the Type of the File
        /// </summary>
        /// <returns>Dictionary of the file type and its extension</returns>
        public static Dictionary<string, string> GetMimeType()
        {
            return new Dictionary<string, string>
            {
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
                {".pdf", "application/pdf"}
            };
        }
        #endregion

        #region CreateFolder
        /// <summary>
        /// Creates a folder with specified filepath
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="projectCode"></param>
        /// <returns></returns>
        public static void CreateFolder(string filePath, string projectCode)
        {
            bool isExists = Directory.Exists(filePath);
            if (!isExists)
                Directory.CreateDirectory(filePath);

            isExists = Directory.Exists(filePath + "/" + projectCode);
            if (!isExists)
                Directory.CreateDirectory(filePath + "/" + projectCode);
        }
        #endregion

    }
}
