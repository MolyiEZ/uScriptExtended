using HarmonyLib;
using Rocket.API.Extensions;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using uScript.Unturned;

namespace ModuleEvents
{
	public class ModuleEvents : ScriptModuleBase
	{
		private Harmony harmony;

		protected override void OnModuleLoaded()
		{
			U.Events.OnBeforePlayerConnected += AddComponentPlayer;
			Level.onPreLevelLoaded += OnPreLevelLoaded;

			harmony = new Harmony("uScriptExtended");
			harmony.PatchAll();

			BarricadeManager.onDamageBarricadeRequested += (CSteamID instigatorSteamID, Transform barricadeTransform, ref ushort pendingTotalDamage, ref bool shouldAllow, EDamageOrigin damageOrigin) =>
			{
				onDamageBarricade.Invoke(instigatorSteamID, barricadeTransform, ref pendingTotalDamage, ref shouldAllow, damageOrigin);
			};

			StructureManager.onDamageStructureRequested += (CSteamID instigatorSteamID, Transform structureTransform, ref ushort pendingTotalDamage, ref bool shouldAllow, EDamageOrigin damageOrigin) =>
			{
				onDamageStructure.Invoke(instigatorSteamID, structureTransform, ref pendingTotalDamage, ref shouldAllow, damageOrigin);
			};

			ResourceManager.onDamageResourceRequested += (CSteamID instigatorSteamID, Transform objectTransform, ref ushort pendingTotalDamage, ref bool shouldAllow, EDamageOrigin damageOrigin) =>
			{
				onDamageResourceRequested.Invoke(instigatorSteamID, objectTransform, ref pendingTotalDamage, ref shouldAllow, damageOrigin);
			};

			ItemManager.onTakeItemRequested += (Player player, byte x, byte y, uint instanceID, byte to_x, byte to_y, byte to_rot, byte to_page, ItemData itemData, ref bool shouldAllow) =>
			{
				onTakeItemRequested.Invoke(player, x, y, instanceID, to_x, to_y, to_rot, to_page, itemData, ref shouldAllow);
			};

			VehicleManager.onSiphonVehicleRequested += (InteractableVehicle vehicle, Player instigatingPlayer, ref bool shouldAllow, ref ushort desiredAmount) =>
			{
				onSiphonVehicleRequested.Invoke(vehicle, instigatingPlayer, ref shouldAllow, ref desiredAmount);
			};

			BarricadeManager.onBarricadeSpawned += (BarricadeRegion region, BarricadeDrop drop) =>
			{
				onBarricadeSpawned.Invoke(region, drop);
			};

			StructureManager.onStructureSpawned += (StructureRegion region, StructureDrop drop) =>
			{
				onStructureSpawned.Invoke(region, drop);
			};

			VehicleManager.onVehicleLockpicked += (InteractableVehicle vehicle, Player instigatingPlayer, ref bool allow) =>
			{
				onVehicleLockpicked.Invoke(vehicle, instigatingPlayer, ref allow);
			};

			VehicleManager.onDamageTireRequested += (CSteamID instigatorSteamID, InteractableVehicle vehicle, int tireIndex, ref bool shouldAllow, EDamageOrigin damageOrigin) =>
			{
				onDamageTireRequested.Invoke(instigatorSteamID, vehicle, tireIndex, ref shouldAllow, damageOrigin);
			};

			VehicleManager.onVehicleCarjacked += (InteractableVehicle vehicle, Player instigatingPlayer, ref bool allow, ref Vector3 force, ref Vector3 torque) =>
			{
				onVehicleCarjacked.Invoke(vehicle, instigatingPlayer, ref allow, ref force, ref torque);
			};

			VehicleManager.onRepairVehicleRequested += (CSteamID instigatorSteamID, InteractableVehicle vehicle, ref ushort pendingTotalHealing, ref bool shouldAllow) =>
			{
				onRepairVehicleRequested.Invoke(instigatorSteamID, vehicle, ref pendingTotalHealing, ref shouldAllow);
			};
		}

		public static event DamageBarricadeRequestHandler onDamageBarricade;

		public static event DamageResourceRequestHandler onDamageResourceRequested;

		public static event TakeItemRequestHandler onTakeItemRequested;

		public static event SiphonVehicleRequestHandler onSiphonVehicleRequested;

		public static event BarricadeSpawnedHandler onBarricadeSpawned;

		public static event StructureSpawnedHandler onStructureSpawned;

		public static event DamageStructureRequestHandler onDamageStructure;

		public static event DayNightUpdated onDayNightUpdated;

		public static event MoonUpdated onMoonUpdated;

		public static event RainUpdated onRainUpdated;

		public static event SnowUpdated onSnowUpdated;

