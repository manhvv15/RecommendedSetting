using QuanLyChung365TruocDangNhap.Hr.Entities.AdministrationEntity.EmployeeManager;
using QuanLyChung365TruocDangNhap.Hr.Entities.AdministrationEntity.PersonnelChangeEntity;
using QuanLyChung365TruocDangNhap.Hr.Entities.ManageRecuitmentEntities.ListCandidateEntities;
using QuanLyChung365TruocDangNhap.Hr.Entities.SalaryAndBenefits;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
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
using static QuanLyChung365TruocDangNhap.Hr.Popups.SarlaryAndBenefitPopups.BonusPopups.AddListAchievementsPopup;

namespace QuanLyChung365TruocDangNhap.Hr.Popups.SarlaryAndBenefitPopups.BonusPopups
{
    /// <summary>
    /// Interaction logic for EditIndividualAchievementsPopup.xaml
    /// </summary>
    /// \
    public class cboxValue3
    {
        public cboxValue3(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }
        public string Value { get; set; }
    }
    
    public partial class EditIndividualAchievementsPopup : UserControl
    {
        // deligate event hide popups
        public delegate void HidePopup(int mode);
        public static event HidePopup hidePopup;
        string token;
        string comid;
        string id;
        string achievement_id;
        string content;
        string dep_id;
        string created_by;
        string achievement_at;
        string achievement_type;
        string appellation;
        string achievement_level;

        //string dep_id;
        List<Items6> listDepartment = new List<Items6>();
        public EditIndividualAchievementsPopup(string token, string comid,string id, string achievement_id, string content, string dep_id, string achievement_at, string achievement_type, string appellation, string achievement_level, string created_by, string achievement_name)
        {
            InitializeComponent();
            this.token = token;
            this.comid = comid;
            this.id = id;
            GetDatalistDepartment();

            List<cboxValue3> list = new List<cboxValue3>();
            list.Add(new cboxValue3("Huân chương", "1"));
            list.Add(new cboxValue3("Huy chương", "2"));
            list.Add(new cboxValue3("Giấy khen", "3"));
            list.Add(new cboxValue3("Thăng chức", "4"));
            list.Add(new cboxValue3("Kỉ niệm chương", "5"));
            list.Add(new cboxValue3("Tiền mặt", "6"));
            cbx1.ItemsSource = list;

            //List<cboxValue4> listA = new List<cboxValue4>();

            this.achievement_id = achievement_id;
            this.content = content;
            this.dep_id = dep_id;
            this.created_by = created_by;
            this.achievement_at = achievement_at;
            this.appellation = appellation;
            this.achievement_level = achievement_level;

            txtSoQD.Text = achievement_id;
            txtNDKhenThuong.Text = content;
            txtNguoiKyQD.Text = created_by;
            DTdate.Text = ConvertDate(achievement_at);
            DTdate.SelectedDate = DateTime.Parse(ConvertDate(achievement_at));
            cbx1.SelectedItem = list.FirstOrDefault(t => t.Value == achievement_type);
            txtDanhHieu.Text = appellation;
            txtCapKhen.Text = achievement_level;

            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy"; //for the second type
            Thread.CurrentThread.CurrentCulture = ci;
        }
        private void CancelPopup(object sender, MouseButtonEventArgs e)
        {
            hidePopup(0);
        }
        private void EditAchieven(object sender, MouseButtonEventArgs e)
        {
            if(ValidateForm())
                EditAchievenGetData();
        }
        private async void EditAchievenGetData()
        {
            var httpClient = new HttpClient();
            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = HttpMethod.Put;
            string api = APIs.API.updateAchievementGroup;

            InputAchievement input = new InputAchievement();
            input.achievement_level = txtCapKhen.Text;
            input.achievement_id = txtSoQD.Text;
            input.content = txtNDKhenThuong.Text;
            input.achievement_at = DTdate.SelectedDate.Value.ToString("yyyy-MM-dd");
            input.created_by = txtNguoiKyQD.Text;
            input.appellation = txtDanhHieu.Text;
            input.achievement_type = int.Parse(cbx1.SelectedValue.ToString());
            input.id = int.Parse(id);
            input.depId = int.Parse(cbxTenPhongBan.SelectedValue.ToString());
            input.depName = cbxTenPhongBan.Text;
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            // Thiết lập Content
            var content = new StringContent(JsonConvert.SerializeObject(input), null, "application/json");
            httpRequestMessage.Content = content;
            httpRequestMessage.RequestUri = new Uri(api);
            httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);
            

            // Thực hiện Post
            var response = await httpClient.SendAsync(httpRequestMessage);

            var responseContent = await response.Content.ReadAsStringAsync();

            ProcessEntity result = JsonConvert.DeserializeObject<ProcessEntity>(responseContent);

            if (result.data.result)
            {
                hidePopup(1);
                MessageBox.Show("Sửa khen thưởng thành công!");
            }
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
                cbxTenPhongBan.SelectedItem = listDepartment.FirstOrDefault(t => t.dep_id == dep_id);
            }
            catch
            {

            }
        }

        private string ConvertDate(string date)
        {
            DateTime myDate;
            try
            {
                myDate = DateTime.ParseExact(date, "dd/MM/yyyy",
                                              System.Globalization.CultureInfo.InvariantCulture);
                return myDate.ToString("MM/dd/yyyy");
            }
            catch
            {
                try
                {
                    myDate = DateTime.ParseExact(date, "d/M/yyyy",
                                                  System.Globalization.CultureInfo.InvariantCulture);
                    return myDate.ToString("MM/dd/yyyy");
                }
                catch
                {
                    return "";
                }
            }
        }

        private bool ValidateForm()
        {
            if (txtSoQD.Text.Trim() == "")
            {
                MessageBox.Show("Số quy định không được để trống.");
                return false;
            }

            if (txtNDKhenThuong.Text.Trim() == "")
            {
                MessageBox.Show("Nội dung khen thưởng không được để trống.");
                return false;
            }

            if (cbxTenPhongBan.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn tên phòng ban.");
                return false;
            }

            if (txtNguoiKyQD.Text.Trim() == "")
            {
                MessageBox.Show("Người ký quyết định không được để trống.");
                return false;
            }

            if (DTdate.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng chọn thời điểm.");
                return false;
            }

            if (cbx1.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn hình thức khen thưởng.");
                return false;
            }

            if (txtDanhHieu.Text.Trim() == "")
            {
                MessageBox.Show("Danh hiệu không được để trống.");
                return false;
            }

            if (txtCapKhen.Text.Trim() == "")
            {
                MessageBox.Show("Cấp khen không được để trống.");
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
    }
}
