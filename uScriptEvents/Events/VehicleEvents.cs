using HarmonyLib;
using JetBrains.Annotations;
using Rocket.Core.Logging;
using SDG.NetTransport;
using SDG.Unturned;
using System.Runtime.Remoting.Contexts;
using UnityEngine;
using uScript.Unturned;

namespace uScriptClothingEvents
{
	public class VehicleEvents
	{
		public delegate void OnVehicleHornDelegate(Player player, InteractableVehicle vehicle, ref bool cancel);
		public delegate void OnVehicleHeadLightsDelegate(Player player, InteractableVehicle vehicle, ref bool cancel);
		public delegate void OnVehicleHookDelegate(Player player, InteractableVehicle vehicle, ref bool cancel);

		public static event OnVehicleHornDelegate OnVehicleHorn;
		public static event OnVehicleHeadLightsDelegate OnVehicleHeadLightsUpdated;
		public static event OnVehicleHookDelegate OnVehicleHook;

		[UsedImplicitly]
		[HarmonyPatch]
		public static class Patches
		{
			[UsedImplicitly]
			[HarmonyPatch(typeof(VehicleManager))]
			[HarmonyPatch("ReceiveVehicleHornRequest")]
			[HarmonyPrefix]
			public static bool ReceiveVehicleHornRequest(in ServerInvocationContext context)
			{
				var cancel = false;
				Player player = context.GetPlayer();
				if (!(player == null))
				{
					InteractableVehicle vehicle = player.movement.getVehicle();
					if (!(vehicle == null) && vehicle.asset.hasHorn && vehicle.canUseHorn && vehicle.checkDriver(player.channel.owner.playerID.steamID))
					{
						OnVehicleHorn?.Invoke(player, vehicle, ref cancel);
						return !cancel;
					}
				}
				return !cancel;
			}

			[UsedImplicitly]
			[HarmonyPatch(typeof(VehicleManager))]
			[HarmonyPatch("ReceiveToggleVehicleHeadlights")]
			[HarmonyPrefix]
			public static bool ReceiveToggleVehicleHeadlights(in ServerInvocationContext context, bool wantsHeadlightsOn)
			{
				var cancel = false;
				Player player = context.GetPlayer();
				if (!(player == null))
				{
					InteractableVehicle vehicle = player.movement.getVehicle();
					if (!(vehicle == null) && wantsHeadlightsOn != vehicle.headlightsOn && vehicle.canTurnOnLights && vehicle.checkDriver(player.channel.owner.playerID.steamID) && vehicle.asset.hasHeadlights)
					{
						OnVehicleHeadLightsUpdated?.Invoke(player, vehicle, ref cancel);
					}
				}
				return !cancel;
			}

			[UsedImplicitly]
			[HarmonyPatch(typeof(VehicleManager))]
			[HarmonyPatch("ReceiveUseVehicleBonus")]
			[HarmonyPrefix]
			public static bool ReceiveUseVehicleBonus(in ServerInvocationContext context, byte bonusType)
			{
				var cancel = false;
				Player player = context.GetPlayer();
				if (player == null) return !cancel;

				InteractableVehicle vehicle = player.movement.getVehicle();
				if (vehicle == null || !vehicle.checkDriver(player.channel.owner.playerID.steamID)) return !cancel;

				if(bonusType == 1) OnVehicleHook?.Invoke(player, vehicle, ref cancel);

				return !cancel;
			}
		}
	}
}
