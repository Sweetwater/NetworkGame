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
using TestSilver;
using SilverlightGame.Utility;
using System.Diagnostics;
using System.Reflection;
using SilverlightGame.Scene;
using SilverlightGame.Graphics;
using System.Windows.Interop;
using System.Collections;
using System.Collections.Generic;
using TestSilver.source.Utility;
using SilverlightGame.source.Utility;

namespace SilverlightGame
{
    public class GameXXX
    {
        public delegate void Update(double dt);
        public event Update update;

        public delegate void Draw(double dt);
        public event Draw draw;

        public InputManager inputManager;
        public NetworkManager networkManager;

        public Graphic graphic;

        public MainPage MainPage { get; set; }
        public Canvas RootContainer { get; set; }

        protected DateTime lastTick;


        public GameXXX()
        {
        }

        public void Initialize()
        {
            SetInitialParam();

            this.graphic = new Graphic(this);

            this.inputManager = new InputManager();
            this.inputManager.Initialize(this.MainPage);
            this.update += this.inputManager.Update;

            this.networkManager = new NetworkManager();
            
            var titileScene = new TitleScene(this);
            titileScene.Initialize();
            this.update += titileScene.Update;
            this.draw += titileScene.Draw;

            lastTick = DateTime.Now;
            CompositionTarget.Rendering += CompositionTarget_Rendering;
        }

        private void SetInitialParam()
        {
            var initParame = Application.Current.Host.InitParams;
            foreach (string key in initParame.Keys)
            {
                Debug.WriteLine("initParam : " + key + " : " + initParame[key]);
            }
        }

        public void Destroy()
        {
            CompositionTarget.Rendering -= CompositionTarget_Rendering;

            this.networkManager.StopPolling();

            this.update -= this.inputManager.Update;
            this.inputManager.Destroy();
        }

        public void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            EnterFrame();
        }

        public void EnterFrame()
        {
            DateTime now = DateTime.Now;
            TimeSpan elapsed = now - lastTick;
            lastTick = now;

            double deltaTime = elapsed.TotalSeconds;

            if (inputManager.isDown(Key.Escape))
            {
                Destroy();
            }

            if (update != null) update(deltaTime);
            if (draw != null) draw(deltaTime);
        }
    }
}
