using ModuleEvents;
using Rocket.Unturned.Events;
using Rocket.Unturned.Permissions;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using uScript.API.Attributes;
using uScript.Core;
using uScript.Module.Main;
using uScript.Module.Main.Classes;
using uScript.Module.Main.Events;
using uScript.Unturned;
using uScriptClothingEvents;
using static SDG.Unturned.PlayerVoice;
using static uScript.Module.Main.PlayerEvents;
using static uScriptBarricades.Generator;
using static uScriptEvents.AnimalFunction;
using static uScriptEvents.ZombieFunction;
using static uScriptItems.Gun;
using static uScriptPlayers.Players;

namespace uScriptEvents
{
	[ScriptEvent("onBarricadeDamaged", "player, barricade, damage, cause, *cancel")]
	public class OnDamageBarricade : ScriptEvent
	{
		public override EventInfo EventHook(out object instance)
		{
			instance = null;
			return typeof(ModuleEvents.ModuleEvents).GetEvent("onDamageBarricade", BindingFlags.Public | BindingFlags.Static);
		}

		[ScriptEventSubscription]
		public void onDamageBarricadeRequested(CSteamID instigatorSteamID, Transform barricadeTransform, ref ushort pendingTotalDamage, ref bool shouldAllow, EDamageOrigin damageOrigin)
		{
			var args = new ExpressionValue[]
			{
				ExpressionValue.CreateObject(new PlayerClass(PlayerTool.getPlayer(instigatorSteamID))),
				ExpressionValue.CreateObject(new BarricadeClass(barricadeTransform.transform)),
				(double)pendingTotalDamage,
				damageOrigin.ToString(),
				!shouldAllow
			};

			RunEvent(this, args);
			shouldAllow = !args[4];
		}
	}

	[ScriptEvent("onBarricadeBuilded", "barricade")]
	public class OnBarricadeBuilded : ScriptEvent
	{
		public override EventInfo EventHook(out object instance)
		{
			instance = null;
			return typeof(ModuleEvents.ModuleEvents).GetEvent("onBarricadeSpawned", BindingFlags.Public | BindingFlags.Static);
		}

		[ScriptEventSubscription]
		public void OnBarricadeBuild(BarricadeRegion region, BarricadeDrop drop)
		{
			var args = new ExpressionValue[]
			{
				ExpressionValue.CreateObject(new BarricadeClass(drop.model))
			};

			RunEvent(this, args);
		}
	}

	[ScriptEvent("onBarricadeSalvaged", "player, barricade, *cancel")]
	public class OnBarricadeSalvaged : ScriptEvent
	{
		public override EventInfo EventHook(out object instance)
		{
			instance = null;
			return typeof(BarricadeDrop).GetEvent("OnSalvageRequested_Global", BindingFlags.Public | BindingFlags.Static);
		}

		[ScriptEventSubscription]
		public void onBarricadeSalv(BarricadeDrop barricade, SteamPlayer instigatorClient, ref bool shouldAllow)
		{
			var args = new ExpressionValue[]
			{
				ExpressionValue.CreateObject(new PlayerClass(PlayerTool.getPlayer(new CSteamID(instigatorClient.playerID.steamID.m_SteamID)))),
				ExpressionValue.CreateObject(new BarricadeClass(barricade.model)),
				!shouldAllow
			};

			RunEvent(this, args);
			shouldAllow = !args[2];
		}
	}

	[ScriptEvent("onPlayerRelayVoice", "player, isWalkie, *cancel")]
	public class OnPlayerRelayVoice : ScriptEvent
	{
		public override EventInfo EventHook(out object instance)
		{
			instance = null;
			return typeof(PlayerVoice).GetEvent("onRelayVoice", BindingFlags.Public | BindingFlags.Static);
		}

		[ScriptEventSubscription]
		public void onRelayVoice(PlayerVoice speaker, bool wantsToUseWalkieTalkie, ref bool shouldAllow, ref bool shouldBroadcastOverRadio, ref RelayVoiceCullingHandler cullingHandler)
		{
			var args = new ExpressionValue[]
			{
				ExpressionValue.CreateObject(new PlayerClass(speaker.player)),
				wantsToUseWalkieTalkie,
				!shouldAllow
			};
			RunEvent(this, args);
			shouldAllow = !args[2];
		}
	}

	[ScriptEvent("onPlayerSwapSeats", "player, vehicle, fromseat, toseat, *cancel")]
	public class OnPlayerSwapSeats : ScriptEvent
	{
		public override EventInfo EventHook(out object instance)
		{
			instance = null;
			return typeof(VehicleManager).GetEvent("onSwapSeatRequested", BindingFlags.Public | BindingFlags.Static);
		}

		[ScriptEventSubscription]
		public void onPlayerSwap(Player player, InteractableVehicle vehicle, ref bool shouldAllow, byte fromSeatIndex, ref byte toSeatIndex)
		{
			var args = new ExpressionValue[]
			{
				ExpressionValue.CreateObject(new PlayerClass(player)),
				ExpressionValue.CreateObject(new VehicleClass(vehicle)),
				(double)fromSeatIndex,
				(double)toSeatIndex,
				!shouldAllow
			};
			
			RunEvent(this, args);
			shouldAllow = !args[4];
		}
	}

	[ScriptEvent("onPlayerJoinRequested", "playerSteam, rejectionReason")]
	public class OnPlayerJoinRequested : ScriptEvent
	{
		public override EventInfo EventHook(out object instance)
		{
			instance = null;
			return typeof(UnturnedPermissions).GetEvent("OnJoinRequested", BindingFlags.Public | BindingFlags.Static);
		}

