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
using System.Windows.Media.Effects;

namespace ProjetSurface
{
    /// <summary>
    /// Interaction logic for SurfaceWindow1.xaml
    /// </summary>
    public partial class SurfaceWindow1 : SurfaceWindow
    {
        private Process _serverProcess;
        private Dictionary<String, Bubble> bubblesList;

        private PlayList playList;

        internal PlayList PlayList
        {
            get { return playList; }
            set { playList = value; }
        }

        private FileDeLecture fileLecture;

        internal FileDeLecture FileLecture
        {
            get { return fileLecture; }
            set { fileLecture = value; }
        }

        //private String _serverAddress = "http://134.59.215.194:8080";
        private String _serverAddress = "http://nodejs-ihmdj.rhcloud.com:8000";
        private SocketManager _sm;

        //Declare a delegate for Async operation.
        public delegate void AsyncMethodCaller();

        private Random m_Random;

        private Player player;

        private Bubble bubble;

        private Storyboard actualSongAnimation;

        private Song songDragged;

        bool captured = false;
        double x_shape, x_canvas, y_shape, y_canvas;
        UIElement source = null;
        UIElement sourceSelected = null;
        Image imageVinyl;

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

            bubblesList = new Dictionary<string, Bubble>();
            playList = PlayList.Instance;
            fileLecture = FileDeLecture.Instance;

            _initializeSongs();

            _initializeSocket();

            player = new Player(this);

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
            stopButton.TouchDown += btnStop_Click;
            playButton.Click += btnPlay_Click;
            playButton.TouchDown += btnPlay_Click;
            previousButton.Click += btnPrevious_Click;
            previousButton.TouchDown += btnPrevious_Click;
            nextButton.Click += btnNext_Click;
            nextButton.TouchDown += btnNext_Click;

        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            if(actualSongAnimation != null)
                actualSongAnimation.Stop();
            
            if (fileLecture.isEmpty())
                return;

            Song s = fileLecture.Previous();
            player.PlaySong(false, s, true);

            updateBubble();

        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (fileLecture.isEmpty())
                return;

            Song s = fileLecture.Next();
            if (s != null)
            {
                if (actualSongAnimation != null)
                {
                    actualSongAnimation.Stop();
                }
                player.PlaySong(false, s, true);
                updateBubble();
            }
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            if (actualSongAnimation != null)
                actualSongAnimation.Stop();

            if (fileLecture.isEmpty())
                return;

            player.StopSong();
            e.Handled = true;
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            if (fileLecture.isEmpty())
                return;

            player.PlaySong(false);        
            e.Handled = true;

            updateBubble();
        }

        private void _initializeSocket()
        {
            this._sm = new SocketManager(this._serverAddress, this);
        }

        private void _initializeSongs()
        {
            Song s1 = new Song("Flashlight", "M. Poisson", "../../Resources/Flashlight.mp3", Song.Category.TECHNO);
            Song s2 = new Song("Hot Hands", "M. Soleil", "../../Resources/Hot Hands.mp3", Song.Category.TECHNO);
            Song s3 = new Song("Want U Back", "Jackson 5", "../../Resources/i_want_you_back-jackson5.mp3", Song.Category.ANNEES_70);
            Song s4 = new Song("ABC", "Jackson 5", "../../Resources/abc-jackson5.mp3", Song.Category.ANNEES_70);
            Song s5 = new Song("Thriller", "Michael Jackson", "../../Resources/thriller-michael_jackson.mp3", Song.Category.ANNEES_80);
            Song s6 = new Song("Beat It", "Michael Jackson", "../../Resources/beat_it-michael_jackson.mp3", Song.Category.ANNEES_80);
            Song s7 = new Song("No woman no cry", "Bob Marley", "../../Resources/Bob_Marley-No_woman_no_cry.mp3", Song.Category.REGGAE);
            Song s8 = new Song("Buffalo soldier", "Bob Marley", "../../Resources/Bob_Marley-Buffalo_Soldier.mp3", Song.Category.REGGAE);
            Song s9 = new Song("One Love", "Bob Marley", "../../Resources/Bob_Marley-One_Love.mp3", Song.Category.REGGAE);
            Song s10 = new Song("Reggae Shark", "M. Shark", "../../Resources/The_Key_of_Awesome-Reggae_Shark.mp3", Song.Category.REGGAE);
            Song s11 = new Song("Hunger of the pine", "Alt-J", "../../Resources/Alt-J-Hunger_Of_The_Pine.mp3", Song.Category.POP_ROCK);
            Song s12 = new Song("Left Hand Free", "Alt-J", "../../Resources/alt-J-Left_Hand_Free.mp3", Song.Category.POP_ROCK);
            Song s13 = new Song("Flitzpleasure", "Alt-J", "../../Resources/Alt-J_Fitzpleasure.mp3", Song.Category.POP_ROCK);
            Song s14 = new Song("Mathilda", "Alt-J", "../../Resources/Alt-J_Matilda.mp3", Song.Category.POP_ROCK);


            playList.Add(s1.Id, s1);
            playList.Add(s2.Id, s2);
            playList.Add(s3.Id, s3);
            playList.Add(s4.Id, s4);
            playList.Add(s5.Id, s5);
            playList.Add(s6.Id, s6);
            playList.Add(s7.Id, s7);
            playList.Add(s8.Id, s8);
            playList.Add(s9.Id, s9);
            playList.Add(s10.Id, s10);
            playList.Add(s11.Id, s11);
            playList.Add(s12.Id, s12);
            playList.Add(s13.Id, s13);
            playList.Add(s14.Id, s14);

            _newBubble(s1);
            _newBubble(s2);
            _newBubble(s3);
            _newBubble(s4);
            _newBubble(s5);
            _newBubble(s6);
            _newBubble(s7);
            _newBubble(s8);
        }

