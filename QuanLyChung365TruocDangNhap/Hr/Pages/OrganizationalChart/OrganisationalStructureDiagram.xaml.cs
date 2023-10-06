
using QuanLyChung365TruocDangNhap.Hr.Core;
using QuanLyChung365TruocDangNhap.Hr.Entities.AdministrationEntity.EmployeeManager;
using QuanLyChung365TruocDangNhap.Hr.Entities.AdministrationEntity.PersonnelChangeEntity;
using QuanLyChung365TruocDangNhap.Hr.Entities.HomeEntity;
using QuanLyChung365TruocDangNhap.Hr.Entities.OrganizationalChartEntities;
using QuanLyChung365TruocDangNhap.Hr.Pages.OrganizationalChart.ListDepPage;
using QuanLyChung365TruocDangNhap.Hr.Pages.OrganizationalChart.ListNestPage;
using QuanLyChung365TruocDangNhap.Hr.Popups.OrganizationalChartPopup;
using QuanLyChung365TruocDangNhap.Hr.View;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
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
using getListEmployee = QuanLyChung365TruocDangNhap.Hr.Entities.AdministrationEntity.PersonnelChangeEntity.getListEmployee;
using Manager = QuanLyChung365TruocDangNhap.Hr.Entities.OrganizationalChartEntities.Manager;

namespace QuanLyChung365TruocDangNhap.Hr.Pages.OrganizationalChart
{
    /// <summary>
    /// Interaction logic for OrganizationalChartPage.xaml
    /// </summary>
    /// 
    // *********************TRANG SƠ ĐỒ CƠ CẤU TỔ CHỨC****************************

    public partial class OrganisationalStructureDiagram : Page, INotifyPropertyChanged
    {

        string token;
        string id;

        //public static Dictionary<string, string> listDepartment = new Dictionary<string, string>();
        public Dictionary<string, string> listCom = new Dictionary<string, string>();

        List<Items6> itemsListDep = new List<Items6>();
        List<Items7> items2 = new List<Items7>();
        List<NestEntity> itemListNest = new List<NestEntity>();
        List<NestEntity> itemListGroup = new List<NestEntity>();

        //ban giám đốc
        string managerPre;
        string vicePresident;

        public string ManagerPre
        {
            get
            {
                return managerPre;
            }
            set
            {
                managerPre = value;
                OnPropertyChanged("Manager");
            }
        }
        public string VicePresident
        {
            get
            {
                return vicePresident;
            }
            set
            {
                vicePresident = value;
                OnPropertyChanged("VicePresident");
            }
        }


        string totalEmployee;
        string totalEmployeeNoTimeKeep;
        int totalEmployeeTimeKeep = 0;



        public string TotalEmployee
        {
            get
            {
                return totalEmployee;
            }
            set
            {
                totalEmployee = value;
                OnPropertyChanged("TotalEmployee");
            }
        }

        public string TotalEmployeeNoTimeKeep
        {
            get
            {
                return totalEmployeeNoTimeKeep;
            }
            set
            {
                totalEmployeeNoTimeKeep = value;
                //TotalEmployeeTimeKeep = (totalEmployee = totalEmployeeNoTimeKeep);
                OnPropertyChanged("TotalEmployeeNoTimeKeep");
            }
        }