		Dictionary<string, ESteamRejection> rejectionReasons = new Dictionary<string, ESteamRejection>()
		{
			{ "SERVER_FULL", ESteamRejection.SERVER_FULL },
			{ "WRONG_HASH_LEVEL", ESteamRejection.WRONG_HASH_LEVEL },
			{ "WRONG_HASH_ASSEMBLY", ESteamRejection.WRONG_HASH_ASSEMBLY },
			{ "WRONG_VERSION", ESteamRejection.WRONG_VERSION },
			{ "WRONG_PASSWORD", ESteamRejection.WRONG_PASSWORD },
			{ "NAME_PLAYER_SHORT", ESteamRejection.NAME_PLAYER_SHORT },
			{ "NAME_PLAYER_LONG", ESteamRejection.NAME_PLAYER_LONG },
			{ "NAME_PLAYER_INVALID", ESteamRejection.NAME_PLAYER_INVALID },
			{ "NAME_PLAYER_NUMBER", ESteamRejection.NAME_PLAYER_NUMBER },
			{ "NAME_CHARACTER_SHORT", ESteamRejection.NAME_CHARACTER_SHORT },
			{ "NAME_CHARACTER_LONG", ESteamRejection.NAME_CHARACTER_LONG },
			{ "NAME_CHARACTER_INVALID", ESteamRejection.NAME_CHARACTER_INVALID },
			{ "NAME_CHARACTER_NUMBER", ESteamRejection.NAME_CHARACTER_NUMBER },
			{ "PRO_SERVER", ESteamRejection.PRO_SERVER },
			{ "PRO_CHARACTER", ESteamRejection.PRO_CHARACTER },
			{ "PRO_DESYNC", ESteamRejection.PRO_DESYNC },
			{ "PRO_APPEARANCE", ESteamRejection.PRO_APPEARANCE },
			{ "ALREADY_PENDING", ESteamRejection.ALREADY_PENDING },
			{ "ALREADY_CONNECTED", ESteamRejection.ALREADY_CONNECTED },
			{ "NOT_PENDING", ESteamRejection.NOT_PENDING },
			{ "LATE_PENDING", ESteamRejection.LATE_PENDING },
			{ "WHITELISTED", ESteamRejection.WHITELISTED },
			{ "AUTH_VERIFICATION", ESteamRejection.AUTH_VERIFICATION },
			{ "AUTH_NO_STEAM", ESteamRejection.AUTH_NO_STEAM },
			{ "AUTH_LICENSE_EXPIRED", ESteamRejection.AUTH_LICENSE_EXPIRED },
			{ "AUTH_VAC_BAN", ESteamRejection.AUTH_VAC_BAN },
			{ "AUTH_ELSEWHERE", ESteamRejection.AUTH_ELSEWHERE },
			{ "AUTH_TIMED_OUT", ESteamRejection.AUTH_TIMED_OUT },
			{ "AUTH_USED", ESteamRejection.AUTH_USED },
			{ "AUTH_NO_USER", ESteamRejection.AUTH_NO_USER },
			{ "AUTH_PUB_BAN", ESteamRejection.AUTH_PUB_BAN },
			{ "AUTH_ECON_DESERIALIZE", ESteamRejection.AUTH_ECON_DESERIALIZE },
			{ "AUTH_ECON_VERIFY", ESteamRejection.AUTH_ECON_VERIFY },
			{ "PING", ESteamRejection.PING },
			{ "PLUGIN", ESteamRejection.PLUGIN },
			{ "CLIENT_MODULE_DESYNC", ESteamRejection.CLIENT_MODULE_DESYNC },
			{ "SERVER_MODULE_DESYNC", ESteamRejection.SERVER_MODULE_DESYNC },
			{ "WRONG_LEVEL_VERSION", ESteamRejection.WRONG_LEVEL_VERSION },
			{ "WRONG_HASH_ECON", ESteamRejection.WRONG_HASH_ECON },
			{ "WRONG_HASH_MASTER_BUNDLE", ESteamRejection.WRONG_HASH_MASTER_BUNDLE },
			{ "LATE_PENDING_STEAM_AUTH", ESteamRejection.LATE_PENDING_STEAM_AUTH },
			{ "LATE_PENDING_STEAM_ECON", ESteamRejection.LATE_PENDING_STEAM_ECON },
			{ "LATE_PENDING_STEAM_GROUPS", ESteamRejection.LATE_PENDING_STEAM_GROUPS },
			{ "NAME_PRIVATE_LONG", ESteamRejection.NAME_PRIVATE_LONG },
			{ "NAME_PRIVATE_INVALID", ESteamRejection.NAME_PRIVATE_INVALID },
			{ "NAME_PRIVATE_NUMBER", ESteamRejection.NAME_PRIVATE_NUMBER }
		};

		[ScriptEventSubscription]
		public void onPlayerJoin(CSteamID playerSteam, ref ESteamRejection? rejectionReason)
		{
			var args = new ExpressionValue[]
			{
				ExpressionValue.CreateObject(new PlayerSteam(playerSteam)),
				rejectionReason.ToString()
			};

			RunEvent(this, args);

			if (rejectionReasons.ContainsKey(args[1].ToString()))
			{
				rejectionReason = rejectionReasons[args[1].ToString()];
			}
		}
	}

	[ScriptEvent("onPlayerTakingItem", "player, itemId, *cancel")]
	public class OnPlayerTakingItem : ScriptEvent
	{
		public override EventInfo EventHook(out object instance)
		{
			instance = null;
			return typeof(ModuleEvents.ModuleEvents).GetEvent("onTakeItemRequested", BindingFlags.Public | BindingFlags.Static);
		}

		[ScriptEventSubscription]
		public void onPlayerTaking(Player player, byte x, byte y, uint instanceID, byte to_x, byte to_y, byte to_rot, byte to_page, ItemData itemData, ref bool shouldAllow)
		{
			var args = new ExpressionValue[]
			{
				ExpressionValue.CreateObject(new PlayerClass(player)),
				ExpressionValue.CreateObject(new ItemClass(itemData.item.id)),
				!shouldAllow
			};

			RunEvent(this, args);
			shouldAllow = !args[2];
		}
	}

	[ScriptEvent("onResourceDamaged", "player, damage, cause, *cancel")]
	public class OnResourceDamaged : ScriptEvent
	{
		public override EventInfo EventHook(out object instance)
		{
			instance = null;
			return typeof(ModuleEvents.ModuleEvents).GetEvent("onDamageResourceRequested", BindingFlags.Public | BindingFlags.Static);
		}

		[ScriptEventSubscription]
		public void onResourceDam(CSteamID instigatorSteamID, Transform objectTransform, ref ushort pendingTotalDamage, ref bool shouldAllow, EDamageOrigin damageOrigin)
		{
			var args = new ExpressionValue[]
			{
				ExpressionValue.CreateObject(new PlayerClass(PlayerTool.getPlayer(instigatorSteamID))),
				(double)pendingTotalDamage,
				damageOrigin.ToString(),
				!shouldAllow
			};

			RunEvent(this, args);
			pendingTotalDamage = (ushort)Convert.ToInt16((double)args[1]);
			shouldAllow = !args[3];
		}
	}

	[ScriptEvent("onPlayerUnequipped", "player, item, *cancel")]
	public class OnPlayerUnequipped : ScriptEvent
	{
		public override EventInfo EventHook(out object instance)
		{
			instance = null;
			return typeof(InternalEvents).GetEvent("OnDequipRequested", BindingFlags.Public | BindingFlags.Static);
		}

		[ScriptEventSubscription]
		public void onPlayerUn(Player player, PlayerEquipment equipment, ref bool shouldAllow)
		{
			var args = new ExpressionValue[]
			{
				ExpressionValue.CreateObject(new PlayerClass(player)),
				ExpressionValue.CreateObject(new ItemClass(equipment.itemID)),
				!shouldAllow
			};

			RunEvent(this, args);
			shouldAllow = !args[2];
		}
	}

	[ScriptEvent("onSiphonVehicleRequest", "player, vehicle, amount, *cancel")]
	public class OnSiphonVehicleReques : ScriptEvent
	{
		public override EventInfo EventHook(out object instance)
		{
			instance = null;
			return typeof(ModuleEvents.ModuleEvents).GetEvent("onSiphonVehicleRequested", BindingFlags.Public | BindingFlags.Static);
		}

