using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using QuanLyChung365TruocDangNhap.ChamCong365.Core;
using QuanLyChung365TruocDangNhap.ChamCong365.Entities;
using QuanLyChung365TruocDangNhap.ChamCong365.Entities.Company;
using Newtonsoft.Json;

namespace QuanLyChung365TruocDangNhap.ChamCong365.Views.Pages
{
    /// <summary>
    /// Interaction logic for Thong_Ke_De_Xuat_Nghi.xaml
    /// </summary>
    public partial class Thong_Ke_De_Xuat_Nghi : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public Thong_Ke_De_Xuat_Nghi(MainWindow main)
        {
            InitializeComponent();
            Main = main;
            this.DataContext = this;
            getDataCa();
            
        }
        public MainWindow Main;
        public List<string> listMonth { get; set; } = new List<string>() { "Chọn tháng", "Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4", "Tháng 5", "Tháng 6", "Tháng 7", "Tháng 8", "Tháng 9", "Tháng 10", "Tháng 11", "Tháng 12", };
        public List<string> listYear { get; set; } = new List<string>() { "Chọn năm", "Năm 2020", "Năm 2021", "Năm 2022", "Năm 2023", "Năm 2024", "Năm 2025", };
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
        private int totalEP = 0;
        private Item_ThongKeDeXuatNghi info;
        private List<Item_ThongKeDeXuatNghi> _ListDeXuat;
        public List<Item_ThongKeDeXuatNghi> ListDeXuat
        {
            get { return _ListDeXuat; }
            set { _ListDeXuat = value; OnPropertyChanged(); }
        }

        private List<Item_ThongKeDeXuatNghi> _ListEPDeXuat;
        public List<Item_ThongKeDeXuatNghi> ListEPDeXuat
        {
            get { return _ListEPDeXuat; }
            set
            {
                _ListEPDeXuat = value;
                OnPropertyChanged();
            }
        }

        private List<Employee> _listEp;
        public List<Employee> listEp
        {
            get { return _listEp; }
            set { _listEp = value; OnPropertyChanged(); }
        }
        private List<Employee> _listEmployee;
        public List<Employee> listEmployee
        {
            get { return _listEmployee; }
            set { _listEmployee = value; OnPropertyChanged(); }
        }

        private List<Entities.DSCaLamViec> _listCa;

        public List<DSCaLamViec> listCa
        {
            get { return _listCa; }
            set
            {
                /*if (value == null) value = new List<DSCaLamViec>();
                value.Insert(0,new DSCaLamViec() { shift_id="-1",shift_name="Tất cả các ca" });*/
                _listCa = value;
                OnPropertyChanged();
            }
        }

