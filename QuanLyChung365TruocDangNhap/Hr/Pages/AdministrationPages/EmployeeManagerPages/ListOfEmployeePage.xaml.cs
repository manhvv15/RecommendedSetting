using QuanLyChung365TruocDangNhap.Hr.Entities.AdministrationEntity.PersonnelChangeEntity;
using QuanLyChung365TruocDangNhap.Hr.Entities.HomeEntity;
using QuanLyChung365TruocDangNhap.Hr.Popups.AdministrationPopups.ListEmployee;
using QuanLyChung365TruocDangNhap.Hr.View;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reactive;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
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
using static QRCoder.PayloadGenerator;
using static System.Net.WebRequestMethods;
using Delete = QuanLyChung365TruocDangNhap.Hr.Popups.AdministrationPopups.ListEmployee.Delete;

namespace QuanLyChung365TruocDangNhap.Hr.Pages.AdministrationPages.EmployeeManagerPages
{
    /// <summary>
    /// Interaction logic for ListOfEmployeePage.xaml
    /// </summary>
    /// //id công ty 3310
    public partial class ListOfEmployeePage : Page, INotifyPropertyChanged
    {
        string token;
        string id; // id công ty
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
        public Dictionary<string, string> listDepartment = new Dictionary<string, string>();
        public Dictionary<string, string> listBranch = new Dictionary<string, string>();
        public Dictionary<string, string> getListEmployeess = new Dictionary<string, string>();


        bool perAdd, perEdit, perDel; // quyền thêm, sửa, xóa

        public bool PerAdd
        {
            get { return perAdd; }
            set
            {
                perAdd = value;
                OnPropertyChanged("PerAdd");
            }
        }

        public bool PerEdit
        {
            get { return perEdit; }
            set
            {
                perEdit = value;
                OnPropertyChanged("PerEdit");
            }
        }

