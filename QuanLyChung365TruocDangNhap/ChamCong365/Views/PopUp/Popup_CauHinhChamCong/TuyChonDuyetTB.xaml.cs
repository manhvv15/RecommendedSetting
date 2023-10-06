using QuanLyChung365TruocDangNhap.ChamCong365.Entities.Company;
using QuanLyChung365TruocDangNhap.ChamCong365.Views.Pages.CauHinhChamCong;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

namespace QuanLyChung365TruocDangNhap.ChamCong365.Views.PopUp.Popup_CauHinhChamCong
{
    /// <summary>
    /// Interaction logic for TuyChonDuyetTB.xaml
    /// </summary>
    public partial class TuyChonDuyetTB : UserControl
    {
        public TuyChonDuyetTB(MainWindow main,CauHinhChamCong_Main ch, ThietBiChoDuyet data)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            DataPop = ch;
            Data = data;
        }
        CauHinhChamCong_Main DataPop;
        ThietBiChoDuyet Data;
        MainWindow Main;
        private void Duyet(object sender, MouseButtonEventArgs e)
        {
            using (WebClient http = new WebClient())
            {
                var apilink = "http://210.245.108.202:3000/api/qlc/checkdevice/add";
                var pram = new List<string>();

                switch (Main.Type)
                {
                    case 1:
                        http.Headers.Add("Authorization", "Bearer " + Main.APIStaff.data.data.access_token);
                        break;
                    case 2:
                        http.Headers.Add("Authorization", "Bearer " + Main.APICompany.data.data.access_token);
                        break;
                    default:
                        break;
                }
                http.QueryString.Add("ed_id", Data.ed_id);
                //http.QueryString.Add("type", "1");
                http.UploadValuesCompleted += (sender1, e1) =>
                {
                    DataPop.popup.Visibility = Visibility.Visible;
                };
                http.UploadValuesTaskAsync(new Uri(apilink), http.QueryString);
            }
        }

        private void Xoa(object sender, MouseButtonEventArgs e)
        {
            using (WebClient http = new WebClient())
            {
                var apilink = "http://210.245.108.202:3000/api/qlc/checkdevice/delete";
                var pram = new List<string>();

                switch (Main.Type)
                {
                    case 1:
                        http.Headers.Add("Authorization", "Bearer " + Main.APIStaff.data.data.access_token);
                        break;
                    case 2:
                        http.Headers.Add("Authorization", "Bearer " + Main.APICompany.data.data.access_token);
                        break;
                    default:
                        break;
                }
                http.QueryString.Add("ed_id", Data.ed_id);
                http.UploadValuesCompleted += (sender1, e1) =>
                {
                    DataPop.popup2.Visibility = Visibility.Visible;
                    DataPop.listTBCD.Remove(Data);
                    DataPop.listTBCD = DataPop.listTBCD.ToList();
                };
                http.UploadValuesTaskAsync(new Uri(apilink), http.QueryString);
            }
        }
    }
}
