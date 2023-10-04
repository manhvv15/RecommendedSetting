using QuanLyChung365TruocDangNhap.ChamCong365.Entities;
using QuanLyChung365TruocDangNhap.ChamCong365.Entities.Company;
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
using QuanLyChung365TruocDangNhap.ChamCong365.Core;
using System.Data;
using System.Reflection;
using QuanLyChung365TruocDangNhap.ChamCong365.Entities.History_Attenden_Staff;
using System.Net;
using System.Security.Cryptography;

namespace QuanLyChung365TruocDangNhap.ChamCong365.Views.Pages.Xuat_Cong
{
    /// <summary>
    /// Interaction logic for Bang_Cong_Nhan_Vien.xaml
    /// </summary>
    public partial class Xuat_Cong_CongTy : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public Xuat_Cong_CongTy(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;

            dpStart.SelectedDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            dpEnd.SelectedDate = DateTime.Today;
            table.AutoReponsiveColumn(1, "IDnTen");

            getData();
        }
        //
        public List<string> listCheck { get; set; } = new List<string> { "Nhân viên có chấm công", "Toàn bộ nhân viên" };
        public MainWindow Main { get; set; }
        private int _IsSmallSize = 0;
        public int IsSmallSize
        {
            get { return _IsSmallSize; }
            set
            {
                _IsSmallSize = value;
                OnPropertyChanged("IsSmallSize");
            }
        }

