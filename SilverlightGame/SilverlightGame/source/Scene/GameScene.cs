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
using SilverlightGame.Utility;
using SilverlightGame.Graphics;
using System.Runtime.Serialization.Json;
using System.Json;
using SilverlightGame.Object;
using System.Reflection.Emit;
using TestSilver.Network;
using SilverlightGame.Network;
using SilverlightGame.Data;

namespace SilverlightGame.Scene
{
    public class GameScene
    {
        private GameXXX game;
        private Map map;
        private MapDrawer mapDrawer;
        private MapClickController mapClickController;
        private NetworkManager network;
        private NetworkController networkController;

        private JsonObject reciveData;

        public GameScene(GameXXX game)
        {
            this.game = game;
            this.mapClickController = new MapClickController();
        }

        public void Initialize()
        {
            this.map = this.game.Map;
            this.mapDrawer = this.game.MapDrawer;
            this.network = this.game.Network;
            this.networkController = this.game.NetworkController;

            mapClickController.Initialize(game.Input, map);

            network.reciveData += ReciveData;
            networkController.StartPolling();

            game.update += Update;
            mapDrawer.Visible = true;
        }

        public void Destroy()
        {
            mapDrawer.Visible = false;
            game.update -= Update;
            
            networkController.StopPolling();
            network.reciveData -= ReciveData;

            mapClickController.Destroy();
        }

        public void ReciveData(JsonObject reciveData)
        {
            this.reciveData = reciveData;
            if (reciveData["command"] == "areaData") {
                UpdateMap(reciveData["areaDatas"]);
            }
        }

        private void UpdateMap(JsonValue areaDatas)
        {
            for (int i = 0; i < areaDatas.Count; i++)
            {
                var data = areaDatas[i];
                var areaID = (int)data["areaID"];
                var info = map.GetAreaInfo(areaID);
                var isChanged = info.SetData(data);
                if (isChanged)
                {
                    SetAreaColor(info);
                }
            }
        }

        public void Update(double dt) {
            mapClickController.Update(dt);

            var areaID = mapClickController.ClickAreaID;
            if (areaID != Map.EmptyArea)
            {
            	var myPlayer = game.Player.KeyName;
                var myColor = game.Player.Color;
                var info = map.GetAreaInfo(areaID);
                var isChanged = info.SetData(myPlayer, myColor);
                if (isChanged)
                {
                	networkController.PostClickArea(areaID);
                	SetAreaColor(info);
                }
            }
        }
        
        private void SetAreaColor(AreaInfo info)
        {
        	var conflictColor = 255;
        	var maxPlayer = game.Match.MaxPlayer;

        	Color fillColor;
        	var colorCount = info.Colors.Count;
        	if (colorCount == 1) {
        		fillColor = Utils.ToColor(info.FirstColor);
        	}
        	else {
                var rate = 1.0 - ((double)colorCount / maxPlayer);
                var rgb = (byte)(conflictColor * rate);
        		fillColor = Color.FromArgb(255, rgb, rgb, rgb);
        	}
        	
        	var fillBrsuh = new SolidColorBrush(fillColor);
        	info.Shape.Fill = fillBrsuh;
        }
    }
}