        public void _newBubble(Song s)
        {
            bubble = new Bubble(s, this);

            bubblesList.Add(s.Id, bubble);
            test_bubble.Items.Add(bubble.ScatterItem);

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
            Application.Current.Dispatcher.Invoke(new Action(() => fadeAnimation(0.0f, 1.0f, 1.0f, bubble.ScatterItem, false)));
            startMoving(bubble.ScatterItem, s, bubble.Image);
            //});
            //t.Start();
        }

        public void deleteBubble(Song s)
        {
            var canvasList = playlistPanel.Children.OfType<Canvas>().ToList();
            foreach (Canvas c in canvasList)
            {
                if(c.Name.Equals(s.Id))
                    playlistPanel.Children.Remove(c);
            }
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

        private void startMoving(ScatterViewItem s, Song song, Image image)
    {
        Point newPoint;
        newPoint = GetRandomPoint();

        //MoveTo(test_bubble,newPoint.X, newPoint.Y);
        Application.Current.Dispatcher.Invoke(new Action(() => MoveTo(s, newPoint.X, newPoint.Y, 7.0f, false, song, image)));
        //startMoving();
    }

        public void MoveTo(ScatterViewItem target, double newX, double newY, float duration, bool stop, Song song, Image image)
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
                    startMoving(target, song, image);
                }
            };

            stb.Begin(this, true);

            //image.TouchDown += (sender, eventArgs) =>
            image.MouseDown += (sender, eventArgs) =>
            {
                stb.Stop(this);
                target.Center = target.ActualCenter;
                eventArgs.Handled = true;           
            };

            //image.TouchUp += (sender, eventArgs) =>
            image.MouseUp += (sender, eventArgs) =>
            {
                target.Center = target.ActualCenter;
                //target.Center = eventArgs.GetPosition(null);
                //startMoving(target);
                Application.Current.Dispatcher.Invoke(new Action(() => fadeAnimation(1.0f, 0.0f, 1.0f, target, true)));

                if (fileLecture.isEmpty())
                    player.LoadSong(song);               

                fileLecture.Add(song);        
               
                eventArgs.Handled = true;
            };

            target.Center = target.ActualCenter;
        }

        private void fadeAnimation(float from, float to, float duration, ScatterViewItem target, Boolean suppress)
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

            if (suppress)
            {
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

                    Song song = playList.getSongById(target.Name);
                    canvas.Name = song.Id;

                    Image image = new Image();
                    image.Width = 200;
                    image.Height = 200;
                    image.Source = new BitmapImage(
                        new Uri("Resources/vinyl.png", UriKind.Relative));

                    canvas.Children.Add(image);

                    //<Border BorderBrush="{x:Null}" Height="50">
                    //    <TextBlock Text="Your text" VerticalAlignment="Center"/>
                    //</Border>

                    // Debut text block
                    TextBlock text = new TextBlock();
                   
                    text.Text = song.Name;
                    text.Foreground = Brushes.Black;
                    text.TextAlignment = TextAlignment.Center;
                    text.Width = canvas.Width;
                    //text.Background = Brushes.White;
                    //text.Opacity = 0.7;
                    //text.TextWrapping = TextWrapping.Wrap;
                    //text.Margin = new Thickness(40, 0, 40, 0);
                    //text.VerticalAlignment = VerticalAlignment.Center;

                    Canvas.SetLeft(text, 0);
                    Canvas.SetTop(text, 100);
                    canvas.Children.Add(text);
                    // fin text block

                    //image.TouchDown += (sender2, eventArgs) =>
                    /*image.MouseDown += (sender2, eventArgs) =>
                    {

                    };*/

                    //image.TouchUp += (sender2, eventArgs) =>
                    image.MouseUp += (sender2, eventArgs) =>
                    {
                        //Mouse.Capture(null);
                        //captured = false;
                        //Mouse.OverrideCursor = Cursors.Hand;
                          
                        player.PlaySong(false, song, true);
                        fileLecture.Current_index = fileLecture.getIndexSong(song);

                        if (actualSongAnimation != null)
                            actualSongAnimation.Stop();

                        updateBubble();
                        
                        //TODO 
                        //playlistPanel.Children.
                    };
                     

                    //image.TouchDown += (sender2, eventArgs) =>
                    /*image.MouseDown += (sender2, eventArgs) =>
                    {
                        Image i = eventArgs.Source as Image;
                        Canvas c = (Canvas)i.Parent;
                        songDragged = playList.getSongById(c.Name);

                        source = (UIElement)i;
                        sourceSelected = source;
                        Mouse.Capture(source);
                        captured = true;
                        x_shape = Canvas.GetLeft(source);
                        x_canvas = eventArgs.GetPosition(main_stack).X;
                        y_shape = Canvas.GetTop(source);
                        y_canvas = eventArgs.GetPosition(main_stack).Y;

                        //DragDrop.DoDragDrop(i, "Song was Dragged!", DragDropEffects.Copy);
                    };*/

                    //image.TouchMove += (sender2, eventArgs) =>
                    /*image.MouseMove += (sender2, eventArgs) =>
                    {
                        if (captured)
                        {
                            Console.Write("MOVEON");
                            double x = eventArgs.GetPosition(main_stack).X;
                            double y = eventArgs.GetPosition(main_stack).Y;
                            x_shape += x - x_canvas;
                            Canvas.SetLeft(source, x_shape);
                            x_canvas = x;
                            y_shape += y - y_canvas;
                            Canvas.SetTop(source, y_shape);
                            y_canvas = y;
                            DragDrop.DoDragDrop(source, "Song was Dragged!", DragDropEffects.Copy);
                        }
                    };*/

                    image.GiveFeedback += (sender2, eventArgs) =>
                    {
                        if (eventArgs.Effects == DragDropEffects.Copy)
                        {
                            //Mouse.OverrideCursor = imageVinyl;
                            Mouse.OverrideCursor = null;
                            //Mouse.OverrideCursor = ((TextBlock)this.Resources["CursorVinyl"]).Cursor;
                        }
                        else
                            eventArgs.UseDefaultCursors = true;

                        eventArgs.Handled = true;
                    };

                    //canvas.SetTop(canvas, NewBody.YPosition);
                    //canvas.SetLeft(canvas, NewBody.XPosition);

                    playlistPanel.Children.Add(canvas);
                };
            }

            // Begin the storyboard
            storyboard.Begin(this, true);

            #endregion

        }

        // Drag target
        private void Song_Drop(object sender, DragEventArgs e)
        {
           // string draggedText = (string)e.Data.GetData(DataFormats.StringFormat);
            //Label l = e.Source as Label;
            //l.Content = draggedText;
            Console.WriteLine("drop");
            deleteBubble(songDragged);
            //songDragged = null;  
        }

        private void updateBubble()
        {
            /*foreach (Canvas c in playlistPanel.Children)
            {
                ((TextBlock)c.Children[1]).Foreground = Brushes.Navy;
                ((TextBlock)c.Children[1]).FontSize = 12;
            }*/

            Canvas c1 = (Canvas)playlistPanel.Children[fileLecture.Current_index];

            rotateCanvas(c1);

            //TextBlock text = (TextBlock)c1.Children[1];
            //text.Foreground = Brushes.DarkGray;
            //text.FontSize = 20;
        }

        public void rotateCanvas(Canvas c)
        {
            Image i = c.Children.OfType<Image>().First();
            TextBlock t = c.Children.OfType<TextBlock>().First();
            actualSongAnimation = new Storyboard();
            actualSongAnimation.Duration = new Duration(TimeSpan.FromSeconds(4.0));
            DoubleAnimation rotateAnimation = new DoubleAnimation()
            {
                From = 0,
                To = 360,
                Duration = actualSongAnimation.Duration
            };

            DoubleAnimation rotateText = new DoubleAnimation()
            {
                From = 0,
                To = 360,
                Duration = actualSongAnimation.Duration
            };

            Vector offset = VisualTreeHelper.GetOffset(i);
            i.RenderTransform = new RotateTransform(0, 100, 100);
            t.RenderTransform = new RotateTransform(0, 100, 0);
            Storyboard.SetTarget(rotateAnimation, i);
            Storyboard.SetTargetProperty(rotateAnimation, new PropertyPath("(UIElement.RenderTransform).(RotateTransform.Angle)"));
            Storyboard.SetTarget(rotateText, t);
            Storyboard.SetTargetProperty(rotateText, new PropertyPath("(UIElement.RenderTransform).(RotateTransform.Angle)"));

            actualSongAnimation.Children.Add(rotateAnimation);
            actualSongAnimation.Children.Add(rotateText);
            //s.FillBehavior = FillBehavior.Stop;
            actualSongAnimation.RepeatBehavior = RepeatBehavior.Forever;
            actualSongAnimation.Begin();
        }

        private Point GetRandomPoint()
        {
            var possibleX = new List<int> { 80, 1320, 80, 720};

            int x;
            int y;
            x = m_Random.Next(0, 4);
            if ((x == 0)||(x == 1))
            {
                x = possibleX[x];
                y = m_Random.Next(80, 720);
            }
            else
            {
                y = possibleX[x];
                x = m_Random.Next(80, 1320);
            }
            return new Point(x,y);
        }

        public Bubble getBubbleBySong(String id_song)
        {
            if (bubblesList.ContainsKey(id_song))
            {
                return bubblesList[id_song];
            }
            return null;
        }

        // This event occurs when a TagVisualization object is added to the visual tree.
        private void OnVisualizationAdded(object sender, TagVisualizerEventArgs e)
        {
            // Get a reference to the TagVisualization object.
            TagVisualization tv = e.TagVisualization;
            // Add a handler for the GotTag event.
            tv.GotTag += new RoutedEventHandler(OnGotTag);
            // Add a handler for the LostTag event.
            tv.LostTag += new RoutedEventHandler(OnLostTag);
            // Keep the original effect. This effect is needed to return to the original 
            // effect if the tag is removed and then replaced before LostTagTimeout.
            tv.Tag = tv.Effect;
        }

        DropShadowEffect dse = new DropShadowEffect();

        private void OnLostTag(object sender, RoutedEventArgs e)
        {
            // Get a reference to the TagVisualization object.
            TagVisualization tv = (TagVisualization)e.Source;

            // Create an animation to change the border size of the drop shadow effect.
            DoubleAnimation borderAnimation =
                new DoubleAnimation(42, 7, TimeSpan.FromMilliseconds(1600));

            // Set the color of the drop shadow to one of the SurfaceColors.
            dse.Color = SurfaceColors.ControlAccentColor;

            // Set any of the other drop shadow properties.
            dse.BlurRadius = 30;

            // Add the effect and start the animation.
            tv.Effect = dse;
            tv.Effect.BeginAnimation(DropShadowEffect.ShadowDepthProperty, borderAnimation);

            tv.Effect = (Effect)tv.Tag;

            switch (tv.VisualizedTag.Value)
            {
                case 0x38:
                    foreach (KeyValuePair<string, Bubble> entry in bubblesList)
                    {
                        // do something with entry.Value or entry.Key
                        Bubble b = entry.Value;
                        if (b.S._Category != Song.Category.ANNEES_70)
                        {
                            Application.Current.Dispatcher.Invoke(new Action(() => fadeAnimation(0.0f, 1.0f, 1.0f, b.ScatterItem, false)));
                        }
                    }
                    break;
                defaut:
                    break;
            }

            
            // At this point, the TagVisualization object has a drop shadow that is getting smaller,
            // while the interior is fading (because TagRemovedBehavior == TagRemovedBehavior.Fade).
            // If the tag is put back on the Microsoft Surface screen before LostTagTimeout, the OnGotTag event (above)
            // occurs, and the TagVisualization receives its original effect.
        }

        private void OnGotTag(object sender, RoutedEventArgs e)
        {
            // Get a reference to the TagVisualization object.
            TagVisualization tv = (TagVisualization)e.Source;
            // Apply the original effect.
            tv.Effect = (Effect)tv.Tag;

            switch (tv.VisualizedTag.Value)
            {
                case 0x38:
                    foreach (KeyValuePair<string, Bubble> entry in bubblesList)
                    {
                        // do something with entry.Value or entry.Key
                        Bubble b = entry.Value;
                        if (b.S._Category != Song.Category.ANNEES_70) {
                            Application.Current.Dispatcher.Invoke(new Action(() => fadeAnimation(1.0f, 0.0f, 1.0f, b.ScatterItem, false)));
                        }
                    }
                    break;
                defaut:
                    break;
            }
        }
    }

}