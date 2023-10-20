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
using uScript.Unturned;
using static uScriptBarricades.Generator;
using static uScriptEvents.AnimalFunction;
using static uScriptVehicle.VehicleLook;

namespace uScriptVehicle
{
	public class VehicleLook : ScriptModuleBase
	{
		[ScriptClass("vehicleLook")]
		public class VehicleLookClass
		{
			public InteractableVehicle Vehicle { get; }
			public Vector3 Value { get; }

			public VehicleLookClass(InteractableVehicle vehicle, Vector3 value)
			{
				Vehicle = vehicle;
				Value = value;
			}

			[ScriptFunction(null)]
			public BarricadeClass GetBarricade()
			{
				Transform aim = this.Vehicle.transform;
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
				if (!Physics.Raycast(this.Vehicle.transform.position, this.Value, out var hitInfo, float.PositiveInfinity, 512))
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
				if (!Physics.Raycast(this.Vehicle.transform.position, this.Value, out var hitInfo, float.PositiveInfinity, RayMasks.BLOCK_COLLISION))
				{
					return null;
				}

				return new Vector3Class(hitInfo.point);
			}

			[ScriptFunction(null)]
			public StructureClass GetStructure()
			{
				Transform aim = this.Vehicle.transform;
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
				Transform aim = this.Vehicle.transform;
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


	[ScriptTypeExtension(typeof(VehicleClass))]
	public class FunctionsVehicle
	{
		[ScriptFunction("look")]
		public static VehicleLookClass getLook([ScriptInstance] ExpressionValue instance, Vector3Class value)
		{
			if (instance.Data is not VehicleClass vehicle) return null;
			return new VehicleLookClass(vehicle.Vehicle, value.Vector3);
		}

		[ScriptFunction("enter")]
		public static void enterVehicle([ScriptInstance] ExpressionValue instance, PlayerClass player)
		{
			if (instance.Data is not VehicleClass vehicle) return;
			VehicleManager.ServerForcePassengerIntoVehicle(player.Player, vehicle.Vehicle);
		}

		[ScriptFunction("get_isDrowned")]
		public static bool getIsDrowned([ScriptInstance] ExpressionValue instance)
		{
			if (instance.Data is not VehicleClass vehicle) return false;
			return vehicle.Vehicle.isDrowned;
		}

		[ScriptFunction("get_isBatteryFull")]
		public static bool getisBatteryFull([ScriptInstance] ExpressionValue instance)
		{
			if (instance.Data is not VehicleClass vehicle) return false;
			return vehicle.Vehicle.isBatteryFull;
		}

		[ScriptFunction("get_isBatteryReplaceable")]
		public static bool getisBatteryReplaceable([ScriptInstance] ExpressionValue instance)
		{
			if (instance.Data is not VehicleClass vehicle) return false;
			return vehicle.Vehicle.isBatteryReplaceable;
		}

		[ScriptFunction("get_isEmpty")]
		public static bool getisEmpty([ScriptInstance] ExpressionValue instance)
		{
			if (instance.Data is not VehicleClass vehicle) return false;
			return vehicle.Vehicle.isEmpty;
		}

		[ScriptFunction("get_isEngineOn")]
		public static bool getisEngineOn([ScriptInstance] ExpressionValue instance)
		{
			if (instance.Data is not VehicleClass vehicle) return false;
			return vehicle.Vehicle.isEngineOn;
		}

		[ScriptFunction("get_isEnginePowered")]
		public static bool getisEnginePowered([ScriptInstance] ExpressionValue instance)
		{
			if (instance.Data is not VehicleClass vehicle) return false;
			return vehicle.Vehicle.isEnginePowered;
		}

		[ScriptFunction("get_isExitable")]
		public static bool getisExitable([ScriptInstance] ExpressionValue instance)
		{
			if (instance.Data is not VehicleClass vehicle) return false;
			return vehicle.Vehicle.isExitable;
		}

		[ScriptFunction("get_isInsideNoDamageZone")]
		public static bool getisInsideNoDamageZone([ScriptInstance] ExpressionValue instance)
		{
			if (instance.Data is not VehicleClass vehicle) return false;
			return vehicle.Vehicle.isInsideNoDamageZone;
		}

		[ScriptFunction("get_isInsideSafezone")]
		public static bool getisInsideSafezone([ScriptInstance] ExpressionValue instance)
		{
			if (instance.Data is not VehicleClass vehicle) return false;
			return vehicle.Vehicle.isInsideSafezone;
		}

		[ScriptFunction("get_isRefillable")]
		public static bool getisRefillable([ScriptInstance] ExpressionValue instance)
		{
			if (instance.Data is not VehicleClass vehicle) return false;
			return vehicle.Vehicle.isRefillable;
		}

		[ScriptFunction("get_isRepaired")]
		public static bool getisRepaired([ScriptInstance] ExpressionValue instance)
		{
			if (instance.Data is not VehicleClass vehicle) return false;
			return vehicle.Vehicle.isRepaired;
		}

		[ScriptFunction("get_isSiphonable")]
		public static bool getisSiphonable([ScriptInstance] ExpressionValue instance)
		{
			if (instance.Data is not VehicleClass vehicle) return false;
			return vehicle.Vehicle.isSiphonable;
		}

		[ScriptFunction("get_isSkinned")]
		public static bool getisSkinned([ScriptInstance] ExpressionValue instance)
		{
			if (instance.Data is not VehicleClass vehicle) return false;
			return vehicle.Vehicle.isSkinned;
		}

		[ScriptFunction("get_isTireReplaceable")]
		public static bool getisTireReplaceable([ScriptInstance] ExpressionValue instance)
		{
			if (instance.Data is not VehicleClass vehicle) return false;
			return vehicle.Vehicle.isTireReplaceable;
		}

		[ScriptFunction("get_isUnderwater")]
		public static bool getisUnderwater([ScriptInstance] ExpressionValue instance)
		{
			if (instance.Data is not VehicleClass vehicle) return false;
			return vehicle.Vehicle.isUnderwater;
		}

		[ScriptFunction("get_forward")]
		public static Vector3Class getForward([ScriptInstance] ExpressionValue instance)
		{
			if (instance.Data is not VehicleClass vehicle) return null;
			return new Vector3Class(vehicle.Vehicle.transform.forward);
		}

		[ScriptFunction("get_backward")]
		public static Vector3Class getBackward([ScriptInstance] ExpressionValue instance)
		{
			if (instance.Data is not VehicleClass vehicle) return null;
			return new Vector3Class(-vehicle.Vehicle.transform.forward);
		}

		[ScriptFunction("get_right")]
		public static Vector3Class getRight([ScriptInstance] ExpressionValue instance)
		{
			if (instance.Data is not VehicleClass vehicle) return null;
			return new Vector3Class(vehicle.Vehicle.transform.right);
		}

		[ScriptFunction("get_left")]
		public static Vector3Class getLeft([ScriptInstance] ExpressionValue instance)
		{
			if (instance.Data is not VehicleClass vehicle) return null;
			return new Vector3Class(-vehicle.Vehicle.transform.right);
		}

		[ScriptFunction("get_up")]
		public static Vector3Class getUp([ScriptInstance] ExpressionValue instance)
		{
			if (instance.Data is not VehicleClass vehicle) return null;
			return new Vector3Class(vehicle.Vehicle.transform.up);
		}

		[ScriptFunction("get_down")]
		public static Vector3Class getDown([ScriptInstance] ExpressionValue instance)
		{
			if (instance.Data is not VehicleClass vehicle) return null;
			return new Vector3Class(-vehicle.Vehicle.transform.up);
		}
	}
}
