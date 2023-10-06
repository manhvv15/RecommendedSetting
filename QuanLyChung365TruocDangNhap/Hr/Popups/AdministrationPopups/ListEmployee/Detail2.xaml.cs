using QuanLyChung365TruocDangNhap.Hr.Entities.AdministrationEntity.EmployeeManager;
using QuanLyChung365TruocDangNhap.Hr.Entities.ListItemCombobox;
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

namespace QuanLyChung365TruocDangNhap.Hr.Popups.AdministrationPopups.ListEmployee
{
    /// <summary>
    /// Interaction logic for Detail2.xaml
    /// </summary>
    public partial class Detail2 : UserControl
    {
        string token;
        string ep_id;
        string com_id;
        getListEmployee1 employee;
        public Dictionary<string, string> listDepartment = new Dictionary<string, string>();
        public Dictionary<string, string> listBranch = new Dictionary<string, string>();
        public Dictionary<string, string> listTo = new Dictionary<string, string>();
        public Dictionary<string, string> listNhom = new Dictionary<string, string>();
        // deligate event hide popups
        public delegate void HidePopup(int mode);
        public static event HidePopup hidePopup;
        public Detail2(string token, string ep_id, string com_id, Dictionary<string, string> listBranch, Dictionary<string, string> listDepartment)
        {
            InitializeComponent();
            this.token = token;
            this.ep_id = ep_id;
            this.com_id = com_id;
            this.listBranch = listBranch;
            this.listDepartment = listDepartment;
            listTo.Add("", "Chọn tổ");
            listNhom.Add("", "Chọn nhóm");
            SetUpCombobox();
            GetData();
        }

        private void SetUpCombobox()
        {
            cbxGender.ItemsSource = ListItemCombobox.ListCbxGender;
            cbxEducation.ItemsSource = ListItemCombobox.ListCbxEducationEmployee;
            cbxExp.ItemsSource = ListItemCombobox.ListCbxExpEmployee;
            cbxMarried.ItemsSource = ListItemCombobox.ListMarried;
            cbxPosition.ItemsSource = ListItemCombobox.ListPositionApply;
            cbxBranch.ItemsSource = listBranch;
            cbxDepartment.ItemsSource = listDepartment;
        }

        private async void GetData()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = "http://210.245.108.202:3000/api/qlc/managerUser/list";
                Dictionary<string, string> form = new Dictionary<string, string>();
                form.Add("com_id", com_id.ToString());
                form.Add("ep_id", ep_id.ToString());