		[ScriptEventSubscription]
		public void OnSiphonVehicleReque(InteractableVehicle vehicle, Player instigatingPlayer, ref bool shouldAllow, ref ushort desiredAmount)
		{
			var args = new ExpressionValue[]
			{
				ExpressionValue.CreateObject(new PlayerClass(instigatingPlayer)),
				ExpressionValue.CreateObject(new VehicleClass(vehicle)),
				desiredAmount,
				!shouldAllow
			};

			RunEvent(this, args);
			shouldAllow = !args[3];
		}
	}

	[ScriptEvent("onVehicleLockRequest", "vehicle, *cancel")]
	public class OnVehicleLocked : ScriptEvent
	{
		public override EventInfo EventHook(out object instance)
		{
			instance = null;
			return typeof(VehicleManager).GetEvent("OnToggleVehicleLockRequested", BindingFlags.Public | BindingFlags.Static);
		}

		[ScriptEventSubscription]
		public void OnVehicleLock(InteractableVehicle vehicle, ref bool shouldAllow)
		{
			var args = new ExpressionValue[]
			{
				ExpressionValue.CreateObject(new VehicleClass(vehicle)),
				!shouldAllow
			};

			RunEvent(this, args);
			shouldAllow = !args[1];
		}
	}

	[ScriptEvent("onStructureBuilded", "structure")]
	public class OnStructureBuilded : ScriptEvent
	{
		public override EventInfo EventHook(out object instance)
		{
			instance = null;
			return typeof(ModuleEvents.ModuleEvents).GetEvent("onStructureSpawned", BindingFlags.Public | BindingFlags.Static);
		}

		[ScriptEventSubscription]
		public void OnStructureBuild(StructureRegion region, StructureDrop drop)
		{
			var args = new ExpressionValue[]
			{
				ExpressionValue.CreateObject(new StructureClass(drop.model))
			};

			RunEvent(this, args);
		}
	}

	[ScriptEvent("onStructureDamaged", "player, structure, damage, cause, *cancel")]
	public class OnStructureDamaged : ScriptEvent
	{
		public override EventInfo EventHook(out object instance)
		{
			instance = null;
			return typeof(ModuleEvents.ModuleEvents).GetEvent("onDamageStructure", BindingFlags.Public | BindingFlags.Static);
		}

		[ScriptEventSubscription]
		public void onDamageBarricadeRequested(CSteamID instigatorSteamID, Transform structureTransform, ref ushort pendingTotalDamage, ref bool shouldAllow, EDamageOrigin damageOrigin)
		{
			var args = new ExpressionValue[]
			{
				ExpressionValue.CreateObject(new PlayerClass(PlayerTool.getPlayer(instigatorSteamID))),
				ExpressionValue.CreateObject(new StructureClass(structureTransform.transform)),
				(double)pendingTotalDamage,
				damageOrigin.ToString(),
				!shouldAllow
			};

			RunEvent(this, args);
			shouldAllow = !args[4];
		}
	}

	[ScriptEvent("onStructureSalvaged", "player, structure, *cancel")]
	public class OnStructureSalvaged : ScriptEvent
	{
		public override EventInfo EventHook(out object instance)
		{
			instance = null;
			return typeof(StructureDrop).GetEvent("OnSalvageRequested_Global", BindingFlags.Public | BindingFlags.Static);
		}

		[ScriptEventSubscription]
		public void OnStructureSalv(StructureDrop structure, SteamPlayer instigatorClient, ref bool shouldAllow)
		{
			var args = new ExpressionValue[]
			{
				ExpressionValue.CreateObject(new PlayerClass(PlayerTool.getPlayer(new CSteamID(instigatorClient.playerID.steamID.m_SteamID)))),
				ExpressionValue.CreateObject(new StructureClass(structure.model)),
				!shouldAllow
			};

			RunEvent(this, args);
			shouldAllow = !args[2];
		}
	}

	[ScriptEvent("onPlayerHealthUpdated", "player, newHealth")]
	public class OnPlayerHealthUpdated : ScriptEvent
	{
		public override EventInfo EventHook(out object instance)
		{
			instance = null;
			return typeof(InternalEvents).GetEvent("OnHealthUpdated", BindingFlags.Public | BindingFlags.Static);
		}

		[ScriptEventSubscription]
		public void OnPlayerHealth(Player player, byte newHealth)
		{
			var args = new ExpressionValue[]
			{
				ExpressionValue.CreateObject(new PlayerClass(player)),
				(double)newHealth
			};

			RunEvent(this, args);
		}
	}

	[ScriptEvent("onPlayerFoodUpdated", "player, newFood")]
	public class OnPlayerFoodUpdated : ScriptEvent
	{
		public override EventInfo EventHook(out object instance)
		{
			instance = null;
			return typeof(InternalEvents).GetEvent("OnFoodUpdated", BindingFlags.Public | BindingFlags.Static);
		}

		[ScriptEventSubscription]
		public void OnPlayerFood(Player player, byte newFood)
		{
			var args = new ExpressionValue[]
			{
				ExpressionValue.CreateObject(new PlayerClass(player)),
				(double)newFood
			};

			RunEvent(this, args);
		}
	}

	[ScriptEvent("onPlayerWaterUpdated", "player, newWater")]
	public class OnPlayerWaterUpdated : ScriptEvent
	{
		public override EventInfo EventHook(out object instance)
		{
			instance = null;
			return typeof(InternalEvents).GetEvent("OnWaterUpdated", BindingFlags.Public | BindingFlags.Static);
		}

		[ScriptEventSubscription]
		public void OnPlayerWater(Player player, byte newWater)
		{
			var args = new ExpressionValue[]
			{
				ExpressionValue.CreateObject(new PlayerClass(player)),
				(double)newWater
			};

			RunEvent(this, args);
		}
	}

	[ScriptEvent("onPlayerVirusUpdated", "player, newVirus")]
	public class OnPlayerVirusUpdated : ScriptEvent
	{
		public override EventInfo EventHook(out object instance)
		{
			instance = null;
			return typeof(InternalEvents).GetEvent("OnVirusUpdated", BindingFlags.Public | BindingFlags.Static);
		}

		[ScriptEventSubscription]
		public void OnPlayerVirus(Player player, byte newVirus)
		{
			var args = new ExpressionValue[]
			{
				ExpressionValue.CreateObject(new PlayerClass(player)),
				(double)newVirus
			};

			RunEvent(this, args);
		}
	}

	[ScriptEvent("onPlayerStaminaUpdated", "player, newStamina")]
	public class OnPlayerStaminaUpdated : ScriptEvent
	{
		public override EventInfo EventHook(out object instance)
		{
			instance = null;
			return typeof(InternalEvents).GetEvent("OnStaminaUpdated", BindingFlags.Public | BindingFlags.Static);
		}

		[ScriptEventSubscription]
		public void OnPlayerVirus(Player player, byte newStamina)
		{
			var args = new ExpressionValue[]
			{
				ExpressionValue.CreateObject(new PlayerClass(player)),
				(double)newStamina
			};

			RunEvent(this, args);
		}
	}

