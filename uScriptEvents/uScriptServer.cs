using Rocket.Unturned.Effects;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using uScript.API.Attributes;
using uScript.Core;
using uScript.Module.Main.Classes;
using uScript.Unturned;
using static uScriptPlayers.Players;

namespace uScriptPlayers
{
	[ScriptModule("serverExtended")]
	public class Server : ScriptModuleBase
	{
		[ScriptFunction("getPlayersInRadius")]
		public static ExpressionValue getPlayersInRadius(Vector3Class center, double radius)
		{
			List<Player> list = new List<Player>();
			PlayerTool.getPlayersInRadius(center.Vector3, (float)radius, list);
			return ExpressionValue.Array(Enumerable.Select<Player, ExpressionValue>(list, (Player t) => ExpressionValue.CreateObject(new PlayerClass(t))));
		} 
	}
}
