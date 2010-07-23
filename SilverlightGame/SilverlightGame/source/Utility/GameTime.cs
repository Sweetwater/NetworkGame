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

namespace SilverlightGame.Utility
{
    public class GameTime
    {
        public DateTime StartTime{ get; private set; }
        public TimeSpan TotalElapsed { get; private set; }
        public TimeSpan FrameElapsed { get; private set; }
        public double Delta { get; private set; }

        private DateTime lastFrame;
        private DateTime nowFrame;

        public GameTime ()
	    {
	    }
        public void Initialize()
        {
            Reset();
        }
        public void Reset()
        {
            StartTime = DateTime.Now;
            lastFrame = StartTime;
        }

        public void Update()
        {
            nowFrame = DateTime.Now;
            TotalElapsed = nowFrame - StartTime;
            FrameElapsed = nowFrame - lastFrame;
            lastFrame = nowFrame;

            Delta = FrameElapsed.TotalSeconds;
        }
    }
}
