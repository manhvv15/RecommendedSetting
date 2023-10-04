using QuanLyChung365TruocDangNhap.ChamCong365.Entities;
using QuanLyChung365TruocDangNhap.ChamCong365.Entities.History_Attenden_Staff;
using QuanLyChung365TruocDangNhap.ChamCong365.Entities.Staff;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace QuanLyChung365TruocDangNhap.ChamCong365.Views.Pages.QL_Nhan_Vien
{
    /// <summary>
    /// Interaction logic for Home_Stuff.xaml
    /// </summary>
    public partial class Home_Stuff : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public Home_Stuff(MainWindow main)
        {
            InitializeComponent();
            this.DataContext = this;

            Main = main;

            getData();
        }
        //
        public MainWindow Main { get; set; }
        //SizeChanged
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

        private List<LichSuDiemDanh> _listDiemDanh;
        public List<LichSuDiemDanh> listDiemDanh
        {
            get { return _listDiemDanh; }
            set { _listDiemDanh = value; OnPropertyChanged(); }
        }

        private List<Shift_Manage_Item> _listCa;
        public List<Shift_Manage_Item> listCa
        {
            get { return _listCa; }
            set { _listCa = value; OnPropertyChanged(); }
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
            API_Shift_Manage api = JsonConvert.DeserializeObject<API_Shift_Manage>(respon.Content.ReadAsStringAsync().Result);
            if (api.data != null) return api;
            return null;
        }
        private async Task<API_LichSuDiemDanh_List> getDiemDanh()
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
                var apilink = "http://210.245.108.202:3000/api/qlc/timekeeping/get_history_time_keeping_by_company";
                var form = new Dictionary<string, string>();
                form.Add("page", "1");
                form.Add("pageSize", "10");
                HttpResponseMessage respon = await http.PostAsync(apilink, new FormUrlEncodedContent(form));
                API_LichSuDiemDanh_List api = JsonConvert.DeserializeObject<API_LichSuDiemDanh_List>(respon.Content.ReadAsStringAsync().Result);
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
        private async Task<API_LichSuDiemDanh_List> getDiemDanhBD(string startdate, string enddate, int length = 1)
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
                var apilink = string.Format("http://210.245.108.202:3000/api/qlc/timekeeping/get_history_time_keeping_by_company?start_date={0}&end_date={1}&length={2}", startdate, enddate, length);
                var form = new Dictionary<string, string>();
                form.Add("start_date", startdate);
                form.Add("end_date", enddate);
                form.Add("page", "1");
                form.Add("pageSize", length.ToString());
                HttpResponseMessage respon = await http.PostAsync(apilink, new FormUrlEncodedContent(form));
                API_LichSuDiemDanh_List api = JsonConvert.DeserializeObject<API_LichSuDiemDanh_List>(respon.Content.ReadAsStringAsync().Result);
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
        private async Task<API_Shift_Manage> getCa(int page = 1)
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
                var apilink = "http://210.245.108.202:3000/api/qlc/shift/list?com_id=" + Main.APIStaff.data.data.user_info.com_id;
                var pram = new Dictionary<string,string>();
                if (page <= 0) page = 1;
                var offset = (page - 1) * 20;
                pram.Add("com_id", Main.APIStaff.data.data.user_info.com_id);
                HttpResponseMessage respon = await http.GetAsync(apilink);
                API_Shift_Manage api = JsonConvert.DeserializeObject<API_Shift_Manage>(respon.Content.ReadAsStringAsync().Result);
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
        private async void getData()
        {
            var T2 = StartOfWeek(DateTime.Today, DayOfWeek.Monday);
            var T3 = T2.AddDays(1);
            var T4 = T3.AddDays(1);
            var T5 = T4.AddDays(1);
            var T6 = T5.AddDays(1);
            var T7 = T6.AddDays(1);
            var CN = T7.AddDays(1);

            tblDDLate.Text = tblDDError.Text = tblDDMonth.Text = "0 lần";
            var startMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var endMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));

            var listDiemDanhMonth = new List<LichSuDiemDanh>();
            var listAllCa = new List<Shift_Manage_Item>();

            var k1 = getDiemDanhBD(startMonth.ToString("yyyy-MM-dd"), endMonth.ToString("yyyy-MM-dd")).ContinueWith(tt => this.Dispatcher.Invoke(() =>
              {
                  if (tt.Result != null)
                  {
                      var late = new List<LichSuDiemDanh>();
                      int dem = int.Parse(tt.Result.data.totalItems);
                      getDiemDanhBD(startMonth.ToString("yyyy-MM-dd"), endMonth.ToString("yyyy-MM-dd"), dem).ContinueWith(zz => this.Dispatcher.Invoke(() =>
                      {
                          if (zz.Result != null)
                          {
                              listDiemDanhMonth = zz.Result.data.items;
                              int error = 0;
                              int monthdd = 0;
                              listDiemDanhMonth.ForEach(l =>
                              {
                                  if (l.is_success == "0") error++;
                                  if (l.is_success == "1") monthdd++;

                              });
                              tblDDError.Text = error.ToString() + " lần";
                              tblDDMonth.Text = monthdd.ToString() + " lần";

                              getCa().ContinueWith(cc => this.Dispatcher.Invoke(() =>
                              {
                                  if (cc.Result != null) listAllCa = cc.Result.data.items;
                                  int DDlate = 0;
                                  listDiemDanhMonth.RemoveAll(x => x.is_success == "0");
                                  for (int i = 0; i < (endMonth - startMonth).TotalDays; i++)
                                  {
                                      DateTime setday = startMonth.AddDays(i);
                                      var listTg = listDiemDanhMonth.Where(x => x.attime.ToString("yyyy-MM-dd") == setday.ToString("yyyy-MM-dd")).ToList();
                                      if (listTg != null)
                                      {
                                          var ca_in_day = new List<Shift_Manage_Item>();
                                          listTg.ForEach(x =>
                                          {
                                              var ca = listAllCa.SingleOrDefault(uu => uu.shift_id == x.shift_id);
                                              if (ca != null)
                                              {
                                                  if (!ca_in_day.Any(z => z.shift_id == x.shift_id))
                                                  {
                                                      ca_in_day.Add(ca);
                                                  }
                                              }

                                          });
                                          ca_in_day = ca_in_day.OrderBy(x => x.start_timex).ToList();
                                          ca_in_day.ForEach(shift =>
                                          {
                                              var ca = listAllCa.SingleOrDefault(x => x.shift_id == shift.shift_id);
                                              if (ca != null)
                                              {
                                                  var listDDinCa = listTg.Where(x => x.shift_id == shift.shift_id).ToList();
                                                  listDDinCa = listDDinCa.OrderBy(x => x.attime).ToList();
                                                  if (listDDinCa != null)
                                                  {
                                                      DateTime start, startlate;
                                                      DateTime end, endlate;
                                                      bool vao, ra;
                                                      vao = ra = false;
                                                      List<LichSuDiemDanh> seen = new List<LichSuDiemDanh>();
                                                      if ((string.IsNullOrEmpty(ca.start_time_latest) || ca.start_time_latest=="00:00:00") && !string.IsNullOrEmpty(ca.start_time) && DateTime.TryParse(ca.start_time, out start))
                                                      {
                                                          if (listDDinCa.Count > 0)
                                                          {
                                                              foreach (var item in listDDinCa)
                                                              {
                                                                  if (!seen.Contains(item)) seen.Add(item);
                                                                  if (item.attime.TimeOfDay <= start.TimeOfDay)
                                                                  {
                                                                      vao = true;
                                                                      break;
                                                                  }
                                                              }
                                                              if (!vao) DDlate++;
                                                          }
                                                      }
                                                      else if (!string.IsNullOrEmpty(ca.start_time_latest) && ca.start_time_latest != "00:00:00" && !string.IsNullOrEmpty(ca.start_time) && DateTime.TryParse(ca.start_time, out start) && DateTime.TryParse(ca.start_time_latest, out startlate))
                                                      {
                                                          if (listDDinCa.Count > 0)
                                                          {
                                                              foreach (var item in listDDinCa)
                                                              {
                                                                  if (!seen.Contains(item)) seen.Add(item);
                                                                  if (item.attime.TimeOfDay >= startlate.TimeOfDay && item.attime.TimeOfDay <= start.TimeOfDay)
                                                                  {
                                                                      vao = true;
                                                                      break;
                                                                  }
                                                              }
                                                              if (!vao) DDlate++;
                                                          }
                                                      }
                                                      listDDinCa.Reverse();
                                                      seen.ForEach(s =>
                                                      {
                                                          listDDinCa.RemoveAll(k => k.sheet_id == s.sheet_id);
                                                      });
                                                      if (!string.IsNullOrEmpty(ca.end_time) && (string.IsNullOrEmpty(ca.end_time_earliest) || ca.end_time_earliest=="00:00:00") && DateTime.TryParse(ca.end_time, out end))
                                                      {
                                                          if (listDDinCa.Count > 0)
                                                          {
                                                              foreach (var item in listDDinCa)
                                                              {
                                                                  if (item.attime.TimeOfDay >= end.TimeOfDay)
                                                                  {
                                                                      ra = true;
                                                                      break;
                                                                  }
                                                              }
                                                              if (!ra) DDlate++;
                                                          }
                                                      }
                                                      else if (!string.IsNullOrEmpty(ca.end_time) && string.IsNullOrEmpty(ca.end_time_earliest) && ca.end_time_earliest != "00:00:00" && DateTime.TryParse(ca.end_time, out end) && DateTime.TryParse(ca.end_time_earliest, out endlate))
                                                      {
                                                          if (listDDinCa.Count > 0)
                                                          {
                                                              foreach (var item in listDDinCa)
                                                              {
                                                                  if (item.attime.TimeOfDay >= end.TimeOfDay && item.attime.TimeOfDay <= endlate.TimeOfDay)
                                                                  {
                                                                      ra = true;
                                                                      break;
                                                                  }
                                                              }
                                                              if (!ra) DDlate++;
                                                          }
                                                      }
                                                  }
                                              }
                                          });
                                      }

                                  }
                                  tblDDLate.Text = DDlate.ToString() + " lần";
                              }));
                          }
                      }));
                  }
              }));

            // diem danh gan day
            getDiemDanh().ContinueWith(tt => this.Dispatcher.Invoke(() =>
            {
                if (tt.Result != null)
                {
                    listDiemDanh = tt.Result.data.items;
                }
            }));
            //bieu do diem danh theo tuan
            getDiemDanhBD(T2.ToString("yyyy-MM-dd"), CN.ToString("yyyy-MM-dd")).ContinueWith(tt => this.Dispatcher.Invoke(() =>
            {
                if (tt.Result != null)
                {
                    int dem = int.Parse(tt.Result.data.totalItems);
                    getDiemDanhBD(T2.ToString("yyyy-MM-dd"), CN.ToString("yyyy-MM-dd"), dem).ContinueWith(zz => this.Dispatcher.Invoke(() =>
                    {
                        if (zz.Result != null)
                        {
                            var list = zz.Result.data.items;
                            list.RemoveAll(i => i.is_success == "0");
                            var dembd = new List<int>() { 0, 0, 0, 0, 0, 0, 0 };
                            var col = new List<Rectangle>() { DDT2, DDT3, DDT4, DDT5, DDT6, DDT7, DDCN, };
                            list.ForEach(g =>
                            {
                                if (g.date == T2.ToString("dd-MM-yyyy")) dembd[0]++;
                                if (g.date == T3.ToString("dd-MM-yyyy")) dembd[1]++;
                                if (g.date == T4.ToString("dd-MM-yyyy")) dembd[2]++;
                                if (g.date == T5.ToString("dd-MM-yyyy")) dembd[3]++;
                                if (g.date == T6.ToString("dd-MM-yyyy")) dembd[4]++;
                                if (g.date == T7.ToString("dd-MM-yyyy")) dembd[5]++;
                                if (g.date == CN.ToString("dd-MM-yyyy")) dembd[6]++;
                            });
                            for (int i = 0; i < col.Count; i++)
                            {
                                if (dembd[i] > 0) col[i].Visibility = Visibility.Visible;
                                if (dembd[i] > 0 && dembd[i] < 5)
                                {
                                    Grid.SetRow(col[i], Grid.GetRow(col[i]) - dembd[i] + 1);
                                    Grid.SetRowSpan(col[i], dembd[i]);
                                }
                                if (dembd[i] > 0 && dembd[i] >= 5)
                                {
                                    Grid.SetRow(col[i], Grid.GetRow(col[i]) - 5 + 1);
                                    Grid.SetRowSpan(col[i], 5);
                                }
                            }
                        }
                    }));
                }
            }));
        }
        //
        public DateTime StartOfWeek(DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }
        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.ActualWidth > 910)
            {
                IsSmallSize = 0;
            }
            else
            {
                IsSmallSize = 1;
            }

        }
        private void DataGrid_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Main.scrollMain.OnPreviewMouseWheel(sender, e);
        }
    }
}
