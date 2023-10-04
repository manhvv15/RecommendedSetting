using QuanLyChung365TruocDangNhap.ChamCong365.Entities;
using QuanLyChung365TruocDangNhap.ChamCong365.Entities.Company;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
    /// Interaction logic for CauHinhChamCong_QR_Popup.xaml
    /// </summary>
    public partial class CauHinhChamCong_QR_Popup : UserControl
    {
        public CauHinhChamCong_QR_Popup(MainWindow main, ListCor qr)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            QR = qr;
        }
        public MainWindow Main { get; set; }
        public ListCor QR { get; set; }
        public Action DelSuccess
        {
            get { return (Action)GetValue(DelSuccessProperty); }
            set { SetValue(DelSuccessProperty, value); }
        }
        public static readonly DependencyProperty DelSuccessProperty =
            DependencyProperty.Register("DelSuccess", typeof(Action), typeof(CauHinhChamCong_QR_Popup));
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //var y = Mouse.GetPosition(Main).Y;
            //var x = Mouse.GetPosition(Main).X;
            //var right = Main.ActualWidth - x - (this.ActualWidth / 2);
            //if (right >= 20) this.Margin = new Thickness(0, y, right, 0);
            //else this.Margin = new Thickness(0, y, 20, 0);

            var y = Mouse.GetPosition(Main).Y;
            var x = Mouse.GetPosition(Main).X;
            if (Main.ActualHeight - y < this.ActualHeight)
            {
                this.Margin = new Thickness(0, y - this.ActualHeight, Main.ActualWidth - x, 0);
            }
            else this.Margin = new Thickness(0, y, Main.ActualWidth - x, 0);
        }
        private void InfoQR(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=f30f0b61e761b8926941f232ea7cccb9." + Main.APICompany.data.data.user_info.com_id + "." + Main.APICompany.data.data.user_info.com_pass + "&link=https://chamcong.timviec365.vn/quan-ly-cong-ty/chi-tiet-qr.html?cor_id=" + QR.cor_id);
            Main.ClosePopup();
        }
        private async Task delQR()
        {
            try
            {

                Dictionary<string, string> form = new Dictionary<string, string>();
                form.Add("cor_id", QR.cor_id);
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
                var respon = await httpClient.PostAsync("http://210.245.108.202:3000/api/qlc/trackingQR/delete", new FormUrlEncodedContent(form));
                API_Respon add = JsonConvert.DeserializeObject<API_Respon>(respon.Content.ReadAsStringAsync().Result);
                if (add.data != null && add.data.result)
                {
                    var n = new PopUp.Notify1();
                    n.Type = PopUp.Notify1.NotifyType.Success;
                    n.setMessage("Bạn đã xóa QR thành công !");
                    Main.ShowPopup(n);
                    if (DelSuccess != null) DelSuccess();
                }
                else
                {
                    var n = new PopUp.Notify1();
                    n.Type = PopUp.Notify1.NotifyType.Error;
                    n.setMessage("Bạn đã xóa QR thất bại, vui lòng thử lại sau!");
                    Main.ShowPopup(n);
                }
            }
            catch (Exception ex)
            {

                var n = new PopUp.Notify1();
                n.Type = PopUp.Notify1.NotifyType.Error;
                n.setMessage(ex.Message);
                Main.ShowPopup(n);
            }
        }
        private async void DeleteQR(object sender, MouseButtonEventArgs e)
        {
            var c = new PopUp.ConfirmBox();
            c.ConfirmTitle = "Xóa mã QR";
            c.Message = "Bạn có chắc chắn muốn xóa mã QR không?";
            Main.ClosePopup();
            Main.ShowPopup(c);
            c.Accept = () =>
            {
                delQR();
            };
        }
    }
}
