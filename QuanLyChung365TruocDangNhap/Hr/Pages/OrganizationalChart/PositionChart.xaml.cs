using QuanLyChung365TruocDangNhap.Hr.Entities.OrganizationalChartEntities;
using QuanLyChung365TruocDangNhap.Hr.Popups.OrganizationalChartPopup;
using QuanLyChung365TruocDangNhap.Hr.View;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
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

namespace QuanLyChung365TruocDangNhap.Hr.Pages.OrganizationalChart
{
    /// <summary>
    /// Interaction logic for PositionChart.xaml
    /// </summary>
    /// 


    public partial class PositionChart : Page, INotifyPropertyChanged
    {
        string token;
        string id;
        string pho_to_truong;

        // deligate event show popups
        public delegate void ShowPopup(object obj);
        public static event ShowPopup onShowPopup;

        //ban giám đốc
        string managerPre;
        string vicePresident;

        public string ManagerPre
        {
            get
            {
                return managerPre;
            }
            set
            {
                managerPre = value;
                OnPropertyChanged("Manager");
            }
        }

        public string VicePresident
        {
            get
            {
                return vicePresident;
            }
            set
            {
                vicePresident = value;
                OnPropertyChanged("VicePresident");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string newName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(newName));
            }
        }

        public PositionChart(string token, string id, string pho_to_truong)
        {
            InitializeComponent();
            this.token = token;
            this.id = id;
            this.pho_to_truong = pho_to_truong;
            GetAll();

        }

        private void GetAll()
        {
            GetListChild();
            GetListposition();
            GetListEmpOfficial();
            GetListEmpProbation();
            GetListEmpParttime();
            GetListEmpIntership();
        }

        private void HidePopup(int mode)
        {
            if (mode == 1)
            {
                GetAll();
            }
            onShowPopup("");
        }

