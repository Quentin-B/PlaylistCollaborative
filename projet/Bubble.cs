using Microsoft.Surface.Presentation.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ProjetSurface
{
    public class Bubble
    {
        private ScatterViewItem scatterItem;
        private int defaultSize;
        private int nbLikes;
        private Song s;

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

        public Bubble(Song s)
        {
            this.s = s;

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
            text.Foreground = new SolidColorBrush(Colors.White);
            text.TextAlignment = TextAlignment.Center;
            text.TextWrapping = TextWrapping.Wrap;
            text.Margin = new Thickness(40, 0, 40, 0);
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
            int new_size = defaultSize + (30 * nbLikes);

            scatterItem.Width = new_size;
            scatterItem.Height = new_size;

            canvas.Width = new_size;
            canvas.Height = new_size;

            image.Width = canvas.Width;
            image.Height = canvas.Height;

            text.FontSize += (2 * nbLikes); 
        }
    }
}
