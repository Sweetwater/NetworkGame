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
using TestSilver.Utility;

namespace SilverlightGame.Scene
{
    public class GameScene
    {
        private GameXXX game;
        private Map map;
        private NetworkManager network;

        private JsonObject reciveData;

        public GameScene(GameXXX game)
        {
            this.game = game;
        }

        public void Initialize()
        {
            this.map = this.game.Map;
            this.network = this.game.Network;

            var data = new JsonObject();
            data["command"] = "polling";
            data["matchKeyName"] = game.Match.KeyName;
            var pollingData = "data=" + data.ToString();
            network.reciveData += ReciveData;
            network.StartPolling(pollingData);

            map.Visible = true;
            game.update += map.Update;
            game.update += Update;
        }

        public void Destroy()
        {
            network.StopPolling();
            network.reciveData -= ReciveData;

            game.update -= Update;
            game.update -= map.Update;
            game.Map.Visible = false;
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
                var number = (int)data["number"];
                var colors = data["colors"];
                var color = Utils.ToColor(0);
                for (int j = 0; j < colors.Count; j++)
                {
                    var color2 = Utils.ToColor((uint)colors[j]);
                    color = Utils.AddColor(color, color2);
                }
                var brush = new SolidColorBrush(color);
                map.GetAreaShape(number).Fill = brush;
            }
        }

        public void Update(double dt) {
            var clickArea = map.ClickArea;
            if (clickArea != null) {
                var color = Utils.ToColor(game.Player.Color);
                var brush = new SolidColorBrush(color);
                clickArea.Fill = brush;

                var json = new JsonObject();
                json["command"] = "area_click";
                json["matchKeyName"] = game.Match.KeyName;
                json["playerKeyName"] = game.Player.KeyName;
                json["areaNumber"] = (int)clickArea.Tag;

                var send = "data=" + json.ToString();
                this.game.Network.SetSendPostRequest(send);
            }
        }

    }
}
