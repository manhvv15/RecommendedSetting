using QuanLyChung365TruocDangNhap.Hr.Entities.ListItemCombobox;
using QuanLyChung365TruocDangNhap.Hr.Entities.ManageRecuitmentEntities.ListCandidateEntities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
//using System.Windows.Shapes;

namespace QuanLyChung365TruocDangNhap.Hr.Popups.RecruitmentManagementPopups.ListOfCandidatesPopups
{
    /// <summary>
    /// Interaction logic for CandidatePofilePopup.xaml
    /// </summary>
    public partial class CandidatePofilePopup : UserControl
    {
        string token;
        public Dictionary<string, string> listAllEmployee = new Dictionary<string, string>();
        // danh sách vị trí tuyển dụng
        public Dictionary<string, string> listRecruitment = new Dictionary<string, string>();
        int first_star_vote = 0;
        //int skill_vote = 0;

        List<Skill> listSkill = new List<Skill>();
        int stt_skill = 0;
        string file_cv_path = "";


        // deligate event hide popups
        public delegate void HidePopup(int mode, int id_process);
        public static event HidePopup hidePopup;


        public CandidatePofilePopup(string token, Dictionary<string, string> listAllEmployee, Dictionary<string, string> listRecruitment)
        {
            InitializeComponent();
            this.token = token;
            this.listAllEmployee = listAllEmployee;
            this.listRecruitment = listRecruitment;
            SetUpCombobox();
            DisplayFirstStarVote(first_star_vote);

        }

        private void SetUpCombobox()
        {
            cbxGender.ItemsSource = ListItemCombobox.ListCbxGender;
            cbxEducation.ItemsSource = ListItemCombobox.ListCbxEducation;
            cbxExp.ItemsSource = ListItemCombobox.ListCbxExp;
            cbxMarried.ItemsSource = ListItemCombobox.ListMarried;
            cbxRecuitment.ItemsSource = listAllEmployee;
            cbxRecommend.ItemsSource = listAllEmployee;
            cbxPosition.ItemsSource = listRecruitment;
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

        private void AddNewCandidate(object sender, MouseButtonEventArgs e)
        {
            tbForcus.Focus();
            if (ValidateForm())
                AddNewCandidate();
        }

        private async void AddNewCandidate()
        {
            try
            {
                var client = new HttpClient();
                var multiForm = new MultipartFormDataContent();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                multiForm.Add(new StringContent(tbName.Text), "name");
                multiForm.Add(new StringContent(tbEmail.Text), "email");
                multiForm.Add(new StringContent(tbPhone.Text), "phone");
                string gender = cbxGender.SelectedValue.ToString();
                multiForm.Add(new StringContent(gender), "gender");
                multiForm.Add(new StringContent(dpDateOfBirth.SelectedDate.Value.ToString("yyyy-MM-dd")), "birthday");
                string education = cbxEducation.SelectedValue.ToString();
                multiForm.Add(new StringContent(education), "education");
                string exp = cbxExp.SelectedValue.ToString();
                multiForm.Add(new StringContent(exp), "exp");
                string married = cbxMarried.SelectedValue.ToString();
                multiForm.Add(new StringContent(married), "isMarried");
                multiForm.Add(new StringContent(tbAddress.Text), "address");
                multiForm.Add(new StringContent(tbCVFrom.Text), "cvFrom");
                string id = cbxRecuitment.SelectedValue.ToString();
                multiForm.Add(new StringContent(id), "userHiring");
                //multiForm.Add(new StringContent("10"), "recruitmentNewsId");
                multiForm.Add(new StringContent(dpTimeSendCV.SelectedDate.Value.ToString("yyyy-MM-dd")), "timeSendCv");
                multiForm.Add(new StringContent(first_star_vote.ToString()), "firstStarVote");
                multiForm.Add(new StringContent(DateTime.Now.ToString("yyyy-MM-dd")), "createdAt");
                if (cbxRecommend.SelectedIndex > -1)
                {
                    id = cbxRecommend.SelectedValue.ToString();
                    multiForm.Add(new StringContent(id), "userRecommend");
                }
                else
                {
                    multiForm.Add(new StringContent(""), "userRecommend");
                }

                id = cbxPosition.SelectedValue.ToString();
                multiForm.Add(new StringContent(id), "recruitmentNewsId");
                List<SkillPost> skillPosts = new List<SkillPost>();
                foreach (var item in listSkill)
                {
                    SkillPost skillPost = new SkillPost() { name = item.skill_name, star = item.skill_vote };
                    skillPosts.Add(skillPost);
                    

                   
                }
                if(skillPosts != null && skillPosts.Count > 0)
                foreach(var item in skillPosts)
                {
                    var json = JsonConvert.SerializeObject(item);
                    multiForm.Add(new StringContent(json), "listSkill[]");
                }
                multiForm.Add(new StringContent(tbSchool.Text), "school");
                multiForm.Add(new StringContent(tbHomeTown.Text), "hometown");
                //add file and directly upload it
                if (file_cv_path != "")
                {
                    FileStream fs = File.OpenRead(file_cv_path);
                    multiForm.Add(new StreamContent(fs), "cv", Path.GetFileName(file_cv_path));
                }

                // send request to API
                var url = APIs.API.add_candidate;
                var response = await client.PostAsync(url, multiForm);
                var responseContent = await response.Content.ReadAsStringAsync();
                ProcessEntity result = JsonConvert.DeserializeObject<ProcessEntity>(responseContent);
                if (result.data.result)
                {
                    hidePopup(1, 0);
                    MessageBox.Show("Thêm mới thành công!");
                }
                else
                {
                    MessageBox.Show("Thêm mới thất bại, vui lòng thử lại.");
                }
            }
            catch
            {
                MessageBox.Show("Có lỗi xảy ra, vui lòng thử lại!");
            }
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
            hidePopup(0, 0);
        }

        private void ChooseCV(object sender, MouseButtonEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();



            // Set filter for file extension and default file extension 
            //dlg.DefaultExt = ".png";
            dlg.Filter = "PDF Files (*.pdf)|*.pdf|JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg";


            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.SafeFileName;
                file_cv_path = dlg.FileName;
                tbFileName.Text = filename;
                btnDeleteCV.Visibility = Visibility.Visible;
            }
        }

