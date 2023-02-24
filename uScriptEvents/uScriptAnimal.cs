using Newtonsoft.Json.Linq;
using SDG.Unturned;
using Steamworks; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine;
using uScript.API.Attributes;
using uScript.Core;
using uScript.Module.Main.Classes;
using uScript.Unturned;

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
			public void moveTo(Vector3Class position)
			{
				this.Animal.alertGoToPoint(position.Vector3, true);
			}

			[ScriptFunction(null)]
			public void runFrom(Vector3Class position)
			{
				this.Animal.alertRunAwayFromPoint(position.Vector3, true);
			}

			[ScriptFunction(null)]
			public void startle()
			{
				AnimalManager.sendAnimalStartle(this.Animal);
			}

			[ScriptFunction(null)]
			public void damage(ushort amount, bool dropLoot = true, string ragdoll = "NONE")
			{
				EPlayerKill playerKill;
				uint xp;
				if(ragdolls.TryGetValue(ragdoll, out ERagdollEffect ragdollEff))
				{
					Animal.askDamage(amount, this.Animal.transform.position, out playerKill, out xp, true, dropLoot, ragdollEff);
				}
				else
				{
					Rocket.Core.Logging.Logger.LogWarning("uScriptExtended module from uScript => Ragdoll must be 'NONE', 'BRONZE', 'GOLD', 'SILVER', or 'ZERO'.");
				}
			}

			[ScriptFunction(null)]
			public void kill(string ragdoll = "NONE")
			{
				if (ragdolls.TryGetValue(ragdoll, out ERagdollEffect ragdollEff))
				{
					AnimalManager.sendAnimalDead(this.Animal, this.Animal.transform.position, ragdollEff);
				}
				else
				{
					Rocket.Core.Logging.Logger.LogWarning("uScriptExtended module from uScript => Ragdoll must be 'NONE', 'BRONZE', 'GOLD', 'SILVER', or 'ZERO'.");
				}
			}

			[ScriptFunction(null)]
			public void dropLoot()
			{
				AnimalManager.dropLoot(this.Animal);
			}

			[ScriptProperty(null)]
			public bool isAttacking
			{
				get
				{
					return Convert.ToBoolean(ReflectionUtil.ReflectionUtil.getValue("isAttacking", this.Animal));
				}
				set
				{
					ReflectionUtil.ReflectionUtil.setValue("isAttacking", value, this.Animal);
				}
			}

			[ScriptProperty(null)]
			public bool isMoving
			{
				get
				{
					return Convert.ToBoolean(ReflectionUtil.ReflectionUtil.getValue("isMoving", this.Animal));
				}
				set
				{
					ReflectionUtil.ReflectionUtil.setValue("isMoving", value, this.Animal);
				}
			}

			[ScriptProperty(null)]
			public bool isRunning
			{
				get
				{
					return Convert.ToBoolean(ReflectionUtil.ReflectionUtil.getValue("isRunning", this.Animal));
				}
				set
				{
					ReflectionUtil.ReflectionUtil.setValue("isRunning", value, this.Animal);
				}
			}

			[ScriptProperty(null)]
			public bool isWandering
			{
				get
				{
					return Convert.ToBoolean(ReflectionUtil.ReflectionUtil.getValue("isWandering", this.Animal));
				}
				set
				{
					ReflectionUtil.ReflectionUtil.setValue("isWandering", value, this.Animal);
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
					ReflectionUtil.ReflectionUtil.setValue("_isFleeing", value, this.Animal);
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
					ReflectionUtil.ReflectionUtil.setValue("isHunting", value, this.Animal);
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
					ReflectionUtil.ReflectionUtil.setValue("health", value, this.Animal);
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

		}
	}
}
