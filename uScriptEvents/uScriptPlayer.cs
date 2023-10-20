using HarmonyLib;
using Rocket.Unturned.Effects;
using Rocket.Unturned.Player;
using SDG.NetTransport;
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
using uScriptExtended;
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
			public SteamPending SteamPend { get; }

			public PlayerSteam(CSteamID player)
			{
				this.Player = player;
				this.SteamPend = Provider.pending.FirstOrDefault(x => x.playerID.steamID.m_SteamID == Player.m_SteamID);
			}

			[ScriptProperty(null)]
			public string Name
			{
				get
				{
					return this.SteamPend.playerID.characterName;
				}
				set
				{
					this.SteamPend.playerID.characterName = value;
				}
			}
			[ScriptProperty(null)]
			public string Id
			{
				get
				{
					return this.Player.m_SteamID.ToString();
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
			if (instance.Data is not PlayerClass player) return;
			player.Player.interact.sendSalvageTimeOverride(time);			
		}

		[ScriptFunction("arrestCustom")]
		public static void arrestCustom([ScriptInstance] ExpressionValue instance, ushort id, ushort strength)
		{
			if (instance.Data is not PlayerClass player) return;
			player.Player.equipment.dequip();
			player.Player.animator.captorID = new CSteamID(ulong.Parse(player.Id));
			player.Player.animator.captorItem = id;
			player.Player.animator.captorStrength = strength;
			player.Player.animator.sendGesture(EPlayerGesture.ARREST_START, true);
		}

		[ScriptFunction("teleportCustom")]
		public static void teleportCustom([ScriptInstance] ExpressionValue instance, Vector3Class vector, float rotation)
		{
			if (instance.Data is not PlayerClass player) return;
			player.Player.teleportToLocation(vector.Vector3, rotation);
		}

		[ScriptFunction("teleportCustom")]
		public static void teleportCustom([ScriptInstance] ExpressionValue instance, double x, double y, double z, float rotation)
		{
			if (instance.Data is not PlayerClass player) return;
			player.Player.teleportToLocation(new Vector3((float)x, (float)y, (float)z), rotation);
		}

		[ScriptFunction("get_hasEarpiece")]
		public static bool getEarpiece([ScriptInstance] ExpressionValue instance, int number)
		{
			if (instance.Data is not PlayerClass player) return false;
			return player.Player.voice.hasEarpiece;
		}

		[ScriptFunction("get_temperature")]
		public static string getTemperature([ScriptInstance] ExpressionValue instance)
		{
			if (instance.Data is not PlayerClass player) return null;
			return player.Player.life.temperature.ToString();
		}

		[ScriptFunction("get_stamina")]
		public static ushort getStamina([ScriptInstance] ExpressionValue instance)
		{ 
			if (instance.Data is not PlayerClass player) return 0;
			return player.Player.life.stamina;
		}

		[ScriptFunction("set_stamina")]
		public static void setStamina([ScriptInstance] ExpressionValue instance, float value)
		{
			if (instance.Data is not PlayerClass player) return;
			player.Player.life.serverModifyStamina(value);
		}

		[ScriptFunction("get_isSafe")]
		public static bool getSafe([ScriptInstance] ExpressionValue instance)
		{
			if (instance.Data is not PlayerClass player) return false;
			return player.Player.movement.isSafe;
		}

		[ScriptFunction("get_isRadiated")]
		public static bool getRadiated([ScriptInstance] ExpressionValue instance)
		{
			if (instance.Data is not PlayerClass player) return false;
			return player.Player.movement.isRadiated;
		}

		[ScriptFunction("get_isGrounded")]
		public static bool getGrounded([ScriptInstance] ExpressionValue instance)
		{
			if (instance.Data is not PlayerClass player) return false;
			return player.Player.movement.isGrounded;
		}

		[ScriptFunction("get_oxygen")]
		public static ushort getOxygen([ScriptInstance] ExpressionValue instance)
		{
			if (instance.Data is not PlayerClass player) return 0;
			return player.Player.life.oxygen;
		}

		[ScriptFunction("set_oxygen")]
		public static void setOxygen([ScriptInstance] ExpressionValue instance, byte value)
		{
			if (instance.Data is not PlayerClass player) return;
			RUtil.setValue("_oxygen", typeof(PlayerLife), player.Player.life, value);
		}
	}

	[ScriptTypeExtension(typeof(PlayerLookClass))]
	public class PlayerLookExtension
	{
		[ScriptFunction("getAnimal")]
		public static AnimalClass getAnimal([ScriptInstance] ExpressionValue instance)
		{
			if (instance.Data is not PlayerLookClass player) return null;

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
			if (instance.Data is not PlayerLookClass player) return null;

			RaycastHit hit;

			Ray ray = new Ray(player.Player.look.aim.position, player.Player.look.aim.forward);

			Physics.Raycast(ray, out hit, float.PositiveInfinity, RayMasks.AGENT);

			if (!(hit.transform != null)) return null;

			Zombie zombie = hit.transform.GetComponent<Zombie>();
			if (!(zombie != null)) return null;

			return new ZombieClass(zombie);
		}
	}

	[ScriptTypeExtension(typeof(PlayerInventoryClass))]
	public class PlayerInventoryExtension
	{
		[ScriptFunction("addItemAuto")]
		public static void addItemCustom([ScriptInstance] ExpressionValue instance, ushort itemId, byte amount = 1, bool autoEquipWeapon = true, bool autoEquipUseable = true, bool autoEquipClothing = true)
		{
			if (instance.Data is not PlayerInventoryClass playerInventory) return;

			for(int i = 0; i < amount; i++)
			{
				playerInventory.Player.inventory.forceAddItemAuto(new Item(itemId, true), autoEquipWeapon, autoEquipUseable, autoEquipClothing);
			}
		}
	}
}
