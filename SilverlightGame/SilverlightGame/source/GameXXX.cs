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
using SilverlightGame.Object;

namespace SilverlightGame
{
    public class GameXXX
    {
        public delegate void Update(double dt);
        public event Update update;

        public delegate void Draw(double dt);
        public event Draw draw;

        public InputManager InputManager;
        public NetworkManager NetworkManager;

        public Camera Camera;

        public Graphic Graphic;

        public MainPage MainPage { get; set; }
        public Canvas RootContainer { get; set; }

        public double SreenWidth {
            get { return RootContainer.Width; }
        }
        public double SreenHeight
        {
            get { return RootContainer.Height; }
        }

        public double CenterX
        { 
            get { return RootContainer.Width / 2d; }
        }
        public double CenterY
        {
            get { return RootContainer.Height / 2d; }
        }

        private Random syncRandom;
        public Random SyncRandom {
            get { return syncRandom; }
        }

        protected DateTime lastTick;

        public GameXXX()
        {
        }

        public void Initialize()
        {
            SetInitialParam();

            var seed = 123485606;
            this.syncRandom = new Random();

            this.InputManager = new InputManager();
            this.InputManager.Initialize(this.MainPage);
            this.update += this.InputManager.Update;

            this.Camera = new Camera(this);
            this.Camera.Origin = new Point(CenterX, CenterY);
            this.update += this.Camera.Update;

            this.Graphic = new Graphic(this);

            this.NetworkManager = new NetworkManager();
            
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

            this.NetworkManager.StopPolling();

            this.update -= this.InputManager.Update;
            this.InputManager.Destroy();
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

            if (InputManager.isDown(Key.Escape))
            {
                Destroy();
            }

//            MyLog.WriteTextBlock("Zoom :" + this.Camera.Zoom);

            if (update != null) update(deltaTime);
            if (draw != null) draw(deltaTime);
        }
    }
}
