using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
using QuanLyChung365TruocDangNhap.ChamCong365.Core;
using QuanLyChung365TruocDangNhap.ChamCong365.Entities;
using QuanLyChung365TruocDangNhap.ChamCong365.Entities.Company;
using QuanLyChung365TruocDangNhap.ChamCong365.Entities.History_Attenden_Staff;
using Emgu.CV.Flann;
using Newtonsoft.Json;

namespace QuanLyChung365TruocDangNhap.ChamCong365.Views.Pages
{
    /// <summary>
    /// Interaction logic for List_Stuff_all.xaml
    /// </summary>
    public partial class List_Stuff_All : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public List_Stuff_All(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;

            table.AutoReponsiveColumn(1, "IDnTen");
            table2.AutoReponsiveColumn(1, "IDnTen");
            
            getData();
        }
        private MainWindow Main;
        private int pageEP = 1;
        private int totalEP = 0;
        public int index = 0;
        private int _IsSmallSize;
        public int IsSmallSize
        {
            get { return _IsSmallSize; }
            set { _IsSmallSize = value; OnPropertyChanged("IsSmallSize"); }
        }
        private List<Employee> _listEmployee;
        public List<Employee> listEmployee
        {
            get { return _listEmployee; }
            set { _listEmployee = value; OnPropertyChanged(); }
        }
        private List<Employee> _listEmployee1;
        public List<Employee> listEmployee1
        {
            get { return _listEmployee1; }
            set { _listEmployee1 = value; OnPropertyChanged(); }
        }
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

        private List<Item_List_Staff_All> _List_Staff_Duyet;
        public List<Item_List_Staff_All> List_Staff_Duyet
        {
            get { return _List_Staff_Duyet; }
            set
            {
                _List_Staff_Duyet = value;
                OnPropertyChanged();
            }
        }
        private List<ChildCompany> _listCom;
        public List<ChildCompany> listCom
        {
            get { return _listCom; }
            set { _listCom = value; OnPropertyChanged(); }
        }