	[ScriptEvent("onPlayerVisionUpdated", "player, isViewing")]
	public class OnPlayerVisionUpdated : ScriptEvent
	{
		public override EventInfo EventHook(out object instance)
		{
			instance = null;
			return typeof(InternalEvents).GetEvent("OnVisionUpdated", BindingFlags.Public | BindingFlags.Static);
		}

		[ScriptEventSubscription]
		public void OnPlayerVision(Player player, bool isViewing)
		{
			var args = new ExpressionValue[]
			{
				ExpressionValue.CreateObject(new PlayerClass(player)),
				isViewing
			};

			RunEvent(this, args);
		}
	}

	[ScriptEvent("onPlayerOxygenUpdated", "player, newOxygen")]
	public class OnPlayerOxygenUpdated : ScriptEvent
	{
		public override EventInfo EventHook(out object instance)
		{
			instance = null;
			return typeof(InternalEvents).GetEvent("OnOxygenUpdated", BindingFlags.Public | BindingFlags.Static);
		}

		[ScriptEventSubscription]
		public void OnPlayerOxygen(Player player, byte newOxygen)
		{
			var args = new ExpressionValue[]
			{
				ExpressionValue.CreateObject(new PlayerClass(player)),
				(double)newOxygen
			};

			RunEvent(this, args);
		}
	}

	[ScriptEvent("onPlayerBleedingUpdated", "player, isBleeding")]
	public class OnPlayerBleedingUpdated : ScriptEvent
	{
		public override EventInfo EventHook(out object instance)
		{
			instance = null;
			return typeof(InternalEvents).GetEvent("OnBleedingUpdated", BindingFlags.Public | BindingFlags.Static);
		}

		[ScriptEventSubscription]
		public void OnPlayerBleeding(Player player, bool newBleeding)
		{
			var args = new ExpressionValue[]
			{
				ExpressionValue.CreateObject(new PlayerClass(player)),
				newBleeding
			};

			RunEvent(this, args);
		}
	}

	[ScriptEvent("onPlayerBrokenUpdated", "player, isBroken")]
	public class OnPlayerBrokenUpdated : ScriptEvent
	{
		public override EventInfo EventHook(out object instance)
		{
			instance = null;
			return typeof(InternalEvents).GetEvent("OnBrokenUpdated", BindingFlags.Public | BindingFlags.Static);
		}

		[ScriptEventSubscription]
		public void OnPlayerBroken(Player player, bool newBroken)
		{
			var args = new ExpressionValue[]
			{
				ExpressionValue.CreateObject(new PlayerClass(player)),
				newBroken
			};

			RunEvent(this, args);
		}
	}

	[ScriptEvent("onPlayerTemperatureUpdated", "player, temperature")]
	public class OnPlayerTemperatureUpdated : ScriptEvent
	{
		public override EventInfo EventHook(out object instance)
		{
			instance = null;
			return typeof(InternalEvents).GetEvent("OnTemperatureUpdated", BindingFlags.Public | BindingFlags.Static);
		}

		[ScriptEventSubscription]
		public void OnPlayerTemperature(Player player, EPlayerTemperature newTemperature)
		{
			var args = new ExpressionValue[]
			{
				ExpressionValue.CreateObject(new PlayerClass(player)),
				newTemperature.ToString()
			};

			RunEvent(this, args);
		}
	}

	[ScriptEvent("onDayNightUpdated", "isDayTime")]
	public class OnDayNightUpdated : ScriptEvent
	{
		public override EventInfo EventHook(out object instance)
		{
			instance = null;
			return typeof(ModuleEvents.ModuleEvents).GetEvent("onDayNightUpdated", BindingFlags.Public | BindingFlags.Static);
		}

		[ScriptEventSubscription]
		public void onDayNight(bool isDaytime)
		{
			var args = new ExpressionValue[]
			{
				isDaytime
			};

			RunEvent(this, args);
		}
	}

	[ScriptEvent("onMoonUpdated", "isFullMoon")]
	public class OnMoonUpdated : ScriptEvent
	{
		public override EventInfo EventHook(out object instance)
		{
			instance = null;
			return typeof(ModuleEvents.ModuleEvents).GetEvent("onMoonUpdated", BindingFlags.Public | BindingFlags.Static);
		}

		[ScriptEventSubscription]
		public void OnMoon(bool isFullMoon)
		{
			var args = new ExpressionValue[]
			{
				isFullMoon
			};

			RunEvent(this, args);
		}
	}

	[ScriptEvent("onRainUpdated", "rain")]
	public class OnRainUpdated : ScriptEvent
	{
		public override EventInfo EventHook(out object instance)
		{
			instance = null;
			return typeof(ModuleEvents.ModuleEvents).GetEvent("onRainUpdated", BindingFlags.Public | BindingFlags.Static);
		}

		[ScriptEventSubscription]
		public void OnRain(ELightingRain rain)
		{
			var args = new ExpressionValue[]
			{
				rain.ToString()
			};

			RunEvent(this, args);
		}
	}

	[ScriptEvent("onSnowUpdated", "rain")]
	public class OnSnowUpdated : ScriptEvent
	{
		public override EventInfo EventHook(out object instance)
		{
			instance = null;
			return typeof(ModuleEvents.ModuleEvents).GetEvent("onSnowUpdated", BindingFlags.Public | BindingFlags.Static);
		}

		[ScriptEventSubscription]
		public void OnSnow(ELightingSnow snow)
		{
			var args = new ExpressionValue[]
			{
				snow.ToString()
			};

			RunEvent(this, args);
		}
	}

	[ScriptEvent("onVehicleLockpick", "player, vehicle, *cancel")]
	public class onVehicleLockpick : ScriptEvent
	{
		public override EventInfo EventHook(out object instance)
		{
			instance = null;
			return typeof(ModuleEvents.ModuleEvents).GetEvent("onVehicleLockpicked", BindingFlags.Public | BindingFlags.Static);
		}

		[ScriptEventSubscription]
		public void onVehicleLock(InteractableVehicle vehicle, Player instigatingPlayer, ref bool allow)
		{
			if (instigatingPlayer == null) return;
			var args = new ExpressionValue[]
			{
				ExpressionValue.CreateObject(new PlayerClass(instigatingPlayer)),
				ExpressionValue.CreateObject(new VehicleClass(vehicle)),
				!allow
			};

			RunEvent(this, args);
			allow = !args[2];
		}
	}

	[ScriptEvent("onVehicleRepair", "player, vehicle, totalHealing, *cancel")]
	public class OnVehicleRepair : ScriptEvent
	{
		public override EventInfo EventHook(out object instance)
		{
			instance = null;
			return typeof(ModuleEvents.ModuleEvents).GetEvent("onRepairVehicleRequested", BindingFlags.Public | BindingFlags.Static);
		}

