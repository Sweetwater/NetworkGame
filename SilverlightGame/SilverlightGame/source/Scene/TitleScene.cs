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
using System.Json;
using SilverlightGame.source.Utility;
using SilverlightGame.Object;

namespace SilverlightGame.Scene
{
    public class TitleScene
    {
        private GameXXX game;
        private MyImage image;
        private Graphic graphic;

        private Map map;

        private JsonObject reciveData;

        private int x = 0;
        private int y = 0;
        private int z = 0; 

        public TitleScene(GameXXX game)
        {
            this.game = game;
            this.graphic = game.Graphic;
        }

        public void Initialize() {
            this.image = ResouceManager.CreateImage("Media/brownplane1.png");

            this.map = new Map(this.game);
            this.map.CreateMap();

            //game.networkManager.reciveData += ReciveData;
            //game.networkManager.StartPolling();

            this.reciveData = null;
        }

        public void Destroy() {
            game.NetworkManager.StopPolling();
            game.NetworkManager.reciveData += ReciveData;
        }

        public void ReciveData(JsonObject data) {
            reciveData = data;
        }


        public void Update(double dt) {
            int oldX = x;
            int oldY = y;
            if (game.InputManager.isDown(Key.Left))
            {
                x -= 1;
            }
            else if (game.InputManager.isDown(Key.Right))
            {
                x += 1;
            }

            if (game.InputManager.isDown(Key.Up))
            {
                y -= 1;
            }
            else if (game.InputManager.isDown(Key.Down))
            {
                y += 1;
            }

            if (oldX != x || oldY != y)
            {
                var data = "x=" + x +"&y=" + y;
                x = oldX;
                y = oldY;
                game.NetworkManager.SetSendPostRequest(data);
            }

            if (reciveData != null) {
                x = (int)this.reciveData["x"];
                y = (int)this.reciveData["y"];
//                MyLog.WriteTextBlock("x: " + x + " y:" + y);

                this.reciveData = null;
            }
        }

        private int a = 0;
        public void Draw(double dt)
        {
            if (a == 0) {
              ///  graphic.DrawImage(image, x, y, z); 
            }
            a++;

           this.map.Draw(dt);
           
        }
    }
}
