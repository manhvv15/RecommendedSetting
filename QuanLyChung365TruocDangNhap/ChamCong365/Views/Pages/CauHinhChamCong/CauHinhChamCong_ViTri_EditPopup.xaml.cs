using QuanLyChung365TruocDangNhap.ChamCong365.Entities;
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
    /// Interaction logic for CauHinhChamCong_ViTri_EditPopup.xaml
    /// </summary>
    public partial class CauHinhChamCong_ViTri_EditPopup : UserControl
    {
        public CauHinhChamCong_ViTri_EditPopup(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
        }
        //
        public MainWindow Main { get; set; }
        public Action Success
        {
            get { return (Action)GetValue(SuccessProperty); }
            set { SetValue(SuccessProperty, value); }
        }
        public static readonly DependencyProperty SuccessProperty =
            DependencyProperty.Register("Success", typeof(Action), typeof(CauHinhChamCong_ViTri_EditPopup));
        //
        private void ClosePopup(object sender, MouseButtonEventArgs e)
        {
            Main.ClosePopup();
        }
        private async void UpdatePopup(object sender, MouseButtonEventArgs e)
        {
            var allow = true;
            tblValidateDiaChi.Text = tblValidateBanKinh.Text = "";
            if (string.IsNullOrEmpty(txtDiaChi.Text))
            {
                allow = false;
                tblValidateDiaChi.Text = App.Current.Resources["text_Validate_Trong"] as string;
            }
            if (string.IsNullOrEmpty(txtBanKinh.Text))
            {
                allow = false;
                tblValidateBanKinh.Text = App.Current.Resources["text_Validate_Trong"] as string;
            }
            //var lat = mapView.Position.Lat;
            //var lng = mapView.Position.Lng;
            //if (string.IsNullOrEmpty(lat.ToString()) && string.IsNullOrEmpty(lng.ToString()))
            //{
            //    allow = false;
            //    tblValidateDiaChi.Text = "Error";
            //}
            if (allow)
            {


                //try
                //{
                //    HttpClient httpClient = new HttpClient();
                //    Dictionary<string, string> form = new Dictionary<string, string>();

                //    form.Add("cor_location_name", txtDiaChi.Text);
                //    form.Add("cor_lat", lat.ToString().Replace(',','.'));
                //    form.Add("cor_long",lng.ToString().Replace(',', '.'));
                //    form.Add("cor_radius", txtBanKinh.Text);

                //    switch (Main.Type)
                //    {
                //        case 1:
                //            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Main.APIStaff.data.data.access_token);
                //            break;
                //        case 2:
                //            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Main.APICompany.data.data.access_token);
                //            break;
                //        default:
                //            break;
                //    }

                //    var respon = await httpClient.PostAsync("https://chamcong.24hpay.vn/service/add_coordinate.php", new FormUrlEncodedContent(form));
                //    API_Respon add = JsonConvert.DeserializeObject<API_Respon>(respon.Content.ReadAsStringAsync().Result);
                //    if (add.data != null && add.data.result)
                //    {
                //        var x = new PopUp.Notify1();
                //        x.Type = PopUp.Notify1.NotifyType.Success;
                //        x.Message = "Thêm tọa độ công ty thành công";
                //        Main.ShowPopup(x);

                //        if (Success != null) Success();
                //    }
                //    else
                //    {
                //        var x = new PopUp.Notify1();
                //        x.Type = PopUp.Notify1.NotifyType.Error;
                //        x.Message = "Thêm tọa độ công ty thất bại, vui lòng thử lại sau";
                //        Main.ShowPopup(x);
                //    }
                //}
                //catch (Exception ex)
                //{
                //    var x = new PopUp.Notify1();
                //    x.Type = PopUp.Notify1.NotifyType.Error;
                //    x.Message = ex.Message;
                //    Main.ShowPopup(x);
                //}
            }
        }
        private void mapView_Loaded(object sender, RoutedEventArgs e)
        {
            //GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;
            //mapView.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            //// lets the user drag the map
            //mapView.CanDragMap = true;
            //// lets the user drag the map with the left mouse button
            //mapView.DragButton = MouseButton.Left;

            //// choose your provider here
            //mapView.MapProvider = GMap.NET.MapProviders.OpenStreetMapProvider.Instance;
            //mapView.Position = new GMap.NET.PointLatLng(21.030653, 105.847130);

            //mapView.MinZoom = 2;
            //mapView.MaxZoom = 20;
            //mapView.Zoom = 10;
        }
        private void txtDiaChi_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Enter) mapView.SetPositionByKeywords(txtDiaChi.Text);
        }
    }
}
