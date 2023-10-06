using QuanLyChung365TruocDangNhap.ChamCong365.Core;
using QuanLyChung365TruocDangNhap.ChamCong365.Entities;
using QuanLyChung365TruocDangNhap.ChamCong365.Entities.Company;
using QuanLyChung365TruocDangNhap.ChamCong365.Entities.History_Attenden_Staff;
using QuanLyChung365TruocDangNhap.ChamCong365.Views.PopUp.Popup_CauHinhChamCong;
using Newtonsoft.Json;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuanLyChung365TruocDangNhap.ChamCong365.Views.Pages.CauHinhChamCong
{
    /// <summary>
    /// Interaction logic for CauHinhChamCong_Main.xaml
    /// </summary>
    public partial class CauHinhChamCong_Main : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public CauHinhChamCong_Main(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this;
            tableAttStaff.AutoReponsiveColumn(2, "Display_ep_name");
            Main = main;
            getData();
            /*getEmp("", "");*/

            ckAppTimViec.IsChecked = ckQR.IsChecked = ckCTY.IsChecked = ckNV.IsChecked = ckAppPC365.IsChecked = ckAppChatQR.IsChecked = ckAppChat.IsChecked = ckIPNV.IsChecked = ckIPCTY.IsChecked = ckWifi.IsChecked = ckViTri.IsChecked = false;
            getComDetail().ContinueWith(tt => this.Dispatcher.Invoke(() =>
            {
                if (tt.Result!=null)
                {
                    ComDetail = tt.Result.data.data;

                    imgLogo1.Source = tt.Result.data.data.logo;
                    if (string.IsNullOrEmpty(tt.Result.data.data.com_logo))
                    {
                        imgLogo1.Source = new BitmapImage(new Uri("https://chamcong.timviec365.vn/images/logo_com.png"));
                    }
                    imgLogo2.Source = tt.Result.data.data.logo;

                    if (tt.Result.data.data.type_timekeeping.Contains("1")) ckAppTimViec.IsChecked = true;
                    if (tt.Result.data.data.type_timekeeping.Contains("2")) ckQR.IsChecked = true;
                    if (tt.Result.data.data.type_timekeeping.Contains("3")) ckCTY.IsChecked = true;
                    if (tt.Result.data.data.type_timekeeping.Contains("4")) ckNV.IsChecked = true;
                    if (tt.Result.data.data.type_timekeeping.Contains("5")) ckAppPC365.IsChecked = true;
                    if (tt.Result.data.data.type_timekeeping.Contains("6")) ckIPNV.IsChecked = true;
                    if (tt.Result.data.data.type_timekeeping.Contains("7")) ckIPCTY.IsChecked = true;
                    if (tt.Result.data.data.type_timekeeping.Contains("8")) ckAppChat.IsChecked = true;
                    if (tt.Result.data.data.id_way_timekeeping.Contains("2")) ckWifi.IsChecked = true;
                    if (tt.Result.data.data.id_way_timekeeping.Contains("3")) ckViTri.IsChecked = true;
                    if (tt.Result.data.data.enable_scan_qr == "1") ckAppChatQR.IsChecked = true;
                }
            }));

            tabitemCauHinh = new List<TabItem>() { tabitemWIFI, tabitemViTri, tabitemIp };
            tabitemIps = new List<TabItem>() { tabitemIpNV, tabitemIpCTY };
        }
        public CauHinhChamCong_Main(MainWindow main, int index = 0)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;
            getData();
            /*getEmp("","");*/
            imgLogo1.Source = main.APICompany.data.data.user_info.logo;
            imgLogo2.Source = main.APICompany.data.data.user_info.logo;

            TabIndexx = index;
        }
        //
        public List<TabItem> tabitemCauHinh { get; set; }
        public List<TabItem> tabitemIps { get; set; }

        private DetailCompany _ComDetail;
        public DetailCompany ComDetail
        {
            get { return _ComDetail; }
            set { _ComDetail = value;OnPropertyChanged(); }
        }


        private int _IsSmallSize = 0;
        public int IsSmallSize
        {
            get { return _IsSmallSize; }
            set { _IsSmallSize = value; OnPropertyChanged(); }
        }

        private int _TabIndex;
        public int TabIndexx
        {
            get { return _TabIndex; }
            set
            {
                if (value == 3)
                {
                    System.Diagnostics.Process.Start("https://chamcong.timviec365.vn/huong-dan.html?hd=3");
                    tab.SelectedIndex = 0;
                }
                else _TabIndex = value;
                OnPropertyChanged();
            }
        }
        public MainWindow Main { get; set; }

        private List<Wifi> _listWifi;
        public List<Wifi> listWifi
        {
            get { return _listWifi; }
            set { _listWifi = value; OnPropertyChanged(); }
        }

        private List<ToaDo> _listToaDo;
        public List<ToaDo> listToaDo
        {
            get { return _listToaDo; }
            set { _listToaDo = value; OnPropertyChanged(); }
        }

        private List<IP> _listIPEp;
        public List<IP> listIPEp
        {
            get { return _listIPEp; }
            set { _listIPEp = value; OnPropertyChanged(); }
        }

        private List<IP> _listIPCom;
        public List<IP> listIPCom
        {
            get { return _listIPCom; }
            set { _listIPCom = value; OnPropertyChanged(); }
        }

        private List<ListCor> _listQR;
        public List<ListCor> listQR
        {
            get { return _listQR; }
            set { _listQR = value; OnPropertyChanged(); }
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
        private List<Item_List_Staff_All> _listep;
        public List<Item_List_Staff_All> listep
        {
            get { return _listep; }
            set { _listep = value; OnPropertyChanged(); }
        }
        private List<ThietBiChoDuyet> _listTBCD;
        public List<ThietBiChoDuyet> listTBCD
        {
            get { return _listTBCD; }
            set { _listTBCD = value; OnPropertyChanged(); }
        }
        //
        string idDep="", idCom="";
        private int pageEP = 1;
        private int totalEP = 0;
        private void getEmp(string idDep, string idCom, int page, int length)
        {
            try
            {
                using (WebClient http = new WebClient())
                {
                    var apilink = "http://210.245.108.202:3000/api/qlc/managerUser/list";
                    var pram = new List<string>();

                    switch (Main.Type)
                    {
                        case 1:
                            http.Headers.Add("Authorization", "Bearer " + Main.APIStaff.data.data.access_token);
                            if (!string.IsNullOrEmpty(idCom)) http.QueryString.Add("com_id", idCom);
                            break;
                        case 2:
                            http.Headers.Add("Authorization", "Bearer " + Main.APICompany.data.data.access_token);
                            if (!string.IsNullOrEmpty(idCom)) http.QueryString.Add("com_id", idCom);
                            else http.QueryString.Add("com_id", Main.APICompany.data.data.user_info.com_id);

                            break;
                        default:
                            break;
                    }
                    if (!string.IsNullOrEmpty(idDep)) http.QueryString.Add("dep_id", idDep);
                    var offset = (page - 1) * 20;
                    if (page <= 0) page = 1;
                    //http.QueryString.Add("offset", offset.ToString());
                    //http.QueryString.Add("length", "10000");
                    http.QueryString.Add("pageNumber", page.ToString());
                    http.QueryString.Add("pageSize", "10000");
                    http.UploadValuesCompleted += (sender, e) =>
                    {
                        try
                        {
                            API_List_Staff_All api = JsonConvert.DeserializeObject<API_List_Staff_All>(UnicodeEncoding.UTF8.GetString(e.Result));
                            if (api.data != null)
                            {
                                if (listep == null) listep = new List<Item_List_Staff_All>();
                                listep.Clear();
                                listep.AddRange(api.data.items);
                                listep = listep.ToList();
                                searchbarEP.Refresh();
                                totalEP = int.Parse(api.data.totalItems);
                            }
                        }
                        catch
                        {

                        }
                    };
                    http.UploadValuesTaskAsync(new Uri(apilink), http.QueryString);
                }

            }
            catch (Exception)
            {
            }
        }
        private void getSevice(string idDep, string idCom, string idEp)
        {
            try
            {
                using (WebClient http = new WebClient())
                {
                    var apilink = "http://210.245.108.202:3000/api/qlc/managerUser/list";
                    var pram = new List<string>();

                    switch (Main.Type)
                    {
                        case 1:
                            http.Headers.Add("Authorization", "Bearer " + Main.APIStaff.data.data.access_token);
                            if (!string.IsNullOrEmpty(idCom)) http.QueryString.Add("com_id", idCom);
                            break;
                        case 2:
                            http.Headers.Add("Authorization", "Bearer " + Main.APICompany.data.data.access_token);
                            if (!string.IsNullOrEmpty(idCom)) http.QueryString.Add("com_id", idCom);
                            else http.QueryString.Add("com_id", Main.APICompany.data.data.user_info.com_id);

                            break;
                        default:
                            break;
                    }
                    if (!string.IsNullOrEmpty(idDep)) http.QueryString.Add("dep_id", idDep);
                    /*var offset = (page - 1) * 20;
                    //http.QueryString.Add("offset", offset.ToString());
                    //http.QueryString.Add("length", "10000");
                    http.QueryString.Add("pageNumber", page.ToString());
                    http.QueryString.Add("pageSize", "10000");*/
                    http.UploadValuesCompleted += (sender, e) =>
                    {
                        API_List_Staff_All api = JsonConvert.DeserializeObject<API_List_Staff_All>(UnicodeEncoding.UTF8.GetString(e.Result));
                        if(api.data != null)
                        {
                            if (listep == null) listep = new List<Item_List_Staff_All>();
                            listep.Clear();
                            listep.AddRange(api.data.items);
                            listep = listep.ToList();
                            searchbarEP.Refresh();
                            totalEP = int.Parse(api.data.totalItems);
                        }
                    };
                    http.UploadValuesTaskAsync(new Uri(apilink), http.QueryString);
                }

            }
            catch (Exception)
            {
            }
        }
        private void sreachbarEP_ScrollToEnd()
        {
            /*if (listep.Count + (pageEP + 1 * 20) <= totalEP)
            {
                pageEP++;
                getEmp(idDep,idCom,pageEP, 20);
            }*/
        }

        private void getTBCD(string idCom = "", string idDep = "", string idEp = "")
        {
            try
            {
                using (WebClient http = new WebClient())
                {
                    var apilink = "http://210.245.108.202:3000/api/qlc/checkdevice/list";
                    var pram = new List<string>();

                    switch (Main.Type)
                    {
                        case 1:
                            http.Headers.Add("Authorization", "Bearer " + Main.APIStaff.data.data.access_token);
                            if (!string.IsNullOrEmpty(idCom)) http.QueryString.Add("com_id", idCom);
                            else http.QueryString.Add("com_id", Main.APIStaff.data.data.user_info.com_id);
                            break;
                        case 2:
                            http.Headers.Add("Authorization", "Bearer " + Main.APICompany.data.data.access_token);
                            if (!string.IsNullOrEmpty(idCom)) http.QueryString.Add("com_id", idCom);
                            else http.QueryString.Add("com_id", Main.APICompany.data.data.user_info.com_id);
                            break;
                        default:
                            break;
                    }
                    //http.QueryString.Add("filter_by[browse]", "true");
                    //http.QueryString.Add("get_all", "true");
                    if (!string.IsNullOrEmpty(idDep)) http.QueryString.Add("dep_id", idDep);
                    if (!string.IsNullOrEmpty(idEp)) http.QueryString.Add("ep_id", idEp);
                    http.UploadValuesCompleted += (sender, e) =>
                    {
                        try
                        {
                            API_ThietBiChoDuyet api = JsonConvert.DeserializeObject<API_ThietBiChoDuyet>(UnicodeEncoding.UTF8.GetString(e.Result));
                            if (api.data != null)
                            {
                                if (listTBCD == null) listTBCD = new List<ThietBiChoDuyet>();
                                listTBCD.Clear();
                                listTBCD.AddRange(api.data.items);
                                listTBCD = listTBCD.ToList();
                            }
                        }
                        catch
                        {

                        }
                    };
                    http.UploadValuesTaskAsync(new Uri(apilink), http.QueryString);
                }

            }
            catch (Exception)
            {
            }
        }

        private async Task<API_Com_ChiTiet> getComDetail()
        {
            try
            {
                string apilink = "http://210.245.108.202:3000/api/qlc/company/info";
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
                var check = respon.Content.ReadAsStringAsync().Result;
                API_Com_ChiTiet api = JsonConvert.DeserializeObject<API_Com_ChiTiet>(respon.Content.ReadAsStringAsync().Result);
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
        private async Task<List<Wifi>> loadWIfi()
        {
            try
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
                var apilink = "http://210.245.108.202:3000/api/qlc/trackingWifi/list";
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("companyID", idcom);
                HttpResponseMessage respon = await http.PostAsync(apilink, new FormUrlEncodedContent(data));
                var check = respon.Content.ReadAsStringAsync().Result;
                API_Wifi_List api = JsonConvert.DeserializeObject<API_Wifi_List>(respon.Content.ReadAsStringAsync().Result);
                if (api.data != null && api.data.items != null)
                {
                    return api.data.items;
                }
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
        private async Task<List<ToaDo>> loadToaDo()
        {
            try
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
                var apilink = "http://210.245.108.202:3000/api/qlc/company_coordinate/list";
                HttpResponseMessage respon = await http.PostAsync(apilink, new FormUrlEncodedContent(new Dictionary<string, string>()));
                API_ToaDo_List api = JsonConvert.DeserializeObject<API_ToaDo_List>(respon.Content.ReadAsStringAsync().Result);
                if (api.data != null && api.data.items != null)
                {
                    return api.data.items;
                }
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
        private async Task<API_IP_List> getIP(int page = 1)
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
                var apilink = "http://210.245.108.202:3000/api/qlc/company_web_ip/list";
                var pram = new List<string>();
                if (page <= 0) page = 1;
                var offset = (page - 1) * 20;
                pram.Add("off_set=" + offset.ToString());

                if (pram.Count > 0)
                {
                    apilink += "?" + string.Join("&", pram);
                }
                Dictionary<string,string> form = new Dictionary<string,string>();
                HttpResponseMessage respon = await http.PostAsync(apilink, new FormUrlEncodedContent(form));
                var check = respon.Content.ReadAsStringAsync().Result;
                API_IP_List api = JsonConvert.DeserializeObject<API_IP_List>(respon.Content.ReadAsStringAsync().Result);
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
        private async Task<API_QR_List> getQR(int page = 1)
        {
            try
            {
                HttpClient http = new HttpClient();
                var apilink = "http://210.245.108.202:3000/api/qlc/trackingQR/get_config_timekeeping_qr";
                var pram = new List<string>();

                switch (Main.Type)
                {
                    case 1:
                        http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Main.APIStaff.data.data.access_token);
                        //pram.Add("id_com="+Main.APIStaff.data.data.user_info.com_id);
                        break;
                    case 2:
                        http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Main.APICompany.data.data.access_token);
                        break;
                    default:
                        break;
                }
                /*if (page <= 0) page = 1;
                var offset = (page - 1) * 20;
                pram.Add("off_set=" + offset.ToString());

                if (pram.Count > 0)
                {
                    apilink += "?" + string.Join("&", pram);
                }*/
                Dictionary<string, string> form = new Dictionary<string, string>();
                HttpResponseMessage respon = await http.PostAsync(apilink, new FormUrlEncodedContent(form));
                API_QR_List api = JsonConvert.DeserializeObject<API_QR_List>(respon.Content.ReadAsStringAsync().Result);
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
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Main.APIStaff.data.data.access_token);
                        break;
                    case 2:
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Main.APICompany.data.data.access_token);
                        break;
                    default:
                        break;
                }
                Dictionary<string, string> form = new Dictionary<string, string>();
                form.Add("com_id", idcom);
                int i = 0;
                List<Item_List_PhongBan> list = new List<Item_List_PhongBan>();
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
        private async Task getData()
        {
            loadWIfi().ContinueWith(tt => this.Dispatcher.Invoke(() => listWifi = tt.Result));
            loadToaDo().ContinueWith(tt => this.Dispatcher.Invoke(() => listToaDo = tt.Result));
            getQR().ContinueWith(tt => this.Dispatcher.Invoke(() =>
            {
                if (tt.Result != null)
                {
                    listQR = tt.Result.data.config_timekeeping.list_cor;
                }
            }));
            getIP().ContinueWith(tt => this.Dispatcher.Invoke(() =>
            {
                if (tt.Result != null)
                {
                    listIPCom = new List<IP>();
                    listIPEp = new List<IP>();
                    tt.Result.data.items.ForEach(i =>
                    {
                        if (i.type == "1") listIPEp.Add(i);
                        if (i.type == "2") listIPCom.Add(i);
                    });
                    listIPEp = listIPEp.ToList();
                    listIPCom = listIPCom.ToList();
                }
            }));
            getChildCom().ContinueWith(tt => this.Dispatcher.Invoke(() =>
            {
                listCom = tt.Result;
                //listCom.Insert(0, new ChildCompany() { com_id = "-1", com_name = "Tất cả công ty" });
                listCom = listCom.ToList();
                cbbCom.SelectedIndex = listCom.Count - 1;
            }));
            getEmp("","",1,20);
            getTBCD();
        }
        //
        private void DownLoadPC365(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://chamcong.timviec365.com/");
        }
        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.ActualWidth <= 768)
            {
                DockPanel.SetDock(border1, Dock.Top);
                DockPanel.SetDock(border2, Dock.Top);
                border1.Margin = new Thickness(0, 0, 0, 10);
                border2.Margin = new Thickness(0, 0, 0, 10);
            }
            else
            {
                DockPanel.SetDock(border1, Dock.Left);
                border1.Margin = new Thickness(0, 0, 10, 0);

                DockPanel.SetDock(border2, Dock.Left);
                border2.Margin = new Thickness(0, 0, 10, 0);
            }

            if (this.ActualWidth > 768)
            {
                IsSmallSize = 0;
            }
            else IsSmallSize = 1;
        }
        //wifi
        private async void WifiMacDinh(object sender, MouseButtonEventArgs e)
        {
            var x = (sender as Border).Tag as Wifi;
            if (x != null)
            {
                try
                {
                    Dictionary<string, string> form = new Dictionary<string, string>();
                    form.Add("wifi_id", x.wifi_id);

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
                    var respon = await httpClient.PostAsync("http://210.245.108.202:3000/api/qlc/TrackingWifi/set_company_wifi_default", new FormUrlEncodedContent(form));
                    API_Respon add = JsonConvert.DeserializeObject<API_Respon>(respon.Content.ReadAsStringAsync().Result);
                    if (add.data != null && add.data.result)
                    {
                        var pop = new PopUp.Notify1();
                        pop.Type = PopUp.Notify1.NotifyType.Success;
                        pop.Message = "Thêm mới wifi thành công";
                        Main.ShowPopup(pop);

                        var z = listWifi.Find(i => i.wifi_id == x.wifi_id);
                        listWifi.ForEach(i => i.is_default = "0");
                        z.is_default = "1";
                        listWifi = listWifi.ToList();
                    }
                    else
                    {
                        var pop = new PopUp.Notify1();
                        pop.Type = PopUp.Notify1.NotifyType.Error;
                        pop.Message = "Thêm mới wifi thất bại, vui lòng thử lại sau";
                        Main.ShowPopup(pop);
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
        private void AddWifi(object sender, MouseButtonEventArgs e)
        {
            var pop = new CauHinhChamCong.CauHinhChamCong_Wifi_EditPopup(Main);
            Main.ShowPopup(pop);

            pop.Success = () =>
            {
                loadWIfi().ContinueWith(tt => this.Dispatcher.Invoke(() => listWifi = tt.Result));
            };
        }
        private void EditWifi(object sender, MouseButtonEventArgs e)
        {
            var x = (sender as Border).Tag as Wifi;
            if (x != null)
            {
                var pop = new CauHinhChamCong.CauHinhChamCong_Wifi_EditPopup(Main, x);
                Main.ShowPopup(pop);
                pop.Success = () =>
                {
                    var w = listWifi.Find(i => i.wifi_id == x.wifi_id);
                    w.name_wifi = pop.Info.name_wifi;
                    w.mac_address = pop.Info.mac_address;
                    listWifi = listWifi.ToList();
                };
            }
        }
        private void DeleteWifi(object sender, MouseButtonEventArgs e)
        {
            var x = new PopUp.Noti_Delete(Main);
            x.Message = App.Current.Resources["text_Wifi_DeleteQues"] as string;
            Main.ShowPopup(x);
            x.Accept = async () =>
            {
                //try
                //{
                //    Dictionary<string, string> form = new Dictionary<string, string>();
                //    form.Add("ghi_chu", "");
                //    HttpClient httpClient = new HttpClient();
                //switch (Main.Type)
                //{
                //    case 1:
                //        http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Main.APIStaff.data.data.access_token);
                //        break;
                //    case 2:
                //        http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Main.APICompany.data.data.access_token);
                //        break;
                //    default:
                //        break;
                //}
                //    var respon = await httpClient.PostAsync("https://chamcong.24hpay.vn/service/delete_company_wifi.php", new FormUrlEncodedContent(form));
                //    API_Respon add = JsonConvert.DeserializeObject<API_Respon>(respon.Content.ReadAsStringAsync().Result);
                //    if (add.data != null && add.data.result)
                //    {
                //        MessageBox.Show("thanh cong");
                //    }
                //    else
                //    {
                //        MessageBox.Show("loi");
                //    }
                //}
                //catch (Exception ex)
                //{
                //    MessageBox.Show(ex.Message);
                //}
            };
        }
        //vi tri
        private void AddViTri(object sender, MouseButtonEventArgs e)
        {
            var x = new CauHinhChamCong_ViTri_EditPopup(Main);
            Main.ShowPopup(x);

            x.Success = () =>
            {
                loadToaDo().ContinueWith(tt => this.Dispatcher.Invoke(() => listToaDo = tt.Result));
            };
        }
        private void DeleteViTri(object sender, MouseButtonEventArgs e)
        {
            var x = (sender as Border).Tag as ToaDo;
            if (x != null)
            {
                var p = new PopUp.Noti_Delete(Main);
                p.Message = App.Current.Resources["text_DeleteQues"] as string;
                Main.ShowPopup(p);

                p.Accept = async () =>
                {
                    try
                    {
                        Dictionary<string, string> form = new Dictionary<string, string>();
                        form.Add("cor_id", x.cor_id);

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
                        var respon = await httpClient.PostAsync("http://210.245.108.202:3000/api/qlc/company_coordinate/delete", new FormUrlEncodedContent(form));
                        API_Respon add = JsonConvert.DeserializeObject<API_Respon>(respon.Content.ReadAsStringAsync().Result);
                        if (add.data != null && add.data.result)
                        {
                            var pop = new PopUp.Notify1();
                            pop.Type = PopUp.Notify1.NotifyType.Success;
                            pop.Message = "Xóa tọa độ thành công";
                            Main.ShowPopup(pop);

                            listToaDo.RemoveAll(i => i.cor_id == x.cor_id);
                            listToaDo = listToaDo.ToList();
                        }
                        else
                        {
                            var pop = new PopUp.Notify1();
                            pop.Type = PopUp.Notify1.NotifyType.Error;
                            pop.Message = "Xóa tọa độ thất bại, vui lòng thử lại sau";
                            Main.ShowPopup(pop);
                        }
                    }
                    catch (Exception ex)
                    {
                        var pop = new PopUp.Notify1();
                        pop.Type = PopUp.Notify1.NotifyType.Error;
                        pop.Message = ex.Message;
                        Main.ShowPopup(pop);
                    }
                };
            }
        }
        private async void ToaDoMacDinh(object sender, MouseButtonEventArgs e)
        {
            var x = (sender as Border).Tag as ToaDo;
            if (x != null)
            {
                try
                {
                    Dictionary<string, string> form = new Dictionary<string, string>();
                    form.Add("cor_id", x.cor_id);

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
                    var respon = await httpClient.PostAsync("http://210.245.108.202:3000/api/qlc/company_coordinate/set_coordinate_default", new FormUrlEncodedContent(form));
                    API_Respon add = JsonConvert.DeserializeObject<API_Respon>(respon.Content.ReadAsStringAsync().Result);
                    if (add.data != null && add.data.result)
                    {

                        var p = new PopUp.Notify1();
                        p.Type = PopUp.Notify1.NotifyType.Success;
                        p.Message = "Cập nhật tọa độ mặc định thành công";
                        Main.ShowPopup(p);

                        var z = listToaDo.Find(i => i.cor_id == x.cor_id);
                        listToaDo.ForEach(i => i.is_default = "0");
                        z.is_default = "1";
                        listToaDo = listToaDo.ToList();
                    }
                    else
                    {
                        var p = new PopUp.Notify1();
                        p.Type = PopUp.Notify1.NotifyType.Error;
                        p.Message = "Cập nhật tọa độ mặc định thất bại, vui lòng thử lại sau";
                        Main.ShowPopup(p);
                    }
                }
                catch (Exception ex)
                {
                    var p = new PopUp.Notify1();
                    p.Type = PopUp.Notify1.NotifyType.Error;
                    p.Message = ex.Message;
                    Main.ShowPopup(p);
                }
            }
        }
        // ip
        private void AddIpNhanVien(object sender, MouseButtonEventArgs e)
        {
            var x = new CauHinhChamCong_IP_EditPopup(Main);
            x.IpType = 1;
            Main.ShowPopup(x);

            x.Success = () =>
            {
                getIP().ContinueWith(tt => this.Dispatcher.Invoke(() =>
                {
                    if (tt.Result != null)
                    {
                        listIPCom = new List<IP>();
                        listIPEp = new List<IP>();
                        tt.Result.data.items.ForEach(i =>
                        {
                            if (i.type == "1") listIPEp.Add(i);
                            if (i.type == "2") listIPCom.Add(i);
                        });
                        listIPEp = listIPEp.ToList();
                        listIPCom = listIPCom.ToList();
                    }
                }));
            };
        }
        private void EditIPNhanVien(object sender, MouseButtonEventArgs e)
        {
            IP z = (sender as Border).Tag as IP;
            if (z != null)
            {
                var x = new CauHinhChamCong_IP_EditPopup(Main, z);
                x.IpType = 1;
                Main.ShowPopup(x);

                x.Success = () =>
                {
                    var k = listIPEp.Find(b => b.ip_id == z.ip_id);
                    if (k != null)
                    {
                        k.name_ip = x.Info.name_ip;
                        k.ip_address = x.Info.ip_address;
                    }
                    listIPEp = listIPEp.ToList();
                };
            }

        }
        private void DeleteIPNhanVien(object sender, MouseButtonEventArgs e)
        {
            IP z = (sender as Border).Tag as IP;
            var x = new PopUp.Noti_Delete(Main);
            x.Message = App.Current.Resources["text_DeleteQues"] as string;
            Main.ShowPopup(x);

            x.Accept = async () =>
            {
                try
                {
                    Dictionary<string, string> form = new Dictionary<string, string>();
                    form.Add("id_acc", z.ip_id);
                    HttpClient httpClient = new HttpClient();
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
                    HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Delete;
                    httpRequestMessage.Content = new FormUrlEncodedContent(form);
                    httpRequestMessage.RequestUri = new Uri("http://210.245.108.202:3000/api/qlc/SetIp/delete");
                    var respon = await httpClient.SendAsync(httpRequestMessage);
                    var check = respon.Content.ReadAsStringAsync().Result;
                    API_Respon add = JsonConvert.DeserializeObject<API_Respon>(respon.Content.ReadAsStringAsync().Result);
                    if (add.data != null && add.data.result)
                    {
                        var pop = new PopUp.Notify1();
                        pop.Type = PopUp.Notify1.NotifyType.Success;
                        pop.Message = "Xóa IP mạng thành công";
                        Main.ShowPopup(pop);
                        listIPEp.RemoveAll(k => k.ip_id == z.ip_id);
                        listIPEp = listIPEp.ToList();
                    }
                    else
                    {
                        var pop = new PopUp.Notify1();
                        pop.Type = PopUp.Notify1.NotifyType.Error;
                        pop.Message = "Xóa IP mạng thất bại, vui lòng thử lại sau";
                        Main.ShowPopup(pop);
                    }
                }
                catch (Exception ex)
                {
                    var pop = new PopUp.Notify1();
                    pop.Type = PopUp.Notify1.NotifyType.Error;
                    pop.Message = ex.Message;
                    Main.ShowPopup(pop);
                }
            };
        }
        private void AddIpCongTy(object sender, MouseButtonEventArgs e)
        {
            var x = new CauHinhChamCong_IP_EditPopup(Main);
            x.IpType = 2;
            Main.ShowPopup(x);

            x.Success = () =>
            {
                getIP().ContinueWith(tt => this.Dispatcher.Invoke(() =>
                {
                    if (tt.Result != null)
                    {
                        listIPCom = new List<IP>();
                        listIPEp = new List<IP>();
                        tt.Result.data.items.ForEach(i =>
                        {
                            if (i.type == "1") listIPEp.Add(i);
                            if (i.type == "2") listIPCom.Add(i);
                        });
                        listIPEp = listIPEp.ToList();
                        listIPCom = listIPCom.ToList();
                    }
                }));
            };
        }
        private void EditIpCongTy(object sender, MouseButtonEventArgs e)
        {
            IP z = (sender as Border).Tag as IP;
            if (z != null)
            {
                var x = new CauHinhChamCong_IP_EditPopup(Main, z);
                x.IpType = 2;
                Main.ShowPopup(x);

                x.Success = () =>
                {
                    var k = listIPCom.Find(b => b.ip_id == z.ip_id);
                    if (k != null)
                    {
                        k.name_ip = x.Info.name_ip;
                        k.ip_address = x.Info.ip_address;
                    }
                    listIPCom = listIPCom.ToList();
                };
            }
        }
        private void DeleteIpCongTy(object sender, MouseButtonEventArgs e)
        {
            IP z = (sender as Border).Tag as IP;
            var x = new PopUp.Noti_Delete(Main);
            x.Message = App.Current.Resources["text_DeleteQues"] as string;
            Main.ShowPopup(x);

            x.Accept = async () =>
            {
                try
                {
                    Dictionary<string, string> form = new Dictionary<string, string>();
                    form.Add("id_acc", z.ip_id);
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
                    HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Delete;
                    httpRequestMessage.Content = new FormUrlEncodedContent(form);
                    httpRequestMessage.RequestUri = new Uri("http://210.245.108.202:3000/api/qlc/SetIp/delete");
                    var respon = await httpClient.SendAsync(httpRequestMessage);
                    var check = respon.Content.ReadAsStringAsync().Result;
                    API_Respon add = JsonConvert.DeserializeObject<API_Respon>(respon.Content.ReadAsStringAsync().Result);
                    if (add.data != null && add.data.result)
                    {
                        var pop = new PopUp.Notify1();
                        pop.Type = PopUp.Notify1.NotifyType.Success;
                        pop.Message = "Xóa IP mạng thành công";
                        Main.ShowPopup(pop);
                        listIPCom.RemoveAll(k => k.ip_id == z.ip_id);
                        listIPCom = listIPCom.ToList();
                    }
                    else
                    {
                        var pop = new PopUp.Notify1();
                        pop.Type = PopUp.Notify1.NotifyType.Error;
                        pop.Message = "Xóa IP mạng thất bại, vui lòng thử lại sau";
                        Main.ShowPopup(pop);
                    }
                }
                catch (Exception ex)
                {
                    var pop = new PopUp.Notify1();
                    pop.Type = PopUp.Notify1.NotifyType.Error;
                    pop.Message = ex.Message;
                    Main.ShowPopup(pop);
                }
            };
        }
        //cau hinh
        private async Task CauHinhChamCong(int k, int type = 1)
        {
            try
            {
                if (Main != null)
                    if (!string.IsNullOrEmpty(ComDetail.type_timekeeping))
                    {
                        List<string> x = new List<string>();
                        if (ComDetail.type_timekeeping.Contains(",")) x = ComDetail.type_timekeeping.Split(',').ToList();
                        else x.Add(ComDetail.type_timekeeping);
                        if (type == 1)
                        {
                            if (!x.Contains(k.ToString())) x.Add(k.ToString());
                        }
                        else if (type == 2)
                        {
                            if (x.Contains(k.ToString())) x.RemoveAll(m => m == k.ToString());
                        }
                        Dictionary<string, string> form = new Dictionary<string, string>();
                        if (x.Count > 0) form.Add("type_timekeeping", string.Join(",", x));
                        else form.Add("type_timekeeping", "\"\"");
                        HttpClient httpClient = new HttpClient();
                        switch (Main.Type)
                        {
                            case 1:
                                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Main.APIStaff.data.data.access_token);
                                //form.Add("id_com" ,Main.APIStaff.data.data.user_info.com_id);
                                break;
                            case 2:
                                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Main.APICompany.data.data.access_token);
                                break;
                            default:
                                break;
                        }
                        var respon = await httpClient.PostAsync("http://210.245.108.202:3000/api/qlc/company/update_type_timekeeping", new FormUrlEncodedContent(form));
                        API_Respon add = JsonConvert.DeserializeObject<API_Respon>(respon.Content.ReadAsStringAsync().Result);
                        if (add.data != null && add.data.result)
                        {
                            var n = new PopUp.Notify1();
                            n.Type = PopUp.Notify1.NotifyType.Success;
                            n.setMessage("Cập nhật cấu hình chấm công thành công");
                            ComDetail.type_timekeeping = string.Join(",", x);
                            Main.ShowPopup(n);
                        }
                        else
                        {
                            var n = new PopUp.Notify1();
                            n.Type = PopUp.Notify1.NotifyType.Error;
                            n.setMessage("Cập nhật cấu hình chấm công thất bại, vui lòng thử lại sau");
                            Main.ShowPopup(n);
                        }
                    }

            }
            catch (Exception ex)
            {
                var n = new PopUp.Notify1();
                n.Type = PopUp.Notify1.NotifyType.Error;
                n.setMessage(ex.Message);
                Main.ShowPopup(n);
            }
        }
        private async Task CauHinhChamCong1(int k, int type = 1)
        {
            try
            {
                if (Main != null)
                    if (!string.IsNullOrEmpty(ComDetail.id_way_timekeeping))
                    {
                        List<string> x = new List<string>();
                        if (ComDetail.id_way_timekeeping.Contains(",")) x = ComDetail.id_way_timekeeping.Split(',').ToList();
                        else x.Add(ComDetail.id_way_timekeeping);
                        if (type == 1)
                        {
                            if (!x.Contains(k.ToString())) x.Add(k.ToString());
                        }
                        else if (type == 2)
                        {
                            if (x.Contains(k.ToString())) x.RemoveAll(m => m == k.ToString());
                        }
                        Dictionary<string, string> form = new Dictionary<string, string>();
                        if (x.Count > 0) form.Add("lst_way_id", string.Join(",", x));
                        else form.Add("lst_way_id", "\"\"");
                        HttpClient httpClient = new HttpClient();
                        switch (Main.Type)
                        {
                            case 1:
                                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Main.APIStaff.data.data.access_token);
                                form.Add("id_com" ,Main.APIStaff.data.data.user_info.com_id);
                                break;
                            case 2:
                                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Main.APICompany.data.data.access_token);
                                break;
                            default:
                                break;
                        }
                        var respon = await httpClient.PostAsync("http://210.245.108.202:3000/api/qlc/company/update_way_timekeeping", new FormUrlEncodedContent(form));
                        API_Respon add = JsonConvert.DeserializeObject<API_Respon>(respon.Content.ReadAsStringAsync().Result);
                        if (add.data != null && add.data.result)
                        {
                            var n = new PopUp.Notify1();
                            n.Type = PopUp.Notify1.NotifyType.Success;
                            n.setMessage("Cập nhật cấu hình chấm công thành công");
                            ComDetail.id_way_timekeeping = string.Join(",", x);
                            Main.ShowPopup(n);
                        }
                        else
                        {
                            var n = new PopUp.Notify1();
                            n.Type = PopUp.Notify1.NotifyType.Error;
                            n.setMessage("Cập nhật cấu hình chấm công thất bại, vui lòng thử lại sau");
                            Main.ShowPopup(n);
                        }
                    }

            }
            catch (Exception ex)
            {
                var n = new PopUp.Notify1();
                n.Type = PopUp.Notify1.NotifyType.Error;
                n.setMessage(ex.Message);
                Main.ShowPopup(n);
            }
        }
        private void ckWifi_Unchecked(object sender, RoutedEventArgs e)
        {
            if (tabitemCauHinh != null)
                foreach (var item in tabitemCauHinh)
                {
                    if (item.Visibility == Visibility.Visible && item != tabitemWIFI)
                    {
                        tabCauHinh.SelectedIndex = tabitemCauHinh.IndexOf(item);
                        if (item == tabitemIp)
                        {
                            if (tabitemIps != null)
                                foreach (var item1 in tabitemIps)
                                {
                                    if (item1.Visibility == Visibility.Visible && item1 != tabitemIpNV)
                                    {
                                        tabIp.SelectedIndex = tabitemIps.IndexOf(item1);
                                        break;
                                    }
                                }
                        }
                        break;
                    }
                }
        }
        private void ckViTri_Unchecked(object sender, RoutedEventArgs e)
        {
            if (tabitemCauHinh != null)
                foreach (var item in tabitemCauHinh)
                {
                    if (item.Visibility == Visibility.Visible && item != tabitemViTri)
                    {
                        tabCauHinh.SelectedIndex = tabitemCauHinh.IndexOf(item);
                        if (item == tabitemIp)
                        {
                            if (tabitemIps != null)
                                foreach (var item1 in tabitemIps)
                                {
                                    if (item1.Visibility == Visibility.Visible && item1 != tabitemIpNV)
                                    {
                                        tabIp.SelectedIndex = tabitemIps.IndexOf(item1);
                                        break;
                                    }
                                }
                        }
                        break;
                    }
                }
        }
        private void ckIPNV_Unchecked(object sender, RoutedEventArgs e)
        {
            if (tabitemIps != null)
                foreach (var item in tabitemIps)
                {
                    if (item.Visibility == Visibility.Visible && item != tabitemIpNV)
                    {
                        tabIp.SelectedIndex = tabitemIps.IndexOf(item);
                        break;
                    }
                }
        }
        private void ckIPCTY_Unchecked(object sender, RoutedEventArgs e)
        {
            if (tabitemIps != null)
                foreach (var item in tabitemIps)
                {
                    if (item.Visibility == Visibility.Visible && item != tabitemIpCTY)
                    {
                        tabIp.SelectedIndex = tabitemIps.IndexOf(item);
                        break;
                    }
                }
        }
        private void ckAppTimViec_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ckAppTimViec.IsChecked == true)
            {
                CauHinhChamCong(1, 2);
            }
            else if (ckAppTimViec.IsChecked == false)
            {
                CauHinhChamCong(1, 1);
            }
            ckAppTimViec.IsChecked = ckAppTimViec.IsChecked == true?false:true;
        }
        private void ckAppPC365_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ckAppPC365.IsChecked == true)
            {
                CauHinhChamCong(5, 2);
            }
            else if (ckAppPC365.IsChecked == false)
            {
                CauHinhChamCong(5, 1);
            }
        }
        private void ckWifi_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ckWifi.IsChecked == true)
            {
                CauHinhChamCong1(2, 2);
            }
            else if (ckWifi.IsChecked == false)
            {
                CauHinhChamCong1(2, 1);
            }
        }
        private void ckViTri_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ckViTri.IsChecked == true)
            {
                CauHinhChamCong1(3, 2);
            }
            else if (ckViTri.IsChecked == false)
            {
                CauHinhChamCong1(3, 1);
            }
        }
        private void ckNV_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ckNV.IsChecked == true)
            {
                CauHinhChamCong(4, 2);
            }
            else if (ckNV.IsChecked == false)
            {
                CauHinhChamCong(4, 1);
            }
        }
        private void ckIPNV_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ckIPNV.IsChecked == true)
            {
                CauHinhChamCong(6, 2);
            }
            else if (ckIPNV.IsChecked == false)
            {
                CauHinhChamCong(6, 1);
            }
        }
        private void ckCTY_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ckCTY.IsChecked == true)
            {
                CauHinhChamCong(3, 2);
            }
            else if (ckCTY.IsChecked == false)
            {
                CauHinhChamCong(3, 1);
            }
        }
        private void ckIPCTY_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ckIPCTY.IsChecked == true)
            {
                CauHinhChamCong(7, 2);
            }
            else if (ckIPCTY.IsChecked == false)
            {
                CauHinhChamCong(7, 1);
            }
        }
        private void ckChat_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ckAppChat.IsChecked == true)
            {
                CauHinhChamCong(8, 2);
            }
            else if (ckAppChat.IsChecked == false)
            {
                CauHinhChamCong(8, 1);
            }
            ckAppChat.IsChecked = ckAppChat.IsChecked == true ? false : true;
        }
        private async void ckQR_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ckQR.IsChecked == true)
            {
                CauHinhChamCong(2, 2);
            }
            else if (ckQR.IsChecked == false)
            {
                CauHinhChamCong(2, 1);
            }
        }
        private void QRPopup(object sender, MouseButtonEventArgs e)
        {
            var z = (sender as Border).Tag as ListCor;
            if (z != null)
            {
                var x = new CauHinhChamCong_QR_Popup(Main, z);
                Main.ShowPopup(x);

                x.DelSuccess = () =>
                {
                    listQR.RemoveAll(k => k.cor_id == z.cor_id);
                    listQR = listQR.ToList();
                };
            }
        }

        private void cbbCom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var z = cbbCom.SelectedItem as ChildCompany;
            if (z != null)
            {
                getDep(z.com_id).ContinueWith(tt => this.Dispatcher.Invoke(() =>
                {
                    listDep = tt.Result;
                    cbbPb.SelectedIndex = 0;
                    /*listDep.Insert(0, new Item_List_PhongBan() { dep_id = "-1", dep_name = "Tất cả phòng ban" });
                    listDep = listDep.ToList();*/
                }));
            }
        }

        private void cbbDep_selecttionChanged(object sender, SelectionChangedEventArgs e)
        {
            var z = cbbCom.SelectedItem as ChildCompany;
            Item_List_PhongBan pb = cbbPb.SelectedItem as Item_List_PhongBan;
            if(pb != null)
            {
                if(pb.dep_id != "-1")
                {
                    getEmp(pb.dep_id, z.com_id, 0, 0);
                }
                else
                {
                    getEmp("", z.com_id, 0, 0);
                }
            }
        }

        private void LocDS(object sender, MouseButtonEventArgs e)
        {
            string idcom = "", iddep ="", idep = "";
            if(cbbCom.SelectedIndex >= 0)
            {
                var com = (ChildCompany)cbbCom.SelectedItem;
                idCom = com.com_id;
            }
            if(cbbPb.SelectedIndex >= 1)
            {
                var dep = (Item_List_PhongBan)cbbPb.SelectedItem;
                iddep = dep.dep_id;
            }
            if(searchbarEP.SelectedIndex >= 0)
            {
                var ep = (Item_List_Staff_All)searchbarEP.SelectedItem;
                string x = searchbarEP.Text.ToString();
                idep = x.Split(' ')[0];
            }
            getTBCD(idcom, iddep, idep);
        }
        private List<string> ListIDSelected = new List<string>();
        private bool _IsCheckAll = false;

        public bool IsCheckAll
        {
            get { return _IsCheckAll; }
            set { _IsCheckAll = value; OnPropertyChanged(); }
        }
        
        private void CheckBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Border cb = sender as Border;
            ThietBiChoDuyet tb = (ThietBiChoDuyet)cb.DataContext;
            if(tb.isCheck == false)
            {
                ListIDSelected.Add(tb.ed_id);
                tb.isCheck = true;
                if (ListIDSelected.Count == listTBCD.Count)
                {
                    IsCheckAll = true;
                }
            }
            else
            {
                tb.isCheck = false;
                ListIDSelected.Remove(tb.ed_id);
                IsCheckAll = false;
            }
            
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(IsCheckAll == true)
            {
                ListIDSelected.Clear();
                foreach(var item in listTBCD)
                {
                    item.isCheck = false;
                }
                IsCheckAll = false;
            }
            else
            {
                ListIDSelected.Clear();
                foreach (var item in listTBCD)
                {
                    item.isCheck = true;
                    ListIDSelected.Add(item.ed_id);
                }
                IsCheckAll = true;
            }
        }

        private void DuyetThietBi(object sender, MouseButtonEventArgs e)
        {
            try
            {
                foreach(var item in ListIDSelected)
                {
                    using (WebClient http = new WebClient())
                    {
                        var apilink = "http://210.245.108.202:3000/api/qlc/checkdevice/add";
                        var pram = new List<string>();

                        switch (Main.Type)
                        {
                            case 1:
                                http.Headers.Add("Authorization", "Bearer " + Main.APIStaff.data.data.access_token);
                                break;
                            case 2:
                                http.Headers.Add("Authorization", "Bearer " + Main.APICompany.data.data.access_token);
                                break;
                            default:
                                break;
                        }
                        http.QueryString.Add("ed_id", item);
                        //http.QueryString.Add("type", "1");
                        http.UploadValuesCompleted += (sender1, e1) =>
                        {
                            popup.Visibility = Visibility.Visible;
                            int index = listTBCD.FindIndex(i => i.ed_id == item);
                            listTBCD.RemoveAt(index);
                            listTBCD = listTBCD.ToList();
                        };
                        http.UploadValuesTaskAsync(new Uri(apilink), http.QueryString);
                    }
                }

            }
            catch (Exception)
            {
            }
        }

        private void XacNhan(object sender, MouseButtonEventArgs e)
        {
            popup.Visibility = Visibility.Hidden;
        }
        private void XacNhan1(object sender, MouseButtonEventArgs e)
        {
            popup2.Visibility = Visibility.Hidden;
        }

        private void DockPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DockPanel dp = sender as DockPanel;
            ThietBiChoDuyet tb = (ThietBiChoDuyet)dp.DataContext;
            var pop = new TuyChonDuyetTB(Main, this, tb);
            popup1.Children.Add(pop);
            var position = Mouse.GetPosition(popup1);
            pop.Margin = new Thickness(position.X - 120, position.Y, 0, 0);
            popup1.Visibility = Visibility.Visible;
        }

        private void ClosePopup(object sender, MouseButtonEventArgs e)
        {
            popup1.Children.Clear();
            popup1.Visibility = Visibility.Hidden;
        }

        private void ckChatQR(object sender, MouseButtonEventArgs e)
        {
            using (WebClient httpClient = new WebClient())
            {
                try
                {
                    httpClient.Headers.Add("Authorization", "Bearer " + Main.APICompany.data.data.access_token);
                    if(ckAppChatQR.IsChecked == true)
                        httpClient.QueryString.Add("enable_scan_qr", "0");
                    else
                        httpClient.QueryString.Add("enable_scan_qr", "1");
                    httpClient.UploadValuesCompleted += (sender1, e1) =>
                    {
                        var n = new PopUp.Notify1();
                        n.Type = PopUp.Notify1.NotifyType.Success;
                        n.setMessage("Cập nhật cấu hình chấm công thành công");
                        Main.ShowPopup(n);
                        ckAppChatQR.IsChecked = ckAppChatQR.IsChecked == true ? false : true;
                    };
                    httpClient.UploadValuesTaskAsync(new Uri("http://210.245.108.202:3000/api/qlc/trackingQR/update_enable_qr"), "POST", httpClient.QueryString);
                }
                catch (Exception ex)
                {
                }
            }
        }
    }
}
