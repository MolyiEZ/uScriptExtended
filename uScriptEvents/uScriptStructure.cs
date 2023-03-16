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

namespace uScriptStructure
{
	[ScriptTypeExtension(typeof(StructureClass))]
	public class FunctionStructure
	{
		[ScriptFunction("get_isWired")]
		public static bool getIsWired([ScriptInstance] ExpressionValue instance)
		{
			if (!(instance.Data is StructureClass structure))
				return false;

			Vector3 structurePosition = structure.StructureTransform.position;
			float maxPowerRange = PowerTool.MAX_POWER_RANGE;

			if (Level.info != null && Level.info.configData != null && Level.info.configData.Has_Global_Electricity)
			{
				return true;
			}
			else
			{
				List<InteractableGenerator> generators = PowerTool.checkGenerators(structurePosition, maxPowerRange, ushort.MaxValue);

				foreach (InteractableGenerator generator in generators)
				{
					if (generator.isPowered && generator.fuel > 0 && (generator.transform.position - structurePosition).sqrMagnitude < generator.sqrWirerange)
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
			if (!(instance.Data is StructureClass structure)) return 0;
			StructureDrop structureDrop = StructureManager.FindStructureByRootTransform(structure.StructureTransform);
			return MeasurementTool.byteToAngle(structureDrop.GetServersideData().angle_x);
		}

		[ScriptFunction("get_angley")]
		public static float angley([ScriptInstance] ExpressionValue instance)
		{
			if (!(instance.Data is StructureClass structure)) return 0;
			StructureDrop structureDrop = StructureManager.FindStructureByRootTransform(structure.StructureTransform);
			return MeasurementTool.byteToAngle(structureDrop.GetServersideData().angle_y);
		}

		[ScriptFunction("get_anglez")]
		public static float anglez([ScriptInstance] ExpressionValue instance)
		{
			if (!(instance.Data is StructureClass structure)) return 0;
			StructureDrop structureDrop = StructureManager.FindStructureByRootTransform(structure.StructureTransform);
			return MeasurementTool.byteToAngle(structureDrop.GetServersideData().angle_z);
		}
	}
}
