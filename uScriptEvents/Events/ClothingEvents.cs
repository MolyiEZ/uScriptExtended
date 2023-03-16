using HarmonyLib;
using JetBrains.Annotations;
using Rocket.Core.Logging;
using SDG.Unturned;
using UnityEngine;
using uScript.Unturned;
using static uScript.Module.Main.PlayerEvents;

namespace uScriptClothingEvents
{
	public class ClothingEvents
	{
		public delegate void OnPlayerEquippingClothing(Player player, ItemJar item, byte quality, byte[] state, EClothingSlot type, ref bool cancel);
		public delegate void OnPlayerUnequippingClothing(Player player, ItemJar item, byte quality, byte[] state, EClothingSlot type, ref bool cancel);

		public static event OnPlayerEquippingClothing OnEquippingClothing;
		public static event OnPlayerUnequippingClothing OnUnequippingClothing;

		[UsedImplicitly]
		[HarmonyPatch]
		public static class Patches
		{
			[UsedImplicitly]
			[HarmonyPatch(typeof(PlayerClothing), nameof(PlayerClothing.askWearBackpack),
				typeof(ItemBackpackAsset), typeof(byte), typeof(byte[]), typeof(bool))]
			[HarmonyPrefix]
			public static bool AskWearBackpack(PlayerClothing __instance, ItemBackpackAsset asset, byte quality, byte[] state)
			{
				var cancel = false;
				if (asset is not ItemBackpackAsset backpackAsset)
				{
					var play = __instance.player.clothing;
					var item = new ItemJar(new Item(play.backpack, 1, play.backpackQuality, play.backpackState));
					OnUnequippingClothing?.Invoke(__instance.player, item, quality, state, EClothingSlot.BACKPACK, ref cancel);
					return !cancel;
				}
				else
				{
					var item = new ItemJar(new Item(backpackAsset.id, 1, quality, state));
					OnEquippingClothing?.Invoke(__instance.player, item, quality, state, EClothingSlot.BACKPACK, ref cancel);
					return !cancel;
				}
			}

			[UsedImplicitly]
			[HarmonyPatch(typeof(PlayerClothing), nameof(PlayerClothing.askWearGlasses),
				typeof(ItemGlassesAsset), typeof(byte), typeof(byte[]), typeof(bool))]
			[HarmonyPrefix]
			public static bool AskWearGlasses(PlayerClothing __instance, ItemGlassesAsset asset, byte quality, byte[] state)
			{
				var cancel = false;
				if (asset is not ItemGlassesAsset glassesAsset)
				{
					var play = __instance.player.clothing;
					var item = new ItemJar(new Item(play.glasses, 1, play.glassesQuality, play.glassesState));
					OnUnequippingClothing?.Invoke(__instance.player, item, quality, state, EClothingSlot.GLASSES, ref cancel);
					return !cancel;
				}
				else
				{
					var item = new ItemJar(new Item(glassesAsset.id, 1, quality, state));
					OnEquippingClothing?.Invoke(__instance.player, item, quality, state, EClothingSlot.GLASSES, ref cancel);
					return !cancel;
				}
			}

			[UsedImplicitly]
			[HarmonyPatch(typeof(PlayerClothing), nameof(PlayerClothing.askWearHat),
				typeof(ItemHatAsset), typeof(byte), typeof(byte[]), typeof(bool))]
			[HarmonyPrefix]
			public static bool AskWearHat(PlayerClothing __instance, ItemHatAsset asset, byte quality, byte[] state)
			{
				var cancel = false;
				if (asset is not ItemHatAsset hatAsset)
				{
					var play = __instance.player.clothing;
					var item = new ItemJar(new Item(play.hat, 1, play.hatQuality, play.hatState));
					OnUnequippingClothing?.Invoke(__instance.player, item, quality, state, EClothingSlot.HAT, ref cancel);
					return !cancel;
				}
				else
				{
					var item = new ItemJar(new Item(hatAsset.id, 1, quality, state));
					OnEquippingClothing?.Invoke(__instance.player, item, quality, state, EClothingSlot.HAT, ref cancel);
					return !cancel;
				}
			}

