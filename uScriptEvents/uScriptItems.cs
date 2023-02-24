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

		[ScriptClass("itemN")]
		public class ItemNClass
		{
			public Item Item { get; set; }

			public ItemNClass(Item item)
			{
				this.Item = item;
			}

			[ScriptProperty(null)]
			public float id
			{
				get
				{
					return this.Item.id;
				}
			}

			[ScriptProperty(null)]
			public string name
			{
				get
				{
					return ((ItemAsset)Assets.find(EAssetType.ITEM, Item.id))?.itemName ?? "Unknown";
				}
			}

			[ScriptProperty(null)]
			public string description
			{
				get
				{
					return ((ItemAsset)Assets.find(EAssetType.ITEM, Item.id))?.itemDescription ?? "Unknown"; 
				}
			}

			[ScriptProperty(null)]
			public byte durability
			{
				get
				{
					return this.Item.durability;
				}
				set
				{
					this.Item.durability = value;
				}
			}

			[ScriptProperty(null)]
			public string itemType
			{
				get
				{
					return this.Item.GetType().ToString();
					
				}
			}

			[ScriptProperty(null)]
			public string rarity
			{
				get
				{
					return this.Item.GetAsset().rarity.ToString();

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
			ItemGunAsset gun = item.Item.interactableItem.GetComponent<ItemGunAsset>();
			if (!(gun != null)) return null;
			return new GunClass(gun);
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
			return ((ItemAsset)Assets.find(EAssetType.ITEM, item.Id))?.itemDescription ?? "Unknown";
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

		[ScriptFunction("get_rarity")]
		public static string getRarity([ScriptInstance] ExpressionValue instance)
		{
			if (!(instance.Data is ItemClass item)) return null;
			ItemAsset asset = item.Item.GetAsset<ItemAsset>();
			if (!(asset != null)) return null;
			return asset.rarity.ToString();
		}
	}
}
