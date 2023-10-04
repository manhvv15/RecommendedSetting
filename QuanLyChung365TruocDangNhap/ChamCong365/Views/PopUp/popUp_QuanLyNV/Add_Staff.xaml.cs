using QuanLyChung365TruocDangNhap.ChamCong365.Entities;
using QuanLyChung365TruocDangNhap.ChamCong365.Entities.Company;
using QuanLyChung365TruocDangNhap.ChamCong365.Entities.History_Attenden_Staff;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace QuanLyChung365TruocDangNhap.ChamCong365.Views.PopUp.popUp_QuanLyNV
{
    /// <summary>
    /// Interaction logic for Add_Staff.xaml
    /// </summary>
    public partial class Add_Staff : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public Add_Staff(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            getData();

        }
        public Add_Staff(MainWindow main, Item_List_Staff_All allStaff)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            getData(allStaff);
            info = allStaff;
        }
        public enum EditTypes { add, edit }
        private EditTypes _Type;
        public EditTypes Type
        {
            get { return _Type; }
            set { _Type = value; OnPropertyChanged(); }
        }
        public List<string> listGioiTinh { get; set; } = new List<string> { "Nam", "Nữ", "Khác" };
        public List<string> listHocVan { get; set; } = new List<string> { "Chọn trình độ học vấn", "Trên đại học", "Đại học", "Cao Đẳng", "Trung cấp", "Đào tạo nghề", "Trung học phổ thông", "Trung học cơ sở", "Tiểu học" };
        public List<string> listHonNhan { get; set; } = new List<string> { "Đã có gia đình", "Độc thân" };
        public List<string> listKinhNghiem { get; set; } = new List<string> { "Chưa có kinh nghiệm", "Dưới 1 năm kinh nghiệm", "1 năm", "2 năm", "3 năm", "4 năm", "5 năm", "Trên 5 năm" };
        public List<string> listChucVu { get; set; } = new List<string> { "SINH VIÊN THỰC TẬP", "NHÂN VIÊN PART TIME", "NHÂN VIÊN THỬ VIỆC", "NHÂN VIÊN CHÍNH THỨC", "TRƯỞNG NHÓM", "NHÓM PHÓ", "TỔ TRƯỞNG", "PHÓ TỔ TRƯỞNG", "TRƯỞNG BAN DỰ ÁN", "PHÓ BAN DỰ ÁN", "TRƯỞNG PHÒNG", "PHÓ TRƯỞNG PHÒNG", "GIÁM ĐỐC", "PHÓ GIÁM ĐỐC", "TỔNG GIÁM ĐỐC", "PHÓ TỔNG GIÁM ĐỐC", "TỔNG GIÁM ĐỐC TẬP ĐOÀN", "PHÓ TỔNG GIÁM ĐỐC TẬP ĐOÀN", "CHỦ TỊCH HỘI ĐỒNG QUẢN TRỊ", "PHÓ CHỦ TỊCH HỘI ĐỒNG QUẢN TRỊ", "THÀNH VIÊN HỘI ĐỒNG QUẢN TRỊ" };
        private Item_List_Staff_All info;
        private List<Item_List_Staff_All> _List_Staff_All;
        public List<Item_List_Staff_All> List_Staff_All
        {
            get { return _List_Staff_All; }
            set
            {
                _List_Staff_All = value;
                OnPropertyChanged();
            }
        }
        private List<ChildCompany> _listCom;
        public List<ChildCompany> listCom
        {
            get { return _listCom; }
            set
            {
                _listCom = value;
                if (value != null)
                    OnPropertyChanged();
            }
        }
        private List<QuyenTruyCap> _listQuyen;
        public List<QuyenTruyCap> listQuyen
        {
            get { return _listQuyen; }
            set { _listQuyen = value; OnPropertyChanged(); }
        }
        private List<Item_List_PhongBan> _listDep;
        public List<Item_List_PhongBan> listDep
        {
            get { return _listDep; }
            set { _listDep = value; OnPropertyChanged(); }
        }
        public Action Success
        {
            get { return (Action)GetValue(SuccessProperty); }
            set { SetValue(SuccessProperty, value); }
        }
        public static readonly DependencyProperty SuccessProperty =
            DependencyProperty.Register("Success", typeof(Action), typeof(Add_Staff));
        private MainWindow Main;
        Regex regex = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", RegexOptions.CultureInvariant | RegexOptions.Singleline);
        Regex regexSDT = new Regex(@"^((09|03|07|08|05|04)+([0-9]{8})\b)$", RegexOptions.CultureInvariant | RegexOptions.Singleline);
        private async Task<List<ChildCompany>> getCom()
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
                var respon = await httpClient.PostAsync(apilink,new FormUrlEncodedContent(form));
                API_Company_List api = JsonConvert.DeserializeObject<API_Company_List>(respon.Content.ReadAsStringAsync().Result);
                if (api.data != null && api.data.items != null)
                {
                    api.data.items.Add(new ChildCompany() { com_id = Main.APICompany.data.data.user_info.com_id, com_name = Main.APICompany.data.data.user_info.com_name, com_logo = Main.APICompany.data.data.user_info.com_logo, com_address = Main.APICompany.data.data.user_info.com_address, com_email = Main.APICompany.data.data.user_info.com_email, com_parent_id = "", com_phone = Main.APICompany.data.data.user_info.com_phone, com_role_id = Main.APICompany.data.data.user_info.com_role_id });
                    return api.data.items;
                }
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
        private async Task<List<QuyenTruyCap>> getQuyen()
        {
            try
            {
                List<QuyenTruyCap> quyen = new List<QuyenTruyCap>();
                quyen.Add(new QuyenTruyCap() { role_id = "2", role_name = "Top manager"});
                quyen.Add(new QuyenTruyCap() { role_id = "1", role_name = "Admin"});
                quyen.Add(new QuyenTruyCap() { role_id = "3", role_name = "Nhân viên"});
                quyen.Add(new QuyenTruyCap() { role_id = "4", role_name = "Nhân sự quản lý phần mềm"});
                /*HttpClient http = new HttpClient();
                switch (Main.Type)
                {
                    case 1:
                        http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Main.APIStaff.data.data.access_token);
                        break;
                    case 2:
                        http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Main.APICompany.data.data.access_token);
                        break;
                    default:
                        break;
                }
                var apilink = "https://chamcong.24hpay.vn/service/get_update_permission.php?id_ep_update=20";
                HttpResponseMessage respon = await http.GetAsync(apilink);
                API_QuyenTruyCap_List api = JsonConvert.DeserializeObject<API_QuyenTruyCap_List>(respon.Content.ReadAsStringAsync().Result);
                if (api.data != null && api.data.items != null)
                {
                    return api.data.items;
                }*/
                return quyen;
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
        public async Task<List<Item_List_PhongBan>> getDep(string idcom, bool full = false)
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
                form.Add("com_id", idcom);
                var respon = await httpClient.PostAsync(apilink, new FormUrlEncodedContent(form));
                API_List_PhongBan ts = JsonConvert.DeserializeObject<API_List_PhongBan>(respon.Content.ReadAsStringAsync().Result);
                if (ts != null)
                {
                    if (ts.data != null && ts.data.items != null)
                    {
                        ts.data.items.RemoveAll(item => item == null);
                        list = ts.data.items;
                    }
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

            List<Task> k = new List<Task>()
            {
                //getDep(comid, true).ContinueWith(tt => this.Dispatcher.Invoke(() => listDep = tt.Result)),
                getQuyen().ContinueWith(tt => this.Dispatcher.Invoke(() => listQuyen = tt.Result)),
                getCom().ContinueWith(tt => this.Dispatcher.Invoke(() => {
                    listCom = tt.Result;
                    listCom.Insert(0, new ChildCompany(){com_id = "-1", com_name = "Chọn công ty"});
                    listCom = listCom.ToList();

                    if (tt.Result.Count>0)
                    {
                        cbbCom.SelectedIndex=0;
                        //cbbCom.SelectedIndex=tt.Result.Count-1;
                    }
                    var z = cbbCom.SelectedItem as ChildCompany;
                    if (z != null)
                    {
                        getDep(z.com_id).ContinueWith(zz => this.Dispatcher.Invoke(() =>
                        {
                            listDep = zz.Result;
                            listDep.Insert(0, new Item_List_PhongBan() { dep_id = "-1", dep_name = "Phòng ban" });
                            listDep = listDep.ToList();
                            //cbbPB.SelectedIndex=0;
                        }));
                    }
                })),


            };
        }
        private async Task getData(Item_List_Staff_All allStaff)
        {

            List<Task> k = new List<Task>()
            {
                //getDep(comid, true).ContinueWith(tt => this.Dispatcher.Invoke(() => listDep = tt.Result)),
                getQuyen().ContinueWith(tt => this.Dispatcher.Invoke(() => listQuyen = tt.Result)),
                getCom().ContinueWith(tt => this.Dispatcher.Invoke(() => {
                    listCom = tt.Result;
                    listCom.Insert(0, new ChildCompany(){com_id = "-1", com_name = "Chọn công ty"});
                    listCom = listCom.ToList();
                    if (!string.IsNullOrEmpty(allStaff.com_id))
                    {
                        cbbCom.SelectedIndex = listCom.FindIndex(x => x.com_id == allStaff.com_id);

                        getDep(allStaff.com_id).ContinueWith(zz => this.Dispatcher.Invoke(() =>
                        {
                            listDep = zz.Result;
                            listDep.Insert(0, new Item_List_PhongBan() { dep_id = "-1", dep_name = "Phòng ban" });
                            listDep = listDep.ToList();

                            int index = listDep.FindIndex(x => x.dep_id == allStaff.dep_id);
                            if (!string.IsNullOrEmpty(allStaff.dep_id)) cbbPB.SelectedIndex = index;
                        }));
                    }
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
                            info = allStaff;
                            Type = EditTypes.edit;
                            tbName.Text = info.ep_name;
                            tbEmail.Text = info.ep_email;
                            tbAddress.Text = info.ep_address;
                            tbPhone.Text = info.ep_phone;
                            if (!string.IsNullOrEmpty(info.ep_gender)) cbbGender.SelectedIndex = int.Parse(info.ep_gender);
                            if (!string.IsNullOrEmpty(info.ep_education)) cbbEdu.SelectedIndex = int.Parse(info.ep_education);
                            if (!string.IsNullOrEmpty(info.ep_married)) cbbMarried.SelectedIndex = int.Parse(info.ep_married);
                            if (!string.IsNullOrEmpty(info.ep_exp)) cbbExp.SelectedIndex = int.Parse(info.ep_exp);

                            DateTime birth = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                            if (!string.IsNullOrEmpty(info.ep_birth_day)) BirthDate.SelectedDate = birth.AddSeconds(long.Parse(info.ep_birth_day));

                            DateTime startDate = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                            if (!string.IsNullOrEmpty(info.start_working_time) && long.Parse(info.start_working_time) > 0) StarDate.SelectedDate = startDate.AddSeconds(long.Parse(info.start_working_time));
                        });
                    }
                });
            });


        }
        private async void SetQuyenNhanVien(object sender, SelectionChangedEventArgs e)
        {
            var cbo = (sender as ComboBox);
            var t = info;
            var item = cbo.SelectedItem as QuyenTruyCap;

            if (t != null && item != null && t.role_id != item.role_id)
            {
                try
                {
                    Dictionary<string, string> form = new Dictionary<string, string>();
                    form.Add("id_ep_update", t.ep_id);
                    form.Add("role_id", item.role_id);
                    HttpClient httpClient = new HttpClient();
                    switch (Main.Type)
                    {
                        case 1:
                            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Main.APIStaff.data.data.access_token);
                            break;
                        case 2:
                            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Main.APICompany.data.data.access_token);
                            break;
                        default:
                            break;
                    }
                    var respon = await httpClient.PostAsync("https://api.timviec365.vn/api/qlc/managerUser/update_permission", new FormUrlEncodedContent(form));
                    var check = respon.Content.ReadAsStringAsync().Result;
                    API_Respon add = JsonConvert.DeserializeObject<API_Respon>(respon.Content.ReadAsStringAsync().Result);
                    /*if (add.data != null && add.data.result)
                    {
                        var x = new PopUp.Notify1();
                        x.Type = PopUp.Notify1.NotifyType.Success;
                        x.Message = "Cập nhật thông tin thành công";
                        Main.ShowPopup(x);
                    }
                    else
                    {
                        var x = new PopUp.Notify1();
                        x.Type = PopUp.Notify1.NotifyType.Error;
                        x.Message = "Cập nhật thông tin thất bại, vui lòng thử lại sau";
                        Main.ShowPopup(x);
                    }*/
                }
                catch (Exception ex)
                {
                    var x = new PopUp.Notify1();
                    x.Type = PopUp.Notify1.NotifyType.Error;
                    x.Message = ex.Message;
                    Main.ShowPopup(x);
                }
            }
        }
        private void ExitPopUp_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Main.ClosePopup();
        }
        private void txt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private async void Accept_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            bool allow = true;
            tblAddressValidate.Text = "";
            tblBirthDateValidate.Text = "";
            tblEduValidate.Text = "";
            tblEmailValidate.Text = "";
            tblNameValidate.Text = "";
            tblPassValidate.Text = "";
            tblPhoneValidate.Text = "";
            tblRePassValidate.Text = "";
            tblStartDateValidate.Text = "";
            if (string.IsNullOrEmpty(tbName.Text) && tbName.Text.Length < 6)
            {
                allow = false;
                tblNameValidate.Text = "Họ tên tối thiểu 6 ký tự";
            }
            if (string.IsNullOrEmpty(tbPass.Text) && tbPass.Text.Length < 6 && Type==EditTypes.add)
            {
                allow = false;
                tblPassValidate.Text = "Mật khẩu tối thiểu 6 ký tự";
            }
            if (tbPass.Text != tbRePass.Text && Type == EditTypes.add)
            {
                allow = false;
                tblRePassValidate.Text = "Nhập lại mật khẩu chưa chính xác";
            }
            if (string.IsNullOrEmpty(tbEmail.Text))
            {
                allow = false;
                tblEmailValidate.Text = "Vui lòng nhập địa chỉ Email";
            }
            if (!regex.IsMatch(tbEmail.Text))
            {
                allow = false;
                tblEmailValidate.Text = "Vui lòng nhập đúng định dạng Email";
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
            if (cbbEdu.SelectedIndex < 1)
            {
                allow = false;
                tblEduValidate.Text = "Vui lòng chọn trình độ học vấn";
            }
            if (string.IsNullOrEmpty(BirthDate.Text))
            {
                allow = false;
                tblBirthDateValidate.Text = "vui lòng chọn ngày sinh";
            }
            if (string.IsNullOrEmpty(StarDate.Text))
            {
                allow = false;
                tblStartDateValidate.Text = "vui lòng chọn ngày bắt đầu làm việc";
            }
            if (cbbPB.SelectedItem == null)
            {
                allow = false;
                cbbPBValidate.Text = "Vui lòng không để trống phòng ban";
            }
            if (allow)
            {
                try
                {
                    switch (Type)
                    {
                        case EditTypes.add:
                            HttpClient http = new HttpClient();
                            switch (Main.Type)
                            {
                                case 1:
                                    http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Main.APIStaff.data.data.access_token);
                                    break;
                                case 2:
                                    http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Main.APICompany.data.data.access_token);
                                    break;
                                default:
                                    break;
                            }
                            Dictionary<string, string> form = new Dictionary<string, string>();
                            form.Add("email", tbEmail.Text);
                            form.Add("userName", tbName.Text);
                            form.Add("phoneTK", tbPhone.Text);
                            form.Add("password", tbPass.Text);
                            form.Add("address", tbAddress.Text);
                            if(cbbPB.SelectedItem!=null) form.Add("dep_id", (cbbPB.SelectedItem as Item_List_PhongBan).dep_id);
                            form.Add("com_id", (cbbCom.SelectedItem as ChildCompany).com_id);
                            if (cbbQuyen.SelectedItem != null) form.Add("role", (cbbQuyen.SelectedItem as QuyenTruyCap).role_id);
                            form.Add("position_id", (ccbCV.SelectedIndex.ToString()));
                            form.Add("gender", cbbGender.SelectedIndex.ToString());
                            form.Add("birthday", BirthDate.SelectedDate.Value.ToString("yyyy-MM-dd"));
                            form.Add("education", cbbEdu.SelectedIndex.ToString());
                            form.Add("start_working_time", StarDate.SelectedDate.Value.ToString("yyyy-MM-dd"));
                            form.Add("married", cbbMarried.SelectedIndex.ToString());
                            form.Add("experience", cbbExp.SelectedIndex.ToString());
                            HttpResponseMessage respon = await http.PostAsync("http://210.245.108.202:3000/api/qlc/managerUser/create", new FormUrlEncodedContent(form));
                            API_Respon api = JsonConvert.DeserializeObject<API_Respon>(respon.Content.ReadAsStringAsync().Result);
                            if (api.data != null && api.data.result == true)
                            {
                                if (Success != null) Success();
                                Main.ClosePopup();
                            }
                            break;
                        case EditTypes.edit:
                            HttpClient http1 = new HttpClient();
                            switch (Main.Type)
                            {
                                case 1:
                                    http1.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Main.APIStaff.data.data.access_token);
                                    break;
                                case 2:
                                    http1.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Main.APICompany.data.data.access_token);
                                    break;
                                default:
                                    break;
                            }
                            Dictionary<string, string> form1 = new Dictionary<string, string>();
                            form1.Add("userName", tbName.Text);
                            form1.Add("phoneTK", tbPhone.Text);
                            form1.Add("address", tbAddress.Text);
                            form1.Add("dep_id", (cbbPB.SelectedItem as Item_List_PhongBan).dep_id);
                            form1.Add("com_id", (cbbCom.SelectedItem as ChildCompany).com_id);
                            form1.Add("idQLC", info.ep_id);
                            form1.Add("position_id", (ccbCV.SelectedIndex.ToString()));
                            form1.Add("married", cbbMarried.SelectedIndex.ToString());
                            form1.Add("experience", cbbExp.SelectedIndex.ToString());
                            form1.Add("birthday", BirthDate.SelectedDate.Value.ToString("yyyy-MM-dd"));
                            form1.Add("gender", cbbGender.SelectedIndex.ToString());
                            form1.Add("education", cbbEdu.SelectedIndex.ToString());
                            form1.Add("group_id", info.group_id);
                            form1.Add("start_working_time", StarDate.SelectedDate.Value.ToString("yyyy-MM-dd"));
                            //form1.Add("group_id", info.group_id);
                            //form1.Add("role", (cbbQuyen.SelectedItem as QuyenTruyCap).role_id);
                            HttpResponseMessage respon1 = await http1.PostAsync("http://210.245.108.202:3000/api/qlc/employee/updateInfoEmployeeComp", new FormUrlEncodedContent(form1));
                            API_Respon api1 = JsonConvert.DeserializeObject<API_Respon>(respon1.Content.ReadAsStringAsync().Result);
                            if (api1.data != null && api1.data.result == true)
                            {
                                if (Success != null) Success();
                                Main.ClosePopup();
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
        int dem = 0;
        private void cbbCom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var z = cbbCom.SelectedItem as ChildCompany;
            if (z != null && dem > 0)
            {
                getDep(z.com_id).ContinueWith(tt => this.Dispatcher.Invoke(() =>
                {
                    listDep = tt.Result;
                    listDep.Insert(0, new Item_List_PhongBan() { dep_id = "-1", dep_name = "Phòng ban" });
                    listDep = listDep.ToList();
                }));
            }
            dem++;
        }
    }
}
