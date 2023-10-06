using QuanLyChung365TruocDangNhap.Hr.Entities.AdministrationEntity.PersonnelChangeEntity;
using QuanLyChung365TruocDangNhap.Hr.Entities.ListItemCombobox;
using QuanLyChung365TruocDangNhap.Hr.View;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

namespace QuanLyChung365TruocDangNhap.Hr.Popups.AdministrationPopups.BreakTheRules
{
    /// <summary>
    /// Interaction logic for AddNewPopup.xaml
    /// </summary>
    public partial class AddNewPopup : UserControl
    {
        string token;
        string id;
        int TypeUser;
        int page_now = 1;
        const int COUNT_RECORD_PER_PAGE = 1000;

        // deligate event hide popups
        public delegate void HidePopup(int mode); //0: 0 load lại, 1:load lại
        public static event HidePopup hidePopup;

        List<Items3> listEmployee = new List<Items3>();
        List<Items6> listDepartment = new List<Items6>();
        List<Items7> listCom = new List<Items7>();

        public AddNewPopup(string token, string id, int TypeUser)
        {
            InitializeComponent();
            this.token = token;
            this.id = id;
            this.TypeUser = TypeUser;
            GetDatalistEmployee();
            SetUpCombobox();
            GetDatalistDepartment();
            GetDatalistCom();
        }
        private void SetUpCombobox()
        {
            cbxChucvu.ItemsSource = ListItemCombobox.ListPositionApply;
        }
        private void CancelPopup(object sender, MouseButtonEventArgs e)
        {
            hidePopup(0);
        }

        //Danh sách công ty
        private async void GetDatalistCom()
        {

            var httpClient = new HttpClient();
            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = HttpMethod.Post;
            string api = APIs.API.listCompany;

            httpRequestMessage.RequestUri = new Uri(api);
            httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);

            var response = await httpClient.SendAsync(httpRequestMessage);
            var responseContent = await response.Content.ReadAsStringAsync();

            listCompany result = JsonConvert.DeserializeObject<listCompany>(responseContent);
            listCom = result.data.items;
            cbxCompany.ItemsSource = listCom;
        }

