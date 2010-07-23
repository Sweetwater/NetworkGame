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
using SilverlightGame.Utility;
using System.Collections.Generic;

namespace TestSilver.Network
{
    public class NetworkManager
    {
        public delegate void ReciveData(JsonObject data);
        public event ReciveData reciveData;

        private bool isSuspend;

        private double intervalTime = 1.0;
        private double intervalTimer;

        private bool isPolling;
        private string pollingData;
        
        private Queue<string> postDatas;

        private WebClient webClient;
        private Uri address;

        public NetworkManager()
        {
            this.isSuspend = false;
            this.webClient = new WebClient();
            this.postDatas = new Queue<string>();
        }

        public void Initialize(Uri address) {
            MyLog.WriteLine("--------NetworkManager Initialize :" + address);
            MyLog.WriteLine("    interval :" + intervalTime);


            this.intervalTimer = 0;

            this.address = address;
            this.webClient.UploadStringCompleted += UploadStringCompleted;
        }

        public void Destroy()
        {
            StopPolling();

            this.webClient.UploadStringCompleted -= UploadStringCompleted;
            MyLog.WriteLine("--------NetworkManager Desotroy ");
        }

        public void Suspend()
        {
            this.isSuspend = true;
            MyLog.WriteLine("--------NetworkManager Suspend ");
        }

        public void Resume()
        {
            this.isSuspend = false;
            MyLog.WriteLine("--------NetworkManager Resume ");
        }

        public void Update(double dt)
        {
            if (intervalTime > 0)
            {
                this.intervalTimer -= dt;
            }

            if (isSuspend) return;

            var isPostRequest = (postDatas.Count != 0);
            var isSendRequest = (isPolling || isPostRequest);
            var isNotBusy = (webClient.IsBusy == false);

            if (isSendRequest && isNotBusy && intervalTimer <= 0)
            {
                if (isPostRequest)
                {
                    SendPost(postDatas.Dequeue());
                }
                else if (isPolling)
                {
                    SendPolling();
                }
                this.intervalTimer = intervalTime;
            }
        }


        public void StartPolling(string pollingData)
        {
            MyLog.WriteLine("--------StartPolling");

            this.pollingData = pollingData;
            this.isPolling = true;
        }

        public void StopPolling()
        {
            this.isPolling = false;
            MyLog.WriteLine("--------StopPolling");
        }

        public void SendPostRequest(string data)
        {
            MyLog.WriteLine(5,"    SetSendPostRequest :" + data);
            this.postDatas.Enqueue(data);
        }

        private void SendPost(string data)
        {
            MyLog.WriteLine(5,"    SendPost :" + data);
            webClient.UploadStringAsync(address, "POST", data);
        }

        private void SendPolling()
        {
            MyLog.WriteLine(1,"    SendGet");
            webClient.UploadStringAsync(address, "POST", pollingData);
        }

        private void UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            if (e == null)
            {
                MyLog.WriteLine(4, "!!!! UploadStringCompleted arg is null !!!!");
            }
            else if (e.Error != null)
            {
                MyLog.WriteLineError("!!!! UploadStringCompleted error !!!!");
                MyLog.WriteLineError("        " + e.Error.Message);
            }
            else
            {
                MyLog.WriteLine(4, "    UploadStringCompleted");
                
                var result = e.Result;
                JsonObject data = (JsonObject)JsonObject.Parse(result);

                MyLog.WriteLine(4, "    result : " + result);
                foreach (string key in data.Keys)
                {
                    MyLog.WriteLine(4, "    key : " + key + " data : " + data[key].ToString());
                }

                if (reciveData != null)
                {
                    reciveData(data);
                }
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