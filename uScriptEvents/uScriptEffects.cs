using Rocket.Unturned.Effects;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using uScript.API.Attributes;
using uScript.Core;
using uScript.Module.Main;
using uScript.Module.Main.Classes;
using uScript.Unturned;
using uScriptPlayers;
using static SDG.Unturned.WeatherAsset;
using static uScriptBarricades.Generator;

namespace uScriptBarricades
{
	public class Effects : ScriptModuleBase
	{
		[ScriptModule("effectspawn")]
		public static class EffectModule
		{
			[ScriptFunction(null)]
			public static void Effect(string effectID, Vector3Class position)
			{
				Guid guid = Guid.Parse(effectID);
				Vector3 pos = position.Vector3;
				TriggerEffectParameters parameters2 = new TriggerEffectParameters(guid);
				parameters2.position = pos;
				parameters2.relevantDistance = 1024f;
				EffectManager.triggerEffect(parameters2);
			}

			[ScriptFunction(null)]
			public static void EffectPlayer(string effectID, Vector3Class position, PlayerClass player)
			{
				Guid guid = Guid.Parse(effectID);
				Vector3 pos = position.Vector3;
				TriggerEffectParameters parameters = new TriggerEffectParameters(guid);
				parameters.position = pos;
				parameters.SetRelevantPlayer(PlayerTool.getSteamPlayer(player.Name));
				EffectManager.triggerEffect(parameters);
			}

			[ScriptFunction(null)]
			public static void EffectClear(string effectID) 
			{
				Guid guid = Guid.Parse(effectID);
				EffectManager.ClearEffectByGuid_AllPlayers(guid);
				EffectManager.ReceiveEffectClearByGuid(guid);
			}
		}
	}
}
