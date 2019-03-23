using System;
using System.Collections.Generic;
using System.IO;

using GameServer;
using GameServer.addon;
using GameServer.events;

namespace AntiMat
{
	public class Addon : IModule, IEventListener
	{
		public string SOURCEFILE;
		public bool Flag;
		
		List<string> MatsList = new List<string>();
		
		public void OnLoaded()
		{
			SOURCEFILE = Addons.GetDirectory() + "mats.txt";
			
			if(!File.Exists(SOURCEFILE)) File.WriteAllText(SOURCEFILE, "");
			else MatsList.AddRange(File.ReadAllLines(SOURCEFILE));
			
			Events.AddListener(this);
		}
		
		public void OnDisabled()
		{
			Events.RemoveListener(this);
		}
		
		public int Version = 1;
		public string GetMetadata()
		{
			return "AntiMat version " + Version;
		}
		
		public string GetDescription()
		{
			return "AntiMat";	
		}
		
		public void Handler(Event HandledEvent)
		{
			if(HandledEvent.GetCode() == Events.Code_PlayerActionEvent)
			{
				PlayerActionEvent pae = (PlayerActionEvent) HandledEvent;
				
				if(pae.Action == PlayerActionEvent.Actions.Chat)
				{
					string[] message = ((string) pae.Data[0]).Split(' ', '.', ',', ':', ';');
					
					string return_message = string.Empty;
					
					foreach(var mat in MatsList)
					{
						foreach(var mess in message)
						{
							if(mat.ToLower() == mess.ToLower())
							{
								return_message += mess.Replace(mess.Substring(1, mess.Length - 1), new string('*', mess.Length - 1));
								Flag = true;
								pae.Cancelled = true;
							}
						}
					}
					if(Flag)
						Server.BroadcastMessage(pae.Player.Name + ": " + return_message);
				}
			}
		}
	}
}