		public static event VehicleLockpickedSignature onVehicleLockpicked;

		public static event RepairVehicleRequestHandler onRepairVehicleRequested;

		public static event DamageTireRequestHandler onDamageTireRequested;

		public static event VehicleCarjackedSignature onVehicleCarjacked;

		private static void AddComponentPlayer(UnturnedPlayer player)
		{
			player.Player.gameObject.AddComponent<InternalEvents>();
		}

		private static void OnPreLevelLoaded(int level)
		{
			LightingManager.onDayNightUpdated += (bool isDaytime) =>
			{
				onDayNightUpdated.Invoke(isDaytime);
			};

			LightingManager.onMoonUpdated += (bool isFullMoon) =>
			{
				onMoonUpdated.Invoke(isFullMoon);
			};

			LightingManager.onRainUpdated += (ELightingRain rain) =>
			{
				onRainUpdated.Invoke(rain);
			};

			LightingManager.onSnowUpdated += (ELightingSnow snow) =>
			{
				onSnowUpdated.Invoke(snow);
			};
		}
	}

	public class InternalEvents : MonoBehaviour
	{
		public static event DequipRequested OnDequipRequested;

		public static event HealthUpdated OnHealthUpdated;

		public static event FoodUpdated OnFoodUpdated;

		public static event WaterUpdated OnWaterUpdated;

		public static event VirusUpdated OnVirusUpdated;

		public static event StaminaUpdated OnStaminaUpdated;

		public static event VisionUpdated OnVisionUpdated;

		public static event OxygenUpdated OnOxygenUpdated;

		public static event BleedingUpdated OnBleedingUpdated;

		public static event BrokenUpdated OnBrokenUpdated;

		public static event TemperatureUpdated OnTemperatureUpdated;

		public static event SafetyUpdated OnSafetyUpdated;

		public static event RadiationUpdated OnRadiationUpdated;

		private void Awake()
		{
			var player = gameObject.transform.GetComponent<Player>();

			player.equipment.onDequipRequested += (PlayerEquipment equipment, ref bool shouldAllow) =>
				OnDequipRequested?.Invoke(player, equipment, ref shouldAllow);

			player.life.onHealthUpdated += (byte newHealth) =>
				OnHealthUpdated?.Invoke(player, newHealth);

			player.life.onFoodUpdated += (byte newFood) =>
				OnFoodUpdated?.Invoke(player, newFood);

			player.life.onWaterUpdated += (byte newWater) =>
				OnWaterUpdated?.Invoke(player, newWater);

			player.life.onVirusUpdated += (byte newVirus) =>
				OnVirusUpdated?.Invoke(player, newVirus);

			player.life.onStaminaUpdated += (byte newStamina) =>
				OnStaminaUpdated?.Invoke(player, newStamina);

			player.life.onVisionUpdated += (bool isViewing) =>
				OnVisionUpdated?.Invoke(player, isViewing);

			player.life.onOxygenUpdated += (byte newOxygen) =>
				OnOxygenUpdated?.Invoke(player, newOxygen);

			player.life.onBleedingUpdated += (bool newBleeding) =>
				OnBleedingUpdated?.Invoke(player, newBleeding);

			player.life.onBrokenUpdated += (bool newBroken) =>
				OnBrokenUpdated?.Invoke(player, newBroken);

			player.life.onTemperatureUpdated += (EPlayerTemperature newTemperature) =>
				OnTemperatureUpdated?.Invoke(player, newTemperature);

			player.movement.onSafetyUpdated += (bool isSafe) =>
				OnSafetyUpdated?.Invoke(player, isSafe);

			player.movement.onRadiationUpdated += (bool isRadiated) =>
				OnRadiationUpdated?.Invoke(player, isRadiated);
		}

		public delegate void DequipRequested(Player player, PlayerEquipment equipment, ref bool shouldAllow);

		public delegate void HealthUpdated(Player player, byte newHealth);

		public delegate void FoodUpdated(Player player, byte newFood);

		public delegate void WaterUpdated(Player player, byte newWater);

		public delegate void VirusUpdated(Player player, byte newVirus);

		public delegate void StaminaUpdated(Player player, byte newStamina);

		public delegate void VisionUpdated(Player player, bool isViewing);

		public delegate void OxygenUpdated(Player player, byte newOxygen);

		public delegate void BleedingUpdated(Player player, bool newBleeding);

		public delegate void BrokenUpdated(Player player, bool newBroken);

		public delegate void TemperatureUpdated(Player player, EPlayerTemperature newTemperature);

		public delegate void SafetyUpdated(Player player, bool isSafe);

		public delegate void RadiationUpdated(Player player, bool isRadiated);
	}
}
