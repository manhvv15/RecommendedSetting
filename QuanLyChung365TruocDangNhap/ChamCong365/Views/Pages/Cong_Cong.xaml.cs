using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
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
using Newtonsoft.Json;

namespace QuanLyChung365TruocDangNhap.ChamCong365.Views.Pages
{
    /// <summary>
    /// Interaction logic for Cong_Cong.xaml
    /// </summary>
    public partial class Cong_Cong : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public Cong_Cong(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this;
            Main = main;

            int d = DateTime.Today.Month;
            if (d >= 0) cboMonth.SelectedIndex = d;
            if (listYear.Contains("Năm "+DateTime.Today.Year.ToString())) cboYear.SelectedIndex = listYear.FindIndex(x => x == "Năm " + DateTime.Today.Year.ToString());

            tableAttStaff.AutoReponsiveColumn(1, "IDnTen");

            getData();
        }
        //
        public MainWindow Main;
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
        public List<string> listMonth { get; set; } = new List<string> { "Chọn tháng", "Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4", "Tháng 5", "Tháng 6", "Tháng 7", "Tháng 8", "Tháng 9", "Tháng 10", "Tháng 11", "Tháng 12", };
        public List<string> listYear { get; set; } = new List<string> { "Chọn năm", "Năm 2020", "Năm 2021", "Năm 2022", "Năm 2023", "Năm 2024", "Năm 2025", "Năm 2026", "Năm 2027", "Năm 2028", "Năm 2029", "Năm 2030" };
        public List<string> listChucVu { get; set; } = new List<string> { "","SINH VIÊN THỰC TẬP", "NHÂN VIÊN THỬ VIỆC", "NHÂN VIÊN CHÍNH THỨC", "TRƯỞNG NHÓM", "PHÓ TRƯỞNG PHÒNG", "TRƯỞNG PHÒNG", "PHÓ GIÁM ĐỐC", "GIÁM ĐỐC", "NHÂN VIÊN PART TIME", "PHÓ BAN DỰ ÁN", "TRƯỞNG BAN DỰ ÁN", "PHÓ TỔ TRƯỞNG", "TỔ TRƯỞNG", "PHÓ TỔNG GIÁM ĐỐC","", "TỔNG GIÁM ĐỐC", "THÀNH VIÊN HỘI ĐỒNG QUẢN TRỊ","", "NHÓM PHÓ", "TỔNG GIÁM ĐỐC TẬP ĐOÀN", "PHÓ TỔNG GIÁM ĐỐC TẬP ĐOÀN" };

        private List<Employee> _listEp;
        public List<Employee> listEp
        {
            get { return _listEp; }
            set { _listEp = value; OnPropertyChanged(); }
        }

        private List<Employee> _listEpCK;
        public List<Employee> listEpCK
        {
            get { return _listEpCK; }
            set { _listEpCK = value; OnPropertyChanged(); }
        }

