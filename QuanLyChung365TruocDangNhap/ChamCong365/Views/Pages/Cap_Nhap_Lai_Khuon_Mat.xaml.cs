using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
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
using Newtonsoft.Json;

namespace QuanLyChung365TruocDangNhap.ChamCong365.Views.Pages
{
    /// <summary>
    /// Interaction logic for Cap_Nhap_Lai_Khuon_Mat.xaml
    /// </summary>
    public partial class Cap_Nhap_Lai_Khuon_Mat : Page, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public Cap_Nhap_Lai_Khuon_Mat(MainWindow main)
        {
            InitializeComponent();
            Main = main;
            this.DataContext = this;
            table.AutoReponsiveColumn(1, "IDnTen");

            getData();
        }
        public MainWindow Main;
        private int pageEP = 1;
        private int totalEP = 0;
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

        private int _totalStaff;

        public int totalStaff
        {
            get { return _totalStaff; }
            set { _totalStaff = value; OnPropertyChanged(); }
        }


        private List<Employee> _listEmployee;
        public List<Employee> listEmployee
        {
            get { return _listEmployee; }
            set { _listEmployee = value; OnPropertyChanged(); }
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
        public Action Success
        {
            get { return (Action)GetValue(SuccessProperty); }
            set { SetValue(SuccessProperty, value); }
        }
        public static readonly DependencyProperty SuccessProperty =
            DependencyProperty.Register("Success", typeof(Action), typeof(Cap_Nhap_Lai_Khuon_Mat));
        private async Task loadEP(string idCom, string idDep, int page = 1, int lenght = 20)
        {
            HttpClient http = new HttpClient();
            var apilink = "http://210.245.108.202:3000/api/qlc/managerUser/list";
            var pram = new List<string>();
            Dictionary<string, string> form = new Dictionary<string, string>();
            switch (Main.Type)
            {
                case 1:
                    http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Main.APIStaff.data.data.access_token);
                    //pram.Add("id_com=" + Main.APIStaff.data.data.user_info.com_id);
                    form.Add("com_id", Main.APIStaff.data.data.user_info.com_id);
                    break;
                case 2:
                    http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Main.APICompany.data.data.access_token);
                    if (!string.IsNullOrEmpty(idCom)) form.Add("com_id", idCom);
                    else form.Add("com_id", Main.APICompany.data.data.user_info.com_id);
                    break;
                default:
                    break;
            }
            if (page <= 0) page = 1;
            var offset = (page - 1) * 20;
            if (!string.IsNullOrEmpty(idDep)) form.Add("dep_id", idDep);
            //pram.Add("off_set=" + offset.ToString());
            //pram.Add("length=" + lenght.ToString());
            //pram.Add("filter_by[active]=true");
            form.Add("pageNumber", page.ToString());
            
