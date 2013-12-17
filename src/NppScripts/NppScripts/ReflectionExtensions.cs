using System;
using System.Reflection;

namespace NppScripts
{
    /// <summary>
    /// This class contains extension methods for a more convenient way of making Reflection based dispatch-style calls for the object members.
    /// </summary>
    public static class ReflectionExtensions
    {
        /// <summary>
        /// Gets the value of the object field.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="name">The name.</param>
        /// <param name="throwOnError">if set to <c>true</c>  then it throw on error.</param>
        /// <returns>Value of the field</returns>
        /// <exception cref="System.Exception">ReflectionExtensions: cannot find field &lt;name&gt;</exception>
        public static object GetField(this object obj, string name, bool throwOnError = true)
        {
            //Note GetField(s) does not return base class fields like GetProperty does.
            //BindingFlags.FlattenHierarchy does not make any difference (Reflection bug).
            //Thus we have to process base classes explicitly.
            var type = obj.GetType();
            var field = type.GetFieldInfo(name);
            if (field != null)
                return field.GetValue(obj);

            if (throwOnError)
                throw new Exception("ReflectionExtensions: cannot find field " + name);

            return null;
        }

        /// <summary>
        /// Gets the value of the static field.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="name">The name.</param>
        /// <param name="throwOnError">if set to <c>true</c> then it throw on error.</param>
        /// <returns>Value of the field</returns>
        /// <exception cref="System.Exception">ReflectionExtensions: cannot find field &lt;name&gt;</exception>
        public static object GetStaticField(this Type type, string name, bool throwOnError = true)
        {
            var field = type.GetFieldInfo(name);
            if (field != null)
                return field.GetValue(null);

            if (throwOnError)
                throw new Exception("ReflectionExtensions: cannot find field " + name);

            return null;
        }

        /// <summary>
        /// Gets the <c>FieldInfo</c> of the <c>Type</c>.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="name">The name.</param>
        /// <param name="throwOnError">if set to <c>true</c> then it throw on error.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">ReflectionExtensions: cannot find field &lt;name&gt;</exception>
        public static FieldInfo GetFieldInfo(this Type type, string name, bool throwOnError = true)
        {
            //Note GetField(s) does not return base class fields like GetProperty does.
            //BindingFlags.FlattenHierarchy does not make any difference (Reflection bug).
            //Thus we have to process base classes explicitly.
            while (type != null)
            {
                var field = type.GetField(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

                if (field != null)
                    return field;
                type = type.BaseType;
            }

            if (throwOnError)
                throw new Exception("ReflectionExtensions: cannot find field " + name);

            return null;
        }

        /// <summary>
        /// Sets the field value.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="name">The field name.</param>
        /// <param name="value">The value.</param>
        /// <param name="throwOnError">if set to <c>true</c> [throw on error].</param>
        /// <exception cref="System.Exception">ReflectionExtensions: cannot find field &lt;name&gt;</exception>
        public static void SetField(this object obj, string name, object value, bool throwOnError = true)
        {
            //Note GetField(s) does not return base class fields like GetProperty does.
            //BindingFlags.FlattenHierarchy does not make any difference (Reflection bug).
            //Thus we have to process base classes explicitly.
            var type = obj.GetType();
            while (type != null)
            {
                var field = type.GetField(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (field != null)
                {
                    field.SetValue(obj, value);
                    return;
                }
                type = type.BaseType;
            }

            if (throwOnError)
                throw new Exception("ReflectionExtensions: cannot find field " + name);
        }

        /// <summary>
        /// Gets the property value.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="name">The property name.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">ReflectionExtensions: cannot find property &lt;name&gt;</exception>
        public static object GetProp(this object obj, string name)
        {
            var property = obj.GetType().GetProperty(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (property == null)
                throw new Exception("ReflectionExtensions: cannot find property " + name);
            return property.GetValue(obj, null);
        }

        /// <summary>
        /// Sets the property value.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="name">The property name.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="System.Exception">ReflectionExtensions: cannot find property &lt;name&gt;</exception>
        public static void SetProp(this object obj, string name, object value)
        {
            var property = obj.GetType().GetProperty(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (property == null)
                throw new Exception("ReflectionExtensions: cannot find property " + name);
            property.SetValue(obj, value, null);
        }

        /// <summary>
        /// Calls the specified object method by name.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="name">The name of the method.</param>
        /// <param name="args">The arguments of the method.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">ReflectionExtensions: cannot find method &lt;name&gt;</exception>
        public static object Call(this object obj, string name, params object[] args)
        {
            var method = obj.GetType().GetMethod(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod);
            if (method == null)
                throw new Exception("ReflectionExtensions: cannot find method " + name);
            return method.Invoke(obj, args);
        }

        /// <summary>
        /// Calls the specified object static method by name.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="name">The method name.</param>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">ReflectionExtensions: cannot find method &lt;name&gt;</exception>
        public static object CallStatic(this Type type, string name, params object[] args)
        {
            var method = type.GetMethod(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.InvokeMethod);
            if (method == null)
                throw new Exception("ReflectionExtensions: cannot find method " + name);
            return method.Invoke(null, args);
        }

        /// <summary>
        /// Calls the specified object method by name if such a method exists.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="name">The method name.</param>
        /// <param name="args">The arguments.</param>
        public static void CallIfExists(this object obj, string name, params object[] args)
        {
            var method = obj.GetType().GetMethod(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod);
            if (method != null)
                method.Invoke(obj, args);
        }
    }
}