        private List<CongCongItem> _listCC;
        public List<CongCongItem> listCC
        {
            get { return _listCC; }
            set
            {
                _listCC = value;
                if (value != null) listCC.ForEach(i => i.STT = listCC.IndexOf(i) + 1);
                OnPropertyChanged();
            }
        }
        //
        private async Task<API_Employee_List> getEp(int page = 1,int length=20)
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
                form.Add("pageNumber", page.ToString());
                form.Add("pageSize", length.ToString());
                HttpResponseMessage respon = await http.PostAsync(apilink,new FormUrlEncodedContent(form));
                API_Employee_List api = JsonConvert.DeserializeObject<API_Employee_List>(respon.Content.ReadAsStringAsync().Result);
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
        private async Task<API_Employee_List> getEpCK(int length = 1)
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
                HttpResponseMessage respon = await http.PostAsync(apilink,new FormUrlEncodedContent(form));
                API_Employee_List api = JsonConvert.DeserializeObject<API_Employee_List>(respon.Content.ReadAsStringAsync().Result);
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
        private async Task<API_CongCong_List> getCongCong(int page = 1, string year = "", string month = "", string idEp = "")
        {
            try
            {
                if (page <= 0) page = 1;
                Dictionary<string, string> form = new Dictionary<string, string>();

                if (!string.IsNullOrEmpty(year))
                {
                    if (!string.IsNullOrEmpty(month))
                    {
                        form.Add("time_send_form", new DateTime(int.Parse(year), int.Parse(month), 1).ToString("yyyy-MM-dd"));
                        switch (int.Parse(month))
                        {
                            case 1:
                            case 3:
                            case 5:
                            case 7:
                            case 8:
                            case 10:
                            case 12:
                                form.Add("time_send_to", new DateTime(int.Parse(year), int.Parse(month), 31).ToString("yyyy-MM-dd"));
                                break;
                            case 2:
                                form.Add("time_send_to", new DateTime(int.Parse(year), int.Parse(month), 28).ToString("yyyy-MM-dd"));
                                break;
                            case 4:
                            case 6:
                            case 9:
                            case 11:
                                form.Add("time_send_to", new DateTime(int.Parse(year), int.Parse(month), 30).ToString("yyyy-MM-dd"));
                                break;
                        }
                    }
                    else
                    {
                        form.Add("time_send_form", new DateTime(int.Parse(year), 1, 1).ToString("yyyy-MM-dd"));
                        form.Add("time_send_to", new DateTime(int.Parse(year), 12, 31).ToString("yyyy-MM-dd"));
                    }
                        //form.Add("date", year);
                }

                if (!string.IsNullOrEmpty(idEp)) form.Add("id_user", idEp);

                switch (Main.Type)
                {
                    case 1:
                        form.Add("id_com", Main.APIStaff.data.data.user_info.com_id);
                        break;
                    case 2:
                        form.Add("id_com", Main.APICompany.data.data.user_info.com_id);
                        break;
                    default:
                        break;
                }
                form.Add("page", page.ToString());
                form.Add("type_dx", "17");
                form.Add("type_duyet", "11");
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
                var respon = await httpClient.PostAsync("http://210.245.108.202:3005/api/vanthu/DeXuat/admin_danh_sach_de_xuat", new FormUrlEncodedContent(form));
                API_List_De_Xuat Dexuat = JsonConvert.DeserializeObject<API_List_De_Xuat>(respon.Content.ReadAsStringAsync().Result);
                API_CongCong_List add = new API_CongCong_List();
                add.data = new CongCongData();
                add.data.items = new List<CongCongItem>();
                if (Dexuat != null && Dexuat.data.data != null && Dexuat.data.data.Count > 0)
                {
                    foreach (var item in Dexuat.data.data)
                    {
                        using (WebClient web = new WebClient())
                        {
                            web.Headers.Add("Authorization", "Bearer " + Main.APICompany.data.data.access_token);
                            web.QueryString.Add("_id", item._id.ToString());
                            var result = web.UploadValues(new Uri("http://210.245.108.202:3005/api/vanthu/catedx/showCTDX"), "POST", web.QueryString);
                            API_Detail_De_Xuat api = JsonConvert.DeserializeObject<API_Detail_De_Xuat>(UTF8Encoding.UTF8.GetString(result));
                            add.data.items.Add(new CongCongItem() { id_de_xuat = item._id.ToString(), id_user = api.data.detailDeXuat[0].id_nguoi_tao.ToString(), name_user = api.data.detailDeXuat[0].nguoi_tao, NoiDung = api.data.detailDeXuat[0].thong_tin_chung.xac_nhan_cong, id_user_duyet = api.data.detailDeXuat[0].lanh_dao_duyet[1].idQLC.ToString(), id_user_theo_doi = api.data.detailDeXuat[0].nguoi_theo_doi[0].idQLC.ToString(), ng_duyet = api.data.detailDeXuat[0].lanh_dao_duyet[1].userName });
                        }
                    }
                }
                
                if (add.data != null) return add;
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
        private async Task getData()
        {
            var year = "";
            if (listYear.Contains(DateTime.Today.Year.ToString())) year = listYear.Find(x => x == "Năm " + DateTime.Today.Year.ToString()).Split(' ')[1];
            getCongCong(1, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString()).ContinueWith(tt => this.Dispatcher.Invoke(() =>
            {
                if (tt.Result != null)
                {
                    listCC = tt.Result.data.items;
                    pagin.TotalRecords = int.Parse(tt.Result.data.totalItems);
                    getEpCK().ContinueWith(kk => this.Dispatcher.Invoke(() =>
                    {
                        if (kk.Result != null)
                        {
                            var dem = int.Parse(kk.Result.data.totalItems);
                            getEpCK(dem).ContinueWith(mm => this.Dispatcher.Invoke(() =>
                            {
                                listEpCK = mm.Result.data.items;
                                listCC.ForEach(e =>
                                {
                                    var t = listEpCK.Where(j => j.ep_id == e.id_user).SingleOrDefault();
                                    if (t != null)
                                    {
                                        e.dep_name = t.dep_name;
                                        if (!string.IsNullOrEmpty(t.position_id))
                                        {
                                            e.chuc_vu = listChucVu[int.Parse(t.position_id)];
                                        }

                                    }
                                    var d = listEpCK.Where(j => j.ep_id == e.id_user_duyet).SingleOrDefault();
                                    if (d != null)
                                    {
                                        e.ng_duyet = d.ep_name;
                                    }
                                });
                                listCC = listCC.ToList();
                            }));
                        }
                    }));
                }
            }));

            getEp(1,500).ContinueWith(tt => this.Dispatcher.Invoke(() =>
            {
                if (tt.Result != null)
                {
                    listEp = tt.Result.data.items;
                }
            }));
            
        }
        //size_changed
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
        private void tableAttStaff_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Main.scrollMain.OnPreviewMouseWheel(sender, e);
        }
        private async void ckCongCong_Checked(object sender, RoutedEventArgs e)
        {
            var z = (sender as CheckBox).Tag as CongCongItem;
            if (z != null)
            {
                try
                {
                    Dictionary<string, string> form = new Dictionary<string, string>();
                    /*form.Add("ep_id", z.id_user);
                    form.Add("shift_id", z.NoiDung.id_ca_xnc);
                    form.Add("start_time", z.NoiDung.time_vao_ca);
                    form.Add("end_time", z.NoiDung.time_het_ca);*/
                    form.Add("_id", z.id_de_xuat);
                    form.Add("type", "1");
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
                    var respon = await httpClient.PostAsync("http://210.245.108.202:3005/api/vanthu/editdx/edit_active", new FormUrlEncodedContent(form));
                    API_Respon add = JsonConvert.DeserializeObject<API_Respon>(respon.Content.ReadAsStringAsync().Result);
                    if (add.data != null && add.data.result)
                    {
                        var x = new PopUp.Notify1();
                        x.Type = PopUp.Notify1.NotifyType.Success;
                        x.Message = "Cộng công thành công";
                        Main.ShowPopup(x);

                        var k = listCC.Find(u => u.id_de_xuat == z.id_de_xuat);
                        if (k != null)
                        {
                            k.active = "1";
                        }
                        listCC = listCC.ToList();
                    }
                    else
                    {
                        var x = new PopUp.Notify1();
                        x.Type = PopUp.Notify1.NotifyType.Error;
                        x.Message = "Cộng công thất bại, vui lòng thử lại sau";
                        Main.ShowPopup(x);
                    }
                    
                    /*Dictionary<string, string> form1 = new Dictionary<string, string>();
                    form1.Add("id_dx", z.id_de_xuat);
                    form1.Add("active", "1");
                    HttpClient httpClient1 = new HttpClient();
                    switch (Main.Type)
                    {
                        case 1:
                            httpClient1.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Main.APIStaff.data.data.access_token);
                            break;
                        case 2:
                            httpClient1.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Main.APICompany.data.data.access_token);
                            break;
                        default:
                            break;
                    }
                    var respon1 = await httpClient1.PostAsync("https://vanthu.timviec365.vn/api/cc365_xn_dx_cong.php", new FormUrlEncodedContent(form1));*/
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
        private void btnGet_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var year = "";
            var month = "";
            var idep = "";
            if (cboEp.SelectedItem != null) idep = (cboEp.SelectedItem as Employee).ep_id;
            if (cboMonth.SelectedIndex >= 1) month = cboMonth.SelectedIndex.ToString();
            if (cboYear.SelectedIndex >= 1) year = cboYear.SelectedItem as string;
            year = year.Split(' ')[1];
            getCongCong(1, year, month, idep).ContinueWith(tt => this.Dispatcher.Invoke(() =>
              {
                  if (tt.Result != null)
                  {
                      listCC = tt.Result.data.items;
                      pagin.TotalRecords = int.Parse(tt.Result.data.totalItems);
                      getEpCK().ContinueWith(kk => this.Dispatcher.Invoke(() =>
                      {
                          if (kk.Result != null)
                          {
                              var dem = int.Parse(kk.Result.data.totalItems);
                              getEpCK(dem).ContinueWith(mm => this.Dispatcher.Invoke(() =>
                              {
                                  listEpCK = mm.Result.data.items;
                                  listCC.ForEach(b =>
                                  {
                                      var t = listEpCK.Where(j => j.ep_id == b.id_user).SingleOrDefault();
                                      if (t != null)
                                      {
                                          b.dep_name = t.dep_name;
                                          if (!string.IsNullOrEmpty(t.position_id))
                                          {
                                              b.chuc_vu = listChucVu[int.Parse(t.position_id)];
                                          }

                                      }
                                      var d = listEpCK.Where(j => j.ep_id == b.id_user_duyet).SingleOrDefault();
                                      if (d != null)
                                      {
                                          b.ng_duyet = d.ep_name;
                                      }
                                  });
                                  listCC = listCC.ToList();
                              }));
                          }
                      }));
                  }
              }));
        }
        private async void delCongCong(object sender, RoutedEventArgs e)
        {
            var z = (sender as CheckBox).Tag as CongCongItem;
            if (z != null)
            {
                try
                {
                    Dictionary<string, string> form = new Dictionary<string, string>();
                    form.Add("_id", z.id_de_xuat);
                    form.Add("type", "2");
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
                    var respon = await httpClient.PostAsync("http://210.245.108.202:3005/api/vanthu/editdx/edit_active", new FormUrlEncodedContent(form));
                    var g = respon.Content.ReadAsStringAsync().Result;
                    API_Respon add = JsonConvert.DeserializeObject<API_Respon>(respon.Content.ReadAsStringAsync().Result);
                    var x = new PopUp.Notify1();
                    x.Type = PopUp.Notify1.NotifyType.Success;
                    x.Message = "Xóa cộng công thành công";
                    Main.ShowPopup(x);

                    listCC.RemoveAll(u => u.id_de_xuat == z.id_de_xuat);
                    listCC = listCC.ToList();
                    /*if (add == null || (add.data != null && add.data.result))
                    {
                        var x = new PopUp.Notify1();
                        x.Type = PopUp.Notify1.NotifyType.Success;
                        x.Message = "Xóa cộng công thành công";
                        Main.ShowPopup(x);

                        listCC.RemoveAll(u => u.id_de_xuat == z.id_de_xuat);
                        listCC = listCC.ToList();
                    }
                    else
                    {
                        var x = new PopUp.Notify1();
                        x.Type = PopUp.Notify1.NotifyType.Error;
                        x.Message = "Xóa cộng công thất bại, vui lòng thử lại sau";
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
        private void GhiChu(object sender, MouseButtonEventArgs e)
        {
            var z = (sender as Border).Tag as CongCongItem;
            if (z != null)
            {
                var n = new PopUp.Notify1();
                n.Message = z.NoiDung.ly_do;
                Main.ShowPopup(n);
            }
        }
    }
}