        //chuyển page
        private void NavigateToPage(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            string name = textBlock.Name;
            String uri = "OrganizationalChart/" + name + "Page";
            switch (name)
            {
                case "OrganisationalStructureDiagram":
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(HomeView))
                        {
                            (window as HomeView).MainContent.Navigate(new OrganisationalStructureDiagram(token, id, pho_to_truong));
                        }
                    };
                    break;
                case "PositionChart":
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(HomeView))
                        {
                            (window as HomeView).MainContent.Navigate(new PositionChart(token, id, pho_to_truong));
                        }
                    };
                    break;
                case "RightToUse":
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(HomeView))
                        {
                            (window as HomeView).MainContent.Navigate(new RightToUse(token, id, pho_to_truong));
                        }
                    };
                    break;
                case "LeadershipBiography":
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(HomeView))
                        {
                            (window as HomeView).MainContent.Navigate(new LeadershipBiography(token, id, pho_to_truong));
                        }
                    };
                    break;
            }
        }

        //Lấy danh sách ban giám đốc
        private async void GetListChild()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = "http://210.245.108.202:3000/api/qlc/company/child/list";

                httpRequestMessage.RequestUri = new Uri(api);

                // Thiết lập Header
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);

                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                listChildEntities result = JsonConvert.DeserializeObject<listChildEntities>(responseContent);
                if (result.data != null)
                {
                    List<Item> items = result.data.items;
                    foreach (var item in items[0].manager)
                    {
                        item.type = "Giám Đốc:";
                        nameManager.Text = item.ep_name;
                    }
                    foreach (var item in items[0].deputy)
                    {
                        VicePresident = item.ep_name;
                        item.type = "Phó Giám Đốc:";
                        icListChill.Items.Add(item);
                    }
                }
            }
            catch
            {
                //MessageBox.Show("Lỗi đăng nhập, vui lòng thử lại!");
            }
        }

        //Tổng số nhân viên chính thức
        private async void GetListEmpOfficial()
        {
            var httpClient = new HttpClient();
            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = HttpMethod.Post;

            httpRequestMessage.RequestUri = new Uri("http://210.245.108.202:3006/api/hr/report/reportDetail");

            // Thiết lập Header
            httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);
            var form = new Dictionary<string, string>();
            form.Add("positionId", "3");
            form.Add("winform", "1");
            // Thực hiện Post
            var response = await httpClient.SendAsync(httpRequestMessage);

            var responseContent = await response.Content.ReadAsStringAsync();
            listPossition result = JsonConvert.DeserializeObject<listPossition>(responseContent);
            if (result.data != null)
            {
                TongNhanVienChinhThuc.Text = result.data.totalItems;
            }
        }

        //Tổng số nhân viên thử việc
        private async void GetListEmpProbation()
        {
            var httpClient = new HttpClient();
            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = HttpMethod.Post;

            httpRequestMessage.RequestUri = new Uri("http://210.245.108.202:3006/api/hr/report/reportDetail");

            // Thiết lập Header
            httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);
            var form = new Dictionary<string, string>();
            form.Add("position_id", "2");
            form.Add("winform", "1");
            // Thực hiện Post
            var response = await httpClient.SendAsync(httpRequestMessage);

            var responseContent = await response.Content.ReadAsStringAsync();
            listPossition result = JsonConvert.DeserializeObject<listPossition>(responseContent);
            if (result.data != null)
            {
                TongSoNVThuViec.Text = result.data.totalItems;
            }
        }

        //Tổng số nhân viên part time
        private async void GetListEmpParttime()
        {
            var httpClient = new HttpClient();
            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = HttpMethod.Post;

            httpRequestMessage.RequestUri = new Uri("http://210.245.108.202:3000/api/hr/report/reportDetail");

            // Thiết lập Header
            httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);
            var form = new Dictionary<string, string>();
            form.Add("positionId", "9");
            form.Add("winform", "1");
            // Thực hiện Post
            var response = await httpClient.SendAsync(httpRequestMessage);

            var responseContent = await response.Content.ReadAsStringAsync();
            listPossition result = JsonConvert.DeserializeObject<listPossition>(responseContent);
            if (result.data != null)
            {
                TongSoNVParttime.Text = result.data.totalItems;
            }
        }

        //Tổng số nhân viên thực tập
        private async void GetListEmpIntership()
        {
            var httpClient = new HttpClient();
            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = HttpMethod.Get;

            httpRequestMessage.RequestUri = new Uri("http://210.245.108.202:3006/api/hr/report/reportDetail");
            var form = new Dictionary<string, string>();
            form.Add("link", "bieu-do-danh-sach-nhan-vien.html");
            form.Add("winform", "1");
            form.Add("positionId", "1");
            // Thiết lập Header
            httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);

            // Thực hiện Post
            var response = await httpClient.SendAsync(httpRequestMessage);

            var responseContent = await response.Content.ReadAsStringAsync();
            listPossition result = JsonConvert.DeserializeObject<listPossition>(responseContent);
            if (result.data != null)
            {
                TongSoNVThucTap.Text = result.data.totalItems;
            }
        }

        //lọc chức vụ sơ đồ chức vụ
        private async void GetListposition()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;

                httpRequestMessage.RequestUri = new Uri("http://210.245.108.202:3000/api/hr/report/reportDetail");

                // Thiết lập Header
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);
                var form = new Dictionary<string, string>();
                form.Add("winform", "1");
                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                listPossition result = JsonConvert.DeserializeObject<listPossition>(responseContent);

                if (result.data != null)
                {
                    string total = result.data.totalItems;

                    foreach (var item in result.data.items)
                    {
                        if (item.position_id == "13")
                        {
                            icListToTruong.Items.Add(item);
                        }
                        if (item.position_id == "12")
                        {
                            icListPhoToTruong.Items.Add(item);
                        }
                        if (item.position_id == "4")
                        {
                            icListTruongNhom.Items.Add(item);
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private void ShowPopupSeeMore(object sender, MouseButtonEventArgs e)
        {
            SeeMorePopup seeMorePopup = new SeeMorePopup();
            onShowPopup(seeMorePopup);
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                scroll.ScrollToVerticalOffset(scroll.VerticalOffset);
                scroll.ScrollToHorizontalOffset(scroll.HorizontalOffset - e.Delta);
            }
            else
                scroll.ScrollToVerticalOffset(scroll.VerticalOffset - e.Delta);
        }
    }
}
