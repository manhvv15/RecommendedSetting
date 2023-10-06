using QuanLyChung365TruocDangNhap.Hr.Entities.ManageRecuitmentEntities.ListCandidateEntities;
using QuanLyChung365TruocDangNhap.Hr.Popups.RecruitmentManagementPopups.ListOfCandidatesPopups;
using QuanLyChung365TruocDangNhap.Hr.Popups.RecruitmentManagementPopups.ListOfCandidatesPopups.CandidateDetailPopups;
using QuanLyChung365TruocDangNhap.Hr.View;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
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
    /// Interaction logic for CandidateDetailProcessInterviewPage.xaml
    /// </summary>
    public partial class CandidateDetailProcessInterviewPage : Page
    {
        string id;
        string token;
        string process_id;
        string url_cv = "";
        string name_process;
        CandidateDetailEntity candidateDetailGetJob;
        Dictionary<string, string> listAllEmployee = new Dictionary<string, string>();
        Dictionary<string, string> listRecruitmentNew = new Dictionary<string, string>();
        // deligate event show popups
        public delegate void ShowPopup(object obj);
        public static event ShowPopup onShowPopup;
        List<Items> listItem = new List<Items>();
        public CandidateDetailProcessInterviewPage(string token, string id, Dictionary<string, string> listAllEmployee, string process_id, string name_process, Dictionary<string, string> listRecruitmentNew, bool perEdit, List<Items> listItem)
        {
            InitializeComponent();
            this.token = token;
            this.id = id;
            this.listAllEmployee = listAllEmployee;
            this.process_id = process_id;
            this.name_process = name_process;
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
            EditYourResumePopup.hidePopup += HidePopup;
            CvCandidatePopup.hidePopup += HidePopup;
        }



        private async void GetData()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.detail_candidate_process;

                httpRequestMessage.RequestUri = new Uri(api);

                var parameters = new List<KeyValuePair<string, string>>();

                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                parameters.Add(new KeyValuePair<string, string>("id", id));
                //parameters.Add(new KeyValuePair<string, string>("process_id", process_id));

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
            //tbNameProcess.Text = name_process;

            if (candidateDetailGetJob.success.data.data != null)
            {
                if (candidateDetailGetJob.success.data.data.detail_candidate != null)
                {

                    tbNameTitle.Text = candidateDetailGetJob.success.data.data.detail_candidate.name; //Tên chi tiết giai đoạn ứng viên
                    //Thông tin ứng viên
                    tbID.Text = candidateDetailGetJob.success.data.data.detail_candidate.id;
                    tbName.Text = candidateDetailGetJob.success.data.data.detail_candidate.name;
                    tbEmail.Text = candidateDetailGetJob.success.data.data.detail_candidate.email;
                    tbPhone.Text = candidateDetailGetJob.success.data.data.detail_candidate.phone;
                    tbGender.Text = candidateDetailGetJob.success.data.data.detail_candidate.can_gender;
                    string dateOfBirth = candidateDetailGetJob.success.data.data.detail_candidate.can_birthday;
                    if (ConvertDate(dateOfBirth, "dd/MM/yyyy") != "")
                    {
                        tbDateOfBirth.Text = ConvertDate(dateOfBirth, "dd/MM/yyyy");
                    }

                    //quê quán
                    if (candidateDetailGetJob.success.data.data.detail_candidate.hometown == "")
                    {
                        tbHomeTown.Text = "Chưa cập nhật";
                    }
                    else
                    {
                        tbHomeTown.Text = candidateDetailGetJob.success.data.data.detail_candidate.hometown;
                    }

                    //Trình độ học vấn
                    if (candidateDetailGetJob.success.data.data.detail_candidate.can_education == "")
                    {
                        tbEducation.Text = "Chưa cập nhật";
                    }
                    else
                    {
                        tbEducation.Text = candidateDetailGetJob.success.data.data.detail_candidate.can_education;

                    }

                    if (candidateDetailGetJob.success.data.data.detail_candidate.school == "")
                    {
                        tbSchool.Text = "Chưa cập nhật";
                    }
                    else
                    {
                        tbSchool.Text = candidateDetailGetJob.success.data.data.detail_candidate.school;
                    }
                    tbExp.Text = candidateDetailGetJob.success.data.data.detail_candidate.can_exp;
                    tbMarried.Text = candidateDetailGetJob.success.data.data.detail_candidate.can_is_married;
                    tbAddress.Text = candidateDetailGetJob.success.data.data.detail_candidate.can_address;
                    //end


                    //Thông tin tuyển dụng
                    tbCvFrom.Text = candidateDetailGetJob.success.data.data.detail_candidate.cv_from;
                    tbNewTitle.Text = candidateDetailGetJob.success.data.data.detail_candidate.new_title;
                    if (listAllEmployee.ContainsKey(candidateDetailGetJob.success.data.data.detail_candidate.user_hiring))
                    {
                        tbHR1.Text = listAllEmployee[candidateDetailGetJob.success.data.data.detail_candidate.user_hiring];
                    }
                    if (listAllEmployee.ContainsKey(candidateDetailGetJob.success.data.data.detail_candidate.user_hiring))
                    {
                        tbHR2.Text = listAllEmployee[candidateDetailGetJob.success.data.data.detail_candidate.user_hiring];
                    }
                    //end

                    //Giai đoạn chuyển
                    int index = listItem.FindIndex(item => item.ID == candidateDetailGetJob.success.data.data.detail_interview.process_interview_id);
                    if (index != -1)
                    {
                        tbGiaiDoanChuyen.Text = listItem[index].Name;
                    }

                    string date = candidateDetailGetJob.success.data.data.detail_candidate.created_at;
                    if (ConvertDate(date, "dd/MM/yyyy") != "")
                    {
                        tbTimeSendCV.Text = ConvertDate(date, "dd/MM/yyyy");
                    }
                    gridStar.DataContext = candidateDetailGetJob.success.data.data.detail_candidate;
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
                if (candidateDetailGetJob.success.data.data.detail_interview != null)
                {
                    if (candidateDetailGetJob.success.data.data.detail_interview.resired_salary == "")
                    {
                        tbResiredSalary.Text = "Chưa cập nhật";
                    }
                    else
                    {
                        tbResiredSalary.Text = candidateDetailGetJob.success.data.data.detail_interview.resired_salary + " VNĐ";
                    }

                    if (candidateDetailGetJob.success.data.data.detail_interview.salary == "")
                    {
                        tbSalary.Text = "Chưa cập nhật";
                    }
                    else
                    {
                        tbSalary.Text = candidateDetailGetJob.success.data.data.detail_interview.salary + " VNĐ";
                    }


                    if (candidateDetailGetJob.success.data.data.detail_interview.note == "")
                    {
                        tbNote.Text = "Chưa cập nhật";
                    }
                    else
                    {
                        tbNote.Text = candidateDetailGetJob.success.data.data.detail_interview.note;
                    }
                    string date = candidateDetailGetJob.success.data.data.detail_interview.created_at;
                    if (ConvertDate(date, "dd/MM/yyyy") != "")
                    {
                        tbSwitch.Text = ConvertDate(date, "dd/MM/yyyy");
                    }

                    date = candidateDetailGetJob.success.data.data.detail_interview.interview_time;
                    if (ConvertDate(date, "HH:mm dd/MM/yyyy") != "")
                    {
                        tbTimeInterview.Text = ConvertDate(date, "HH:mm dd/MM/yyyy");
                    }
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
        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            MessageBox.Show("Download completed!");
            string cv = candidateDetailGetJob.success.data.data.detail_candidate.cv;
            CvCandidatePopup editCandidateGetJobPopup = new CvCandidatePopup(token, id, url_cv, cv);
            onShowPopup(editCandidateGetJobPopup);
        }
        private void ViewFileCandidate(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = url_cv,
                UseShellExecute = true
            });

            //string cv = candidateDetailGetJob.success.data.data.detail_candidate.cv;
            //Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            //dlg.FileName = cv; // Default file name
            //dlg.DefaultExt = ".png"; // Default file extension
            //dlg.Filter = "PNG|*.png"; // Filter files by extension

            //// Show save file dialog box
            ////Nullable<bool> result = dlg
            //string curFile = @"C:\Users\Nguyen Ngoc Nhat\Downloads\";

            //bool exist = Directory.Exists(curFile);

            //if (!exist)
            //{
            //    File.Delete(cv);

            //}
            //else
            //{

            //    // Process save file dialog box results
            //    WebClient webClient = new WebClient();
            //    webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
            //    //webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
            //    webClient.DownloadFileAsync(new Uri(url_cv),
            //        @"C:\Users\Nguyen Ngoc Nhat\Downloads\" + cv);
            //    // Save document
            //    string filename = dlg.FileName;
            //}
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
            EditYourResumePopup editYourResumePopup = new EditYourResumePopup(token, listAllEmployee, listRecruitmentNew, candidateDetailGetJob);
            onShowPopup(editYourResumePopup);
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
