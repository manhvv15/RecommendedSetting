using QuanLyChung365TruocDangNhap.Hr.Entities.ManageRecuitmentEntities.PerformRecuitmentEntities;
using QuanLyChung365TruocDangNhap.Hr.View;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for RecruitmentInformationDetailPage.xaml
    /// </summary>
    public partial class RecruitmentInformationDetailPage : Page
    {
        string token;
        string id;
        string hr_name;
        PerformDetail performDetail;
        public RecruitmentInformationDetailPage(string token, string id, string hr_name)
        {
            InitializeComponent();
            this.token = token;
            this.id = id;
            this.hr_name = hr_name;
            GetData();
        }

        private async void GetData()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.perform_detail;

                httpRequestMessage.RequestUri = new Uri(api);

                var parameters = new List<KeyValuePair<string, string>>();

                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                parameters.Add(new KeyValuePair<string, string>("recruitmentNewsId", id));

                // Thiết lập Content
                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                performDetail = JsonConvert.DeserializeObject<PerformDetail>(responseContent);
                if(performDetail.success != null)
                {
                    BindingTitle();
                    if(performDetail.success.data.listCandidate == null)
                    {
                        tbNoData1.Visibility = Visibility.Visible;
                        tbNoData2.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        tbNoData1.Visibility = Visibility.Collapsed;
                        tbNoData2.Visibility = Visibility.Collapsed;
                        icListCandidate.ItemsSource = performDetail.success.data.listCandidate;
                        icListCandidate2.ItemsSource = performDetail.success.data.listCandidate;
                    }

                    if (performDetail.success.data.listInterview == null)
                    {
                        tbNoData3.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        tbNoData3.Visibility = Visibility.Collapsed;
                        icListInterview.ItemsSource = performDetail.success.data.listInterview;
                    }

                    if (performDetail.success.data.listInterviewPass == null)
                    {
                        tbNoData4.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        tbNoData4.Visibility = Visibility.Collapsed;
                        icListInterviewPass.ItemsSource = performDetail.success.data.listInterviewPass;
                    }

                    if (performDetail.success.data.listInterviewFail == null)
                    {
                        tbNoData5.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        tbNoData5.Visibility = Visibility.Collapsed;
                        icListInterviewFail.ItemsSource = performDetail.success.data.listInterviewFail;
                    }

                    if (performDetail.success.data.listOfferJob == null)
                    {
                        tbNoData6.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        tbNoData6.Visibility = Visibility.Collapsed;
                        icListOfferJob.ItemsSource = performDetail.success.data.listOfferJob;
                    }
                }

            }
            catch
            {
                //MessageBox.Show("Lỗi đăng nhập, vui lòng thử lại!");
            }
        }

        private void BindingTitle()
        {
            tbHR.Text = hr_name;
            tbTitle.Text = performDetail.success.data.data.title_full;
            tbNumber.Text = performDetail.success.data.data.number;
            tbCreatedBy.Text = performDetail.success.data.data.created_by;
            tbSalary.Text = performDetail.success.data.data.salary;
            performDetail.success.data.data.recruitment_time = ConvertDate(performDetail.success.data.data.recruitment_time, "dd/MM/yyyy");
            performDetail.success.data.data.recruitment_time_to = ConvertDate(performDetail.success.data.data.recruitment_time_to, "dd/MM/yyyy");
            tbRecruitmentTime.Text = performDetail.success.data.data.recruitment_time;
            tbRecruitmentTimeTo.Text = performDetail.success.data.data.recruitment_time_to;

        }

        private void NavigateBack(object sender, MouseButtonEventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(HomeView))
                {
                    (window as HomeView).MainContent.NavigationService.GoBack();
                }
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
    }
}
