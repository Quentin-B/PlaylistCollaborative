using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Input;
using System.Diagnostics;
using System.Net;
using System.IO;
using System.Threading;
using Un4seen.Bass;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace ProjetSurface
{
    /// <summary>
    /// Interaction logic for SurfaceWindow1.xaml
    /// </summary>
    public partial class SurfaceWindow1 : SurfaceWindow
    {
        private Process _serverProcess;
        private static LinkedList<Song> playlist;

        public static LinkedList<Song> Playlist
        {
            get { return playlist; }
            set { playlist = value; }
        }

        //private String _serverAddress = "http://134.59.215.194:8080";
        private String _serverAddress = "http://nodejs-ihmdj.rhcloud.com:8000";
        private SocketManager _sm;

        //Declare a delegate for Async operation.
        public delegate void AsyncMethodCaller();

        private Random m_Random;

        private Player player;

        private ScatterViewItem item;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SurfaceWindow1()
        {
            InitializeComponent();

            // Add handlers for window availability events
            AddWindowAvailabilityHandlers();

            /*this._startServer();

            string localIp = this._getLocalIPAddress();

            Console.WriteLine("http://" + localIp + ":" + this._serverPort.ToString());

            this._sm = new SocketManager("http://localhost:" + this._serverPort.ToString());
            */
            this.m_Random = new Random();

            player = new Player();
            playlist = new LinkedList<Song>();
            _initializeSongs();

            _initializeSocket();

            //Music music = new Music("Id_music", "Title_music", "Artist_music", "Genre_music");
            //string json = JsonConvert.SerializeObject(employee, Formatting.Indented, new KeysJsonConverter(typeof(Employee)));
           
            //Console.WriteLine("valeur de music : " + JsonConvert.SerializeObject(music));

            //stopButton.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.stop));
            //playButton.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.play));
            
            //for (int i = 0; i < 6; i++)
            //{
               // Thread t = new Thread(() =>
               // {
               // });

               //  t.Start();
            //}

            //player = new Player();
            //player.LoadSong("../../Resources/Flashlight.mp3");
            //player.PlaySong(false);

            stopButton.Click += btnStop_Click;
            stopButton.MouseDown += btnStop_Click;
            playButton.Click += btnPlay_Click;
            playButton.MouseDown += btnPlay_Click;

        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            player.StopSong();
            item.Width += 100;
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            player.PlaySong(false);
        }

        private void _initializeSocket()
        {
            this._sm = new SocketManager(this._serverAddress);
        }

        private void _initializeSongs()
        {
            Song s1 = new Song("Flashlight", "Inconnu", "../../Resources/Flashlight.mp3");
            Song s2 = new Song("Flashlight", "Inconnu", "../../Resources/Hot Hands.mp3");
            Song s3 = new Song("Much too", "Inconnu", "../../Resources/Much Too Much.mp3");
            Song s4 = new Song("Flashlight", "Inconnu", "../../Resources/Flashlight.mp3");
            Song s5 = new Song("Flashlight", "Inconnu", "../../Resources/Flashlight.mp3");

            SurfaceWindow1.playlist.AddLast(s1);
            SurfaceWindow1.playlist.AddLast(s2);
            SurfaceWindow1.playlist.AddLast(s3);
            SurfaceWindow1.playlist.AddLast(s4);
            SurfaceWindow1.playlist.AddLast(s5);

            _newBubble(s1);
            _newBubble(s2);
            _newBubble(s3);
            _newBubble(s4);
            _newBubble(s5);
        }

        private void _newBubble(Song s)
        {
            item = new ScatterViewItem();
            item.Width = 200;
            item.Height = 200;
            //item.Content = "Object 2";
            item.Orientation = 0;
            item.CanScale = false;
            item.CanMove = true;
            item.Center = new Point(300, 100);
            //item.ActualCenter = new Point(300, 100);
            item.Background = new SolidColorBrush(Colors.Transparent);
            item.ShowsActivationEffects = false;
            Canvas canvas = new Canvas();
            canvas.Width = 200;
            canvas.Height = 200;

            Image image = new Image();
            image.Width = canvas.Width;
            image.Height = canvas.Height;
            image.Source = new BitmapImage(
                new Uri("Resources/bubble.png", UriKind.Relative));

            canvas.Children.Add(image);

            TextBlock textBlock = new TextBlock();
            textBlock.Text = s.Name;
            textBlock.Foreground = new SolidColorBrush(Colors.White);
            textBlock.TextAlignment = TextAlignment.Center;
            textBlock.TextWrapping = TextWrapping.Wrap;
            textBlock.Margin = new Thickness(40, 0, 40, 0);
            Canvas.SetLeft(textBlock, 0);
            Canvas.SetTop(textBlock, 100);

            canvas.Children.Add(textBlock);

            //DoubleAnimation da = new DoubleAnimation();
            //da.From = 0;
            //da.To = 360;
            //da.Duration = new Duration(TimeSpan.FromSeconds(5));
            //da.RepeatBehavior = RepeatBehavior.Forever;
            //rotateTransform.BeginAnimation(RotateTransform.AngleProperty, da);

            item.Content = canvas;

            test_bubble.Items.Add(item);

            /*DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 360;
            da.Duration = new Duration(TimeSpan.FromSeconds(5));
            da.RepeatBehavior = RepeatBehavior.Forever;
            canvas.BeginAnimation(RotateTransform.AngleProperty, da);*/

            //Thread t = new Thread(() =>
            //{
            // });

            //  t.Start();
            startMoving(item, s);
            //});
            //t.Start();
        }

        private void _startServer()
        {
            this._serverProcess = new Process();
            this._serverProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            this._serverProcess.StartInfo.CreateNoWindow = false;
            this._serverProcess.StartInfo.UseShellExecute = false;
            this._serverProcess.StartInfo.FileName = "cmd.exe";
            this._serverProcess.StartInfo.Arguments = "/c cd ../../../PaintServer/PaintServer/ & node PaintServer.js";
            this._serverProcess.EnableRaisingEvents = true;
            this._serverProcess.Start();
        }

        private string _getLocalIPAddress()
        {
            IPHostEntry host;
            string localIP = "";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    break;
                }
            }
            return localIP;
        }

        /// <summary>
        /// Occurs when the window is about to close. 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            this._serverProcess.CloseMainWindow();

            // Remove handlers for window availability events
            RemoveWindowAvailabilityHandlers();
        }

        /// <summary>
        /// Adds handlers for window availability events.
        /// </summary>
        private void AddWindowAvailabilityHandlers()
        {
            // Subscribe to surface window availability events
            ApplicationServices.WindowInteractive += OnWindowInteractive;
            ApplicationServices.WindowNoninteractive += OnWindowNoninteractive;
            ApplicationServices.WindowUnavailable += OnWindowUnavailable;
        }

        /// <summary>
        /// Removes handlers for window availability events.
        /// </summary>
        private void RemoveWindowAvailabilityHandlers()
        {
            // Unsubscribe from surface window availability events
            ApplicationServices.WindowInteractive -= OnWindowInteractive;
            ApplicationServices.WindowNoninteractive -= OnWindowNoninteractive;
            ApplicationServices.WindowUnavailable -= OnWindowUnavailable;
        }

        /// <summary>
        /// This is called when the user can interact with the application's window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowInteractive(object sender, EventArgs e)
        {
            //TODO: enable audio, animations here
        }

        /// <summary>
        /// This is called when the user can see but not interact with the application's window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowNoninteractive(object sender, EventArgs e)
        {
            //TODO: Disable audio here if it is enabled

            //TODO: optionally enable animations here
        }

        /// <summary>
        /// This is called when the application's window is not visible or interactive.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowUnavailable(object sender, EventArgs e)
        {
            //TODO: disable audio, animations here
        }

        private void startMoving(ScatterViewItem s, Song song)
    {
        Point newPoint;
        newPoint = GetRandomPoint();

        //MoveTo(test_bubble,newPoint.X, newPoint.Y);
        Application.Current.Dispatcher.Invoke(new Action(() => MoveTo(s, newPoint.X, newPoint.Y, 7.0f, false, song)));
        //startMoving();
    }

        public void MoveTo(ScatterViewItem target, double newX, double newY, float duration, bool stop, Song song)
        {

            Storyboard stb = new Storyboard();
            PointAnimation moveCenter = new PointAnimation();
            Point endPoint = new Point(newX, newY);

            moveCenter.From = target.Center;
            moveCenter.To = endPoint;
            moveCenter.Duration = new Duration(TimeSpan.FromSeconds(duration));
            moveCenter.FillBehavior = FillBehavior.Stop;

            stb.Children.Add(moveCenter);

            Storyboard.SetTarget(moveCenter, target);
            Storyboard.SetTargetProperty(moveCenter, new PropertyPath(ScatterViewItem.CenterProperty));

            moveCenter.Completed += (sender, eventArgs) =>
            {
                if (!(sender is Mouse)&&(!stop))
                {
                    target.Center = new Point(newX, newY);
                    startMoving(target, song);
                }
            };

            stb.Begin(this, true);

            target.TouchDown += (sender, eventArgs) =>
            //target.MouseDown += (sender, eventArgs) =>
            {
                stb.Stop(this);
                target.Center = target.ActualCenter;
                eventArgs.Handled = true;
            };

            target.TouchUp += (sender, eventArgs) =>
            //target.MouseUp += (sender, eventArgs) =>
            {
                target.Center = target.ActualCenter;
                //target.Center = eventArgs.GetPosition(null);
                //startMoving(target);
                Application.Current.Dispatcher.Invoke(new Action(() => fadeAnimation(1.0f, 0.0f, 1.0f, target)));

                player.StopSong();
                player.LoadSong(song.Location);
                player.PlaySong(false);
                eventArgs.Handled = true;
            };

            target.Center = target.ActualCenter;
        }

        private void fadeAnimation(float from, float to, float duration, ScatterViewItem target)
        {
            #region Fade in
            // Create a storyboard to contain the animations.
            Storyboard storyboard = new Storyboard();

            // Create a DoubleAnimation to fade the not selected option control
            DoubleAnimation animation = new DoubleAnimation();

            animation.From = from;
            animation.To = to;
            animation.Duration = new Duration(TimeSpan.FromSeconds(duration));
            //animation.FillBehavior = FillBehavior.Stop; 
            // Configure the animation to target de property Opacity
            Storyboard.SetTarget(animation, target);
            Storyboard.SetTargetProperty(animation, new PropertyPath(ScatterViewItem.OpacityProperty));
            // Add the animation to the storyboard
            storyboard.Children.Add(animation);

            storyboard.Completed += delegate(object sender, EventArgs e)
            {
                // call UIElementManager to finally hide the element
                //this.UIElementManager.GetInstance().Hide(target);
                //target.Opacity = to; // otherwise Opacity will be reset to 1
                //RemoveChildHelper.RemoveChild(test_bubble, target);
                test_bubble.Items.Remove(target);

                Canvas canvas = new Canvas();
                canvas.Width = 200;
                canvas.Height = 200;


                Image image = new Image();
                image.Width = 200;
                image.Height = 200;
                image.Source = new BitmapImage(
                    new Uri("Resources/bubble.png", UriKind.Relative));

                canvas.Children.Add(image);
                //canvas.SetTop(canvas, NewBody.YPosition);
                //canvas.SetLeft(canvas, NewBody.XPosition);

                playlistPanel.Children.Add(canvas);
            };

            // Begin the storyboard
            storyboard.Begin(this, true);

            #endregion

        }


        private Point GetRandomPoint()
        {
            var possibleX = new List<int> { 0, 1400, 0, 800};

            int x;
            int y;
            x = m_Random.Next(0, 4);
            if ((x == 0)||(x == 1))
            {
                x = possibleX[x];
                y = m_Random.Next(0, 800);
            }
            else
            {
                y = possibleX[x];
                x = m_Random.Next(0, 1400);
            }
            return new Point(x,y);
        }
    }
}