			[UsedImplicitly]
			[HarmonyPatch(typeof(PlayerClothing), nameof(PlayerClothing.askWearMask),
				typeof(ItemMaskAsset), typeof(byte), typeof(byte[]), typeof(bool))]
			[HarmonyPrefix]
			public static bool AskWearMask(PlayerClothing __instance, ItemMaskAsset asset, byte quality, byte[] state)
			{
				var cancel = false;
				if (asset is not ItemMaskAsset maskAsset)
				{
					var play = __instance.player.clothing;
					var item = new ItemJar(new Item(play.mask, 1, play.maskQuality, play.maskState));
					OnUnequippingClothing?.Invoke(__instance.player, item, quality, state, EClothingSlot.MASK, ref cancel);
					return !cancel;
				}
				else
				{
					var item = new ItemJar(new Item(maskAsset.id, 1, quality, state));
					OnEquippingClothing?.Invoke(__instance.player, item, quality, state, EClothingSlot.MASK, ref cancel);
					return !cancel;
				}
			}

			[UsedImplicitly]
			[HarmonyPatch(typeof(PlayerClothing), nameof(PlayerClothing.askWearPants),
				typeof(ItemPantsAsset), typeof(byte), typeof(byte[]), typeof(bool))]
			[HarmonyPrefix]
			public static bool AskWearPants(PlayerClothing __instance, ItemPantsAsset asset, byte quality, byte[] state)
			{
				var cancel = false;
				if (asset is not ItemPantsAsset pantsAsset)
				{
					var play = __instance.player.clothing;
					var item = new ItemJar(new Item(play.pants, 1, play.pantsQuality, play.pantsState));
					OnUnequippingClothing?.Invoke(__instance.player, item, quality, state, EClothingSlot.PANTS, ref cancel);
					return !cancel;
				}
				else
				{
					var item = new ItemJar(new Item(pantsAsset.id, 1, quality, state));
					OnEquippingClothing?.Invoke(__instance.player, item, quality, state, EClothingSlot.PANTS, ref cancel);
					return !cancel;
				}
			}

			[UsedImplicitly]
			[HarmonyPatch(typeof(PlayerClothing), nameof(PlayerClothing.askWearShirt),
				typeof(ItemShirtAsset), typeof(byte), typeof(byte[]), typeof(bool))]
			[HarmonyPrefix]
			public static bool AskWearShirt(PlayerClothing __instance, ItemShirtAsset asset, byte quality, byte[] state)
			{
				var cancel = false;
				if (asset is not ItemShirtAsset shirtAsset)
				{
					var play = __instance.player.clothing;
					var item = new ItemJar(new Item(play.shirt, 1, play.shirtQuality, play.shirtState));
					OnUnequippingClothing?.Invoke(__instance.player, item, quality, state, EClothingSlot.SHIRT, ref cancel);
					return !cancel;
				}
				else
				{
					var item = new ItemJar(new Item(shirtAsset.id, 1, quality, state));
					OnEquippingClothing?.Invoke(__instance.player, item, quality, state, EClothingSlot.SHIRT, ref cancel);
					return !cancel;
				}
			}

			[UsedImplicitly]
			[HarmonyPatch(typeof(PlayerClothing), nameof(PlayerClothing.askWearVest),
				typeof(ItemVestAsset), typeof(byte), typeof(byte[]), typeof(bool))]
			[HarmonyPrefix]
			public static bool AskWearVest(PlayerClothing __instance, ItemVestAsset asset, byte quality, byte[] state)
			{
				var cancel = false;
				if (asset is not ItemVestAsset vestAsset)
				{
					var play = __instance.player.clothing;
					var item = new ItemJar(new Item(play.vest, 1, play.vestQuality, play.vestState));
					OnUnequippingClothing?.Invoke(__instance.player, item, quality, state, EClothingSlot.VEST, ref cancel);
					return !cancel;
				}
				else
				{
					var item = new ItemJar(new Item(vestAsset.id, 1, quality, state));
					OnEquippingClothing?.Invoke(__instance.player, item, quality, state, EClothingSlot.VEST, ref cancel);
					return !cancel;
				}
			}
		}
	}
}
