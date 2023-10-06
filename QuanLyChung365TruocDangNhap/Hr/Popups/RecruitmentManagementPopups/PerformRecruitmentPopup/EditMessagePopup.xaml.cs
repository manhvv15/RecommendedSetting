using QuanLyChung365TruocDangNhap.Hr.Entities.ListItemCombobox;
using Microsoft.Office.Interop.Excel;
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

namespace QuanLyChung365TruocDangNhap.Hr.Popups.RecruitmentManagementPopups.PerformRecruitmentPopup
{
    /// <summary>
    /// Interaction logic for EditMessagePopup.xaml
    /// </summary>
    public partial class EditMessagePopup : UserControl
    {
        string token;
        string id;
        Entities.ManageRecuitmentEntities.PerformRecuitmentEntities.PerformDetail performDetail;
        Dictionary<string, string> listAllEmployee = new Dictionary<string, string>();
        Dictionary<string, string> listAllRecruitmentNew = new Dictionary<string, string>();

        public delegate void HidePopup(int mode);
        public static HidePopup hidePopup;
        public EditMessagePopup(string token, string id, Dictionary<string, string> listAllEmployee)
        {
            InitializeComponent();
            this.token = token;
            this.id = id;
            this.listAllEmployee = listAllEmployee;
            SetupCombobox();
            GetData();


            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy"; //for the second type
            Thread.CurrentThread.CurrentCulture = ci;

        }

        private void SetupCombobox()
        {
            GetAllRecruitmentNew();
            cbxGender.ItemsSource = ListItemCombobox.ListCbxGender;
            cbxSalary.ItemsSource = ListItemCombobox.ListSalary;
            cbxMethodWork.ItemsSource = ListItemCombobox.ListMethodWork;
            cbxExp.ItemsSource = ListItemCombobox.ListCbxExp;
            cbxDegree.ItemsSource = ListItemCombobox.ListCbxEducation;
            cbxMemberFolow.ItemsSource = listAllEmployee;
            cbxHrName.ItemsSource = listAllEmployee;
            cbxPosition.ItemsSource = ListItemCombobox.ListPositionRecruit;
            cbxCate.ItemsSource = ListItemCombobox.ListCareer;
            cbxCit.ItemsSource = ListItemCombobox.ListProvince;
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

                performDetail = JsonConvert.DeserializeObject<Entities.ManageRecuitmentEntities.PerformRecuitmentEntities.PerformDetail>(responseContent);
                if (performDetail.result)
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
            tbTitle.Text = performDetail.success.data.data.title;
            tbAddress.Text = performDetail.success.data.data.address;
            cbxSalary.SelectedItem = ListItemCombobox.ListSalary.FirstOrDefault(t => t.ID == performDetail.success.data.data.salary_id);
            tbNumber.Text = performDetail.success.data.data.number;
            dp1.Text = ConvertDate(performDetail.success.data.data.recruitment_time, "dd/MM/yyyy");
            dp1.SelectedDate = DateTime.Parse(ConvertDate(performDetail.success.data.data.recruitment_time, "dd/MM/yyyy"));
            dp2.Text = ConvertDate(performDetail.success.data.data.recruitment_time_to, "dd/MM/yyyy");
            dp2.SelectedDate = DateTime.Parse(ConvertDate(performDetail.success.data.data.recruitment_time_to, "dd/MM/yyyy"));
            tbJobDetail.Text = performDetail.success.data.data.job_detail;
            cbxMethodWork.SelectedItem = ListItemCombobox.ListMethodWork.FirstOrDefault(t => t.ID == performDetail.success.data.data.woking_form);
            tbProbationaryTime.Text = performDetail.success.data.data.probationary_time;
            tbMoneyTip.Text = performDetail.success.data.data.money_tip;
            tbDescription.Text = performDetail.success.data.data.job_description;
            tbBenefit.Text = performDetail.success.data.data.interest;
            cbxRecruitment.SelectedItem = listAllRecruitmentNew.FirstOrDefault(t => t.Key == performDetail.success.data.data.recruitmen_id);
            cbxGender.SelectedItem = ListItemCombobox.ListCbxGender.FirstOrDefault(t => t.ID == performDetail.success.data.data.gender);
            cbxDegree.SelectedItem = ListItemCombobox.ListCbxEducation.FirstOrDefault(t => t.ID == performDetail.success.data.data.degree);
            tbJobRequired.Text = performDetail.success.data.data.job_require;
            cbxMemberFolow.SelectedItem = listAllEmployee.FirstOrDefault(t => t.Key == performDetail.success.data.data.member_follow);
            cbxHrName.SelectedItem = listAllEmployee.FirstOrDefault(t => t.Key == performDetail.success.data.data.hr_name);
            cbxExp.SelectedItem = ListItemCombobox.ListCbxExp.FirstOrDefault(t => t.ID == performDetail.success.data.data.job_exp);
            cbxPosition.SelectedItem = ListItemCombobox.ListPositionRecruit.FirstOrDefault(t => t.ID == performDetail.success.data.data.position_apply);
            cbxCit.SelectedItem = ListItemCombobox.ListProvince.FirstOrDefault(t => t.ID == performDetail.success.data.data.cit_id);
            cbxCate.SelectedItem = ListItemCombobox.ListCareer.FirstOrDefault(t => t.ID == performDetail.success.data.data.cate_id);
        }

