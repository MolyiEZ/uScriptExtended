using Newtonsoft.Json.Converters;
using System.Text.RegularExpressions;
using uScript.API.Attributes;
using uScript.Core;

namespace uScriptEvents
{
	[ScriptTypeExtension(typeof(ExpressionValue))]
	public class FunctionString
	{
		[ScriptFunction("isMatch")]
		public static bool isMatch([ScriptInstance] ExpressionValue instance, string value)
		{
			if (!(instance.Data is string str)) return false;
			return Regex.IsMatch(str, value);
		}
	}
}