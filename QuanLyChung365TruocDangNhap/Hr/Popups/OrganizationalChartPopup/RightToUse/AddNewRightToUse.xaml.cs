using QuanLyChung365TruocDangNhap.Hr.Entities.AdministrationEntity.PersonnelChangeEntity;
using QuanLyChung365TruocDangNhap.Hr.Entities.ListItemCombobox;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
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

namespace QuanLyChung365TruocDangNhap.Hr.Popups.OrganizationalChartPopup.RightToUse
{
    /// <summary>
    /// Interaction logic for AddNewRightToUse.xaml
    /// </summary>
    public partial class AddNewRightToUse : UserControl
    {
        string token;
        string id;

        public static Dictionary<string, string> listDepartment = new Dictionary<string, string>(); // save list Department
        public Dictionary<string, string> listEmployee = new Dictionary<string, string>(); // save list Employee


        // deligate event hide popups
        public delegate void HidePopup(int mode); //0: 0 load lại, 1:load lại
        public static event HidePopup hidePopup;


        public AddNewRightToUse(string token,string id)
        {
            InitializeComponent();
            this.token = token;
            this.id = id;

            GetAllData();
        }

        //get all void
        private void GetAllData()
        {
            GetDatalistDepartment();
            cbxChucvu.ItemsSource = ListItemCombobox.ListPositionApply;
            GetDatalistEmployee();
        }
        //end getall void


        //get list Department
        private async void GetDatalistDepartment()
        {
            try
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

                if (result.data == null) return;

                listDepartment = new Dictionary<string, string>();
                listDepartment.Add("", "Tất cả phòng ban");

                foreach (var item in result.data.items)
                {
                    listDepartment.Add(item.dep_id, item.dep_name);
                }
                cbxTenPhongBan.Items.Refresh();
                cbxTenPhongBan.ItemsSource = listDepartment;
                return;
            }
            catch (Exception)
            {
                MessageBox.Show("Có lỗi xảy ra,vui lòng kiểm tra lại!");
            }
        }
        //End list Department

        //get list employee
        private async void GetDatalistEmployee()
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


                if (result.data == null) return;
                listEmployee.Add("", "Chọn nhân viên");

                foreach (var item in result.data.items)
                {
                    listEmployee.Add(item.ep_id, item.ep_name);
                }
                cbxTenNV.Items.Refresh();
                cbxTenNV.ItemsSource = listEmployee;
                return;
            }
            catch (Exception)
            {
                MessageBox.Show("Có lỗi xảy ra,vui lòng kiểm tra lại!");
            }
        }
        //end get list employee


        //search input cbx listDepartment

        private void ShowCombobox(object sender, MouseButtonEventArgs e)
        {
            cbxTenPhongBan.IsDropDownOpen = !cbxTenPhongBan.IsDropDownOpen;
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
                cbxTenPhongBan.ItemsSource = listDepartment.Where(t => t.Value.ToLower().Contains(textSearch.ToLower()));
            }
        }

        private void cbxTenPhongBan_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                cbxTenPhongBan.SelectedIndex = -1;
                string textSearch = cbxTenPhongBan.Text;
                cbxTenPhongBan.Items.Refresh();
                cbxTenPhongBan.ItemsSource = listDepartment.Where(t => t.Value.ToLower().Contains(textSearch.ToLower()));
            }
        }

        //End search input cbx listDepartment

        //search input cbx ListEmployee

        private void cbxTenNV_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            cbxTenNV.SelectedIndex = -1;
            string textSearch = cbxTenNV.Text + e.Text;
            cbxTenNV.IsDropDownOpen = true;
            if (textSearch == "")
            {
                cbxTenNV.Text = "";
                cbxTenNV.Items.Refresh();
                cbxTenNV.ItemsSource = listEmployee;
                cbxTenNV.SelectedIndex = -1;
            }
            else
            {
                cbxTenNV.ItemsSource = "";
                cbxTenNV.Items.Refresh();
                cbxTenNV.ItemsSource = listEmployee.Where(t => t.Value.ToLower().Contains(textSearch.ToLower()));
            }
        }

        private void cbxTenNV_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                cbxTenNV.SelectedIndex = -1;
                string textSearch = cbxTenNV.Text;
                cbxTenNV.Items.Refresh();
                cbxTenNV.ItemsSource = listEmployee.Where(t => t.Value.ToLower().Contains(textSearch.ToLower()));
            }
        }
        //End search input cbx ListEmployee

        private void CancelPopup(object sender, MouseButtonEventArgs e)
        {
            hidePopup(0);
        }

        private void AddAchievementsPopup(object sender, MouseButtonEventArgs e)
        {
            AddData();
        }

        private async void AddData()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();

                httpRequestMessage.Method = HttpMethod.Post;
                string api = "http://210.245.108.202:3006/api/hr/organizationalStructure/updateEmpUseSignature";

                httpRequestMessage.RequestUri = new Uri(api);
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);
                var parameters = new List<KeyValuePair<string, string>>();

                parameters.Add(new KeyValuePair<string, string>("empId", cbxTenNV.SelectedValue.ToString()));
                //parameters.Add(new KeyValuePair<string, string>("ep_signature", "1"));


                // Thiết lập Content
                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                var response = await httpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();
                EntitySuccessMessage result = JsonConvert.DeserializeObject<EntitySuccessMessage>(responseContent);
                if (result.data != null && result.data.result)
                {
                    hidePopup(1);
                    MessageBox.Show("Thêm thành công");
                }
                else
                {
                    MessageBox.Show(result.data.message);
                }
            }
            catch
            {
                MessageBox.Show("Có lỗi xảy ra, vui lòng thử lại!");
            }
        }

    }
}
