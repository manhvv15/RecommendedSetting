using QuanLyChung365TruocDangNhap.Hr.Entities.ManageRecuitmentEntities.ListCandidateEntities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
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
using static System.Net.WebRequestMethods;

namespace QuanLyChung365TruocDangNhap.Hr.Popups.RecruitmentManagementPopups.ListOfCandidatesPopups.TransportPopups
{
    /// <summary>
    /// Interaction logic for CancelJobPopup.xaml
    /// </summary>
    public partial class CancelJobPopup : UserControl
    {
        string token;
        string id_candidate;
        string id_process_old; // id gđ cũ
        string id_process_new;
        CandidateDetailEntity candidateDetailEntity;
        public Dictionary<string, string> listAllEmployee = new Dictionary<string, string>();
        // danh sách vị trí tuyển dụng
        public Dictionary<string, string> listRecruitment = new Dictionary<string, string>();
        public Dictionary<string, string> listStatus = new Dictionary<string, string>();
        int first_star_vote = 0;
        //int skill_vote = 0;

        List<Skill> listSkill = new List<Skill>();
        int stt_skill = 0;

        // deligate event hide popups
        public delegate void HidePopup(int mode, int process_id_old, int process_id_new);
        public static event HidePopup hidePopup;
        public CancelJobPopup(string token, string id_candidate, Dictionary<string, string> listAllEmployee, Dictionary<string, string> listRecruitment, string id_process_old, string id_process_new)
        {
            InitializeComponent();
            this.token = token;
            this.id_candidate = id_candidate;
            this.listAllEmployee = listAllEmployee;
            this.listRecruitment = listRecruitment;
            this.id_process_old = id_process_old;
            this.id_process_new = id_process_new;
            cbxPosition.ItemsSource = listRecruitment;
            cbxHiring.ItemsSource = listAllEmployee;
            listStatus.Add("1", "Hủy phỏng vấn");
            listStatus.Add("2", "Hủy nhận việc");
            listStatus.Add("3", "Hủy học việc");
            cbxStatus.ItemsSource = listStatus;
            GetData();


            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy"; //for the second type
            Thread.CurrentThread.CurrentCulture = ci;
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
                parameters.Add(new KeyValuePair<string, string>("canId", id_candidate));

                // Thiết lập Content
                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                candidateDetailEntity = JsonConvert.DeserializeObject<CandidateDetailEntity>(responseContent);

                if (candidateDetailEntity.result)
                {
                    BindingData();
                }
            }
            catch
            {
                //MessageBox.Show("Lỗi đăng nhập, vui lòng thử lại!");
            }
        }

