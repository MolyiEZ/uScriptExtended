using Rocket.Unturned.Effects;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using uScript.API.Attributes;
using uScript.Core;
using uScript.Module.Main.Classes;
using uScript.Module.Main.Modules;
using uScript.Unturned;
using static uScriptEvents.AnimalFunction;
using static uScriptEvents.ZombieFunction;
using static uScriptPlayers.Players;

namespace uScriptPlayers
{
	[ScriptModule("serverExtended")]
	public class Server : ScriptModuleBase
	{
		[ScriptFunction("getPlayersInRadius")]
		public static ExpressionValue getPlayersInRadius(Vector3Class center, double radius)
		{
			List<Player> list = new List<Player>();
			PlayerTool.getPlayersInRadius(center.Vector3, (float)radius, list);
			return ExpressionValue.Array(Enumerable.Select<Player, ExpressionValue>(list, (Player t) => ExpressionValue.CreateObject(new PlayerClass(t))));
		}

		[ScriptFunction("getAnimalsInRadius")]
		public static ExpressionValue getAnimalsInRadius(Vector3Class center, double radius)
		{
			List<Animal> list = new List<Animal>();
			AnimalManager.getAnimalsInRadius(center.Vector3, (float)radius, list);
			return ExpressionValue.Array(Enumerable.Select<Animal, ExpressionValue>(list, (Animal t) => ExpressionValue.CreateObject(new AnimalClass(t))));
		}

		[ScriptFunction("getAnimal")]
		public static AnimalClass getAnimal(ushort index)
		{
			return new AnimalClass(AnimalManager.getAnimal(index));
		}

		[ScriptFunction("clearAllAnimals")]
		public static void clearAnimals() => AnimalManager.askClearAllAnimals();

		[ScriptFunction("getZombiesInRadius")]
		public static ExpressionValue getZombiesInRadius(Vector3Class center, double radius)
		{
			List<Zombie> list = new List<Zombie>();
			ZombieManager.getZombiesInRadius(center.Vector3, (float)radius, list);
			return ExpressionValue.Array(Enumerable.Select<Zombie, ExpressionValue>(list, (Zombie t) => ExpressionValue.CreateObject(new ZombieClass(t))));
		}
	}
}
