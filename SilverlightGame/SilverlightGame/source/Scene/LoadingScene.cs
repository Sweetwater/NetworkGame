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
using SilverlightGame.source.Utility;
using SilverlightGame.Object;
using System.Reflection.Emit;

namespace SilverlightGame.Scene
{
    public class LoadingScene
    {
        private GameXXX game;
        private JsonObject reciveData;

        bool isLoadComplete;

        private double minLoadTime = 5.0;
        private double elapsedTime;

        private TextBlock loandingText;

        public LoadingScene(GameXXX game)
        {
            this.game = game;
            this.loandingText = new TextBlock();
            this.loandingText.Inlines.Add("Now Loaind...");
            this.loandingText.FontSize = 40;
            this.loandingText.TextAlignment = TextAlignment.Center;
            var x = game.CenterX - loandingText.ActualWidth / 2;
            var y = game.CenterY - loandingText.ActualHeight / 2;
            this.loandingText.SetValue(Canvas.LeftProperty, x);
            this.loandingText.SetValue(Canvas.TopProperty, y);
        }

        public void Initialize()
        {
            this.game.Root.Children.Add(this.loandingText);
            this.elapsedTime = minLoadTime;

            var json = new JsonObject();
            json["command"] = "entry";
            var send = "data=" + json.ToString();

            game.Network.reciveData += ReciveData;
            game.Network.SetSendPostRequest(send);

            this.isLoadComplete = false;
        }

        public void Destroy()
        {
            game.Network.reciveData -= ReciveData;
            this.game.Root.Children.Remove(this.loandingText);
        }

        public void ReciveData(JsonObject data)
        {
            this.reciveData = data;
        }

        public void Update(double dt) {
            if (reciveData != null)
            {
                if (reciveData["command"] == "entry")
                {
                    this.game.Map.Initialize(reciveData["matchInfo"]["mapSeed"]);
                    this.isLoadComplete = true;
                }
                reciveData = null;
            }

            if (minLoadTime < 0)
            {
                if (isLoadComplete) {
                    Destroy();
                }
            }
            else {
                minLoadTime -= dt;
            }


        }
    }
}
