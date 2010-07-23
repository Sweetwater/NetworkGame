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

            mapClickController.Initialize(game.Input, mapDrawer);

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
            //for (int i = 0; i < areaDatas.Count; i++)
            //{
            //    var data = areaDatas[i];
            //    var number = (int)data["number"];
            //    var colors = data["colors"];
            //    var color = Utils.ToColor(0);
            //    for (int j = 0; j < colors.Count; j++)
            //    {
            //        var color2 = Utils.ToColor((uint)colors[j]);
            //        color = Utils.AddColor(color, color2);
            //    }
            //    var brush = new SolidColorBrush(color);
            //    mapDrawer.GetAreaShape(number).Fill = brush;
            //}
        }

        public void Update(double dt) {
            mapClickController.Update(dt);

            var areaID = mapClickController.ClickAreaID;
            if (areaID != Map.EmptyArea)
            {
                networkController.PostClickArea(areaID);
            }

            // TODO エリア塗り処理
            //　クライアントでの塗りと
            //　ネットワークからの塗りを統合する
            // マップクラスにやらせる？
            // エリアデータは誰が持つ？
            //　マップにチェンジドイベントで
            //　ドロワで受け取る？

            //if (clickArea != null) {
            //    var color = Utils.ToColor(game.Player.Color);
            //    var brush = new SolidColorBrush(color);
            //    clickArea.Fill = brush;
            //}
        }
    }
}
