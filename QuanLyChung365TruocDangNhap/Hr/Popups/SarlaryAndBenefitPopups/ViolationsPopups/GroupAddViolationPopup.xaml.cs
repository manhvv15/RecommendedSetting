using QuanLyChung365TruocDangNhap.Hr.Entities.AdministrationEntity.PersonnelChangeEntity;
using QuanLyChung365TruocDangNhap.Hr.Entities.ManageRecuitmentEntities.ListCandidateEntities;
using QuanLyChung365TruocDangNhap.Hr.Entities.SalaryAndBenefits;
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

namespace QuanLyChung365TruocDangNhap.Hr.Popups.SarlaryAndBenefitPopups.ViolationsPopups
{
    /// <summary>
    /// Interaction logic for GroupAddViolationPopup.xaml
    /// </summary>
    public partial class GroupAddViolationPopup : UserControl
    {
        // deligate event hide popups
        public delegate void HidePopup(int mode);
        public static event HidePopup hidePopup;

        string token;
        List<Items6> listDepartment = new List<Items6>();
        public GroupAddViolationPopup(string token)
        {
            InitializeComponent();
            this.token = token;
            GetDatalistDepartment();
        }
        private async void GetDatalistDepartment()
        {
            var httpClient = new HttpClient();
            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = HttpMethod.Post;
            string api = APIs.API.listDepartment;

            httpRequestMessage.RequestUri = new Uri(api);
            httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);

            var response = await httpClient.SendAsync(httpRequestMessage);
            var responseContent = await response.Content.ReadAsStringAsync();

            listDepartment result = JsonConvert.DeserializeObject<listDepartment>(responseContent);
            listDepartment = result.data.items;
            cbxTenPhongBan.ItemsSource = listDepartment;

        }
        private void AddNewGroupViolation(object sender, MouseButtonEventArgs e)
        {
            if (ValidateForm())
                AddDataGroupViolation();
        }
        private string StringFromRichTextBox(RichTextBox rtb)
        {
            TextRange textRange = new TextRange(
                rtb.Document.ContentStart,
                rtb.Document.ContentEnd
            );
            string replacement = Regex.Replace(textRange.Text, @"\t|\n|\r", " ");
            return replacement;
        }
        public class InputKyLuatNhom
        {
            public string regulatory_basis { get; set; }
            public string infringe_name { get; set; }
            public string created_by { get; set; }
            public string infringe_at { get; set; }
            public int infringe_type { get; set; }
            public string number_violation { get; set; }
            public List<int> list_user { get; set; } = new List<int>();
            public List<string> list_user_name { get; set; } = new List<string>();
            public int price { get; set; }
            public int dep_id { get; set; }
            public string dep_name { get; set; }
        }
        private async void AddDataGroupViolation()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.addInfingesGroup;

                httpRequestMessage.RequestUri = new Uri(api);
                InputKyLuatNhom input = new InputKyLuatNhom();
                input.price = int.Parse(tbNumber.Text);
                input.dep_id = int.Parse(cbxTenPhongBan.SelectedValue.ToString());
                input.dep_name = cbxTenPhongBan.Text;
                input.number_violation = tbSoQDXLVP.Text;
                input.infringe_name = tbSoQDXLVP.Text;
                input.regulatory_basis = tbCanCuQD.Text;
                input.infringe_at = DTdate.SelectedDate.Value.ToString("yyyy-MM-dd");
                input.created_by = tbNguoiKyQD.Text;
                input.infringe_type = int.Parse(tbHinhThucXLVP.Text);

                //var parameters = new List<KeyValuePair<string, string>>();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var content = new StringContent( JsonConvert.SerializeObject(input), null, "application/json");
                httpRequestMessage.Content = content;

                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                ProcessEntity result = JsonConvert.DeserializeObject<ProcessEntity>(responseContent);

                if (result.data.result)
                {
                    hidePopup(1);
                    MessageBox.Show("Thêm kỷ luật nhóm thành công!");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        private void CancelPopup(object sender, MouseButtonEventArgs e)
        {
            hidePopup(0);
        }
        private void cbx2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cbxTenPhongBan.SelectedValuePath.ToString();
        }

        private bool ValidateForm()
        {
            if (tbTenLoiVP.Text.Trim() ==  "")
            {
                MessageBox.Show("Tên vi phạm không được để trống!");
                return false;
            }
            if (tbCanCuQD.Text.Trim() == "") {
                MessageBox.Show("Vui lòng nhập căn cứ quy định!");
                return false;
            }
            if (tbSoQDXLVP.Text.Trim() == "") {
                MessageBox.Show("Vui lòng nhập số quy định xử lý vi phạm!");
                return false;
            }
            if (DTdate.SelectedDate == null) {
                MessageBox.Show("Thời gian không được để trống!");
                return false;
            }
            if (tbHinhThucXLVP.Text.Trim() == "") {
                MessageBox.Show("Vui lòng nhập hình thức xử lý vi phạm!");
                return false;
            }
            if (cbxTenPhongBan.SelectedIndex == -1) {
                MessageBox.Show("Vui lòng chọn phòng ban!");
                return false;
            }
            if (tbNguoiKyQD.Text.Trim() == "") {
                MessageBox.Show("Vui lòng nhập người ký!");
                return false;
            }

            //if (tbNumber.Text.Trim() == "")
            //{
            //    MessageBox.Show("Số tiền không được để trống");
            //    return false;
            //}

            if (tbLydo.Text.Trim() == "")
            {
                MessageBox.Show("Lý do không được bỏ trống");
                return false;
            }

            if (Convert.ToInt32(tbNumber.Text) <= 0)
            {
                MessageBox.Show("Số tiền phải lớn hơn 0");
                return false;
            }

            return true;
        }
        private void cbxTenPhongBan_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                cbxTenPhongBan.SelectedIndex = -1;
                string textSearch = cbxTenPhongBan.Text;
                cbxTenPhongBan.Items.Refresh();
                cbxTenPhongBan.ItemsSource = listDepartment.Where(t => t.dep_name.ToLower().Contains(textSearch.ToLower()));
            }
        }

        private void cbxTenPhongBan_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            cbxTenPhongBan.SelectedIndex = -1;
            string textSearch = cbxTenPhongBan.Text + e.Text;
            cbxTenPhongBan.IsDropDownOpen = true;
            if (textSearch == "")
            {
                cbxTenPhongBan.Text = "";
                cbxTenPhongBan.Items.Refresh();
                cbxTenPhongBan.ItemsSource = listDepartment;
                cbxTenPhongBan.SelectedIndex = -1;
            }
            else
            {
                cbxTenPhongBan.ItemsSource = "";
                cbxTenPhongBan.Items.Refresh();
                cbxTenPhongBan.ItemsSource = listDepartment.Where(t => t.dep_name.ToLower().Contains(textSearch.ToLower()));
            }
        }
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
