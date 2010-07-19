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
using System.Json;
using SilverlightGame.Utility;

namespace SilverlightGame.Data
{
    public class MatchInfo
    {
        public string KeyName;
        public int MapSeed;

        public void SetData(JsonValue data)
        {
            this.KeyName = data["keyName"];
            this.MapSeed = data["mapSeed"];

            MyLog.WriteLine("MatchInfo SetData");
            MyLog.WriteLine("    KeyName : " + this.KeyName);
            MyLog.WriteLine("    MapSeed : " + this.MapSeed);
        }
    }
}
