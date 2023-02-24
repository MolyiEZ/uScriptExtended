using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ReflectionUtil
{
	public static class ReflectionUtil
	{
		public static object getValue<Class>(string name, Class class1)
		{

			if (!class1.GetType().IsClass)
			{
				Rocket.Core.Logging.Logger.LogError("Parameter 'class1' sent in to 'getValue' is not a class! Please contact me on discord: !");
				return null;
			}

			if (hasProperty(class1.GetType(), name))
			{
				PropertyInfo field = class1.GetType().GetProperty(name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static);
				if (field.GetGetMethod().IsStatic)
				{
					return field.GetValue(null, null);
				}
				return field.GetValue(class1, null);
			}
			else if (hasField(class1.GetType(), name))
			{
				FieldInfo field = class1.GetType().GetField(name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static);
				if (field.IsStatic)
				{
					return field.GetValue(null);
				}
				return field.GetValue(class1);
			}
			else
			{
				Rocket.Core.Logging.Logger.LogWarning("Failed to find Field/Property called " + name + "!");
				return null;
			}
		}


		public static void setValue<Class>(string name, object value, Class class1)
		{

			if (!class1.GetType().IsClass)
			{
				Rocket.Core.Logging.Logger.LogError("Parameter 'class1' sent in to 'setValue' is not a class! Please contact me on discord:!");
				return;
			}

			if (hasProperty(class1.GetType(), name))
			{
				PropertyInfo field = class1.GetType().GetProperty(name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static);
				if (field.GetGetMethod().IsStatic)
				{
					field.SetValue(null, Convert.ChangeType(value, field.PropertyType), null);
				}
				field.SetValue(class1, Convert.ChangeType(value, field.PropertyType), null);
			}
			else if (hasField(class1.GetType(), name))
			{
				FieldInfo field = class1.GetType().GetField(name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static);
				if (field.IsStatic)
				{
					field.SetValue(null, value);
				}
				field.SetValue(class1, value);
			}
			else
			{
				Rocket.Core.Logging.Logger.LogWarning("Failed to find Field/Property called " + name + "!");
				return;
			}
		}


		public static object callMethod<Class>(string name, Class class1, params object[] parameters)
		{

			if (!class1.GetType().IsClass)
			{
				Rocket.Core.Logging.Logger.LogError("Parameter 'class1' sent in to 'callMethod' is not a class! Please contact me on discord:!");
				return null;
			}

			var method = class1.GetType().GetMethod(name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static);

			if (method.IsStatic)
			{
				return method.Invoke(null, parameters);
			}
			else
			{
				return method.Invoke(class1, parameters);
			}

		}


		public static bool hasProperty(this Type type, string name)
		{
			return (type.GetProperty(name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static) != null);
		}

		public static bool hasField(this Type type, string name)
		{
			return (type.GetField(name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static) != null);
		}

	}
}