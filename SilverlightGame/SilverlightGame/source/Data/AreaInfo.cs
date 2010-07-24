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
using System.Collections.Generic;

namespace SilverlightGame.Data
{
    public class AreaInfo
    {
    	public int AreaID;
        public Polygon Shape;
    	public Dictionary<string, uint> Colors;
    	public Dictionary<string, string> Players;

        public uint FirstColor{
            get {
                var values = Colors.Values;
                var enume =  values.GetEnumerator();
                enume.MoveNext();
                var current = enume.Current;
                return current;
            }
        }


		public AreaInfo(int areaID)
		{
            this.AreaID = areaID;
			this.Colors = new Dictionary<string, uint>();
			this.Players = new Dictionary<string, string>();
		}

        public bool SetData(JsonValue data)
        {
            var isChanged = false;
        	var players = data["players"];
        	var colors = data["colors"];
        	for (int i = 0; i < players.Count; i++)
        	{
                var playerName = players[i];
                var color = colors[i];
                var isDataChanged = SetData(playerName, color);
                isChanged |= isDataChanged;
            }

            return isChanged;
        }

        public bool SetData(string playerName, uint color)
        {
            var isChange = (isDataEqual(playerName, color) == false);

            if (isChange == false) return false;

            this.Players[playerName] = playerName;
			this.Colors[playerName] = color;

            return true;
        }

        private bool isDataEqual(string playerName, uint color)
        {
            if (this.Players.ContainsKey(playerName) == false) return false;
            
            var isPlayerEqual = (this.Players[playerName] == playerName);
            var isColorEqual = (this.Colors[playerName] == color);

            return (isPlayerEqual && isColorEqual);
        }
	}
}

