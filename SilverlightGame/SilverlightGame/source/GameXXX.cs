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
using TestSilver.Network;
using SilverlightGame.Object;
using SilverlightGame.Data;
using SilverlightGame.Network;

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
        public delegate void Draw(double dt);
        public delegate void Resume();
        public delegate void Suspend();
        public event Update update;
        public event Draw draw;
        public event Resume resume;
        public event Suspend suspend;

        public Canvas Root { get; private set; }

        public GameTime Time { get; private set; }

        public InputManager Input { get; private set; }
        public NetworkManager Network { get; private set; }
        public NetworkController NetworkController { get; private set; }
        
        public Graphic Graphic { get; private set; }
        public Random SyncRandom { get; private set; }
        
        public Camera Camera { get; private set; }
        public CameraController CameraController { get; private set; }

        public Map Map { get; private set; }
        public MapDrawer MapDrawer { get; private set; }
        public MatchInfo Match { get; private set; }
        public PlayerInfo Player { get; private set; }

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

        private TimeSpan gameTimeLimit = new TimeSpan(0, 5, 0);

        public GameXXX(Canvas root)
        {
            this.Root = root;
            this.Time = new GameTime();
            this.Input = new InputManager();
            this.Network = new NetworkManager();
            this.NetworkController = new NetworkController(this, Network);
            this.Camera = new Camera();
            this.Map = new Map(this);
            this.MapDrawer = new MapDrawer(this);
            this.Match = new MatchInfo();
            this.Player = new PlayerInfo();
        }

        public void Initialize() {
            MyLog.WriteLine("--GameXXX Initialize -------- begin");
            SetInitialParam();

            Time.Initialize();
            Input.Initialize(this.Root);
            Network.Initialize(this.serviceUri);

            this.CameraController = new CameraController(Camera, Input);

            this.update += Network.Update;
            this.update += Input.Update;
            this.update += CameraController.Update;

            this.suspend += Network.Suspend;
            this.resume += Network.Resume;

            CompositionTarget.Rendering += CompositionTarget_Rendering;
            Root.LostFocus += LostFocus;
            Root.GotFocus += GotFocus;

            Reset();
            MyLog.WriteLine("--GameXXX Initialize -------- end");
        }

        public void Destroy()
        {
            MyLog.WriteLine("--GameXXX Destroy -------- begin");
            
            Root.GotFocus += GotFocus;
            Root.LostFocus += LostFocus;
            CompositionTarget.Rendering -= CompositionTarget_Rendering;

            this.resume += Network.Resume;
            this.suspend += Network.Suspend;

            this.update -= CameraController.Update;
            this.update -= Input.Update;
            this.update -= Network.Update;

            this.Input.Destroy();
            this.Network.Destroy();
            this.MapDrawer.Destroy();
            this.Map.Destroy();

            MyLog.WriteLine("--GameXXX Destroy -------- end");
        }

        private void SetInitialParam()
        {
            MyLog.WriteLine("--GameXXX SetInitialParam -------- being");
            var initParame = Application.Current.Host.InitParams;
            foreach (string key in initParame.Keys)
            {
                Debug.WriteLine("initParam : " + key + " : " + initParame[key]);
            }
            MyLog.WriteLine("--GameXXX SetInitialParam -------- end");
        }


        public void Reset()
        {
            MyLog.WriteLine("--GameXXX Reset -------- begin");
            MyLog.WriteLine("TimeLimit :" + gameTimeLimit);
            Time.Reset();

            Camera.Origin = new Point(HalfWidth, HalfHeight);
            Camera.Position = new Point(0, 0);
            CameraController.MinLimit = new Point(-HalfWidth, -HalfHeight);
            CameraController.MaxLimit = new Point(HalfWidth, HalfHeight);

            var loadingScene = new LoadingScene(this);
            loadingScene.Initialize();

            MyLog.WriteLine("--GameXXX Reset -------- end");
        }

        public void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            EnterFrame();
        }
        public void LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.suspend != null) suspend();
        }
        public void GotFocus(object sender, RoutedEventArgs e)
        {
            if (this.resume != null) resume();
        }

        public void EnterFrame()
        {
            if (Input.isDown(Key.Escape))
            {
                MyLog.WriteLine("!!!! GameXXX ESC !!!!");
                Destroy();
            }

            Time.Update();
            if (Time.TotalElapsed > gameTimeLimit)
            {
                MyLog.WriteLine("!!!! GameXXX time limit over !!!!");
                Destroy();
            }

            if (update != null) update(Time.Delta);
            if (draw != null) draw(Time.Delta);
        }
    }
}
