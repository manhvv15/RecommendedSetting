using QuanLyChung365TruocDangNhap.GioiHanIpVaPhanMem.Popup;
using QuanLyChung365TruocDangNhap.Popup.TruocDangNhap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
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
using QuanLyChung365TruocDangNhap.GioiHanIpVaPhanMem.Entities;
using Newtonsoft.Json;

namespace QuanLyChung365TruocDangNhap.GioiHanIpVaPhanMem.Pages
{
    /// <summary>
    /// Interaction logic for PageDanhSachNhanVien.xaml
    /// </summary>
    public partial class PageDanhSachNhanVien : Page
    {
        public PageDanhSachNhanVien()
        {
            InitializeComponent();

            GetListUserIPApp();


        }

        private void BtnSetUp_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ucChonNhanVienVaCapQuyen uc = new ucChonNhanVienVaCapQuyen();
            this.body.Children.Clear(); 
            object content = uc.Content;
            uc.Content = null;
            this.body.Children.Add(content as UIElement);


     
        }

        private async void GetListUserIPApp()
        {
            var handler = new HttpClientHandler();

            using (var httpClient = new HttpClient(handler))
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://api.timviec365.vn/api/qlc/settingIPApp/listUser"))
                {

                    request.Headers.TryAddWithoutValidation("authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJkYXRhIjp7Il9pZCI6MTM3NzQ0OSwiaWRUaW1WaWVjMzY1IjoyMzI0MTYsImlkUUxDIjoxNjY0LCJpZFJhb05oYW5oMzY1IjowLCJlbWFpbCI6InRyYW5nY2h1b2k0QGdtYWlsLmNvbSIsInBob25lVEsiOiJ0cmFuZ2NodW9pNEBnbWFpbC5jb20iLCJjcmVhdGVkQXQiOjE2NjM4MzY0MDUsInR5cGUiOjEsImNvbV9pZCI6MTY2NCwidXNlck5hbWUiOiJDw7RuZyB0eSBj4buVIHBo4bqnbiB4deG6pXQgbmjhuq1wIGto4bqpdSAxMjMifSwiaWF0IjoxNjk2NDg5NTU0LCJleHAiOjE2OTY1NzU5NTR9._uyz2uRQel7hvumnq0zFQREMDfN7wC4LlP4ULmLwIto");


                    request.Content = new StringContent("");
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                    var response = await httpClient.SendAsync(request);
                    if(response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        SettingIpAppEntities.Root result = JsonConvert.DeserializeObject<SettingIpAppEntities.Root>(responseContent);
                        var list = result.data.data;
                        dsGioiHanIpVaUngDung.ItemsSource = list;
                    }

                }
            }
        }

        private void Edit_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ucChinhSuaGioiHan uc = new ucChinhSuaGioiHan();
            this.body.Children.Clear();
            object content = uc.Content;
            uc.Content = null;
            this.body.Children.Add(content as UIElement);

        }

        private void Delelte_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ucXoaNhanVienGioiHanIP uc = new ucXoaNhanVienGioiHanIP();
            this.body.Children.Clear();
            object content = uc.Content;
            uc.Content = null;
            this.body.Children.Add(content as UIElement);

        }
    }
}
