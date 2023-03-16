using HarmonyLib;
using JetBrains.Annotations;
using Rocket.Core.Logging;
using SDG.Unturned;
using System.Reflection;
using System;
using UnityEngine;
using uScript.Unturned;
using static uScript.Module.Main.PlayerEvents;
using UnityEngine.UI;

namespace uScriptClothingEvents
{
	public class AnimalEvents
	{
		public delegate void AnimalSpawned(Animal nativeAnimal);
		public delegate void AnimalFleeing(Animal nativeAnimal, ref Vector3 direction, ref bool sendToPack, ref bool cancel);
		public delegate void AnimalAttackingPoint(Animal nativeAnimal, ref Vector3 point, ref bool sendToPack, ref bool cancel);
		public delegate void AnimalAttackingPlayer(Animal nativeAnimal, ref Player player, ref bool sendToPack, ref bool cancel);

		public static event AnimalSpawned OnAnimalAdded;
		public static event AnimalSpawned OnAnimalRevived;
		public static event AnimalFleeing OnAnimalFleeing;
		public static event AnimalAttackingPoint OnAnimalAttackingPoint;
		public static event AnimalAttackingPlayer OnAnimalAttackingPlayer;

		[UsedImplicitly]
		[HarmonyPatch]
		public static class Patches
		{
			[UsedImplicitly]
			[HarmonyPatch(typeof(AnimalManager), "addAnimal")]
			[HarmonyPostfix]
			public static void AddAnimal(Animal __result)
			{
				if (__result != null)
				{
					OnAnimalAdded?.Invoke(__result);
				}
			}

			[UsedImplicitly]
			[HarmonyPatch(typeof(Animal), nameof(Animal.tellAlive))]
			[HarmonyPostfix]
			public static void TellAlive(Animal __instance)
			{
				OnAnimalRevived?.Invoke(__instance);
			}

			[UsedImplicitly]
			[HarmonyPatch(typeof(Animal), nameof(Animal.alertDirection))]
			[HarmonyPrefix]
			public static bool AlertDirection(Animal __instance, ref Vector3 newDirection, ref bool sendToPack)
			{
				var cancel = false;

				OnAnimalFleeing?.Invoke(__instance, ref newDirection, ref sendToPack, ref cancel);

				return !cancel;
			}

			[UsedImplicitly]
			[HarmonyPatch(typeof(Animal), nameof(Animal.alertGoToPoint))]
			[HarmonyPrefix]
			public static bool AlertGoToPoint(Animal __instance, ref Vector3 point, ref bool sendToPack)
			{
				var cancel = false;

				OnAnimalAttackingPoint?.Invoke(__instance, ref point, ref sendToPack, ref cancel);

				return !cancel;
			}

			[UsedImplicitly]
			[HarmonyPatch(typeof(Animal), nameof(Animal.alertPlayer))]
			[HarmonyPrefix]
			public static bool AlertPlayer(Animal __instance, ref Player newPlayer, ref bool sendToPack)
			{
				var cancel = false;

				OnAnimalAttackingPlayer?.Invoke(__instance, ref newPlayer, ref sendToPack, ref cancel);

				return !cancel;
			}
		}
	}
}
