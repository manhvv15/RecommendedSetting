using QuanLyChung365TruocDangNhap.ChamCong365.Entities;
using QuanLyChung365TruocDangNhap.ChamCong365.Entities.Company;
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

namespace QuanLyChung365TruocDangNhap.ChamCong365.Views.Pages.CauHinhChamCong
{
    /// <summary>
    /// Interaction logic for CauHinhChamCong_Wifi_EditPopup.xaml
    /// </summary>
    public partial class CauHinhChamCong_Wifi_EditPopup : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public CauHinhChamCong_Wifi_EditPopup(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            Type = EditTypes.add;
        }
        public CauHinhChamCong_Wifi_EditPopup(MainWindow main,Wifi w)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            Info = w;
            Type = EditTypes.edit;
            txtTen.Text = w.name_wifi;
            txtMac.Text = w.mac_address;
        }
        //
        public enum EditTypes { add,edit }
        private EditTypes _Type;
        public EditTypes Type
        {
            get { return _Type; }
            set { _Type = value;OnPropertyChanged(); }
        }
        public Wifi Info { get; set; }
        public MainWindow Main { get; set; }
        public Action Success
        {
            get { return (Action)GetValue(SuccessProperty); }
            set { SetValue(SuccessProperty, value); }
        }
        public static readonly DependencyProperty SuccessProperty =
            DependencyProperty.Register("Success", typeof(Action), typeof(CauHinhChamCong_Wifi_EditPopup));
        //
        private void ClosePopup(object sender, MouseButtonEventArgs e)
        {
            Main.ClosePopup();
        }
        private async void UpdatePopup(object sender, MouseButtonEventArgs e)
        {
            var allow = true;
            tblValidateTen.Text = tblValidateMac.Text = "";
            if (string.IsNullOrEmpty(txtTen.Text))
            {
                allow = false;
                tblValidateTen.Text = App.Current.Resources["text_Validate_Trong"] as string;
            }
            if (string.IsNullOrEmpty(txtMac.Text))
            {
                allow = false;
                tblValidateMac.Text = App.Current.Resources["text_Validate_Trong"] as string;
            }
            if (allow)
            {
                try
                {
                    Dictionary<string, string> form = new Dictionary<string, string>();
                    HttpClient httpClient = new HttpClient();
                    switch (Main.Type)
                    {
                        case 1:
                            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Main.APIStaff.data.data.access_token);
                            break;
                        case 2:
                            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Main.APICompany.data.data.access_token);
                            break;
                        default:
                            break;
                    }
                    switch (Type)
                    {
                        case EditTypes.add:
                            form.Add("name_wifi", txtTen.Text);
                            form.Add("mac_address", txtMac.Text);
                            form.Add("ip_address", "");

                            var respon = await httpClient.PostAsync("http://210.245.108.202:3000/api/qlc/TrackingWifi/create", new FormUrlEncodedContent(form));
                            API_Respon add = JsonConvert.DeserializeObject<API_Respon>(respon.Content.ReadAsStringAsync().Result);
                            if (add.data != null && add.data.result)
                            {
                                var x = new PopUp.Notify1();
                                x.Type = PopUp.Notify1.NotifyType.Success;
                                x.Message = "Thêm mới wifi thành công";
                                Main.ShowPopup(x);
                                if (Success != null) Success();
                            }
                            else
                            {
                                var mess = "";
                                if (add.error != null) mess = add.error.message;
                                else mess = "Thêm mới wifi thất bại, vui lòng thử lại sau";

                                var x = new PopUp.Notify1();
                                x.Type = PopUp.Notify1.NotifyType.Error;
                                x.Message = mess;
                                Main.ShowPopup(x);
                            }
                            break;
                        case EditTypes.edit:
                            form.Add("wifi_id", Info.wifi_id);
                            form.Add("name_wifi", txtTen.Text);
                            form.Add("mac_address", txtMac.Text);
                            form.Add("ip_address", "");

                            respon = await httpClient.PostAsync("http://210.245.108.202:3000/api/qlc/TrackingWifi/edit", new FormUrlEncodedContent(form));
                            API_Respon edit = JsonConvert.DeserializeObject<API_Respon>(respon.Content.ReadAsStringAsync().Result);
                            if (edit.data != null && edit.data.result)
                            {
                                Info.name_wifi = txtTen.Text;
                                Info.mac_address = txtMac.Text;

                                var x = new PopUp.Notify1();
                                x.Type = PopUp.Notify1.NotifyType.Success;
                                x.Message = "Cập nhật wifi thành công";
                                Main.ShowPopup(x);

                                if (Success != null) Success();
                            }
                            else
                            {
                                var mess = "";
                                if (edit.error != null) mess= edit.error.message;
                                else mess= "Cập nhật wifi thất bại, vui lòng thử lại sau";
                                var x = new PopUp.Notify1();
                                x.Type = PopUp.Notify1.NotifyType.Error;
                                x.Message = mess;
                                Main.ShowPopup(x);
                            }
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {

                    var x = new PopUp.Notify1();
                    x.Type = PopUp.Notify1.NotifyType.Error;
                    x.Message = ex.Message;
                    Main.ShowPopup(x);
                }
                Main.ClosePopup();
            }
        }
    }
}
