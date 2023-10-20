using HarmonyLib;
using SDG.Unturned;
using Steamworks; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using uScript.API.Attributes;
using uScript.Core;
using uScript.Module.Main.Classes;
using uScript.Unturned;
using uScriptExtended;
using static uScriptVehicle.VehicleLook;

namespace uScriptEvents
{
	public class AnimalFunction : ScriptModuleBase
	{
		public static Dictionary<string, ERagdollEffect> ragdolls = new Dictionary<string, ERagdollEffect>()
		{
			{"NONE", ERagdollEffect.NONE},
			{"BRONZE", ERagdollEffect.BRONZE},
			{"SILVER", ERagdollEffect.SILVER},
			{"GOLD", ERagdollEffect.GOLD},
			{"ZERO", ERagdollEffect.ZERO_KELVIN},
		};

		[ScriptClass("animal")]
		public class AnimalClass
		{
			public Animal Animal { get; set; }

			public AnimalClass(Animal animal)
			{
				this.Animal = animal;
			}

			[ScriptFunction(null)]
			public void moveTo(Vector3Class position) => this.Animal.alertGoToPoint(position.Vector3, true);

			[ScriptFunction(null)]
			public void runFrom(Vector3Class position) => this.Animal.alertRunAwayFromPoint(position.Vector3, true);

			[ScriptFunction(null)]
			public void startle() => AnimalManager.sendAnimalStartle(this.Animal);

			[ScriptFunction(null)]
			public void damage(ushort amount, bool dropLoot = true, string ragdoll = "NONE")
			{
				EPlayerKill playerKill;
				uint xp;
				if(ragdolls.TryGetValue(ragdoll, out ERagdollEffect ragdollEff)) Animal.askDamage(amount, this.Animal.transform.position, out playerKill, out xp, true, dropLoot, ragdollEff);
				else Rocket.Core.Logging.Logger.LogWarning("uScriptExtended module from uScript => Ragdoll must be 'NONE', 'BRONZE', 'GOLD', 'SILVER', or 'ZERO'.");
			}

			[ScriptFunction(null)]
			public void kill(string ragdoll = "NONE")
			{
				if (ragdolls.TryGetValue(ragdoll, out ERagdollEffect ragdollEff)) AnimalManager.sendAnimalDead(this.Animal, this.Animal.transform.position, ragdollEff);
				else Rocket.Core.Logging.Logger.LogWarning("uScriptExtended module from uScript => Ragdoll must be 'NONE', 'BRONZE', 'GOLD', 'SILVER', or 'ZERO'.");
			}

			[ScriptFunction(null)]
			public void dropLoot() => AnimalManager.dropLoot(this.Animal);

			[ScriptProperty(null)]
			public bool isAttacking
			{
				get
				{
					return Convert.ToBoolean(RUtil.getValue("isAttacking", typeof(Animal), this.Animal));
				}
				set
				{
					RUtil.setValue("isAttacking", typeof(Animal), this.Animal, value);
				}
			}

			[ScriptProperty(null)]
			public bool isMoving
			{
				get
				{
					return Convert.ToBoolean(RUtil.getValue("isMoving", typeof(Animal), this.Animal));
				}
				set
				{
					RUtil.setValue("isMoving", typeof(Animal), this.Animal, value);
				}
			}

			[ScriptProperty(null)]
			public bool isRunning
			{
				get
				{
					return Convert.ToBoolean(RUtil.getValue("isRunning", typeof(Animal), this.Animal));
				}
				set
				{
					RUtil.setValue("isRunning", typeof(Animal), this.Animal, value);
				}
			}

			[ScriptProperty(null)]
			public bool isWandering
			{
				get
				{
					return Convert.ToBoolean(RUtil.getValue("isWandering", typeof(Animal), this.Animal));
				}
				set
				{
					RUtil.setValue("isWandering", typeof(Animal), this.Animal, value);
				}
			}

			[ScriptProperty(null)]
			public bool isFleeing
			{
				get
				{
					return this.Animal.isFleeing;
				}
				set 
				{
					RUtil.setValue("_isFleeing", typeof(Animal), this.Animal, value);
				}
			}

			[ScriptProperty(null)]
			public bool isHunting
			{
				get
				{
					return this.Animal.isHunting;
				}
				set
				{
					RUtil.setValue("isHunting", typeof(Animal), this.Animal, value);
				}
				
			}

			[ScriptProperty(null)]
			public ushort instanceId
			{
				get
				{
					return this.Animal.index;
				}
			}

			[ScriptProperty(null)]
			public ushort health
			{
				get
				{
					return (ushort)this.Animal.GetHealth();
				}
				set 
				{
					RUtil.setValue("health", typeof(Animal), this.Animal, value);
				}
			}

			[ScriptProperty(null)]
			public ushort id
			{
				get
				{
					return this.Animal.id;
				}
			}

