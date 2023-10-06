using QuanLyChung365TruocDangNhap.Hr.Entities.ListItemCombobox;
using QuanLyChung365TruocDangNhap.Hr.Entities.ManageRecuitmentEntities.ListCandidateEntities;
using QuanLyChung365TruocDangNhap.Hr.Pages.ManageRecruitmentPages.ListOfCandidatesPages;
using Microsoft.Office.Interop.Excel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
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

namespace QuanLyChung365TruocDangNhap.Hr.Popups.RecruitmentManagementPopups.ListOfCandidatesPopups.CandidateDetailPopups
{
    /// <summary>
    /// Interaction logic for EditCandidateFailJobPopup.xaml
    /// </summary>
    public partial class EditCandidateFailJobPopup : UserControl
    {
        string token;
        string id;
        CandidateDetailEntity candidateDetailEntity;
        public Dictionary<string, string> listAllEmployee = new Dictionary<string, string>();
        // danh sách vị trí tuyển dụng
        public Dictionary<string, string> listRecruitment = new Dictionary<string, string>();
        int first_star_vote = 0;
        //int skill_vote = 0;

        List<Skill> listSkill = new List<Skill>();
        List<Items> listItem = new List<Items>();
        // deligate event hide popups
        public delegate void HidePopup(int mode);
        public static event HidePopup hidePopup;

        public EditCandidateFailJobPopup(string token, Dictionary<string, string> listAllEmployee, Dictionary<string, string> listRecruitment, CandidateDetailEntity candidateDetailEntity, List<Items> listItem,string id)
        {
            InitializeComponent();
            this.token = token;
            this.listAllEmployee = listAllEmployee;
            this.listRecruitment = listRecruitment;
            this.candidateDetailEntity = candidateDetailEntity;
            this.listItem = listItem;
            this.id = id;
            SetUpCombobox();
            BindingData();
            DisplayFirstStarVote(first_star_vote);
        }

        private void SetUpCombobox()
        {
            cbxGender.ItemsSource = ListItemCombobox.ListCbxGender;
            cbxEducation.ItemsSource = ListItemCombobox.ListCbxEducation;
            cbxExp.ItemsSource = ListItemCombobox.ListCbxExp;
            cbxMarried.ItemsSource = ListItemCombobox.ListMarried;
            cbxRecruitment.ItemsSource = listAllEmployee;
            cbxRecommend.ItemsSource = listAllEmployee;
            cbxPosition.ItemsSource = listRecruitment;
        }