        //Lấy toàn bộ danh sách phòng ban
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
                listDepartment = result.data.items;
                cbxPhongBan.ItemsSource = listDepartment;
            }
            catch { }
        }

        //lấy toàn bộ danh sách nhân viên 
        private async void GetDatalistEmployee()
        {
            var httpClient = new HttpClient();
            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = HttpMethod.Post;


            string api = "http://210.245.108.202:3000/api/qlc/managerUser/list";

            Dictionary<string, string> form = new Dictionary<string, string>();
            form.Add("com_id", id.ToString());
            form.Add("pageNumber", page_now.ToString());
            form.Add("pageSize", COUNT_RECORD_PER_PAGE.ToString());

            httpRequestMessage.RequestUri = new Uri(api);
            httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);
            httpRequestMessage.Content = new FormUrlEncodedContent(form);
            var response = await httpClient.SendAsync(httpRequestMessage);
            var responseContent = await response.Content.ReadAsStringAsync();

            getListEmployee result = JsonConvert.DeserializeObject<getListEmployee>(responseContent);
            try
            {
                listEmployee = result.data.items;
                foreach (var item in listEmployee)
                {
                    string dep_name = " (Chưa cập nhật)";
                    if (item.dep_name != null && item.dep_name != "")
                    {
                        dep_name = " (" + item.dep_name + ")";
                    }
                    item.ep_name = item.ep_name + dep_name;


                }
                cbxTenNV.ItemsSource = listEmployee;
            }
            catch
            {

            }

        }

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
                cbxTenNV.ItemsSource = listEmployee.Where(t => t.ep_name.ToLower().Contains(textSearch.ToLower()));
            }
        }

        private void cbxTenNV_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                cbxTenNV.SelectedIndex = -1;
                string textSearch = cbxTenNV.Text;
                cbxTenNV.Items.Refresh();
                cbxTenNV.ItemsSource = listEmployee.Where(t => t.ep_name.ToLower().Contains(textSearch.ToLower()));
            }
        }

        //công ty
        private void cbxCompany_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            cbxCompany.SelectedIndex = -1;
            string textSearch = cbxCompany.Text + e.Text;
            cbxCompany.IsDropDownOpen = true;
            if (textSearch == "")
            {
                cbxCompany.Text = "";
                cbxCompany.Items.Refresh();
                cbxCompany.ItemsSource = listCom;
                cbxCompany.SelectedIndex = -1;
            }
            else
            {
                cbxCompany.ItemsSource = "";
                cbxCompany.Items.Refresh();
                cbxCompany.ItemsSource = listCom.Where(t => t.com_name.ToLower().Contains(textSearch.ToLower()));
            }
        }

        private void cbxCompany_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                cbxCompany.SelectedIndex = -1;
                string textSearch = cbxCompany.Text;
                cbxCompany.Items.Refresh();
                cbxCompany.ItemsSource = listCom.Where(t => t.com_name.ToLower().Contains(textSearch.ToLower()));
            }
        }

        private void AddNewData(object sender, MouseButtonEventArgs e)
        {
            DisplayDataAdd();
        }
        private async void DisplayDataAdd()
        {
            /*var httpClient = new HttpClient();
            var httpRequestMessage = new HttpRequestMessage();

            httpRequestMessage.Method = HttpMethod.Post;
            httpRequestMessage.RequestUri = new Uri("https://chamcong.24hpay.vn/api_web_hr/update_employe_resign_quit_job.php");
            httpRequestMessage.Headers.Add("Authorization", token);
            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("id_ep", cbxTenNV.SelectedValue.ToString()));
            parameters.Add(new KeyValuePair<string, string>("com_id",id));
            if(cbxChucvu.SelectedValue !=null)
            parameters.Add(new KeyValuePair<string, string>("current_position", cbxChucvu.SelectedValue.ToString()));
            if(cbxPhongBan.SelectedValue != null)
            parameters.Add(new KeyValuePair<string, string>("current_dep_id", cbxPhongBan.SelectedValue.ToString()));
            parameters.Add(new KeyValuePair<string, string>("created_at", ConvertToUnixTimestamp(Convert.ToDateTime(DTdate.Text)).ToString()));
            string note = StringFromRichTextBox(rtbLyDo);
            parameters.Add(new KeyValuePair<string, string>("note", note));

            var content = new FormUrlEncodedContent(parameters);
            httpRequestMessage.Content = content;
            var response = await httpClient.SendAsync(httpRequestMessage);
            var responseContent = response.Content.ReadAsStringAsync().Result;
            EntitySuccessMessage result = JsonConvert.DeserializeObject<EntitySuccessMessage>(responseContent);
            if (result.data != null)
            {
                hidePopup(1);
                MessageBox.Show("Thêm thành công");
            }
            else
            {
                MessageBox.Show(result.error.message);
            }*/

            using (WebClient web = new WebClient())
            {
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.GetType() == typeof(HomeView))
                    {
                        (window as HomeView).Loadding.Visibility = Visibility.Visible;
                    }
                };
                web.Headers.Add("Authorization", "Bearer " + token);
                web.QueryString.Add("ep_id", cbxTenNV.SelectedValue.ToString());
                web.QueryString.Add("com_id", id);
                web.QueryString.Add("current_position", cbxChucvu.SelectedValue.ToString());
                web.QueryString.Add("current_dep_id", cbxPhongBan.SelectedValue.ToString());
                web.QueryString.Add("created_at", DTdate.SelectedDate.Value.ToString("yyyy-MM-dd"));
                string note = StringFromRichTextBox(rtbLyDo);
                web.QueryString.Add("note", note);

                web.UploadValuesCompleted += (s, e) =>
                {
                    EntitySuccessMessage result = JsonConvert.DeserializeObject<EntitySuccessMessage>(UnicodeEncoding.UTF8.GetString(e.Result));
                    if (result.data != null)
                    {
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(HomeView))
                            {
                                (window as HomeView).Loadding.Visibility = Visibility.Collapsed;
                            }
                        };
                        hidePopup(1);
                        MessageBox.Show("Thêm thành công");
                    }
                    else
                    {
                        MessageBox.Show(result.error.message);
                    }
                };
                web.UploadValuesTaskAsync("http://210.245.108.202:3006/api/hr/personalChange/updateQuitJobNew", web.QueryString);
            }
        }
        private string StringFromRichTextBox(RichTextBox rtb)
        {
            TextRange textRange = new TextRange(
                rtb.Document.ContentStart,
                rtb.Document.ContentEnd
            );

            return textRange.Text;
        }
        public double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
        }
        public T GetFirstChildOfType<T>(DependencyObject dependencyObject) where T : DependencyObject
        {
            if (dependencyObject == null)
            {
                return null;
            }

            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(dependencyObject); i++)
            {
                var child = VisualTreeHelper.GetChild(dependencyObject, i);

                var result = (child as T) ?? GetFirstChildOfType<T>(child);

                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }
        private void cbxTenNV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            cb.IsEditable = false;
            cbxChucvu.ForceCursor = true;
            cb.IsEditable = true;
            TextBox txt = GetFirstChildOfType<TextBox>(cbxTenNV);
            if (txt != null)
            {
                txt.SelectionStart = 0;
                txt.SelectionLength = 0;
            }
        }
    }
}
