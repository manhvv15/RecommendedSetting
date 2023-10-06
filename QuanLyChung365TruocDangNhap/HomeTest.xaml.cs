using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace QuanLyChung365TruocDangNhap
{
    /// <summary>
    /// Interaction logic for HomeTest.xaml
    /// </summary>
    public partial class HomeTest : Window, INotifyPropertyChanged
    {
        protected virtual void OnPropertyChanged(string newName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(newName));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public HomeTest()
        {
            InitializeComponent();
        }
        private int _sideBarStatus;

        public int sideBarStatus
        {
            get { return _sideBarStatus; }
            set { _sideBarStatus = value; OnPropertyChanged("sideBarStatus"); }
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            sideBarStatus = sideBarStatus == 0 ? 1 : 0;
            /*if(Gr.Width != 60)
            {
                Gr.Width = 60;
                tb.Visibility = Visibility.Collapsed;
            }
            else
            {
                Gr.Width = 450;
                tb.Visibility = Visibility.Visible;
            }*/
        }
    }
}
