﻿using QuanLyChung365TruocDangNhap.Hr.Entities.ListItemCombobox;
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
    /// Interaction logic for EditCandidateGetJobPopup.xaml
    /// </summary>
    public partial class EditCandidateGetJobPopup : UserControl
    {
        string token;
        string id;
        CandidateDetailEntity candidateDetailEntity;
        public Dictionary<string, string> listAllEmployee = new Dictionary<string, string>();
        // danh sách vị trí tuyển dụng
        public Dictionary<string, string> listRecruitment = new Dictionary<string, string>();
        int first_star_vote = 0;
        //int skill_vote = 0;
        List<Items> listItem = new List<Items>();
        List<Skill> listSkill = new List<Skill>();
        // deligate event hide popups
        public delegate void HidePopup(int mode);
        public static event HidePopup hidePopup;

        public EditCandidateGetJobPopup(string token, Dictionary<string, string> listAllEmployee, Dictionary<string, string> listRecruitment, CandidateDetailEntity candidateDetailEntity, List<Items> listItem,string id)
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
            cbxInterview.ItemsSource = listAllEmployee;
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
                tbNote.Text = candidateDetailEntity.success.data.data.detail_get_job.note;
                tbResiredSalary.Text = candidateDetailEntity.success.data.data.detail_get_job.resired_salary;
                tbSalary.Text = candidateDetailEntity.success.data.data.detail_get_job.salary;
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
                    dpTimeSendCV.SelectedDate = DateTime.Parse(/*ConvertDate(*/candidateDetailEntity.success.data.data.detail_candidate.time_send_cv/*, "MM/dd/yyyy")*/);
                }
                if (ConvertDate(candidateDetailEntity.success.data.data.detail_get_job.created_at, "MM/dd/yyyy") != "")
                {
                    dpTimeSwitch.Text = ConvertDate(candidateDetailEntity.success.data.data.detail_get_job.created_at, "MM/dd/yyyy");
                    dpTimeSwitch.SelectedDate = DateTime.Parse(/*ConvertDate(*/candidateDetailEntity.success.data.data.detail_get_job.created_at/*, "MM/dd/yyyy")*/);
                }
                if (ConvertDate(candidateDetailEntity.success.data.data.detail_get_job.interview_time, "MM/dd/yyyy") != "")
                {
                    dpDate.Text = ConvertDate(candidateDetailEntity.success.data.data.detail_get_job.interview_time, "MM/dd/yyyy");
                    dpDate.SelectedDate = DateTime.Parse(/*ConvertDate(*/candidateDetailEntity.success.data.data.detail_get_job.interview_time/*, "MM/dd/yyyy")*/);
                }

                cbxPosition.SelectedItem = listRecruitment.FirstOrDefault(t => t.Key == candidateDetailEntity.success.data.data.detail_candidate.recruitment_news_id);
                cbxRecruitment.SelectedItem = listAllEmployee.FirstOrDefault(t => t.Key == candidateDetailEntity.success.data.data.detail_candidate.user_hiring);
                cbxRecommend.SelectedItem = listAllEmployee.FirstOrDefault(t => t.Key == candidateDetailEntity.success.data.data.detail_candidate.user_recommend);
                cbxInterview.SelectedItem = listAllEmployee.FirstOrDefault(t => t.Key == candidateDetailEntity.success.data.data.detail_get_job.ep_interview);
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
                multiForm.Add(new StringContent(tbResiredSalary.Text), "resiredSalary");
                multiForm.Add(new StringContent(tbSalary.Text), "salary");
                multiForm.Add(new StringContent(dpDate.SelectedDate.Value.ToString("yyyy-MM-dd")), "interviewTime");
                multiForm.Add(new StringContent(cbxInterview.SelectedValue.ToString()), "empInterview");
                multiForm.Add(new StringContent(tbNote.Text), "note");
                multiForm.Add(new StringContent(candidateDetailEntity.success.data.data.detail_candidate.email), "email");
                multiForm.Add(new StringContent(candidateDetailEntity.success.data.data.detail_candidate.name), "name");
                multiForm.Add(new StringContent(first_star_vote.ToString()), "starVote");
                multiForm.Add(new StringContent(candidateDetailEntity.success.data.data.detail_candidate.recruitment_news_id), "recruitmentNewsId");
                multiForm.Add(new StringContent(candidateDetailEntity.success.data.data.detail_candidate.cv_from), "cvFrom");
                multiForm.Add(new StringContent(candidateDetailEntity.success.data.data.detail_candidate.time_send_cv), "timeSendCv");
                multiForm.Add(new StringContent(candidateDetailEntity.success.data.data.detail_candidate.user_hiring), "userHiring");
                multiForm.Add(new StringContent("0"), "checkEmail");
                if(!string.IsNullOrEmpty(candidateDetailEntity.success.data.data.detail_candidate.contentsend))
                    multiForm.Add(new StringContent(candidateDetailEntity.success.data.data.detail_candidate.contentsend), "contentsend");
                else
                    multiForm.Add(new StringContent("1"), "contentsend");
                // send request to API
                var url = APIs.API.edit_candidate_get_job;
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
            if (tbResiredSalary.Text == "")
            {
                MessageBox.Show("Vui lòng điền mức lương mong muốn.");
                return false;
            }

            if (tbSalary.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng điền mức lương thực.");
                return false;
            }

            if (cbxInterview.SelectedIndex == -1 || cbxInterview.SelectedValue.ToString() == "")
            {
                MessageBox.Show("Vui lòng chọn nhân viên tham gia.");
                return false;
            }
            if (dpDate.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng chọn thời gian hẹn.");
                return false;
            }

            if(tbPhone.Text.Length > 15)
            {
                MessageBox.Show("SĐT không hợp lệ!");
                return false;
            }

            return true;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private bool ValidateEmail()
        {
            string email = tbEmail.Text;
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);
            if (match.Success)
                return true;
            else
                return false;
        }

        private bool ValidatePhoneNumber()
        {
            string email = tbPhone.Text;
            Regex regex = new Regex(@"^([0-9]{9,})$");
            Match match = regex.Match(email);
            if (match.Success)
                return true;
            else
                return false;
        }

        private void cbxInterview_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            cbxInterview.SelectedIndex = -1;
            string textSearch = cbxInterview.Text + e.Text;
            cbxInterview.IsDropDownOpen = true;
            if (textSearch == "")
            {
                cbxInterview.Text = "";
                cbxInterview.Items.Refresh();
                cbxInterview.ItemsSource = listAllEmployee;
                cbxInterview.SelectedIndex = -1;
            }
            else
            {
                cbxInterview.ItemsSource = "";
                cbxInterview.Items.Refresh();
                cbxInterview.ItemsSource = listAllEmployee.Where(t => t.Value.ToLower().Contains(textSearch.ToLower()));
            }
        }

        private void cbxInterview_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                cbxInterview.SelectedIndex = -1;
                string textSearch = cbxInterview.Text;
                cbxInterview.Items.Refresh();
                cbxInterview.ItemsSource = listAllEmployee.Where(t => t.Value.ToLower().Contains(textSearch.ToLower()));
            }
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