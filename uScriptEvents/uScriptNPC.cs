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
	public class NPCFunction : ScriptModuleBase
	{
		[ScriptClass("npc")]
		public class NPCCLass
		{
			public ObjectNPCAsset NPC { get; set; }

			public NPCCLass(ObjectNPCAsset npc)
			{
				this.NPC = npc;
			}

			[ScriptProperty(null)]
			public string name
			{
				get
				{
					return this.NPC.name;
				}
			}

		}
	}
}
