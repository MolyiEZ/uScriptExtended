using HarmonyLib;
using JetBrains.Annotations;
using Rocket.Core.Logging;
using SDG.NetTransport;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
using UnityEngine;
using UnityEngine.UI;
using uScript.Unturned;
using uScriptPlayers;

namespace uScriptClothingEvents
{
	public class VehicleEvents
	{
		public delegate void OnVehicleHornDelegate(Player player, InteractableVehicle vehicle, ref bool cancel);
		public delegate void OnVehicleHeadLightsDelegate(Player player, InteractableVehicle vehicle, ref bool cancel);
		public delegate void OnVehicleHookDelegate(Player player, InteractableVehicle vehicle, InteractableVehicle vehicleHooked, ref bool cancel);
		public delegate void OnVehicleHookReleaseDelegate(Player player, InteractableVehicle vehicle, InteractableVehicle vehicleHooked, ref bool cancel);

		public static event OnVehicleHornDelegate OnVehicleHorn;
		public static event OnVehicleHeadLightsDelegate OnVehicleHeadLightsUpdated;
		public static event OnVehicleHookDelegate OnVehicleHook;
		public static event OnVehicleHookReleaseDelegate OnVehicleHookRelease;

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
			[HarmonyPatch(typeof(InteractableVehicle))]
			[HarmonyPatch("useHook")]
			[HarmonyPrefix]
			public static bool useHook(InteractableVehicle __instance)
			{
				var cancel = false;
				if (!__instance.isDriven) return false;
				Player player = __instance.passengers[0].player.player;
				if (player == null) return false;

				var hookedF = AccessTools.Field(typeof(InteractableVehicle), "hooked");
				var hooked = (List<HookInfo>)hookedF.GetValue(__instance);

				var clearHookedMethod = AccessTools.Method(typeof(InteractableVehicle), "clearHooked");

				if (hooked.Count > 0)
				{
					clearHookedMethod.Invoke(__instance, null);
					return false;
				}

				var grabF = AccessTools.Field(typeof(InteractableVehicle), "grab");
				var grab = (Collider[])grabF.GetValue(__instance);

				var hookF = AccessTools.Field(typeof(InteractableVehicle), "hook");
				var hook = (Transform)hookF.GetValue(__instance);

				var ignoreCollisionWithVehicleMethod = AccessTools.Method(typeof(InteractableVehicle), "ignoreCollisionWithVehicle", new[] { typeof(InteractableVehicle), typeof(bool) });


				int num = Physics.OverlapSphereNonAlloc(hook.position, 3f, grab, 67108864);
				for (int i = 0; i < num; i++)
				{
					InteractableVehicle vehicle = DamageTool.getVehicle(grab[i].transform);
					if (!(vehicle == null) && !(vehicle == __instance) && vehicle.isEmpty && !vehicle.isHooked && !vehicle.isExploded && vehicle.asset.engine != EEngine.TRAIN)
					{
						OnVehicleHook?.Invoke(player, __instance, vehicle, ref cancel);
						if (cancel) return false;
						HookInfo hookInfo = new HookInfo();
						hookInfo.target = vehicle.transform;
						hookInfo.vehicle = vehicle;
						hookInfo.deltaPosition = hook.InverseTransformPoint(vehicle.transform.position);
						hookInfo.deltaRotation = Quaternion.FromToRotation(hook.forward, vehicle.transform.forward);
						hooked.Add(hookInfo);
						vehicle.isHooked = true;
						ignoreCollisionWithVehicleMethod.Invoke(__instance, new object[] { vehicle, true });
					}
				}

				return false;
			}

			[UsedImplicitly]
			[HarmonyPatch(typeof(InteractableVehicle))]
			[HarmonyPatch("clearHooked")]
			[HarmonyPrefix]
			public static bool clearHooked(InteractableVehicle __instance)
			{
				var cancel = false;
				var hookedF = AccessTools.Field(typeof(InteractableVehicle), "hooked");
				var hooked = (List<HookInfo>)hookedF.GetValue(__instance);

				if (!__instance.isDriven)
				{
					hooked.Clear();
					return false;
				}
				Player player = __instance.passengers[0].player.player;
				if (player == null)
				{
					hooked.Clear();
					return false;
				}

				var ignoreCollisionWithVehicleMethod = AccessTools.Method(typeof(InteractableVehicle), "ignoreCollisionWithVehicle", new[] { typeof(InteractableVehicle), typeof(bool) });

				foreach (HookInfo item in hooked)
				{
					if (!(item.vehicle == null))
					{
						OnVehicleHookRelease?.Invoke(player, __instance, item.vehicle, ref cancel);
						if (cancel) return false;
						item.vehicle.isHooked = false;
						ignoreCollisionWithVehicleMethod.Invoke(__instance, new object[] { item.vehicle, false });
					}
				}

				hooked.Clear();

				return false;
			}
		}
	}
}
