using QuanLyChung365TruocDangNhap.ChamCong365.Entities;
using QuanLyChung365TruocDangNhap.ChamCong365.Entities.Company;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.CompilerServices;
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

namespace QuanLyChung365TruocDangNhap.ChamCong365.Views.Pages
{
    /// <summary>
    /// Interaction logic for Setting_Account.xaml
    /// </summary>
    public partial class Setting_Account : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public Setting_Account(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;

            infoCom.Visibility = infoEp.Visibility = Visibility.Collapsed;
            stackCom.Visibility = stackEp.Visibility = Visibility.Collapsed;

            switch (main.Type)
            {
                case 1:
                    infoEp.Visibility = stackEp.Visibility = Visibility.Visible;
                    var ep = main.APIStaff.data.data.user_info;
                    imgEp.Source = ep.image;
                    tblEpName.Text = ep.ep_name;
                    tblEpID.Text = "ID: " + ep.ep_id;

                    tblID1.Text = ep.ep_id;
                    tblComName1.Text = ep.com_name;
                    txtNameStaff.Text = ep.ep_name;
                    tblEmail.Text = ep.ep_email;
                    txtSDT.Text = ep.ep_phone;
                    txtDiaChi.Text = ep.ep_address;

                    cboGioiTinh.SelectedIndex = int.Parse(ep.ep_gender) - 1;

                    DateTime bir;
                    if (!string.IsNullOrEmpty(ep.ep_birth_day) && DateTime.TryParse(ep.ep_birth_day, out bir)) dpNgaySinh.SelectedDate = bir;

                    cboHocVan.SelectedIndex = int.Parse(ep.ep_education) - 1;
                    if (int.Parse(ep.ep_married) == 1) cboHonNhan.SelectedIndex = 1;
                    else if (int.Parse(ep.ep_married) == 2) cboHonNhan.SelectedIndex = 0;

                    DateTime w;
                    if (!string.IsNullOrEmpty(ep.start_working_time) && DateTime.TryParse(ep.start_working_time, out w)) tblStartDate.Text = w.ToString("dd/MM/yyyy");

                    getDep().ContinueWith(tt => this.Dispatcher.Invoke(() =>
                    {
                        if (tt.Result!=null)
                        {
                            listDep = tt.Result.data.items;
                            if(!string.IsNullOrEmpty(ep.dep_id))
                            cboPhongBan.SelectedIndex = listDep.FindIndex(o=>o.dep_id==ep.dep_id);
                        }
                    }));

                    cboKinhNghiem.SelectedIndex = int.Parse(ep.ep_exp)-1;

                    if(!string.IsNullOrEmpty(ep.position_id))
                    tblChucVu.Text = listChucVu.Find(x=>listChucVu.IndexOf(x)==int.Parse(ep.position_id));
                    break;
                case 2:
                    infoCom.Visibility = stackCom.Visibility = Visibility.Visible;
                    imgCom.Source = main.APICompany.data.data.user_info.logo;
                    tbNameCom.Text = Main.APICompany.data.data.user_info.com_name;
                    tbAddressCom.Text = Main.APICompany.data.data.user_info.com_address;
                    tbPhoneCom.Text = Main.APICompany.data.data.user_info.com_phone;
                    tblComEmail.Text = Main.APICompany.data.data.user_info.com_email;
                    tblComName.Text = Main.APICompany.data.data.user_info.com_name;
                    tblComPhone.Text = Main.APICompany.data.data.user_info.com_phone;
                    tblID.Text = "ID: " + main.APICompany.data.data.user_info.com_id;
                    break;
                default:
                    break;
            }
        }
        //
        public MainWindow Main;

        public List<string> listGioiTinh { get; set; } = new List<string> { "Nam", "Nữ", "Khác" };
        public List<string> listHocVan { get; set; } = new List<string> { "Trên đại học", "Đại học", "Cao Đẳng", "Đào tạo nghề", "Trung học phổ thông", "Trung học cơ sở", "Tiểu học" };
        public List<string> listHonNhan { get; set; } = new List<string> { "Đã có gia đình", "Độc thân" };
        public List<string> listKinhNghiem { get; set; } = new List<string> { "Chưa có kinh nghiệm", "Dưới 1 năm kinh nghiệm", "1 năm", "2 năm", "3 năm", "4 năm", "5 năm", "Trên 5 năm" };
        public List<string> listChucVu { get; set; } = new List<string> { "", "SINH VIÊN THỰC TẬP", "NHÂN VIÊN THỬ VIỆC", "NHÂN VIÊN CHÍNH THỨC", "TRƯỞNG NHÓM", "PHÓ TRƯỞNG PHÒNG", "TRƯỞNG PHÒNG", "PHÓ GIÁM ĐỐC", "GIÁM ĐỐC", "NHÂN VIÊN PART TIME", "PHÓ BAN DỰ ÁN", "TRƯỞNG BAN DỰ ÁN", "PHÓ TỔ TRƯỞNG", "TỔ TRƯỞNG", "PHÓ TỔNG GIÁM ĐỐC", "", "TỔNG GIÁM ĐỐC", "THÀNH VIÊN HỘI ĐỒNG QUẢN TRỊ", "", "NHÓM PHÓ", "TỔNG GIÁM ĐỐC TẬP ĐOÀN", "PHÓ TỔNG GIÁM ĐỐC TẬP ĐOÀN" };