		[ScriptEventSubscription]
		public void onVehicleRep(CSteamID instigatorSteamID, InteractableVehicle vehicle, ref ushort pendingTotalHealing, ref bool shouldAllow)
		{
			var args = new ExpressionValue[]
			{
				ExpressionValue.CreateObject(new PlayerClass(PlayerTool.getPlayer(instigatorSteamID))),
				ExpressionValue.CreateObject(new VehicleClass(vehicle)),
				(double)pendingTotalHealing,
				!shouldAllow
			};

			RunEvent(this, args);
			shouldAllow = !args[3];
		}
	}

	[ScriptEvent("onVehicleCarjack", "player, vehicle, force, torque, *cancel")]
	public class OnVehicleCarjack : ScriptEvent
	{
		public override EventInfo EventHook(out object instance)
		{
			instance = null;
			return typeof(ModuleEvents.ModuleEvents).GetEvent("onVehicleCarjacked", BindingFlags.Public | BindingFlags.Static);
		}

		[ScriptEventSubscription]
		public void OnVehicleCar(InteractableVehicle vehicle, Player instigatingPlayer, ref bool allow, ref Vector3 force, ref Vector3 torque)
		{
			var args = new ExpressionValue[]
			{
				ExpressionValue.CreateObject(new PlayerClass(instigatingPlayer)),
				ExpressionValue.CreateObject(new VehicleClass(vehicle)),
				ExpressionValue.CreateObject(new Vector3Class(force)),
				ExpressionValue.CreateObject(new Vector3Class(torque)),
				!allow
			};

			RunEvent(this, args);
			allow = !args[4];
		}
	}

	[ScriptEvent("onVehicleTireDamaged", "player, vehicle, cause, *cancel")]
	public class OnVehicleTireDamaged : ScriptEvent
	{
		public override EventInfo EventHook(out object instance)
		{
			instance = null;
			return typeof(ModuleEvents.ModuleEvents).GetEvent("onDamageTireRequested", BindingFlags.Public | BindingFlags.Static);
		}

		[ScriptEventSubscription]
		public void OnVehicleTireDam(CSteamID instigatorSteamID, InteractableVehicle vehicle, int tireIndex, ref bool shouldAllow, EDamageOrigin damageOrigin)
		{
			var args = new ExpressionValue[]
			{
				ExpressionValue.CreateObject(new PlayerClass(PlayerTool.getPlayer(instigatorSteamID))),
				ExpressionValue.CreateObject(new VehicleClass(vehicle)),
				damageOrigin.ToString(),
				!shouldAllow
			};

			RunEvent(this, args);
			shouldAllow = !args[3];
		}
	}

	[ScriptEvent("onPlayerPositionUpdated", "player")]
	public class OnPlayerMoved : ScriptEvent
	{
		public override EventInfo EventHook(out object instance)
		{
			instance = null;
			return typeof(UnturnedPlayerEvents).GetEvent("OnPlayerUpdatePosition", BindingFlags.Public | BindingFlags.Static);
		}

		[ScriptEventSubscription]
		public void OnPlayerMove(UnturnedPlayer player, Vector3 position)
		{
			var args = new ExpressionValue[]
			{
				ExpressionValue.CreateObject(new PlayerClass(player.Player))
			};

			RunEvent(this, args);
		}
	}

	[ScriptEvent("onPlayerSafetyUpdated", "player, isSafe")]
	public class OnPlayerSafetyUpdated : ScriptEvent
	{
		public override EventInfo EventHook(out object instance)
		{
			instance = null;
			return typeof(InternalEvents).GetEvent("OnSafetyUpdated", BindingFlags.Public | BindingFlags.Static);
		}

		[ScriptEventSubscription]
		public void onPlayerSafety(Player player, bool isSafe)
		{
			var args = new ExpressionValue[]
			{
				ExpressionValue.CreateObject(new PlayerClass(player)),
				isSafe
			};

			RunEvent(this, args);
		}
	}

	[ScriptEvent("onPlayerRadiationUpdated", "player, isRadiated")]
	public class OnPlayerRadiationUpdated : ScriptEvent
	{
		public override EventInfo EventHook(out object instance)
		{
			instance = null;
			return typeof(InternalEvents).GetEvent("OnRadiationUpdated", BindingFlags.Public | BindingFlags.Static);
		}

		[ScriptEventSubscription]
		public void OnPlayerRadiation(Player player, bool isRadiated)
		{
			var args = new ExpressionValue[]
			{
				ExpressionValue.CreateObject(new PlayerClass(player)),
				isRadiated
			};

			RunEvent(this, args);
		}
	}

	[ScriptEvent("onGunShooted", "player, item")]
	public class OnBulletSpawned : ScriptEvent
	{
		public override EventInfo EventHook(out object instance)
		{
			instance = null;
			return typeof(UseableGun).GetEvent("onBulletSpawned", BindingFlags.Public | BindingFlags.Static);
		}

		[ScriptEventSubscription]
		public void OnBulletSpawn(UseableGun gun, BulletInfo bullet)
		{
			var args = new ExpressionValue[]
			{
				ExpressionValue.CreateObject(new PlayerClass(gun.player)),
				ExpressionValue.CreateObject(new ItemClass(gun.GetComponent<ItemJar>()))
			};

			RunEvent(this, args);
		}
	}

	[ScriptEvent("onGunSightChanged", "player, item, oldItem, newItem, *cancel")]
	public class OnGunSightChanged : ScriptEvent
	{
		public override EventInfo EventHook(out object instance)
		{
			instance = null;
			return typeof(UseableGun).GetEvent("onChangeSightRequested", BindingFlags.Public | BindingFlags.Static);
		}

		[ScriptEventSubscription]
		public void OnGunSightChange(PlayerEquipment equipment, UseableGun gun, Item oldItem, ItemJar newItem, ref bool shouldAllow)
		{
			var args = new ExpressionValue[]
			{
				ExpressionValue.CreateObject(new PlayerClass(equipment.player)),
				ExpressionValue.CreateObject(new ItemClass(gun.equippedGunAsset.id)),
				(oldItem != null) ? ExpressionValue.CreateObject(new ItemNClass(oldItem)) : ExpressionValue.Null,
				(newItem != null) ? ExpressionValue.CreateObject(new ItemClass(newItem)) : ExpressionValue.Null,
				!shouldAllow
			};

			RunEvent(this, args);
			shouldAllow = !args[4];
		}
	}

	[ScriptEvent("onGunTacticalChanged", "player, item, oldItem, newItem, *cancel")]
	public class OnGunTacticalChanged : ScriptEvent
	{
		public override EventInfo EventHook(out object instance)
		{
			instance = null;
			return typeof(UseableGun).GetEvent("onChangeTacticalRequested", BindingFlags.Public | BindingFlags.Static);
		}

