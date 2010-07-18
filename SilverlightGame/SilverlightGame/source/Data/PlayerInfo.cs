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
    public class PlayerInfo
    {
        public string KeyName;
        public string Name;
        public uint Color;

        public void SetData(JsonValue data)
        {
            this.KeyName = data["keyName"];
            this.Name = data["name"];
            this.Color = data["color"];

            MyLog.WriteLine("PlayerInfo SetData");
            MyLog.WriteLine("    keyName : " + this.KeyName);
            MyLog.WriteLine("    name    : " + this.Name);
            MyLog.WriteLine("    color   : " + this.Color);
        }
    }
}