                httpRequestMessage.RequestUri = new Uri(api);
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);
                httpRequestMessage.Content = new FormUrlEncodedContent(form);
                var response = await httpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                getListEmployee result = JsonConvert.DeserializeObject<getListEmployee>(responseContent);
                if (result.data != null && result.data.items.Count > 0)
                {
                    employee = result.data.items[0];
                    cbxDepartment.SelectedItem = listDepartment.FirstOrDefault(t => t.Key == employee.dep_id);
                    GetAllTo(true);
                    BindingData();
                }
            }
            catch
            {

            }
        }

        private async void GetAllTo(bool mode) // true: binding, false: ko
        {
            if (cbxDepartment.SelectedIndex < 1)
            {
                listTo.Clear();
                listTo.Add("", "Chọn tổ");
                cbxTo.Items.Refresh();
                cbxTo.ItemsSource = listTo;
                return;
            }
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Get;
                string api = "http://210.245.108.202:3000/api/qlc/team/list" + com_id + "&id_nest=" + cbxDepartment.SelectedValue.ToString();
                httpRequestMessage.RequestUri = new Uri(api);
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);
                var form = new Dictionary<string, string>();
                form.Add("com_id", com_id);
                form.Add("dep_id", cbxDepartment.SelectedValue.ToString());
                httpRequestMessage.Content = new FormUrlEncodedContent(form); 
                var response = await httpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                APINestEntity result = JsonConvert.DeserializeObject<APINestEntity>(responseContent);
                if (result.data != null && result.data.data.Count > 0)
                {
                    listTo.Clear();
                    listTo.Add("", "Chọn tổ");
                    foreach (var item in result.data.data)
                    {
                        listTo.Add(item.team_id.ToString(), item.team_name);
                    }
                    cbxTo.Items.Refresh();
                    cbxTo.ItemsSource = listTo;
                    if (mode)
                    {
                        if(employee.parent_gr == null)
                        {
                            cbxTo.SelectedItem = listTo.FirstOrDefault(t => t.Key == employee.group_id);
                        }
                        else
                        {
                            cbxTo.SelectedItem = listTo.FirstOrDefault(t => t.Key == employee.parent_gr);
                            GetAllNhom(true);
                        }
                    }
                }
            }
            catch
            {

            }
        }

        private async void GetAllNhom(bool mode)
        {
            if (cbxTo.SelectedIndex < 1)
            {
                listNhom.Clear();
                listNhom.Add("", "Chọn nhóm");
                cbxNhom.Items.Refresh();
                cbxNhom.ItemsSource = listNhom;
                return;
            }
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = "http://210.245.108.202:3000/api/qlc/group/listAll";
                httpRequestMessage.RequestUri = new Uri(api);
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);
                var form = new Dictionary<string, string>();
                form.Add("com_id", com_id);
                form.Add("team_id", cbxTo.SelectedValue.ToString());
                httpRequestMessage.Content = new FormUrlEncodedContent(form);
                var response = await httpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                GroupEntity result = JsonConvert.DeserializeObject<GroupEntity>(responseContent);
                if (result.data != null && result.data.data.Count > 0)
                {
                    listNhom.Clear();
                    listNhom.Add("", "Chọn nhóm");
                    foreach (var item in result.data.data)
                    {
                        listNhom.Add(item.gr_id.ToString(), item.gr_name);
                    }
                    cbxNhom.Items.Refresh();
                    cbxNhom.ItemsSource = listNhom;
                    if (mode)
                    {
                        cbxNhom.SelectedItem = listNhom.FirstOrDefault(t => t.Key == employee.group_id);
                    }
                }
            }
            catch
            {

            }
        }

        private void BindingData()
        {
            try
            {
                tbID.Text = employee.ep_id;
                tbName.Text = employee.ep_name;
                tbEmail.Text = employee.ep_email;
                tbPhone.Text = employee.ep_phone;
                cbxGender.SelectedItem = ListItemCombobox.ListCbxGender.FirstOrDefault(t => t.ID == employee.ep_gender);
                dpDateOfBirth.Text = ConvertDate(employee.ep_birth_day, "MM/dd/yyyy");
                dpDateOfBirth.SelectedDate = DateTime.Parse(ConvertDate(employee.ep_birth_day, "yyyy-MM-dd"));
                cbxEducation.SelectedItem = ListItemCombobox.ListCbxEducationEmployee.FirstOrDefault(t => t.ID == employee.ep_education);
                cbxExp.SelectedItem = ListItemCombobox.ListCbxExpEmployee.FirstOrDefault(t => t.ID == employee.ep_exp);
                cbxMarried.SelectedItem = ListItemCombobox.ListMarried.FirstOrDefault(t => t.ID == employee.ep_married);
                tbAddress.Text = employee.ep_address;
                if (!string.IsNullOrEmpty(employee.start_working_time))
                {
                    dpTime.Text = ConvertDate(employee.start_working_time, "MM/dd/yyyy");
                    dpTime.SelectedDate = DateTime.Parse(ConvertDate(employee.start_working_time, "yyyy-MM-dd"));
                }
                cbxPosition.SelectedItem = ListItemCombobox.ListPositionApply.FirstOrDefault(t => t.ID == employee.position_id);
                cbxBranch.SelectedItem = listBranch.FirstOrDefault(t => t.Key == employee.com_id);
                cbxDepartment.SelectedItem = listDepartment.FirstOrDefault(t => t.Key == employee.dep_id);
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
                                    try
                                    {
                                        System.DateTime dateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);

                                        // Add the number of seconds in UNIX timestamp to be converted.
                                        dateTime = dateTime.AddSeconds(long.Parse(date));
                                        return dateTime.ToString(format);
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

        }

        private void CancelPopup(object sender, MouseButtonEventArgs e)
        {
            hidePopup(0);
        }

        private void cbxDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GetAllTo(false);
        }

        private void cbxTo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GetAllNhom(false);
        }

        private void UpdateEmployee(object sender, MouseButtonEventArgs e)
        {
            if(ValidateForm())
                UpdateEmployee();
        }

        private async void UpdateEmployee()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.edit_employee;
                httpRequestMessage.RequestUri = new Uri(api);
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);
                var parameters = new List<KeyValuePair<string, string>>();
                parameters.Add(new KeyValuePair<string, string>("idQLC", ep_id));
                parameters.Add(new KeyValuePair<string, string>("userName", tbName.Text));
                parameters.Add(new KeyValuePair<string, string>("phoneTK", tbPhone.Text));
                parameters.Add(new KeyValuePair<string, string>("address", tbAddress.Text));
                parameters.Add(new KeyValuePair<string, string>("birthday", dpDateOfBirth.SelectedDate.Value.ToString("yyyy-MM-dd")));
                parameters.Add(new KeyValuePair<string, string>("gender", cbxGender.SelectedValue.ToString()));
                parameters.Add(new KeyValuePair<string, string>("married", cbxMarried.SelectedValue.ToString()));
                if(cbxBranch.SelectedValue != null)
                    parameters.Add(new KeyValuePair<string, string>("com_id", cbxBranch.SelectedValue.ToString()));
                if(cbxDepartment.SelectedValue != null)
                    parameters.Add(new KeyValuePair<string, string>("dep_id", cbxDepartment.SelectedValue.ToString()));
                parameters.Add(new KeyValuePair<string, string>("position_id", cbxPosition.SelectedValue.ToString()));
                parameters.Add(new KeyValuePair<string, string>("experience", cbxExp.SelectedValue.ToString()));
                parameters.Add(new KeyValuePair<string, string>("education", cbxEducation.SelectedValue.ToString()));
                if(cbxTo.SelectedIndex > -1)
                {
                    parameters.Add(new KeyValuePair<string, string>("team_id", cbxTo.SelectedValue.ToString()));
                }
                else
                {
                    parameters.Add(new KeyValuePair<string, string>("team_id", ""));
                }
                if (cbxNhom.SelectedIndex > -1)
                {
                    parameters.Add(new KeyValuePair<string, string>("group_id", cbxNhom.SelectedValue.ToString()));
                }
                else
                {
                    parameters.Add(new KeyValuePair<string, string>("group_id", ""));
                }
                if(dpTime.SelectedDate != null)
                    parameters.Add(new KeyValuePair<string, string>("start_working_time", dpTime.SelectedDate.Value.ToString("yyyy-MM-dd")));

                // Thiết lập Content
                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;
                var response = await httpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                ResultEntity result = JsonConvert.DeserializeObject<ResultEntity>(responseContent);
                if (result.data != null && result.data.result)
                {
                    hidePopup(1);
                    MessageBox.Show("Cập nhật thành công!");
                }
                else
                {
                    MessageBox.Show("Cập nhật thất bại, vui lòng thử lại");
                }
            }
            catch
            {
                MessageBox.Show("Có lỗi xảy ra, vui lòng thử lại");
            }
        }

        private bool ValidateForm()
        {
            if(tbName.Text.Trim() == "")
            {
                MessageBox.Show("Tên nhân viên không được để trống.");
                return false;
            }

            if(tbPhone.Text.Trim() == "")
            {
                MessageBox.Show("Số điện thoại không được để trống.");
                return false;
            }
            Regex regexPhone  = new Regex(@"^((09|03|07|08|05|04)+([0-9]{8})\b)$", RegexOptions.CultureInvariant | RegexOptions.Singleline);
            if (!regexPhone.IsMatch(tbPhone.ToString()))
            {
                MessageBox.Show("Số điện thoại không đúng định dạng.");
            }
            if (tbPhone.Text.Trim().Length < 9)
            {
                MessageBox.Show("Số điện thoại không đúng định dạng.");
                return false;
            }
            
            if (tbPhone.Text.Length < 10)
            {
                MessageBox.Show("Số điện thoại không đúng định dạng.");
                return false;
            }
            if (tbPhone.Text.Length > 15)
            {
                MessageBox.Show("Số điện thoại không đúng định dạng.");
                return false;
            }


            return true;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
