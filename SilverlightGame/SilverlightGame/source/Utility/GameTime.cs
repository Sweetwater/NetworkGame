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
        private DateTime last;
        private DateTime now;
        public TimeSpan Elapsed { get; private set; }
        public double Delta { get; private set; }

        public GameTime ()
	    {
	    }

        public void Initialize()
        {
            last = DateTime.Now;
        }

        public void Update()
        {
            now = DateTime.Now;
            TimeSpan Elapsed = now - last;
            last = now;

            Delta = Elapsed.TotalSeconds;
        }
    }
}
