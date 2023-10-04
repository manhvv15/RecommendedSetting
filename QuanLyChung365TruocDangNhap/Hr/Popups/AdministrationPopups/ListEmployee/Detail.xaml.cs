using QuanLyChung365TruocDangNhap.Hr.Entities.AdministrationEntity.EmployeeManager;
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

namespace QuanLyChung365TruocDangNhap.Hr.Popups.AdministrationPopups.ListEmployee
{
    /// <summary>
    /// Interaction logic for Detail.xaml
    /// </summary>
    public partial class Detail : UserControl
    {
        string token;
        string ep_id;
        string com_id;
        getListEmployee1 employee;
        // deligate event hide popups
        public delegate void HidePopup(int mode);
        public static event HidePopup hidePopup;
        public Detail(string token, string ep_id, string com_id)
        {
            InitializeComponent();
            this.token = token;
            this.ep_id = ep_id;
            this.com_id = com_id;
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
                    BindingData();
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
                dpDateOfBirth.SelectedDate = DateTime.Parse(ConvertDate(employee.ep_birth_day, "MM/dd/yyyy"));
                cbxEducation.SelectedItem = ListItemCombobox.ListCbxEducationEmployee.FirstOrDefault(t => t.ID == employee.ep_education);
                cbxExp.SelectedItem = ListItemCombobox.ListCbxExpEmployee.FirstOrDefault(t => t.ID == employee.ep_exp);
                cbxMarried.SelectedItem = ListItemCombobox.ListMarried.FirstOrDefault(t => t.ID == employee.ep_married);
                tbAddress.Text = employee.ep_address;
                dpTime.Text = ConvertDate(employee.start_working_time, "MM/dd/yyyy");
                dpTime.SelectedDate = DateTime.Parse(ConvertDate(employee.start_working_time, "MM/dd/yyyy"));
                cbxPosition.SelectedItem = ListItemCombobox.ListPositionApply.FirstOrDefault(t => t.ID == employee.position_id);
                tbBranch.Text = employee.com_name;
                tbPart.Text = employee.dep_name;
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
                                    return "";
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
    }
}