        public int TotalEmployeeTimeKeep
        {
            get
            {
                return totalEmployeeTimeKeep;
            }
            set
            {
                totalEmployeeTimeKeep = value;
                OnPropertyChanged("TotalEmployeeTimeKeep");
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

        string pho_to_truong;

        public static Dictionary<string, string> listNest = new Dictionary<string, string>();
        public OrganisationalStructureDiagram(string token, string id,string pho_to_truong)
        {
            InitializeComponent();
            this.token = token;
            this.id = id;
            this.pho_to_truong = pho_to_truong;
            this.DataContext = this;
            GetAll();

            UpdateDetailDep.hidePopup += HidePopup;
            UpdateDetailNest.hidePopup += HidePopup;
        }

        private void GetAll()
        {
            Loadding.Visibility = Visibility.Visible;
            GetTotalEmployeeNoTimeKeep();
            Thread.Sleep(5000);
            var list = new List<Task>() {
                //GetInfoEmployee(),
                GetDatalistDepartment(),
                GetDatalistCom(),
                GetListChild(),
            };

            list.ForEach(t=> {
                t.ContinueWith(tt =>
                {
                    var ck = new List<bool>();
                    list.ForEach(b=>ck.Add(b.IsCompleted));
                    if (!ck.Contains(false)) 
                        this.Dispatcher.Invoke(()=>Loadding.Visibility=Visibility.Collapsed);
                });
            });

        }
        private void HidePopup(int mode)
        {
            if (mode == 1)
            {
                GetAll();
            }
            onShowPopup("");
        }
        //chuyển page luồng sơ đồ
        private void NavigateToPage(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            string name = textBlock.Name;
            String uri = "OrganizationalChart/" + name + "Page";
            switch (name)
            {
                case "OrganisationalStructureDiagram":
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(HomeView))
                        {
                            (window as HomeView).MainContent.Navigate(new OrganisationalStructureDiagram(token, id,pho_to_truong));
                        }
                    };
                    break;
                case "PositionChart":
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(HomeView))
                        {
                            (window as HomeView).MainContent.Navigate(new PositionChart(token, id, pho_to_truong));
                        }
                    };
                    break;
                case "RightToUse":
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(HomeView))
                        {
                            (window as HomeView).MainContent.Navigate(new RightToUse(token, id, pho_to_truong));
                        }
                    };
                    break;
                case "LeadershipBiography":
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(HomeView))
                        {
                            (window as HomeView).MainContent.Navigate(new LeadershipBiography(token, id, pho_to_truong));
                        }
                    };
                    break;
            }
        }

        private void ZoomOut(object sender, MouseButtonEventArgs e)
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        //Chuyển sang page danh sách nhân viên tổng công ty
        private void NavigateToPageEmp(object sender, MouseButtonEventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(HomeView))
                {
                    (window as HomeView).MainContent.Navigate(new OrganisationalStructureDiagramListEmployee(token, id));
                }
            };
        }

        //Chuyển sang page danh sách nhân viên tổng công ty đã chấm công
        private void NavigateToPageEmpTime(object sender, MouseButtonEventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(HomeView))
                {
                    (window as HomeView).MainContent.Navigate(new OrganisationalStructureDiagramListEmployeeTimekeeping(token, id));
                }
            };
        }

        //Danh sách phòng ban
        private async Task GetDatalistDepartment()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;

                string api = APIs.API.listDepartment;

                httpRequestMessage.RequestUri = new Uri(api);
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);

                var response = await httpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                listDepartment result = JsonConvert.DeserializeObject<listDepartment>(responseContent);
                if (result.data != null && result.data.items.Count > 0)
                {
                    itemsListDep = result.data.items;
                    itemsListDep[0].type = "Head";
                    icItemListdep.ItemsSource = itemsListDep;
                    foreach (Items6 item in itemsListDep)
                    {
                        //danh sách tổ
                        /*var httpClient1 = new HttpClient();
                        httpClient1.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                        var form = new Dictionary<string, string>();
                        form.Add("com_id", id);
                        //form.Add("dep_id", item.dep_id);
                        var response2 = httpClient1.PostAsync("http://210.245.108.202:3000/api/qlc/team/list", new FormUrlEncodedContent(form));
                        var check = response2.Result.Content.ReadAsStringAsync().Result;
                        APINestEntity ItemnestEntity = JsonConvert.DeserializeObject<APINestEntity>(response2.Result.Content.ReadAsStringAsync().Result);*/
                        foreach(var team in dataChart.infoDep.FirstOrDefault(d => d.dep_id.ToString() == item.dep_id).infoTeam)
                        {
                            if(item.ListNestEntity == null) item.ListNestEntity = new List<NestEntity>();
                            item.ListNestEntity.Clear();
                            item.ListNestEntity.Add(new NestEntity() { dep_id = item.dep_id, gr_id = team.gr_id.ToString(), gr_name = team.gr_name, total_empNes = team.tong_nv.ToString(), total_empNes_noTimeKeep = team.tong_nv_chua_diem_danh.ToString(), total_empNes_TimeKeep = team.tong_nv_da_diem_danh });
                        }
                        if (item.ListNestEntity != null && item.ListNestEntity.Count > 0)
                        {
                            itemListNest = item.ListNestEntity;
                            itemListNest[itemListNest.Count - 1].typeListNest = "Tail";
                            if (itemListNest.Count != 0)
                            {
                                itemListNest[0].typeListNest = "Head";
                                if (itemListNest.Count == 1)
                                {
                                    itemListNest[0].typeListNest = "Center";
                                }
                            }

                            foreach (var item1 in item.ListNestEntity)
                            {
                                try
                                {
                                    if (id != dataChart.parent_com_id.ToString())
                                    {
                                        item1.total_empNes = dataChart.infoChildCompany.FirstOrDefault(c => c.com_id.ToString() == id).infoDep.FirstOrDefault(d => d.dep_id.ToString() == item1.dep_id).infoTeam.FirstOrDefault(t => t.gr_id.ToString() == item1.gr_id).tong_nv.ToString();
                                        item1.total_empNes_TimeKeep = dataChart.infoChildCompany.FirstOrDefault(c => c.com_id.ToString() == id).infoDep.FirstOrDefault(d => d.dep_id.ToString() == item1.dep_id).infoTeam.FirstOrDefault(t => t.gr_id.ToString() == item1.gr_id).tong_nv_da_diem_danh;
                                        item1.total_empNes_noTimeKeep = dataChart.infoChildCompany.FirstOrDefault(c => c.com_id.ToString() == id).infoDep.FirstOrDefault(d => d.dep_id.ToString() == item1.dep_id).infoTeam.FirstOrDefault(t => t.gr_id.ToString() == item1.gr_id).tong_nv_chua_diem_danh.ToString();
                                    }
                                    else
                                    {
                                        item1.total_empNes = dataChart.infoDep.FirstOrDefault(d => d.dep_id.ToString() == item1.dep_id).infoTeam.FirstOrDefault(t => t.gr_id.ToString() == item1.gr_id).tong_nv.ToString();
                                        item1.total_empNes_TimeKeep = dataChart.infoDep.FirstOrDefault(d => d.dep_id.ToString() == item1.dep_id).infoTeam.FirstOrDefault(t => t.gr_id.ToString() == item1.gr_id).tong_nv_da_diem_danh;
                                        item1.total_empNes_noTimeKeep = dataChart.infoDep.FirstOrDefault(d => d.dep_id.ToString() == item1.dep_id).infoTeam.FirstOrDefault(t => t.gr_id.ToString() == item1.gr_id).tong_nv_chua_diem_danh.ToString();
                                    }
                                }
                                catch { }
                                //danh sách nhóm
                                /*var form1 = new Dictionary<string, string>();
                                form1.Add("com_id", id);
                                //form1.Add("team_id", item1.gr_id);
                                var responseGroup = httpClient.PostAsync("http://210.245.108.202:3000/api/qlc/group/listAll", new FormUrlEncodedContent(form1));
                                var check1 = responseGroup.Result.Content.ReadAsStringAsync().Result;
                                APINestEntity ItemGroupEntity = JsonConvert.DeserializeObject<APINestEntity>(responseGroup.Result.Content.ReadAsStringAsync().Result);*/
                                foreach (var group in dataChart.infoDep.FirstOrDefault(d => d.dep_id.ToString() == item.dep_id).infoTeam.FirstOrDefault(t => t.gr_id.ToString() == item1.gr_id).infoGroup)
                                {
                                    if (item1.ListGroupEntity == null) item1.ListGroupEntity = new List<NestEntity>();
                                    item1.ListGroupEntity.Clear();
                                    item1.ListGroupEntity.Add(new NestEntity() { dep_id = item.dep_id, gr_id = group.gr_id.ToString(), gr_name = group.gr_name, total_empNes = group.group_tong_nv.ToString(), total_empNes_noTimeKeep = (group.group_tong_nv - group.tong_nv_da_diem_danh).ToString(), total_empNes_TimeKeep = group.tong_nv_da_diem_danh });
                                }
                                if (item1.ListGroupEntity != null && item1.ListGroupEntity.Count > 0)
                                {
                                    item1.type = "LineTop";
                                    itemListGroup = item1.ListGroupEntity;
                                    itemListGroup[itemListGroup.Count - 1].typeGroup = "Tail";
                                    if (itemListGroup.Count != 0)
                                    {
                                        itemListGroup[0].typeGroup = "Head";
                                        if (itemListGroup.Count == 1)
                                        {
                                            itemListGroup[0].typeGroup = "Center";
                                        }
                                    }
                                }
                            }
                            item.typeBottom = "LineBottom";
                        }

                        //tổng số nhân viên chưa điểm danh
                        /*HttpRequestMessage getInfoByDepartment = new HttpRequestMessage();
                        getInfoByDepartment.Method = HttpMethod.Get;
                        getInfoByDepartment.RequestUri = new Uri("https://chamcong.24hpay.vn/api_web_hr/getInfoByDepartment.php?id_com=" + id + "&dep_id=" + item.dep_id);
                        getInfoByDepartment.Headers.Add("Authorization", token);
                        var responsegetInfoByDepartment = httpClient.SendAsync(getInfoByDepartment);
                        TotalEmployeeEntity TotalEmployeeInfoByDepartment = JsonConvert.DeserializeObject<TotalEmployeeEntity>(responsegetInfoByDepartment.Result.Content.ReadAsStringAsync().Result);*/
                        var TotalEmployeeInfoByDepartment = dataChart.infoDep.FirstOrDefault(d => d.dep_id.ToString() == item.dep_id);
                        if (TotalEmployeeInfoByDepartment != null)
                        {
                            item.total_emp = TotalEmployeeInfoByDepartment.total_emp.ToString();
                            item.total_emp_TimeKeep = TotalEmployeeInfoByDepartment.tong_nv_da_diem_danh;
                            item.total_emp_noTimeKeep = TotalEmployeeInfoByDepartment.tong_nv_chua_diem_danh.ToString();
                        }

                    }
                }
            }
            catch (Exception)
            {
                //MessageBox.Show("Error");
            }
        }

        //Danh sách công ty con
        private async Task GetDatalistCom()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.listCompany;

                httpRequestMessage.RequestUri = new Uri(api);
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);

                var response = await httpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                listCompany result = JsonConvert.DeserializeObject<listCompany>(responseContent);

                if (result.data != null)
                {
                    items2 = result.data.items;
                    foreach (var item in items2)
                    {
                        if (item.com_parent_id != null)
                        {
                            icItemListCompany.ItemsSource = items2;
                            items2[items2.Count - 1].type = "Tail";
                            if (itemsListDep.Count == 0)
                            {
                                items2[0].type = "Head";
                            }
                        }
                        else
                        {
                            if(item.com_id == "3312")
                            {
                                itemsListDep[itemsListDep.Count - 1].typeTail = "TailDep";
                            }
                        }
                    }
                }
                else
                {

                    if (itemsListDep.Count != 0)
                    {
                        itemsListDep[itemsListDep.Count - 1].type = "Tail";
                    }
                }
            }
            catch (Exception)
            {
                //MessageBox.Show("Error");
            }
        }

        private List<Items6> listNestEntity;

        public List<Items6> ListNestEntity
        {
            get { return listNestEntity; }
            set { listNestEntity = value; OnPropertyChanged("ListNestEntity"); }
        }

        //Chuyển sang page danh sách nhân viên tổng công ty chưa chấm công
        private void NavigateToPageEmpNoTime(object sender, MouseButtonEventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(HomeView))
                {
                    (window as HomeView).MainContent.Navigate(new OrganisationalStructureDiagramListEmployeeNoTimekeeping(token, id));
                }
            };
        }

        //Chuyển sang page danh sách nhân viên phòng ban

        private void NavigateToPageSum(object sender, MouseButtonEventArgs e)
        {
            Grid grid = sender as Grid;
            string dep_id = ((Items6)grid.DataContext).dep_id;
            string dep_name = ((Items6)grid.DataContext).dep_name;
            NavigateToPageSum(dep_id, dep_name);
        }

        private void NavigateToPageSum(string dep_id, string dep_name)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(HomeView))
                {
                    (window as HomeView).MainContent.Navigate(new ListEmpDep(token, id, dep_id, dep_name, ""));
                }
            };
        }

        //Chuyển sang page danh sách nhân viên phòng ban chưa chấm công

        private void NavigateToPageNoTimeKeep(object sender, MouseButtonEventArgs e)
        {
            Grid grid = sender as Grid;
            string dep_id = ((Items6)grid.DataContext).dep_id;
            string dep_name = ((Items6)grid.DataContext).dep_name;
            NavigateToPageNoTimeKeep(dep_id, dep_name);
        }

        private void NavigateToPageNoTimeKeep(string dep_id, string dep_name)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(HomeView))
                {
                    (window as HomeView).MainContent.Navigate(new ListDepNoTimeKeep(token, id, dep_id, dep_name));
                }
            };
        }

        //Chuyển sang page danh sách nhân viên phòng ban đã chấm công

        private void NavigateToPageTimeKeep(object sender, MouseButtonEventArgs e)
        {
            Grid grid = sender as Grid;
            string dep_id = ((Items6)grid.DataContext).dep_id;
            string dep_name = ((Items6)grid.DataContext).dep_name;
            NavigateToPageTimeKeep(dep_id, dep_name);
        }

        private void NavigateToPageTimeKeep(string dep_id, string dep_name)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(HomeView))
                {
                    (window as HomeView).MainContent.Navigate(new ListDepTimeKeep(token, id, dep_id, dep_name));
                }
            };
        }

        //Lấy tổng số nhân viên công ty
        private async Task GetInfoEmployee()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Get;
                string api = "https://chamcong.24hpay.vn/api_web_hr/getInfoByCompany.php?id_com=" + id;

                httpRequestMessage.RequestUri = new Uri(api);

                // Thiết lập Header
                httpRequestMessage.Headers.Add("Authorization", token);

                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                TotalEmployeeEntity result = JsonConvert.DeserializeObject<TotalEmployeeEntity>(responseContent);
                if (result.data != null)
                {
                    TotalEmployee = result.data.items.tong_nv;
                    TotalEmployeeNoTimeKeep = result.data.items.tong_nv_chua_diem_danh;
                    TotalEmployeeTimeKeep = result.data.items.tong_nv_da_diem_danh;
                }
            }
            catch
            {
                //MessageBox.Show("Lỗi đăng nhập, vui lòng thử lại!");
            }
        }


        //Lấy danh sách ban giám đốc
        private async Task GetListChild()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = "http://210.245.108.202:3000/api/qlc/company/child/list";

                httpRequestMessage.RequestUri = new Uri(api);

                // Thiết lập Header
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);

                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                listChildEntities result = JsonConvert.DeserializeObject<listChildEntities>(responseContent);
                if (result.data != null)
                {
                    List<Item> items = result.data.items;
                    if(items[0].manager != null)
                    foreach (var item in items[0].manager)
                    {
                        item.type = "Giám Đốc:";
                        icListChill.Items.Add(item);
                    }
                    if(items[0].deputy != null)
                    foreach (var item in items[0].deputy)
                    {
                        VicePresident = item.ep_name;
                        item.type = "Phó Giám Đốc:";
                        icListChill.Items.Add(item);
                    }
                }
            }
            catch
            {
                //MessageBox.Show("Lỗi đăng nhập, vui lòng thử lại!");
            }
        }

        public static InfoCompany dataChart { get; set; }
        //Lấy tổng số nhân viên công ty chưa điểm danh
        private void GetTotalEmployeeNoTimeKeep()
        {
            try
            {
                /*var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = "http://210.245.108.202:3006/api/hr/organizationalStructure/detailInfoCompany";

                httpRequestMessage.RequestUri = new Uri(api);

                // Thiết lập Header
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);

                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();*/
                WebClient web = new WebClient();
                web.Headers.Add("Authorization", "Bearer " + token);
                var respo = web.UploadValues(new Uri("http://210.245.108.202:3006/api/hr/organizationalStructure/detailInfoCompany"), "POST", web.Headers);
                var check = UTF8Encoding.UTF8.GetString(respo);
                DetailOrganization result = JsonConvert.DeserializeObject<DetailOrganization>(UTF8Encoding.UTF8.GetString(respo));
                if (result.data == null) return;
                TotalEmployee = result.data.infoCompany.tong_nv.ToString();
                TotalEmployeeNoTimeKeep = result.data.infoCompany.tong_nv_chua_diem_danh.ToString();
                TotalEmployeeTimeKeep = result.data.infoCompany.tong_nv_da_diem_danh;
                //txtTotalEmployeeNoTimeKeep.Text = Convert.ToInt32(result.data.infoCompany.tong_nv_chua_diem_danh).ToString();
                //txtTotalEmployeeTimeKeep.Text = Convert.ToInt32(txtTotalEmployee.Text) - Convert.ToInt32(result.data.infoCompany.tong_nv) + "";
                dataChart = result.data.infoCompany;
            }
            catch
            {
                //MessageBox.Show("Lỗi đăng nhập, vui lòng thử lại!");
            }
        }

        //Lấy tổng số nhân viên phòng ban đã điểm danh


        private List<getListEmployee> totalDep;

        public List<getListEmployee> TotalDep
        {
            get { return totalDep; }
            set { totalDep = value; OnPropertyChanged("ListEmp"); }
        }


        private void ShowDetailPopup(object sender, MouseButtonEventArgs e)
        {
            DetailDep detailDep = new DetailDep();
            onShowPopup(detailDep);
        }

        private void ShowUpdatePopup(object sender, MouseButtonEventArgs e)
        {
            Grid grid = sender as Grid;
            string dep_id = ((Items6)grid.DataContext).dep_id;
            string dep_name = ((Items6)grid.DataContext).dep_name;
            string total_emp = ((Items6)grid.DataContext).total_emp;

            UpdateDetailDep detailDep = new UpdateDetailDep(token,dep_id,dep_name,total_emp,pho_to_truong);
            onShowPopup(detailDep);
        }


        private void ShowPopupOption(object sender, MouseButtonEventArgs e)
        {
            if (popupOption.Visibility == Visibility.Collapsed)
            {
                popupOption.Visibility = Visibility.Visible;
            }
            else
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

        //Chuyển page sang danh sách tổ chưa chấm công
        private void NavigateToPageNestNoTimeKeep(object sender, MouseButtonEventArgs e)
        {
            Grid grid = sender as Grid;
            string gr_id = ((NestEntity)grid.DataContext).gr_id;
            string gr_name = ((NestEntity)grid.DataContext).gr_name;
            string dep_id = ((NestEntity)grid.DataContext).dep_id;

            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(HomeView))
                {
                    (window as HomeView).MainContent.Navigate(new ListNestNoTimeKeep(token, id, gr_id, gr_name, dep_id));
                }
            };
        }

        //Chuyển page sang danh sách tổ chấm công
        private void NavigateToPageNestTimeKeep(object sender, MouseButtonEventArgs e)
        {
            Grid grid = sender as Grid;
            string gr_id = ((NestEntity)grid.DataContext).gr_id;
            string gr_name = ((NestEntity)grid.DataContext).gr_name;
            string dep_id = ((NestEntity)grid.DataContext).dep_id;


            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(HomeView))
                {
                    (window as HomeView).MainContent.Navigate(new ListNestTimeKeep(token, id, gr_id, gr_name, dep_id));
                }
            };
        }

        //chuyển page sang danh sách tổng nhân viên tổ
        private void NavigateToPageNestSum(object sender, MouseButtonEventArgs e)
        {
            Grid grid = sender as Grid;
            string gr_id = ((NestEntity)grid.DataContext).gr_id;
            string gr_name = ((NestEntity)grid.DataContext).gr_name;
            string dep_id = ((NestEntity)grid.DataContext).dep_id;

            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(HomeView))
                {
                    (window as HomeView).MainContent.Navigate(new ListEmpNest(token, id, gr_id, gr_name, dep_id));
                }
            };
        }

        //Show chỉnh sửa tổ
        private void ShowUpdateNess(object sender, MouseButtonEventArgs e)
        {
            Grid grid = sender as Grid;
            string dep_id = ((Items6)grid.DataContext).dep_id;
            string dep_name = ((Items6)grid.DataContext).dep_name;

            string gr_name = ((NestEntity)grid.DataContext).gr_name;
            string gr_id = ((NestEntity)grid.DataContext).gr_id;

            UpdateDetailNest detailDep = new UpdateDetailNest(token, dep_id, dep_name, gr_name, gr_id);
            onShowPopup(detailDep);
        }

        private void icListChill_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            try
            {
                if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)) 
                {
                    scroll.ScrollToVerticalOffset(scroll.VerticalOffset);
                    scroll.ScrollToHorizontalOffset(scroll.HorizontalOffset - e.Delta); 
                }
            }
            catch { }
        }
    }
}