        public bool PerDel
        {
            get { return perDel; }
            set
            {
                perDel = value;
                OnPropertyChanged("PerDel");
            }
        }
       

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string newName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(newName));
            }
        }

        // deligate event show popups
        public delegate void ShowPopup(object obj);

        public static event ShowPopup onShowPopup;

        public ListOfEmployeePage(string token, string id, bool perAdd, bool perEdit, bool perDel)
        {
            InitializeComponent();
            this.token = token;
            this.id = id;
            this.PerAdd = perAdd;
            this.PerEdit = perEdit;
            this.PerDel = perDel;
            GetListEmployee();
            GetListDepartment();
            GetAllBranch();
            DataContext = this;
            TotalPage.Text = total_page2.ToString();
            Delete.hidePopup += HidePopup;
            Detail.hidePopup += HidePopup;
            Detail2.hidePopup += HidePopup;

            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy"; //for the second type
            Thread.CurrentThread.CurrentCulture = ci;
        }
        ///lấy danh sách nhân viên
        private async void GetListEmployee()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.getlistEmplouyee;

                httpRequestMessage.RequestUri = new Uri(api);
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);
                Dictionary<string, string> form = new Dictionary<string, string>();
                form.Add("com_id", id);
                httpRequestMessage.Content = new FormUrlEncodedContent(form);
                var response = await httpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                getListEmployee result = JsonConvert.DeserializeObject<getListEmployee>(responseContent);


                if (result.data == null) return;
                getListEmployeess.Add("", "Chọn nhân viên");
                foreach (var item in result.data.items)
                {
                    //string dep_name = " (Chưa cập nhật)";
                    //if (item.dep_name != null && item.dep_name != "")
                    //{
                    //    dep_name = " (" + item.dep_name + ")";
                    //}
                    //item.ep_name = item.ep_name + dep_name;
                    getListEmployeess.Add(item.ep_id, item.ep_name);
                }
                cbxTenDoiTuong.Items.Refresh();
                cbxTenDoiTuong.ItemsSource = getListEmployeess;
                GetData();
            }
            catch (Exception)
            {
                MessageBox.Show("Đã xảy ra lỗi,vui lòng kiểm tra lại!");
            }
        }
        //lấy danh sách phòng ban
        private async void GetListDepartment()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = "http://210.245.108.202:3000/api/qlc/department/list";
                httpRequestMessage.RequestUri = new Uri(api);
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);
                Dictionary<string, string> form = new Dictionary<string, string>();
                form.Add("com_id", id);
                httpRequestMessage.Content = new FormUrlEncodedContent(form);
                var response = await httpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                listDepartment result = JsonConvert.DeserializeObject<listDepartment>(responseContent);
                if (result.data != null && result.data.items.Count > 0)
                {
                    //listDepartment.Add("", "Bộ phận");
                    listDepartment.Add("", "BAN GIÁM ĐỐC");
                   
                    List<Items6> listItem = result.data.items;
                    foreach (var item in listItem)
                    {
                        listDepartment.Add(item.dep_id, item.dep_name);
                    }
                    cbxDepartment.Items.Refresh();
                    cbxDepartment.ItemsSource = listDepartment;
                    GetData();
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        //lấy dữ liệu
        private async void GetData(int lenght = 10)
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();

                httpRequestMessage.Method = HttpMethod.Post;
                
                string api = "http://210.245.108.202:3000/api/qlc/managerUser/list";
                Dictionary<string, string> form = new Dictionary<string, string>();
                if (cbxDepartment.SelectedIndex > 0)
                {
                    form.Add("dep_id", cbxDepartment.SelectedValue.ToString());
                }
                if (cbxTenDoiTuong.SelectedIndex > 0)
                {
                    form.Add("ep_id", cbxTenDoiTuong.SelectedValue.ToString());
                }
                form.Add("pageNumber", page_now2.ToString());
                form.Add("pageSize", lenght.ToString());
                httpRequestMessage.RequestUri = new Uri(api);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                httpRequestMessage.Content = new FormUrlEncodedContent(form);
                var response = await httpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                getListEmployee result = JsonConvert.DeserializeObject<getListEmployee>(responseContent);
                if (result.data != null)
                {
                    total2 = result.data.totalItems;
                    Paging2();
                    List<Items3> ListEmployee = result.data.items;
                    foreach (var item in ListEmployee)
                    {
                        if (item.dep_id != null && listDepartment.ContainsKey(item.dep_id))
                        {
                            item.dep_name = listDepartment[item.dep_id];
                        }
                        else
                        {
                            item.dep_name = "Chưa cập nhật";
                        }

                        string dep_name = " (Chưa cập nhật)";
                        if (item.dep_name != null && item.dep_name != "")
                        {
                            dep_name = " (" + item.dep_name + ")";
                        }
                        item.ep_name = item.ep_name + dep_name;
                        System.DateTime dateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);

                        // Add the number of seconds in UNIX timestamp to be converted.
                        dateTime = dateTime.AddSeconds(long.Parse(item.create_time));
                        item.create_time = dateTime.ToString("dd-MM-yyyy");
                    }
                    icListEmployee.Items.Refresh();
                    icListEmployee.ItemsSource = ListEmployee;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Đã xảy ra lỗi,vui lòng kiểm tra lại!");
            }
        }
        private string ConvertDate(string date, string format)
        {
            DateTime myDate;
            try
            {
                myDate = DateTime.ParseExact(date, "dd-MM-yyyyTHH:mm",
                                              System.Globalization.CultureInfo.InvariantCulture);
                return myDate.ToString(format);
            }
            catch
            {
                try
                {
                    myDate = DateTime.ParseExact(date, "dd-MM-yyyy",
                                                  System.Globalization.CultureInfo.InvariantCulture);
                    return myDate.ToString(format);
                }
                catch
                {
                    try
                    {
                        myDate = DateTime.ParseExact(date, "dd-MM-yyyy",
                                                      System.Globalization.CultureInfo.InvariantCulture);
                        return myDate.ToString(format);
                    }
                    catch
                    {
                        try
                        {
                            myDate = DateTime.ParseExact(date, "dd/MM/yyyy",
                                                          System.Globalization.CultureInfo.InvariantCulture);
                            return myDate.ToString(format);
                        }
                        catch
                        {
                            try
                            {
                                myDate = DateTime.ParseExact(date, "dd/MM/yyyy",
                                                              System.Globalization.CultureInfo.InvariantCulture);
                                return myDate.ToString(format);
                            }
                            catch
                            {
                                try
                                {
                                    myDate = DateTime.ParseExact(date, "d/M/yyyy",
                                                                  System.Globalization.CultureInfo.InvariantCulture);
                                    return myDate.ToString(format);
                                }
                                catch
                                {
                                    return "";
                                }
                            }
                        }
                    }
                }
            }

        }

        //lấy danh sách chi nhánh
        private async void GetAllBranch()
        {
            try
            {
                var httpClient = new HttpClient();

                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.all_branch;

                httpRequestMessage.RequestUri = new Uri(api);
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);
                var response = await httpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                listCompany result = JsonConvert.DeserializeObject<listCompany>(responseContent);
                if (result.data != null && result.data.items.Count > 0)
                {
                    listBranch.Add("", "Chọn chi nhánh");
                    foreach (var item in result.data.items)
                    {
                        listBranch.Add(item.com_id, item.com_name);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Đã xảy ra lỗi,vui lòng kiểm tra lại!");
            }
        }

        private void HidePopup(int mode)
        {
            if (mode == 1)
            {
                GetData();
            }
            onShowPopup("");
        }

        private void tbSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                PageNow2 = "1";
                GetData();
            }
        }

       

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
        private void LinkWebAddEmployee(object sender, MouseButtonEventArgs e)
        {
            HomeView Main = new HomeView();
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(HomeView))
                {
                    Main = (HomeView)window;
                }
            };
            if (Main.TypeUser == 1)
            {
                Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=81b016d57ec189daa8e04dd2d59a22c3." + Main.EpId + "." + Main.pass + "&link=" + "https://chamcong.timviec365.vn/quan-ly-cong-ty.html");
            }
            else
            {
                Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=f30f0b61e761b8926941f232ea7cccb9." + Main.Id + "." + Main.pass + "&link=" + "https://chamcong.timviec365.vn/quan-ly-cong-ty/nhan-vien.html");
            }
            e.Handled = true;
        }

        private void ClickSearch(object sender, MouseButtonEventArgs e)
        {
            PageNow2 = "1";
            GetData();
        }

        private void Page_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(this);
            double x = p.X;
            double y = p.Y;
            double left = popupOption.Margin.Left;
            double top = popupOption.Margin.Top;
            if ((x < left || x > (left + popupOption.ActualWidth)) && (y < top || y > top + popupOption.ActualHeight))
            {
                popupOption.Visibility = Visibility.Collapsed;
            }
        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            Grid grid = sender as Grid;
            grid.Children[0].Visibility = Visibility.Visible;
            TextBlock textBlock = (TextBlock)grid.Children[1];
            var converter = new BrushConverter();
            textBlock.Foreground = (Brush)converter.ConvertFromString("#4C5BD4");
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            Grid grid = sender as Grid;
            grid.Children[0].Visibility = Visibility.Collapsed;
            TextBlock textBlock = (TextBlock)grid.Children[1];
            var converter = new BrushConverter();
            textBlock.Foreground = (Brush)converter.ConvertFromString("#474747");
        }

        private void ShowPopupOption(object sender, MouseButtonEventArgs e)
        {
            Path path = sender as Path;
            Point p = e.GetPosition(this);
            double x = p.X;
            double y = p.Y - 30;
            if (x + popupOption.Width > page.ActualWidth)
            {
                x = page.ActualWidth - popupOption.Width - 10;
            }
            Thickness thickness = new Thickness(x, y, 0, 0);
            popupOption.Margin = thickness;
            popupOption.Visibility = Visibility.Visible;
            popupOption.DataContext = path.DataContext;
        }

        private void ShowPopupDelete(object sender, MouseButtonEventArgs e)
        {
            Items3 dataEntity = (Items3)popupOption.DataContext;
            string id = dataEntity.ep_id;
            Delete delete = new Delete(token, id);
            onShowPopup(delete);
            popupOption.Visibility = Visibility.Collapsed;
        }

        private void cbxDepartment_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            cbxDepartment.SelectedIndex = -1;
            string textSearch = cbxDepartment.Text + e.Text;
            cbxDepartment.IsDropDownOpen = true;
            if (textSearch == "")
            {
                cbxDepartment.Text = "";
                cbxDepartment.Items.Refresh();
                cbxDepartment.ItemsSource = listDepartment;
                cbxDepartment.SelectedIndex = -1;
            }
            else
            {
                cbxDepartment.ItemsSource = "";
                cbxDepartment.Items.Refresh();
                cbxDepartment.ItemsSource = listDepartment.Where(t => t.Value.ToLower().Contains(textSearch.ToLower()));
            }
        }

        private void cbxDepartment_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                cbxDepartment.SelectedIndex = -1;
                string textSearch = cbxDepartment.Text;
                cbxDepartment.Items.Refresh();
                cbxDepartment.ItemsSource = listDepartment.Where(t => t.Value.ToLower().Contains(textSearch.ToLower()));
            }
        }
        private void ShowPopupEdit(object sender, MouseButtonEventArgs e)
        {
            Items3 dataEntity = (Items3)popupOption.DataContext;
            string ep_id = dataEntity.ep_id;
            Detail2 detail = new Detail2(token, ep_id, id, listBranch, listDepartment);
            onShowPopup(detail);
            popupOption.Visibility = Visibility.Collapsed;
        }

        private void ShowPopupDetail(object sender, MouseButtonEventArgs e)
        {
            Items3 dataEntity = (Items3)popupOption.DataContext;
            string ep_id = dataEntity.ep_id;
            Detail detail = new Detail(token, ep_id, id);
            onShowPopup(detail);
            popupOption.Visibility = Visibility.Collapsed;
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (popupOption.Visibility == Visibility.Visible)
            {
                popupOption.Visibility = Visibility.Collapsed;
            }
            else
            {
                Border border = sender as Border;
                Point p = e.GetPosition(this);
                double x = p.X;
                double y = p.Y - 30;
                if (x + popupOption.Width > page.ActualWidth)
                {
                    x = page.ActualWidth - popupOption.Width - 10;
                }
                Thickness thickness = new Thickness(x, y, 0, 0);
                popupOption.Margin = thickness;
                popupOption.Visibility = Visibility.Visible;
                popupOption.DataContext = border.DataContext;
            }
        }

        private void cbxTenDoiTuong_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            cbxTenDoiTuong.SelectedIndex = -1;
            string textSearch = cbxTenDoiTuong.Text + e.Text;
            cbxTenDoiTuong.IsDropDownOpen = true;
            if (textSearch == "")
            {
                cbxTenDoiTuong.Text = "";
                cbxTenDoiTuong.Items.Refresh();
                cbxTenDoiTuong.ItemsSource = getListEmployeess;
                cbxTenDoiTuong.SelectedIndex = -1;
            }
            else
            {
                cbxTenDoiTuong.ItemsSource = "";
                cbxTenDoiTuong.Items.Refresh();
                cbxTenDoiTuong.ItemsSource = getListEmployeess.Where(t => t.Value.ToLower().Contains(textSearch.ToLower()));
            }
        }

        private void cbxTenDoiTuong_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                cbxTenDoiTuong.SelectedIndex = -1;
                string textSearch = cbxTenDoiTuong.Text;
                cbxTenDoiTuong.Items.Refresh();
                cbxTenDoiTuong.ItemsSource = getListEmployeess.Where(t => t.Value.ToLower().Contains(textSearch.ToLower()));
            }
        }
        private void select_pagani(object sender, SelectionChangedEventArgs e)
        {
            GetData((int)cbxPage.SelectedItem);
        }

        private void scroll1_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                scroll1.ScrollToVerticalOffset(scroll1.VerticalOffset);
                scroll.ScrollToHorizontalOffset(scroll.HorizontalOffset - e.Delta);
            }
            else
                scroll1.ScrollToVerticalOffset(scroll1.VerticalOffset - e.Delta);
        }

        private List<int> _Records = new List<int>() { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
        public List<int> Records
        {
            get { return _Records; }
        }
    }
}
