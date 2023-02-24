using Newtonsoft.Json.Linq;
using SDG.Unturned;
using Steamworks; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine;
using uScript.API.Attributes;
using uScript.Core;
using uScript.Module.Main.Classes;
using uScript.Module.Main.Modules;
using uScript.Unturned;

namespace uScriptEvents
{
	public class ZombieFunction : ScriptModuleBase
	{
		public static Dictionary<string, ERagdollEffect> ragdolls = new Dictionary<string, ERagdollEffect>()
		{
			{"NONE", ERagdollEffect.NONE},
			{"BRONZE", ERagdollEffect.BRONZE},
			{"SILVER", ERagdollEffect.SILVER},
			{"GOLD", ERagdollEffect.GOLD},
			{"ZERO", ERagdollEffect.ZERO_KELVIN},
		};

		public static Dictionary<string, EZombieSpeciality> specialityOptions = new Dictionary<string, EZombieSpeciality>
		{
			{ "NONE", EZombieSpeciality.NONE },
			{ "NORMAL", EZombieSpeciality.NORMAL },
			{ "MEGA", EZombieSpeciality.MEGA },
			{ "CRAWLER", EZombieSpeciality.CRAWLER },
			{ "SPRINTER", EZombieSpeciality.SPRINTER },
			{ "FLANKER_FRIENDLY", EZombieSpeciality.FLANKER_FRIENDLY },
			{ "FLANKER_STALK", EZombieSpeciality.FLANKER_STALK },
			{ "BURNER", EZombieSpeciality.BURNER },
			{ "ACID", EZombieSpeciality.ACID },
			{ "BOSS_ELECTRIC", EZombieSpeciality.BOSS_ELECTRIC },
			{ "BOSS_WIND", EZombieSpeciality.BOSS_WIND },
			{ "BOSS_FIRE", EZombieSpeciality.BOSS_FIRE },
			{ "BOSS_ALL", EZombieSpeciality.BOSS_ALL },
			{ "BOSS_MAGMA", EZombieSpeciality.BOSS_MAGMA },
			{ "SPIRIT", EZombieSpeciality.SPIRIT },
			{ "BOSS_SPIRIT", EZombieSpeciality.BOSS_SPIRIT },
			{ "BOSS_NUCLEAR", EZombieSpeciality.BOSS_NUCLEAR },
			{ "DL_RED_VOLATILE", EZombieSpeciality.DL_RED_VOLATILE },
			{ "DL_BLUE_VOLATILE", EZombieSpeciality.DL_BLUE_VOLATILE },
			{ "BOSS_ELVER_STOMPER", EZombieSpeciality.BOSS_ELVER_STOMPER },
			{ "BOSS_KUWAIT", EZombieSpeciality.BOSS_KUWAIT },
		};

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
			public void spawnZombie(Vector3Class position, string speciality = "NONE", float angle = 0f)
			{
				if (specialityOptions.TryGetValue(speciality, out EZombieSpeciality zombieType))
				{
					ZombieManager.sendZombieAlive(this.Zombie, 1, (byte)zombieType, this.Zombie.shirt, this.Zombie.pants, this.Zombie.hat, this.Zombie.gear, position.Vector3, (byte)angle);
				}
				else
				{
					Rocket.Core.Logging.Logger.LogWarning("uScriptExtended module from uScript => Speciality must be one of the valid options (See GitHub Explanation in documentation).");
				}
			}


			[ScriptFunction(null)]
			public string speciality
			{
				get
				{
					return this.Zombie.speciality.ToString();
				}
				set
				{
					if (specialityOptions.TryGetValue(value, out EZombieSpeciality speciality))
					{
						ZombieManager.sendZombieSpeciality(this.Zombie, speciality);
					}
					else
					{
						Rocket.Core.Logging.Logger.LogWarning("uScriptExtended module from uScript => Speciality must be one of the valid options (See GitHub Explanation in documentation).");
					}
				}
			}

			[ScriptFunction(null)]
			public void Kill(string ragdoll = "NONE")
			{
				if (ragdolls.TryGetValue(ragdoll, out ERagdollEffect ragdollEff))
				{
					ZombieManager.sendZombieDead(this.Zombie, this.Zombie.transform.position, ragdollEff);
				}
				else
				{
					Rocket.Core.Logging.Logger.LogWarning("uScriptExtended module from uScript => Ragdoll must be 'NONE', 'BRONZE', 'GOLD', 'SILVER', or 'ZERO'.");
				}
			}

			[ScriptFunction(null)]
			public void damage(ushort amount, bool dropLoot = true, string ragdoll = "NONE")
			{
				EPlayerKill playerKill;
				uint xp;
				if (ragdolls.TryGetValue(ragdoll, out ERagdollEffect ragdollEff))
				{
					this.Zombie.askDamage(amount, this.Zombie.transform.position, out playerKill, out xp, true, dropLoot, EZombieStunOverride.None, ragdollEff);
				}
				else
				{
					Rocket.Core.Logging.Logger.LogWarning("uScriptExtended module from uScript => Ragdoll must be 'NONE', 'BRONZE', 'GOLD', 'SILVER', or 'ZERO'.");
				}
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
				set
				{
					this.Zombie.transform.position = value.Vector3;
				}
			}
		}
	}
}
