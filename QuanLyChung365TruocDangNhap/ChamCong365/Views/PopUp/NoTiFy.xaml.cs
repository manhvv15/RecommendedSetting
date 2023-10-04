using QuanLyChung365TruocDangNhap.ChamCong365.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuanLyChung365TruocDangNhap.ChamCong365.Views.PopUp
{
    /// <summary>
    /// Interaction logic for NoTiFy.xaml
    /// </summary>
    public partial class NoTiFy : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public NoTiFy(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            ShowNoti = Main.ListNoti;
        }
        private int pageNoti = 1;
        private int totalNoti = 0;
        public class ItemsList
        {
            public string Name { get; set; }
            public string Time { get; set; }
        }
        private List<ItemsList> _ListItems;
        public List<ItemsList> ListItems
        {
            get { return _ListItems; }
            set
            {
                _ListItems = value;
                OnPropertyChanged();
            }
        }

        private List<Item_NotifyCom> _ShowNoti;

        public List<Item_NotifyCom> ShowNoti
        {
            get { return _ShowNoti; }
            set { _ShowNoti = value; OnPropertyChanged(); }
        }

        private MainWindow Main;

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var y = Mouse.GetPosition(Main).Y;
            var x = Mouse.GetPosition(Main).X;
            this.Margin = new Thickness(0, y, Main.ActualWidth - x, 0);
        }
        private T GetFirstChildOfType<T>(DependencyObject dependencyObject) where T : DependencyObject
        {
            if (dependencyObject == null)
            {
                return null;
            }

            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(dependencyObject); i++)
            {
                var child = VisualTreeHelper.GetChild(dependencyObject, i);

                var result = (child as T) ?? GetFirstChildOfType<T>(child);

                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        private void ListView_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var lv = sender as ListView;
            var scroll = GetFirstChildOfType<ScrollViewer>(lv);
            if (scroll != null)
            {
                if (scroll.ScrollableHeight > 0 && scroll.ScrollableHeight == scroll.VerticalOffset)
                {
                    Main.NotiPage++;
                    Main.loadNoti(Main.NotiPage).ContinueWith(tt => this.Dispatcher.Invoke(() =>
                    {
                        ShowNoti = Main.ListNoti;
                    }));

                }
            }
        }
    }
}
