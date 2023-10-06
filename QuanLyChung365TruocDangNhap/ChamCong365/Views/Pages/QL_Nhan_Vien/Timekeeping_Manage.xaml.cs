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
using QuanLyChung365TruocDangNhap.ChamCong365.Entities.Staff;
using Newtonsoft.Json;

namespace QuanLyChung365TruocDangNhap.ChamCong365.Views.Pages.QL_Nhan_Vien
{
    /// <summary>
    /// Interaction logic for Timekeeping_Manage.xaml
    /// </summary>
    /// 
    public partial class Timekeeping_Manage : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public Timekeeping_Manage(MainWindow main)
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
        public class DiemDanhNhieuNgay
        {
            public string date { get; set; }
            public List<LichSuDiemDanh> data { get; set; }
        }
        private List<DiemDanhNhieuNgay> _listDiemDanhNhieuNgay;
        public List<DiemDanhNhieuNgay> listDiemDanhNhieuNgay
        {
            get { return _listDiemDanhNhieuNgay; }
            set { _listDiemDanhNhieuNgay = value; OnPropertyChanged(); }
        }
        //
        private async Task<API_LichSuDiemDanh_List> getDataDiemDanh(string startDate = "", string endDate = "", int page = 1)
        {
            HttpClient http = new HttpClient();
            var param = new Dictionary<string,string>();
            switch (Main.Type)
            {
                case 1:
                    http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Main.APIStaff.data.data.access_token);
                    param.Add("ep_id", Main.APIStaff.data.data.user_info.ep_id);
                    break;
                case 2:
                    http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Main.APICompany.data.data.access_token);
                    break;
                default:
                    break;
            }
            var apilink = "http://210.245.108.202:3000/api/qlc/timekeeping/get_history_time_keeping_by_company";
            if (!string.IsNullOrEmpty(startDate)) param.Add("start_date", startDate);
            if (!string.IsNullOrEmpty(endDate)) param.Add("end_date", endDate);
            param.Add("page", page.ToString());
            param.Add("pageSize", "20");
            HttpResponseMessage respon = await http.PostAsync(apilink, new FormUrlEncodedContent(param));
            API_LichSuDiemDanh_List api = JsonConvert.DeserializeObject<API_LichSuDiemDanh_List>(respon.Content.ReadAsStringAsync().Result);
            return api;
        }
        private async Task getData()
        {
            var today = DateTime.Today.ToString("yyyy-MM-dd");
            tblToday.Text = DateTime.Today.ToString("dddd dd/MM/yyyy");
            dpStart.SelectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dpEnd.SelectedDate = DateTime.Today;
            var day = (dpEnd.SelectedDate - dpStart.SelectedDate).Value.Days;

            getDataDiemDanh(today, today).ContinueWith(tt => this.Dispatcher.Invoke(() =>
            {
                if (tt.Result.data != null && tt.Result.data.items != null)
                {
                    listDiemDanh = tt.Result.data.items;
                }
            }));
            getDataDiemDanh(dpStart.SelectedDate.Value.ToString("yyyy-MM-dd"), dpEnd.SelectedDate.Value.ToString("yyyy-MM-dd")).ContinueWith(tt => this.Dispatcher.Invoke(() =>
            {
                if (tt.Result != null)
                {
                    pagin.TotalRecords = int.Parse(tt.Result.data.totalItems);
                    listDiemDanhNhieuNgay = loadDiemDanhNhieuNgay(day, tt.Result.data.items);
                }
            }));
        }
        private List<DiemDanhNhieuNgay> loadDiemDanhNhieuNgay(int day, List<LichSuDiemDanh> tt)
        {
            var diemdanh = new List<DiemDanhNhieuNgay>();
            for (int i = 0; i <= day; i++)
            {
                var selecteddate = dpStart.SelectedDate.Value.AddDays(i);
                var list = new List<LichSuDiemDanh>();
                tt.ForEach(j =>
                {
                    DateTime attime;
                    if (!string.IsNullOrEmpty(j.at_time) && DateTime.TryParse(j.at_time, out attime))
                    {
                        if (selecteddate.ToString("dddd dd/MM/yyyy") == attime.ToString("dddd dd/MM/yyyy"))
                        {
                            list.Add(j);
                        }
                    }
                });
                if (list.Count > 0) diemdanh.Add(new DiemDanhNhieuNgay()
                {
                    date = selecteddate.ToString("dddd dd/MM/yyyy"),
                    data = list
                });
            }
            diemdanh.Reverse();
            return diemdanh;
        }
        //
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
        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {

            if ((sender as Grid).ActualWidth < lv.ActualWidth + st.ActualWidth + 40)
            {
                DockPanel.SetDock(st, Dock.Top);
            }
            else DockPanel.SetDock(st, Dock.Right);
        }
        private void lv_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Main.scrollMain.OnPreviewMouseWheel(sender, e);
        }
        private void cal_SelectionChanged(object sernder, SelectionChangedEventArgs e)
        {
            if (cal.SelectedDate != null)
            {
                var day = cal.SelectedDate.Value.ToString("yyyy-MM-dd");
                tblToday.Text = cal.SelectedDate.Value.ToString("dddd dd/MM/yyyy");
                getDataDiemDanh(day, day).ContinueWith(tt => this.Dispatcher.Invoke(() =>
                {
                    if (tt.Result.data != null && tt.Result.data.items != null)
                    {
                        listDiemDanh = tt.Result.data.items;
                    }
                }));
            }
        }
        private void DiemDanhCacNgay(object sender, MouseButtonEventArgs e)
        {
            var x = dpStart.SelectedDate.Value.ToString("yyyy-MM-dd");
            var y = dpEnd.SelectedDate.Value.ToString("yyyy-MM-dd");
            var day = (dpEnd.SelectedDate - dpStart.SelectedDate).Value.Days;
            listDiemDanhNhieuNgay = new List<DiemDanhNhieuNgay>();
            getDataDiemDanh(x, y).ContinueWith(tt => this.Dispatcher.Invoke(() =>
            {
                if (tt.Result.data != null && tt.Result.data.items != null)
                {
                    pagin.TotalRecords = int.Parse(tt.Result.data.totalItems);
                    listDiemDanhNhieuNgay = loadDiemDanhNhieuNgay(day, tt.Result.data.items); ;
                }
            }));
        }
        private void pagin_SelectionChange(object sender, SelectionChangedEventArgs e)
        {
            var day = (dpEnd.SelectedDate - dpStart.SelectedDate).Value.Days;
            getDataDiemDanh(dpStart.SelectedDate.Value.ToString("yyyy-MM-dd"), dpEnd.SelectedDate.Value.ToString("yyyy-MM-dd"),pagin.SelectedPage).ContinueWith(tt => this.Dispatcher.Invoke(() =>
            {
                if (tt.Result != null)
                {
                    listDiemDanhNhieuNgay = loadDiemDanhNhieuNgay(day, tt.Result.data.items);
                }
            }));
        }
    }
}
