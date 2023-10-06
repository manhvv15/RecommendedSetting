using QuanLyChung365TruocDangNhap.Hr.Entities.HRReportEntity;
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

namespace QuanLyChung365TruocDangNhap.Hr.Pages.HRReportPages.HRReportPages
{
    /// <summary>
    /// Interaction logic for RecruitmentReport.xaml
    /// </summary>
    public partial class RecruitmentReport : Page, INotifyPropertyChanged
    {
        const int COUNT_RECORD_PER_PAGE1 = 3;
        int page_now1 = 1;
        int total1;
        int total_page1 = 1;
        public string PageNow1
        {
            get { return page_now1.ToString(); }
            set
            {
                page_now1 = Convert.ToInt32(value);
                OnPropertyChanged("PageNow1");
            }
        }

        const int COUNT_RECORD_PER_PAGE2 = 3;
        int page_now2 = 1;
        int total2;
        int total_page2 = 1;
        public string PageNow2
        {
            get { return page_now2.ToString(); }
            set
            {
                page_now2 = Convert.ToInt32(value);
                OnPropertyChanged("PageNow2");
            }
        }

        const int COUNT_RECORD_PER_PAGE3 = 10;
        int page_now3 = 1;
        int total3;
        int total_page3 = 1;
        public string PageNow3
        {
            get { return page_now3.ToString(); }
            set
            {
                page_now3 = Convert.ToInt32(value);
                OnPropertyChanged("PageNow3");
            }
        }

        bool flag = false;
        string token;
        string com_id;
        public Dictionary<string, string> listAllEmployee = new Dictionary<string, string>();
        public event PropertyChangedEventHandler PropertyChanged;

