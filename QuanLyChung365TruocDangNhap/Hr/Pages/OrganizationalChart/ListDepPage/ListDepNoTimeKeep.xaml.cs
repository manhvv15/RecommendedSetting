using QuanLyChung365TruocDangNhap.Hr.Entities.AdministrationEntity.PersonnelChangeEntity;
using QuanLyChung365TruocDangNhap.Hr.Entities.OrganizationalChartEntities;
using QuanLyChung365TruocDangNhap.Hr.View;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
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

namespace QuanLyChung365TruocDangNhap.Hr.Pages.OrganizationalChart.ListDepPage
{
    /// <summary>
    /// Interaction logic for ListDepNoTimeKeep.xaml
    /// </summary>
    public partial class ListDepNoTimeKeep : Page, INotifyPropertyChanged
    {
        string token;
        string id;
        string dep_id;
        string dep_name;

        const int COUNT_RECORD_PER_PAGE2 = 10;
        int page_now2 = 1;
        int total2;
        int total_page2 = 1;
        public string PageNow2
        {
            get { return page_now2.ToString(); }
            set
            {
                page_now2 = Convert.ToInt32(value);
                OnPropertyChanged("PageNow2");
            }
        }

        // deligate event show popups
        public delegate void ShowPopup(object obj);
        public static event ShowPopup onShowPopup;


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string newName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(newName));
            }
        }
        public ListDepNoTimeKeep(string token, string id, string dep_id, string dep_name)
        {
            InitializeComponent();
            this.token = token;
            this.id = id;
            this.dep_id = dep_id;
            this.dep_name = dep_name;
            this.DataContext = this;
            txtDepname.Text = dep_name;
            TotalPage.Text = total_page2.ToString();
            GetData();
        }
        private void NavigateBack(object sender, MouseButtonEventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(HomeView))
                {
                    (window as HomeView).MainContent.NavigationService.GoBack();
                }
            }
        }

        private static string HtmlToPlainText(string html)
        {
            const string tagWhiteSpace = @"(>|$)(\W|\n|\r)+<";//matches one or more (white space or line breaks) between '>' and '<'
            const string stripFormatting = @"<[^>]*(>|$)";//match any character between '<' and '>', even when end tag is missing
            const string lineBreak = @"<(br|BR)\s{0,1}\/{0,1}>";//matches: <br>,<br/>,<br />,<BR>,<BR/>,<BR />
            var lineBreakRegex = new Regex(lineBreak, RegexOptions.Multiline);
            var stripFormattingRegex = new Regex(stripFormatting, RegexOptions.Multiline);
            var tagWhiteSpaceRegex = new Regex(tagWhiteSpace, RegexOptions.Multiline);

            var text = html;
            //Decode html specific characters
            text = System.Net.WebUtility.HtmlDecode(text);
            //Remove tag whitespace/line breaks
            text = tagWhiteSpaceRegex.Replace(text, "><");
            //Replace <br /> with line breaks
            text = text.Replace("</br>", "\n");
            text = stripFormattingRegex.Replace(text, string.Empty);

            return text;
        }
        //get list employee

        private async void GetData()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();

                httpRequestMessage.Method = HttpMethod.Post;
                string api = "http://210.245.108.202:3006/api/hr/organizationalStructure/listEmUntimed";

                httpRequestMessage.RequestUri = new Uri(api);
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);
                var form = new Dictionary<string, string>();
                form.Add("type_timekeep", "2");
                form.Add("winform", "1");
                form.Add("dep_id", dep_id);
                form.Add("com_id", id);
                form.Add("page", page_now2.ToString());
                form.Add("pageSize", COUNT_RECORD_PER_PAGE2.ToString());
                httpRequestMessage.Content = new FormUrlEncodedContent(form);
                var response = await httpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                getListEmployee result = JsonConvert.DeserializeObject<getListEmployee>(responseContent);
                if (result.data != null)
                {
                    total2 = result.data.totalItems;
                    Paging2();
                    foreach (Items3 item in result.data.items)
                    {
                        if (item.dep_name == null)
                        {
                            item.dep_name = "Chưa cập nhật";
                        }

                        item.ep_birth_day = Convert.ToDateTime(item.ep_birth_day).ToString("dd-MM-yyyy");

                        //var httpRequestMessageLichLamViec = new HttpRequestMessage();
                        //httpRequestMessageLichLamViec.Method = HttpMethod.Post;
                        //httpRequestMessageLichLamViec.RequestUri = new Uri("https://chamcong.24hpay.vn/service/detail_cycle.php?id_ep=" + item.ep_id + "&month_apply=" + DateTime.Now.ToString("yyyy-MM-dd"));
                        //httpRequestMessageLichLamViec.Headers.Add("Authorization", token);

                        //var responseLichLamViec = await httpClient.SendAsync(httpRequestMessageLichLamViec);
                        //DetailCycle resultLichLamViec = JsonConvert.DeserializeObject<DetailCycle>(responseLichLamViec.Content.ReadAsStringAsync().Result);
                        //if (resultLichLamViec.data != null)
                        //{
                        //    int index = resultLichLamViec.data.item.cy_detail.FindIndex(lich => lich.date.Equals(DateTime.Now.ToString("yyyy-MM-dd")) && !String.IsNullOrEmpty(lich.shift_id.Trim()));
                        //    if (index > -1)
                        //    {
                        //        string CaHienTai = "";
                        //        string[] listCa = resultLichLamViec.data.item.cy_detail[index].shift_id.Split(',');
                        //        foreach (string ca in listCa)
                        //        {
                        //            var httpRequestMessageChiTietCa = new HttpRequestMessage();
                        //            httpRequestMessageChiTietCa.Method = HttpMethod.Post;
                        //            httpRequestMessageChiTietCa.RequestUri = new Uri("https://chamcong.24hpay.vn/api_web_hr/detailShift.php?id=" + ca + "&id_com=" + id);

                        //            var responseChiTietCa = await httpClient.SendAsync(httpRequestMessageChiTietCa);
                        //            detailShiftEntities resultChiTietCa = JsonConvert.DeserializeObject<detailShiftEntities>(responseChiTietCa.Content.ReadAsStringAsync().Result);
                        //            if (resultChiTietCa.data != null)
                        //            {

                        //                string CurrentTime = "";

                        //                if (DateTime.Now.Hour < 10)
                        //                {
                        //                    CurrentTime = "0" + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
                        //                }
                        //                else if (DateTime.Now.Minute < 10)
                        //                {
                        //                    CurrentTime = DateTime.Now.Hour + ":" + "0" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
                        //                }
                        //                else if (DateTime.Now.Minute < 10)
                        //                {
                        //                    CurrentTime = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + "0" + DateTime.Now.Second;
                        //                }
                        //                else if (DateTime.Now.Hour < 10 && DateTime.Now.Minute < 10 && DateTime.Now.Minute < 10)
                        //                {
                        //                    CurrentTime = "0" + DateTime.Now.Hour + ":" + "0" + DateTime.Now.Minute + ":" + "0" + DateTime.Now.Second;
                        //                }
                        //                else
                        //                {
                        //                    CurrentTime = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
                        //                }

                        //                //string CurrentTime = "";

                        //                //CurrentTime = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second == "h:m:s" ? ("0" + DateTime.Now.Hour + ":" + "0" + DateTime.Now.Minute + ":" + "0" + DateTime.Now.Second) : (DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second);
                        //                //CurrentTime = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second == "h:m:ss" ? ("0" + DateTime.Now.Hour + ":" + "0" + DateTime.Now.Minute + ":" + DateTime.Now.Second) : (DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second);
                        //                //CurrentTime = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second == "h:mm:s" ? ("0" + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + "0" + DateTime.Now.Second) : (DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second);
                        //                //CurrentTime = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second == "h:mm:ss" ? ("0" + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second) : (DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second);
                        //                //string CurrentTime = "0" + DateTime.Now.Hour + ":" + "0" + DateTime.Now.Minute + ":" + "0" + DateTime.Now.Second;
                        //                string startTime = resultChiTietCa.data.detailShift.start_time_latest;
                        //                string endTime = resultChiTietCa.data.detailShift.end_time_earliest;
                        //                if (CurrentTime.CompareTo(startTime) > 0 && CurrentTime.CompareTo(endTime) < 0)
                        //                {
                        //                    CaHienTai = ca;
                        //                    break;
                        //                }
                        //            }
                        //        }
                        //        if (String.IsNullOrEmpty(CaHienTai))
                        //        {
                        //            item.noi_dung = "Nghỉ theo lịch làm việc";
                        //        }
                        //        else
                        //        {
                        //            var httpRequestMessageDeXuat = new HttpRequestMessage();
                        //            httpRequestMessageDeXuat.Method = HttpMethod.Post;
                        //            httpRequestMessageDeXuat.RequestUri = new Uri("https://vanthu.timviec365.vn/api/hr365_get_de_xuat.php");

                        //            var parameters = new List<KeyValuePair<string, string>>();
                        //            parameters.Add(new KeyValuePair<string, string>("id_user", item.ep_id));
                        //            parameters.Add(new KeyValuePair<string, string>("date", DateTime.Now.ToString("yyyy-MM")));
                        //            httpRequestMessageDeXuat.Content = new FormUrlEncodedContent(parameters);
                        //            var responseDeXuat = await httpClient.SendAsync(httpRequestMessageDeXuat);
                        //            ProposeEntities resultDeXuat = JsonConvert.DeserializeObject<ProposeEntities>(responseDeXuat.Content.ReadAsStringAsync().Result);

                        //            if (resultDeXuat.data != null)
                        //            {
                        //                if (resultDeXuat.data.totalItems.Equals("0"))
                        //                {
                        //                    item.noi_dung = "Nghỉ sai quy định";
                        //                }
                        //                else
                        //                {
                        //                    ThongTinDeXuat deXuat = new ThongTinDeXuat();
                        //                    bool CoDeXuat = false;
                        //                    foreach (ProposeEntity itemDeXuat in resultDeXuat.data.items)
                        //                    {
                        //                        deXuat = JsonConvert.DeserializeObject<ThongTinDeXuat>(itemDeXuat.noi_dung);
                        //                        foreach (NDDeXuat nd in deXuat.nd)
                        //                        {
                        //                            int StartDay = Convert.ToDateTime(nd.ngaybatdau_nghi).Day;
                        //                            int EndDay = Convert.ToDateTime(nd.ngayketthuc_nghi).Day;
                        //                            if (DateTime.Now.Day >= StartDay && DateTime.Now.Day <= EndDay)
                        //                            {
                        //                                if (CaHienTai.Equals(nd.ca_nghi) || String.IsNullOrEmpty(nd.ca_nghi))
                        //                                {
                        //                                    item.xem_chi_tiet = "(Xem chi tiết đề xuất)";
                        //                                    item.noi_dung = HtmlToPlainText(deXuat.ly_do);
                        //                                    item.link_dx = itemDeXuat.link;
                        //                                    CoDeXuat = true;
                        //                                }
                        //                                break;
                        //                            }
                        //                        }
                        //                        if (CoDeXuat)
                        //                        {
                        //                            break;
                        //                        }
                        //                    }
                        //                    if (!CoDeXuat)
                        //                    {
                        //                        item.noi_dung = "Nghỉ sai quy định";
                        //                    }
                        //                }
                        //            }
                        //            else
                        //            {
                        //                item.noi_dung = "Chưa thiết lập lịch làm việc";
                        //            }
                        //        }
                        //    }
                        //    else
                        //    {
                        //        item.noi_dung = "Nghỉ theo lịch làm việc";
                        //    }
                        //}
                        //else
                        //{
                        //    item.noi_dung = "Chưa thiết lập lịch làm việc";
                        //}
                    }
                    icListEmployee.Items.Refresh();
                    icListEmployee.ItemsSource = result.data.items;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Đã xảy ra lỗi,vui lòng kiểm tra lại!");
            }
        }
        //end get list employee
        private void NextPage2(object sender, MouseButtonEventArgs e)
        {
            InvalidBtn2();
            int page_next = page_now2 + 1;
            PageNow2 = page_next.ToString();
            GetData();
        }

        private void PrevPage2(object sender, MouseButtonEventArgs e)
        {
            InvalidBtn2();
            int page_prev = page_now2 - 1;
            PageNow2 = page_prev.ToString();
            GetData();
        }

        private void InvalidBtn2()
        {
            btnPrev2.IsEnabled = false;
            btnPrev2.Opacity = 0.3;
            btnNext2.IsEnabled = false;
            btnNext2.Opacity = 0.3;
        }

        private void Paging2()
        {
            if (total2 == 0)
            {
                txtNoData.Visibility = Visibility.Visible;
                total_page2 = 1;
            }
            else
            {
                if (total2 % COUNT_RECORD_PER_PAGE2 == 0)
                {
                    total_page2 = total2 / COUNT_RECORD_PER_PAGE2;
                    TotalPage.Text = total_page2.ToString();
                }
                else
                {
                    total_page2 = total2 / COUNT_RECORD_PER_PAGE2 + 1;
                    TotalPage.Text = total_page2.ToString();
                }
            }
            IsSetValidBtn2();
        }

        private void IsSetValidBtn2()
        {
            if (page_now2 == 1)
            {
                btnPrev2.IsEnabled = false;
                btnPrev2.Opacity = 0.3;
            }
            else
            {
                btnPrev2.IsEnabled = true;
                btnPrev2.Opacity = 1;
            }

            if (page_now2 == total_page2)
            {
                btnNext2.IsEnabled = false;
                btnNext2.Opacity = 0.3;
            }
            else
            {
                btnNext2.IsEnabled = true;
                btnNext2.Opacity = 1;
            }
        }
        private void QLVT_click(object sender, MouseButtonEventArgs e)
        {
            /*Process.Start(new ProcessStartInfo("https://vanthu.timviec365.vn/trang-quan-ly-de-xuat.html"));*/
            TextBlock tb = sender as TextBlock;
            Items3 data = (Items3)tb.DataContext;
            HomeView Main = new HomeView();
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(HomeView))
                {
                    Main = (window as HomeView); break;
                }
            };
            if (Main.TypeUser == 1)
                Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=81b016d57ec189daa8e04dd2d59a22c3." + Main.EpId + "." + Main.pass + "&link=" + data.link_dx);
            else
                Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=f30f0b61e761b8926941f232ea7cccb9." + Main.Id + "." + Main.pass + "&link=" + data.link_dx);

        }

        private void scroll_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                scData.ScrollToVerticalOffset(scData.VerticalOffset);
                scroll.ScrollToHorizontalOffset(scroll.HorizontalOffset - e.Delta);
            }
        }
    }
}
