using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static AppBO.Utility.Utility;

namespace AppBO.Utility
{
    public class Utility
    {
        public class ProccessResult
        {
            public bool Status { get; set; }
            public string Message { get; set; }
            public object Data { get; set; }
        }

        public class ProccessStatus
        {
            public static string Success = "Success";
            public static string Fail = "Fail";
            public static object Data = null;
        }

        public class keyValue
        {
            public static string RoleScopeAdmin = "admin";
            public static string RoleScopeApp = "app";

            public static string OperationInsert = "insert";
            public static string OperationUpdate = "update";
            public static string OperationDelete = "delete";
        }

        public class ApiResponse
        {
            public bool Status { get; set; } = true;

            public string Message { get; set; } = ProccessStatus.Success;

            public object Data { get; set; }
        }

        public string PasswordHash(string Password, bool isView)
        {
            StringBuilder sb = new StringBuilder();

            //Create an MD5 object
            using (MD5 md5 = MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash
                byte[] inputBytes = Encoding.UTF8.GetBytes(Password);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
            }

            return sb.ToString();
        }
    }

    public static class Extention
    {
        public static List<T> ToList<T>(this DataTable table) where T : class, new()
        {
            try
            {
                List<T> list = new List<T>();

                foreach (var row in table.AsEnumerable())
                {
                    T obj = new T();

                    foreach (var prop in obj.GetType().GetProperties())
                    {
                        try
                        {
                            PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
                            propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType), null);
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    list.Add(obj);
                }

                return list;
            }
            catch
            {
                return null;
            }
        }

        public static T MapObject<T>(object source) where T : new()
        {
            if (source is IEnumerable<object> sourceCollection && typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(List<>))
            {
                // Handle mapping for collections
                Type itemType = typeof(T).GetGenericArguments()[0];
                var targetList = (IList)Activator.CreateInstance(typeof(T));

                foreach (var item in sourceCollection)
                {
                    var mappedItem = Activator.CreateInstance(itemType);
                    foreach (var prop in item.GetType().GetProperties())
                    {
                        var targetProp = itemType.GetProperty(prop.Name);
                        if (targetProp != null && targetProp.CanWrite)
                        {
                            targetProp.SetValue(mappedItem, prop.GetValue(item));
                        }
                    }
                    targetList.Add(mappedItem);
                }

                return (T)targetList;
            }
            else
            {
                // Handle mapping for single objects
                T target = new T();
                foreach (var prop in source.GetType().GetProperties())
                {
                    var targetProp = typeof(T).GetProperty(prop.Name);
                    if (targetProp != null && targetProp.CanWrite)
                    {
                        targetProp.SetValue(target, prop.GetValue(source));
                    }
                }
                return target;
            }
        }


        public static async Task<T> ToModel<T>(this Task<ApiResponse> responseTask)
        {
            var response = await responseTask;

            // Handle null or invalid response data gracefully
            if (response?.Data == null)
            {
                if (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(List<>))
                {
                    // Return empty list
                    return (T)Activator.CreateInstance(typeof(T));
                }

                // If T is a class with a parameterless constructor, return a new instance instead of null
                if (typeof(T).IsClass && typeof(T).GetConstructor(Type.EmptyTypes) != null)
                {
                    return Activator.CreateInstance<T>();
                }

                // Otherwise, return default(T)
                return default!;
            }

            var data = response.Data.ToString();

            try
            {
                var jToken = JToken.Parse(data);

                if (jToken.Type == JTokenType.Array)
                {
                    // If T is a list type, deserialize correctly
                    if (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(List<>))
                    {
                        return jToken.ToObject<T>();
                    }
                    else
                    {
                        throw new InvalidOperationException("Expected an object but received a list.");
                    }
                }
                else
                {
                    // Deserialize as an object
                    return jToken.ToObject<T>();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to deserialize response data: {ex.Message}");
            }
        }

        public static T? ToObject<T>(this object? data) where T : class
        {
            if (data == null) return null; // Return null if the data is null

            try
            {
                // Parse the data as a JObject and convert it to the desired type
                return JObject.Parse(data.ToString()).ToObject<T>();
            }
            catch
            {
                // Handle any parsing or conversion errors gracefully
                return null;
            }
        }

        public static IQueryable<TTarget> MapTo<TSource, TTarget>(this IQueryable<TSource> source) where TTarget : new()
        {
            return source.Select(item => MapObject<TTarget>(item));
        }
    }
}
