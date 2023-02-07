using Rocket.Unturned.Player;
using SDG.NetTransport;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using uScript.API.Attributes;
using uScript.Core;
using uScript.Module.Main.Classes;

namespace uScriptVehicle
{
	[ScriptTypeExtension(typeof(VehicleClass))]
	public class FunctionsVehicle
	{
		[ScriptFunction("enter")]
		public static void enterVehicle([ScriptInstance] ExpressionValue instance, PlayerClass player)
		{
			if (!(instance.Data is VehicleClass vehicle)) return;
			VehicleManager.ServerForcePassengerIntoVehicle(player.Player, vehicle.Vehicle);
		}

		[ScriptFunction("setBarricade")]
		public static void SetBarricade([ScriptInstance] ExpressionValue instance, ushort id, Vector3Class position, float angle_x = 0f, float angle_y = 0f, float angle_z = 0f, string owner = "0", string group = "0")
		{
			if (!(instance.Data is VehicleClass vehicle)) return;

			Barricade barricade = new Barricade(id);

			Vector3 pos = position.Vector3;

			BarricadeManager.dropPlantedBarricade(vehicle.Vehicle.transform, barricade, pos, Quaternion.Euler(angle_x, angle_y, angle_z), Convert.ToUInt64(owner), Convert.ToUInt64(group));
		}
	}
}
