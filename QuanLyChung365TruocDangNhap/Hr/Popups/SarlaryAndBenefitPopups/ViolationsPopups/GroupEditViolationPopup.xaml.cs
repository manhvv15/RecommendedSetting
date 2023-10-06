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

namespace QuanLyChung365TruocDangNhap.Hr.Popups.SarlaryAndBenefitPopups.ViolationsPopups
{
    /// <summary>
    /// Interaction logic for GroupEditViolationPopup.xaml
    /// </summary>
    public partial class GroupEditViolationPopup : UserControl
    {
        // deligate event hide popups
        public delegate void HidePopup(int mode);
        public static event HidePopup hidePopup;

        string token;
        string comid;
        string dep_id;
        string infringe_name;
        string created_by;
        string infringe_at;
        string infringe_type;
        string number_violation;
        string regulatory_basis;
        string id;

        List<Items6> listDepartment = new List<Items6>();
        public GroupEditViolationPopup(string token,string comid, string dep_id, string infringe_name, string created_by, string infringe_at, string infringe_type, string number_violation, string regulatory_basis, string id)
        {
            this.token = token;
            this.comid = comid;
            this.id = id;
            this.dep_id = dep_id;
            InitializeComponent();
            GetDatalistDepartment();
            tbTenLoiVP.Text = infringe_name;
            tbCanCuQD.Text = regulatory_basis;
            tbSoQDXLVP.Text = number_violation;
            DTdate.Text = infringe_at;
            DTdate.SelectedDate = DateTime.Parse(infringe_at);
            tbHinhThucXLVP.Text = infringe_type;
            tbNguoiKyQD.Text = created_by;

            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy"; //for the second type
            Thread.CurrentThread.CurrentCulture = ci;
        }
        private void CancelPopup(object sender, MouseButtonEventArgs e)
        {
            hidePopup(0);
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
            cbxTenPhongBan.SelectedItem = listDepartment.FirstOrDefault(t => t.dep_id == dep_id);

        }
        private void UpdateNewGroupViolation(object sender, MouseButtonEventArgs e)
        {
            UpdateDataGroupViolation();
        }
        public class InputViPhamNhom
        {
            public string regulatory_basis { get; set; }
            public string infringe_name { get; set; }
            public string created_by { get; set; }
            public string infringe_at { get; set; }
            public int infringe_type { get; set; }
            public string number_violation { get; set; }
            public List<int> list_user { get; set; } = new List<int>();
            public List<string> list_user_name { get; set; } = new List<string>();
            public List<object> dep_id { get; set; }
            public List<object> dep_name { get; set; }
            public int price { get; set; }
        }
        private async void UpdateDataGroupViolation()
        {
            var httpClient = new HttpClient();
            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = HttpMethod.Put;
            string api = APIs.API.updateInfingesGroup;

            httpRequestMessage.RequestUri = new Uri(api);
            var parameters = new List<KeyValuePair<string, string>>();
            httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);
            InputViPhamNhom inputViPhamNhom= new InputViPhamNhom();
            //inputViPhamNhom.dep_name = cbxTenPhongBan.Text;
            //inputViPhamNhom.dep_id = cbxTenPhongBan.SelectedValue.ToString();
            parameters.Add(new KeyValuePair<string, string>("depId", cbxTenPhongBan.SelectedValue.ToString()));
            parameters.Add(new KeyValuePair<string, string>("depName", cbxTenPhongBan.Text));
            parameters.Add(new KeyValuePair<string, string>("infringe_name", tbTenLoiVP.Text));
            parameters.Add(new KeyValuePair<string, string>("created_by", tbNguoiKyQD.Text));
            parameters.Add(new KeyValuePair<string, string>("infringe_at", DTdate.SelectedDate.Value.ToString("yyyy-MM-dd")));
            parameters.Add(new KeyValuePair<string, string>("infringe_type", tbHinhThucXLVP.Text));
            parameters.Add(new KeyValuePair<string, string>("number_violation", tbSoQDXLVP.Text));
            parameters.Add(new KeyValuePair<string, string>("regulatory_basis", tbCanCuQD.Text));

            parameters.Add(new KeyValuePair<string, string>("id", id));


            // Thiết lập Content
            var content = new FormUrlEncodedContent(parameters);
            httpRequestMessage.Content = content;

            // Thực hiện Post
            var response = await httpClient.SendAsync(httpRequestMessage);
            var responseContent = await response.Content.ReadAsStringAsync();

            ProcessEntity result = JsonConvert.DeserializeObject<ProcessEntity>(responseContent);

            if (result.result)
            {
                hidePopup(1);
                MessageBox.Show("Chỉnh sửa kỷ luật thành công!");
            }
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
