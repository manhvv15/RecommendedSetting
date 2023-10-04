using QuanLyChung365TruocDangNhap.ChamCong365.Entities.Company;
using QuanLyChung365TruocDangNhap.ChamCong365.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using QuanLyChung365TruocDangNhap.ChamCong365.Entities;

namespace QuanLyChung365TruocDangNhap.ChamCong365.Views.Pages
{
    /// <summary>
    /// Interaction logic for Quan_Ly_Quyen_Truy_Cap.xaml
    /// </summary>
    public partial class Quan_Ly_Quyen_Truy_Cap : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public Quan_Ly_Quyen_Truy_Cap(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            this.table.AutoReponsiveColumn(1, "IDnTen");
            getData().GetAwaiter();
        }
        public MainWindow Main;
        private List<Employee> _listNV;
        public List<Employee> listNV
        {
            get { return _listNV; }
            set
            {
                _listNV = value;
                OnPropertyChanged();
            }
        }

        private List<Employee> _listNVShow;
        public List<Employee> listNVShow
        {
            get { return _listNVShow; }
            set
            {
                _listNVShow = value;
                OnPropertyChanged();
            }
        }

        private List<QuyenTruyCap> _listQuyen;
        public List<QuyenTruyCap> listQuyen
        {
            get { return _listQuyen; }
            set { _listQuyen = value; OnPropertyChanged(); }
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
        //
        private async Task<API_Employee_List> getNhanVien(string com = "", string role = "", string ep = "", int page = 1,int length=20)
        {
            try
            {
                if (page <= 0) page = 1;
                var offset = (page - 1) * 20;
                HttpClient http = new HttpClient();
                var apilink = $"http://210.245.108.202:3000/api/qlc/managerUser/list";

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
               
                /*if (!string.IsNullOrEmpty(com)) apilink += "&id_com=" + com;
                if (!string.IsNullOrEmpty(ep)) apilink += ("&filter_by[ep_id]=" + ep);
                if (!string.IsNullOrEmpty(role)) apilink += ("&filter_by[role]=" + role);*/
                var form = new Dictionary<string, string>();
                form.Add("pageNumber", page.ToString());
                form.Add("pageSize", "20");
                form.Add("com_id", com);
                form.Add("ep_id", ep);
                form.Add("role_id", role);
                HttpResponseMessage respon = await http.PostAsync(apilink, new FormUrlEncodedContent(form));
                var check = respon.Content.ReadAsStringAsync().Result;
                API_Employee_List api = JsonConvert.DeserializeObject<API_Employee_List>(respon.Content.ReadAsStringAsync().Result);
                if (api.data != null && api.data.items != null)
                {
                    //if (pagin.TotalRecords != listNV.Count) pagin.TotalRecords = int.Parse(api.data.totalItems);
                    return api;
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
                quyen.Add(new QuyenTruyCap() { role_id = "-1", role_name = "Tất cả"});
                quyen.Add(new QuyenTruyCap() { role_id = "1", role_name = "Admin"});
                quyen.Add(new QuyenTruyCap() { role_id = "3", role_name = "Nhân viên"});
                quyen.Add(new QuyenTruyCap() { role_id = "4", role_name = "Nhân sự quản lý phần mềm"});
                /*HttpClient http = new HttpClient();
                var apilink = "https://chamcong.24hpay.vn/service/get_update_permission.php?id_ep_update=20&id_com=3312";
                switch (Main.Type)
                {
                    case 1:
                        http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Main.APICompany.data.data.access_token);
                        break;
                    case 2:
                        http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Main.APICompany.data.data.access_token);
                        break;
                    default:
                        break;
                }
                HttpResponseMessage respon = await http.GetAsync(apilink);
                API_QuyenTruyCap_List api = JsonConvert.DeserializeObject<API_QuyenTruyCap_List>(respon.Content.ReadAsStringAsync().Result);
                if (api.data != null && api.data.items != null)
                {
                    for(int i =0; i<api.data.items.Count; i++)
                    {
                        if (api.data.items[i].role_name == "Top manager")
                        {
                            api.data.items.RemoveAt(i);
                        }
                    }
                    api.data.items.Insert(0, new QuyenTruyCap() { role_id = "-1", role_name = "Tất cả" });
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
                List<ChildCompany> list = new List<ChildCompany>();
                
                var respon = await httpClient.PostAsync(apilink,new FormUrlEncodedContent(form));
                API_Company_List ts = JsonConvert.DeserializeObject<API_Company_List>(respon.Content.ReadAsStringAsync().Result);
                if (ts != null)
                {
                    if (ts.data != null && ts.data.items != null)
                    {
                        ts.data.items.RemoveAll(item => item == null);
                        list = ts.data.items;
                        list.Add(new ChildCompany() { com_id = Main.APICompany.data.data.user_info.com_id, com_name = Main.APICompany.data.data.user_info.com_name, com_logo = Main.APICompany.data.data.user_info.com_logo, com_address = Main.APICompany.data.data.user_info.com_address, com_email = Main.APICompany.data.data.user_info.com_email, com_parent_id = "", com_phone = Main.APICompany.data.data.user_info.com_phone, com_role_id = Main.APICompany.data.data.user_info.com_role_id });
                    }
                    return list;
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
        private async Task getData()
        {
            List<Task> k = new List<Task>()
            {
                getQuyen().ContinueWith(tt => this.Dispatcher.Invoke(() => listQuyen = tt.Result)),
                getCom().ContinueWith(tt => this.Dispatcher.Invoke(() => {
                    listCom = tt.Result;
                    if (tt.Result.Count>0)
                    {
                        cboCom.SelectedIndex=tt.Result.Count-1;
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
                            switch (Main.Type)
                            {
                                case 1:
                                    getNhanVien(Main.APIStaff.data.data.user_info.com_id,"","",1,20).ContinueWith(aa => this.Dispatcher.Invoke(() =>
                                    {
                                        listNVShow = listNV = aa.Result.data.items;
                                        pagin.TotalRecords = int.Parse(aa.Result.data.totalItems);
                                    }));
                                    break;
                                case 2:
                                    getNhanVien("","","",1,500).ContinueWith(aa => this.Dispatcher.Invoke(() =>
                                    {
                                        listNVShow = listNV = aa.Result.data.items;
                                        pagin.TotalRecords = int.Parse(aa.Result.data.totalItems);
                                    }));
                                    break;
                                default:
                                    break;
                            }
                        });
                    }
                });
            });
        }
        //
        private void table_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Main.scrollMain.OnPreviewMouseWheel(sender, e);
        }
        private void pagin_SelectionChange(object sender, SelectionChangedEventArgs e)
        {
            string com = cboCom.SelectedItem != null ? (cboCom.SelectedItem as ChildCompany).com_id : "";
            string ep = cboNV.SelectedItem != null ? (cboNV.SelectedItem as Employee).ep_id : "";
            string role = cboRole.SelectedItem != null ? (cboRole.SelectedItem as QuyenTruyCap).role_id : "";
            if(cboRole.SelectedIndex == 0)
            {
                role = "";
            }
            getNhanVien(com, role, ep, pagin.SelectedPage).ContinueWith(tt => this.Dispatcher.Invoke(() =>
            {
                if (tt.Result.data.items != null)
                {
                    listNVShow = tt.Result.data.items;
                }
            }));

        }
        private void LocData(object sender, MouseButtonEventArgs e)
        {
            string com = cboCom.SelectedItem != null ? (cboCom.SelectedItem as ChildCompany).com_id : "";
            string ep = cboNV.SelectedItem != null ? (cboNV.SelectedItem as Employee).ep_id : "";
            string role = cboRole.SelectedItem != null ? (cboRole.SelectedItem as QuyenTruyCap).role_id : "";
            if(cboRole.SelectedIndex == 0)
            {
                role = "";
            }
            getNhanVien(com, role, ep).ContinueWith(tt => this.Dispatcher.Invoke(() =>
            {
                if (tt.Result != null)
                {
                    listNVShow = tt.Result.data.items;
                    pagin.TotalRecords = int.Parse(tt.Result.data.totalItems);
                }
            }));
        }
        private async void SetQuyenNhanVien(object sender, SelectionChangedEventArgs e)
        {
            var cbo = (sender as ComboBox);
            var t = cbo.Tag as Employee;
            var item = cbo.SelectedItem as QuyenTruyCap;
            if (t!=null && item!=null && t.role_id!=item.role_id)
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
                            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Main.APIStaff.data.data.access_token);
                            break;
                        case 2:
                            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Main.APICompany.data.data.access_token);
                            break;
                        default:
                            break;
                    }
                    var respon = await httpClient.PostAsync("https://api.timviec365.vn/api/qlc/managerUser/update_permission", new FormUrlEncodedContent(form));
                    var check = respon.Content.ReadAsStringAsync().Result;
                    API_Respon add = JsonConvert.DeserializeObject<API_Respon>(respon.Content.ReadAsStringAsync().Result);
                    if (add.data != null && add.data.result)
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
                    }
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

        private void cboRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string com = cboCom.SelectedItem != null ? (cboCom.SelectedItem as ChildCompany).com_id : "";
            string role = cboRole.SelectedItem != null ? (cboRole.SelectedItem as QuyenTruyCap).role_id : "";
            if (cboRole.SelectedIndex == 0)
            {
                role = "";
            }
            getNhanVien(com, role, "", 1,500).ContinueWith(tt => this.Dispatcher.Invoke(() =>
            {
                if (tt.Result.data.items != null)
                {
                    listNV = tt.Result.data.items;
                    cboNV.Refresh();
                }
            }));
        }
    }
}