        private void BindingData()
        {
            if (candidateDetailEntity.success == null || candidateDetailEntity.success.data == null || candidateDetailEntity.success.data.data.detail_candidate == null) return;
            try
            {
                tbName.Text = candidateDetailEntity.success.data.data.detail_candidate.name;
                tbEmail.Text = candidateDetailEntity.success.data.data.detail_candidate.email;
                if (ConvertDate(candidateDetailEntity.success.data.data.detail_candidate.can_birthday, "MM/dd/yyyy") != "")
                {
                    dpDateOfBirth.Text = ConvertDate(candidateDetailEntity.success.data.data.detail_candidate.can_birthday, "MM/dd/yyyy");
                    dpDateOfBirth.SelectedDate = DateTime.Parse(ConvertDate(candidateDetailEntity.success.data.data.detail_candidate.can_birthday, "MM/dd/yyyy"));
                }
                tbPhone.Text = candidateDetailEntity.success.data.data.detail_candidate.phone;
                tbHomeTown.Text = candidateDetailEntity.success.data.data.detail_candidate.hometown;
                tbSchool.Text = candidateDetailEntity.success.data.data.detail_candidate.school;
                tbAddress.Text = candidateDetailEntity.success.data.data.detail_candidate.can_address;
                tbCVFrom.Text = candidateDetailEntity.success.data.data.detail_candidate.cv_from;
                cbxExp.SelectedItem = ListItemCombobox.ListCbxExp.FirstOrDefault(t => t.value == candidateDetailEntity.success.data.data.detail_candidate.can_exp);
                cbxEducation.SelectedItem = ListItemCombobox.ListCbxEducation.FirstOrDefault(t => t.value == candidateDetailEntity.success.data.data.detail_candidate.can_education);
                cbxMarried.SelectedItem = ListItemCombobox.ListMarried.FirstOrDefault(t => t.value == candidateDetailEntity.success.data.data.detail_candidate.can_is_married);
                cbxGender.SelectedItem = ListItemCombobox.ListCbxGender.FirstOrDefault(t => t.value == candidateDetailEntity.success.data.data.detail_candidate.can_gender);
                if (ConvertDate(candidateDetailEntity.success.data.data.detail_candidate.time_send_cv, "MM/dd/yyyy") != "")
                {
                    dpTimeSendCV.Text = ConvertDate(candidateDetailEntity.success.data.data.detail_candidate.time_send_cv, "MM/dd/yyyy");
                    dpTimeSendCV.SelectedDate = DateTime.Parse(ConvertDate(candidateDetailEntity.success.data.data.detail_candidate.time_send_cv, "MM/dd/yyyy"));
                }

                tbNote.Text = candidateDetailEntity.success.data.data.detail_fail_job.note;
                cbxPosition.SelectedItem = listRecruitment.FirstOrDefault(t => t.Key == candidateDetailEntity.success.data.data.detail_candidate.recruitment_news_id);
                cbxRecruitment.SelectedItem = listAllEmployee.FirstOrDefault(t => t.Key == candidateDetailEntity.success.data.data.detail_candidate.user_hiring);
                cbxRecommend.SelectedItem = listAllEmployee.FirstOrDefault(t => t.Key == candidateDetailEntity.success.data.data.detail_candidate.user_recommend);
                Int32.TryParse(candidateDetailEntity.success.data.data.detail_candidate.star_vote, out first_star_vote);

                listSkill = candidateDetailEntity.success.data.list_skill;
                icListSkill.ItemsSource = listSkill;
                GetData();
            }
            catch
            {

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

        private void DisplayFirstStarVote(int star)
        {
            var converter = new BrushConverter();
            Brush brush = (Brush)converter.ConvertFromString("#FDBE1E");
            switch (star)
            {
                case 0:
                    star1.Fill = null;
                    star2.Fill = null;
                    star3.Fill = null;
                    star4.Fill = null;
                    star5.Fill = null;
                    break;
                case 1:
                    star1.Fill = brush;
                    star2.Fill = null;
                    star3.Fill = null;
                    star4.Fill = null;
                    star5.Fill = null;
                    break;
                case 2:
                    star1.Fill = brush;
                    star2.Fill = brush;
                    star3.Fill = null;
                    star4.Fill = null;
                    star5.Fill = null;
                    break;
                case 3:
                    star1.Fill = brush;
                    star2.Fill = brush;
                    star3.Fill = brush;
                    star4.Fill = null;
                    star5.Fill = null;
                    break;
                case 4:
                    star1.Fill = brush;
                    star2.Fill = brush;
                    star3.Fill = brush;
                    star4.Fill = brush;
                    star5.Fill = null;
                    break;
                case 5:
                    star1.Fill = brush;
                    star2.Fill = brush;
                    star3.Fill = brush;
                    star4.Fill = brush;
                    star5.Fill = brush;
                    break;
            }
        }

        private void CancelPopup(object sender, MouseButtonEventArgs e)
        {
            hidePopup(0);
        }

        private void ClickUpdate(object sender, MouseButtonEventArgs e)
        {
            if (ValidateForm())
                UpdateCandidate();
        }

        private async void UpdateCandidate()
        {
            try
            {
                var client = new HttpClient();
                var multiForm = new MultipartFormDataContent();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                multiForm.Add(new StringContent(candidateDetailEntity.success.data.data.detail_candidate.id), "canId");
                multiForm.Add(new StringContent("2"), "type");
                multiForm.Add(new StringContent(candidateDetailEntity.success.data.data.detail_candidate.email), "email");
                if(string.IsNullOrEmpty(candidateDetailEntity.success.data.data.detail_candidate.contentsend))
                    multiForm.Add(new StringContent("1"), "contentsend");
                else
                    multiForm.Add(new StringContent(candidateDetailEntity.success.data.data.detail_candidate.contentsend), "contentsend");
                multiForm.Add(new StringContent(candidateDetailEntity.success.data.data.detail_candidate.name), "name");
                multiForm.Add(new StringContent(candidateDetailEntity.success.data.data.detail_candidate.cv_from), "cvFrom");
                multiForm.Add(new StringContent(candidateDetailEntity.success.data.data.detail_candidate.user_hiring), "userHiring");
                multiForm.Add(new StringContent(candidateDetailEntity.success.data.data.detail_candidate.time_send_cv), "timeSendCv");
                multiForm.Add(new StringContent(candidateDetailEntity.success.data.data.detail_candidate.recruitment_news_id), "recruitmentNewsId");
                multiForm.Add(new StringContent(candidateDetailEntity.success.data.data.detail_candidate.star_vote), "starVote");
                multiForm.Add(new StringContent(tbNote.Text), "note");

                // send request to API
                var url = APIs.API.edit_candidate_fail_job;
                var response = await client.PostAsync(url, multiForm);
                var responseContent = await response.Content.ReadAsStringAsync();
                ProcessEntity result = JsonConvert.DeserializeObject<ProcessEntity>(responseContent);
                if (result.data.result)
                {
                    hidePopup(1);
                    MessageBox.Show("Cập nhật thành công!");
                }
                else
                {
                    MessageBox.Show(result.error.message);
                }
            }
            catch
            {
                MessageBox.Show("Có lỗi xảy ra, vui lòng thử lại!");
            }
        }

        private bool ValidateForm()
        {
            return true;
        }

        private async void GetData()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.detail_candidate_fail_job;
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


                if (candidateDetailEntity.success.data.data != null)
                {
                    GetListSkill();
                    List<ListSchedule> listSchedules = candidateDetailEntity.success.data.data.list_schedule;
                    if (candidateDetailEntity.success.data.data.list_schedule != null)
                    {
                        icGiaiDoanChuyen.ItemsSource = candidateDetailEntity.success.data.data.list_schedule;
                        foreach (var item in listSchedules)
                        {
                            int index = listItem.FindIndex(t => t.ID == item.process_interview_id);
                            if (index != -1)
                            {
                                item.process_interview_id = listItem[index].Name;
                            }
                            item.interview_time = ConvertDate(item.interview_time, "dd/MM/yyyy HH:mm ");
                        }
                    }
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
                    candidateDetailEntity.success.data.list_skill = candidateDetailEntity.success.data.list_skill;
                    icListSkill.ItemsSource = candidateDetailEntity.success.data.list_skill;
                }
            }
            catch
            {
                //MessageBox.Show("Lỗi đăng nhập, vui lòng thử lại!");
            }
        }
    }
}