		[ScriptEventSubscription]
		public void OnGunTacticalChange(PlayerEquipment equipment, UseableGun gun, Item oldItem, ItemJar newItem, ref bool shouldAllow)
		{
			var args = new ExpressionValue[]
			{
				ExpressionValue.CreateObject(new PlayerClass(equipment.player)),
				ExpressionValue.CreateObject(new ItemClass(gun.equippedGunAsset.id)),
				(oldItem != null) ? ExpressionValue.CreateObject(new ItemNClass(oldItem)) : ExpressionValue.Null,
				(newItem != null) ? ExpressionValue.CreateObject(new ItemClass(newItem)) : ExpressionValue.Null,
				!shouldAllow
			};

			RunEvent(this, args);
			shouldAllow = !args[4];
		}
	}

	[ScriptEvent("onGunGripChanged", "player, item, oldItem, newItem, *cancel")]
	public class OnGunGripChanged : ScriptEvent
	{
		public override EventInfo EventHook(out object instance)
		{
			instance = null;
			return typeof(UseableGun).GetEvent("onChangeGripRequested", BindingFlags.Public | BindingFlags.Static);
		}

		[ScriptEventSubscription]
		public void OnGunGripChange(PlayerEquipment equipment, UseableGun gun, Item oldItem, ItemJar newItem, ref bool shouldAllow)
		{
			var args = new ExpressionValue[]
			{
				ExpressionValue.CreateObject(new PlayerClass(equipment.player)),
				ExpressionValue.CreateObject(new ItemClass(gun.equippedGunAsset.id)),
				(oldItem != null) ? ExpressionValue.CreateObject(new ItemNClass(oldItem)) : ExpressionValue.Null,
				(newItem != null) ? ExpressionValue.CreateObject(new ItemClass(newItem)) : ExpressionValue.Null,
				!shouldAllow
			};

			RunEvent(this, args);
			shouldAllow = !args[4];
		}
	}

	[ScriptEvent("onGunBarrelChanged", "player, item, oldItem, newItem, *cancel")]
	public class OnGunBarrelChanged : ScriptEvent
	{
		public override EventInfo EventHook(out object instance)
		{
			instance = null;
			return typeof(UseableGun).GetEvent("onChangeBarrelRequested", BindingFlags.Public | BindingFlags.Static);
		}

		[ScriptEventSubscription]
		public void OnGunBarrelChange(PlayerEquipment equipment, UseableGun gun, Item oldItem, ItemJar newItem, ref bool shouldAllow)
		{
			var args = new ExpressionValue[]
			{
				ExpressionValue.CreateObject(new PlayerClass(equipment.player)),
				ExpressionValue.CreateObject(new ItemClass(gun.equippedGunAsset.id)),
				(oldItem != null) ? ExpressionValue.CreateObject(new ItemNClass(oldItem)) : ExpressionValue.Null,
				(newItem != null) ? ExpressionValue.CreateObject(new ItemClass(newItem)) : ExpressionValue.Null,
				!shouldAllow
			};

			RunEvent(this, args);
			shouldAllow = !args[4];
		}
	}

	[ScriptEvent("onGunMagazineChanged", "player, item, oldItem, newItem, *cancel")]
	public class OnGunMagazineChanged : ScriptEvent
	{
		public override EventInfo EventHook(out object instance)
		{
			instance = null;
			return typeof(UseableGun).GetEvent("onChangeMagazineRequested", BindingFlags.Public | BindingFlags.Static);
		}

		[ScriptEventSubscription]
		public void OnGunMagazineChange(PlayerEquipment equipment, UseableGun gun, Item oldItem, ItemJar newItem, ref bool shouldAllow)
		{
			var args = new ExpressionValue[]
			{
				ExpressionValue.CreateObject(new PlayerClass(equipment.player)),
				ExpressionValue.CreateObject(new ItemClass(gun.equippedGunAsset.id)),
				(oldItem != null) ? ExpressionValue.CreateObject(new ItemNClass(oldItem)) : ExpressionValue.Null,
				(newItem != null) ? ExpressionValue.CreateObject(new ItemClass(newItem)) : ExpressionValue.Null,
				!shouldAllow
			};

			RunEvent(this, args);
			shouldAllow = !args[4];
		}
	}

	[ScriptEvent("onGunBulletHit", "player, item, hit, *cancel")]
	public class OnGunBulletHit : ScriptEvent
	{
		public override EventInfo EventHook(out object instance)
		{
			instance = null;
			return typeof(UseableGun).GetEvent("onBulletHit", BindingFlags.Public | BindingFlags.Static);
		}

		[ScriptEventSubscription]
		public void OnGunBullet(UseableGun gun, BulletInfo bullet, InputInfo hit, ref bool shouldAllow)
		{
			var args = new ExpressionValue[]
			{
				ExpressionValue.CreateObject(new PlayerClass(gun.player)),
				ExpressionValue.CreateObject(new ItemClass(gun.equippedGunAsset.id)),
				ExpressionValue.CreateObject(new HitClass(hit)),
				!shouldAllow
			};

			RunEvent(this, args);
			shouldAllow = !args[3];
		}
	}

	[ScriptEvent("onFarmHarvest", "player, barricade, *cancel")]
	public class OnFarmHarvested : ScriptEvent
	{
		public override EventInfo EventHook(out object instance)
		{
			instance = null;
			return typeof(InteractableFarm).GetEvent("OnHarvestRequested_Global", BindingFlags.Public | BindingFlags.Static);
		}

		[ScriptEventSubscription]
		public void OnFarmHarvest(InteractableFarm harvestable, SteamPlayer instigatorPlayer, ref bool shouldAllow)
		{
			var args = new ExpressionValue[]
			{
				ExpressionValue.CreateObject(new PlayerClass(instigatorPlayer.player)),
				ExpressionValue.CreateObject(new BarricadeClass(harvestable.transform)),
				!shouldAllow
			};

			RunEvent(this, args);
			shouldAllow = !args[2];
		}
	}

	[ScriptEvent("onPlayerDamagedCustom", "player, killer, *cancel, *damage, cause, limb, ragdoll")]
	public class OnPlayerDamagedCustom : ScriptEvent
	{
		public override EventInfo EventHook(out object instance)
		{
			instance = null;
			return typeof(DamageTool).GetEvent("damagePlayerRequested", BindingFlags.Public | BindingFlags.Static);
		}

		Dictionary<string, ERagdollEffect> ragdolls = new Dictionary<string, ERagdollEffect>()
		{
			{"NONE", ERagdollEffect.NONE},
			{"BRONZE", ERagdollEffect.BRONZE},
			{"SILVER", ERagdollEffect.SILVER},
			{"GOLD", ERagdollEffect.GOLD},
			{"ZERO", ERagdollEffect.ZERO_KELVIN},
		};

