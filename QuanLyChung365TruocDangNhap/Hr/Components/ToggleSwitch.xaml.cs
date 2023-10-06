using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuanLyChung365TruocDangNhap.Hr.Components
{
    /// <summary>
    /// Interaction logic for ToggleSwitch.xaml
    /// </summary>
    public partial class ToggleSwitch : UserControl
    {

        //public string  Abcd
        //{
        //    get { return (string)GetValue(TitleProperty); }
        //    set { SetValue(TitleProperty, value); }
        //}

        //public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Abcd", typeof(string), typeof(ToggleSwitch), new PropertyMetadata(String.Empty));

        private bool status = false;

        public ToggleSwitch()
        {
            InitializeComponent();
            //SolidColorBrush solidColorBrush = new SolidColorBrush();
            //solidColorBrush.Color = (Color)ColorConverter.ConvertFromString("#1A73E8");
            //ellipse.Background = solidColorBrush;
            //this.RegisterName("solidColorBrush", solidColorBrush);
            //ellipse.Margin = new Thickness(13, 0, 0, 0);
        }


        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!status)
            {
                SwitchOn();
            }
            else
            {
                SwitchOff();
            }

            status = !status;

        }

        private void SwitchOff()
        {
            ThicknessAnimation thicknessAnimation = new ThicknessAnimation();
            Storyboard storyboard = new Storyboard();
            thicknessAnimation = new ThicknessAnimation();
            thicknessAnimation.From = ellipse.Margin;
            thicknessAnimation.To = new Thickness(0, 0, 0, 0);
            thicknessAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.3));
            storyboard.Children.Add(thicknessAnimation);
            Storyboard.SetTargetProperty(thicknessAnimation, new PropertyPath(Ellipse.MarginProperty));
            Storyboard.SetTargetName(thicknessAnimation, ellipse.Name);

            //ColorAnimation mouseEnterColorAnimation = new ColorAnimation();
            //mouseEnterColorAnimation.To = (Color)ColorConverter.ConvertFromString("#FFFFFF");
            //mouseEnterColorAnimation.Duration = TimeSpan.FromSeconds(1);
            //Storyboard.SetTargetName(mouseEnterColorAnimation, "solidColorBrush");
            //Storyboard.SetTargetProperty(
            //    mouseEnterColorAnimation, new PropertyPath(SolidColorBrush.ColorProperty));

            //storyboard.Children.Add(mouseEnterColorAnimation);

            storyboard.Begin(this);
            var converter = new System.Windows.Media.BrushConverter();
            ellipse.Fill = (Brush)converter.ConvertFromString("#FFFFFF");
            rectangle.Background = (Brush)converter.ConvertFromString("#CCCCCC");

        }

        private void SwitchOn()
        {
            ThicknessAnimation thicknessAnimation = new ThicknessAnimation();
            Storyboard storyboard = new Storyboard();
            thicknessAnimation = new ThicknessAnimation();
            thicknessAnimation.From = ellipse.Margin;
            thicknessAnimation.To = new Thickness(13, 0, 0, 0);
            thicknessAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.3));
            storyboard.Children.Add(thicknessAnimation);
            Storyboard.SetTargetProperty(thicknessAnimation, new PropertyPath(Ellipse.MarginProperty));
            Storyboard.SetTargetName(thicknessAnimation, ellipse.Name);
            storyboard.Begin(this);
            var converter = new System.Windows.Media.BrushConverter();
            ellipse.Fill = (Brush)converter.ConvertFromString("#1A73E8");
            rectangle.Background = (Brush)converter.ConvertFromString("#8DB9F4");
        }
    }
}
