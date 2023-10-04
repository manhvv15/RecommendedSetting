﻿using QuanLyChung365TruocDangNhap.Hr.Entities.SalaryAndBenefits;
using QuanLyChung365TruocDangNhap.Hr.Popups.SarlaryAndBenefitPopups.BonusPopups;
using QuanLyChung365TruocDangNhap.Hr.View;
using HRApp.Popups.SarlaryAndBenefitPopups.BonusPopups;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
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

namespace QuanLyChung365TruocDangNhap.Hr.Pages.SalaryAndBenefitPages.BonusPages
{
    /// <summary>
    /// Interaction logic for PersonalBonusPage.xaml
    /// </summary>
    public partial class PersonalBonusPage : Page, INotifyPropertyChanged
    {
        const int COUNT_RECORD_PER_PAGE = 10;
        int page_now = 1;
        int total;
        int total_page = 1;
        string token;
        string com_id;
        int TypeUser;
        Dictionary<string, string> listAchivementCbx = new Dictionary<string, string>();

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

        // deligate event show popups
        public delegate void ShowPopup(object obj);
        public static event ShowPopup onShowPopup;
        public PersonalBonusPage(string token, string com_id,int TypeUser, bool perAdd, bool perEdit, bool perDel)
        {
            InitializeComponent();
            this.token = token;
            this.com_id = com_id;
            this.TypeUser = TypeUser;
            this.PerAdd = perAdd;
            this.PerEdit = perEdit;
            this.PerDel = perDel;
            GetData();
            listAchivementCbx.Add("1", "Huân chương");
            listAchivementCbx.Add("2", "Huy chương");
            listAchivementCbx.Add("3", "Giấy khen");
            listAchivementCbx.Add("4", "Thăng chức");
            listAchivementCbx.Add("5", "Kỉ niệm chương");
            listAchivementCbx.Add("6", "Tiền mặt");
            DataContext = this;

            AddListAchievementsPopup.hidePopup += HidePopup;
            EditAchievementsPopup.hidePopup += HidePopup;
        }

