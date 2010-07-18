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

namespace SilverlightGame.Scene
{
    public class GameScene
    {
        private GameXXX game;
        private Map map;

        private JsonObject reciveData;

        public GameScene(GameXXX game)
        {
            this.game = game;
        }

        public void Initialize()
        {
            this.map = this.game.Map;

            map.Visible = true;
            game.update += map.Update;
            game.update += Update;
        }

        public void Destroy()
        {
            game.update -= Update;
            game.update -= map.Update;
            game.Map.Visible = false;
        }

        public void Update(double dt) {
            var clickArea = map.ClickArea;
            if (clickArea != null) {
                var color = Utils.ToColor(game.PlayerInfo.Color);
                var brush = new SolidColorBrush(color);
                clickArea.Fill = brush;
            }
        }

    }
}
