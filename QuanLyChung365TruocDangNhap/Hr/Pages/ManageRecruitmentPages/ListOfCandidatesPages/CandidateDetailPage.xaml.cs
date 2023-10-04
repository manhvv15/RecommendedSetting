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
    /// Interaction logic for CandidateDetailPage.xaml
    /// </summary>
    public partial class CandidateDetailPage : Page
    {
        string token;
        string id;
        CandidateDetailEntity candidateDetailEntity;
        Dictionary<string, string> listAllEmployee = new Dictionary<string, string>();
        Dictionary<string, string> listRecruitmentNew = new Dictionary<string, string>();
        string url_cv = "";

        // deligate event show popups
        public delegate void ShowPopup(object obj);
        public static event ShowPopup onShowPopup;

        public CandidateDetailPage(string token, string id, Dictionary<string, string> listAllEmployee, Dictionary<string, string> listRecruitmentNew, bool perEdit)
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
            GetData();

            EditCandidateProfilePopup2.hidePopup += HidePopup;
        }

        private async void GetData()
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
                parameters.Add(new KeyValuePair<string, string>("id", id));

                // Thiết lập Content
                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                candidateDetailEntity = JsonConvert.DeserializeObject<CandidateDetailEntity>(responseContent);

                BindingData();

            }
            catch
            {
                //MessageBox.Show("Lỗi đăng nhập, vui lòng thử lại!");
            }
        }
        //Lưu dữ liệu 
        private void BindingData()
        {
            if (candidateDetailEntity.success == null || candidateDetailEntity.success.data == null || candidateDetailEntity.success.data.data == null) return;
            try
            {
                tbNameTitle.Text = candidateDetailEntity.success.data.data.name; //tên chi tiết ứng viên giai đoạn nhận hồ sơ
                //Thông tin ứng viên
                tbName.Text = candidateDetailEntity.success.data.data.name;
                tbID.Text = candidateDetailEntity.success.data.data.id;
                tbEmail.Text = candidateDetailEntity.success.data.data.email;
                tbPhone.Text = candidateDetailEntity.success.data.data.phone;
                tbSex.Text = candidateDetailEntity.success.data.data.can_gender;
                tbHometown.Text = candidateDetailEntity.success.data.data.hometown;
                tbSchool.Text = candidateDetailEntity.success.data.data.school;
                if(ConvertDate(candidateDetailEntity.success.data.data.can_birthday, "dd/MM/yyyy") != "")
                {
                    tbDateOfBirth.Text = ConvertDate(candidateDetailEntity.success.data.data.can_birthday, "dd/MM/yyyy");
                }
                if (candidateDetailEntity.success.data.data.hometown == null)
                {
                    tbHometown.Text = "Chưa cập nhật";
                }
                else
                {
                    tbHometown.Text = candidateDetailEntity.success.data.data.hometown;
                }
                tbEducation.Text = candidateDetailEntity.success.data.data.can_education;
                if (candidateDetailEntity.success.data.data.school == null)
                {
                    tbSchool.Text = "Chưa cập nhật";
                }
                else
                {
                    tbSchool.Text = candidateDetailEntity.success.data.data.school;

                }
                tbExp.Text = candidateDetailEntity.success.data.data.can_exp;
                tbMarried.Text = candidateDetailEntity.success.data.data.can_is_married;
                tbAddress.Text = candidateDetailEntity.success.data.data.can_address;
                //end
                

                //Thông tin tuyển dụng
                tbCvFrom.Text = candidateDetailEntity.success.data.data.cv_from;
                tbTitle.Text = candidateDetailEntity.success.data.data.new_title;
                tbHR.Text = listAllEmployee[candidateDetailEntity.success.data.data.user_hiring];
                //end

                //Quá trình tuyển dụng
                if (ConvertDate(candidateDetailEntity.success.data.data.created_at, "dd/MM/yyyy") != "")
                {
                    tbTimeSendCV.Text = ConvertDate(candidateDetailEntity.success.data.data.created_at, "dd/MM/yyyy");
                }
                gridStar.DataContext = candidateDetailEntity.success.data.data;
                //end

                icSkillVote.ItemsSource = candidateDetailEntity.success.data.list_skill;
                string cv = candidateDetailEntity.success.data.data.cv;
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
            catch
            {
                MessageBox.Show("Có lỗi xảy ra,vui lòng kiểm tra lại!");
            }
            
        }

        //Định dạng datetime
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

        private void HidePopup(int mode)
        {
            if (mode == 1)
            {
                GetData();
            }
            onShowPopup("");
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

        private void ViewFileCandidate(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = url_cv,
                UseShellExecute = true
            });
        }

        private void ShowPopupEdit(object sender, MouseButtonEventArgs e)
        {
            EditCandidateProfilePopup2 editCandidateProfilePopup = new EditCandidateProfilePopup2(token, listAllEmployee, listRecruitmentNew, candidateDetailEntity);
            onShowPopup(editCandidateProfilePopup);
        }
    }
}
