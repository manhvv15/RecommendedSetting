using QuanLyChung365TruocDangNhap.Hr.Entities.HomeEntity;
using QuanLyChung365TruocDangNhap.Hr.Entities.OrganizationalChartEntities;
using QuanLyChung365TruocDangNhap.Hr.Popups.OverviewPopups;
using QuanLyChung365TruocDangNhap.Hr.Popups.RecruitmentManagementPopups.ListOfCandidatesPopups.CandidateDetailPopups;
using QuanLyChung365TruocDangNhap.Hr.View;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
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
using static QRCoder.PayloadGenerator;

namespace QuanLyChung365TruocDangNhap.Hr.Pages.HomePages
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        System.Windows.Threading.DispatcherTimer Timer = new System.Windows.Threading.DispatcherTimer();
        string token;
        string id;
        //public delegate void ShowPopup(object obj);

        //public static event ShowPopup onShowPopup;

        Thickness thicknessColumn1L = new Thickness(0, 49, 0, 0);
        Thickness thicknessColmn1S = new Thickness(0, 180, 0, 0);
        Thickness thicknessGridHeadL = new Thickness(0, 50, 20, 0);
        Thickness thicknessGridHeadS = new Thickness(0, 50, 0, 0);

        public HomePage(string token,string id, string user_name)
        {
            InitializeComponent();
            this.token = token;
            this.id = id;
            //txtUsername.Text = user_name;
            GetAllData();
            Timer.Tick += new EventHandler(Timer_Click);

            Timer.Interval = new TimeSpan(0, 0, 1);

            Timer.Start();
        }

        public HomePage()
        {
            InitializeComponent();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //EditCandidateProfilePopup menuPopups = new EditCandidateProfilePopup();
            //onShowPopup(menuPopups);
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (homePage.ActualWidth < 900)
            {
                column1.Margin = thicknessColmn1S;
                Grid.SetColumnSpan(gridHead, 2);
                gridHead.Margin = thicknessGridHeadS;
            }
            else
            {
                column1.Margin = thicknessColumn1L;
                Grid.SetColumnSpan(gridHead, 1);
                gridHead.Margin = thicknessGridHeadL;
            }
        }

        private void GetAllData()
        {
            GetInfoEmployee();
            //GetTotalEmployee();
            GetAchievements();
            GetTotalCandidate();
            GetTotalViolation();
            GetTotalInterview();
            GetTotalOfferJob();
            GetWeather();
            GetDayOfWeek();
        }


        //Lấy tổng số nhân viên công ty
        private async void GetInfoEmployee()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = "http://210.245.108.202:3006/api/hr/organizationalStructure/detailInfoCompany";

                httpRequestMessage.RequestUri = new Uri(api);

                // Thiết lập Header
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);

                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                DetailOrganization result = JsonConvert.DeserializeObject<DetailOrganization>(responseContent);
                if (result.data == null) return;
                txtTotalEmployee.Text = result.data.infoCompany.tong_nv.ToString();
            }
            catch
            {
                //MessageBox.Show("Lỗi đăng nhập, vui lòng thử lại!");
            }
        }


        //private async void GetTotalEmployee()
        //{
        //    try
        //    {
        //        var httpClient = new HttpClient();
        //        var httpRequestMessage = new HttpRequestMessage();
        //        httpRequestMessage.Method = HttpMethod.Get;
        //        string api = APIs.API.total_employee_api;

        //        httpRequestMessage.RequestUri = new Uri(api);

        //        // Thiết lập Header
        //        httpRequestMessage.Headers.Add("Authorization", token);

        //        // Thực hiện Post
        //        var response = await httpClient.SendAsync(httpRequestMessage);

        //        var responseContent = await response.Content.ReadAsStringAsync();

        //        TotalEmployeeEntity result = JsonConvert.DeserializeObject<TotalEmployeeEntity>(responseContent);
        //        if (result.data == null) return;
        //        txtTotalEmployee.Text = (Convert.ToInt32(result.data.total_active) + Convert.ToInt32(result.data.total_not_active)).ToString();
        //    }
        //    catch
        //    {
        //        //MessageBox.Show("Lỗi đăng nhập, vui lòng thử lại!");
        //    }
        //}

        private async void GetAchievements()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.achievements_total_per_month;

                httpRequestMessage.RequestUri = new Uri(api);
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var parameters = new List<KeyValuePair<string, string>>();

                // Thiết lập Content
                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                AccountEmployeeEntity result = JsonConvert.DeserializeObject<AccountEmployeeEntity>(responseContent);
                if(result.result) txtAchievements.Text = result.success.data.total.ToString();
            }
            catch
            {
                //MessageBox.Show("Lỗi đăng nhập, vui lòng thử lại!");
            }
        }

        private async void GetTotalViolation()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.violation_total_per_month;

                httpRequestMessage.RequestUri = new Uri(api);
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var parameters = new List<KeyValuePair<string, string>>();
                // Thiết lập Content
                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                AccountEmployeeEntity result = JsonConvert.DeserializeObject<AccountEmployeeEntity>(responseContent);
                if (result.result)
                {
                    txtViolation.Text = result.success.data.total.ToString();
                }
            }
            catch
            {
                //MessageBox.Show("Lỗi đăng nhập, vui lòng thử lại!");
            }
        }

        private async void GetTotalInterview()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.interview_total;
                httpRequestMessage.RequestUri = new Uri(api);
                var parameters = new List<KeyValuePair<string, string>>();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                AccountEmployeeEntity result = JsonConvert.DeserializeObject<AccountEmployeeEntity>(responseContent);

                if (result.result)
                {
                    txtTotalInterview.Text = result.success.data.total.ToString();

                }
            }
            catch
            {

            }
        }

        private async void GetTotalOfferJob()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.offerjob_total;
                httpRequestMessage.RequestUri = new Uri(api);
                var parameters = new List<KeyValuePair<string, string>>();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                AccountEmployeeEntity result = JsonConvert.DeserializeObject<AccountEmployeeEntity>(responseContent);

                if (result.result)
                {
                    txtTotalOfferJob.Text = result.success.data.total.ToString();
                }
            }
            catch
            {

            }
        }

        private async void GetTotalCandidate()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.candidate_total;

                httpRequestMessage.RequestUri = new Uri(api);

                var parameters = new List<KeyValuePair<string, string>>();

                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                // Thiết lập Content
                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                AccountEmployeeEntity result = JsonConvert.DeserializeObject<AccountEmployeeEntity>(responseContent);

                if (result.result)
                {
                    txtCandidateTotal.Text = result.success.data.total.ToString();
                }
            }
            catch
            {
                //MessageBox.Show("Lỗi đăng nhập, vui lòng thử lại!");
            }
        }

        private async void GetWeather()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Get;
                string api = APIs.API.weather_api;

                HttpResponseMessage response = await httpClient.GetAsync(api);

                var responseContent = await response.Content.ReadAsStringAsync();

                WeatherEntity result = JsonConvert.DeserializeObject<WeatherEntity>(responseContent);

                txtTemp.Text = ((int)result.temp).ToString();
                imgWeather.Source = new BitmapImage(new Uri(result.icon));
            }
            catch
            {
                //MessageBox.Show("Lỗi đăng nhập, vui lòng thử lại!");
            }
        }
        private void Timer_Click(object sender, EventArgs e)
        {
            DateTime d;
            d = DateTime.Now;
            if(d.Minute >= 10 && d.Second >= 10)
            {
                txtClock.Text = d.Hour + ":" + d.Minute + ":" + d.Second;
            }
            else
            {
                if(d.Minute >= 10)
                    txtClock.Text = d.Hour + ":" + d.Minute + ":0" + d.Second;
                else
                    if(d.Second >= 10)
                        txtClock.Text = d.Hour + ":0" + d.Minute + ":" + d.Second;
                else
                    txtClock.Text = d.Hour + ":0" + d.Minute + ":0" + d.Second;
            }
        }

        private void GetDayOfWeek()
        {
            try
            {
                CultureInfo languageDay = new CultureInfo("vi-VN");
                DateTime dt = DateTime.Now;
                //txtDate.Text = dt.DayOfWeek.ToString() + ", ";
                txtDate.Text = languageDay.DateTimeFormat.GetDayName(dt.DayOfWeek) + ", ";
                txtDate.Text += dt.ToString("dd/MM/yyyy").Replace("-","/");
                txtMonth.Text = dt.ToString("MMM");
            }
            catch
            {

            }
        }
    }
}
