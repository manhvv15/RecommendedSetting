using QuanLyChung365TruocDangNhap.Hr.Entities.ManageRecuitmentEntities.ListCandidateEntities;
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
    /// Interaction logic for CandidateDetailContractJobPage.xaml
    /// </summary>
    public partial class CandidateDetailContractJobPage : Page
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
        public CandidateDetailContractJobPage(string token, string id, Dictionary<string, string> listAllEmployee, Dictionary<string, string> listRecruitmentNew, bool perEdit, List<Items> listItem)
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
            EditContractPopup.hidePopup += HidePopup;
        }

        private async void GetData()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.detail_candidate_contract_job;

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
                GetListSkill();
                BindingData();

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
            if (candidateDetailGetJob.success.data.data != null)
            {
                if (candidateDetailGetJob.success.data.data.detail_candidate != null)
                {
                    List<ListSchedule> listSchedules = candidateDetailGetJob.success.data.data.list_schedule;

                    tbNameTitle.Text = candidateDetailGetJob.success.data.data.detail_candidate.name; //Tên chi tiết ứng viên
                    //Thông tin ứng viên
                    tbID.Text = candidateDetailGetJob.success.data.data.detail_candidate.id;
                    tbName.Text = candidateDetailGetJob.success.data.data.detail_candidate.name;
                    tbEmail.Text = candidateDetailGetJob.success.data.data.detail_candidate.email;
                    tbPhone.Text = candidateDetailGetJob.success.data.data.detail_candidate.phone;
                    tbGender.Text = candidateDetailGetJob.success.data.data.detail_candidate.can_gender;
                    string dateBirth = candidateDetailGetJob.success.data.data.detail_candidate.can_birthday;
                    if (ConvertDate(dateBirth, "dd/MM/yyyy") != "")
                    {
                        tbDateOfBirth.Text = ConvertDate(dateBirth, "dd/MM/yyyy");
                    }
                    tbHomeTown.Text = candidateDetailGetJob.success.data.data.detail_candidate.hometown;
                    tbEducation.Text = candidateDetailGetJob.success.data.data.detail_candidate.can_education;
                    tbSchool.Text = candidateDetailGetJob.success.data.data.detail_candidate.school;
                    tbExp.Text = candidateDetailGetJob.success.data.data.detail_candidate.can_exp;
                    tbMarried.Text = candidateDetailGetJob.success.data.data.detail_candidate.can_is_married;
                    tbAddress.Text = candidateDetailGetJob.success.data.data.detail_candidate.can_address;

                    //Thông tin tuyển dụng
                    tbCvFrom.Text = candidateDetailGetJob.success.data.data.detail_candidate.cv_from;
                    tbNewTitle.Text = candidateDetailGetJob.success.data.data.detail_candidate.new_title;
                    if (listAllEmployee.ContainsKey(candidateDetailGetJob.success.data.data.detail_candidate.user_hiring))
                    {
                        tbHR1.Text = listAllEmployee[candidateDetailGetJob.success.data.data.detail_candidate.user_hiring];
                    }

                    //Quản lý tuyển dụng
                    string dateTimeSendCV = candidateDetailGetJob.success.data.data.detail_candidate.time_send_cv;
                    if (ConvertDate(dateTimeSendCV, "dd/MM/yyyy") != "")
                    {
                        tbTimeSendCV.Text = ConvertDate(dateTimeSendCV, "dd/MM/yyyy");
                    }
                    gridStar.DataContext = candidateDetailGetJob.success.data.data.detail_candidate;

                    //Giai đoạn chuyển chuyển lặp 
                    if (candidateDetailGetJob.success.data.data.list_schedule != null)
                    {
                        icGiaiDoanChuyen.ItemsSource = candidateDetailGetJob.success.data.data.list_schedule;
                        foreach (var item in listSchedules)
                        {
                            int index = listItem.FindIndex(t => t.ID == item.process_interview_id);
                            if (index != -1)
                            {
                                item.process_interview_id = listItem[index].Name;
                            }
                            item.interview_time = ConvertDate(item.interview_time, "HH:mm dd/MM/yyyy");
                            if (listAllEmployee.ContainsKey(item.ep_interview))
                            {
                                item.name_schedule = listAllEmployee[item.ep_interview];
                            }
                            else
                            {
                                item.name_schedule = "Chưa cập nhật";
                            }
                            item.created_at = ConvertDate(item.created_at, "dd/MM/yyyy");
                        }
                    }

                    //Giai đoạn chuyển
                    if (listAllEmployee.ContainsKey(candidateDetailGetJob.success.data.data.detail_candidate.user_hiring))
                    {
                        tbHR4.Text = listAllEmployee[candidateDetailGetJob.success.data.data.detail_candidate.user_hiring];
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
                }


                if (candidateDetailGetJob.success.data.data.detail_contact_job != null)
                {
                    tbResiredSalary.Text = candidateDetailGetJob.success.data.data.detail_contact_job.resired_salary + " VNĐ";
                    tbSalary.Text = candidateDetailGetJob.success.data.data.detail_contact_job.salary + " VNĐ";
                    tbNote.Text = candidateDetailGetJob.success.data.data.detail_contact_job.note;
                    string date = candidateDetailGetJob.success.data.data.detail_contact_job.offer_time;
                    if (ConvertDate(date, "dd/MM/yyyy") != "")
                    {
                        tbSwitch.Text = ConvertDate(date, "dd/MM/yyyy");
                    }

                    tbHR4.Text = listAllEmployee[candidateDetailGetJob.success.data.data.detail_contact_job.ep_offer];
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
            EditContractPopup editContractPopup = new EditContractPopup(token, listAllEmployee, listRecruitmentNew, candidateDetailGetJob,listItem,id);
            onShowPopup(editContractPopup);
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
