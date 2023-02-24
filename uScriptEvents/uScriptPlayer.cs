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
using static uScriptEvents.AnimalFunction;
using static uScriptEvents.ZombieFunction;

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

		[ScriptFunction("arrestCustom")]
		public static void arrestCustom([ScriptInstance] ExpressionValue instance, ushort id, ushort strength)
		{
			if (!(instance.Data is PlayerClass player)) return;
			player.Player.equipment.dequip();
			player.Player.animator.captorID = new CSteamID(ulong.Parse(player.Id));
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

		[ScriptFunction("get_hasEarpiece")]
		public static bool getEarpiece([ScriptInstance] ExpressionValue instance, int number)
		{
			if (!(instance.Data is PlayerClass player)) return false;
			return player.Player.voice.hasEarpiece;
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

		[ScriptFunction("get_isSafe")]
		public static bool getSafe([ScriptInstance] ExpressionValue instance)
		{
			if (!(instance.Data is PlayerClass player)) return false;
			return player.Player.movement.isSafe;
		}

		[ScriptFunction("get_isRadiated")]
		public static bool getRadiated([ScriptInstance] ExpressionValue instance)
		{
			if (!(instance.Data is PlayerClass player)) return false;
			return player.Player.movement.isRadiated;
		}

		[ScriptFunction("get_isGrounded")]
		public static bool getGrounded([ScriptInstance] ExpressionValue instance)
		{
			if (!(instance.Data is PlayerClass player)) return false;
			return player.Player.movement.isGrounded;
		}

		[ScriptFunction("get_oxygen")]
		public static ushort getOxygen([ScriptInstance] ExpressionValue instance)
		{
			if (!(instance.Data is PlayerClass player)) return 0;
			return player.Player.life.oxygen;
		}

		[ScriptFunction("set_oxygen")]
		public static void setOxygen([ScriptInstance] ExpressionValue instance, float value)
		{
			if (!(instance.Data is PlayerClass player)) return;
			ReflectionUtil.ReflectionUtil.setValue("_oxygen", (byte)value, player.Player.life);
		}
	}

	[ScriptTypeExtension(typeof(PlayerLookClass))]
	public class PlayerLookExtension
	{
		[ScriptFunction("getAnimal")]
		public static AnimalClass getAnimal([ScriptInstance] ExpressionValue instance)
		{
			if (!(instance.Data is PlayerLookClass player)) return null;

			RaycastHit hit;

			Ray ray = new Ray(player.Player.look.aim.position, player.Player.look.aim.forward);

			Physics.Raycast(ray, out hit, float.PositiveInfinity, RayMasks.AGENT);

			if (!(hit.transform != null)) return null;

			Animal animal = hit.transform.GetComponent<Animal>();
			if (!(animal != null)) return null;

			return new AnimalClass(animal);
		}

		[ScriptFunction("getZombie")]
		public static ZombieClass getZombie([ScriptInstance] ExpressionValue instance)
		{
			if (!(instance.Data is PlayerLookClass player)) return null;

			RaycastHit hit;

			Ray ray = new Ray(player.Player.look.aim.position, player.Player.look.aim.forward);

			Physics.Raycast(ray, out hit, float.PositiveInfinity, RayMasks.AGENT);

			if (!(hit.transform != null)) return null;

			Zombie zombie = hit.transform.GetComponent<Zombie>();
			if (!(zombie != null)) return null;

			return new ZombieClass(zombie);
		}
	}
}