        private void CancelPopup(object sender, MouseButtonEventArgs e)
        {
            hidePopup(0);
        }

        private async void GetAllRecruitmentNew()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.list_recuitment;

                httpRequestMessage.RequestUri = new Uri(api);

                var parameters = new List<KeyValuePair<string, string>>();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                parameters.Add(new KeyValuePair<string, string>("page", "1"));
                parameters.Add(new KeyValuePair<string, string>("pageSize", "10000"));
                parameters.Add(new KeyValuePair<string, string>("name", ""));

                // Thiết lập Content
                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                Entities.ManageRecuitmentEntities.RecuitmentProcessEntities.RecruitmentProcessEntity result = JsonConvert.DeserializeObject<Entities.ManageRecuitmentEntities.RecuitmentProcessEntities.RecruitmentProcessEntity>(responseContent);

                if (result.result)
                {
                    foreach (var item in result.success.data.data)
                    {
                        string fullName = "(QTTD" + item.id + ") " + item.name;
                        listAllRecruitmentNew.Add(item.id, fullName);
                    }

                    cbxRecruitment.ItemsSource = listAllRecruitmentNew;
                }

            }
            catch
            {
                //MessageBox.Show("Lỗi đăng nhập, vui lòng thử lại!");
            }
        }

        private void ClickEditNew(object sender, MouseButtonEventArgs e)
        {
            if (ValidateForm())
                EditNewRecruitment();
        }

        private async void EditNewRecruitment()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.editRecruitmentNew;

                httpRequestMessage.RequestUri = new Uri(api);

                var parameters = new List<KeyValuePair<string, string>>();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                parameters.Add(new KeyValuePair<string, string>("recruitmentNewsId", id));
                parameters.Add(new KeyValuePair<string, string>("title", tbTitle.Text));
                parameters.Add(new KeyValuePair<string, string>("posApply", cbxPosition.SelectedValue.ToString()));
                parameters.Add(new KeyValuePair<string, string>("cityId", cbxCit.SelectedValue.ToString()));
                parameters.Add(new KeyValuePair<string, string>("address", tbAddress.Text));
                if (cbxCate.SelectedIndex > -1)
                {
                    parameters.Add(new KeyValuePair<string, string>("cateId", cbxCate.SelectedValue.ToString()));
                }
                else
                {
                    parameters.Add(new KeyValuePair<string, string>("cateId", ""));
                }
                parameters.Add(new KeyValuePair<string, string>("salaryId", cbxSalary.SelectedValue.ToString()));
                parameters.Add(new KeyValuePair<string, string>("number", tbNumber.Text));
                parameters.Add(new KeyValuePair<string, string>("timeStart", dp1.SelectedDate.Value.ToString("MM/dd/yyyy")));
                parameters.Add(new KeyValuePair<string, string>("timeEnd", dp2.SelectedDate.Value.ToString("MM/dd/yyyy")));
                parameters.Add(new KeyValuePair<string, string>("jobDetail", tbJobDetail.Text));
                parameters.Add(new KeyValuePair<string, string>("wokingForm", cbxMethodWork.SelectedValue.ToString()));
                parameters.Add(new KeyValuePair<string, string>("probationaryTime", tbProbationaryTime.Text));
                parameters.Add(new KeyValuePair<string, string>("moneyTip", tbMoneyTip.Text));
                parameters.Add(new KeyValuePair<string, string>("jobDes", tbDescription.Text));
                parameters.Add(new KeyValuePair<string, string>("interest", tbBenefit.Text));
                if (cbxRecruitment.SelectedIndex > -1)
                {
                    parameters.Add(new KeyValuePair<string, string>("recruitmentId", cbxRecruitment.SelectedValue.ToString()));
                }
                else
                {
                    parameters.Add(new KeyValuePair<string, string>("recruitmentId", ""));
                }
                parameters.Add(new KeyValuePair<string, string>("jobExp", cbxExp.SelectedValue.ToString()));
                parameters.Add(new KeyValuePair<string, string>("gender", cbxGender.SelectedValue.ToString()));
                parameters.Add(new KeyValuePair<string, string>("degree", cbxDegree.SelectedValue.ToString()));
                parameters.Add(new KeyValuePair<string, string>("jobRequire", tbJobRequired.Text));

                if (cbxMemberFolow.SelectedIndex > -1)
                {
                    parameters.Add(new KeyValuePair<string, string>("memberFollow", cbxMemberFolow.SelectedValue.ToString()));
                }
                else
                {
                    parameters.Add(new KeyValuePair<string, string>("memberFollow", ""));
                }

                if (cbxHrName.SelectedIndex > -1)
                {
                    parameters.Add(new KeyValuePair<string, string>("hrName", cbxHrName.SelectedValue.ToString()));
                }
                else
                {
                    parameters.Add(new KeyValuePair<string, string>("hrName", ""));
                }

                // Thiết lập Content
                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                Entities.ManageRecuitmentEntities.ListCandidateEntities.ProcessEntity result = JsonConvert.DeserializeObject<Entities.ManageRecuitmentEntities.ListCandidateEntities.ProcessEntity>(responseContent);

                if (result.data.result)
                {
                    hidePopup(1);
                    MessageBox.Show("Cập nhật thành công!");
                }
                else
                {
                    MessageBox.Show("Cập nhật thất bại, vui lòng thử lại.");
                }

            }
            catch
            {
                MessageBox.Show("Có lỗi xảy ra, vui lòng thử lại!");
            }
        }

        private string ConvertDate(string date, string format)
        {
            try
            {
                DateTime myDate = DateTime.ParseExact(date, "dd/MM/yyyy",
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
                        DateTime myDate = DateTime.ParseExact(date, "d/M/yyyy",
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

        private bool ValidateForm()
        {
            if (tbTitle.Text.Trim() == "")
            {
                MessageBox.Show("Tiêu đề không được để trống.");
                return false;
            }

            if (cbxPosition.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn vị trí tuyển dụng.");
                return false;
            }

            if (cbxCit.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn địa điểm làm việc.");
                return false;
            }

            if (tbJobDetail.Text.Trim() == "")
            {
                MessageBox.Show("Chi tiết công việc không được để trống.");
                return false;
            }

            /*if (cbxMethodWork.SelectedIndex == 0)
            {
                MessageBox.Show("Vui lòng chọn hình thức làm việc.");
                return false;
            }*/

            if (cbxMethodWork.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn hình thức làm việc");
                return false;
            }

            if (cbxSalary.SelectedIndex == -1)
            {
                MessageBox.Show("Mức lương không được để trống.");
                return false;
            }

            if (tbNumber.Text.Trim() == "")
            {
                MessageBox.Show("Số lượng tuyển không được để trống.");
                return false;
            }

            if (Convert.ToInt32(tbNumber.Text) ==  0)
            {
                MessageBox.Show("Số lượng tuyển không không hợp lệ.");
                return false;
            }
            if (dp1.SelectedDate == null || dp2.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng điền đầy đủ thời hạn tuyển.");
                return false;
            }

            if (tbDescription.Text.Trim() == "")
            {
                MessageBox.Show("Mô tả công việc không được để trống");
                return false;
            }

            if (tbBenefit.Text.Trim() == "")
            {
                MessageBox.Show("Quyền lợi ứng viên không được để trống");
                return false;
            }

            if (cbxExp.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn kinh nghiệm.");
                return false;
            }

            if (cbxDegree.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn bằng cấp.");
                return false;
            }

            if (cbxGender.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn giới tính.");
                return false;
            }

            if (tbJobRequired.Text.Trim() == "")
            {
                MessageBox.Show("Yêu cầu công việc không được để trống.");
                return false;
            }
            if(tbAddress.Text.Trim() == "")
            {
                MessageBox.Show("Địa chỉ tuyển dụng không được để trống!");
                return false;
            }
            return true;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void cbxCit_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            cbxCit.SelectedIndex = -1;
            string textSearch = cbxCit.Text + e.Text;
            cbxCit.IsDropDownOpen = true;
            if (textSearch == "")
            {
                cbxCit.Text = "";
                cbxCit.Items.Refresh();
                cbxCit.ItemsSource = ListItemCombobox.ListProvince;
                cbxCit.SelectedIndex = -1;
            }
            else
            {
                cbxCit.ItemsSource = "";
                cbxCit.Items.Refresh();
                cbxCit.ItemsSource = ListItemCombobox.ListProvince.Where(t => t.value.ToLower().Contains(textSearch.ToLower()));
            }
        }

        private void cbxCit_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                cbxCit.SelectedIndex = -1;
                string textSearch = cbxCit.Text;
                cbxCit.Items.Refresh();
                cbxCit.ItemsSource = ListItemCombobox.ListProvince.Where(t => t.value.ToLower().Contains(textSearch.ToLower()));
            }
        }

        private void cbxCate_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            cbxCate.SelectedIndex = -1;
            string textSearch = cbxCate.Text + e.Text;
            cbxCate.IsDropDownOpen = true;
            if (textSearch == "")
            {
                cbxCate.Text = "";
                cbxCate.Items.Refresh();
                cbxCate.ItemsSource = ListItemCombobox.ListCareer;
                cbxCate.SelectedIndex = -1;
            }
            else
            {
                cbxCate.ItemsSource = "";
                cbxCate.Items.Refresh();
                cbxCate.ItemsSource = ListItemCombobox.ListCareer.Where(t => t.value.ToLower().Contains(textSearch.ToLower()));
            }
        }

        private void cbxCate_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                cbxCate.SelectedIndex = -1;
                string textSearch = cbxCate.Text;
                cbxCate.Items.Refresh();
                cbxCate.ItemsSource = ListItemCombobox.ListCareer.Where(t => t.value.ToLower().Contains(textSearch.ToLower()));
            }
        }

        private void cbxRecruitment_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            cbxRecruitment.SelectedIndex = -1;
            string textSearch = cbxRecruitment.Text + e.Text;
            cbxRecruitment.IsDropDownOpen = true;
            if (textSearch == "")
            {
                cbxRecruitment.Text = "";
                cbxRecruitment.Items.Refresh();
                cbxRecruitment.ItemsSource = listAllRecruitmentNew;
                cbxRecruitment.SelectedIndex = -1;
            }
            else
            {
                cbxRecruitment.ItemsSource = "";
                cbxRecruitment.Items.Refresh();
                cbxRecruitment.ItemsSource = listAllRecruitmentNew.Where(t => t.Value.ToLower().Contains(textSearch.ToLower()));
            }
        }

        private void cbxRecruitment_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                cbxRecruitment.SelectedIndex = -1;
                string textSearch = cbxRecruitment.Text;
                cbxRecruitment.Items.Refresh();
                cbxRecruitment.ItemsSource = listAllRecruitmentNew.Where(t => t.Value.ToLower().Contains(textSearch.ToLower()));
            }
        }

        private void cbxDegree_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            cbxDegree.SelectedIndex = -1;
            string textSearch = cbxDegree.Text + e.Text;
            cbxDegree.IsDropDownOpen = true;
            if (textSearch == "")
            {
                cbxDegree.Text = "";
                cbxDegree.Items.Refresh();
                cbxDegree.ItemsSource = ListItemCombobox.ListCbxEducation;
                cbxDegree.SelectedIndex = -1;
            }
            else
            {
                cbxDegree.ItemsSource = "";
                cbxDegree.Items.Refresh();
                cbxDegree.ItemsSource = ListItemCombobox.ListCbxEducation.Where(t => t.value.ToLower().Contains(textSearch.ToLower()));
            }
        }

        private void cbxDegree_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                cbxDegree.SelectedIndex = -1;
                string textSearch = cbxDegree.Text;
                cbxDegree.Items.Refresh();
                cbxDegree.ItemsSource = ListItemCombobox.ListCbxEducation.Where(t => t.value.ToLower().Contains(textSearch.ToLower()));
            }
        }

        private void cbxMemberFolow_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            cbxMemberFolow.SelectedIndex = -1;
            string textSearch = cbxMemberFolow.Text + e.Text;
            cbxMemberFolow.IsDropDownOpen = true;
            if (textSearch == "")
            {
                cbxMemberFolow.Text = "";
                cbxMemberFolow.Items.Refresh();
                cbxMemberFolow.ItemsSource = listAllEmployee;
                cbxMemberFolow.SelectedIndex = -1;
            }
            else
            {
                cbxMemberFolow.ItemsSource = "";
                cbxMemberFolow.Items.Refresh();
                cbxMemberFolow.ItemsSource = listAllEmployee.Where(t => t.Value.ToLower().Contains(textSearch.ToLower()));
            }
        }

        private void cbxMemberFolow_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                cbxMemberFolow.SelectedIndex = -1;
                string textSearch = cbxMemberFolow.Text;
                cbxMemberFolow.Items.Refresh();
                cbxMemberFolow.ItemsSource = listAllEmployee.Where(t => t.Value.ToLower().Contains(textSearch.ToLower()));
            }
        }

        private void cbxHrName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            cbxHrName.SelectedIndex = -1;
            string textSearch = cbxHrName.Text + e.Text;
            cbxHrName.IsDropDownOpen = true;
            if (textSearch == "")
            {
                cbxHrName.Text = "";
                cbxHrName.Items.Refresh();
                cbxHrName.ItemsSource = listAllEmployee;
                cbxHrName.SelectedIndex = -1;
            }
            else
            {
                cbxHrName.ItemsSource = "";
                cbxHrName.Items.Refresh();
                cbxHrName.ItemsSource = listAllEmployee.Where(t => t.Value.ToLower().Contains(textSearch.ToLower()));
            }
        }

        private void cbxHrName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                cbxHrName.SelectedIndex = -1;
                string textSearch = cbxHrName.Text;
                cbxHrName.Items.Refresh();
                cbxHrName.ItemsSource = listAllEmployee.Where(t => t.Value.ToLower().Contains(textSearch.ToLower()));
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
                cbxPosition.ItemsSource = ListItemCombobox.ListPositionRecruit;
                cbxPosition.SelectedIndex = -1;
            }
            else
            {
                cbxPosition.ItemsSource = "";
                cbxPosition.Items.Refresh();
                cbxPosition.ItemsSource = ListItemCombobox.ListPositionRecruit.Where(t => t.value.ToLower().Contains(textSearch.ToLower()));
            }
        }

        private void cbxPosition_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                cbxPosition.SelectedIndex = -1;
                string textSearch = cbxPosition.Text;
                cbxPosition.Items.Refresh();
                cbxPosition.ItemsSource = ListItemCombobox.ListPositionRecruit.Where(t => t.value.ToLower().Contains(textSearch.ToLower()));
            }
        }

        private void cbxMethodWork_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            cbxMethodWork.SelectedIndex = -1;
            string textSearch = cbxMethodWork.Text + e.Text;
            cbxMethodWork.IsDropDownOpen = true;
            if (textSearch == "")
            {
                cbxMethodWork.Text = "";
                cbxMethodWork.Items.Refresh();
                cbxMethodWork.ItemsSource = ListItemCombobox.ListMethodWork;
                cbxMethodWork.SelectedIndex = -1;
            }
            else
            {
                cbxMethodWork.ItemsSource = "";
                cbxMethodWork.Items.Refresh();
                cbxMethodWork.ItemsSource = ListItemCombobox.ListMethodWork.Where(t => t.value.ToLower().Contains(textSearch.ToLower()));
            }
        }

        private void cbxMethodWork_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                cbxMethodWork.SelectedIndex = -1;
                string textSearch = cbxMethodWork.Text;
                cbxMethodWork.Items.Refresh();
                cbxMethodWork.ItemsSource = ListItemCombobox.ListMethodWork.Where(t => t.value.ToLower().Contains(textSearch.ToLower()));
            }
        }

        private void cbxSalary_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            cbxSalary.SelectedIndex = -1;
            string textSearch = cbxSalary.Text + e.Text;
            cbxSalary.IsDropDownOpen = true;
            if (textSearch == "")
            {
                cbxSalary.Text = "";
                cbxSalary.Items.Refresh();
                cbxSalary.ItemsSource = ListItemCombobox.ListSalary;
                cbxSalary.SelectedIndex = -1;
            }
            else
            {
                cbxSalary.ItemsSource = "";
                cbxSalary.Items.Refresh();
                cbxSalary.ItemsSource = ListItemCombobox.ListSalary.Where(t => t.value.ToLower().Contains(textSearch.ToLower()));
            }
        }

        private void cbxSalary_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                cbxSalary.SelectedIndex = -1;
                string textSearch = cbxSalary.Text;
                cbxSalary.Items.Refresh();
                cbxSalary.ItemsSource = ListItemCombobox.ListSalary.Where(t => t.value.ToLower().Contains(textSearch.ToLower()));
            }
        }

        private void cbxExp_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            cbxExp.SelectedIndex = -1;
            string textSearch = cbxExp.Text + e.Text;
            cbxExp.IsDropDownOpen = true;
            if (textSearch == "")
            {
                cbxExp.Text = "";
                cbxExp.Items.Refresh();
                cbxExp.ItemsSource = ListItemCombobox.ListCbxExp;
                cbxExp.SelectedIndex = -1;
            }
            else
            {
                cbxExp.ItemsSource = "";
                cbxExp.Items.Refresh();
                cbxExp.ItemsSource = ListItemCombobox.ListCbxExp.Where(t => t.value.ToLower().Contains(textSearch.ToLower()));
            }
        }

        private void cbxExp_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                cbxExp.SelectedIndex = -1;
                string textSearch = cbxExp.Text;
                cbxExp.Items.Refresh();
                cbxExp.ItemsSource = ListItemCombobox.ListCbxExp.Where(t => t.value.ToLower().Contains(textSearch.ToLower()));
            }
        }

        private void cbxGender_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            cbxGender.SelectedIndex = -1;
            string textSearch = cbxGender.Text + e.Text;
            cbxGender.IsDropDownOpen = true;
            if (textSearch == "")
            {
                cbxGender.Text = "";
                cbxGender.Items.Refresh();
                cbxGender.ItemsSource = ListItemCombobox.ListCbxGender2;
                cbxGender.SelectedIndex = -1;
            }
            else
            {
                cbxGender.ItemsSource = "";
                cbxGender.Items.Refresh();
                cbxGender.ItemsSource = ListItemCombobox.ListCbxGender2.Where(t => t.value.ToLower().Contains(textSearch.ToLower()));
            }
        }

        private void cbxGender_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                cbxGender.SelectedIndex = -1;
                string textSearch = cbxGender.Text;
                cbxGender.Items.Refresh();
                cbxGender.ItemsSource = ListItemCombobox.ListCbxGender2.Where(t => t.value.ToLower().Contains(textSearch.ToLower()));
            }
        }
    }
}
