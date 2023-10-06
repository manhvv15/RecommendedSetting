using QuanLyChung365TruocDangNhap.Hr.Entities.AdministrationEntity.EmployeeManager;
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
    /// Interaction logic for PersonalEditViolationPopup.xaml
    /// </summary>
    /// 

    public partial class PersonalEditViolationPopup : UserControl
    {
        // deligate event hide popups
        public delegate void HidePopup(int mode);
        public static event HidePopup hidePopup;
        List<getListEmployee1> listEmployees = new List<getListEmployee1>();
        public string token;
        public string id { get; set; }
        public string infringe_name { get; set; }
        public string regulatory_basis { get; set; }
        public string number_violation { get; set; }
        public string infringe_at { get; set; }
        public string created_by { get; set; }
        public string infringe_type { get; set; }
        public string list_user_name { get; set; }
        public string list_user { get; set; }
        public PersonalEditViolationPopup(string token, string id, string infringe_name, string regulatory_basis, string number_violation, string infringe_at, string infringe_type, string created_by, string list_user, string list_user_name)
        {
            this.token = token;
            this.id = id;
            this.list_user_name = list_user_name;
            this.list_user = list_user;
            InitializeComponent();
            GetDataListEmployee();

            tbTenLoiVP.Text = infringe_name;
            tbCanCuQD.Text = regulatory_basis;
            tbSoQDXuLyVP.Text = number_violation;
            DTdate.Text = ConvertDate(infringe_at);
            DTdate.SelectedDate = DateTime.Parse(ConvertDate(infringe_at));
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
        private async void GetDataListEmployee()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.getlistEmplouyee;

                httpRequestMessage.RequestUri = new Uri(api);
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);
                Dictionary<string, string> form = new Dictionary<string, string>();
                form.Add("com_id", id);
                httpRequestMessage.Content = new FormUrlEncodedContent(form);
                var response = await httpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                getListEmployee result = JsonConvert.DeserializeObject<getListEmployee>(responseContent);

                listEmployees = result.data.items;
                cbxTenDoiTuong.ItemsSource = listEmployees;
                cbxTenDoiTuong.SelectedItem = listEmployees.FirstOrDefault(t => t.ep_id == list_user);

            }
            catch (Exception)
            {
                //MessageBox.Show("Error");
            }
        }
        private void EditViolationPopup(object sender, MouseButtonEventArgs e)
        {
            if(ValidateForm())
                EditDataViolationPopup();
        }
        private async void EditDataViolationPopup()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Put;
                string api = APIs.API.updateInfinges;

                httpRequestMessage.RequestUri = new Uri(api);
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);
                var content = new MultipartFormDataContent();
                content.Add(new StringContent(tbTenLoiVP.Text), "infringe_name");
                content.Add(new StringContent(tbCanCuQD.Text), "regulatory_basis");
                content.Add(new StringContent(cbxTenDoiTuong.SelectedValue.ToString()), "list_user[]");
                content.Add(new StringContent(cbxTenDoiTuong.Text), "list_user_name[]");
                content.Add(new StringContent(tbNguoiKyQD.Text), "created_by");
                content.Add(new StringContent(DTdate.SelectedDate.Value.ToString("yyyy-MM-dd")), "infringe_at");
                content.Add(new StringContent(tbHinhThucXLVP.Text), "infringe_type");
                content.Add(new StringContent(tbSoQDXuLyVP.Text), "number_violation");
                content.Add(new StringContent(id), "id");
                // Thiết lập Content
                //var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                ProcessEntity result = JsonConvert.DeserializeObject<ProcessEntity>(responseContent);

                if (result.data.result)
                {
                    hidePopup(1);
                    MessageBox.Show("Chỉnh sửa kỷ luật thành công!");
                }
            }
            catch { }
            
        }

        private bool ValidateForm()
        {
            if (tbTenLoiVP.Text.Trim() == "")
            {
                MessageBox.Show("Tên lỗi vi phạm không được để trống.");
                return false;
            }

            if (tbCanCuQD.Text.Trim() == "")
            {
                MessageBox.Show("Căn cứ quy định không được để trống.");
                return false;
            }

            if (tbSoQDXuLyVP.Text.Trim() == "")
            {
                MessageBox.Show("Số quy định xử lý vi phạm không được để trống.");
                return false;
            }

            if (DTdate.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng điền thời gian vi phạm");
                return false;
            }

            if (tbHinhThucXLVP.Text.Trim() == "")
            {
                MessageBox.Show("Hình thức xử lý vi phạm không được để trống");
                return false;
            }

            if (cbxTenDoiTuong.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn tên đối tượng");
                return false;
            }

            if (tbNguoiKyQD.Text.Trim() == "")
            {
                MessageBox.Show("Người ký quyết định không được để trống.");
                return false;
            }

            return true;
        }

        private string ConvertDate(string date)
        {
            DateTime myDate;
            try
            {
                myDate = DateTime.ParseExact(date, "dd/MM/yyyy",
                                              System.Globalization.CultureInfo.InvariantCulture);
                return myDate.ToString("dd/MM/yyyy");
            }
            catch
            {
                try
                {
                    myDate = DateTime.ParseExact(date, "dd/MM/yyyy",
                                                  System.Globalization.CultureInfo.InvariantCulture);
                    return myDate.ToString("dd/MM/yyyy");
                }
                catch
                {
                    return "";
                }
            }
        }
        private void cbxTenDoiTuong_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            cbxTenDoiTuong.SelectedIndex = -1;
            string textSearch = cbxTenDoiTuong.Text + e.Text;
            cbxTenDoiTuong.IsDropDownOpen = true;
            if (textSearch == "")
            {
                cbxTenDoiTuong.Text = "";
                cbxTenDoiTuong.Items.Refresh();
                cbxTenDoiTuong.ItemsSource = listEmployees;
                cbxTenDoiTuong.SelectedIndex = -1;
            }
            else
            {
                cbxTenDoiTuong.ItemsSource = "";
                cbxTenDoiTuong.Items.Refresh();
                cbxTenDoiTuong.ItemsSource = listEmployees.Where(t => t.ep_name.ToLower().Contains(textSearch.ToLower()));
            }
        }

        private void cbxTenDoiTuong_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                cbxTenDoiTuong.SelectedIndex = -1;
                string textSearch = cbxTenDoiTuong.Text;
                cbxTenDoiTuong.Items.Refresh();
                cbxTenDoiTuong.ItemsSource = listEmployees.Where(t => t.ep_name.ToLower().Contains(textSearch.ToLower()));
            }
        }
    }
}