        private async void getDataCa()
        {
            try
            {
                HttpClient http = new HttpClient();
                var apilink = "http://210.245.108.202:3000/api/qlc/shift/list";
                var pram = new Dictionary<string, string>();

                switch (Main.Type)
                {
                    case 1:
                        http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Main.APIStaff.data.data.access_token);
                        //.Add("com_id", Main.APIStaff.data.data.user_info.com_id);
                        apilink += "?com_id=" + Main.APIStaff.data.data.user_info.com_id;
                        break;
                    case 2:
                        http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Main.APICompany.data.data.access_token);
                        break;
                    default:
                        break;
                }
                HttpResponseMessage respon = await http.GetAsync(apilink);
                API_DSCaLamViec api = JsonConvert.DeserializeObject<API_DSCaLamViec>(respon.Content.ReadAsStringAsync().Result);
                if (api.data != null)
                {
                    listCa = api.data.items;
                    getData();
                }
                /*using (WebClient web = new WebClient())
                {
                    if (Main.Type == 2)
                    {
                        //web.QueryString.Add("com_id", Main.APICompany.data.data.user_info.com_id);
                        web.Headers.Add("Authorization", "Bearer" + Main.APICompany.data.data.access_token);
                    }
                    var e = web.UploadValues("http://210.245.108.202:3000/api/qlc/shift/list", "GET", web.QueryString);

                    *//*web.UploadValuesCompleted += (s, e) =>
                    {*//*
                        try
                        {
                            var check = UnicodeEncoding.UTF8.GetString(e);
                            API_DSCaLamViec api = JsonConvert.DeserializeObject<API_DSCaLamViec>(UnicodeEncoding.UTF8.GetString(e));
                            if (api.data != null)
                            {
                                listCa = api.data.items;
                                getData();
                            }
                        }
                        catch { }
                    *//*};*//*
                }*/
            }
            catch (Exception ex)
            {


            }
        }

        private async Task loadEP(int page = 1, int lenght = 20)
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
            form.Add("pageNumber", page.ToString());
            form.Add("pageSize", lenght.ToString());
            HttpResponseMessage respon = await http.PostAsync(apilink, new FormUrlEncodedContent(form));
            API_Employee_List nv = JsonConvert.DeserializeObject<API_Employee_List>(respon.Content.ReadAsStringAsync().Result);
            if (nv.data != null)
            {
                int.TryParse(nv.data.totalItems, out totalEP);
                if (listEmployee == null) listEmployee = new List<Employee>();
                listEmployee.AddRange(nv.data.items);
                listEmployee = listEmployee.ToList();
                searchbarEP.Refresh();
            }
        }
        private async Task loadEP_DeXuat()
        {
            HttpClient http = new HttpClient();
            string idcom = "";
            switch (Main.Type)
            {
                case 1:
                    http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Main.APIStaff.data.data.access_token);
                    idcom = Main.APIStaff.data.data.user_info.com_id;
                    break;
                case 2:
                    http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Main.APICompany.data.data.access_token);
                    idcom = Main.APICompany.data.data.user_info.com_id;
                    break;
                default:
                    break;
            }
            Dictionary<string, string> form = new Dictionary<string, string>();
            form.Add("id_com", idcom);
            //respon = await http.PostAsync("https://vanthu.timviec365.vn/api/cc365_get_de_xuat.php", new FormUrlEncodedContent(form));
            var respon = await http.PostAsync("http://210.245.108.202:3005/api/vanthu/DeXuat/admin_danh_sach_de_xuat", new FormUrlEncodedContent(form));
            API_List_De_Xuat Dexuat = JsonConvert.DeserializeObject<API_List_De_Xuat>(respon.Content.ReadAsStringAsync().Result);
            API_ThongKeDeXuatNghi nv = new API_ThongKeDeXuatNghi();
            nv.data = new Data_ThongKeDeXuatNghi();
            nv.data.items = new List<Item_ThongKeDeXuatNghi>();
            if (Dexuat != null && Dexuat.data.data != null && Dexuat.data.data.Count > 0)
            {
                /*foreach (var item in Dexuat.data.data)
                {
                    using (WebClient web = new WebClient())
                    {
                        web.Headers.Add("Authorization", "Bearer " + Main.APICompany.data.data.access_token);
                        web.QueryString.Add("_id", item._id.ToString());
                        var result = web.UploadValues(new Uri("http://210.245.108.202:3005/api/vanthu/catedx/showCTDX"), "POST", web.QueryString);
                        API_Detail_De_Xuat api = JsonConvert.DeserializeObject<API_Detail_De_Xuat>(UTF8Encoding.UTF8.GetString(result));
                        nv.data.items.Add(new Item_ThongKeDeXuatNghi() { id_de_xuat = item._id.ToString(), id_user = api.data.detailDeXuat[0].id_nguoi_tao.ToString(), name_user = api.data.detailDeXuat[0].nguoi_tao, info = api.data.detailDeXuat[0].thong_tin_chung.nghi_phep , id_user_duyet = api.data.detailDeXuat[0].lanh_dao_duyet[1].idQLC.ToString(), id_user_theo_doi = api.data.detailDeXuat[0].nguoi_theo_doi[0].idQLC.ToString(), ng_duyet = api.data.detailDeXuat[0].lanh_dao_duyet[1].userName });
                    }
                }*/
                nv.data.totalItems = Dexuat.data.data.Count.ToString();
            }
            if (nv.data != null)
            {
                int.TryParse(nv.data.totalItems, out totalEP);
                if (ListDeXuat == null) ListDeXuat = new List<Item_ThongKeDeXuatNghi>();
                ListDeXuat.Clear();
                ListDeXuat.AddRange(nv.data.items);
                ListDeXuat = ListDeXuat.ToList();
                //searchbarEP.Refresh();
            }
        }
        private async Task<API_ThongKeDeXuatNghi> getDeXuat(string ep = "", string month = "", string year = "", int page = 1)
        {
            HttpClient http = new HttpClient();
            string idcom = "";
            switch (Main.Type)
            {
                case 1:
                    http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Main.APIStaff.data.data.access_token);
                    idcom = Main.APIStaff.data.data.user_info.com_id;
                    break;
                case 2:
                    http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Main.APICompany.data.data.access_token);
                    idcom = Main.APICompany.data.data.user_info.com_id;
                    break;
                default:
                    break;
            }
            if (page <= 0) page = 1;
            var date = "";
            if (!string.IsNullOrEmpty(year)) date = year;
            if (month.Length == 1) month = "0" + month;
            if (!string.IsNullOrEmpty(year) && !string.IsNullOrEmpty(month)) date = year + "-" + month;
            ListDeXuat = new List<Item_ThongKeDeXuatNghi>();
            //loadEP_DeXuat();
            Dictionary<string, string> form = new Dictionary<string, string>();
            //form.Add("id_com", idcom);
            form.Add("id_user", ep);
            //form.Add("type_duyet", info.type_duyet);
            form.Add("time_send_form", date);
            form.Add("page", page.ToString());
            form.Add("type_duyet", "5");
            form.Add("type_dx", "1");
            var respon = await http.PostAsync("http://210.245.108.202:3005/api/vanthu/DeXuat/admin_danh_sach_de_xuat", new FormUrlEncodedContent(form));
            API_List_De_Xuat Dexuat = JsonConvert.DeserializeObject<API_List_De_Xuat>(respon.Content.ReadAsStringAsync().Result);
            API_ThongKeDeXuatNghi api = new API_ThongKeDeXuatNghi();
            api.data = new Data_ThongKeDeXuatNghi();
            api.data.items = new List<Item_ThongKeDeXuatNghi>();
            if (Dexuat != null && Dexuat.data.data != null && Dexuat.data.data.Count > 0)
            {
                foreach (var item in Dexuat.data.data)
                {
                    using (WebClient web = new WebClient())
                    {
                        try
                        {
                            web.Headers.Add("Authorization", "Bearer " + Main.APICompany.data.data.access_token);
                            web.QueryString.Add("_id", item._id.ToString());
                            var result = web.UploadValues(new Uri("http://210.245.108.202:3005/api/vanthu/catedx/showCTDX"), "POST", web.QueryString);
                            API_Detail_De_Xuat api1 = JsonConvert.DeserializeObject<API_Detail_De_Xuat>(UTF8Encoding.UTF8.GetString(result));
                            List<Entities.ND> noiDung = new List<Entities.ND>();
                            for (int i = 0; i < api1.data.detailDeXuat[0].thong_tin_chung.nghi_phep.nd.Count; i++)
                            {
                                var detailND = api1.data.detailDeXuat[0].thong_tin_chung.nghi_phep.nd[i];
                                noiDung.Add(new Entities.ND());
                                noiDung[i].ca_nghi = detailND.ca_nghi;
                                noiDung[i].ngaybatdau_nghi = detailND.bd_nghi;
                                noiDung[i].ngayketthuc_nghi = detailND.kt_nghi;
                            }
                            api.data.items.Add(new Item_ThongKeDeXuatNghi() { id_de_xuat = item._id.ToString(), id_user = api1.data.detailDeXuat[0].id_nguoi_tao.ToString(), name_user = api1.data.detailDeXuat[0].nguoi_tao, info = new InfoThongKeDeXuatNghi() { ly_do = api1.data.detailDeXuat[0].thong_tin_chung.nghi_phep.ly_do, loai_nghi_phep = api1.data.detailDeXuat[0].thong_tin_chung.nghi_phep.loai_np.ToString(), nd = noiDung }, id_user_duyet = api1.data.detailDeXuat[0].lanh_dao_duyet[0].idQLC.ToString(), id_user_theo_doi = api1.data.detailDeXuat[0].nguoi_theo_doi[0].idQLC.ToString(), ng_duyet = api1.data.detailDeXuat[0].lanh_dao_duyet[0].userName });
                        }
                        catch (Exception ex)
                        { 

                        }
                    }
                }
                api.data.totalItems = (Dexuat.data.totalPages * 10).ToString();
            }
            if (api.data != null)
            {
                api.data.items.ForEach(i =>
                {
                    i.STT = api.data.items.IndexOf(i) + 1;
                    foreach(var item in listCa)
                    {
                        if(i.info.nd[0].ca_nghi == item.shift_id)
                        {
                            i.tenCa = item.shift_name;
                        }
                    }
                    if (i.info.nd[0].ca_nghi == "Nghỉ cả ngày")
                    {
                        i.tenCa = "Nghỉ cả ngày";
                    }
                    
                });
                //ListDeXuat = api.data.items;
            }
            return api;

        }
        private async Task getData()
        {
            var year = DateTime.Today.ToString("yyyy");
            var month = DateTime.Today.Month;
            cboYear.SelectedIndex = listYear.FindIndex(x => x == "Năm " + year);
            cboMonth.SelectedIndex = month;
            var dem = 0;
            var k = new List<Task>
            {
                getEp().ContinueWith(tt=>this.Dispatcher.Invoke(()=>{
                    if(tt.Result!=null)
                    {
                        dem=int.Parse(tt.Result.data.totalItems);
                    }
                })),
                loadEP(1,500).ContinueWith(p=>{

                }),
                getDeXuat("",month.ToString(),year,1).ContinueWith(tt => this.Dispatcher.Invoke(() =>
                {
                    if (tt.Result != null)
                    {
                        ListEPDeXuat=ListDeXuat = tt.Result.data.items;
                        ListDeXuat.ForEach(i => i.STT = ListDeXuat.IndexOf(i) + 1);
                        ListDeXuat = ListDeXuat.ToList();
                        pagin.TotalRecords=int.Parse(tt.Result.data.totalItems);
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
                            getEp("", dem).ContinueWith(zz => this.Dispatcher.Invoke(() =>
                            {
                                if (zz.Result != null)
                                {
                                    listEp = zz.Result.data.items;
                                    ListDeXuat.ForEach(i =>
                                    {
                                        var ep = listEp.Where(h => h.ep_id == i.id_user).SingleOrDefault();
                                        if (ep != null) i.dep_name = ep.dep_name;

                                        var duyet = listEp.Where(h => h.ep_id == i.id_user_duyet).SingleOrDefault();
                                        if (duyet != null) i.ng_duyet = duyet.ep_name;
                                    });
                                    ListDeXuat = ListDeXuat.ToList();
                                }
                            }));
                        });
                    }
                });
            });
        }
        private async Task<API_Employee_List> getEp(string ep = "", int length = 1)
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
                        form.Add("com_id", Main.APIStaff.data.data.user_info.com_id);
                        break;
                    case 2:
                        http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Main.APICompany.data.data.access_token);
                        form.Add("com_id", Main.APICompany.data.data.user_info.com_id);
                        break;
                    default:
                        break;
                }
                //form.Add("pageNumber", page.ToString());
                form.Add("pageSize", length.ToString());
                form.Add("ep_id", ep);
                HttpResponseMessage respon = await http.PostAsync(apilink, new FormUrlEncodedContent(form));
                API_Employee_List api = JsonConvert.DeserializeObject<API_Employee_List>(respon.Content.ReadAsStringAsync().Result);
                if (api.data != null) return api;
                return null;
            }
            catch (Exception ex)
            {

                var x = new PopUp.Notify1();
                x.Type = PopUp.Notify1.NotifyType.Error;
                x.Message = ex.Message;
                Main.ShowPopup(x);
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

        private void table_Ghi_Nhan_Cong_Staff_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Main.scrollMain.OnPreviewMouseWheel(sender, e);
        }

        private void pagin_SelectionChange(object sender, SelectionChangedEventArgs e)
        {
            var month = cboMonth.SelectedItem != null && cboMonth.SelectedIndex >= 1 ? cboMonth.SelectedIndex.ToString() : "";
            var year = cboYear.SelectedItem != null && cboYear.SelectedIndex >= 1 ? cboYear.SelectedItem as string : "";
            year = year.Split(' ')[1];
            var dem = 0;
            var k = new List<Task>
            {
                getEp().ContinueWith(tt=>this.Dispatcher.Invoke(()=>{
                    if(tt.Result!=null)
                    {
                        dem=int.Parse(tt.Result.data.totalItems);
                    }
                })),
                getDeXuat("",month,year,pagin.SelectedPage).ContinueWith(tt => this.Dispatcher.Invoke(() =>
                {
                    if (tt.Result != null)
                    {
                        ListEPDeXuat=ListDeXuat = tt.Result.data.items;
                        ListDeXuat.ForEach(i => i.STT = ListDeXuat.IndexOf(i) + 1);
                        ListDeXuat = ListDeXuat.ToList();
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
                            getEp("", dem).ContinueWith(zz => this.Dispatcher.Invoke(() =>
                            {
                                if (zz.Result != null)
                                {
                                    listEp = zz.Result.data.items;
                                    ListDeXuat.ForEach(i =>
                                    {
                                        var ep = listEp.Where(h => h.ep_id == i.id_user).SingleOrDefault();
                                        if (ep != null) i.dep_name = ep.dep_name;

                                        var duyet = listEp.Where(h => h.ep_id == i.id_user_duyet).SingleOrDefault();
                                        if (duyet != null) i.ng_duyet = duyet.ep_name;
                                    });
                                    ListDeXuat = ListDeXuat.ToList();
                                }
                            }));
                        });
                    }
                });
            });
        }
        private void sreachbarEP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (searchbarEP.SelectedItem == null || searchbarEP.SelectedIndex == -1)
            {
                var month = cboMonth.SelectedItem != null && cboMonth.SelectedIndex >= 1 ? cboMonth.SelectedIndex.ToString() : "";
                var year = cboYear.SelectedItem != null && cboYear.SelectedIndex >= 1 ? cboYear.SelectedItem as string : "";
                year = year.Split(' ')[1];
                getDeXuat("", month.ToString(), year, 1).ContinueWith(tt => this.Dispatcher.Invoke(() =>
                {
                    if (tt.Result != null)
                    {
                        ListEPDeXuat = ListDeXuat = tt.Result.data.items;
                        ListDeXuat.ForEach(i => i.STT = ListDeXuat.IndexOf(i) + 1);
                        ListDeXuat = ListDeXuat.ToList();
                        pagin.TotalRecords = int.Parse(tt.Result.data.totalItems);
                    }
                }));
            }
        }
        private void btnGet_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var month = cboMonth.SelectedItem != null && cboMonth.SelectedIndex >= 1 ? cboMonth.SelectedIndex.ToString() : "";
            var year = cboYear.SelectedItem != null && cboYear.SelectedIndex >= 1 ? cboYear.SelectedItem as string : "";
            year = year.Split(' ')[1];
            var ep_id = searchbarEP.SelectedItem != null && searchbarEP.SelectedIndex > -1 ? searchbarEP.SelectedItem as Employee : null;
            var dem = 0;
            string x = searchbarEP.Text.Split(' ')[0];
            var k = new List<Task>
            {
                getEp().ContinueWith(tt=>this.Dispatcher.Invoke(()=>{
                    if(tt.Result!=null)
                    {
                        dem=int.Parse(tt.Result.data.totalItems);
                    }
                })),
                getDeXuat(x,month,year,1).ContinueWith(tt => this.Dispatcher.Invoke(() =>
                {
                    if (tt.Result != null)
                    {
                        tt.Result.data.items.ForEach(i => i.STT = tt.Result.data.items.IndexOf(i) + 1);
                        ListEPDeXuat=ListDeXuat = tt.Result.data.items;
                        ListDeXuat = ListDeXuat.ToList();
                        pagin.TotalRecords=int.Parse(tt.Result.data.totalItems);
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
                            getEp("", dem).ContinueWith(zz => this.Dispatcher.Invoke(() =>
                            {
                                if (zz.Result != null)
                                {
                                    listEp = zz.Result.data.items;
                                    ListDeXuat.ForEach(i =>
                                    {
                                        var ep = listEp.Where(h => h.ep_id == i.id_user).SingleOrDefault();
                                        if (ep != null) i.dep_name = ep.dep_name;

                                        var duyet = listEp.Where(h => h.ep_id == i.id_user_duyet).SingleOrDefault();
                                        if (duyet != null) i.ng_duyet = duyet.ep_name;
                                    });
                                    ListDeXuat = ListDeXuat.ToList();
                                }
                            }));
                        });
                    }
                });
            });
        }
    }
}
