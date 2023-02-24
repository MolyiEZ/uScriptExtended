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

		[ScriptFunction("get_isDrowned")]
		public static bool getIsDrowned([ScriptInstance] ExpressionValue instance)
		{
			if (!(instance.Data is VehicleClass vehicle)) return false;
			return vehicle.Vehicle.isDrowned;
		}

		[ScriptFunction("get_isBatteryFull")]
		public static bool getisBatteryFull([ScriptInstance] ExpressionValue instance)
		{
			if (!(instance.Data is VehicleClass vehicle)) return false;
			return vehicle.Vehicle.isBatteryFull;
		}

		[ScriptFunction("get_isBatteryReplaceable")]
		public static bool getisBatteryReplaceable([ScriptInstance] ExpressionValue instance)
		{
			if (!(instance.Data is VehicleClass vehicle)) return false;
			return vehicle.Vehicle.isBatteryReplaceable;
		}

		[ScriptFunction("get_isEmpty")]
		public static bool getisEmpty([ScriptInstance] ExpressionValue instance)
		{
			if (!(instance.Data is VehicleClass vehicle)) return false;
			return vehicle.Vehicle.isEmpty;
		}

		[ScriptFunction("get_isEngineOn")]
		public static bool getisEngineOn([ScriptInstance] ExpressionValue instance)
		{
			if (!(instance.Data is VehicleClass vehicle)) return false;
			return vehicle.Vehicle.isEngineOn;
		}

		[ScriptFunction("get_isEnginePowered")]
		public static bool getisEnginePowered([ScriptInstance] ExpressionValue instance)
		{
			if (!(instance.Data is VehicleClass vehicle)) return false;
			return vehicle.Vehicle.isEnginePowered;
		}

		[ScriptFunction("get_isExitable")]
		public static bool getisExitable([ScriptInstance] ExpressionValue instance)
		{
			if (!(instance.Data is VehicleClass vehicle)) return false;
			return vehicle.Vehicle.isExitable;
		}

		[ScriptFunction("get_isInsideNoDamageZone")]
		public static bool getisInsideNoDamageZone([ScriptInstance] ExpressionValue instance)
		{
			if (!(instance.Data is VehicleClass vehicle)) return false;
			return vehicle.Vehicle.isInsideNoDamageZone;
		}

		[ScriptFunction("get_isInsideSafezone")]
		public static bool getisInsideSafezone([ScriptInstance] ExpressionValue instance)
		{
			if (!(instance.Data is VehicleClass vehicle)) return false;
			return vehicle.Vehicle.isInsideSafezone;
		}

		[ScriptFunction("get_isRefillable")]
		public static bool getisRefillable([ScriptInstance] ExpressionValue instance)
		{
			if (!(instance.Data is VehicleClass vehicle)) return false;
			return vehicle.Vehicle.isRefillable;
		}

		[ScriptFunction("get_isRepaired")]
		public static bool getisRepaired([ScriptInstance] ExpressionValue instance)
		{
			if (!(instance.Data is VehicleClass vehicle)) return false;
			return vehicle.Vehicle.isRepaired;
		}

		[ScriptFunction("get_isSiphonable")]
		public static bool getisSiphonable([ScriptInstance] ExpressionValue instance)
		{
			if (!(instance.Data is VehicleClass vehicle)) return false;
			return vehicle.Vehicle.isSiphonable;
		}

		[ScriptFunction("get_isSkinned")]
		public static bool getisSkinned([ScriptInstance] ExpressionValue instance)
		{
			if (!(instance.Data is VehicleClass vehicle)) return false;
			return vehicle.Vehicle.isSkinned;
		}

		[ScriptFunction("get_isTireReplaceable")]
		public static bool getisTireReplaceable([ScriptInstance] ExpressionValue instance)
		{
			if (!(instance.Data is VehicleClass vehicle)) return false;
			return vehicle.Vehicle.isTireReplaceable;
		}

		[ScriptFunction("get_isUnderwater")]
		public static bool getisUnderwater([ScriptInstance] ExpressionValue instance)
		{
			if (!(instance.Data is VehicleClass vehicle)) return false;
			return vehicle.Vehicle.isUnderwater;
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
