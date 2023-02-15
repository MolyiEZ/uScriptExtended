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
			public void kill(string ragdoll)
			{
				switch(ragdoll)
				{
					case "NONE":
						AnimalManager.sendAnimalDead(this.Animal, this.Animal.transform.position, ERagdollEffect.NONE);
					break;

					case "BRONZE":
						AnimalManager.sendAnimalDead(this.Animal, this.Animal.transform.position, ERagdollEffect.BRONZE);
					break;

					case "SILVER":
						AnimalManager.sendAnimalDead(this.Animal, this.Animal.transform.position, ERagdollEffect.SILVER);
					break;

					case "GOLD":
						AnimalManager.sendAnimalDead(this.Animal, this.Animal.transform.position, ERagdollEffect.GOLD);
					break;

					case "ZERO":
						AnimalManager.sendAnimalDead(this.Animal, this.Animal.transform.position, ERagdollEffect.ZERO_KELVIN);
					break;

					default:
						Console.WriteLine("uScriptExtended module from uScript => Ragdoll must be 'NONE', 'BRONZE', 'GOLD', 'SILVER', or 'ZERO'.", Console.ForegroundColor = ConsoleColor.Red);
						Console.ResetColor();
						break;
				}
			}

			[ScriptFunction(null)]
			public void dropLoot()
			{
				AnimalManager.dropLoot(this.Animal);
			}

			[ScriptProperty(null)]
			public bool isFleeing
			{
				get
				{
					return this.Animal.isFleeing;
				}
			}

			[ScriptProperty(null)]
			public bool isHunting
			{
				get
				{
					return this.Animal.isHunting;
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
			public float health
			{
				get
				{
					return this.Animal.GetHealth();
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
			}

		}
	}
}
