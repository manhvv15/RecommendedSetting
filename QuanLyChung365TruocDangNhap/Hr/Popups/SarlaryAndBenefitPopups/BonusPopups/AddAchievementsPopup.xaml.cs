using QuanLyChung365TruocDangNhap.Hr.Entities.AdministrationEntity.PersonnelChangeEntity;
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

namespace QuanLyChung365TruocDangNhap.Hr.Popups.SarlaryAndBenefitPopups.BonusPopups
{
    /// <summary>
    /// Interaction logic for AddAchievementsPopup.xaml
    /// </summary>
    /// 
    public class cboxValue2
    {
        public cboxValue2(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }
        public string Value { get; set; }
    }
    public partial class AddAchievementsPopup : UserControl
    {
        // deligate event hide popups

        public delegate void HidePopup(int mode);
        public static event HidePopup hidePopup;

        string token;


        List<Items6> listDepartment = new List<Items6>();
        public AddAchievementsPopup(string token)
        {
            this.token = token;
            InitializeComponent();
            List<cboxValue2> list = new List<cboxValue2>();
            list.Add(new cboxValue2("Huân chương", "1"));
            list.Add(new cboxValue2("Huy chương", "2"));
            list.Add(new cboxValue2("Giấy khen", "3"));
            list.Add(new cboxValue2("Thăng chức", "4"));
            list.Add(new cboxValue2("Kỉ niệm chương", "5"));
            list.Add(new cboxValue2("Tiền mặt", "6"));
            cbx1.ItemsSource = list;
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
            try
            {
                listDepartment = result.data.items;
                cbxTenPhongBan.ItemsSource = listDepartment;
            }
            catch
            {

            }
            

        }
        private void CancelPopup(object sender, MouseButtonEventArgs e)
        {
            hidePopup(0);
        }
        private void AddtAchievementsPopup(object sender, MouseButtonEventArgs e)
        {
            if(ValidateForm())
                AddListAchievements();
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
        private async void AddListAchievements()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.AddAchievementsGroup;

                httpRequestMessage.RequestUri = new Uri(api);

                var parameters = new List<KeyValuePair<string, string>>();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                parameters.Add(new KeyValuePair<string, string>("depId", cbxTenPhongBan.SelectedValue.ToString()));
                parameters.Add(new KeyValuePair<string, string>("depName", cbxTenPhongBan.Text));
                parameters.Add(new KeyValuePair<string, string>("achievement_id", txtSoQD.Text));
                parameters.Add(new KeyValuePair<string, string>("content", txtNDKhenThuong.Text));
                parameters.Add(new KeyValuePair<string, string>("achievement_at", DTdate.SelectedDate.Value.ToString("yyyy-MM-dd")));
                parameters.Add(new KeyValuePair<string, string>("created_by", txtNguoiKyQD.Text));
                parameters.Add(new KeyValuePair<string, string>("appellation", txtDanhHieu.Text));
                parameters.Add(new KeyValuePair<string, string>("achievement_level", txtCapKhen.Text));

                if (cbx1.SelectedValue.ToString() == "6")
                {
                    //string api1 = APIs.API.add_punish_emp;
                    parameters.Add(new KeyValuePair<string, string>("price", tbNumber.Text));
                    //parameters.Add(new KeyValuePair<string, string>("des", tbLydo.Text));
                    //parameters.Add(new KeyValuePair<string, string>("type", "1"));
                }

                parameters.Add(new KeyValuePair<string, string>("achievement_type", cbx1.SelectedValue.ToString()));

                
                // Thiết lập Content
                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                AcgievementEntity result = JsonConvert.DeserializeObject<AcgievementEntity>(responseContent);

                if (result.result)
                {
                    hidePopup(1);
                    MessageBox.Show("Thêm mới thành công");
                }
                else
                {
                    MessageBox.Show("Thêm mới thất bại, vui lòng thử lại");
                }

            }
            catch
            {
                MessageBox.Show("Có lỗi xảy ra, vui lòng thử lại!");
            }
        }

        private bool ValidateForm()
        {
            if(txtSoQD.Text.Trim() == "")
            {
                MessageBox.Show("Số quy định không được để trống.");
                return false;
            }

            if(txtNDKhenThuong.Text.Trim() == "")
            {
                MessageBox.Show("Nội dung khen thưởng không được để trống.");
                return false;
            }

            if(cbxTenPhongBan.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn tên phòng ban.");
                return false;
            }

            if(txtNguoiKyQD.Text.Trim() == "")
            {
                MessageBox.Show("Người ký quyết định không được để trống.");
                return false;
            }

            if(DTdate.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng chọn thời điểm.");
                return false;
            }

            if(cbx1.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn hình thức khen thưởng.");
                return false;
            }
            if(txtDanhHieu.Text.Trim() == "")
            {
                MessageBox.Show("Danh hiệu không được để trống.");
                return false;
            }
            if(txtCapKhen.Text.Trim() == "")
            {
                MessageBox.Show("Cấp khen không được để trống.");
                return false;
            }
      
            if(cbx1.SelectedIndex == 5)
            {
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


        private void Cbx1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string combobox = cbx1.SelectedValue.ToString();

            if (combobox == "6")
            {
                inputNuberMoney.Visibility = Visibility.Visible;
                inputNote.Visibility = Visibility.Visible;
            }
            else
            {
                inputNuberMoney.Visibility = Visibility.Collapsed;
                inputNote.Visibility = Visibility.Collapsed;

            }
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