			[ScriptProperty(null)]
			public PlayerClass targetPlayer
			{
				get
				{
					return new PlayerClass(this.Animal.GetTargetPlayer());
				}
			}

			[ScriptProperty(null)]
			public Vector3Class targetPoint
			{
				get
				{
					return new Vector3Class(this.Animal.target);
				}
			}

			[ScriptProperty(null)]
			public Vector3Class position
			{
				get
				{
					return new Vector3Class(this.Animal.transform.position);
				}
				set 
				{
					this.Animal.transform.position = value.Vector3;
				}
			}

			[ScriptProperty(null)]
			public Vector3Class forward
			{
				get
				{
					return new Vector3Class(this.Animal.transform.forward);
				}
			}

			[ScriptProperty(null)]
			public Vector3Class backward
			{
				get
				{
					return new Vector3Class(-this.Animal.transform.forward);
				}
			}

			[ScriptProperty(null)]
			public Vector3Class right
			{
				get
				{
					return new Vector3Class(this.Animal.transform.right);
				}
			}

			[ScriptProperty(null)]
			public Vector3Class left
			{
				get
				{
					return new Vector3Class(-this.Animal.transform.right);
				}
			}

			[ScriptProperty(null)]
			public Vector3Class up
			{
				get
				{
					return new Vector3Class(this.Animal.transform.up);
				}
			}

			[ScriptProperty(null)]
			public Vector3Class down
			{
				get
				{
					return new Vector3Class(-this.Animal.transform.up);
				}
			}

			[ScriptFunction(null)]
			public AnimalLookClass Look(Vector3Class value)
			{
				return new AnimalLookClass(this.Animal, value.Vector3);
			}
		}

		[ScriptClass("animalLook")]
		public class AnimalLookClass
		{
			public Animal Animal { get; }
			public Vector3 Value { get;  }

			public AnimalLookClass(Animal animal, Vector3 value)
			{
				Animal = animal;
				Value = value;
			}

			[ScriptFunction(null)]
			public BarricadeClass GetBarricade()
			{
				Transform aim = this.Animal.transform;
				if (!Physics.Raycast(aim.position, this.Value, out var hitInfo, float.PositiveInfinity, RayMasks.BLOCK_COLLISION))
				{
					return null;
				}

				if (!Physics.Raycast(aim.position, this.Value, out var hitInfo2, float.PositiveInfinity, 134217728) || hitInfo.transform != hitInfo2.transform)
				{
					return null;
				}

				if ((hitInfo2.transform.name == "Hinge" || hitInfo2.transform.name == "Left_Hinge" || hitInfo2.transform.name == "Right_Hinge") && hitInfo2.transform.parent.name == "Skeleton")
				{
					return new BarricadeClass(hitInfo2.transform.parent.parent);
				}

				return new BarricadeClass(hitInfo2.transform);
			}

			[ScriptFunction(null)]
			public PlayerClass GetPlayer()
			{
				if (!Physics.Raycast(this.Animal.transform.position, this.Value, out var hitInfo, float.PositiveInfinity, 512))
				{
					return null;
				}

				Player component = hitInfo.transform.GetComponent<Player>();
				if (!(component != null))
				{
					return null;
				}

				return new PlayerClass(component);
			}

			[ScriptFunction(null)]
			public Vector3Class GetPoint()
			{
				if (!Physics.Raycast(this.Animal.transform.position, this.Value, out var hitInfo, float.PositiveInfinity, RayMasks.BLOCK_COLLISION))
				{
					return null;
				}

				return new Vector3Class(hitInfo.point);
			}

			[ScriptFunction(null)]
			public StructureClass GetStructure()
			{
				Transform aim = this.Animal.transform;
				if (!Physics.Raycast(aim.position, this.Value, out var hitInfo, float.PositiveInfinity, RayMasks.BLOCK_COLLISION))
				{
					return null;
				}

				if (!Physics.Raycast(aim.position, this.Value, out var hitInfo2, float.PositiveInfinity, 268435456) || hitInfo.transform != hitInfo2.transform)
				{
					return null;
				}

				return new StructureClass(hitInfo2.transform);
			}

			[ScriptFunction(null)]
			public VehicleClass GetVehicle()
			{
				Transform aim = this.Animal.transform;
				if (!Physics.Raycast(aim.position, this.Value, out var hitInfo, float.PositiveInfinity, RayMasks.BLOCK_COLLISION))
				{
					return null;
				}

				if (!Physics.Raycast(aim.position, this.Value, out var hit, float.PositiveInfinity, 67108864) || hitInfo.transform != hit.transform)
				{
					return null;
				}

				InteractableVehicle interactableVehicle = VehicleManager.vehicles.FirstOrDefault((InteractableVehicle v) => v.transform == hit.transform);
				if (!(interactableVehicle != null))
				{
					return null;
				}

				return new VehicleClass(interactableVehicle);
			}
		}
	}
}