            var respon = await http.PostAsync(apilink, new FormUrlEncodedContent(form));
            API_Employee_List nv = JsonConvert.DeserializeObject<API_Employee_List>(respon.Content.ReadAsStringAsync().Result);
            if (nv.data != null)
            {
                int.TryParse(nv.data.totalItems, out totalEP);
                if (listEmployee == null) listEmployee = new List<Employee>();
                if (!string.IsNullOrEmpty(idDep))
                {
                    listEmployee.Clear();
                }
                listEmployee = nv.data.items;
                listEmployee = listEmployee.ToList();
                searchbarEP.Refresh();
            }
        }
        private async Task getData()
        {
            string id_com = "";
            switch (Main.Type)
            {
                case 1:
                    id_com = Main.APIStaff.data.data.user_info.com_id;
                    break;
                case 2:
                    id_com = Main.APICompany.data.data.user_info.com_id;
                    break;
                default:
                    break;
            }
            listEmployee = new List<Employee>();
            loadEP("","",1,500);
            getChildCom().ContinueWith(tt => this.Dispatcher.Invoke(() =>
            {
                listCom = tt.Result;
                cbbCom.SelectedIndex = listCom.Count - 1;
            }));
            getNV(id_com).ContinueWith(tt => this.Dispatcher.Invoke(() =>
            {
                if (tt.Result != null)
                {
                    List_Staff_All = tt.Result.data.items;
                    pagin.TotalRecords = int.Parse(tt.Result.data.totalItems);
                    totalStaff = int.Parse(tt.Result.data.totalItems);
                }
            }));
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
                list.Insert(0, new Item_List_PhongBan() { dep_id = "-1", dep_name = "Tất cả phòng ban" });
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
                        if (!string.IsNullOrEmpty(com)) form.Add("com_id", com);
                        else form.Add("com_id", Main.APIStaff.data.data.user_info.com_id);
                        break;
                    case 2:
                        http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Main.APICompany.data.data.access_token);
                        if (!string.IsNullOrEmpty(com)) form.Add("com_id", com);
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
        }
        private void table_Cap_Nhap_Lai_Khuon_Mat_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Main.scrollMain.OnPreviewMouseWheel(sender, e);
        }
        private void cbbCom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var z = cbbCom.SelectedItem as ChildCompany;
            if (z != null)
            {
                getDep(z.com_id).ContinueWith(tt => this.Dispatcher.Invoke(() =>
                {
                    listDep = tt.Result;
                    cbbPB.SelectedIndex = 0;
                    /*listDep.Insert(0, new Item_List_PhongBan() { dep_id = "-1", dep_name = "Tất cả phòng ban" });
                    listDep = listDep.ToList();*/
                }));
            }
        }
        private void table_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Main.scrollMain.OnPreviewMouseWheel(sender, e);
        }
        private void sreachbarEP_ScrollToEnd()
        {
            /*if (listEmployee.Count + (pageEP + 1 * 20) <= totalEP)
            {
                pageEP++;
                loadEP("","",pageEP, 20);
            }*/
        }
        private async void CheckBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                    if (vis is DataGridRow)
                    {
                        var row = (DataGridRow)vis;
                        var z = row.Item as Item_List_Staff_All;

                        if (z != null)
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
                            form.Add("list_id", z.ep_id);
                            HttpResponseMessage respon = await http.PostAsync("http://210.245.108.202:3000/api/qlc/face/add", new FormUrlEncodedContent(form));
                            API_Respon api = JsonConvert.DeserializeObject<API_Respon>(respon.Content.ReadAsStringAsync().Result);
                            if (z.allow_update_face == "0")
                            {
                                if (api.data != null && api.data.result == true)
                                {
                                    if (Success != null) Success();
                                    var ep = List_Staff_All.Find(i => i.ep_id == z.ep_id);
                                    ep.allow_update_face = "1";
                                    List_Staff_All = List_Staff_All.ToList();

                                    var x = new Views.PopUp.Notify1();
                                    x.Type = PopUp.Notify1.NotifyType.Success;
                                    x.Message = "Cấp quyền thành công";
                                    Main.ShowPopup(x);
                                }
                            }
                            else
                            {
                                if (api.data != null && api.data.result == true)
                                {
                                    if (Success != null) Success();
                                    var ep = List_Staff_All.Find(i => i.ep_id == z.ep_id);
                                    ep.allow_update_face = "0";
                                    List_Staff_All = List_Staff_All.ToList();
                                    var x = new Views.PopUp.Notify1();
                                    x.Type = PopUp.Notify1.NotifyType.Success;
                                    x.Message = "Huỷ quyền thành công";
                                    Main.ShowPopup(x);
                                }
                            }
                        }
                        break;
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void pagin_SelectionChange(object sender, SelectionChangedEventArgs e)
        {
            string com = cbbCom.SelectedItem != null ? (cbbCom.SelectedItem as ChildCompany).com_id : "";
            string dep = cbbPB.SelectedItem != null ? (cbbPB.SelectedItem as Item_List_PhongBan).dep_id : "";
            string ep = searchbarEP.SelectedItem != null ? (searchbarEP.SelectedItem as Employee).ep_id : "";

            getNV(com, dep, ep, pagin.SelectedPage).ContinueWith(tt => this.Dispatcher.Invoke(() =>
            {
                if (tt.Result != null)
                {
                    List_Staff_All = tt.Result.data.items;
                }
            }));
        }
        private void btn_ApDung_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string com = cbbCom.SelectedItem != null ? (cbbCom.SelectedItem as ChildCompany).com_id : "";
            string dep = cbbPB.SelectedItem != null ? (cbbPB.SelectedItem as Item_List_PhongBan).dep_id : "";
            string ep = searchbarEP.SelectedItem != null ? (searchbarEP.SelectedItem as Employee).ep_id : "";
            if(cbbPB.SelectedIndex == 0)
            {
                dep = "";
            }
            if(searchbarEP.SelectedItem != null)
            {
                ep = searchbarEP.Text.Split(' ')[0];
            }
            getNV(com, dep, ep).ContinueWith(tt => this.Dispatcher.Invoke(() =>
            {
                if (tt.Result != null)
                {
                    List_Staff_All = tt.Result.data.items;
                    pagin.TotalRecords = int.Parse(tt.Result.data.totalItems);
                }
            }));
        }

        private void tb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(tb.SelectedIndex == 0)
            {
                //Main.SideBarIndex = 11;
                /*var pop = new Views.Pages.List_Stuff_All(Main);
                Main.HomeSelectionPage.NavigationService.Navigate(pop);
                pop.tab.SelectedIndex = 0;
                Main.Title = App.Current.Resources["textQuanLyNhanVien"] as string;*/
                Main.sidebar.SelectedIndex = 11;
            }
            else if(tb.SelectedIndex == 1)
            {
                Main.SideBarIndex = 11;
                var pop = new Views.Pages.List_Stuff_All(Main);
                Main.HomeSelectionPage.NavigationService.Navigate(pop);
                pop.tab.SelectedIndex = 1;
            }
            else if(tb.SelectedIndex == 3)
            {
                Main.SideBarIndex = 11;
                var pop = new Views.Pages.List_Stuff_All(Main);
                Main.HomeSelectionPage.NavigationService.Navigate(pop);
                pop.tab.SelectedIndex = 3;
            }
        }

        private void cbbPB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var com = cbbCom.SelectedItem as ChildCompany;
            var dep = cbbPB.SelectedItem as Item_List_PhongBan;
            if (dep.dep_id != "-1")
            {
                loadEP(com.com_id, dep.dep_id, 1, 500);
            }
            else
            {
                loadEP(com.com_id, "", 1, 500);
            }
        }
    }
}