        private List<BangCong> _listCong;
        public List<BangCong> listCong
        {
            get { return _listCong; }
            set
            {
                var z = table.Columns.ToList();
                z.ForEach(c =>
                {
                    int index = z.IndexOf(c);
                    if (index > 2) table.Columns.Remove(c);
                });
                _listCong = value;
                if (value != null)
                {
                    int max = 1;
                    value.ForEach(x =>
                    {
                        if (x.lst_time.Count > max) max = x.lst_time.Count;
                    });
                    for (int i = 0; i < max; i++)
                    {
                        DataGridTextColumn col = new DataGridTextColumn();
                        col.Header = "Thời gian";
                        col.ElementStyle = App.Current.Resources["textRowCenter"] as Style;
                        col.Binding = new Binding() { Path = new PropertyPath("lst_time[" + i.ToString() + "]") };
                        table.Columns.Add(col);
                    }
                }
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

        private List<Employee> _listEp;
        public List<Employee> listEp
        {
            get { return _listEp; }
            set { _listEp = value; OnPropertyChanged(); }
        }
        private int EpPage = 1;
        private string EpKey = "";
        //
        private async Task<API_BangCong_List> getBangCong(int page = 1)
        {
            try
            {
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

                var apilink = "http://210.245.108.202:3000/api/qlc/timekeeping/com/success";
                var pram = new List<string>();
                var form = new Dictionary<string, string>();
                if (cboCom.SelectedItem != null) form.Add("com_id", cboCom.SelectedValue.ToString());
                if (cboDep.SelectedItem != null && cboDep.SelectedIndex >= 1) form.Add("dep_id", cboDep.SelectedValue.ToString());
                if (cboEP.SelectedItem != null) form.Add("idQLC", (cboEP.SelectedItem as Employee).ep_id);

                if (dpStart.SelectedDate != null) form.Add("inputOld", dpStart.SelectedDate.Value.ToString("yyyy/MM/dd"));
                if (dpEnd.SelectedDate != null) form.Add("inputNew", dpEnd.SelectedDate.Value.ToString("yyyy/MM/dd"));

                if (page <= 0) page = 1;
                var offset = (page - 1) * 20;
                form.Add("pageNumber", page.ToString());
                form.Add("pageSize", "20");

                HttpResponseMessage respon = await http.PostAsync(apilink, new FormUrlEncodedContent(form));
                var check = respon.Content.ReadAsStringAsync().Result;
                API_BangCong_List api = JsonConvert.DeserializeObject<API_BangCong_List>(respon.Content.ReadAsStringAsync().Result);
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
        private void  getXuatExcel()
        {
            try
            {
                using (WebClient http = new WebClient())
                {
                    var apilink = "http://210.245.108.202:3000/api/qlc/timekeeping/com/success";
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

                    if (cboCom.SelectedItem != null) http.QueryString.Add("com_id" , cboCom.SelectedValue.ToString());
                    if (cboDep.SelectedItem != null && cboDep.SelectedIndex >= 1) http.QueryString.Add("dep_id", cboDep.SelectedValue.ToString());
                    if (cboEP.SelectedItem != null) http.QueryString.Add("idQLC", (cboEP.SelectedItem as Employee).ep_id);

                    if (dpStart.SelectedDate != null) http.QueryString.Add("inputOld", dpStart.SelectedDate.Value.ToString("yyyy-MM-dd"));
                    if (dpEnd.SelectedDate != null) http.QueryString.Add("inputNew", dpEnd.SelectedDate.Value.ToString("yyyy-MM-dd"));
                    http.QueryString.Add("pageNumber", "1");
                    http.QueryString.Add("pageSize", "1000000");
                    http.UploadValuesCompleted += (sender, e) =>
                    {
                        API_XuatExcel api = JsonConvert.DeserializeObject<API_XuatExcel>(UnicodeEncoding.UTF8.GetString(e.Result));
                        if (api.list_total != null)
                        {
                            var start = dpStart.SelectedDate.Value.ToString("dd/MM/yyyy");
                            var end = dpEnd.SelectedDate.Value.ToString("dd/MM/yyyy");
                            Microsoft.Win32.SaveFileDialog sv = new Microsoft.Win32.SaveFileDialog();
                            sv.Filter = "Microsoft Excel Worksheet | *.xlsx";
                            sv.FileName = "data";
                            if (sv.ShowDialog() == true)
                            {
                                var data = new List<ExcelData>();
                                int max = 1;
                                List<ExcelData> list1 = api.list_total;
                                list1.ForEach(i =>
                                {
                                    data.Add(new ExcelData
                                    {
                                        ep_id = i.ep_id,
                                        ep_name = i.ep_name,
                                        ts_date = i.ts_date,
                                        day_of_week = i.day_of_week,
                                        num_to_calculate = i.num_to_calculate,
                                        late = i.late,
                                        early = i.early,
                                        total_time = i.total_time,
                                        num_overtime = i.num_overtime,
                                        lst_time = i.lst_time,
                                    });
                                });

                                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                                var file = new FileInfo(sv.FileName);
                                ExportExcel(data, file, start, end).ContinueWith(zz => this.Dispatcher.Invoke(() =>
                                {
                                    var x = new PopUp.ConfirmBox();
                                    x.ConfirmTitle = "Xuất file excel";
                                    x.Message = "Xuất file excel thành công, bạn có muốn mở file không?";
                                    Main.ShowPopup(x);
                                    x.Accept = () =>
                                    {
                                        System.Diagnostics.Process.Start(file.FullName);
                                    };
                                }));
                                this.Dispatcher.Invoke(() => loading.Visibility = Visibility.Collapsed);
                            }
                        }
                        };
                        http.UploadValuesTaskAsync(new Uri(apilink), http.QueryString);
                }
            }
            catch (Exception)
            {
            }
            /*try
            {
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

                var apilink = "https://chamcong.24hpay.vn/service/export_now_pc.php";
                var pram = new List<string>();

                if (cboCom.SelectedItem != null) pram.Add("id_com=" + cboCom.SelectedValue);
                if (cboDep.SelectedItem != null && cboDep.SelectedIndex >= 1) pram.Add("id_dep=" + cboDep.SelectedValue);
                if (cboEP.SelectedItem != null) pram.Add("id_ep=" + (cboEP.SelectedItem as Employee).ep_id);

                if (dpStart.SelectedDate != null) pram.Add("start_date=" + dpStart.SelectedDate.Value.ToString("yyyy-MM-dd"));
                if (dpEnd.SelectedDate != null) pram.Add("end_date=" + dpEnd.SelectedDate.Value.ToString("yyyy-MM-dd"));

                if (pram.Count > 0)
                {
                    apilink += "?" + string.Join("&", pram);
                }

                HttpResponseMessage respon = await http.PostAsync(apilink);
                API_XuatExcel api = JsonConvert.DeserializeObject<API_XuatExcel>(respon.Content.ReadAsStringAsync().Result);
                if (api.list_total != null) return api;
                return null;
            }
            catch (Exception ex)
            {

                var x = new PopUp.Notify1();
                x.Type = PopUp.Notify1.NotifyType.Success;
                x.Message = ex.Message;
                Main.ShowPopup(x);
                return null;
            }*/
        }
        private async Task<API_Company_List> getCom(int page = 1)
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
                if (ts.data != null) return ts;
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
        private async Task<API_List_PhongBan> getDep(int page = 1)
        {
            try
            {
                if (cboCom.SelectedItem != null)
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
                form.Add("com_id", cboCom.SelectedValue.ToString());
                var respon = await httpClient.PostAsync(apilink, new FormUrlEncodedContent(form));
                    API_List_PhongBan api = JsonConvert.DeserializeObject<API_List_PhongBan>(respon.Content.ReadAsStringAsync().Result);
                    if (api.data != null) return api;
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
        private async Task<API_Employee_List> getEp(int page = 1,int length=20)
        {
            try
            {
                if (cboCom.SelectedItem != null)
                {
                    HttpClient http = new HttpClient();
                    var apilink = "http://210.245.108.202:3000/api/qlc/managerUser/list";
                    var pram = new List<string>();
                    Dictionary<string, string> form = new Dictionary<string, string>();
                    switch (Main.Type)
                    {
                        case 1:
                            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Main.APIStaff.data.data.access_token);
                            if (cboCom.SelectedItem != null && cboCom.SelectedIndex >= 1) form.Add("com_id", cboCom.SelectedValue.ToString());
                            else form.Add("com_id", Main.APIStaff.data.data.user_info.com_id);
                            break;
                        case 2:
                            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Main.APICompany.data.data.access_token);
                            if (cboCom.SelectedItem != null && cboCom.SelectedIndex >= 1) form.Add("com_id", cboCom.SelectedValue.ToString());
                            else form.Add("com_id", Main.APICompany.data.data.user_info.com_id);
                            break;
                        default:
                            break;
                    }

                    if (cboDep.SelectedItem != null && cboDep.SelectedIndex >= 1) form.Add("id_com", cboDep.SelectedValue.ToString());
                    form.Add("pageNumber", page.ToString());
                    form.Add("pageSize", length.ToString());
                    HttpResponseMessage respon = await http.PostAsync(apilink, new FormUrlEncodedContent(form));


                    API_Employee_List api = JsonConvert.DeserializeObject<API_Employee_List>(respon.Content.ReadAsStringAsync().Result);
                    if (api.data != null) return api;
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
        private async Task<API_LichLamViec_ChiTiet> getLichLamViec(string epID, string date)
        {
            try
            {
                HttpClient http = new HttpClient();
                http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Main.APIStaff.data.data.access_token);

                var apilink = "https://chamcong.24hpay.vn/service/detail_cycle.php";
                var pram = new List<string>();

                pram.Add("id_ep=" + epID);
                if (!string.IsNullOrEmpty(date)) pram.Add("month_apply=" + date);

                if (pram.Count > 0)
                {
                    apilink += "?" + string.Join("&", pram);
                }

                HttpResponseMessage respon = await http.GetAsync(apilink);
                API_LichLamViec_ChiTiet api = JsonConvert.DeserializeObject<API_LichLamViec_ChiTiet>(respon.Content.ReadAsStringAsync().Result);
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
        private async Task<API_Shift_Manage> getCa()
        {
            HttpClient http = new HttpClient();
            var apilink = "http://210.245.108.202:3000/api/qlc/shift/list";
            var pram = new Dictionary<string,string>();

            switch (Main.Type)
            {
                case 1:
                    http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Main.APIStaff.data.data.access_token);
                    apilink = apilink + "?com_id=" + Main.APIStaff.data.data.user_info.com_id;
                    pram.Add("com_id", Main.APIStaff.data.data.user_info.com_id);
                    break;
                case 2:
                    http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Main.APICompany.data.data.access_token);
                    break;
                default:
                    break;
            }
            HttpResponseMessage respon = await http.GetAsync(apilink);
            API_Shift_Manage api = JsonConvert.DeserializeObject<API_Shift_Manage>(respon.Content.ReadAsStringAsync().Result);
            if (api.data != null) return api;
            return null;
        }
        private async Task getData()
        {
            loading.Visibility = Visibility.Visible;
            List<Task> k = new List<Task>()
            {
                getCom().ContinueWith(tt=>this.Dispatcher.Invoke(()=>{
                    if (tt.Result!=null)
                    {
                        listCom=tt.Result.data.items;
                        if (listCom!=null&& listCom.Count>0)
                        {
                            cboCom.SelectedIndex=listCom.Count-1;
                            getDep().ContinueWith(hh=>this.Dispatcher.Invoke(()=>{
                                if (hh.Result!=null)
                                {
                                    listDep=hh.Result.data.items;
                                    listDep.Insert(0,new Item_List_PhongBan(){ dep_id="-1",dep_name="Phòng ban (tất cả)"});
                                    listDep=listDep.ToList();
                                    cboDep.SelectedIndex = 0;
                                }
                            }));
                        }
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
                            getEp(EpPage,500).ContinueWith(ee => this.Dispatcher.Invoke(() =>
                            {
                                if (ee.Result != null)
                                {
                                    listEp = ee.Result.data.items;
                                }
                            }));
                            getBangCong().ContinueWith(zz => this.Dispatcher.Invoke(() =>
                            {
                                if (zz.Result != null)
                                {
                                    listCong = zz.Result.data.items;
                                    pagin.TotalRecords = zz.Result.data.totalItems;

                                }
                                loading.Visibility = Visibility.Collapsed;
                            }));
                        });
                    }
                });
            });

        }
        //
        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.ActualWidth > 910)
            {
                IsSmallSize = 0;
            }
            else if (this.ActualWidth <= 910 && this.ActualWidth > 500)
            {
                IsSmallSize = 1;
            }
            else
            {
                IsSmallSize = 2;
            }
        }
        //EXCEL
        private async Task ExportExcel(List<ExcelData> data, FileInfo file, string startDate = "", string endDate = "")
        {
            if (file.Exists)
            {
                file.Delete();
            }
            using (var package = new ExcelPackage(file))
            {
                var ws = package.Workbook.Worksheets.Add("Bảng công theo tháng");

                ws.Cells["A3"].Offset(0, 0).Value = "Mã NV";
                ws.Cells["A3"].Offset(0, 1).Value = "Tên nhân viên";
                ws.Cells["A3"].Offset(0, 2).Value = "Ngày";
                ws.Cells["A3"].Offset(0, 3).Value = "Thứ";
                ws.Cells["A3"].Offset(0, 4).Value = "Công";
                ws.Cells["A3"].Offset(0, 5).Value = "Muộn(phút)";
                ws.Cells["A3"].Offset(0, 6).Value = "Sớm(phút)";
                ws.Cells["A3"].Offset(0, 7).Value = "Số giờ";
                ws.Cells["A3"].Offset(0, 8).Value = "Công tăng ca";

                int max = 1;
                data.ForEach(dd =>
                {
                    if (dd.lst_time.Count > max) max = dd.lst_time.Count;
                });

                ws.Cells["A1"].Value = "CHI TIẾT CHẤM CÔNG CÔNG TY " + Main.APICompany.data.data.user_info.com_name.ToUpper();
                ws.Cells[1, 1, 1, 9 + max].Merge = true;
                ws.Row(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ws.Row(1).Style.Font.Bold = true;
                ws.Row(1).Style.Font.Size = 13;

                ws.Cells["A2"].Value = string.Format("Từ ngày {0} đến ngày {1}", startDate, endDate);
                ws.Cells[2,1,2,9+max].Merge = true;
                ws.Row(2).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ws.Row(2).Style.Font.Bold = true;
                ws.Row(2).Style.Font.Size = 13;

                for (int i = 0; i < max; i++)
                {
                    ws.Cells["A3"].Offset(0, 9 + i).Value = "Thời gian";
                }


                ws.Row(3).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ws.Row(3).Style.Font.Bold = true;
                ws.Row(3).Style.Font.Size = 13;

                for (int i = 0; i < data.Count; i++)
                {
                    ws.Row(i + 4).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Row(i + 4).Style.Font.Size = 13;
                }

                for (int i = 0; i <data.Count; i++)
                {
                    ws.Cells[4+i, 1].Value = data[i].ep_id;
                    ws.Cells[4+i, 2].Value = data[i].ep_name;
                    ws.Cells[4+i, 3].Value = data[i].ts_date;
                    ws.Cells[4+i, 4].Value = data[i].day_of_week;
                    ws.Cells[4+i, 5].Value = data[i].num_to_calculate;
                    ws.Cells[4+i, 6].Value = data[i].late;
                    ws.Cells[4+i, 7].Value = data[i].early;
                    ws.Cells[4+i, 8].Value = data[i].total_time;
                    ws.Cells[4+i, 9].Value = data[i].num_overtime;
                    for (int j = 0; j < data[i].lst_time.Count; j++)
                    {
                        ws.Cells[4 + i, 10+j].Value = DateTime.Parse(data[i].lst_time[j]).ToString("hh:mm tt");
                    }
                }

                

                ws.Cells["A3:" + ws.Cells.End.Address].AutoFitColumns();
                package.SaveAsync();
            }
        }
        private void btnEx_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            loading.Visibility = Visibility.Visible;
            var list = new List<BangCong>();
            var start = dpStart.SelectedDate.Value.ToString("dd/MM/yyyy");
            var end = dpEnd.SelectedDate.Value.ToString("dd/MM/yyyy");
            bool flag = true;
            getXuatExcel();/*.ContinueWith(tt => this.Dispatcher.Invoke(() =>
            {
                if (tt.Result != null)
                {
                    Microsoft.Win32.SaveFileDialog sv = new Microsoft.Win32.SaveFileDialog();
                    sv.Filter = "Microsoft Excel Worksheet | *.xlsx";
                    sv.FileName = "data";
                    if (sv.ShowDialog() == true)
                    {
                        var data = new List<ExcelData>();
                        list = list.OrderBy(l => l.ep_id).ToList();
                        list = list.OrderBy(l => l.date).ToList();
                        int max = 1;
                        list.ForEach(l =>
                        {
                            if (l.lst_time.Count > max) max = l.lst_time.Count;
                        });
                        List<ExcelData> list1 = tt.Result.list_total;
                        list1.ForEach(i =>
                        {
                            data.Add(new ExcelData
                            {
                                ep_id = i.ep_id,
                                ep_name = i.ep_name,
                                ts_date = i.ts_date,
                                day_of_week = i.day_of_week,
                                num_to_calculate = i.num_to_calculate,
                                late = i.late,
                                early = i.early,
                                total_time = i.total_time,
                                num_overtime = i.num_overtime,
                                lst_time = i.lst_time,
                            });
                        });

                        ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                        var file = new FileInfo(sv.FileName);
                        ExportExcel(data, file, start, end).ContinueWith(zz => this.Dispatcher.Invoke(() =>
                        {
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
                    this.Dispatcher.Invoke(() => loading.Visibility = Visibility.Collapsed);
                    *//*var page = tt.Result.data.totalItems / 20;
                    if (tt.Result.data.totalItems % 20 > 0) page++;
                    List<Task> k = new List<Task>();
                    for (int i = 0; i < page; i++)
                    {
                        k.Add(getBangCong(page - i).ContinueWith(zz => this.Dispatcher.Invoke(() => { if (tt.Result != null) list.AddRange(zz.Result.data.items); })));
                    }
                    k.ForEach(t =>
                    {
                        t.ContinueWith(dd =>
                        {
                            var ck = new List<bool>();
                            k.ForEach(h => ck.Add(h.IsCompleted));
                            if (!ck.Contains(false) && flag)
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
                                    int max = 1;
                                    list.ForEach(l =>
                                    {
                                        if (l.lst_time.Count > max) max = l.lst_time.Count;
                                    });
                                    list.ForEach(i =>
                                    {
                                        data.Add(new ExcelData
                                        {
                                            ma_nv = i.ep_id,
                                            ten_nv = i.ep_name,
                                            ngay = i.ts_date,
                                            thu = i.day_of_week,
                                            cong = "0",
                                            muon = "0",
                                            som = "0",
                                            so_gio = "0",
                                            cong_tang_ca = "0",
                                            thoigian = i.lst_time,
                                        });
                                    });

                                    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                                    var file = new FileInfo(sv.FileName);
                                    ExportExcel(data, file, start, end).ContinueWith(zz => this.Dispatcher.Invoke(() =>
                                     {
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
                                this.Dispatcher.Invoke(() => loading.Visibility = Visibility.Collapsed);
                            }
                        });
                    });*//*
                }
            }));*/
        }
        //
        private void btnGet_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            getBangCong().ContinueWith(tt => this.Dispatcher.Invoke(() =>
            {
                if (tt.Result != null)
                {
                    listCong = tt.Result.data.items;
                    pagin.TotalRecords = tt.Result.data.totalItems;
                }
            }));
        }
        private void pagin_SelectionChange(object sender, SelectionChangedEventArgs e)
        {
            loading.Visibility = Visibility.Visible;
            getBangCong(pagin.SelectedPage).ContinueWith(tt => this.Dispatcher.Invoke(() =>
            {
                if (tt.Result != null)
                {
                    listCong = tt.Result.data.items;
                    loading.Visibility = Visibility.Collapsed;
                }
            }));
        }
        private void DataGrid_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Main.scrollMain.OnPreviewMouseWheel(sender, e);
        }
        private void cboCom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboCom.SelectedItem != null)
            {
                getDep().ContinueWith(tt => this.Dispatcher.Invoke(() =>
                {
                    if (tt.Result != null)
                    {
                        listDep = tt.Result.data.items;
                        listDep.Insert(0, new Item_List_PhongBan() { dep_id = "-1", dep_name = "Phòng ban (tất cả)" });
                        listDep = listDep.ToList();
                        cboDep.SelectedIndex = 0;

                        getEp().ContinueWith(zz => this.Dispatcher.Invoke(() =>
                        {
                            if (zz.Result != null)
                            {
                                listEp = zz.Result.data.items;
                                cboEP.Refresh();
                            }
                        }));
                    }
                }));
            }
        }
        private void cboEP_ScrollToEnd()
        {
            EpPage++;
            if (listEp == null) listEp = new List<Employee>();
            getEp(EpPage).ContinueWith(tt => this.Dispatcher.Invoke(() =>
            {
                if (tt.Result != null)
                {
                    listEp.AddRange(tt.Result.data.items);
                    listEp = listEp.ToList();
                    cboEP.Refresh();
                }
            }));
        }
        private void cboEP_SearchIsNull(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox)
            {
                EpKey = (sender as TextBox).Text;
                EpPage = 1;
                getEp().ContinueWith(tt => this.Dispatcher.Invoke(() =>
                {
                    if (tt.Result != null)
                    {
                        listEp = tt.Result.data.items;
                        cboEP.Refresh();
                    }
                }));
            }
        }
        private void cboDep_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            getEp().ContinueWith(tt => this.Dispatcher.Invoke(() =>
            {
                if (tt.Result != null)
                {
                    listEp = tt.Result.data.items;
                    cboEP.Refresh();
                }
            }));
        }
    }
}
