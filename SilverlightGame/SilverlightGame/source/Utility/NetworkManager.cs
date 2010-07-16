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
using System.IO;
using System.Json;
using System.Diagnostics;
using SilverlightGame.source.Utility;

namespace TestSilver.source.Utility
{
    public class NetworkManager
    {
        public delegate void ReciveData(JsonObject data);
        public event ReciveData reciveData;

        private bool isPolling;

        private bool isSendPost;
        private string postData;
        
        private WebClient webClient;
//        private Uri serviceUri = new Uri("/game_server", UriKind.Relative);
//        private Uri serviceUri = new Uri("http://localhost:8080/game_server", UriKind.Absolute);
        private Uri serviceUri = new Uri("http://nico-nico.appspot.com/game_server", UriKind.Absolute);

        public NetworkManager()
        {
            this.webClient = new WebClient();
            this.webClient.DownloadStringCompleted += _DownloadStringCompleted;
            this.webClient.UploadStringCompleted += _UploadStringCompleted;

            this.isPolling = false;
            this.isSendPost = false;

            MyLog.WriteLine("--------NetworkManager serviceUri :" + serviceUri);
        }

        public void StartPolling()
        {
            if (this.isPolling) return;

            MyLog.WriteLine("--------StartPolling");

            this.isPolling = true;
            SendGet();
        }

        public void StopPolling()
        {
            if (this.isPolling == false) return;

            MyLog.WriteLine("--------StopPolling");


            this.isPolling = false;
        }

        public void SetSendPostRequest(string data)
        {
            MyLog.WriteLine(5,"        SetSendPostRequest :" + data);
            this.postData = data;
            this.isSendPost = true;
        }

        private void SendPost()
        {
            MyLog.WriteLine(5,"        SendPost :" + this.postData);
            this.isSendPost = false;
            webClient.UploadStringAsync(serviceUri, "POST", this.postData);
        }

        private void SendGet()
        {
            MyLog.WriteLine(1,"        SendGet");
            webClient.DownloadStringAsync(serviceUri);
        }

        private void _DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e == null) {
                MyLog.WriteLine(2, "!!!! _DownloadStringCompleted arg is null !!!!");
            }
            else if (e.Error != null)
            {
                MyLog.WriteLineError("!!!! _DownloadStringCompleted error !!!!");
                MyLog.WriteLineError("        " + e.Error.Message);
            }
            else
            {
                MyLog.WriteLine(2, "        _DownloadStringCompleted");
                var result = e.Result;
                JsonObject data = (JsonObject)JsonObject.Parse(result);

                MyLog.WriteLine(2, "            result : " + result);
                
                foreach (string key in data.Keys)
                {
                    MyLog.WriteLine(2, "            key : " + key + " data : " + data[key].ToString());
                }

                if (reciveData != null)
                {
                    reciveData(data);
                }
            }

            if (this.isSendPost) {
                SendPost();
            }
            else if (this.isPolling) {
                SendGet();
            }
        }

        private void _UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            if (e == null)
            {
                MyLog.WriteLine(4, "!!!! _UploadStringCompleted arg is null !!!!");
            }
            else if (e.Error != null)
            {
                MyLog.WriteLineError("!!!! _UploadStringCompleted error !!!!");
                MyLog.WriteLineError("        " + e.Error.Message);
            }
            else
            {
                MyLog.WriteLine(4, "        _UploadStringCompleted");
                var result = e.Result;
                JsonObject data = (JsonObject)JsonObject.Parse(result);

                MyLog.WriteLine(4, "            result : " + result);
                foreach (string key in data.Keys)
                {
                    MyLog.WriteLine(4, "            key : " + key + " data : " + data[key].ToString());
                }

                if (reciveData != null)
                {
                    reciveData(data);
                }
            }

            if (this.isSendPost)
            {
                SendPost();
            }
            else if (this.isPolling)
            {
                SendGet();
            }
        }
    }
}


#if false
    
        public void HowToMakeRequestsToHttpBasedServices()
        {
            Uri serviceUri = new Uri("/slTest", UriKind.Relative);
            WebClient downloader = new WebClient();
            downloader.OpenReadCompleted += new OpenReadCompletedEventHandler(downloader_OpenReadCompleted);
            downloader.OpenReadAsync(serviceUri);
        }

        void downloader_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                Stream responseStream = e.Result;

                JsonObject user = (JsonObject)JsonObject.Load(responseStream);
                string player = user["player"];
                bool is1 = user["is1"];
                int count = user["count"];

                var playerTextBox = new TextBox();
                playerTextBox.Text = player;

                var countTextBox = new TextBox();
                countTextBox.Text = "count :" + count;

                //this.stackPanel.Children.Add(playerTextBox);
                //this.stackPanel.Children.Add(countTextBox);
            }
        }
    }
//public class User
//{
//    public bool IsMember { get; set; }
//    public string Name { get; set; }
//    public int Age { get; set; }
//}

//DataContractJsonSerializer serializer = 
//    new DataContractJsonSerializer(typeof(User));
//User user = (User)serializer.ReadObject(responseStream);

//bool isMember = user.IsMember;
//string name = user.Name;
//int age = user.Age;
#endif