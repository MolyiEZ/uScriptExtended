using Newtonsoft.Json.Linq;
using Rocket.Unturned.Events;
using SDG.Framework.Devkit.Interactable;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using uScript.API.Attributes;
using uScript.Core;
using uScript.Module.Main.Classes;
using uScript.Unturned;
using static uScriptBarricades.Generator;
using static uScriptEvents.AnimalFunction;
using static uScriptEvents.ZombieFunction;

namespace uScriptBarricades
{
	public class Generator : ScriptModuleBase
	{
		[ScriptClass("generator")]
		public class GeneratorClass
		{
			public InteractableGenerator Generator { get; set; }

			public GeneratorClass(InteractableGenerator generator)
			{
				this.Generator = generator;
			}

			[ScriptProperty(null)]
			public BarricadeClass barricade
			{
				get
				{
					return new BarricadeClass(this.Generator.transform);
				}
			}

			[ScriptProperty(null)]
			public bool powered
			{
				get
				{
					return this.Generator.isPowered;
				}
				set 
				{
					BarricadeManager.ServerSetGeneratorPowered(this.Generator, value);
				}
			}

			[ScriptProperty(null)]
			public float wirerange
			{
				get 
				{
					return this.Generator.wirerange;
				}
			}

			[ScriptProperty(null)]
			public float fuel
			{
				get
				{
					return this.Generator.fuel;
				}
			}
		}

		[ScriptClass("fire")]
		public class FireClass 
		{
			public InteractableFire Fire { get; set; }

			public FireClass(InteractableFire fire)
			{
				this.Fire = fire;
			}

			[ScriptProperty(null)]
			public BarricadeClass barricade
			{
				get
				{
					return new BarricadeClass(this.Fire.transform);
				}
			}

			[ScriptProperty(null)]
			public bool lit
			{
				get 
				{
					return this.Fire.isLit;
				}
				set 
				{
					BarricadeManager.ServerSetFireLit(this.Fire, value);
				}
			}
		}

		[ScriptClass("oven")]
		public class OvenClass
		{
			public InteractableOven Oven { get; set; }

			public OvenClass(InteractableOven oven)
			{
				this.Oven = oven;
			}

			[ScriptProperty(null)]
			public BarricadeClass barricade
			{
				get
				{
					return new BarricadeClass(this.Oven.transform);
				}
			}

			[ScriptProperty(null)]
			public bool lit
			{
				get
				{
					return this.Oven.isLit;
				}
				set
				{
					BarricadeManager.ServerSetOvenLit(this.Oven, value);
				}
			}
		}

		[ScriptClass("hit")]
		public class HitClass
		{
			public InputInfo Hit { get; set; }

			public HitClass(InputInfo hit)
			{
				this.Hit = hit;
			}

			[ScriptProperty(null)]
			public Vector3Class normal
			{
				get
				{
					return new Vector3Class(this.Hit.normal);
				}
			}

			[ScriptProperty(null)]
			public Vector3Class direction
			{
				get
				{
					return new Vector3Class(this.Hit.direction);
				}
			}

			[ScriptProperty(null)]
			public Vector3Class point
			{
				get
				{
					return new Vector3Class(this.Hit.point);
				}
			}

			[ScriptProperty(null)]
			public PlayerClass player
			{
				get
				{
					return new PlayerClass(this.Hit.player);
				}
			}

			[ScriptProperty(null)]
			public BarricadeClass barricade
			{
				get
				{
					BarricadeDrop barricadeDrop = BarricadeManager.FindBarricadeByRootTransform(this.Hit.transform);
					if (!(barricadeDrop != null)) return null;
					return new BarricadeClass(barricadeDrop.model);
				}
			}

			[ScriptProperty(null)]
			public VehicleClass vehicle
			{
				get
				{
					return new VehicleClass(this.Hit.vehicle);
				}
			}

			[ScriptProperty(null)]
			public AnimalClass animal
			{
				get
				{
					return new AnimalClass(this.Hit.animal);
				}
			}

			[ScriptProperty(null)]
			public ZombieClass zombie
			{
				get
				{
					return new ZombieClass(this.Hit.zombie);
				}
			}

			[ScriptProperty(null)]
			public string limb
			{
				get
				{
					return this.Hit.limb.ToString();
				}
			}

			[ScriptProperty(null)]
			public string type
			{
				get
				{
					return this.Hit.type.ToString();
				}
			}

		}

		[ScriptClass("farm")]
		public class FarmClass
		{
			public InteractableFarm Farm { get; set; }

			public FarmClass(InteractableFarm farm)
			{
				this.Farm = farm;
			}

			[ScriptProperty(null)]
			public BarricadeClass barricade
			{
				get
				{
					return new BarricadeClass(this.Farm.transform);
				}
			}

			[ScriptProperty(null)]
			public bool isFullyGrown
			{
				get
				{
					return this.Farm.IsFullyGrown;
				}
			}

			[ScriptProperty(null)]
			public bool canFertilize
			{
				get
				{
					return this.Farm.canFertilize;
				}
			}

