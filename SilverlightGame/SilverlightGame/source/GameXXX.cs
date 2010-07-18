#define LOCAL_ABS
//#define SERVER_ABS

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
using SilverlightGame.source.Object;

namespace SilverlightGame
{
    public class GameXXX
    {

#if LOCAL_ABS && DEBUG
        private Uri serviceUri = new Uri("http://localhost:8080/command", UriKind.Absolute);
#elif GROBAL_ABS && DEBUG
        private Uri serviceUri = new Uri("http://nico-nico.appspot.com/command", UriKind.Absolute);
#else
        private Uri serviceUri = new Uri("/command", UriKind.Relative);
#endif

        public delegate void Update(double dt);
        public event Update update;

        public delegate void Draw(double dt);
        public event Draw draw;

        public Canvas Root { get; private set; }

        public GameTime Time { get; private set; }

        public InputManager Input { get; private set; }
        public NetworkManager Network { get; private set; }
        
        public Graphic Graphic { get; private set; }
        public Random SyncRandom { get; private set; }
        
        public Camera Camera { get; private set; }
        public CameraController CameraController { get; private set; }

        public Map Map { get; private set; }


        public double SreenWidth {
            get { return Root.Width; }
        }
        public double SreenHeight {
            get { return Root.Height; }
        }

        public double HalfWidth
        {
            get { return Root.Width / 2d; }
        }
        public double HalfHeight
        {
            get { return Root.Height / 2d; }
        }

        public double CenterX {
            get { return Root.Width / 2d; }
        }
        public double CenterY {
            get { return Root.Height / 2d; }
        }

        protected DateTime lastTick;

        public GameXXX(Canvas root)
        {
            this.Root = root;
            this.Time = new GameTime();
            this.Input = new InputManager();
            this.Network = new NetworkManager();
            this.Camera = new Camera();
            this.Map = new Map(this);
        }

        public void Initialize() {
            MyLog.WriteLine("GameXXX Initialize");
            SetInitialParam();

            Time.Initialize();
            Input.Initialize(this.Root);
            Network.Initialize(this.serviceUri);

            this.CameraController = new CameraController(Camera, Input);

            this.update += Input.Update;
            this.update += CameraController.Update;

            CompositionTarget.Rendering += CompositionTarget_Rendering;

            Reset();
        }

        public void Destroy()
        {
            MyLog.WriteLine("GameXXX Destroy");
            
            CompositionTarget.Rendering -= CompositionTarget_Rendering;

            this.update -= CameraController.Update;
            this.update -= Input.Update;

            this.Input.Destroy();
            this.Network.Destroy();

            MyLog.WriteLine("GameXXX Destroy End");
        }

        private void SetInitialParam()
        {
            var initParame = Application.Current.Host.InitParams;
            foreach (string key in initParame.Keys)
            {
                Debug.WriteLine("initParam : " + key + " : " + initParame[key]);
            }
        }


        public void Reset()
        {
            MyLog.WriteLine("GameXXX Reset");

            Camera.Origin = new Point(HalfWidth, HalfHeight);
            Camera.Position = new Point(0, 0);
            CameraController.MinLimit = new Point(-HalfWidth, -HalfHeight);
            CameraController.MaxLimit = new Point(HalfWidth, HalfHeight);

            var loadingScene = new LoadingScene(this);
            loadingScene.Initialize();
            this.update += loadingScene.Update;
        }

        public void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            EnterFrame();
        }

        public void EnterFrame()
        {
            if (Input.isDown(Key.Escape))
            {
                MyLog.WriteLine("GameXXX ESC");
                Destroy();
            }

            Time.Update();

            if (update != null) update(Time.Delta);
            if (draw != null) draw(Time.Delta);
        }
    }
}
