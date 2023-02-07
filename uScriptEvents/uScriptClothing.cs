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
using uScript.Module.Main.Classes;

namespace uScriptEvents
{
	[ScriptTypeExtension(typeof(PlayerClothingClass))]
	public class FunctionClothing
	{
		[ScriptFunction("removeBackpack")]
		public static void removeBackpack([ScriptInstance] ExpressionValue instance)
		{
			if (!(instance.Data is PlayerClothingClass playerclothing)) return;
			playerclothing.Player.clothing.askWearBackpack(0, 0, new byte[0], true);
		}

		[ScriptFunction("removeGlasses")]
		public static void removeGlasses([ScriptInstance] ExpressionValue instance)
		{
			if (!(instance.Data is PlayerClothingClass playerclothing)) return;
			playerclothing.Player.clothing.askWearGlasses(0, 0, new byte[0], true);
		}

		[ScriptFunction("removeHat")]
		public static void removeHat([ScriptInstance] ExpressionValue instance)
		{
			if (!(instance.Data is PlayerClothingClass playerclothing)) return;
			playerclothing.Player.clothing.askWearHat(0, 0, new byte[0], true);
		}

		[ScriptFunction("removeMask")]
		public static void removeMask([ScriptInstance] ExpressionValue instance)
		{
			if (!(instance.Data is PlayerClothingClass playerclothing)) return;
			playerclothing.Player.clothing.askWearMask(0, 0, new byte[0], true);
		}

		[ScriptFunction("removePants")]
		public static void removePants([ScriptInstance] ExpressionValue instance)
		{
			if (!(instance.Data is PlayerClothingClass playerclothing)) return;
			playerclothing.Player.clothing.askWearPants(0, 0, new byte[0], true);
		}

		[ScriptFunction("removeShirt")]
		public static void removeShirt([ScriptInstance] ExpressionValue instance)
		{
			if (!(instance.Data is PlayerClothingClass playerclothing)) return;
			playerclothing.Player.clothing.askWearShirt(0, 0, new byte[0], true);
		}

		[ScriptFunction("removeVest")]
		public static void removeVest([ScriptInstance] ExpressionValue instance)
		{
			if (!(instance.Data is PlayerClothingClass playerclothing)) return;
			playerclothing.Player.clothing.askWearVest(0, 0, new byte[0], true);
		}
	}
}
