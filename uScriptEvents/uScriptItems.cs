using SDG.Unturned;
using Steamworks; 
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using uScript.API.Attributes;
using uScript.Core;
using uScript.Module.Main.Classes;
using uScript.Unturned;
using static uScriptItems.Gun;

namespace uScriptItems
{
	public class Gun : ScriptModuleBase
	{
		[ScriptClass("gun")]
		public class GunClass
		{
			public ItemGunAsset Gun { get; set; }

			public GunClass(ItemGunAsset gun)
			{
				this.Gun = gun;
			}

			[ScriptProperty(null)]
			public ItemClass item
			{
				get
				{
					return new ItemClass(this.Gun.item.GetComponent<ItemJar>());
				}
			}

			[ScriptProperty(null)]
			public float range
			{
				get
				{
					return this.Gun.range;
				}
				set
				{
					this.Gun.range = value;
				}
			}

			[ScriptProperty(null)]
			public float damage
			{
				get
				{
					return this.Gun.objectDamage;
				}
				set
				{
					this.Gun.objectDamage = value;
				}
			}

			[ScriptProperty(null)]
			public float fireRate
			{
				get
				{
					return this.Gun.firerate;
				}
				set
				{
					this.Gun.firerate = (byte)value;
				}
			}

			[ScriptProperty(null)]
			public float ammoMax
			{
				get
				{
					return this.Gun.ammoMax;
				}
				set
				{
					this.Gun.ammoMax = (byte)value;
				}
			}

			[ScriptProperty(null)]
			public float ammoMin
			{
				get
				{
					return this.Gun.ammoMin;
				}
				set
				{
					this.Gun.ammoMin = (byte)value;
				}
			}

			[ScriptProperty(null)]
			public float recoilProne
			{
				get
				{
					return this.Gun.recoilProne;
				}
				set
				{
					this.Gun.recoilProne = (byte)value;
				}
			}

			[ScriptProperty(null)]
			public float recoilAim
			{
				get
				{
					return this.Gun.recoilAim;
				}
				set
				{
					this.Gun.recoilAim = (byte)value;
				}
			}

			[ScriptProperty(null)]
			public float recoilCrouch
			{
				get
				{
					return this.Gun.recoilCrouch;
				}
				set
				{
					this.Gun.recoilCrouch = (byte)value;
				}
			}

			[ScriptProperty(null)]
			public float recoilSprint
			{
				get
				{
					return this.Gun.recoilSprint;
				}
				set
				{
					this.Gun.recoilSprint = (byte)value;
				}
			}

			[ScriptProperty(null)]
			public float recoilMultiplier
			{
				get
				{
					return this.Gun.aimingRecoilMultiplier;
				}
				set
				{
					this.Gun.aimingRecoilMultiplier = (byte)value;
				}
			}

			[ScriptProperty(null)]
			public float reloadTime
			{
				get
				{
					return this.Gun.reloadTime;
				}
				set
				{
					this.Gun.reloadTime = value;
				}
			}

			[ScriptProperty(null)]
			public float movementSpeed
			{
				get
				{
					return this.Gun.equipableMovementSpeedMultiplier;
				}
				set
				{
					this.Gun.equipableMovementSpeedMultiplier = value;
				}
			}

			[ScriptProperty(null)]
			public float fireDelay
			{
				get
				{
					return this.Gun.fireDelay;
				}
			}
		}

		[ScriptClass("consumeable")]
		public class ConsumeableClass
		{
			public ItemConsumeableAsset Consumeable { get; set; }

			public ConsumeableClass(ItemConsumeableAsset consumeable)
			{
				this.Consumeable = consumeable;
			}

			[ScriptProperty(null)]
			public ItemClass item
			{
				get
				{
					return new ItemClass(this.Consumeable.item.GetComponent<ItemJar>());
				}
			}

			[ScriptProperty(null)]
			public float food
			{
				get
				{
					return this.Consumeable.food;
				}
			}

			[ScriptProperty(null)]
			public float water
			{
				get
				{
					return this.Consumeable.water;
				}
			}
		}
	}

	[ScriptTypeExtension(typeof(ItemClass))]
	public class FunctionItem
	{
		[ScriptFunction("get_gun")]
		public static GunClass getGun([ScriptInstance] ExpressionValue instance)
		{
			if (!(instance.Data is ItemClass item)) return null;
			ItemGunAsset gun = item.Item.GetAsset<ItemGunAsset>();
			if (!(gun != null)) return null;
			return new GunClass(gun);
		}

		[ScriptFunction("reloadTime")]
		public static void reloadTime([ScriptInstance] ExpressionValue instance, string player, ushort time)
		{
			if (!(instance.Data is ItemClass item)) return;
			ItemGunAsset gun = item.Item.GetAsset<ItemGunAsset>();
			if (!(gun != null)) return;

			ulong parsedPlayerID;
			bool hasParsedPlayerID = UInt64.TryParse(player, out parsedPlayerID);
			if (hasParsedPlayerID != true) return;

			Player playerGiven = PlayerTool.getPlayer(new CSteamID(parsedPlayerID));
			if (playerGiven == null) return;

			gun.reloadTime = (float)time;

			byte itemPage = 0;
			bool hasFoundItem = false;

			Items[] items = playerGiven.inventory.items;
			foreach (Items itemsElement in items)
			{
				if (itemsElement.containsItem(item.Item))
				{
					itemPage = itemsElement.page;
					hasFoundItem = true;
					break;
				}
			}

			if (hasFoundItem != true) return;
			playerGiven.equipment.sendUpdateState();
		}

		[ScriptFunction("get_consumeable")]
		public static ConsumeableClass getConsumeable([ScriptInstance] ExpressionValue instance)
		{
			if (!(instance.Data is ItemClass item)) return null;
			ItemConsumeableAsset consumeable = item.Item.GetAsset<ItemConsumeableAsset>();
			if (!(consumeable != null)) return null;
			return new ConsumeableClass(consumeable);
		}

		[ScriptFunction("get_description")]
		public static string getDescription([ScriptInstance] ExpressionValue instance)
		{
			if (!(instance.Data is ItemClass item)) return null;
			ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, item.Id);
			if (!(itemAsset != null)) return null;
			return itemAsset.itemDescription;
		}

		[ScriptFunction("get_durability")]
		public static ushort getDurability([ScriptInstance] ExpressionValue instance)
		{
			if (!(instance.Data is ItemClass item)) return 0;
			return item.Item.item.durability;
		}

		[ScriptFunction("set_durability")]
		public static void setDurability([ScriptInstance] ExpressionValue instance, ushort value)
		{
			if (!(instance.Data is ItemClass item)) return;
			item.Item.item.durability = (byte)value;
		}
	}
}
