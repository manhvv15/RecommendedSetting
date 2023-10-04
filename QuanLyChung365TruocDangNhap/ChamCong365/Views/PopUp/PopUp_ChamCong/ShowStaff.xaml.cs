using QuanLyChung365TruocDangNhap.ChamCong365.Entities.History_Attenden_Staff;
using QuanLyChung365TruocDangNhap.ChamCong365.Views.Pages.ChamCong;
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

namespace QuanLyChung365TruocDangNhap.ChamCong365.Views.PopUp.PopUp_ChamCong
{
    /// <summary>
    /// Interaction logic for ShowStaff.xaml
    /// </summary>
    public partial class ShowStaff : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ShowStaff(ChamCong_Main main, string epid)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            getNV(epid).ContinueWith(tt => this.Dispatcher.Invoke(() =>
            {
                if (tt.Result != null)
                {
                    if (tt.Result.data.items.Count > 0) ep_name = tt.Result.data.items[0].ep_name;
                    else
                    {
                        var x = new Error_NhanDien(main);
                        x.Message = "Không nhận diện được khuôn mặt!";
                        x.Message2 = "Bạn liên hệ với nhân sự công ty để cập nhật lại khuôn mặt";
                        main.Popup.NavigationService.Navigate(x);
                        main.PopupChamCong.Visibility = Visibility.Visible;
                    }
                }
            }));
        }
        private ChamCong_Main Main;

        private string _ep_name;
        public string ep_name
        {
            get { return _ep_name; }
            set { _ep_name = value; OnPropertyChanged(); }
        }

        public Action ChamCong
        {
            get { return (Action)GetValue(ChamCongProperty); }
            set { SetValue(ChamCongProperty, value); }
        }
        public static readonly DependencyProperty ChamCongProperty =
            DependencyProperty.Register("ChamCong", typeof(Action), typeof(ShowStaff));

        public Action ChamLai
        {
            get { return (Action)GetValue(ChamLaiProperty); }
            set { SetValue(ChamLaiProperty, value); }
        }
        public static readonly DependencyProperty ChamLaiProperty =
            DependencyProperty.Register("ChamLai", typeof(Action), typeof(ShowStaff));
        //
        private async Task<API_List_Staff_All> getNV(string ep = "")
        {
            try
            {
                HttpClient http = new HttpClient();
                var apilink = "http://210.245.108.202:3000/api/qlc/managerUser/list";
                var pram = new List<string>();
                Dictionary<string, string> form = new Dictionary<string, string>();
                http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Main.Main.APICompany.data.data.access_token);
                form.Add("com_id", Main.Main.APICompany.data.data.user_info.com_id);
                form.Add("ep_id", ep);
                HttpResponseMessage respon = await http.PostAsync(apilink, new FormUrlEncodedContent(form));
                API_List_Staff_All api = JsonConvert.DeserializeObject<API_List_Staff_All>(respon.Content.ReadAsStringAsync().Result);
                return api;
            }
            catch (Exception)
            {

                return null;
            }
        }
        private void ChamCong_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ChamCong != null) ChamCong();
        }

        private void ChamLai_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ChamLai != null)
            {
                this.Visibility = Visibility.Collapsed;
                ChamLai();
            }

        }
    }
}
