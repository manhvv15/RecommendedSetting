using QuanLyChung365TruocDangNhap.ChamCong365.Core;
using QuanLyChung365TruocDangNhap.ChamCong365.Entities;
using QuanLyChung365TruocDangNhap.ChamCong365.Entities.Company;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
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

namespace QuanLyChung365TruocDangNhap.ChamCong365.Views.PopUp.PopUp_Company_Child
{
    /// <summary>
    /// Interaction logic for Add_More_Company.xaml
    /// </summary>
    public partial class Add_More_Company : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public Add_More_Company(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            getData();
            Type = EditType.add;
        }
        public Add_More_Company(MainWindow main, Item_Data_Com_Child_Manage item_Data_Com_Child_Manage)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            infoCom = item_Data_Com_Child_Manage;
            Type = EditType.edit;
            getData(infoCom);
        }

        private Item_Data_Com_Child_Manage infoCom;
        public enum EditType { add, edit }
        private EditType _Type;
        public Action Success
        {
            get { return (Action)GetValue(SuccessProperty); }
            set { SetValue(SuccessProperty, value); }
        }
        public static readonly DependencyProperty SuccessProperty =
            DependencyProperty.Register("Success", typeof(Action), typeof(Add_More_Company));
        public EditType Type
        {
            get { return _Type; }
            set { _Type = value; }
        }
        Regex regex = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", RegexOptions.CultureInvariant | RegexOptions.Singleline);

        private string linkFile = "";
        public MainWindow Main;
        private List<Item_Data_Com_Child_Manage> _listCom;
        public List<Item_Data_Com_Child_Manage> listCom
        {
            get { return _listCom; }
            set { _listCom = value; OnPropertyChanged(); }
        }
        public async Task<List<Item_Data_Com_Child_Manage>> getChildCom(bool full = false)
        {
            try
            {
                string apilink = "http://210.245.108.202:3000/api/qlc/company/child/list";
                HttpClient httpClient = new HttpClient();
                Dictionary<string, string> form = new Dictionary<string, string>();
                switch (Main.Type)
                {
                    case 1:
                        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Main.APIStaff.data.data.access_token);
                        form.Add("com_id", Main.APIStaff.data.data.user_info.com_id);
                        break;
                    case 2:
                        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Main.APICompany.data.data.access_token);
                        form.Add("com_id", Main.APICompany.data.data.user_info.com_id);
                        break;
                    default:
                        break;
                }
                int i = 0;

                var respon = await httpClient.PostAsync(apilink, new FormUrlEncodedContent(form));
                List<Item_Data_Com_Child_Manage> list = new List<Item_Data_Com_Child_Manage>();
                API_Com_Child_Manage api = JsonConvert.DeserializeObject<API_Com_Child_Manage>(respon.Content.ReadAsStringAsync().Result);
                if (api.data != null)
                {
                    api.data.items.Add(new Item_Data_Com_Child_Manage() { com_id = Main.APICompany.data.data.user_info.com_id, com_name = Main.APICompany.data.data.user_info.com_name, com_logo = Main.APICompany.data.data.user_info.com_logo, com_address = Main.APICompany.data.data.user_info.com_address, com_email = Main.APICompany.data.data.user_info.com_email, com_parent_id = "", com_phone = Main.APICompany.data.data.user_info.com_phone, com_role_id = Main.APICompany.data.data.user_info.com_role_id });
                    list = api.data.items;
                }
                return list;

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                return null;
            }
        }
        private async Task getData()
        {
            
            getChildCom().ContinueWith(tt => this.Dispatcher.Invoke(() =>
            {
                listCom = tt.Result;
                listCom.Insert(0, new Item_Data_Com_Child_Manage() { com_id = "-1", com_name = "Tất cả công ty" });
                listCom = listCom.ToList();
                ImgLogo.Source = new Item_Data_Com_Child_Manage().image;
            }));
            
        }
        private async Task getData(Item_Data_Com_Child_Manage childCompany)
        {
            List<Task> k = new List<Task>()
            {
                //getDep(comid, true).ContinueWith(tt => this.Dispatcher.Invoke(() => listDep = tt.Result)),
               
                getChildCom().ContinueWith(tt => this.Dispatcher.Invoke(() => {
                    listCom = tt.Result;
                    listCom.Insert(0, new Item_Data_Com_Child_Manage() { com_id = "-1", com_name = "Tất cả công ty" });
                    listCom = listCom.ToList();
                    cbbCom.SelectedIndex = listCom.Count - 1;
                })),
            };
            k.ForEach(t =>
            {
                t.ContinueWith(tt =>
                {
                    var ck = new List<bool>();
                    k.ForEach(h => ck.Add(h.IsCompleted));
                    if (!ck.Contains(false))
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            infoCom = childCompany;
                            Type = EditType.edit;
                            tbNameCom.Text = infoCom.com_name;
                            tbEmailCom.Text = infoCom.com_email;
                            tbAddress.Text = infoCom.com_address;
                            tbPhone.Text = infoCom.com_phone;
                            ImgLogo.Source = infoCom.image;
                        });
                    }
                });
            });

        }

        private void ExitPopUp_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Main.ClosePopup();
        }

        private async void Add_Com_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            bool allow = true;
            tblNameComValidate.Text = "";
            tblEmailComValidate.Text = "";
            tblComValidate.Text = "";
            tblAddressValidate.Text = "";
            tblPhoneValidate.Text = "";
            if (string.IsNullOrEmpty(tbNameCom.Text) && tbNameCom.Text.Length < 6)
            {
                allow = false;
                tblNameComValidate.Text = "Tên công ty tối thiểu 6 ký tự";
            }
            if (string.IsNullOrEmpty(tbEmailCom.Text))
            {
                allow = false;
                tblEmailComValidate.Text = "Vui lòng nhập địa chỉ email";
            }else if (!regex.IsMatch(tbEmailCom.Text))
            {
                allow = false;
                tblEmailComValidate.Text = "Email không đúng định dạng";
            }
            if (string.IsNullOrEmpty(tbPhone.Text))
            {
                allow = false;
                tblPhoneValidate.Text = "Số điện thoại không hợp lệ";
            }
            if (string.IsNullOrEmpty(tbAddress.Text) && tbAddress.Text.Length < 6)
            {
                allow = false;
                tblAddressValidate.Text = "Địa chỉ tối thiểu 6 ký tự";
            }
            if(cbbCom.SelectedItem== null)
            {
                allow = false;
                tblComValidate.Text = "Vui lòng chọn công ty mẹ";
            }
            if (allow)
            {
                try
                {
                    switch (Type)
                    {
                        case EditType.add:
                            HttpClient http = new HttpClient();
                            switch (Main.Type)
                            {
                                case 1:
                                    http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Main.APIStaff.data.data.access_token);
                                    break;
                                case 2:
                                    http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Main.APICompany.data.data.access_token);
                                    break;
                                default:
                                    break;
                            }
                            var content = new MultipartFormDataContent();
                            
                            content.Add(new StringContent(tbEmailCom.Text), "com_email");
                            content.Add(new StringContent(tbNameCom.Text), "com_name");
                            content.Add(new StringContent(tbPhone.Text), "com_phone");
                            content.Add(new StringContent(tbAddress.Text), "com_address");
                            if(linkFile != "")
                            {
                                FileInfo file = new FileInfo(linkFile);
                                content.Add(new StreamContent(new FileStream(file.FullName, FileMode.Open, FileAccess.Read)), "avatarUser", file.Name);
                            }
                            
                            HttpResponseMessage respon = await http.PostAsync("http://210.245.108.202:3000/api/qlc/company/child/create", content);
                            API_Respon api = JsonConvert.DeserializeObject<API_Respon>(respon.Content.ReadAsStringAsync().Result);
                            if (api.data != null && api.data.result == true)
                            {
                                var pop = new Views.PopUp.Notify1();
                                pop.Type = Notify1.NotifyType.Success;
                                pop.Message = "Thêm Công ty con thành công";
                                Main.ShowPopup(pop);
                            }
                            else
                            {
                                var pop = new Views.PopUp.Notify1();
                                pop.Type = Notify1.NotifyType.Error;
                                pop.Message = "Thêm Công ty thất bại";
                                Main.ShowPopup(pop);
                            }
                            break;
                        case EditType.edit:
                            HttpClient http1 = new HttpClient();
                            switch (Main.Type)
                            {
                                case 2:
                                    http1.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Main.APICompany.data.data.access_token);
                                    break;
                                default:
                                    break;
                            }
                            var content1 = new MultipartFormDataContent();
                            try
                            {
                                FileInfo file1 = new FileInfo(linkFile);
                                content1.Add(new StreamContent(new FileStream(file1.FullName, FileMode.Open, FileAccess.Read)), "avatarUser", file1.Name);
                            }
                            catch
                            {

                            }
                            content1.Add(new StringContent(tbNameCom.Text), "userName");
                            content1.Add(new StringContent(tbPhone.Text), "phone");
                            content1.Add(new StringContent(tbAddress.Text), "address");
                            content1.Add(new StringContent(infoCom.com_id), "com_id");
                            HttpResponseMessage respon1 = await http1.PostAsync("http://210.245.108.202:3000/api/qlc/company/child/edit", content1);
                            API_Respon api1 = JsonConvert.DeserializeObject<API_Respon>(respon1.Content.ReadAsStringAsync().Result);
                            if (api1.data != null && api1.data.result == true)
                            {
                                var pop = new Views.PopUp.Notify1();
                                pop.Type = Notify1.NotifyType.Success;
                                pop.Message = "Cập nhật thông tin Công ty con thành công";
                                Main.ShowPopup(pop);
                            }
                            else
                            {
                                var pop = new Views.PopUp.Notify1();
                                pop.Type = Notify1.NotifyType.Error;
                                pop.Message = "Cập nhật thông tin Công ty thất bại";
                                Main.ShowPopup(pop);
                            }
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void ChooseImgLogo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog op = new Microsoft.Win32.OpenFileDialog();
            if (op.ShowDialog() == true)
            {
                ImgLogo.Source = op.FileName.GetThumbnail(300);
                linkFile = op.FileName;
            }
        }

        private void tbPhone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
