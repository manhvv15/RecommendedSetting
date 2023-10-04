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
using QuanLyChung365TruocDangNhap.ChamCong365.Entities.History_Attenden_Staff;
using System.Net;

namespace QuanLyChung365TruocDangNhap.ChamCong365.Views.Pages.Xuat_Cong
{
    /// <summary>
    /// Interaction logic for Xuat_Cong_NhanVien.xaml
    /// </summary>
    public partial class Xuat_Cong_NhanVien : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public Xuat_Cong_NhanVien(MainWindow main)
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
        public MainWindow Main { get; set; }
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

        private List<BangCong> _listCongShow;
        public List<BangCong> listCongShow
        {
            get { return _listCongShow; }
            set { _listCongShow = value; OnPropertyChanged(); }
        }

        private List<Shift_Manage_Item> _ItemsShift;
        public List<Shift_Manage_Item> ShiftItems
        {
            get { return _ItemsShift; }
            set
            {
                _ItemsShift = value;
                OnPropertyChanged();
            }
        }
        //data
        private async Task<API_BangCong_List> getBangCong(int page = 1)
        {
            try
            {
                HttpClient http = new HttpClient();
                http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Main.APIStaff.data.data.access_token);

                var apilink = "http://210.245.108.202:3000/api/qlc/timekeeping/com/success";
                var pram = new Dictionary<string, string>();

                pram.Add("com_id", Main.APIStaff.data.data.user_info.com_id);
                pram.Add("ep_id", Main.APIStaff.data.data.user_info.ep_id);

                if (dpStart.SelectedDate != null) pram.Add("inputOld", dpStart.SelectedDate.Value.ToString("yyyy/MM/dd"));
                if (dpEnd.SelectedDate != null) pram.Add("inputNew", dpEnd.SelectedDate.Value.ToString("yyyy/MM/dd"));

                if (page <= 0) page = 1;
                var offset = (page - 1) * 20;
                pram.Add("pageNumber", page.ToString());
                pram.Add("pageSize", "20");

                if (pram.Count > 0)
                {
                    apilink += "?" + string.Join("&", pram);
                }

                HttpResponseMessage respon = await http.PostAsync(apilink, new FormUrlEncodedContent(pram));
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
        private void getXuatCong()
        {
            try
            {
                using (WebClient http = new WebClient())
                {
                    var apilink = "https://chamcong.24hpay.vn/service/export_now_pc.php";
                    var pram = new List<string>();
                    http.Headers.Add("Authorization", Main.APIStaff.data.data.access_token);
                    http.QueryString.Add("id_com", Main.APIStaff.data.data.user_info.com_id);
                    http.QueryString.Add("id_dep", Main.APIStaff.data.data.user_info.dep_id);
                    http.QueryString.Add("id_ep", Main.APIStaff.data.data.user_info.ep_id);

                    if (dpStart.SelectedDate != null) http.QueryString.Add("start_date", dpStart.SelectedDate.Value.ToString("yyyy-MM-dd"));
                    if (dpEnd.SelectedDate != null) http.QueryString.Add("end_date", dpEnd.SelectedDate.Value.ToString("yyyy-MM-dd"));
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
            var pram = new Dictionary<string, string>();

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
                getBangCong().ContinueWith(tt => this.Dispatcher.Invoke(() =>
                {
                    if (tt.Result != null)
                    {
                        listCongShow = listCong = tt.Result.data.items;
                        pagin.TotalRecords = tt.Result.data.totalItems;
                    }
                })),
                getCa().ContinueWith(tt=>this.Dispatcher.Invoke(()=>{
                    if (tt.Result!=null)
                    {
                        ShiftItems=tt.Result.data.items;
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
                        this.Dispatcher.Invoke(() => loading.Visibility = Visibility.Collapsed);
                    }
                });
            });

        }
        private void loadBangCong()
        {
            if (listCong != null)
            {
                var list = new List<BangCong>();
                var s = (pagin.SelectedPage - 1) * 20;
                for (int i = s; i < s + 20; i++)
                {
                    if (i >= 0 && i < listCong.Count)
                    {

                        list.Add(listCong[i]);
                    }
                }
                listCongShow = list;
            }
        }
        //
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

                for (int i = 0; i < max; i++)
                {
                    ws.Cells["A3"].Offset(0, 9+i).Value = "Thời gian";
                }

                ws.Cells["A1"].Value = "CHI TIẾT CHẤM CÔNG CÔNG TY " + Main.APIStaff.data.data.user_info.com_name.ToUpper();
                ws.Cells[1,1,1,9+max].Merge = true;
                ws.Row(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ws.Row(1).Style.Font.Bold = true;
                ws.Row(1).Style.Font.Size = 13;

                ws.Cells["A2"].Value = string.Format("Từ ngày {0} đến ngày {1}", startDate, endDate);
                ws.Cells[2, 1, 2, 9 + max].Merge = true;
                ws.Row(2).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ws.Row(2).Style.Font.Bold = true;
                ws.Row(2).Style.Font.Size = 13;

                ws.Row(3).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ws.Row(3).Style.Font.Bold = true;
                ws.Row(3).Style.Font.Size = 13;
                
                for (int i = 0; i < data.Count; i++)
                {
                    ws.Row(i + 4).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Row(i + 4).Style.Font.Size = 13;
                }

                for (int i = 0; i < data.Count; i++)
                {
                    ws.Cells[4 + i, 1].Value = data[i].ep_id;
                    ws.Cells[4 + i, 2].Value = data[i].ep_name;
                    ws.Cells[4 + i, 3].Value = data[i].ts_date;
                    ws.Cells[4 + i, 4].Value = data[i].day_of_week;
                    ws.Cells[4 + i, 5].Value = data[i].num_to_calculate;
                    ws.Cells[4 + i, 6].Value = data[i].late;
                    ws.Cells[4 + i, 7].Value = data[i].early;
                    ws.Cells[4 + i, 8].Value = data[i].total_time;
                    ws.Cells[4 + i, 9].Value = data[i].num_overtime;
                    for (int j = 0; j < data[i].lst_time.Count; j++)
                    {
                        ws.Cells[4 + i, 10 + j].Value = DateTime.Parse(data[i].lst_time[j]).ToString("hh:mm tt");
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
            var mm = (dpEnd.SelectedDate.Value - dpStart.SelectedDate.Value).Days;
            List<LichLamViec> lich = new List<LichLamViec>();
            bool flag = true;
            getXuatCong();/*.ContinueWith(tt => this.Dispatcher.Invoke(() =>
            {
                if (tt.Result != null)
                {
                    var page = tt.Result.data.totalItems / 20;
                    if (tt.Result.data.totalItems % 20 > 0) page++;
                    List<Task> k = new List<Task>();
                    for (int i = 1; i <= page; i++)
                    {
                        k.Add(getBangCong(i).ContinueWith(zz => this.Dispatcher.Invoke(() => { if (tt.Result != null) list.AddRange(zz.Result.data.items); })));
                    }
                    for (int i = 0; i < mm; i++)
                    {
                        k.Add(getLichLamViec(Main.APIStaff.data.data.user_info.ep_id, dpStart.SelectedDate.Value.AddDays(i).ToString("yyyy-MM-dd")).ContinueWith(dd => this.Dispatcher.Invoke(() =>
                        {
                            if (dd.Result != null)
                            {
                                if (!lich.Any(x => x.apply_month == dd.Result.data.item.apply_month)) lich.Add(dd.Result.data.item);
                            }
                        })));
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
                                    list = list.OrderBy(l => l.date).ToList();

                                    list.ForEach(i =>
                                    {
                                        BangCong bg = new BangCong();

                                        float cong = 0;
                                        CyDetail cy_Detail = new CyDetail();
                                        lich.ForEach(l =>
                                        {
                                            var month = DateTime.Parse(l.apply_month).Month;
                                            var year = DateTime.Parse(l.apply_month).Year;
                                            if (month == i.date.Month && year == i.date.Year)
                                            {
                                                l.cy_detail.ForEach(cy =>
                                                {
                                                    if (cy.date == i.date.ToString("yyyy-MM-dd"))
                                                    {
                                                        cy_Detail = cy;
                                                    }
                                                });
                                            }
                                        });
                                        cy_Detail.list_shift_id.ToList().ForEach(shift =>
                                        {
                                            bool vao = false;
                                            bool ra = false;
                                            var ft = ShiftItems.Find(x => x.shift_id == shift);
                                            if (ft != null)
                                            {
                                                DateTime starttime, startlate, endtime, endlate;
                                                if (DateTime.TryParse(ft.start_time, out starttime) && DateTime.TryParse(ft.start_time_latest, out startlate) && DateTime.TryParse(ft.end_time_earliest, out endlate))
                                                {
                                                    for (int j = 0; j < i.lst_time.Count; j++)
                                                    {
                                                        if (DateTime.Parse(i.lst_time[j]) >= startlate && DateTime.Parse(i.lst_time[j]) <= starttime)
                                                        {
                                                            vao = true;
                                                            //bg.lst_time.RemoveAt(j);
                                                            break;
                                                        }
                                                        if (DateTime.Parse(i.lst_time[j]) > starttime && DateTime.Parse(i.lst_time[j]) <= endlate)
                                                        {
                                                            ra = true;
                                                            //bg.lst_time.RemoveAt(j);
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                            if (vao) cong += float.Parse(ft.num_to_calculate);
                                            if (ra) cong += float.Parse(ft.num_to_calculate);
                                        });
                                        data.Add(new ExcelData
                                        {
                                            ep_id = i.ep_id,
                                            ep_name = i.ep_name,
                                            ts_date = i.ts_date,
                                            day_of_week = i.day_of_week,
                                            num_to_calculate = double.Parse(cong > 0 && cong < 10 ? "0," + cong.ToString() : (cong / 10).ToString()),
                                            late = 0,
                                            early = 0,
                                            total_time = 0,
                                            num_overtime = 0,
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
                            }

                        });
                    });
                }
            }));*/
        }

        private void table_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Main.scrollMain.OnPreviewMouseWheel(sender, e);
        }

        private void btnGet_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            loading.Visibility = Visibility.Visible;
            getBangCong().ContinueWith(tt => this.Dispatcher.Invoke(() =>
            {
                if (tt.Result != null)
                {
                    listCongShow = listCong = tt.Result.data.items;
                    pagin.TotalRecords = tt.Result.data.totalItems;
                    loading.Visibility = Visibility.Collapsed;
                }
            }));
        }

        private void pagin_SelectionChange(object sender, SelectionChangedEventArgs e)
        {
            //if (listCong.Count < pagin.TotalRecords)
            //{
            //    getBangCong(pagin.SelectedPage).ContinueWith(tt => this.Dispatcher.Invoke(() =>
            //    {
            //        if (tt.Result.data.items != null)
            //        {
            //            listCong.AddRange(tt.Result.data.items);
            //            listCong = listCong.ToList();
            //            loadBangCong();
            //        }
            //    }));
            //}
            //else loadBangCong();

            loading.Visibility = Visibility.Visible;
            getBangCong(pagin.SelectedPage).ContinueWith(tt => this.Dispatcher.Invoke(() =>
            {
                if (tt.Result.data.items != null)
                {
                    //listCong.AddRange(tt.Result.data.items);
                    listCongShow = listCong = tt.Result.data.items;
                    loading.Visibility = Visibility.Collapsed;
                }
            }));
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
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
    }
}
