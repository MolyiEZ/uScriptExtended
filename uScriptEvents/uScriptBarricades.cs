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

namespace uScriptBarricades
{
	public class Generator : ScriptModuleBase
	{
		[ScriptClass("generator")]
		public class GeneratorClass
		{
			public InteractableGenerator Generator { get; }

			public GeneratorClass(InteractableGenerator generator)
			{
				this.Generator = generator;
			}

			[ScriptProperty(null)]
			public BarricadeClass barricade
			{
				get
				{
					return new BarricadeClass(this.Generator.transform);
				}
			}

			[ScriptProperty(null)]
			public bool powered
			{
				get
				{
					return this.Generator.isPowered;
				}
				set 
				{
					BarricadeManager.ServerSetGeneratorPowered(this.Generator, value);
				}
			}

			[ScriptProperty(null)]
			public float wirerange
			{
				get 
				{
					return this.Generator.wirerange;
				}
			}

			[ScriptProperty(null)]
			public float fuel
			{
				get
				{
					return this.Generator.fuel;
				}
			}
		}

		[ScriptClass("fire")]
		public class FireClass 
		{
			public InteractableFire Fire { get; }

			public FireClass(InteractableFire fire)
			{
				this.Fire = fire;
			}

			[ScriptProperty(null)]
			public BarricadeClass barricade
			{
				get
				{
					return new BarricadeClass(this.Fire.transform);
				}
			}

			[ScriptProperty(null)]
			public bool lit
			{
				get 
				{
					return this.Fire.isLit;
				}
				set 
				{
					BarricadeManager.ServerSetFireLit(this.Fire, value);
				}
			}
		}

		[ScriptClass("oven")]
		public class OvenClass
		{
			public InteractableOven Oven { get; }

			public OvenClass(InteractableOven oven)
			{
				this.Oven = oven;
			}

			[ScriptProperty(null)]
			public BarricadeClass barricade
			{
				get
				{
					return new BarricadeClass(this.Oven.transform);
				}
			}

			[ScriptProperty(null)]
			public bool lit
			{
				get
				{
					return this.Oven.isLit;
				}
				set
				{
					BarricadeManager.ServerSetOvenLit(this.Oven, value);
				}
			}
		}
	}

	[ScriptTypeExtension(typeof(BarricadeClass))]
	public class FunctionBarricade
	{
		[ScriptFunction("get_generator")]
		public static GeneratorClass getGenerator([ScriptInstance] ExpressionValue instance)
		{
			if (!(instance.Data is BarricadeClass barricade)) return null;
			InteractableGenerator component = barricade.BarricadeTransform.GetComponent<InteractableGenerator>();
			if (!(component != null))
			{
				return null;
			}
			return new GeneratorClass(component);
		}

		[ScriptFunction("get_fire")]
		public static FireClass getFire([ScriptInstance] ExpressionValue instance)
		{
			if (!(instance.Data is BarricadeClass barricade)) return null;
			InteractableFire component = barricade.BarricadeTransform.GetComponent<InteractableFire>();
			if (!(component != null))
			{
				return null;
			}
			return new FireClass(component);
		}

		[ScriptFunction("get_oven")]
		public static OvenClass getOven([ScriptInstance] ExpressionValue instance)
		{
			if (!(instance.Data is BarricadeClass barricade)) return null;
			InteractableOven component = barricade.BarricadeTransform.GetComponent<InteractableOven>();
			if (!(component != null))
			{
				return null;
			}
			return new OvenClass(component);
		}

		[ScriptFunction("get_isWired")]
		public static bool getIsWired([ScriptInstance] ExpressionValue instance)
		{
			if (!(instance.Data is BarricadeClass barricade)) return false;
			InteractablePower component = barricade.BarricadeTransform.GetComponent<InteractablePower>();
			if (component == null) return false;
			return component.isWired;
		}

		[ScriptFunction("get_anglex")]
		public static float anglex([ScriptInstance] ExpressionValue instance)
		{
			if (!(instance.Data is BarricadeClass barricade)) return 0;
			BarricadeDrop barricadeDrop = BarricadeManager.FindBarricadeByRootTransform(barricade.BarricadeTransform);
			return MeasurementTool.byteToAngle(barricadeDrop.GetServersideData().angle_x);
		}

		[ScriptFunction("get_angley")]
		public static float angley([ScriptInstance] ExpressionValue instance)
		{
			if (!(instance.Data is BarricadeClass barricade)) return 0;
			BarricadeDrop barricadeDrop = BarricadeManager.FindBarricadeByRootTransform(barricade.BarricadeTransform);
			return MeasurementTool.byteToAngle(barricadeDrop.GetServersideData().angle_y);
		}

		[ScriptFunction("get_anglez")]
		public static float anglez([ScriptInstance] ExpressionValue instance)
		{
			if (!(instance.Data is BarricadeClass barricade)) return 0;
			BarricadeDrop barricadeDrop = BarricadeManager.FindBarricadeByRootTransform(barricade.BarricadeTransform);
			return MeasurementTool.byteToAngle(barricadeDrop.GetServersideData().angle_z);
		}
	}
}
