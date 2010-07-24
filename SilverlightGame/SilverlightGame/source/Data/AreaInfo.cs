using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Json;
using SilverlightGame.Utility;

namespace SilverlightGame.Data
{
    public class AreaInfo
    {
    	public int AreaID;
    	public Dictionary<string, uint> Colors;
    	public Dictionary<string, string> Players;

		public void AreaInfo(int areaID)
		{
			this.Are
			this.Colors = new Dictionary<string, uint>();
			this.Players = new Dictionary<string, string>();
		}

        public void SetData(JsonValue data)
        {
        	var players = = data["players"];
        	var colors = data["colors"];
        	for (int i = 0; i < players.Count; i++)
        	{
				var keyName = players[i];
				this.Players[keyName] = keyName;
				this.Colors[keyName] = colors[i];
        	}
        }
    }
}