        private List<Item_List_PhongBan> _listDep;
        public List<Item_List_PhongBan> listDep
        {
            get { return _listDep; }
            set { _listDep = value; OnPropertyChanged(); }
        }
        private int _totalStaff;
        public int totalStaff
        {
            get { return _totalStaff; }
            set { _totalStaff = value; OnPropertyChanged(); }
        }
        private int _totalStaff_Duyet;
        public int totalStaff_Duyet
        {
            get { return _totalStaff_Duyet; }
            set { _totalStaff_Duyet = value; OnPropertyChanged(); }
        }
        private async Task loadEP(int page = 1, int lenght = 20, string idDep="")
        {
            HttpClient http = new HttpClient();
            var apilink = "http://210.245.108.202:3000/api/qlc/managerUser/list";
            var pram = new List<string>();
            Dictionary<string, string> form = new Dictionary<string, string>();
            switch (Main.Type)
            {
                case 1:
                    http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Main.APIStaff.data.data.access_token);
                    form.Add("com_id", Main.APIStaff.data.data.user_info.com_id);
                    break;
                case 2:
                    http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Main.APICompany.data.data.access_token);
                    form.Add("com_id", Main.APICompany.data.data.user_info.com_id);
                    break;
                default:
                    break;
            }
            form.Add("dep_id", idDep);
            form.Add("pageNumber", page.ToString());
            form.Add("pageSize", lenght.ToString());
            HttpResponseMessage respon = await http.PostAsync(apilink, new FormUrlEncodedContent(form));
            API_Employee_List nv = JsonConvert.DeserializeObject<API_Employee_List>(respon.Content.ReadAsStringAsync().Result);
            if (nv.data != null)
            {
                int.TryParse(nv.data.totalItems, out totalEP);
                if (listEmployee == null) listEmployee = new List<Employee>();
                listEmployee.Clear();
                listEmployee.AddRange(nv.data.items);
                listEmployee = listEmployee.ToList();
                searchbarEP.Refresh();
            }
        }
        private async Task loadEP1(int page = 1, int lenght = 20, string idDep="")
        {
            HttpClient http = new HttpClient();
            var apilink = "http://210.245.108.202:3000/api/qlc/managerUser/list";
            var pram = new List<string>();
            Dictionary<string, string> form = new Dictionary<string, string>();
            switch (Main.Type)
            {
                case 1:
                    http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Main.APIStaff.data.data.access_token);
                    form.Add("com_id", Main.APIStaff.data.data.user_info.com_id);
                    break;
                case 2:
                    http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Main.APICompany.data.data.access_token);
                    form.Add("com_id", Main.APICompany.data.data.user_info.com_id);
                    break;
                default:
                    break;
            }
            form.Add("dep_id", idDep);
            form.Add("pageNumber", page.ToString());
            form.Add("pageSize", lenght.ToString());
            HttpResponseMessage respon = await http.PostAsync(apilink, new FormUrlEncodedContent(form));
            API_Employee_List nv = JsonConvert.DeserializeObject<API_Employee_List>(respon.Content.ReadAsStringAsync().Result);
            if (nv.data != null)
            {
                int.TryParse(nv.data.totalItems, out totalEP);
                if (listEmployee1 == null) listEmployee1 = new List<Employee>();
                listEmployee1.Clear();
                listEmployee1.AddRange(nv.data.items);
                listEmployee1 = listEmployee1.ToList();
                searchbarEP2.Refresh();
            }
        }
        public async Task<List<ChildCompany>> getChildCom(bool full = false)
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
                }
                return list;

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
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
        private async Task<API_List_Staff_All> getNV(string com = "", string dep = "", string ep = "", int page = 1, bool duyet = true)
        {
            try
            {
                HttpClient http = new HttpClient();
                var apilink = "http://210.245.108.202:3000/api/qlc/managerUser/list";
                var pram = new List<string>();
                Dictionary<string, string> form = new Dictionary<string, string>();
                switch (Main.Type)
                {
                    case 1:
                        http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Main.APIStaff.data.data.access_token);
                        if (!string.IsNullOrEmpty(com)) form.Add("id_com=", com);
                        break;
                    case 2:
                        http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Main.APICompany.data.data.access_token);
                        if (!string.IsNullOrEmpty(com)) pram.Add("id_com=" + com);
                        else form.Add("com_id", Main.APICompany.data.data.user_info.com_id);
                        break;
                    default:
                        break;
                }
                if (!string.IsNullOrEmpty(dep) && dep != "-1") form.Add("dep_id", dep);
                if (!string.IsNullOrEmpty(ep)) form.Add("ep_id", ep);
                if(!duyet)
                {
                    form.Add("ep_status", "Pending");
                }
                form.Add("pageNumber", page.ToString());
                form.Add("pageSize", "20");
                HttpResponseMessage respon = await http.PostAsync(apilink,new FormUrlEncodedContent(form));
                var check = respon.Content.ReadAsStringAsync().Result;
                API_List_Staff_All api = JsonConvert.DeserializeObject<API_List_Staff_All>(respon.Content.ReadAsStringAsync().Result);
                return api;
            }
            catch (Exception)
            {

                return null;
            }
        }
        //get data table list staff company
        private async Task getData()
        {
            Loading.Visibility = Visibility.Visible;
            listEmployee = new List<Employee>();
            listEmployee1 = new List<Employee>();
            loadEP(1,500,"");
            loadEP1(1,500,"");
            getChildCom().ContinueWith(tt => this.Dispatcher.Invoke(() =>
            {
                listCom = tt.Result;
                listCom.Insert(0, new ChildCompany() { com_id = "-1", com_name = "Tất cả công ty" });
                listCom = listCom.ToList();
                cbbCom.SelectedIndex = listCom.Count - 1;
                Loading.Visibility = Visibility.Collapsed;
            }));
            getNV().ContinueWith(tt => this.Dispatcher.Invoke(() =>
            {
                if (tt.Result != null)
                {
                    List_Staff_All = tt.Result.data.items;
                    paginNV.TotalRecords = int.Parse(tt.Result.data.totalItems);
                    totalStaff = int.Parse(tt.Result.data.totalItems);
                }
            }));
            getNV("", "", "", 1, false).ContinueWith(tt => this.Dispatcher.Invoke(() =>
                {
                    if (tt.Result != null)
                    {
                        List_Staff_Duyet = tt.Result.data.items;
                        int dem;
                        if (!string.IsNullOrEmpty(tt.Result.data.totalItems) && int.TryParse(tt.Result.data.totalItems, out dem))
                        {
                            totalStaff_Duyet = dem;
                            paginDuyet.TotalRecords = dem;
                        }
                    }
                }));


            tab.SelectedIndex = 1;
            tab.SelectedIndex = 0;
        }
        //ShowPopUp
        private void Add_Staff_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var p = new Views.PopUp.popUp_QuanLyNV.Add_Staff(Main);
            Main.ShowPopup(p);

            p.Success = () =>
            {
                getData();
            };
        }
        //scroll when mouse wheel
        private void table_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Main.scrollMain.OnPreviewMouseWheel(sender, e);
        }
        private void sreachbarEP_ScrollToEnd()
        {
            if (listEmployee.Count + (pageEP + 1 * 20) <= totalEP)
            {
                pageEP++;
                loadEP(pageEP, 20,"");
            }
        }
        private void sreachbarEP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void SelectedAllStaff(object sender, MouseButtonEventArgs e)
        {
            var x = sender as CheckBox;
            if (x != null)
            {
                List_Staff_Duyet.ForEach(i => i.IsSelected = !x.IsChecked.Value);
                List_Staff_Duyet = List_Staff_Duyet.ToList();
            }

        }

        private void btn_ApDung_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string com = "";
            if (cbbCom.SelectedItem != null && cbbCom.SelectedIndex > 1) com = (cbbCom.SelectedItem as ChildCompany).com_id;

            string dep = "";
            if (cbbPB.SelectedItem != null && cbbPB.SelectedIndex >= 1) dep = (cbbPB.SelectedItem as Item_List_PhongBan).dep_id;

            string ep = "";
            if (searchbarEP.SelectedItem != null) ep = (searchbarEP.SelectedItem as Employee).ep_name;

            getNV(com, dep, ep).ContinueWith(tt => this.Dispatcher.Invoke(() =>
              {
                  if (tt.Result != null)
                  {
                      List_Staff_All = tt.Result.data.items;
                      paginNV.TotalRecords = int.Parse(tt.Result.data.totalItems);
                      totalStaff = int.Parse(tt.Result.data.totalItems);
                  }
              }));
        }
        private void cbbCom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var z = cbbCom.SelectedItem as ChildCompany;
            if (z != null)
            {
                getDep(z.com_id).ContinueWith(tt => this.Dispatcher.Invoke(() =>
                {
                    listDep = tt.Result;
                    listDep.Insert(0, new Item_List_PhongBan() { dep_id = "-1", dep_name = "Tất cả phòng ban" });
                    listDep = listDep.ToList();
                }));
            }
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.ActualWidth > 980)
            {
                IsSmallSize = 0;
            }
            else if (this.ActualWidth <= 980 && this.ActualWidth > 460)
            {
                IsSmallSize = 1;
            }
            else /*(this.ActualWidth <= 460)*/
            {
                IsSmallSize = 2;
            }
        }

        private void Edit_Staff_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow)
                {
                    var row = (DataGridRow)vis;
                    var z = row.Item as Item_List_Staff_All;

                    if (z != null)
                    {
                        var x = new PopUp.popUp_QuanLyNV.Add_Staff(Main, z);
                        Main.ShowPopup(x);

                        x.Success = () =>
                        {
                            getData();
                        };
                    }
                    break;
                }
        }

        private void DeleteStaff_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow)
                {
                    var row = (DataGridRow)vis;
                    var z = row.Item as Item_List_Staff_All;

                    if (z != null)
                    {
                        try
                        {
                            var c = new PopUp.Noti_Delete(Main);
                            c.Message = "Bạn có chắc chắn muốn xóa Nhân viên này này?";
                            Main.ShowPopup(c);
                            c.Accept = async () =>
                            {
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
                                Dictionary<string, string> form = new Dictionary<string, string>();
                                form.Add("idQLC", z.ep_id);
                                HttpResponseMessage respon = await http.PostAsync("http://210.245.108.202:3000/api/qlc/managerUser/del", new FormUrlEncodedContent(form));
                                API_Respon api = JsonConvert.DeserializeObject<API_Respon>(respon.Content.ReadAsStringAsync().Result);
                                if (api.data != null && api.data.result == true)
                                {
                                    getData();

                                }
                            };

                        }
                        catch (Exception ex)
                        {

                            MessageBox.Show(ex.Message);
                        }
                    }
                    break;
                }
        }

        private void paginNV_SelectionChange(object sender, SelectionChangedEventArgs e)
        {
            getNV("", "", "", paginNV.SelectedPage).ContinueWith(tt => this.Dispatcher.Invoke(() =>
               {
                   if (tt.Result != null)
                   {
                       List_Staff_All = tt.Result.data.items;
                   }
               }));
        }

        private void btn_ApDung2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string com = "";
            if (cbbCom2.SelectedItem != null && cbbCom2.SelectedIndex>1) com = (cbbCom2.SelectedItem as ChildCompany).com_id;

            string dep = "";
            if (cbbPB2.SelectedItem != null && cbbPB2.SelectedIndex >= 1) dep = (cbbPB2.SelectedItem as Item_List_PhongBan).dep_id;

            string ep = "";
            if (searchbarEP2.SelectedItem != null) ep = (searchbarEP2.SelectedItem as Item_List_Staff_All).ep_name;

            getNV(com, dep, ep, 1, false).ContinueWith(tt => this.Dispatcher.Invoke(() =>
              {
                  if (tt.Result != null)
                  {
                      List_Staff_Duyet = tt.Result.data.items;
                      int dem;
                      if (!string.IsNullOrEmpty(tt.Result.data.totalItems) && int.TryParse(tt.Result.data.totalItems, out dem)) totalStaff_Duyet = dem;
                  }
              }));
        }
        private void paginDuyet_SelectionChange(object sender, SelectionChangedEventArgs e)
        {
            string com = "";
            if (cbbCom2.SelectedItem != null && cbbCom2.SelectedIndex > 1) com = (cbbCom2.SelectedItem as ChildCompany).com_id;

            string dep = "";
            if (cbbPB2.SelectedItem != null && cbbPB2.SelectedIndex >= 1) dep = (cbbPB2.SelectedItem as Item_List_PhongBan).dep_id;

            string ep = "";
            if (searchbarEP2.SelectedItem != null) ep = (searchbarEP2.SelectedItem as Item_List_Staff_All).ep_name;

            getNV(com, dep, ep, paginDuyet.SelectedPage, false).ContinueWith(tt => this.Dispatcher.Invoke(() =>
            {
                if (tt.Result != null)
                {
                    List_Staff_Duyet = tt.Result.data.items;
                    int dem;
                    if (!string.IsNullOrEmpty(tt.Result.data.totalItems) && int.TryParse(tt.Result.data.totalItems, out dem)) totalStaff_Duyet = dem;
                }
            }));
        }
        private async Task<bool> deleteNhanVien(string id)
        {
            try
            {
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
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                httpRequestMessage.RequestUri = new Uri("http://210.245.108.202:3000/api/qlc/managerUser/del");
                Dictionary<string, string> form = new Dictionary<string, string>();
                form.Add("idQLC", id);
                httpRequestMessage.Content = new FormUrlEncodedContent(form);
                HttpResponseMessage respon = await http.SendAsync(httpRequestMessage);
                var check = respon.Content.ReadAsStringAsync().Result;
                API_Respon api = JsonConvert.DeserializeObject<API_Respon>(respon.Content.ReadAsStringAsync().Result);
                if (api.data != null && api.data.result == true)
                {
                    getData();
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                return false;
            }
        }
        private async void DuyetNhanVien(List<string> id)
        {
            try
            {
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
                Dictionary<string, string> form = new Dictionary<string, string>();
                form.Add("listUsers", string.Join(",", id));
                HttpResponseMessage respon = await http.PostAsync("https://api.timviec365.vn/api/qlc/managerUser/verifyListUsers", new FormUrlEncodedContent(form));
                API_Respon api = JsonConvert.DeserializeObject<API_Respon>(respon.Content.ReadAsStringAsync().Result);
                if (api.data != null && api.data.result == true)
                {
                    getData();
                    var n = new Views.PopUp.Notify1();
                    n.Type = PopUp.Notify1.NotifyType.Success;
                    n.setMessage("Cập nhật thông tin thành công");
                    Main.ShowPopup(n);
                }
                else
                {
                    var n = new Views.PopUp.Notify1();
                    n.Type = PopUp.Notify1.NotifyType.Error;
                    n.setMessage("Cập nhật thông tin thất bại, vui lòng thử lại sau");
                    Main.ShowPopup(n);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private async void DeleteStaffDuyet_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            List<string> ids = new List<string>();
            List_Staff_Duyet.ForEach(i => { if (i.IsSelected && !ids.Contains(i.ep_id)) ids.Add(i.ep_id); });
            var c = new PopUp.Noti_Delete(Main);
            if (ids.Count > 1) c.Message = "Bạn có chắc chắn muốn xóa các nhân viên này này?";
            else c.Message = "Bạn có chắc chắn muốn xóa nhân viên này này?";
            Main.ShowPopup(c);
            c.Accept = async () =>
            {
                List<Task> k = new List<Task>();
                var ckn = new List<bool>();
                ids.ForEach(i =>
                {
                    k = new List<Task>() { deleteNhanVien(i).ContinueWith(tt => this.Dispatcher.Invoke(() => ckn.Add(tt.Result))), };
                });
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
                                if (!ck.Contains(false))
                                {
                                    var n = new Views.PopUp.Notify1();
                                    n.Type = PopUp.Notify1.NotifyType.Success;
                                    n.setMessage("Cập nhật thông tin thành công");
                                    Main.ShowPopup(n);
                                    tab.SelectedIndex = 1;
                                }
                                else
                                {
                                    var n = new Views.PopUp.Notify1();
                                    n.Type = PopUp.Notify1.NotifyType.Error;
                                    n.setMessage("Cập nhật thông tin thất bại, vui lòng thử lại sau");
                                    Main.ShowPopup(n);
                                    tab.SelectedIndex = 1;
                                }
                            });
                        }
                    });
                });
            };
        }
        private void DuyetNhanVien_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            List<string> ids = new List<string>();
            List_Staff_Duyet.ForEach(i => { if (i.IsSelected && !ids.Contains(i.ep_id)) ids.Add(i.ep_id); });
            DuyetNhanVien(ids);
            tab.SelectedIndex = 1;
        }

        private void tab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(tab.SelectedIndex == 2)
            {
                Main.SideBarIndex = 3;
                var pop = new Views.Pages.Cap_Nhap_Lai_Khuon_Mat(Main);
                Main.HomeSelectionPage.NavigationService.Navigate(pop);
                pop.tb.SelectedIndex = 2;
            }
        }

        private void cbbPB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Item_List_PhongBan pb = (Item_List_PhongBan)(sender as ComboBox).SelectedItem;
            loadEP(1, 500, pb.dep_id);
        }
        private void cbbPB2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Item_List_PhongBan pb = (Item_List_PhongBan)(sender as ComboBox).SelectedItem;
            loadEP1(1, 500, pb.dep_id);
        }
    }
}
