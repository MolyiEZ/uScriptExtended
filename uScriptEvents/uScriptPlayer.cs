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
	public class Players : ScriptModuleBase
	{
		[ScriptClass("playerSteam")]
		public class PlayerSteam
		{
			public CSteamID Player { get; }

			public PlayerSteam(CSteamID player)
			{
				this.Player = player;
			}

			[ScriptProperty(null)]
			public string Name
			{
				get
				{
					SteamPending steamPending = Provider.pending.FirstOrDefault(x => x.playerID.steamID.m_SteamID == Player.m_SteamID);
					return steamPending.playerID.characterName;
				}
				set
				{
					SteamPending steamPending = Provider.pending.FirstOrDefault(x => x.playerID.steamID.m_SteamID == Player.m_SteamID);
					steamPending.playerID.characterName = value;
				}
			}
			[ScriptProperty(null)]
			public string Id
			{
				get
				{
					SteamPending steamPending = Provider.pending.FirstOrDefault(x => x.playerID.steamID.m_SteamID == Player.m_SteamID);
					return steamPending.playerID.steamID.m_SteamID.ToString();
				}
			}
		}

		[ScriptClass("npc")]
		public class NpcClass
		{
			public InteractableObjectNPC Npc { get; }

			public NpcClass(InteractableObjectNPC npc)
			{
				this.Npc = npc;
			}

			[ScriptProperty(null)]
			public ushort id
			{
				get
				{
					return this.Npc.npcAsset.id;
				}
			}
		}
	}

	[ScriptTypeExtension(typeof(PlayerClass))]
	public class FunctionSalvageTime
	{
		[ScriptFunction("set_salvageTime")]
		public static void setSalvageTime([ScriptInstance] ExpressionValue instance, float time)
		{
			if (!(instance.Data is PlayerClass player)) return;
			player.Player.interact.sendSalvageTimeOverride(time);
			
		}

		[ScriptFunction("arrested")]
		public static void arrested([ScriptInstance] ExpressionValue instance, PlayerClass playerc, ushort id, ushort strength)
		{
			if (!(instance.Data is PlayerClass player)) return;
			player.Player.equipment.dequip();
			player.Player.animator.captorID = new CSteamID(ulong.Parse(playerc.Id));
			player.Player.animator.captorItem = id;
			player.Player.animator.captorStrength = strength;
			player.Player.animator.sendGesture(EPlayerGesture.ARREST_START, true);
		}

		[ScriptFunction("get_key")]
		public static bool getKey([ScriptInstance] ExpressionValue instance, int number)
		{
			if (!(instance.Data is PlayerClass player)) return false;
			return player.Player.input.keys[number];
		}

		[ScriptFunction("get_temperature")]
		public static string getTemperature([ScriptInstance] ExpressionValue instance)
		{
			if (!(instance.Data is PlayerClass player)) return null;
			return player.Player.life.temperature.ToString();
		}

		[ScriptFunction("get_stamina")]
		public static ushort getStamina([ScriptInstance] ExpressionValue instance)
		{ 
			if (!(instance.Data is PlayerClass player)) return 0;
			return player.Player.life.stamina;
		}

		[ScriptFunction("set_stamina")]
		public static void setStamina([ScriptInstance] ExpressionValue instance, float value)
		{
			if (!(instance.Data is PlayerClass player)) return;
			player.Player.life.serverModifyStamina(value);
		}
	}
}
