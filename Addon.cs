using System;
using System.Collections.Generic;
using GameServer;
using GameServer.addon;
using GameServer.events;
using System.Threading;

namespace AntiFlood
{
	public class Addon : IModule, IEventListener
	{
		TimeSpan time;
		
		public void OnLoaded()
		{
			time = DateTime.Now.TimeOfDay;
			Events.AddListener(this);
		}
		
		public void OnDisabled()
		{
			Events.RemoveListener(this);
		}
		
		public int Version = 1;
		public string GetMetadata()
		{
			return "AntiFlood version " + Version;
		}
		
		public string GetDescription()
		{
			return "AntiFlood";	
		}
		
		public void Handler(Event HandledEvent)
		{
			if(HandledEvent.GetCode() == Events.Code_PlayerActionEvent)
			{
				PlayerActionEvent pae = (PlayerActionEvent) HandledEvent;
				
				if(pae.Action == PlayerActionEvent.Actions.Chat)
				{
					time = new TimeSpan(time.Hours, time.Minutes, time.Seconds + 3);
					if (time > DateTime.Now.TimeOfDay)
            		{
						pae.Player.SendChatMessage("[Сервер]: В чат можно писать раз в 3 секунды!");
 						pae.Cancelled = true;
            		}
					time = DateTime.Now.TimeOfDay;
				}
			}
		}
	}
}