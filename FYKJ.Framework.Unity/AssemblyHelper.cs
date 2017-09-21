using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;

namespace FYKJ.Framework.Utility
{
    public static class AssemblyHelper
    {
        public static Dictionary<PropertyInfo, T> FindAllPropertyByAttribute<T>(string searchpattern = "*.dll") where T: Attribute
        {
            var dictionary = new Dictionary<PropertyInfo, T>();
            var attributeType = typeof(T);
            foreach (var str2 in Directory.GetFiles(GetBaseDirectory(), searchpattern, SearchOption.TopDirectoryOnly))
            {
                foreach (var type2 in Assembly.LoadFrom(str2).GetLoadableTypes())
                {
                    foreach (var info in type2.GetProperties())
                    {
                        var customAttributes = info.GetCustomAttributes(attributeType, true);
                        if (customAttributes.Length != 0)
                        {
                            dictionary.Add(info, (T)customAttributes.First());
                        }
                    }
                }
            }
            return dictionary;
        }

        public static Dictionary<string, List<T>> FindAllTypeByAttribute<T>(string searchpattern = "*.dll") where T: Attribute
        {
            var dictionary = new Dictionary<string, List<T>>();
            var attributeType = typeof(T);
            foreach (var str2 in Directory.GetFiles(GetBaseDirectory(), searchpattern, SearchOption.TopDirectoryOnly))
            {
                foreach (var type2 in Assembly.LoadFrom(str2).GetLoadableTypes())
                {
                    var assemblyQualifiedName = type2.AssemblyQualifiedName;
                    var customAttributes = type2.GetCustomAttributes(attributeType, true);
                    if (customAttributes.Length != 0)
                    {
                        dictionary.Add(assemblyQualifiedName, new List<T>());
                        foreach (T local in customAttributes)
                        {
                            dictionary[assemblyQualifiedName].Add(local);
                        }
                    }
                }
            }
            return dictionary;
        }

        public static List<Type> FindTypeByInheritType(Type inheritType, string searchpattern = "*.dll")
        {
            return (from str2 in Directory.GetFiles(GetBaseDirectory(), searchpattern, SearchOption.TopDirectoryOnly) from type in Assembly.LoadFrom(str2).GetLoadableTypes() where type.BaseType == inheritType select type).ToList();
        }

        public static T FindTypeByInterface<T>(string searchpattern = "*.dll") where T: class
        {
            var type = typeof(T);
            return (from str2 in Directory.GetFiles(GetBaseDirectory(), searchpattern, SearchOption.TopDirectoryOnly) from type2 in Assembly.LoadFrom(str2).GetLoadableTypes() where (type != type2) && type.IsAssignableFrom(type2) select (Activator.CreateInstance(type2) as T)).FirstOrDefault();
        }

        public static string GetBaseDirectory()
        {
            var privateBinPath = AppDomain.CurrentDomain.SetupInformation.PrivateBinPath;
            if (AppDomain.CurrentDomain.SetupInformation.PrivateBinPath == null)
            {
                privateBinPath = AppDomain.CurrentDomain.BaseDirectory;
            }
            return privateBinPath;
        }

        public static Assembly GetEntryAssembly()
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly != null)
            {
                return entryAssembly;
            }
            if ((HttpContext.Current == null) || (HttpContext.Current.ApplicationInstance == null))
            {
                return Assembly.GetExecutingAssembly();
            }
            var baseType = HttpContext.Current.ApplicationInstance.GetType();
            while ((baseType != null) && (baseType.Namespace == "ASP"))
            {
                baseType = baseType.BaseType;
            }
            return baseType?.Assembly;
        }

        public static IEnumerable<Type> GetLoadableTypes(this Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException exception)
            {
                return from t in exception.Types
                    where t != null
                    select t;
            }
        }

        public static IList<Stream> GetResourceStream(Assembly assembly, Expression<Func<string, bool>> predicate)
        {
            var list = (from str in assembly.GetManifestResourceNames() where predicate.Compile()(str) select assembly.GetManifestResourceStream(str)).ToList();
            new StreamReader(list[0]).ReadToEnd();
            list[0].Position = 0L;
            return list;
        }
    }
}