        private int _IsSmallSizeInfoEP = 0;
        public int IsSmallSizeInfoEP
        {
            get { return _IsSmallSizeInfoEP; }
            set { _IsSmallSizeInfoEP = value; OnPropertyChanged(); }
        }

        private List<Item_List_PhongBan> _listDep;
        public List<Item_List_PhongBan> listDep
        {
            get { return _listDep; }
            set { _listDep = value; OnPropertyChanged(); }
        }
        //
        private async Task<API_List_PhongBan> getDep(int page = 1)
        {
            try
            {
                string apilink = "http://210.245.108.202:3000/api/qlc/department/list";
                HttpClient httpClient = new HttpClient();
                switch (Main.Type)
                {
                    case 1:
                        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Main.APIStaff.data.data.access_token);
                        break;
                    case 2:
                        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Main.APICompany.data.data.access_token);
                        break;
                    default:
                        break;
                }
                List<Item_List_PhongBan> list = new List<Item_List_PhongBan>();
                Dictionary<string, string> form = new Dictionary<string, string>();
                form.Add("com_id", Main.APIStaff.data.data.user_info.com_id);
                var respon = await httpClient.PostAsync(apilink, new FormUrlEncodedContent(form));
                API_List_PhongBan api = JsonConvert.DeserializeObject<API_List_PhongBan>(respon.Content.ReadAsStringAsync().Result);
                if (api.data != null) return api;
                return null;
            }
            catch (Exception ex)
            {

                var x = new PopUp.Notify1();
                x.Type = PopUp.Notify1.NotifyType.Success;
                x.Message = ex.Message;
                Main.ShowPopup(x);
                return null;
            }
        }
        // nhap password cu
        private string _Password = "";
        public string Password
        {
            get { return _Password; }
            set
            {
                _Password = value;
                OnPropertyChanged();
            }
        }
        private bool _ShowPass = false;
        public bool ShowPass
        {
            get { return _ShowPass; }
            set { _ShowPass = value; OnPropertyChanged(); }
        }
        private void ShowPassword(object sender, MouseButtonEventArgs e)
        {
            ShowPass = !ShowPass;
            if (ShowPass)
            {
                txtPassWordCu.Focus();
                txtPassWordCu.SelectionStart = txtPassWordCu.Text.Length;
            }
            else
            {
                ShowPass = false;
                passboxPassWordCu.Password = Password;
                passboxPassWordCu.GetType().GetMethod("Select", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(passboxPassWordCu, new object[] { Password.Length, 0 });
                passboxPassWordCu.Focus();
            }
        }
        private void InputPasswordCu(object sender, RoutedEventArgs e)
        {
            Password = passboxPassWordCu.Password;
        }

        //nhập password moi
        private string _NewPassword = "";
        public string NewPassword
        {
            get { return _NewPassword; }
            set
            {
                _NewPassword = value;
                OnPropertyChanged();
            }
        }
        private bool _ShowNewPass = false;
        public bool ShowNewPass
        {
            get { return _ShowNewPass; }
            set
            {
                _ShowNewPass = value;
                OnPropertyChanged();
            }
        }
        private void InputNewPassword(object sender, RoutedEventArgs e)
        {
            NewPassword = passboxNewPassWord.Password;
        }
        private void ShowNewPassword(object sender, MouseButtonEventArgs e)
        {
            ShowNewPass = !ShowNewPass;
            if (ShowNewPass)
            {
                txtNewPassWord.Focus();
                txtNewPassWord.SelectionStart = txtNewPassWord.Text.Length;
            }
            else
            {
                ShowNewPass = false;
                passboxNewPassWord.Password = NewPassword;
                passboxNewPassWord.GetType().GetMethod("Select", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(passboxNewPassWord, new object[] { NewPassword.Length, 0 });
                passboxNewPassWord.Focus();
            }
        }

        // nhap lai password moi
        private string _RePassword = "";
        public string RePassword
        {
            get { return _RePassword; }
            set
            {
                _RePassword = value; OnPropertyChanged
                   ();
            }
        }
        private bool _ShowRePass = false;
        public bool ShowRePass
        {
            get { return _ShowRePass; }
            set { _ShowRePass = value; OnPropertyChanged(); }
        }
        private void InputRePassword(object sender, RoutedEventArgs e)
        {
            RePassword = passboxRePassWord.Password;
        }
        private void ShowRePassword(object sender, MouseButtonEventArgs e)
        {
            ShowRePass = !ShowRePass;
            if (ShowRePass)
            {
                txtRePassWord.Focus();
                txtRePassWord.SelectionStart = txtRePassWord.Text.Length;
            }
            else
            {
                ShowRePass = false;
                passboxRePassWord.Password = RePassword;
                passboxRePassWord.GetType().GetMethod("Select", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(passboxRePassWord, new object[] { RePassword.Length, 0 });
                passboxRePassWord.Focus();
            }
        }
        //sizeChanged
        private int _IsSmallSize;
        public int IsSmallSize
        {
            get { return _IsSmallSize; }
            set
            {
                _IsSmallSize = value;
                OnPropertyChanged("IsSmallSize");
            }
        }
        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.ActualWidth > 960)
            {
                IsSmallSize = 0;
            }
            else if (this.ActualWidth <= 960 && this.ActualWidth > 656)
            {
                IsSmallSize = 1;
            }
            else
            {
                IsSmallSize = 2;
            }
        }
        private async void Changed_Info_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                HttpClient httpEdit = new HttpClient();
                Dictionary<string, string> formEdit = new Dictionary<string, string>();
                HttpResponseMessage responEdit;
                switch (Main.Type)
                {
                    case 1:
                        httpEdit.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Main.APIStaff.data.data.access_token);
                        break;
                    case 2:
                        httpEdit.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Main.APICompany.data.data.access_token);
                        formEdit.Add("userName", tbNameCom.Text);
                        formEdit.Add("phoneTK", tbPhoneCom.Text);
                        formEdit.Add("address", tbAddressCom.Text);
                        //formEdit.Add("id_com", Main.APICompany.data.data.user_info.com_id);
                        responEdit = await httpEdit.PostAsync("http://210.245.108.202:3000/api/qlc/Company/updateInfoCompany", new FormUrlEncodedContent(formEdit));
                        API_Respon apiEdit = JsonConvert.DeserializeObject<API_Respon>(responEdit.Content.ReadAsStringAsync().Result);
                        if (apiEdit.data != null && apiEdit.data.result == true)
                        {
                            tblComName.Text = tbNameCom.Text;
                            tblComPhone.Text = tbPhoneCom.Text;
                            Main.RefreshComName(tbNameCom.Text);

                            var x = new PopUp.Notify1();
                            x.Type = PopUp.Notify1.NotifyType.Success;
                            x.Message = "Cập nhật thành công";
                            Main.ShowPopup(x);
                        }
                        else
                        {
                            var x = new PopUp.Notify1();
                            x.Type = PopUp.Notify1.NotifyType.Success;
                            x.Message = apiEdit.error.message;
                            Main.ShowPopup(x);
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {

                var x = new PopUp.Notify1();
                x.Type = PopUp.Notify1.NotifyType.Success;
                x.Message = ex.Message;
                Main.ShowPopup(x);
            }

        }
        private void infoEp_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (infoEp.ActualWidth >= 500) IsSmallSizeInfoEP = 0;
            else IsSmallSizeInfoEP = 1;
        }
        private async void Changed_Pass_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            bool allow = true;
            tblPassCuValidate.Text = "";
            tblPassMoiValidate.Text = "";
            tblRePassValidate.Text = "";
            if (string.IsNullOrEmpty(Password))
            {
                allow = false;
                tblPassCuValidate.Text = "Không được để trống";
            }
            if (string.IsNullOrEmpty(NewPassword))
            {
                allow = false;
                tblPassMoiValidate.Text = "Không được để trống";
            }
            if (string.IsNullOrEmpty(RePassword))
            {
                allow = false;
                tblRePassValidate.Text = "Không được để trống";
            }
            if (NewPassword != RePassword)
            {
                allow = false;
                tblRePassValidate.Text = "Nhập lại mật khẩu chưa chính xác";
            }
            if (allow)
            {
                try
                {
                    HttpClient httpEdit = new HttpClient();
                    Dictionary<string, string> formEdit = new Dictionary<string, string>();
                    HttpResponseMessage responEdit;

                    switch (Main.Type)
                    {
                        case 1:
                            httpEdit.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Main.APIStaff.data.data.access_token);
                            formEdit.Add("password", NewPassword);
                            formEdit.Add("re_password", NewPassword);
                            formEdit.Add("old_password", Password);
                            responEdit = await httpEdit.PostAsync("http://210.245.108.202:3000/api/qlc/employee/updatePassword", new FormUrlEncodedContent(formEdit));
                            API_Respon apiEditnv = JsonConvert.DeserializeObject<API_Respon>(responEdit.Content.ReadAsStringAsync().Result);
                            if (apiEditnv.data != null && apiEditnv.data.result == true)
                            {
                                var pop = new PopUp.Notify1();
                                pop.Type = PopUp.Notify1.NotifyType.Success;
                                pop.Message = "Cập nhật thành công";
                                Main.ShowPopup(pop);
                            }
                            else
                            {
                                var pop = new PopUp.Notify1();
                                pop.Type = PopUp.Notify1.NotifyType.Error;
                                pop.Message = apiEditnv.error.message;
                                Main.ShowPopup(pop);
                            }
                            break;
                        case 2:
                            httpEdit.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Main.APICompany.data.data.access_token);
                            formEdit.Add("password", NewPassword);
                            formEdit.Add("re_password", NewPassword);
                            formEdit.Add("old_password", Password);
                            responEdit = await httpEdit.PostAsync("http://210.245.108.202:3000/api/qlc/Company/updateNewPassword", new FormUrlEncodedContent(formEdit));
                            API_Respon apiEdit = JsonConvert.DeserializeObject<API_Respon>(responEdit.Content.ReadAsStringAsync().Result);
                            if (apiEdit.data != null && apiEdit.data.result == true)
                            {
                                var pop = new PopUp.Notify1();
                                pop.Type = PopUp.Notify1.NotifyType.Success;
                                pop.Message = "Cập nhật thành công";
                                Main.ShowPopup(pop);
                            }
                            else
                            {
                                var pop = new PopUp.Notify1();
                                pop.Type = PopUp.Notify1.NotifyType.Error;
                                pop.Message = apiEdit.error.message;
                                Main.ShowPopup(pop);
                            }
                            break;
                        default:
                            break;
                    }

                }
                catch (Exception ex)
                {
                    var pop = new PopUp.Notify1();
                    pop.Type = PopUp.Notify1.NotifyType.Error;
                    pop.Message = ex.Message;
                    Main.ShowPopup(pop);
                }
            }
        }
        private async void Changed_InfoEp_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                HttpClient httpEdit = new HttpClient();
                Dictionary<string, string> formEdit = new Dictionary<string, string>();
                HttpResponseMessage responEdit;
                httpEdit.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Main.APIStaff.data.data.access_token);
                formEdit.Add("userName", txtNameStaff.Text);
                formEdit.Add("phoneTK", txtSDT.Text);
                formEdit.Add("address", txtDiaChi.Text);
                formEdit.Add("dep_id", (cboPhongBan.SelectedItem as Item_List_PhongBan).dep_id);
                formEdit.Add("com_id", Main.APIStaff.data.data.user_info.com_id);
                formEdit.Add("idQLC", Main.APIStaff.data.data.user_info.ep_id);
                formEdit.Add("position_id", listChucVu.FindIndex(x=>x==tblChucVu.Text).ToString());
                formEdit.Add("married", cboHonNhan.SelectedIndex==1?"1":"2");
                formEdit.Add("experience", listKinhNghiem.IndexOf(cboKinhNghiem.SelectedItem as string).ToString());
                formEdit.Add("birthday", dpNgaySinh.SelectedDate.Value.ToString("yyyy-MM-dd") );
                formEdit.Add("gender", (cboGioiTinh.SelectedIndex + 1).ToString() );
                formEdit.Add("education", (cboHocVan.SelectedIndex + 1).ToString() );
                responEdit = await httpEdit.PostAsync("http://210.245.108.202:3000/api/qlc/employee/updateInfoEmployee", new FormUrlEncodedContent(formEdit));
                API_Respon apiEdit = JsonConvert.DeserializeObject<API_Respon>(responEdit.Content.ReadAsStringAsync().Result);
                if (apiEdit.data != null && apiEdit.data.result == true)
                {
                    tblComName.Text = tbNameCom.Text;
                    tblComPhone.Text = tbPhoneCom.Text;
                    Main.RefreshEpName(txtNameStaff.Text);

                    var x = new PopUp.Notify1();
                    x.Type = PopUp.Notify1.NotifyType.Success;
                    x.Message = "Cập nhật thành công";
                    Main.ShowPopup(x);
                }
                else
                {
                    var x = new PopUp.Notify1();
                    x.Type = PopUp.Notify1.NotifyType.Success;
                    x.Message = apiEdit.error.message;
                    Main.ShowPopup(x);
                }
            }
            catch (Exception ex)
            {

                var x = new PopUp.Notify1();
                x.Type = PopUp.Notify1.NotifyType.Success;
                x.Message = ex.Message;
                Main.ShowPopup(x);
            }
        }
    }
}
