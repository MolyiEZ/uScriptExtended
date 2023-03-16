using SDG.Unturned;
using Steamworks; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using uScript.API.Attributes;
using uScript.Core;
using uScript.Module.Main;
using uScript.Module.Main.Classes;

namespace uScriptEvents
{
	[ScriptTypeExtension(typeof(MapClass))]
	public class FunctionMap
	{
		[ScriptFunction("getKey")]
		public static ExpressionValue getKey([ScriptInstance] ExpressionValue instance, ExpressionValue value)
		{
			if (!(instance.Data is MapClass map)) return null;
			foreach (var item in map.Map)
			{
				if (item.Value == value)
				{
					return item.Key;
				}
			}
			return null;
		}
	}
}