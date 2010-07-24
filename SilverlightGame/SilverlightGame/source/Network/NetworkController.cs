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
using TestSilver.Network;

namespace SilverlightGame.Network
{
    public class NetworkController
    {
        private GameXXX game;
        private NetworkManager network;

        public NetworkController(GameXXX game, NetworkManager network)
        {
            this.game = game;
            this.network = network;
        }

        public void StartPolling()
        {
            var data = new JsonObject();
            data["command"] = "polling";
            data["matchKeyName"] = game.Match.KeyName;
            var pollingData = "data=" + data.ToString();
            network.StartPolling(pollingData);
        }

        public void StopPolling()
        {
            network.StopPolling();
        }

        public void PostEntry()
        {
            var json = new JsonObject();
            json["command"] = "entry";
            var send = "data=" + json.ToString();
            network.SendPostRequest(send);
        }

        public void PostClickArea(int areaNumber)
        {
            var json = new JsonObject();
            json["command"] = "area_click";
            json["areaID"] = areaNumber;
            json["matchKeyName"] = game.Match.KeyName;
            json["playerKeyName"] = game.Player.KeyName;
            var send = "data=" + json.ToString();
            network.SendPostRequest(send);
        }
    }
}
