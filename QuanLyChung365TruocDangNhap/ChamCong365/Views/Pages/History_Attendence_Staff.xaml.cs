using QuanLyChung365TruocDangNhap.ChamCong365.Core;
using QuanLyChung365TruocDangNhap.ChamCong365.Entities;
using QuanLyChung365TruocDangNhap.ChamCong365.Entities.Company;
using QuanLyChung365TruocDangNhap.ChamCong365.Entities.History_Attenden_Staff;
using QuanLyChung365TruocDangNhap.ChamCong365.Entities.Staff;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
namespace QuanLyChung365TruocDangNhap.ChamCong365.Views.Pages
{
    /// <summary>
    /// Interaction logic for History_Attendence_Staff.xaml
    /// </summary>
    public partial class History_Attendence_Staff : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public string CustomFormat { get; set; }
        public History_Attendence_Staff(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;

            tableAttStaff.AutoReponsiveColumn(1, "IDnTen");

            dpStart.SelectedDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            dpStart1.SelectedDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            dpEnd.SelectedDate = DateTime.Today;
            dpEnd1.SelectedDate = DateTime.Today;
            getData().GetAwaiter();
        }
        //
        private MainWindow Main;

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

        private List<Htr_Att_Staff> _listStaff;
        public List<Htr_Att_Staff> listStaff
        {
            get { return _listStaff; }
            set { _listStaff = value; OnPropertyChanged(); }
        }
        private List<LSChamCongQR> _listStaff1;
        public List<LSChamCongQR> listStaff1
        {
            get { return _listStaff1; }
            set { _listStaff1 = value; OnPropertyChanged(); }
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

        private List<string> _listtrangthai = new List<string>() { "Tất cả","Thành công", "Thất bại", "Chờ duyệt"};
        public List<string> listtrangthai
        {
            get { return _listtrangthai; }
            set { _listtrangthai = value; OnPropertyChanged(); }
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
        private List<Item_List_PhongBan> _listDep1;
        public List<Item_List_PhongBan> listDep1
        {
            get { return _listDep1; }
            set { _listDep1 = value; OnPropertyChanged(); }
        }

        private int pageEP = 1;
        private int totalEP = 0;
        //get data table from api
        private async Task<List<Htr_Att_Staff>> loadAtt(int page = 1)
        {
            try
            {
                HttpClient http = new HttpClient();
                var apilink = "http://210.245.108.202:3000/api/qlc/timekeeping/get_history_time_keeping_by_company";
                var param = new List<string>();
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

                
                var z = searchbarEP.SelectedItem as Employee; 
                //if (z != null) param.Add("id_ep=" + z.ep_id);
                
                if (page - 1 >= 0) param.Add("off_set=" + ((page - 1) * 20).ToString());
                param.Add("length=20");

                if (param.Count > 0) apilink += "?" + string.Join("&", param);
                Dictionary<string, string> form = new Dictionary<string, string>();
                if (z != null) form.Add("ep_id", z.ep_id.ToString());
                if (dpStart.SelectedDate != null) form.Add("start_date", dpStart.SelectedDate.Value.ToString("yyyy-MM-dd"));
                if (dpEnd.SelectedDate != null) form.Add("end_date", dpEnd.SelectedDate.Value.ToString("yyyy-MM-dd"));
                if (page - 1 >= 0) form.Add("page", page.ToString());
                form.Add("pageSize", "20");
                if(cbbCom.SelectedItem!=null && (cbbCom.SelectedItem as ChildCompany)!=null) form.Add("com_id",(cbbCom.SelectedItem as ChildCompany).com_id);
                HttpResponseMessage respon = await http.PostAsync(apilink, new FormUrlEncodedContent(form));
                API_Htr_Att_Staff api = JsonConvert.DeserializeObject<API_Htr_Att_Staff>(respon.Content.ReadAsStringAsync().Result);
                if (api.data != null)
                {
                    int dem;
                    if (!string.IsNullOrEmpty(api.data.totalItems) && int.TryParse(api.data.totalItems, out dem) && dem != pagin.TotalRecords) pagin.TotalRecords = dem;
                    return api.data.items;
                }
                else return null;
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
        private async Task<List<LSChamCongQR>> loadAtt1(int page = 1)
        {
            try
            {
                HttpClient http = new HttpClient();
                var apilink = "http://210.245.108.202:3000/api/qlc/timekeeping/get_history_time_keeping_by_company";
                var param = new List<string>();
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
                if(cbbCom1.SelectedItem!=null && (cbbCom1.SelectedItem as ChildCompany)!=null) form.Add("com_id", (cbbCom.SelectedItem as ChildCompany).com_id);
                var z = searchbarEP1.SelectedItem as Employee; 
                if (z != null) form.Add("ep_id", z.ep_id);
                if (dpStart1.SelectedDate != null) form.Add("start_date", dpStart1.SelectedDate.Value.ToString("yyyy-MM-dd"));
                if (dpEnd1.SelectedDate != null) form.Add("end_date", dpEnd1.SelectedDate.Value.ToString("yyyy-MM-dd"));
                if (page - 1 >= 0) form.Add("page", page.ToString());
                form.Add("pageSize","20");
                form.Add("type","timekeeping_qr");
                if (searchbartt.SelectedIndex > 0)
                {
                    if(searchbartt.SelectedItem.ToString() == "Chờ duyệt")
                        form.Add("status","2");
                    else if(searchbartt.SelectedItem.ToString() == "Thành công")
                        form.Add("status","1");
                    else form.Add("status","0");

                }
                if (param.Count > 0) apilink += "?" + string.Join("&", param);
                HttpResponseMessage respon = await http.PostAsync(apilink, new FormUrlEncodedContent(form));
                API_LSChamCongQR api = JsonConvert.DeserializeObject<API_LSChamCongQR>(respon.Content.ReadAsStringAsync().Result);
                if (api.data != null)
                {
                    int dem;
                    if (!string.IsNullOrEmpty(api.data.totalItems) && int.TryParse(api.data.totalItems, out dem) && dem != pagin1.TotalRecords) pagin1.TotalRecords = dem;
                    return api.data.items;
                }
                else return null;
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
        private async Task<List<Employee>> loadEP(string idcom, string iddep, int page = 1, int lenght = 5000)
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
            form.Add("dep_id", iddep);
            form.Add("pageNumber", page.ToString());
            form.Add("pageSize", lenght.ToString());
            HttpResponseMessage respon = await http.PostAsync(apilink,new FormUrlEncodedContent(form));
            API_Employee_List nv = JsonConvert.DeserializeObject<API_Employee_List>(respon.Content.ReadAsStringAsync().Result);
            if (nv.data != null)
            {
                int.TryParse(nv.data.totalItems, out totalEP);
                if(tb.SelectedIndex == 0)
                {
                    listEmployee = nv.data.items;
                    searchbarEP.ItemsSource = listEmployee;
                    searchbarEP.Refresh();
                }
                else
                {
                    listEmployee1 = nv.data.items;
                    searchbarEP1.Refresh();
                }
            }
            return nv.data.items;
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
        private async Task<API_LSChamCongQR> getAtt1(int page = 1)
        {
            try
            {
                HttpClient http = new HttpClient();
                var apilink = "http://210.245.108.202:3000/api/qlc/timekeeping/get_history_time_keeping_by_company";
                var param = new List<string>();
                switch (Main.Type)
                {
                    case 1:
                        http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Main.APIStaff.data.data.access_token);
                        param.Add("id_com=" + Main.APIStaff.data.data.user_info.com_id);
                        break;
                    case 2:
                        http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Main.APICompany.data.data.access_token);
                        break;
                    default:
                        break;
                }
                Dictionary<string, string> form = new Dictionary<string, string>();
                var z = searchbarEP1.SelectedItem as Employee;
                if (z != null) form.Add("ep_id", z.ep_id);
                if (dpStart1.SelectedDate != null) form.Add("start_date", dpStart1.SelectedDate.Value.ToString("yyyy-MM-dd"));
                if (dpEnd1.SelectedDate != null) form.Add("end_date", dpEnd1.SelectedDate.Value.ToString("yyyy-MM-dd"));
                if (page - 1 >= 0) form.Add("page", page.ToString());
                form.Add("pageSize", "20");
                form.Add("type", "timekeeping_qr");

                HttpResponseMessage respon = await http.PostAsync(apilink, new FormUrlEncodedContent(form));
                API_LSChamCongQR api = JsonConvert.DeserializeObject<API_LSChamCongQR>(respon.Content.ReadAsStringAsync().Result);
                if (api.data != null)
                {
                    return api;
                }
                else return null;
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
        private async Task<API_Htr_Att_Staff> getAtt(int page = 1)
        {
            try
            {
                HttpClient http = new HttpClient();
                var apilink = "http://210.245.108.202:3000/api/qlc/timekeeping/get_history_time_keeping_by_company";
                var param = new List<string>();
                switch (Main.Type)
                {
                    case 1:
                        http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Main.APIStaff.data.data.access_token);
                        param.Add("id_com=" + Main.APIStaff.data.data.user_info.com_id);
                        break;
                    case 2:
                        http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Main.APICompany.data.data.access_token);
                        break;
                    default:
                        break;
                }

                var z = searchbarEP.SelectedItem as Employee;
                

                if (param.Count > 0) apilink += "?" + string.Join("&", param);
                Dictionary<string, string> form = new Dictionary<string, string>();
                if (z != null) form.Add("ep_id", z.ep_id);
                if (dpStart.SelectedDate != null) form.Add("start_date", dpStart.SelectedDate.Value.ToString("yyyy-MM-dd"));
                if (dpEnd.SelectedDate != null) form.Add("end_date", dpEnd.SelectedDate.Value.ToString("yyyy-MM-dd"));
                if (page - 1 >= 0) form.Add("page", page.ToString());
                form.Add("pageSize","20");
                HttpResponseMessage respon = await http.PostAsync(apilink, new FormUrlEncodedContent(form));
                API_Htr_Att_Staff api = JsonConvert.DeserializeObject<API_Htr_Att_Staff>(respon.Content.ReadAsStringAsync().Result);
                if (api.data != null)
                {
                    return api;
                }
                else return null;
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
        private async Task getData()
        {
            listEmployee = new List<Employee>();
            loadEP("", "").ContinueWith(zz => this.Dispatcher.Invoke(() => listEmployee = zz.Result));
            loadEP("", "").ContinueWith(zz => this.Dispatcher.Invoke(() => listEmployee1 = zz.Result));
            getChildCom().ContinueWith(tt => this.Dispatcher.Invoke(() =>
            {
                listCom = tt.Result;
                listCom.Insert(0, new ChildCompany() { com_id = "-1", com_name = "Tất cả công ty" });
                listCom = listCom.ToList();
                cbbCom.SelectedIndex = listCom.Count - 1;
                cbbCom1.SelectedIndex = listCom.Count - 1;
                loadAtt().ContinueWith(zz => this.Dispatcher.Invoke(() => listStaff = zz.Result));
                loadAtt1().ContinueWith(zz => this.Dispatcher.Invoke(() => listStaff1 = zz.Result));
            }));

        }
        //size_changed
        private void History_Atd_Staff_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.ActualWidth > 960)
            {
                IsSmallSize = 0;
            }
            else if (this.ActualWidth <= 960 && this.ActualWidth > 460)
            {
                IsSmallSize = 1;
            }
            else /*(this.ActualWidth <= 460)*/
            {
                IsSmallSize = 2;
            }
        }
        //scrollview khi lăn chuột
        private void tableAttStaff_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Main.scrollMain.OnPreviewMouseWheel(sender, e);
        }
        private void sreachbarEP_ScrollToEnd()
        {
            if (listEmployee.Count + (pageEP + 1 * 20) <= totalEP)
            {
                pageEP++;
                loadEP("","",pageEP, 20);
            }
        }
        private void btn_ApDung_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            loadAtt().ContinueWith(tt => this.Dispatcher.Invoke(() => listStaff = tt.Result));
        }
        private void btn_ApDung1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            loadAtt1().ContinueWith(tt => this.Dispatcher.Invoke(() => listStaff1 = tt.Result));
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
        private void cbbCom1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var z = cbbCom1.SelectedItem as ChildCompany;
            if (z != null)
            {
                getDep(z.com_id).ContinueWith(tt => this.Dispatcher.Invoke(() =>
                {
                    listDep1 = tt.Result;
                    listDep1.Insert(0, new Item_List_PhongBan() { dep_id = "-1", dep_name = "Tất cả phòng ban" });
                    listDep1 = listDep1.ToList();
                }));
            }
        }
        private void pagin_SelectionChange(object sender, SelectionChangedEventArgs e)
        {
            getAtt(pagin.SelectedPage).ContinueWith(tt => this.Dispatcher.Invoke(() =>
            {
                if (tt.Result!=null)
                {
                    listStaff = tt.Result.data.items;
                }
            }));
        }
        private void pagin1_SelectionChange(object sender, SelectionChangedEventArgs e)
        {
            getAtt1(pagin1.SelectedPage).ContinueWith(tt => this.Dispatcher.Invoke(() =>
            {
                if (tt.Result!=null)
                {
                    listStaff1 = tt.Result.data.items;
                }
            }));
        }
        class ExcelData
        {
            public string ID { get; set; }
            public string Name { get; set; }
            public string Shift { get; set; }
            public string Time { get; set; }
            public string Address { get; set; }
        }
        private async Task ExportExcel<T>(List<T> data, FileInfo file)
        {
            if (file.Exists)
            {
                file.Delete();
            }
            using (var package = new ExcelPackage(file))
            {
                //package.Workbook.Worksheets.Delete("");
                var ws = package.Workbook.Worksheets.Add("Bảng công theo tháng");

                ws.Cells["A1"].Value = "LỊCH SỬ ĐIỂM DANH";
                ws.Cells["A1:E1"].Merge = true;
                ws.Row(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ws.Row(1).Style.Font.Bold = true;
                ws.Row(1).Style.Font.Size = 13;

                ws.Cells["A2"].Value = string.Format("Từ ngày {0} đến ngày {1}", dpStart.SelectedDate.Value.ToString("dd/MM/yyyy"), dpEnd.SelectedDate.Value.ToString("dd/MM/yyyy"));
                ws.Cells["A2:E2"].Merge = true;
                ws.Row(2).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ws.Row(2).Style.Font.Bold = true;
                ws.Row(2).Style.Font.Size = 13;

                ws.Cells["A3"].Offset(0, 0).Value = "Mã NV";
                ws.Cells["A3"].Offset(0, 1).Value = "Tên nhân viên";
                ws.Cells["A3"].Offset(0, 2).Value = "Ca làm việc";
                ws.Cells["A3"].Offset(0, 3).Value = "Thời gian điểm danh";
                ws.Cells["A3"].Offset(0, 4).Value = "Địa điểm";

                ws.Row(3).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ws.Row(3).Style.Font.Bold = true;
                ws.Row(3).Style.Font.Size = 13;

                for (int i = 0; i < data.Count; i++)
                {
                    ws.Row(i + 4).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Row(i + 4).Style.Font.Size = 13;
                }

                var range = ws.Cells["A4"].LoadFromCollection(data);
                ws.Cells["A3:" + range.End.Address].AutoFitColumns();

                package.SaveAsync();
            }
        }
        private void btnEx_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            loading.Visibility = Visibility.Visible;
            var list = new List<Htr_Att_Staff>();
            var p = pagin.Pages;
            bool flag = true;
            var k = new List<Task>();
            for (int i = 0; i < p; i++)
            {
                k.Add(loadAtt(i + 1).ContinueWith(tt => this.Dispatcher.Invoke(() => list.AddRange(tt.Result))));
            }

            k.ForEach(t =>
            {
                t.ContinueWith(tt =>
                {
                    var ck = new List<bool>();
                    k.ForEach(h => ck.Add(h.IsCompleted));
                    if (!ck.Contains(false) && flag)
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            flag = false;
                            Microsoft.Win32.SaveFileDialog sv = new Microsoft.Win32.SaveFileDialog();
                            sv.Filter = "Microsoft Excel Worksheet | *.xlsx";
                            sv.FileName = "data";
                            if (sv.ShowDialog() == true)
                            {
                                var data = new List<ExcelData>();
                                list = list.OrderBy(l => l.ep_id).ToList();
                                list = list.OrderBy(l => l.date).ToList();
                                list.ForEach(i =>
                                {
                                    data.Add(new ExcelData()
                                    {
                                        ID = i.ep_id,
                                        Name = i.ep_name,
                                        Shift = i.shift_name,
                                        Time = i.at_time,
                                        Address = i.ts_location_name,
                                    });
                                });

                                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                                var file = new FileInfo(sv.FileName);
                                ExportExcel<ExcelData>(data, file).ContinueWith(zz => this.Dispatcher.Invoke(() =>
                                {
                                    loading.Visibility = Visibility.Collapsed;
                                    loading.Visibility = Visibility.Collapsed;
                                    var x = new PopUp.ConfirmBox();
                                    x.ConfirmTitle = "Xuất file excel";
                                    x.Message = "Xuất file excel thành công, bạn có muốn mở file không?";
                                    Main.ShowPopup(x);
                                    x.Accept = () =>
                                    {
                                        System.Diagnostics.Process.Start(file.FullName);
                                    };
                                }));
                            }
                            loading.Visibility = Visibility.Collapsed;
                        });
                    }
                });
            });
        }

        private void cbbPB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var com = cbbCom.SelectedItem as ChildCompany;
            var dep = cbbPB.SelectedItem as Item_List_PhongBan;
            if(cbbPB.SelectedIndex > 0)
            {
                loadEP(com.com_id, dep.dep_id).ContinueWith(zz => this.Dispatcher.Invoke(() => listEmployee = zz.Result));
            }
            else
            {
                loadEP("","").ContinueWith(zz => this.Dispatcher.Invoke(() => listEmployee = zz.Result));
            }
            /*searchbarEP.ItemsSource = listEmployee;
            searchbarEP.Refresh();*/
        }
        private void cbbPB1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var com = cbbCom1.SelectedItem as ChildCompany;
            var dep = cbbPB1.SelectedItem as Item_List_PhongBan;
            if (cbbPB.SelectedIndex > 0)
            {
                loadEP(com.com_id, dep.dep_id).ContinueWith(zz => this.Dispatcher.Invoke(() => listEmployee1 = zz.Result));
            }
            else
            {
                loadEP("", "").ContinueWith(zz => this.Dispatcher.Invoke(() => listEmployee1 = zz.Result));
            }
        }
    }
}