		[ScriptEventSubscription]
		public void OnPlayerDamaged(ref DamagePlayerParameters parameters, ref bool shouldAllow)
		{

			Player player = PlayerTool.getPlayer(parameters.killer);
			var ragdoll = "NONE";

			var args = new ExpressionValue[]
			{
				ExpressionValue.CreateObject(new PlayerClass(parameters.player)),
				(player != null) ? ExpressionValue.CreateObject(new PlayerClass(player)) : ExpressionValue.Null,
				!shouldAllow,
				parameters.damage,
				parameters.limb.ToString(),
				parameters.cause.ToString(),
				ragdoll.ToString()
			};

			RunEvent(this, args);
			shouldAllow = !args[2];
			parameters.damage = (float)(double)args[3];

			if (ragdolls.ContainsKey(args[6].ToString()))
			{
				parameters.ragdollEffect = ragdolls[args[6].ToString()];
			}
			else
			{
				Rocket.Core.Logging.Logger.LogWarning("uScriptExtended module from uScript => Ragdoll must be 'NONE', 'BRONZE', 'GOLD', 'SILVER', or 'ZERO'.");
			}
		}
	}

	[ScriptEvent("onAnimalDamaged", "animal, killer, *cancel, *damage, limb, ragdoll")]
	public class OnAnimalDamaged : ScriptEvent
	{
		public override EventInfo EventHook(out object instance)
		{
			instance = null;
			return typeof(DamageTool).GetEvent("damageAnimalRequested", BindingFlags.Public | BindingFlags.Static);
		}

		Dictionary<string, ERagdollEffect> ragdolls = new Dictionary<string, ERagdollEffect>()
		{
			{"NONE", ERagdollEffect.NONE},
			{"BRONZE", ERagdollEffect.BRONZE},
			{"SILVER", ERagdollEffect.SILVER},
			{"GOLD", ERagdollEffect.GOLD},
			{"ZERO", ERagdollEffect.ZERO_KELVIN},
		};

		[ScriptEventSubscription]
		public void OnAnimalDamage(ref DamageAnimalParameters parameters, ref bool shouldAllow)
		{
			Player killer = null;
			if (parameters.instigator is Player player) 
			{
				killer = player;
			}
			var ragdoll = "NONE";

			var args = new ExpressionValue[]
			{
				ExpressionValue.CreateObject(new AnimalClass(parameters.animal)),
				(killer != null) ? ExpressionValue.CreateObject(new PlayerClass(killer)) : ExpressionValue.Null,
				!shouldAllow,
				parameters.damage,
				parameters.limb.ToString(),
				ragdoll.ToString()
			};

			RunEvent(this, args);
			shouldAllow = !args[2];
			parameters.damage = (float)(double)args[3];

			if (ragdolls.ContainsKey(args[5].ToString()))
			{
				parameters.ragdollEffect = ragdolls[args[5].ToString()];
			}
			else
			{
				Rocket.Core.Logging.Logger.LogWarning("uScriptExtended module from uScript => Ragdoll must be 'NONE', 'BRONZE', 'GOLD', 'SILVER', or 'ZERO'.");
			}
		}
	}

	[ScriptEvent("onZombieDamaged", "zombie, killer, *cancel, *damage, limb, ragdoll")]
	public class OnZombieDamaged : ScriptEvent
	{
		public override EventInfo EventHook(out object instance)
		{
			instance = null;
			return typeof(DamageTool).GetEvent("damageZombieRequested", BindingFlags.Public | BindingFlags.Static);
		}

		Dictionary<string, ERagdollEffect> ragdolls = new Dictionary<string, ERagdollEffect>()
		{
			{"NONE", ERagdollEffect.NONE},
			{"BRONZE", ERagdollEffect.BRONZE},
			{"SILVER", ERagdollEffect.SILVER},
			{"GOLD", ERagdollEffect.GOLD},
			{"ZERO", ERagdollEffect.ZERO_KELVIN},
		};

		[ScriptEventSubscription]
		public void OnZombieDamage(ref DamageZombieParameters parameters, ref bool shouldAllow)
		{
			Player killer = null;
			if (parameters.instigator is Player player)
			{
				killer = player;
			}
			var ragdoll = "NONE";

			var args = new ExpressionValue[]
			{
				ExpressionValue.CreateObject(new ZombieClass(parameters.zombie)),
				(killer != null) ? ExpressionValue.CreateObject(new PlayerClass(killer)) : ExpressionValue.Null,
				!shouldAllow,
				parameters.damage,
				parameters.limb.ToString(),
				ragdoll.ToString()
			};

			RunEvent(this, args);
			shouldAllow = !args[2];
			parameters.damage = (float)(double)args[3];

			if (ragdolls.ContainsKey(args[5].ToString()))
			{
				parameters.ragdollEffect = ragdolls[args[5].ToString()];
			}
			else
			{
				Rocket.Core.Logging.Logger.LogWarning("uScriptExtended module from uScript => Ragdoll must be 'NONE', 'BRONZE', 'GOLD', 'SILVER', or 'ZERO'.");
			}
		}
	}
	
	[ScriptEvent("onPlayerClothingEquipping", "player, item, slot, *cancel")]
	public class OnPlayerClothingEquipping : ScriptEvent
	{
		public override EventInfo EventHook(out object instance)
		{
			instance = null;
			return typeof(ClothingEvents).GetEvent("OnEquippingClothing", BindingFlags.Public | BindingFlags.Static);
		}

		[ScriptEventSubscription]
		public void OnPlayerClothingEquip(Player player, ItemJar item, byte quality, byte[] state, EClothingSlot type, ref bool cancel)
		{
			var args = new ExpressionValue[]
			{
				ExpressionValue.CreateObject(new PlayerClass(player)),
				ExpressionValue.CreateObject(new ItemClass(item)),
				type.ToString(),
				cancel
			};

			RunEvent(this, args);
			cancel = args[3];
		}
	}
	
	[ScriptEvent("onPlayerClothingUnequipping", "player, item, slot, *cancel")]
	public class OnPlayerClothingUnequipping : ScriptEvent
	{
		public override EventInfo EventHook(out object instance)
		{
			instance = null;
			return typeof(ClothingEvents).GetEvent("OnUnequippingClothing", BindingFlags.Public | BindingFlags.Static);
		}

		[ScriptEventSubscription]
		public void OnPlayerClothingUnequip(Player player, ItemJar item, byte quality, byte[] state, EClothingSlot type, ref bool cancel)
		{
			var args = new ExpressionValue[]
			{
				ExpressionValue.CreateObject(new PlayerClass(player)),
				ExpressionValue.CreateObject(new ItemClass(item)),
				type.ToString(),
				cancel
			};

			RunEvent(this, args);
			cancel = args[3];
		}
	}

	[ScriptEvent("onAnimalSpawned", "animal")]
	public class OnAnimalSpawned : ScriptEvent
	{
		public override EventInfo EventHook(out object instance)
		{
			instance = null;
			return typeof(AnimalEvents).GetEvent("OnAnimalAdded", BindingFlags.Public | BindingFlags.Static);
		}

		[ScriptEventSubscription]
		public void OnAnimalSpawn(Animal nativeAnimal)
		{
			var args = new ExpressionValue[]
			{
				ExpressionValue.CreateObject(new AnimalClass(nativeAnimal)),
			};

			RunEvent(this, args);
		}
	}

	[ScriptEvent("onAnimalRevived", "animal")]
	public class OnAnimalRevived : ScriptEvent
	{
		public override EventInfo EventHook(out object instance)
		{
			instance = null;
			return typeof(AnimalEvents).GetEvent("OnAnimalRevived", BindingFlags.Public | BindingFlags.Static);
		}