        private void DeleteFileCV(object sender, MouseButtonEventArgs e)
        {
            tbFileName.Text = "";
            file_cv_path = "";
            btnDeleteCV.Visibility = Visibility.Collapsed;
        }

        private bool ValidateForm()
        {
            if (tbName.Text.Trim() == "")
            {
                MessageBox.Show("Tên ứng viên không được để trống!");
                return false;
            }

            if (tbEmail.Text.Trim() == "")
            {
                MessageBox.Show("Email không được để trống!");
                return false;
            }

            if (!ValidateEmail())
            {
                MessageBox.Show("Email không đúng định dạng, vui lòng nhập lại.");
                return false;
            }

            if (tbPhone.Text.Trim() == "")
            {
                MessageBox.Show("Số điện thoại không được để trống!");
                return false;
            }

            if(tbPhone.Text.Length > 15)
            {
                MessageBox.Show("Số điện thoại không hợp lệ");
                return false;
            }

            if (tbPhone.Text.Length < 10)
            {
                MessageBox.Show("Số điện thoại không hợp lệ");
                return false;
            }

            if (!ValidatePhoneNumber())
            {
                MessageBox.Show("Số điện thoại không hợp lệ, vui lòng nhập lại");
                return false;
            }

            if (cbxGender.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn giới tính.");
                return false;
            }
            if (dpDateOfBirth.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng điền ngày sinh ứng viên.");
                return false;
            }

            if(dpDateOfBirth.SelectedDate > DateTime.Now)
            {
                MessageBox.Show("Ngày sinh không hợp lệ");
                return false;
            }

            if (cbxEducation.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn trình độ học vấn.");
                return false;
            }

            if (cbxExp.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn kinh nghiệm làm việc.");
                return false;
            }

            if (cbxMarried.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn tình trạng hôn nhân.");
                return false;
            }

            if (tbAddress.Text.Trim() == "")
            {
                MessageBox.Show("Địa chỉ không được phép để trống!");
                return false;
            }

            if (tbCVFrom.Text.Trim() == "")
            {
                MessageBox.Show("Nguồn ứng viên không được phép để trống!");
                return false;
            }

            if (cbxRecuitment.SelectedIndex == -1 || cbxRecuitment.SelectedValue.ToString() == "")
            {
                MessageBox.Show("Vui lòng chọn nhân viên tuyển dụng.");
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

            if(first_star_vote == 0)    
            {
                MessageBox.Show("Đánh giá không được để trống");
                return false;
            }
            foreach (var item in listSkill)
            {
                if (item.skill_name == "")
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ tên kỹ năng");
                    return false;
                }
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

        private void cbxEducation_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
           cbxEducation.SelectedIndex = -1;
            string textSearch = cbxEducation.Text + e.Text;
            cbxEducation.IsDropDownOpen = true;
            if (textSearch == "")
            {
                cbxEducation.Text = "";
                cbxEducation.Items.Refresh();
                cbxEducation.ItemsSource = ListItemCombobox.ListCbxEducation;
                cbxEducation.SelectedIndex = -1;
            }
            else
            {
                cbxEducation.ItemsSource = "";
                cbxEducation.Items.Refresh();
                cbxEducation.ItemsSource = ListItemCombobox.ListCbxEducation.Where(t => t.value.ToLower().Contains(textSearch.ToLower()));
            }
        }

        private void cbxEducation_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                cbxEducation.SelectedIndex = -1;
                string textSearch = cbxEducation.Text;
                cbxEducation.Items.Refresh();
                cbxEducation.ItemsSource = ListItemCombobox.ListCbxEducation.Where(t => t.value.ToLower().Contains(textSearch.ToLower()));
            }
        }