        private void BindingData()
        {
            if (candidateDetailEntity.success == null || candidateDetailEntity.success.data == null || candidateDetailEntity.success.data.data == null) return;
            try
            {
                tbName.Text = candidateDetailEntity.success.data.data.name;
                tbCVFrom.Text = candidateDetailEntity.success.data.data.cv_from;
                if (ConvertDate(candidateDetailEntity.success.data.data.time_send_cv, "dd/MM/yyyy") != "")
                {
                    dpTimeSendCV.Text = ConvertDate(candidateDetailEntity.success.data.data.time_send_cv, "dd/MM/yyyy");
                    dpTimeSendCV.SelectedDate = DateTime.Parse(ConvertDate(candidateDetailEntity.success.data.data.time_send_cv, "dd/MM/yyyy"));
                }

                cbxPosition.SelectedItem = listRecruitment.FirstOrDefault(t => t.Key == candidateDetailEntity.success.data.data.recruitment_news_id);
                cbxHiring.SelectedItem = listAllEmployee.FirstOrDefault(t => t.Key == candidateDetailEntity.success.data.data.user_hiring);

                Int32.TryParse(candidateDetailEntity.success.data.data.star_vote, out first_star_vote);
                DisplayFirstStarVote(first_star_vote);
                listSkill = candidateDetailEntity.success.data.list_skill;
                icListSkill.ItemsSource = listSkill;
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

        private void SetStar(object sender, MouseButtonEventArgs e)
        {
            Grid grid = sender as Grid;
            string name = grid.Name;
            switch (name)
            {
                case "gstar1":
                    first_star_vote = 1;
                    break;
                case "gstar2":
                    first_star_vote = 2;
                    break;
                case "gstar3":
                    first_star_vote = 3;
                    break;
                case "gstar4":
                    first_star_vote = 4;
                    break;
                case "gstar5":
                    first_star_vote = 5;
                    break;
            }
            DisplayFirstStarVote(first_star_vote);
        }

        private void MouseMoveFirstStar(object sender, MouseEventArgs e)
        {
            Grid grid = sender as Grid;
            string name = grid.Name;
            switch (name)
            {
                case "gstar1":
                    DisplayFirstStarVote(1);
                    break;
                case "gstar2":
                    DisplayFirstStarVote(2);
                    break;
                case "gstar3":
                    DisplayFirstStarVote(3);
                    break;
                case "gstar4":
                    DisplayFirstStarVote(4);
                    break;
                case "gstar5":
                    DisplayFirstStarVote(5);
                    break;
            }
        }

        private void MouseLeaveFirstStar(object sender, MouseEventArgs e)
        {
            DisplayFirstStarVote(first_star_vote);
        }

        private void AddNewSkill(object sender, MouseButtonEventArgs e)
        {
            Skill skill = new Skill() { skill_name = "", skill_vote = "0" };
            skill.stt_skill = stt_skill;
            stt_skill++;
            listSkill.Add(skill);
            icListSkill.Items.Refresh();
            icListSkill.ItemsSource = listSkill;
        }

        private void DeleteSkill(object sender, MouseButtonEventArgs e)
        {
            Grid grid = sender as Grid;
            Skill skill = (Skill)grid.DataContext;
            int stt = skill.stt_skill;
            foreach (var item in listSkill)
            {
                if (item.stt_skill == stt)
                {
                    listSkill.Remove(item);
                    icListSkill.Items.Refresh();
                    icListSkill.ItemsSource = listSkill;
                    break;
                }
            }
        }

        private void SetStarSkill(object sender, MouseButtonEventArgs e)
        {
            tbForcus.Focus();
            Grid grid = sender as Grid;
            Skill skill = (Skill)grid.DataContext;
            string name = grid.Name;
            switch (name)
            {
                case "gsstar1":
                    skill.skill_vote = "1";
                    break;
                case "gsstar2":
                    skill.skill_vote = "2";
                    break;
                case "gsstar3":
                    skill.skill_vote = "3";
                    break;
                case "gsstar4":
                    skill.skill_vote = "4";
                    break;
                case "gsstar5":
                    skill.skill_vote = "5";
                    break;

            }
            icListSkill.Items.Refresh();
            icListSkill.ItemsSource = listSkill;
        }

        private void CancelPopup(object sender, MouseButtonEventArgs e)
        {
            hidePopup(0, 0, 0);
        }

        private void ClickTransport(object sender, MouseButtonEventArgs e)
        {
            tbForcus.Focus();
            if (ValidateForm())
                Transport();
        }

        private async void Transport()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = "";
                var parameters = new List<KeyValuePair<string, string>>();
                if (id_process_new == "1")
                {
                    api = APIs.API.addCandidateGetJob;
                }
                else if (id_process_new == "2")
                {
                    api = APIs.API.addCandidateFailJob;
                    parameters.Add(new KeyValuePair<string, string>("type", "2"));
                }
                else if (id_process_new == "3")
                {
                    api = APIs.API.addCandidateCancelJob;
                }
                else if (id_process_new == "4")
                {
                    api = APIs.API.addCandidateContactJob;
                }
                else
                {
                    api = APIs.API.addCandidateProcessInterview;
                }
                httpRequestMessage.RequestUri = new Uri(api);


                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                parameters.Add(new KeyValuePair<string, string>("canId", id_candidate));
                parameters.Add(new KeyValuePair<string, string>("name", tbName.Text));
                parameters.Add(new KeyValuePair<string, string>("resiredSalary", tbResiredSalary.Text));
                parameters.Add(new KeyValuePair<string, string>("salary", tbSalary.Text));
                parameters.Add(new KeyValuePair<string, string>("starVote", first_star_vote.ToString()));
                parameters.Add(new KeyValuePair<string, string>("userHiring", cbxHiring.SelectedValue.ToString()));
                parameters.Add(new KeyValuePair<string, string>("recruitmentNewsId", cbxPosition.SelectedValue.ToString()));
                parameters.Add(new KeyValuePair<string, string>("cvFrom", tbCVFrom.Text));
                parameters.Add(new KeyValuePair<string, string>("timeSendCv", dpTimeSendCV.SelectedDate.Value.ToString("yyyy-MM-dd")));
                parameters.Add(new KeyValuePair<string, string>("note", tbNote.Text));
                parameters.Add(new KeyValuePair<string, string>("status", cbxStatus.SelectedValue.ToString()));


                // Thiết lập Content
                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                ProcessEntity processEntity = JsonConvert.DeserializeObject<ProcessEntity>(responseContent);

                if (processEntity.result)
                {
                    hidePopup(1, Convert.ToInt32(id_process_old), -3);

                    MessageBox.Show("Chuyển giai đoạn thành công!");
                }
                else
                {
                    MessageBox.Show(processEntity.error.message);
                }
            }
            catch
            {
                MessageBox.Show("Có lỗi xảy ra, vui lòng thử lại!");
            }
        }

