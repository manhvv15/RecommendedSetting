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

namespace QuanLyChung365TruocDangNhap.ChamCong365.Views.Pages.CauHinhChamCong
{
    /// <summary>
    /// Interaction logic for CauHinhChamCong_Wifi_EditPopup.xaml
    /// </summary>
    public partial class CauHinhChamCong_IP_EditPopup : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public CauHinhChamCong_IP_EditPopup(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            Type = EditTypes.add;
        }
        public CauHinhChamCong_IP_EditPopup(MainWindow main, IP ip)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            Info = ip;
            Type = EditTypes.edit;

            txtTen.Text = ip.name_ip;
            txtIP.Text = ip.ip_address;
        }
        //
        public enum EditTypes { add,edit }
        private EditTypes _Type;
        public EditTypes Type
        {
            get { return _Type; }
            set { _Type = value;OnPropertyChanged(); }
        }

        public int IpType { get; set; }
        public IP Info { get; set; }
        public MainWindow Main { get; set; }

        public Action Success
        {
            get { return (Action)GetValue(SuccessProperty); }
            set { SetValue(SuccessProperty, value); }
        }
        public static readonly DependencyProperty SuccessProperty =
            DependencyProperty.Register("Success", typeof(Action), typeof(CauHinhChamCong_IP_EditPopup));
        //
        private void ClosePopup(object sender, MouseButtonEventArgs e)
        {
            Main.ClosePopup();
        }
        private async void UpdatePopup(object sender, MouseButtonEventArgs e)
        {
            var allow = true;
            tblValidateTen.Text = tblValidateIp.Text = "";
            if (string.IsNullOrEmpty(txtTen.Text))
            {
                allow = false;
                tblValidateTen.Text = App.Current.Resources["text_Validate_Trong"] as string;
            }
            if (string.IsNullOrEmpty(txtIP.Text))
            {
                allow = false;
                tblValidateIp.Text = App.Current.Resources["text_Validate_Trong"] as string;
            }
            if (allow)
            {
                try
                {
                    Dictionary<string, string> form = new Dictionary<string, string>();
                    HttpClient httpClient = new HttpClient();
                    HttpResponseMessage respon;
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
                            form.Add("ip_name", txtTen.Text);
                            form.Add("ip_address", txtIP.Text);
                            form.Add("ip_type", IpType.ToString());
                            respon = await httpClient.PostAsync("https://api.timviec365.vn/api/qlc/company_web_ip/add", new FormUrlEncodedContent(form));
                            var check = respon.Content.ReadAsStringAsync().Result;
                            API_Respon add = JsonConvert.DeserializeObject<API_Respon>(respon.Content.ReadAsStringAsync().Result);
                            if (add.data != null && add.data.result)
                            {
                                var x = new PopUp.Notify1();
                                x.Type = PopUp.Notify1.NotifyType.Success;
                                x.Message = "Thêm IP mạng thành công";
                                Main.ShowPopup(x);
                                if (Success != null) Success();
                            }
                            else
                            {
                                var x = new PopUp.Notify1();
                                x.Type = PopUp.Notify1.NotifyType.Error;
                                x.Message = "Thêm IP mạng thất bại, vui lòng thử lại sau";
                                Main.ShowPopup(x);
                            }
                            break;
                        case EditTypes.edit:
                            form.Add("ip_id", Info.ip_id);
                            form.Add("ip_name", txtTen.Text);
                            form.Add("ip_address", txtIP.Text);
                            respon = await httpClient.PostAsync("https://api.timviec365.vn/api/qlc/company_web_ip/edit", new FormUrlEncodedContent(form));
                            API_Respon edit = JsonConvert.DeserializeObject<API_Respon>(respon.Content.ReadAsStringAsync().Result);
                            if (edit.data != null && edit.data.result)
                            {
                                Info.name_ip = txtTen.Text;
                                Info.ip_address = txtIP.Text;
                                var x = new PopUp.Notify1();
                                x.Type = PopUp.Notify1.NotifyType.Success;
                                x.Message = "Cập nhật IP mạng thành công";
                                Main.ShowPopup(x);
                                if (Success != null) Success();
                            }
                            else
                            {
                                var x = new PopUp.Notify1();
                                x.Type = PopUp.Notify1.NotifyType.Error;
                                x.Message = "Cập nhật IP mạng thất bại, vui lòng thử lại sau";
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

            }
        }
    }
}
