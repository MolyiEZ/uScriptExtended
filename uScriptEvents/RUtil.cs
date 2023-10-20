using HarmonyLib;
using System;
using System.Reflection;

namespace uScriptExtended
{
	public static class RUtil
	{
		public static object getValue(string name, Type typeOf, object Obj)
		{
			FieldInfo FInfo = AccessTools.Field(typeOf, name);
			return FInfo.GetValue(Obj);
		}

		public static void setValue(string name, Type typeOf, object Obj, object Value)
		{
			FieldInfo FInfo = AccessTools.Field(typeOf, name);
			FInfo.SetValue(Obj, Value);
		}
	}
}
