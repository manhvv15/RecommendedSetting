using QuanLyChung365TruocDangNhap.ThietLapCongTy.Popups.PopupSoDoToChuc;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace QuanLyChung365TruocDangNhap.ThietLapCongTy.CoCau_ViTri_ToChuc
{
    /// <summary>
    /// Interaction logic for ucSoDoToChuc.xaml
    /// </summary>
    public partial class ucSoDoToChuc : UserControl
    {
        BrushConverter br = new BrushConverter();
        frmMain Main;
        public ucSoDoToChuc(frmMain main)
        {
            InitializeComponent();
            Main = main;
            #region Fake
            DrawShape("Root", 400, 50);

            // Vẽ các hình dạng con và kết nối chúng với gốc
            DrawShape("Child 1", 300, 150);
            ConnectShapes("Root", "Child 1");

            DrawShape("Child 2", 500, 150);
            ConnectShapes("Root", "Child 2");
           
            #endregion
        }
        #region Fake

        private void DrawShape(string text, double left, double top)
        {
            var shape = new Ellipse
            {
                Width = 100,
                Height = 50,
                Fill = Brushes.LightBlue,
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };

            var label = new Label
            {
                Content = text,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            var canvas = new Canvas();
            canvas.Children.Add(shape);
            canvas.Children.Add(label);

            Canvas.SetLeft(canvas, left);
            Canvas.SetTop(canvas, top);

            mindMapCanvas.Children.Add(canvas);
        }

        private void ConnectShapes(string parentText, string childText)
        {
            var parent = FindShape(parentText);
            var child = FindShape(childText);

            if (parent != null && child != null)
            {
                double parentCenterX = Canvas.GetLeft(parent) + parent.Width / 2;
                double parentCenterY = Canvas.GetTop(parent) + parent.Height / 2;
                double childCenterX = Canvas.GetLeft(child) + child.Width / 2;
                double childCenterY = Canvas.GetTop(child) + child.Height / 2;

                if (!double.IsNaN(parentCenterX) && !double.IsNaN(parentCenterY) &&
                    !double.IsNaN(childCenterX) && !double.IsNaN(childCenterY))
                {
                    var line = new Line
                    {
                        X1 = parentCenterX,
                        Y1 = parentCenterY,
                        X2 = childCenterX,
                        Y2 = childCenterY,
                        Stroke = Brushes.Black,
                        StrokeThickness = 20
                    };

                    mindMapCanvas.Children.Add(line);
                }
            }
        }

        private FrameworkElement FindShape(string text)
        {
            foreach (var element in mindMapCanvas.Children)
            {
                if (element is Canvas canvas)
                {
                    var label = canvas.Children.OfType<Label>().FirstOrDefault();
                    if (label != null && label.Content.ToString() == text)
                    {
                        return canvas;
                    }
                }
            }

            return null;
        }
        #endregion

        #region Hover Event
        private void bod_ThemMoiToChuc_MouseEnter(object sender, MouseEventArgs e)
        {
            bod_ThemMoiToChuc.BorderThickness = new Thickness(1);
            bod_ThemMoiToChuc.Background = (Brush)br.ConvertFrom("#339DFA");
           
        }

        private void bod_ThemMoiToChuc_MouseLeave(object sender, MouseEventArgs e)
        {
            bod_ThemMoiToChuc.BorderThickness = new Thickness(0);
            bod_ThemMoiToChuc.Background = (Brush)br.ConvertFrom("#4C5BD4");
        }
        #endregion

        private void bod_ThemMoiToChuc_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Main.pnlShowPopUp.Children.Add(new ucThemToChuc());
        }
    }
}
