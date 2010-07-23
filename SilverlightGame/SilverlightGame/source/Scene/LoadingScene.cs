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
            this.loandingText.Inlines.Add("Now Loading...");
            this.loandingText.FontSize = 40;
            this.loandingText.TextAlignment = TextAlignment.Center;
            var x = game.CenterX - loandingText.ActualWidth / 2;
            var y = game.CenterY - loandingText.ActualHeight / 2;
            this.loandingText.SetValue(Canvas.LeftProperty, x);
            this.loandingText.SetValue(Canvas.TopProperty, y);
        }

        public void Initialize()
        {
            this.elapsedTime = minLoadTime;

            this.game.Root.Children.Add(this.loandingText);

            game.update += this.Update;

            game.Network.reciveData += ReciveData;
            game.NetworkController.PostEntry();

            this.isLoadComplete = false;
        }

        public void Destroy()
        {
            game.Network.reciveData -= ReciveData;
            game.update -= this.Update;

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
                    SetEntryData(reciveData);
                    this.isLoadComplete = true;
                }
                reciveData = null;
            }

            if (minLoadTime > 0)
            {
                minLoadTime -= dt;
            }
            else if (isLoadComplete)
            {
                Destroy();
                var gameScene = new GameScene(this.game);
                gameScene.Initialize();
            }
        }

        private void SetEntryData(JsonObject data)
        {
            game.Match.SetData(data["matchInfo"]);
            game.Player.SetData(data["playerInfo"]);
            game.Map.Initialize(game.Match.MapSeed);
            game.MapDrawer.Initialize(game.Map, game.Match.MapSeed);
        }
    }
}
