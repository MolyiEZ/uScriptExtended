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
	public class ZombieFunction : ScriptModuleBase
	{
		[ScriptClass("zombie")]
		public class ZombieClass
		{
			public Zombie Zombie { get; set; }

			public ZombieClass(Zombie zombie)
			{
				this.Zombie = zombie;
			}

			[ScriptProperty(null)]
			public bool isBoss 
			{
				get
				{
					return this.Zombie.isBoss;
				}
			}

			[ScriptProperty(null)]
			public bool isHyper
			{
				get
				{
					return this.Zombie.isHyper;
				}
			}

			[ScriptProperty(null)]
			public bool isCutesy
			{
				get
				{
					return this.Zombie.isCutesy;
				}
			}

			[ScriptProperty(null)]
			public bool isHunting
			{
				get
				{
					return this.Zombie.isHunting;
				}
				set
				{
					this.Zombie.isHunting = value;
				}
			}

			[ScriptProperty(null)]
			public bool isMega
			{
				get
				{
					return this.Zombie.isMega;
				}
			}

			[ScriptProperty(null)]
			public bool isRadioactive
			{
				get
				{
					return this.Zombie.isRadioactive;
				}
			}

			[ScriptProperty(null)]
			public float maxHealth
			{
				get
				{
					return this.Zombie.GetMaxHealth();
				}
			}

			[ScriptProperty(null)]
			public float health
			{
				get
				{
					return this.Zombie.GetHealth();
				}
			}

			[ScriptFunction(null)]
			public void dropLoot()
			{
				ZombieManager.dropLoot(this.Zombie);
			}

			[ScriptFunction(null)]
			public void acid(Vector3Class origin, Vector3Class direction)
			{
				ZombieManager.sendZombieAcid(this.Zombie, origin.Vector3, direction.Vector3);
			}

			[ScriptFunction(null)]
			public void breath()
			{
				ZombieManager.sendZombieBreath(this.Zombie);
			}

			[ScriptFunction(null)]
			public void charge()
			{
				ZombieManager.sendZombieCharge(this.Zombie);
			}

			[ScriptFunction(null)]
			public void spark(Vector3Class target)
			{
				ZombieManager.sendZombieSpark(this.Zombie, target.Vector3);
			}

			[ScriptFunction(null)]
			public void spit()
			{
				ZombieManager.sendZombieSpit(this.Zombie);
			}

			[ScriptFunction(null)]
			public void stomp()
			{
				ZombieManager.sendZombieStomp(this.Zombie);
			}

			[ScriptFunction(null)]
			public void Throw()
			{
				ZombieManager.sendZombieThrow(this.Zombie);
			}

			[ScriptProperty(null)]
			public Vector3Class position
			{
				get
				{
					return new Vector3Class(this.Zombie.transform.position);
				}
			}
		}
	}
}