        public bool Flag
        {
            get { return flag; }
            set
            {
                flag = value;
                OnPropertyChanged("Flag");
            }
        }
        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (sizeH.ActualWidth > 900)
            {
                Flag = false;
            }
            else
            {
                Flag = true;
            }
        }
        protected virtual void OnPropertyChanged(string newName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(newName));
            }
        }
        public RecruitmentReport(string token, string com_id)
        {
            InitializeComponent();
            this.token = token;
            this.com_id = com_id;
            this.DataContext = this;
            GetAllEmployee();
            GetDataTotalReport();
            GetNewReportActive();
            TotalPage1.Text = total_page1.ToString();
            TotalPage2.Text = total_page2.ToString();
            TotalPage3.Text = total_page3.ToString();
        }

        private async void GetDataTotalReport()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.StatisticalReport;

                httpRequestMessage.RequestUri = new Uri(api);

                var parameters = new List<KeyValuePair<string, string>>();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                var response = await httpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                TotalReportEntity result = JsonConvert.DeserializeObject<TotalReportEntity>(responseContent);

                if (result.result)
                {
                    tbNew.Text = result.success.data.total_new.ToString();
                    tbCandidate.Text = result.success.data.total_candidate.ToString();
                    tbCandidateNumber.Text = result.success.data.total_candidate_number.ToString();
                    tbInterview.Text = result.success.data.total_interview.ToString();
                    tbInterviewPass.Text = result.success.data.total_interview_pass.ToString();
                    tbCancel.Text = result.success.data.total_cancel.ToString();
                }
            }
            catch (Exception)
            {

                //MessageBox.Show("hihiih");
            }
        }

        private async void GetNewReportActive()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.list_report_new_active;

                httpRequestMessage.RequestUri = new Uri(api);

                var parameters = new List<KeyValuePair<string, string>>();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                parameters.Add(new KeyValuePair<string, string>("page", PageNow1));
                parameters.Add(new KeyValuePair<string, string>("pageSize", COUNT_RECORD_PER_PAGE1.ToString()));

                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                var response = await httpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                Entities.ManageRecuitmentEntities.PerformRecuitmentEntities.ListNewActive result = JsonConvert.DeserializeObject<Entities.ManageRecuitmentEntities.PerformRecuitmentEntities.ListNewActive>(responseContent);

                if (result.result)
                {
                    total1 = result.success.data.total;
                    Paging1();
                    icListNewReportActive.Items.Refresh();
                    icListNewReportActive.ItemsSource = result.success.data.data;
                }
            }
            catch (Exception)
            {

                //MessageBox.Show("hihiih");
            }
        }

        private async void GetAllEmployee()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.list_all_employee2;

                httpRequestMessage.RequestUri = new Uri(api);

                // Thiết lập Header
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);
                Dictionary<string, string> form = new Dictionary<string, string>();
                form.Add("com_id", com_id);
                httpRequestMessage.Content = new FormUrlEncodedContent(form);
                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                Entities.HomeEntity.AllEmployeeEntity result = JsonConvert.DeserializeObject<Entities.HomeEntity.AllEmployeeEntity>(responseContent);
                if (result.data == null) return;
                foreach (var item in result.data.items)
                {
                    listAllEmployee.Add(item.ep_id, item.ep_name);
                }
                GetListRecruitmentStaff();
                GetListCandidateRcm();
            }
            catch
            {
                //MessageBox.Show("Lỗi đăng nhập, vui lòng thử lại!");
            }
        }

        private async void GetListRecruitmentStaff()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.reportListRecruitmentStaff;

                httpRequestMessage.RequestUri = new Uri(api);

                var parameters = new List<KeyValuePair<string, string>>();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                parameters.Add(new KeyValuePair<string, string>("page", PageNow2));
                parameters.Add(new KeyValuePair<string, string>("pageSize", COUNT_RECORD_PER_PAGE2.ToString()));

                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                var response = await httpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                ListRecruitmentStaffEntity result = JsonConvert.DeserializeObject<ListRecruitmentStaffEntity>(responseContent);

                if (result.result)
                {
                    total2 = result.success.data.total;
                    Paging2();
                    int index = 1;
                    foreach(var item in result.success.data.data)
                    {
                        if (listAllEmployee.ContainsKey(item.hr_name))
                        {
                            int stt = (page_now2 - 1) * COUNT_RECORD_PER_PAGE2 + index;
                            item.STT = stt;
                            index++;
                            item.Hr_name = listAllEmployee[item.hr_name];
                        }
                    }
                    icListRecruitmentStaff.Items.Refresh();
                    icListRecruitmentStaff.ItemsSource = result.success.data.data;
                }
            }
            catch (Exception)
            {

                //MessageBox.Show("hihiih");
            }
        }

        private async void GetListCandidateRcm()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.reportlistCandidateRcm;

                httpRequestMessage.RequestUri = new Uri(api);

                var parameters = new List<KeyValuePair<string, string>>();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                parameters.Add(new KeyValuePair<string, string>("page", PageNow3));
                parameters.Add(new KeyValuePair<string, string>("pageSize", COUNT_RECORD_PER_PAGE3.ToString()));

                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                var response = await httpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                ListReportCandidateRcm result = JsonConvert.DeserializeObject<ListReportCandidateRcm>(responseContent);

                if (result.result)
                {
                    total3 = result.success.data.total;
                    Paging3();
                    foreach (var item in result.success.data.data)
                    {
                        if (listAllEmployee.ContainsKey(item.user_recommend))
                        {
                            item.name = listAllEmployee[item.user_recommend];
                        }
                    }
                    icListCandidateRcm.Items.Refresh();
                    icListCandidateRcm.ItemsSource = result.success.data.data;
                }
            }
            catch (Exception)
            {

                //MessageBox.Show("hihiih");
            }
        }

        private void NavigateToHRReportPage(object sender, MouseButtonEventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(HomeView))
                {
                    (window as HomeView).MainContent.Navigate(new HRReportPage(token, com_id));
                }
            }
        }

        private void NextPage1(object sender, MouseButtonEventArgs e)
        {
            InvalidBtn1();
            int page_next = page_now1 + 1;
            PageNow1 = page_next.ToString();
            GetNewReportActive();
        }

        private void PrevPage1(object sender, MouseButtonEventArgs e)
        {
            InvalidBtn1();
            int page_prev = page_now1 - 1;
            PageNow1 = page_prev.ToString();
            GetNewReportActive();
        }


        private void InvalidBtn1()
        {
            btnPrev1.IsEnabled = false;
            btnPrev1.Opacity = 0.3;
            btnNext1.IsEnabled = false;
            btnNext1.Opacity = 0.3;
        }

        private void Paging1()
        {
            if (total1 == 0)
            {
                total_page1 = 1;
            }
            else
            {
                if (total1 % COUNT_RECORD_PER_PAGE1 == 0)
                {
                    total_page1 = total1 / COUNT_RECORD_PER_PAGE1;
                    TotalPage1.Text = total_page1.ToString();
                }
                else
                {
                    total_page1 = total1 / COUNT_RECORD_PER_PAGE1 + 1;
                    TotalPage1.Text = total_page1.ToString();
                }
            }
            IsSetValidBtn();
        }

        private void IsSetValidBtn()
        {
            if (page_now1 == 1)
            {
                btnPrev1.IsEnabled = false;
                btnPrev1.Opacity = 0.3;
            }
            else
            {
                btnPrev1.IsEnabled = true;
                btnPrev1.Opacity = 1;
            }

            if (page_now1 == total_page1)
            {
                btnNext1.IsEnabled = false;
                btnNext1.Opacity = 0.3;
            }
            else
            {
                btnNext1.IsEnabled = true;
                btnNext1.Opacity = 1;
            }
        }

        private void NextPage2(object sender, MouseButtonEventArgs e)
        {
            InvalidBtn2();
            int page_next = page_now2 + 1;
            PageNow2 = page_next.ToString();
            GetListRecruitmentStaff();
        }

        private void PrevPage2(object sender, MouseButtonEventArgs e)
        {
            InvalidBtn2();
            int page_prev = page_now2 - 1;
            PageNow2 = page_prev.ToString();
            GetListRecruitmentStaff();
        }

        private void InvalidBtn2()
        {
            btnPrev2.IsEnabled = false;
            btnPrev2.Opacity = 0.3;
            btnNext2.IsEnabled = false;
            btnNext2.Opacity = 0.3;
        }

        private void Paging2()
        {
            if (total2 == 0)
            {
                total_page2 = 1;
            }
            else
            {
                if (total2 % COUNT_RECORD_PER_PAGE2 == 0)
                {
                    total_page2 = total2 / COUNT_RECORD_PER_PAGE2;
                    TotalPage2.Text = total_page2.ToString();
                }
                else
                {
                    total_page2 = total2 / COUNT_RECORD_PER_PAGE2 + 1;
                    TotalPage2.Text = total_page2.ToString();
                }
            }
            IsSetValidBtn2();
        }

        private void IsSetValidBtn2()
        {
            if (page_now2 == 1)
            {
                btnPrev2.IsEnabled = false;
                btnPrev2.Opacity = 0.3;
            }
            else
            {
                btnPrev2.IsEnabled = true;
                btnPrev2.Opacity = 1;
            }

            if (page_now2 == total_page2)
            {
                btnNext2.IsEnabled = false;
                btnNext2.Opacity = 0.3;
            }
            else
            {
                btnNext2.IsEnabled = true;
                btnNext2.Opacity = 1;
            }
        }


        private void NextPage3(object sender, MouseButtonEventArgs e)
        {
            InvalidBtn3();
            int page_next = page_now3 + 1;
            PageNow3 = page_next.ToString();
            GetListCandidateRcm();
        }

        private void PrevPage3(object sender, MouseButtonEventArgs e)
        {
            InvalidBtn3();
            int page_prev = page_now3 - 1;
            PageNow3 = page_prev.ToString();
            GetListCandidateRcm();
        }

        private void InvalidBtn3()
        {
            btnPrev3.IsEnabled = false;
            btnPrev3.Opacity = 0.3;
            btnNext3.IsEnabled = false;
            btnNext3.Opacity = 0.3;
        }

        private void Paging3()
        {
            if (total3 == 0)
            {
                total_page3 = 1;
            }
            else
            {
                if (total3 % COUNT_RECORD_PER_PAGE3 == 0)
                {
                    total_page3 = total3 / COUNT_RECORD_PER_PAGE3;
                    TotalPage3.Text = total_page3.ToString();
                }
                else
                {
                    total_page3 = total3 / COUNT_RECORD_PER_PAGE3 + 1;
                    TotalPage3.Text = total_page3.ToString();
                }
            }
            IsSetValidBtn3();
        }

        private void IsSetValidBtn3()
        {
            if (page_now3 == 1)
            {
                btnPrev3.IsEnabled = false;
                btnPrev3.Opacity = 0.3;
            }
            else
            {
                btnPrev3.IsEnabled = true;
                btnPrev3.Opacity = 1;
            }

            if (page_now3 == total_page3)
            {
                btnNext3.IsEnabled = false;
                btnNext3.Opacity = 0.3;
            }
            else
            {
                btnNext3.IsEnabled = true;
                btnNext3.Opacity = 1;
            }
        }
    }
}
