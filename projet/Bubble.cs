using Microsoft.Surface.Presentation.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProjetSurface
{
    public class Bubble
    {
        private ScatterViewItem scatterItem;
        private int defaultSize;
        private int nbLikes;
        private Song s;
        private SurfaceWindow surface;

        public Song S
        {
            get { return s; }
            set { s = value; }
        }

        public int DefaultSize
        {
            get { return defaultSize; }
            set { defaultSize = value; }
        }

        public ScatterViewItem ScatterItem
        {
            get { return scatterItem; }
            set { scatterItem = value; }
        }
        private Canvas canvas;

        public Canvas Canvas
        {
            get { return canvas; }
            set { canvas = value; }
        }
        private Image image;

        public Image Image
        {
            get { return image; }
            set { image = value; }
        }
        private TextBlock text;

        public TextBlock Text
        {
            get { return text; }
            set { text = value; }
        }

        public Bubble(Song s, SurfaceWindow surface)
        {
            this.s = s;
            this.surface = surface;

            nbLikes = 0;
            defaultSize = 200;

            scatterItem = new ScatterViewItem();
            scatterItem.Width = defaultSize;
            scatterItem.Height = defaultSize;
            //item.Content = "Object 2";
            scatterItem.Orientation = 0;
            scatterItem.CanScale = false;
            scatterItem.CanMove = true;
            scatterItem.Center = new Point(300, 100);
            //item.ActualCenter = new Point(300, 100);
            scatterItem.Background = new SolidColorBrush(Colors.Transparent);
            scatterItem.ShowsActivationEffects = false;
            scatterItem.Name = s.Id;
            
            canvas = new Canvas();
            canvas.Width = defaultSize;
            canvas.Height = defaultSize;

            image = new Image();
            image.Width = canvas.Width;
            image.Height = canvas.Height;
            image.Source = new BitmapImage(
                new Uri("Resources/bubble.png", UriKind.Relative));

            canvas.Children.Add(image);

            text = new TextBlock();
            text.Text = s.Name;
            text.Background = Brushes.Transparent; 

            text.Foreground = Brushes.White;
            text.TextAlignment = TextAlignment.Center;
            text.Width = canvas.Width;
            text.IsEnabled = false;

            Canvas.SetLeft(text, 0);
            Canvas.SetTop(text, 100);

            canvas.Children.Add(text);

            //DoubleAnimation da = new DoubleAnimation();
            //da.From = 0;
            //da.To = 360;
            //da.Duration = new Duration(TimeSpan.FromSeconds(5));
            //da.RepeatBehavior = RepeatBehavior.Forever;
            //rotateTransform.BeginAnimation(RotateTransform.AngleProperty, da);

            scatterItem.Content = canvas;   
        }

        public void like()
        {
            nbLikes++;
            //int new_size = defaultSize + (30 * nbLikes);
            int new_size = defaultSize + (100 * nbLikes);

            /*scatterItem.Width = new_size;
            scatterItem.Height = new_size;

            canvas.Width = new_size;
            canvas.Height = new_size;

            image.Width = canvas.Width;
            image.Height = canvas.Height;

            text.FontSize += (2 * nbLikes);*/

            DoubleAnimation widthAnimation = new DoubleAnimation();
            widthAnimation.From = image.Width;
            widthAnimation.To = 800;
            widthAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));

            DoubleAnimation heightAnimation = new DoubleAnimation();
            widthAnimation.From = image.Height;
            widthAnimation.To = 800;
            Console.WriteLine("new size2" + new_size);
            widthAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));

            DoubleAnimation policeAnimation = new DoubleAnimation();
            policeAnimation.From = text.FontSize;
            policeAnimation.To = text.FontSize + (2 * nbLikes);
            Console.WriteLine("new size3" + new_size);
            policeAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));

            /*Storyboard.SetTarget(widthAnimation, canvas);
            Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(Canvas.WidthProperty));
            Storyboard.SetTarget(heightAnimation, canvas);
            Storyboard.SetTargetProperty(heightAnimation, new PropertyPath(Canvas.HeightProperty));

            Storyboard.SetTarget(widthAnimation, scatterItem);
            Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(ScatterViewItem.WidthProperty));
            Storyboard.SetTarget(heightAnimation, scatterItem);
            Storyboard.SetTargetProperty(heightAnimation, new PropertyPath(ScatterViewItem.HeightProperty));*/

            /*Storyboard.SetTarget(widthAnimation, image);
            Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(Image.WidthProperty));
            Storyboard.SetTarget(heightAnimation, image);
            Storyboard.SetTargetProperty(heightAnimation, new PropertyPath(Image.HeightProperty));
            Storyboard.SetTarget(policeAnimation, text);
            Storyboard.SetTargetProperty(policeAnimation,new PropertyPath(TextBlock.FontSizeProperty));*/

            ScaleTransform scale = new ScaleTransform(1.0, 1.0);
            image.RenderTransformOrigin = new Point(0.5, 0.5);
            image.RenderTransform = scale;

            DoubleAnimation growAnimation = new DoubleAnimation();
            growAnimation.Duration = TimeSpan.FromMilliseconds(300);
            growAnimation.From = 1;
            growAnimation.To = 1.8;

            Storyboard.SetTargetProperty(growAnimation, new PropertyPath("RenderTransform.ScaleX"));
            Storyboard.SetTarget(growAnimation, image);

            Storyboard s = new Storyboard();
            s.FillBehavior = FillBehavior.Stop;
            s.Children.Add(growAnimation);
            //s.Children.Add(widthAnimation);
            //s.Children.Add(heightAnimation);
            //s.Children.Add(policeAnimation);

            s.Completed += delegate(object sender, EventArgs e)
            {
                scatterItem.Width = new_size;
                scatterItem.Height = new_size;

                canvas.Width = new_size;
                canvas.Height = new_size;

                image.Width = new_size;
                image.Height = new_size;

                text.FontSize += (2 * nbLikes);
                text.TextAlignment = TextAlignment.Center;
                text.Width = canvas.Width;

                Canvas.SetLeft(text, 0);
                Canvas.SetTop(text, new_size/2);
            };

            s.Begin();
        }
    }
}
