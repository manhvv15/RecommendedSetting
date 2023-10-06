using QuanLyChung365TruocDangNhap.Hr.Entities.ManageRecuitmentEntities.ListCandidateEntities;
using QuanLyChung365TruocDangNhap.Hr.Popups.RecruitmentManagementPopups.ListOfCandidatesPopups;
using QuanLyChung365TruocDangNhap.Hr.Popups.RecruitmentManagementPopups.ListOfCandidatesPopups.CandidateDetailPopups;
using QuanLyChung365TruocDangNhap.Hr.View;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace QuanLyChung365TruocDangNhap.Hr.Pages.ManageRecruitmentPages.ListOfCandidatesPages
{
    /// <summary>
    /// Interaction logic for CandidateDetailGetJobPage.xaml
    /// </summary>
    public partial class CandidateDetailGetJobPage : Page
    {
        string id;
        string token;
        string url_cv = "";
        CandidateDetailEntity candidateDetailGetJob;
        Dictionary<string, string> listAllEmployee = new Dictionary<string, string>();
        Dictionary<string, string> listRecruitmentNew = new Dictionary<string, string>();
        List<Items> listItem = new List<Items>();
        // deligate event show popups
        public delegate void ShowPopup(object obj);
        public static event ShowPopup onShowPopup;

        public CandidateDetailGetJobPage(string token, string id, Dictionary<string, string> listAllEmployee, Dictionary<string, string> listRecruitmentNew, bool perEdit, List<Items> listItem)
        {
            InitializeComponent();
            this.token = token;
            this.id = id;
            this.listAllEmployee = listAllEmployee;
            this.listRecruitmentNew = listRecruitmentNew;
            if (perEdit)
            {
                gtBtnEdit.Visibility = Visibility.Visible;
            }
            else
            {
                gtBtnEdit.Visibility = Visibility.Collapsed;
            }
            this.listItem = listItem;
            GetData();
            EditCandidateGetJobPopup.hidePopup += HidePopup;
            CvCandidatePopup.hidePopup += HidePopup;
        }

        private async void GetData()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.detail_candidate_get_job;
                httpRequestMessage.RequestUri = new Uri(api);

                var parameters = new List<KeyValuePair<string, string>>();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                parameters.Add(new KeyValuePair<string, string>("id", id));

                // Thiết lập Content
                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                candidateDetailGetJob = JsonConvert.DeserializeObject<CandidateDetailEntity>(responseContent);


                if (candidateDetailGetJob.success.data.data != null)
                {
                    GetListSkill();
                    BindingData();
                }
            }
            catch
            {
                //MessageBox.Show("Lỗi đăng nhập, vui lòng thử lại!");
            }
        }

        private async void GetListSkill()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.detail_candidate;

                httpRequestMessage.RequestUri = new Uri(api);

                var parameters = new List<KeyValuePair<string, string>>();

                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                parameters.Add(new KeyValuePair<string, string>("canId", id));

                // Thiết lập Content
                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                CandidateDetailEntity candidateDetailEntity = JsonConvert.DeserializeObject<CandidateDetailEntity>(responseContent);

                if (candidateDetailEntity.result)
                {
                    candidateDetailGetJob.success.data.list_skill = candidateDetailEntity.success.data.list_skill;
                    icSkillVote.ItemsSource = candidateDetailGetJob.success.data.list_skill;
                }
            }
            catch
            {
                //MessageBox.Show("Lỗi đăng nhập, vui lòng thử lại!");
            }
        }

        private void BindingData()
        {
            List<ListSchedule> listSchedules = candidateDetailGetJob.success.data.data.list_schedule;
            //////////////////////////////////////////
            tbNameTitle.Text = candidateDetailGetJob.success.data.data.detail_candidate.name;
            tbID.Text = candidateDetailGetJob.success.data.data.detail_candidate.id;
            tbName.Text = candidateDetailGetJob.success.data.data.detail_candidate.name;
            tbEmail.Text = candidateDetailGetJob.success.data.data.detail_candidate.email;
            tbPhone.Text = candidateDetailGetJob.success.data.data.detail_candidate.phone;
            tbGender.Text = candidateDetailGetJob.success.data.data.detail_candidate.can_gender;
            tbHomeTown.Text = candidateDetailGetJob.success.data.data.detail_candidate.hometown;
            tbSchool.Text = candidateDetailGetJob.success.data.data.detail_candidate.school;
            if (tbHomeTown.Text == "")
            {
                tbHomeTown.Text = "Chưa cập nhật";
            }
            else
            {
                tbHomeTown.Text = candidateDetailGetJob.success.data.data.detail_candidate.hometown;
            }
            if (tbSchool.Text == "")
            {
                tbSchool.Text = "Chưa cập nhật";
            }
            else
            {
                tbSchool.Text = candidateDetailGetJob.success.data.data.detail_candidate.school;
            }

            tbAddress.Text = candidateDetailGetJob.success.data.data.detail_candidate.can_address;
            tbCvFrom.Text = candidateDetailGetJob.success.data.data.detail_candidate.cv_from;
            string date = candidateDetailGetJob.success.data.data.detail_candidate.time_send_cv;
            if (ConvertDate(date, "dd/MM/yyyy") != "")
            {
                tbTimeSendCV.Text = ConvertDate(date, "dd/MM/yyyy");
            }

            tbNewTitle.Text = candidateDetailGetJob.success.data.data.detail_candidate.new_title;

            //date = candidateDetailGetJob.success.data.data.detail_candidate.can_birthday;
            //if (ConvertDate(date, "dd/MM/yyyy") != "")
            //{
            //    tbDateOfBirth.Text = ConvertDate(date, "dd/MM/yyyy");
            //}

            string dateOfBirth =  candidateDetailGetJob.success.data.data.detail_candidate.can_birthday;
            if (ConvertDate(date, "dd/MM/yyyy") != "")
            {
                tbDateOfBirth.Text = ConvertDate(dateOfBirth, "dd/MM/yyyy");
            }
            //tbDateOfBirth.Text = candidateDetailGetJob.success.data.data.detail_candidate.can_birthday;


            tbNote.Text = candidateDetailGetJob.success.data.data.detail_get_job.note;
            tbEducation.Text = candidateDetailGetJob.success.data.data.detail_candidate.can_education;
            tbExp.Text = candidateDetailGetJob.success.data.data.detail_candidate.can_exp;
            tbMarried.Text = candidateDetailGetJob.success.data.data.detail_candidate.can_is_married;
            gridStar.DataContext = candidateDetailGetJob.success.data.data.detail_candidate;
            tbResiredSalary.Text = candidateDetailGetJob.success.data.data.detail_get_job.resired_salary + " VNĐ";
            tbSalary.Text = candidateDetailGetJob.success.data.data.detail_get_job.salary + " VNĐ";
            date = candidateDetailGetJob.success.data.data.detail_get_job.interview_time;

            if (ConvertDate(date, "HH:mm dd/MM/yyyy") != "")
            {
                tbTimeDate.Text = ConvertDate(date, "HH:mm dd/MM/yyyy");
            }

            string cv = candidateDetailGetJob.success.data.data.detail_candidate.cv;
            if (cv != null && cv != "")
            {
                url_cv = APIs.API.api_cv + cv;
                tbNoFile.Visibility = Visibility.Collapsed;
                tbViewFile.Visibility = Visibility.Visible;
            }
            else
            {
                tbNoFile.Visibility = Visibility.Visible;
                tbViewFile.Visibility = Visibility.Collapsed;
            }

            if (listAllEmployee.ContainsKey(candidateDetailGetJob.success.data.data.detail_candidate.user_hiring))
            {
                tbHR1.Text = listAllEmployee[candidateDetailGetJob.success.data.data.detail_candidate.user_hiring];
            }

            if (listAllEmployee.ContainsKey(candidateDetailGetJob.success.data.data.detail_candidate.user_recommend))
            {
                tbHR2.Text = listAllEmployee[candidateDetailGetJob.success.data.data.detail_candidate.user_hiring];
                tbHR4.Text = listAllEmployee[candidateDetailGetJob.success.data.data.detail_candidate.user_hiring];
            }
        
            if (candidateDetailGetJob.success.data.data.list_schedule != null)
            {
                icGiaiDoanChuyen.ItemsSource = candidateDetailGetJob.success.data.data.list_schedule;
                foreach (var item in listSchedules)
                {
                    int index = listItem.FindIndex(t => t.ID == item.process_interview_id);
                    if(index != -1 )
                    {
                        item.process_interview_id = listItem[index].Name;
                    }

                    if (listAllEmployee.ContainsKey(item.ep_interview))
                    {
                        item.name_schedule = listAllEmployee[item.ep_interview];
                    }
                    else
                    {
                        item.name_schedule = "Chưa cập nhật";
                    }
                    item.created_at = ConvertDate(item.created_at, "dd/MM/yyyy");
                    item.interview_time = ConvertDate(item.interview_time, "HH:mm dd/MM/yyyy");
                }
            }
        }

        private string ConvertDate(string date, string format)
        {
            DateTime myDate;
            try
            {
                myDate = DateTime.ParseExact(date, "yyyy-MM-ddTHH:mm",
                                              System.Globalization.CultureInfo.InvariantCulture);
                return myDate.ToString(format);
            }
            catch
            {
                try
                {
                    myDate = DateTime.ParseExact(date, "yyyy-MM-dd",
                                                  System.Globalization.CultureInfo.InvariantCulture);
                    return myDate.ToString(format);
                }
                catch
                {
                    try
                    {
                        myDate = DateTime.ParseExact(date, "yyyy/MM/dd",
                                                      System.Globalization.CultureInfo.InvariantCulture);
                        return myDate.ToString(format);
                    }
                    catch
                    {
                        try
                        {
                            myDate = DateTime.ParseExact(date, "dd/MM/yyyy",
                                                          System.Globalization.CultureInfo.InvariantCulture);
                            return myDate.ToString(format);
                        }
                        catch
                        {
                            try
                            {
                                myDate = DateTime.ParseExact(date, "MM/dd/yyyy",
                                                              System.Globalization.CultureInfo.InvariantCulture);
                                return myDate.ToString(format);
                            }
                            catch
                            {
                                try
                                {
                                    myDate = DateTime.ParseExact(date, "M/d/yyyy",
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

        }

        private void ViewFileCandidate(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = url_cv,
                UseShellExecute = true
            });
            //CvCandidatePopup editCandidateGetJobPopup = new CvCandidatePopup(token,id);
            //onShowPopup(editCandidateGetJobPopup);
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (Page.ActualWidth <= 900)
            {
                stackPanelContainer.Orientation = Orientation.Vertical;
                scrollviewerContainer.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
                //content.Height = 700;
                gridLeft.Margin = new Thickness(0, 0, 0, 15);
                gridRight.Height = 216;
            }
            else
            {
                stackPanelContainer.Orientation = Orientation.Horizontal;
                //content.Height = Double.NaN;
                scrollviewerContainer.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
                gridLeft.Margin = new Thickness(0, 0, 20, 0);
                gridRight.Height = Double.NaN;
            }
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

        private void ShowPopupEdit(object sender, MouseButtonEventArgs e)
        {
            EditCandidateGetJobPopup editCandidateGetJobPopup = new EditCandidateGetJobPopup(token, listAllEmployee, listRecruitmentNew, candidateDetailGetJob, listItem,id);
            onShowPopup(editCandidateGetJobPopup);
        }

        private void HidePopup(int mode)
        {
            if (mode == 1)
            {
                GetData();
            }
            onShowPopup("");
        }
    }
}