        private void cbxRecuitment_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            cbxRecuitment.SelectedIndex = -1;
            string textSearch = cbxRecuitment.Text + e.Text;
            cbxRecuitment.IsDropDownOpen = true;
            if (textSearch == "")
            {
                cbxRecuitment.Text = "";
                cbxRecuitment.Items.Refresh();
                cbxRecuitment.ItemsSource = listAllEmployee;
                cbxRecuitment.SelectedIndex = -1;
            }
            else
            {
                cbxRecuitment.ItemsSource = "";
                cbxRecuitment.Items.Refresh();
                cbxRecuitment.ItemsSource = listAllEmployee.Where(t => t.Value.ToLower().Contains(textSearch.ToLower()));
            }
        }

        private void cbxRecuitment_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                cbxRecuitment.SelectedIndex = -1;
                string textSearch = cbxRecuitment.Text;
                cbxRecuitment.Items.Refresh();
                cbxRecuitment.ItemsSource = listAllEmployee.Where(t => t.Value.ToLower().Contains(textSearch.ToLower()));
            }
        }

        private void cbxRecommend_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            cbxRecommend.SelectedIndex = -1;
            string textSearch = cbxRecommend.Text + e.Text;
            cbxRecommend.IsDropDownOpen = true;
            if (textSearch == "")
            {
                cbxRecommend.Text = "";
                cbxRecommend.Items.Refresh();
                cbxRecommend.ItemsSource = listAllEmployee;
                cbxRecommend.SelectedIndex = -1;
            }
            else
            {
                cbxRecommend.ItemsSource = "";
                cbxRecommend.Items.Refresh();
                cbxRecommend.ItemsSource = listAllEmployee.Where(t => t.Value.ToLower().Contains(textSearch.ToLower()));
            }
        }

        private void cbxRecommend_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                cbxRecommend.SelectedIndex = -1;
                string textSearch = cbxRecommend.Text;
                cbxRecommend.Items.Refresh();
                cbxRecommend.ItemsSource = listAllEmployee.Where(t => t.Value.ToLower().Contains(textSearch.ToLower()));
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

    public class SkillPost
    {
        public string name { get; set; }
        public string star { get; set; }
    }
}