		[ScriptEventSubscription]
		public void OnAnimalRevive(Animal nativeAnimal)
		{
			var args = new ExpressionValue[]
			{
				ExpressionValue.CreateObject(new AnimalClass(nativeAnimal)),
			};

			RunEvent(this, args);
		}
	}

	[ScriptEvent("onAnimalFleeing", "animal, direction, *cancel")]
	public class OnAnimalFleeing : ScriptEvent
	{
		public override EventInfo EventHook(out object instance)
		{
			instance = null;
			return typeof(AnimalEvents).GetEvent("OnAnimalFleeing", BindingFlags.Public | BindingFlags.Static);
		}

		[ScriptEventSubscription]
		public void OnAnimalFlee(Animal nativeAnimal, ref Vector3 direction, ref bool sendToPack, ref bool cancel)
		{
			var args = new ExpressionValue[]
			{
				ExpressionValue.CreateObject(new AnimalClass(nativeAnimal)),
				ExpressionValue.CreateObject(new Vector3Class(direction)),
				cancel
			};

			RunEvent(this, args);
			cancel = args[2];
		}
	}

	[ScriptEvent("onAnimalAttackingPoint", "animal, point, *cancel")]
	public class OnAnimalAttackingPoint : ScriptEvent
	{
		public override EventInfo EventHook(out object instance)
		{
			instance = null;
			return typeof(AnimalEvents).GetEvent("OnAnimalAttackingPoint", BindingFlags.Public | BindingFlags.Static);
		}

		[ScriptEventSubscription]
		public void OnAnimalAttacking(Animal nativeAnimal, ref Vector3 point, ref bool sendToPack, ref bool cancel)
		{
			var args = new ExpressionValue[]
			{
				ExpressionValue.CreateObject(new AnimalClass(nativeAnimal)),
				ExpressionValue.CreateObject(new Vector3Class(point)),
				cancel
			};

			RunEvent(this, args);
			cancel = args[2];
		}
	}

	[ScriptEvent("onAnimalAttackingPlayer", "animal, player, *cancel")]
	public class OnAnimalAttackingPlayer : ScriptEvent
	{
		public override EventInfo EventHook(out object instance)
		{
			instance = null;
			return typeof(AnimalEvents).GetEvent("OnAnimalAttackingPlayer", BindingFlags.Public | BindingFlags.Static);
		}

		[ScriptEventSubscription]
		public void OnAnimalAttackingPlay(Animal nativeAnimal, ref Player player, ref bool sendToPack, ref bool cancel)
		{
			var args = new ExpressionValue[]
			{
				ExpressionValue.CreateObject(new AnimalClass(nativeAnimal)),
				ExpressionValue.CreateObject(new PlayerClass(player)),
				cancel
			};

			RunEvent(this, args);
			cancel = args[2];
		}
	}

	[ScriptEvent("onVehicleHorn", "player, vehicle, *cancel")]
	public class OnVehicleHorn : ScriptEvent
	{
		public override EventInfo EventHook(out object instance)
		{
			instance = null;
			return typeof(VehicleEvents).GetEvent("OnVehicleHorn", BindingFlags.Public | BindingFlags.Static);
		}

		[ScriptEventSubscription]
		public void OnVehicleHor(Player player, InteractableVehicle vehicle, ref bool cancel)
		{
			var args = new ExpressionValue[]
			{
				ExpressionValue.CreateObject(new PlayerClass(player)),
				ExpressionValue.CreateObject(new VehicleClass(vehicle)),
				cancel
			};

			RunEvent(this, args);
			cancel = args[2];
		}
	}

	[ScriptEvent("onVehicleHeadLightsUpdated", "player, vehicle, *cancel")]
	public class OnVehicleHeadLightsUpdated : ScriptEvent
	{
		public override EventInfo EventHook(out object instance)
		{
			instance = null;
			return typeof(VehicleEvents).GetEvent("OnVehicleHeadLightsUpdated", BindingFlags.Public | BindingFlags.Static);
		}
		
		[ScriptEventSubscription]
		public void OnVehicleHeadLightsUpd(Player player, InteractableVehicle vehicle, ref bool cancel)
		{
			var args = new ExpressionValue[]
			{
				ExpressionValue.CreateObject(new PlayerClass(player)),
				ExpressionValue.CreateObject(new VehicleClass(vehicle)),
				cancel
			};

			RunEvent(this, args);
			cancel = args[2];
		}
	}

	[ScriptEvent("onPlayerFlagUpdated", "player, flagId, flagValue")]
	public class OnPlayerFlagUpdated : ScriptEvent
	{
		public override EventInfo EventHook(out object instance)
		{
			instance = null;
			return typeof(PlayerQuests).GetEvent("onAnyFlagChanged", BindingFlags.Public | BindingFlags.Static);
		}

		[ScriptEventSubscription]
		public void OnPlayerFlagUpdate(PlayerQuests quests, PlayerQuestFlag flag)
		{
			var args = new ExpressionValue[]
			{
				ExpressionValue.CreateObject(new PlayerClass(quests.player)),
				(double)flag.id,
				(double)flag.value
			};

			RunEvent(this, args);
		}
	}

	[ScriptEvent("onPlayerGroupUpdated", "player, oldGroupId, oldGroupRank, newGroupId, newGroupRank")]
	public class OnPlayerGroupUpdated : ScriptEvent
	{
		public override EventInfo EventHook(out object instance)
		{
			instance = null;
			return typeof(PlayerQuests).GetEvent("onGroupChanged", BindingFlags.Public | BindingFlags.Static);
		}

		[ScriptEventSubscription]
		public void OnPlayerGroupUpdate(PlayerQuests sender, CSteamID oldGroupID, EPlayerGroupRank oldGroupRank, CSteamID newGroupID, EPlayerGroupRank newGroupRank)
		{
			var args = new ExpressionValue[]
			{
				ExpressionValue.CreateObject(new PlayerClass(sender.player)),
				oldGroupID.ToString(),
				oldGroupRank.ToString(),
				newGroupID.ToString(),
				newGroupRank.ToString()
			};

			RunEvent(this, args);
		}
	}

	[ScriptEvent("onVehicleHook", "player, vehicle, *cancel")]
	public class OnVehicleHook : ScriptEvent
	{
		public override EventInfo EventHook(out object instance)
		{
			instance = null;
			return typeof(VehicleEvents).GetEvent("OnVehicleHook", BindingFlags.Public | BindingFlags.Static);
		}

		[ScriptEventSubscription]
		public void OnVehicleH(Player player, InteractableVehicle vehicle, ref bool cancel)
		{
			var args = new ExpressionValue[]
			{
				ExpressionValue.CreateObject(new PlayerClass(player)),
				ExpressionValue.CreateObject(new VehicleClass(vehicle)),
				cancel
			};

			RunEvent(this, args);
			cancel = args[2];
		}
	}
}
