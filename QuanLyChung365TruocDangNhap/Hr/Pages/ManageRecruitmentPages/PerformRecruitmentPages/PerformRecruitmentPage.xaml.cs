using QuanLyChung365TruocDangNhap.Hr.Entities.ManageRecuitmentEntities.PerformRecuitmentEntities;
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

namespace QuanLyChung365TruocDangNhap.Hr.Pages.ManageRecruitmentPages.PerformRecruitmentPages
{
    /// <summary>
    /// Interaction logic for PerformRecruitmentPage.xaml
    /// </summary>
    public partial class PerformRecruitmentPage : Page, INotifyPropertyChanged
    {
        //private bool show;
        string token;
        string id;
        bool perAdd, perEdit, perDel; // quyền thêm, sửa, xóa
        public const int ROW_PER_PAGE1 = 5;
        public int page_now1 = 1;
        public int total1;
        public int total_page1;
        List<DataEntity> listData1;

        public const int ROW_PER_PAGE2 = 5;
        public int page_now2 = 1;
        public int total2;
        public int total_page2;
        List<DataEntity3> listData2;

        //public bool Show
        //{
        //    get { return show; }
        //    set
        //    {
        //        show = value;
        //        OnPropertyChanged("Show");
        //    }
        //}

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string newName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(newName));
            }
        }

        public PerformRecruitmentPage()
        {
            InitializeComponent();
        }

        public PerformRecruitmentPage(string token, string id, bool perAdd, bool perEdit, bool perDel)
        {
            InitializeComponent();
            this.token = token;
            this.id = id;
            this.perAdd = perAdd;
            this.perEdit = perEdit;
            this.perDel = perDel;
            listData1 = new List<DataEntity>();
            listData2 = new List<DataEntity3>();
            GetData();
            DataContext = this;
        }

        private void GetData()
        {
            GetTotalCandidateBy();
            GetListNewActive();
            GetListSchedule();
        }

        private async void GetTotalCandidateBy()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.total_candidate_by;

                httpRequestMessage.RequestUri = new Uri(api);

                var parameters = new List<KeyValuePair<string, string>>();

                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                // Thiết lập Content
                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                TotalCandidateBy result = JsonConvert.DeserializeObject<TotalCandidateBy>(responseContent);
                if(result.success != null)
                {
                    int candidateToday = result.success.data.data.candidateToday;
                    if(candidateToday > 0 && candidateToday < 10)
                    {
                        tbCandidateToday.Text = "0" + candidateToday.ToString();
                    }
                    else
                    {
                        tbCandidateToday.Text = candidateToday.ToString();
                    }

                    int candidateWeek = result.success.data.data.candidateWeek;
                    if (candidateWeek > 0 && candidateWeek < 10)
                    {
                        tbCandidateWeek.Text = "0" + candidateWeek.ToString();
                    }
                    else
                    {
                        tbCandidateWeek.Text = candidateWeek.ToString();
                    }

                    int candidateMonth = result.success.data.data.candidateMonth;
                    if (candidateMonth > 0 && candidateMonth < 10)
                    {
                        tbCandidateMonth.Text = "0" + candidateMonth.ToString();
                    }
                    else
                    {
                        tbCandidateMonth.Text = candidateMonth.ToString();
                    }
                }
            }
            catch
            {
                //MessageBox.Show("Lỗi đăng nhập, vui lòng thử lại!");
            }
        }

        private async void GetListNewActive()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.lits_new_active;

                httpRequestMessage.RequestUri = new Uri(api);

                var parameters = new List<KeyValuePair<string, string>>();

                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                parameters.Add(new KeyValuePair<string, string>("rowno", page_now1.ToString()));
                parameters.Add(new KeyValuePair<string, string>("rowperpage", ROW_PER_PAGE1.ToString()));

                // Thiết lập Content
                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                ListNewActive result = JsonConvert.DeserializeObject<ListNewActive>(responseContent);
                if(result.success == null)
                {
                    tbNoData1.Visibility = Visibility.Visible;
                    return;
                }

                tbNoData1.Visibility = Visibility.Collapsed;
                total1 = result.success.data.total;
                Paging1();
                foreach(var item in result.success.data.data)
                {
                    item.recruitment_time = ConvertDate(item.recruitment_time, "dd/MM/yyyy");
                    item.recruitment_time_to = ConvertDate(item.recruitment_time_to, "dd/MM/yyyy");
                    item.sohoso = Decimal(item.sohoso);
                    item.henphongvan = Decimal(item.henphongvan);
                    item.quaphongvan = Decimal(item.quaphongvan);
                    item.huyphongvan = Decimal(item.huyphongvan);
                }
                listData1 = listData1.Concat(result.success.data.data).ToList();
                icListNewActive.ItemsSource = listData1;
            }
            catch
            {
                //MessageBox.Show("Lỗi đăng nhập, vui lòng thử lại!");
            }
        }

        private string ConvertDate(string date, string format)
        {
            try
            {
                DateTime myDate = DateTime.ParseExact(date, "MM/dd/yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);
                return myDate.ToString(format);
            }
            catch
            {
                try
                {
                    DateTime myDate = DateTime.ParseExact(date, "yyyy-MM-dd",
                                       System.Globalization.CultureInfo.InvariantCulture);
                    return myDate.ToString(format);
                }
                catch
                {
                    try
                    {
                        DateTime myDate = DateTime.ParseExact(date, "M/d/yyyy",
                                           System.Globalization.CultureInfo.InvariantCulture);
                        return myDate.ToString(format);
                    }
                    catch
                    {
                        return "";
                    }
                }

            }


        }

        private async void GetListSchedule()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.list_schedule;

                httpRequestMessage.RequestUri = new Uri(api);

                var parameters = new List<KeyValuePair<string, string>>();

                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                parameters.Add(new KeyValuePair<string, string>("rowno", page_now2.ToString()));
                parameters.Add(new KeyValuePair<string, string>("rowperpage", ROW_PER_PAGE2.ToString()));

                // Thiết lập Content
                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                ListSchedule result = JsonConvert.DeserializeObject<ListSchedule>(responseContent);
                if (result.success == null || result.success.data.data.Count == 0)
                {
                    tbNoData2.Visibility = Visibility.Visible;
                    return;
                }

                tbNoData2.Visibility = Visibility.Collapsed;
                total2 = result.success.data.total;
                Paging2();
                foreach (var item in result.success.data.data)
                {
                    //DateTime dateTime = Convert.ToDateTime(item.thoigianphongvan);
                }
                listData2 = listData2.Concat(result.success.data.data).ToList();
                icListSchedule.ItemsSource = listData2;
            }
            catch
            {
                //MessageBox.Show("Lỗi đăng nhập, vui lòng thử lại!");
            }
        }

        private string Decimal(string num)
        {
            int n;
            bool isNumeric = int.TryParse(num, out n);
            if (isNumeric)
            {
                if(n > 0 && n < 10)
                {
                    return "0" + n.ToString();
                }
                else
                {
                    return n.ToString();
                }
            }
            return "0";
        }

        private void Paging1()
        {
            if (total1 % ROW_PER_PAGE1 == 0)
            {
                total_page1 = total1 / ROW_PER_PAGE1;
            }
            else
            {
                total_page1 = total1 / ROW_PER_PAGE1 + 1;
            }
        }

        private void Paging2()
        {
            if (total2 % ROW_PER_PAGE2 == 0)
            {
                total_page2 = total2 / ROW_PER_PAGE2;
            }
            else
            {
                total_page1 = total2 / ROW_PER_PAGE2 + 1;
            }
        }

        private void navigateToPage(object sender, MouseButtonEventArgs e)
        {
            Grid grid = sender as Grid;
            string name = grid.Name;
            switch (name)
            {
                case "PerformRecruitment":
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(HomeView))
                        {
                            (window as HomeView).MainContent.Navigate(new PerformRecruitmentPage(token, id, perAdd, perEdit, perDel));
                        }
                    };
                    break;
                case "RecruitmentInformation":
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(HomeView))
                        {
                            (window as HomeView).MainContent.Navigate(new RecruitmentInformationPage(token, id, perAdd, perEdit, perDel));
                        }
                    };
                    break;
                case "RecruitmentConnection":
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(HomeView))
                        {
                            (window as HomeView).MainContent.Navigate(new RecruitmentConnectionPage());
                        }
                    };
                    break;
                case "RecruitmentFee":
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(HomeView))
                        {
                            (window as HomeView).MainContent.Navigate(new RecruitmentFeePage());
                        }
                    };
                    break;
            }
        }

        //private void page_SizeChanged(object sender, SizeChangedEventArgs e)
        //{
        //    if (page.ActualWidth < 900)
        //    {
        //        Show = false;
        //    }
        //    else
        //    {
        //        Show = true;
        //    }
        //}


        private void Scroll1Changed(object sender, ScrollChangedEventArgs e)
        {
            if (e.VerticalChange > 0)
            {
                if (e.VerticalOffset + e.ViewportHeight == e.ExtentHeight)
                {
                    if(page_now1 < total_page1)
                    {
                        page_now1++;
                        GetListNewActive();
                    }
                }
            }
        }

        private void Scroll2Changed(object sender, ScrollChangedEventArgs e)
        {
            if (e.VerticalChange > 0)
            {
                if (e.VerticalOffset + e.ViewportHeight == e.ExtentHeight)
                {
                    if (page_now2 < total_page2)
                    {
                        page_now2++;
                        GetListSchedule();
                    }
                }
            }
        }
    }
}