        private void NavigateToPage(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            string name = textBlock.Name;
            switch (name)
            {
                case "Personal":
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(HomeView))
                        {
                            (window as HomeView).MainContent.Navigate(new PersonalBonusPage(token, com_id, TypeUser, PerAdd, PerEdit, PerDel));
                        }
                    };
                    break;
                case "Group":
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(HomeView))
                        {
                            (window as HomeView).MainContent.Navigate(new GroupBonusPage(token, com_id, TypeUser, PerAdd, PerEdit, PerDel));
                        }
                    };
                    break;
                case "List":
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(HomeView))
                        {
                            (window as HomeView).MainContent.Navigate(new ListAchievementsPage(token, com_id, TypeUser, PerAdd, PerEdit, PerDel));
                        }
                    };
                    break;
            }

        }
        private void Paging()
        {
            if (total % COUNT_RECORD_PER_PAGE == 0)
            {
                total_page = total / COUNT_RECORD_PER_PAGE;
                TotalPage.Text = total_page.ToString();
            }
            else
            {
                total_page = total / COUNT_RECORD_PER_PAGE + 1;
                TotalPage.Text = total_page.ToString();
            }
            IsSetValidBtn();
        }

        public string PageNow
        {
            get { return page_now.ToString(); }
            set
            {
                page_now = Convert.ToInt32(value);
                OnPropertyChanged("PageNow");
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
        // mode = 0 : ko load lại, 1: load lại
        private void HidePopup(int mode)
        {
            if (mode == 1)
            {
                GetData();
            }
            onShowPopup("");
        }
        private async void GetData()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Get;
                string api = APIs.API.listAchievement;

                httpRequestMessage.RequestUri = new Uri(api);

                var parameters = new List<KeyValuePair<string, string>>();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                parameters.Add(new KeyValuePair<string, string>("type", "1"));
                parameters.Add(new KeyValuePair<string, string>("page", page_now.ToString()));
                parameters.Add(new KeyValuePair<string, string>("pageSize", COUNT_RECORD_PER_PAGE.ToString()));
                parameters.Add(new KeyValuePair<string, string>("keyword", tbSearch.Text));
                api = api + string.Format("?type={0}&page={1}&pageSize={2}&keyword={3}","1",page_now.ToString(),COUNT_RECORD_PER_PAGE.ToString(), tbSearch.Text);
                /*var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;*/

                var response = await httpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                ListAchievementEntity result = JsonConvert.DeserializeObject<ListAchievementEntity>(responseContent);


                if (result.success != null)
                {
                    total = result.success.data.total;
                    Paging();
                    List<DataEntity2> listAchievement = result.success.data.data;
                    foreach (var item in listAchievement)
                    {
                        if (listAchivementCbx.ContainsKey(item.achievement_type))
                        {
                            item.achievement_at = ConvertDate(item.achievement_at, "dd/MM/yyyy");
                            item.achievement_name = listAchivementCbx[item.achievement_type];
                        }
                    }

                    icPersonalBonusPage.Items.Refresh();
                    icPersonalBonusPage.ItemsSource = listAchievement;
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        private void tbSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                PageNow = "1";
                GetData();
            }
        }


        private void NextPage(object sender, MouseButtonEventArgs e)
        {
            InvalidBtn();
            int page_next = page_now + 1;
            PageNow = page_next.ToString();
            GetData();
        }

        private void PrevPage(object sender, MouseButtonEventArgs e)
        {
            InvalidBtn();
            int page_prev = page_now - 1;
            PageNow = page_prev.ToString();
            GetData();
        }

        private void InvalidBtn()
        {
            btnPrev.IsEnabled = false;
            btnPrev.Opacity = 0.3;
            btnNext.IsEnabled = false;
            btnNext.Opacity = 0.3;
        }

        private void IsSetValidBtn()
        {
            if (page_now == 1)
            {
                btnPrev.IsEnabled = false;
                btnPrev.Opacity = 0.3;
            }
            else
            {
                btnPrev.IsEnabled = true;
                btnPrev.Opacity = 1;
            }

            if (page_now == total_page)
            {
                btnNext.IsEnabled = false;
                btnNext.Opacity = 0.3;
            }
            else
            {
                btnNext.IsEnabled = true;
                btnNext.Opacity = 1;
            }
        }
        //Show popup thêm thành tích
        private void ShowPopupAddAchievements(object sender, MouseButtonEventArgs e)
        {
            AddListAchievementsPopup addListAchievementsPopup = new AddListAchievementsPopup(token, com_id, TypeUser);
            onShowPopup(addListAchievementsPopup);
        }
        //Show popup chỉnh sửa thành tích
        private void ShowPopupUpdateAchievements(object sender, MouseButtonEventArgs e)
        {
            Grid grid = sender as Grid;
            DataEntity2 dataEntity = (DataEntity2)grid.DataContext;
            string achievement_id = dataEntity.achievement_id;
            string content = dataEntity.content;
            string appellation = dataEntity.appellation;
            string achievement_level = dataEntity.achievement_level;
            string achievement_at = dataEntity.achievement_at;
            string created_by = dataEntity.created_by;

            string list_user = dataEntity.list_user;
            string list_user_name = dataEntity.list_user_name;
            string id = dataEntity.id;

            string achievement_type = dataEntity.achievement_type;

            EditAchievementsPopup updateListAchievementsPopup = new EditAchievementsPopup(token, id, achievement_id, content, appellation, achievement_level, achievement_at, created_by, achievement_type, list_user, list_user_name);
            onShowPopup(updateListAchievementsPopup);
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

        private void ClickSearch(object sender, MouseButtonEventArgs e)
        {
            PageNow = "1";
            GetData(); 
        }

        private string ConvertDate(string date, string format)
        {
            DateTime myDate;
            try
            {
                myDate = DateTime.ParseExact(date, "yyyy-MM-ddTHH:mm",
                                              System.Globalization.CultureInfo.InvariantCulture);
                return myDate.ToString(format);
            }
            catch
            {
                try
                {
                    myDate = DateTime.ParseExact(date, "yyyy-MM-dd",
                                                  System.Globalization.CultureInfo.InvariantCulture);
                    return myDate.ToString(format);
                }
                catch
                {
                    try
                    {
                        myDate = DateTime.ParseExact(date, "yyyy/MM/dd",
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
                                myDate = DateTime.ParseExact(date, "MM/dd/yyyy",
                                                              System.Globalization.CultureInfo.InvariantCulture);
                                return myDate.ToString(format);
                            }
                            catch
                            {
                                try
                                {
                                    myDate = DateTime.ParseExact(date, "M/d/yyyy",
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
    }
}