			[ScriptProperty(null)]
			public ushort grow
			{
				get
				{
					return this.Farm.grow;
				}
			}

			[ScriptProperty(null)]
			public ushort growth
			{
				get
				{
					return (ushort)this.Farm.growth;
				}
			}

			[ScriptProperty(null)]
			public uint harvestExperience
			{
				get
				{
					return this.Farm.harvestRewardExperience;
				}
				set
				{
					this.Farm.harvestRewardExperience = value;
				}
			}
		}
	}

	[ScriptTypeExtension(typeof(BarricadeClass))]
	public class FunctionBarricade
	{
		[ScriptFunction("get_generator")]
		public static GeneratorClass getGenerator([ScriptInstance] ExpressionValue instance)
		{
			if (!(instance.Data is BarricadeClass barricade)) return null;
			InteractableGenerator component = barricade.BarricadeTransform.GetComponent<InteractableGenerator>();
			if (!(component != null)) return null;
			return new GeneratorClass(component);
		}

		[ScriptFunction("get_fire")]
		public static FireClass getFire([ScriptInstance] ExpressionValue instance)
		{
			if (!(instance.Data is BarricadeClass barricade)) return null;
			InteractableFire component = barricade.BarricadeTransform.GetComponent<InteractableFire>();
			if (!(component != null)) return null;
			return new FireClass(component);
		}

		[ScriptFunction("get_oven")]
		public static OvenClass getOven([ScriptInstance] ExpressionValue instance)
		{
			if (!(instance.Data is BarricadeClass barricade)) return null;
			InteractableOven component = barricade.BarricadeTransform.GetComponent<InteractableOven>();
			if (!(component != null)) return null;
			return new OvenClass(component);
		}

		[ScriptFunction("get_farm")]
		public static FarmClass getFarm([ScriptInstance] ExpressionValue instance)
		{
			if (!(instance.Data is BarricadeClass barricade)) return null;
			InteractableFarm component = barricade.BarricadeTransform.GetComponent<InteractableFarm>();
			if (!(component != null)) return null;
			return new FarmClass(component);
		}

		[ScriptFunction("get_isWired")]
		public static bool getIsWired([ScriptInstance] ExpressionValue instance)
		{
			if (!(instance.Data is BarricadeClass barricade))
				return false;

			Vector3 barricadePosition = barricade.BarricadeTransform.position;
			float maxPowerRange = PowerTool.MAX_POWER_RANGE;

			if (Level.info != null && Level.info.configData != null && Level.info.configData.Has_Global_Electricity)
			{
				return true;
			}

			if (BarricadeManager.FindBarricadeByRootTransform(barricade.BarricadeTransform).interactable?.isPlant == true)
			{
				byte x, y;
				ushort plantIndex;
				BarricadeRegion plantRegion;
				BarricadeManager.tryGetPlant(barricade.BarricadeTransform.parent, out x, out y, out plantIndex, out plantRegion);

				List<InteractableGenerator> generators = PowerTool.checkGenerators(barricadePosition, maxPowerRange, plantIndex);

				foreach (InteractableGenerator generator in generators)
				{
					if (generator.isPowered && generator.fuel > 0 && (generator.transform.position - barricadePosition).sqrMagnitude < generator.sqrWirerange)
					{
						return true;
					}
				}

				return false;
			}
			else
			{
				List<InteractableGenerator> generators = PowerTool.checkGenerators(barricadePosition, maxPowerRange, ushort.MaxValue);

				foreach (InteractableGenerator generator in generators)
				{
					if (generator.isPowered && generator.fuel > 0 && (generator.transform.position - barricadePosition).sqrMagnitude < generator.sqrWirerange)
					{
						return true;
					}
				}

				return false;
			}
		}

		[ScriptFunction("get_anglex")]
		public static float anglex([ScriptInstance] ExpressionValue instance)
		{
			if (!(instance.Data is BarricadeClass barricade)) return 0;
			BarricadeDrop barricadeDrop = BarricadeManager.FindBarricadeByRootTransform(barricade.BarricadeTransform);
			return MeasurementTool.byteToAngle(barricadeDrop.GetServersideData().angle_x);
		}

		[ScriptFunction("get_angley")]
		public static float angley([ScriptInstance] ExpressionValue instance)
		{
			if (!(instance.Data is BarricadeClass barricade)) return 0;
			BarricadeDrop barricadeDrop = BarricadeManager.FindBarricadeByRootTransform(barricade.BarricadeTransform);
			return MeasurementTool.byteToAngle(barricadeDrop.GetServersideData().angle_y);
		}

		[ScriptFunction("get_anglez")]
		public static float anglez([ScriptInstance] ExpressionValue instance)
		{
			if (!(instance.Data is BarricadeClass barricade)) return 0;
			BarricadeDrop barricadeDrop = BarricadeManager.FindBarricadeByRootTransform(barricade.BarricadeTransform);
			return MeasurementTool.byteToAngle(barricadeDrop.GetServersideData().angle_z);
		}
	}
}