        private bool ValidateForm()
        {
            if (tbName.Text.Trim() == "")
            {
                MessageBox.Show("Tên ứng viên không được để trống!");
                return false;
            }

            if (tbCVFrom.Text.Trim() == "")
            {
                MessageBox.Show("Nguồn ứng viên không được phép để trống!");
                return false;
            }


            if (cbxPosition.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn vị trí ứng tuyển.");
                return false;
            }

            if (dpTimeSendCV.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng điền ngày ứng tuyển.");
                return false;
            }

            if (cbxStatus.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn giai đoạn chuyển.");
                return false;
            }

            return true;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void cbxHiring_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            cbxHiring.SelectedIndex = -1;
            string textSearch = cbxHiring.Text + e.Text;
            cbxHiring.IsDropDownOpen = true;
            if (textSearch == "")
            {
                cbxHiring.Text = "";
                cbxHiring.Items.Refresh();
                cbxHiring.ItemsSource = listAllEmployee;
                cbxHiring.SelectedIndex = -1;
            }
            else
            {
                cbxHiring.ItemsSource = "";
                cbxHiring.Items.Refresh();
                cbxHiring.ItemsSource = listAllEmployee.Where(t => t.Value.ToLower().Contains(textSearch.ToLower()));
            }
        }

        private void cbxHiring_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                cbxHiring.SelectedIndex = -1;
                string textSearch = cbxHiring.Text;
                cbxHiring.Items.Refresh();
                cbxHiring.ItemsSource = listAllEmployee.Where(t => t.Value.ToLower().Contains(textSearch.ToLower()));
            }
        }

        private void cbxPosition_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            cbxPosition.SelectedIndex = -1;
            string textSearch = cbxPosition.Text + e.Text;
            cbxPosition.IsDropDownOpen = true;
            if (textSearch == "")
            {
                cbxPosition.Text = "";
                cbxPosition.Items.Refresh();
                cbxPosition.ItemsSource = listRecruitment;
                cbxPosition.SelectedIndex = -1;
            }
            else
            {
                cbxPosition.ItemsSource = "";
                cbxPosition.Items.Refresh();
                cbxPosition.ItemsSource = listRecruitment.Where(t => t.Value.ToLower().Contains(textSearch.ToLower()));
            }
        }

        private void cbxPosition_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                cbxPosition.SelectedIndex = -1;
                string textSearch = cbxPosition.Text;
                cbxPosition.Items.Refresh();
                cbxPosition.ItemsSource = listRecruitment.Where(t => t.Value.ToLower().Contains(textSearch.ToLower()));
            }
        }
    }
}
