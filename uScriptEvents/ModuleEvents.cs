﻿using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using uScript.Module.Main;
using uScript.Module.Main.Classes;
using uScript.Unturned;
using static ModuleEvents.InternalEvents;

namespace ModuleEvents
{
	public class ModuleEvents : ScriptModuleBase
	{
		protected override void OnModuleLoaded()
		{
			U.Events.OnBeforePlayerConnected += ModuleEvents.AddComponent;

			BarricadeManager.onDamageBarricadeRequested += (CSteamID instigatorSteamID, Transform barricadeTransform, ref ushort pendingTotalDamage, ref bool shouldAllow, EDamageOrigin damageOrigin) =>
			{
				ModuleEvents.onDamageBarricade.Invoke(instigatorSteamID, barricadeTransform, ref pendingTotalDamage, ref shouldAllow, damageOrigin);
			};

			StructureManager.onDamageStructureRequested += (CSteamID instigatorSteamID, Transform structureTransform, ref ushort pendingTotalDamage, ref bool shouldAllow, EDamageOrigin damageOrigin) =>
			{
				ModuleEvents.onDamageStructure.Invoke(instigatorSteamID, structureTransform, ref pendingTotalDamage, ref shouldAllow, damageOrigin);
			};

			ResourceManager.onDamageResourceRequested += (CSteamID instigatorSteamID, Transform objectTransform, ref ushort pendingTotalDamage, ref bool shouldAllow, EDamageOrigin damageOrigin) =>
			{
				ModuleEvents.onDamageResourceRequested.Invoke(instigatorSteamID, objectTransform, ref pendingTotalDamage, ref shouldAllow, damageOrigin);
			};

			PlayerInput.onPluginKeyTick += (Player player, uint simulation, byte key, bool state) =>
			{
				ModuleEvents.onPluginKeyTick.Invoke(player, simulation, key, state);
			};

			ItemManager.onTakeItemRequested += (Player player, byte x, byte y, uint instanceID, byte to_x, byte to_y, byte to_rot, byte to_page, ItemData itemData, ref bool shouldAllow) =>
			{
				ModuleEvents.onTakeItemRequested.Invoke(player, x, y, instanceID, to_x, to_y, to_rot, to_page, itemData, ref shouldAllow);
			};

			VehicleManager.onSiphonVehicleRequested += (InteractableVehicle vehicle, Player instigatingPlayer, ref bool shouldAllow, ref ushort desiredAmount) =>
			{
				ModuleEvents.onSiphonVehicleRequested.Invoke(vehicle, instigatingPlayer, ref shouldAllow, ref desiredAmount);
			};

			BarricadeManager.onBarricadeSpawned += (BarricadeRegion region, BarricadeDrop drop) =>
			{
				ModuleEvents.onBarricadeSpawned.Invoke(region, drop);
			};

			StructureManager.onStructureSpawned += (StructureRegion region, StructureDrop drop) =>
			{
				ModuleEvents.onStructureSpawned.Invoke(region, drop);
			};
		}

		public static event DamageBarricadeRequestHandler onDamageBarricade;

		public static event DamageResourceRequestHandler onDamageResourceRequested;

		public static event PluginKeyTickHandler onPluginKeyTick;

		public static event TakeItemRequestHandler onTakeItemRequested;

		public static event SiphonVehicleRequestHandler onSiphonVehicleRequested;

		public static event BarricadeSpawnedHandler onBarricadeSpawned;

		public static event StructureSpawnedHandler onStructureSpawned;

		public static event DamageStructureRequestHandler onDamageStructure;

		private static void AddComponent(UnturnedPlayer player)
		{
			player.Player.gameObject.AddComponent<InternalEvents>();
		}
	}

	public class InternalEvents : MonoBehaviour
	{
		public static event InternalEvents.DequipRequested OnDequipRequested;

		public static event InternalEvents.HealthUpdated OnHealthUpdated;

		public static event InternalEvents.FoodUpdated OnFoodUpdated;

		public static event InternalEvents.WaterUpdated OnWaterUpdated;

		public static event InternalEvents.VirusUpdated OnVirusUpdated;

		public static event InternalEvents.StaminaUpdated OnStaminaUpdated;

		public static event InternalEvents.VisionUpdated OnVisionUpdated;

		public static event InternalEvents.OxygenUpdated OnOxygenUpdated;

		public static event InternalEvents.BleedingUpdated OnBleedingUpdated;

		public static event InternalEvents.BrokenUpdated OnBrokenUpdated;

		public static event InternalEvents.TemperatureUpdated OnTemperatureUpdated;

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
				OnWaterUpdated?.Invoke(player, newVirus);

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
	}
}