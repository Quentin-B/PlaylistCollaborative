using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using SocketIOClient;
//using SocketIOClient.Messages;
//using Quobject.SocketIoClientDotNet.Client;
using Newtonsoft.Json;
using SocketIOClient;



namespace ProjetSurface
{
    class SocketManager
    {
        private String ANDROID = "android_";
        private String SURFACE = "surface_";


        private string _serverURL;
        public string ServerUrl {
            get {
                return this._serverURL;
            }
            set {
                this._serverURL = value;
                this._connection();
            }
        }

        //public Client socket;
        //public Socket socket;

        public SocketManager(string serverUrl)
        {
            this.ServerUrl = serverUrl;
        }

        private void _connection()
        {
            
                System.Net.WebRequest.DefaultWebProxy = null;

                Client socket = new Client(this.ServerUrl);
                /*this.socket.Opened += SocketOpened;
                this.socket.Error += SocketError;
                this.socket.Message += SocketMessage;
                */

                socket.On("connect", (data) =>
                {
                    Console.WriteLine("Connected to PaintServer.");
                    socket.Emit("isTable", null);
                    socket.Emit("echo", "Hello World!");
                    Console.WriteLine("Emit isTable done.");

                });

                socket.On("echo back", (data) =>
                {
                    Console.WriteLine("Retour server : " + data.Json.Args[0]);
                    
                });

                socket.On(SURFACE + "getmusic", (data) =>
                {                                                     
                    Console.WriteLine("Event "+SURFACE+"getmusic received");
                    socket.Emit(SURFACE + "sendmusic", JsonConvert.SerializeObject(SurfaceWindow1.Playlist) );
                   
                });

                socket.On(SURFACE + "plus", (data) =>
                {
                    Console.WriteLine("Event " + SURFACE + "plus received");
                    String id_song = data.Json.Args[0];
                    SurfaceWindow1.plusASong(id_song);

                    //TODO
                    //redraw la bulle plus grosse en fonction du nombre de like
                });

                socket.On("disconnect", (data) =>
                {
                    Console.WriteLine("Disconnected from PaintServer.");
                });

                socket.On("reconnect", (data) =>
                {
                    Console.WriteLine("Connected to PaintServer after " + data + " attempts.");
                });

                socket.On("reconnect_attempt", (data) =>
                {
                    Console.WriteLine("Trying to reconnect to PaintServer.");
                });

                socket.On("reconnecting", (data) =>
                {
                    Console.WriteLine("Trying to connect to PaintServer - Attempt number " + data + ".");
                });

                socket.On("reconnect_error", (data) =>
                {
                    Console.WriteLine("An error occured during reconenction to PaintServer.");
                });

                socket.On("reconnect_failed", (data) =>
                {
                    Console.WriteLine("Failed to connect to PaintServer. No new attempt will be done.");
                });

                /*
                this.socket.On("newTablet", (data) =>
                {
                    Console.WriteLine(data);
                    Dictionary<string, string> dataJObject = JsonConvert.DeserializeObject<Dictionary<string, string>>(data as string);

                    Dictionary<string, string> viewportDesc = new Dictionary<string, string>();
                    viewportDesc.Add("id", dataJObject["id"]);
                    viewportDesc.Add("width", "200");
                    viewportDesc.Add("height", "300");
                    socket.Emit("setTabletViewport", JsonConvert.SerializeObject(viewportDesc));
                });
                */
                socket.Connect();
            
        }

        /*
        public void SocketOpened(object sender, EventArgs e)
        {
            Console.WriteLine("opened event handler");
            Console.WriteLine(e.ToString());
        }

        public void SocketError(object sender, SocketIOClient.ErrorEventArgs e)
        {
            Console.WriteLine("error event handler");
            Console.WriteLine(e.Message);
        }

        public void SocketMessage(object sender, MessageEventArgs e)
        {
            Console.WriteLine("message event handler");
            Console.WriteLine(e.Message);
        }

        */

        
    }
}
