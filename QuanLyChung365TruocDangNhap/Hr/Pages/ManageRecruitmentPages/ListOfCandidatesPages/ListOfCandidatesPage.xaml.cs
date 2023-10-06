using QuanLyChung365TruocDangNhap.Hr.Entities.HomeEntity;
using QuanLyChung365TruocDangNhap.Hr.Entities.ManageRecuitmentEntities.ListCandidateEntities;
using QuanLyChung365TruocDangNhap.Hr.Popups.RecruitmentManagementPopups.ListOfCandidatesPopups;
using QuanLyChung365TruocDangNhap.Hr.Popups.RecruitmentManagementPopups.ListOfCandidatesPopups.TransportPopups;
using QuanLyChung365TruocDangNhap.Hr.View;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
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
using static QRCoder.PayloadGenerator;
using static System.Net.WebRequestMethods;

namespace QuanLyChung365TruocDangNhap.Hr.Pages.ManageRecruitmentPages.ListOfCandidatesPages
{
    /// <summary>
    /// Interaction logic for ListOfCandidatesPage.xaml
    /// </summary>
    public partial class ListOfCandidatesPage : Page, INotifyPropertyChanged
    {
        string token;
        string id; // id công ty
        string type;
        public Dictionary<string, string> listAllEmployee = new Dictionary<string, string>();
        public Dictionary<string, string> listAllEmployee1 = new Dictionary<string, string>();
        public Dictionary<string, string> listProccess;
        public Dictionary<string, string> listRecruitmentNew = new Dictionary<string, string>();


        //Danh sách nhận hồ sơ
        List<DataEntity> listProfile = new List<DataEntity>();
        //Danh sách nhận việc
        List<DataEntity> listGetJob = new List<DataEntity>();
        //Danh sách trượt
        List<DataEntity> listFailJob = new List<DataEntity>();
        //Danh sách hủy
        List<DataEntity> listCancelJob = new List<DataEntity>();
        //Danh sách kí hợp đồng
        List<DataEntity> listContactJob = new List<DataEntity>();


        // items in cbx process
        public List<Items> listItem_cbx4_1 = new List<Items>() {
            new Items() { ID = "0", Name = "Nhận hồ sơ ứng viên" },
            new Items() { ID = "1", Name = "Nhận việc" } ,
            new Items() { ID = "2", Name = "Trượt" } ,
            new Items() { ID = "3", Name = "Hủy" } ,
            new Items() { ID = "4", Name = "Ký hợp đồng" }
        };

        public List<Items> listItem_cbx4_12 = new List<Items>() {
            new Items() { ID = "1", Name = "Nhận việc" } ,
            new Items() { ID = "2", Name = "Trượt" } ,
            new Items() { ID = "3", Name = "Hủy" } ,
            new Items() { ID = "4", Name = "Ký hợp đồng" }
        };
        public List<Items> listItem_cbx4_2 = new List<Items>() {
            new Items() { ID = "-1", Name = "Nhận việc" } ,
            new Items() { ID = "-2", Name = "Trượt" } ,
            new Items() { ID = "-3", Name = "Hủy" } ,
            new Items() { ID = "-4", Name = "Ký hợp đồng" }
        };
        public List<Items> listItem_cbx4_process = new List<Items>() {
            new Items() { ID = "0", Name = "Nhận hồ sơ ứng viên" },
            new Items() { ID = "1", Name = "Nhận việc" } ,
            new Items() { ID = "2", Name = "Trượt" } ,
            new Items() { ID = "3", Name = "Hủy" } ,
            new Items() { ID = "4", Name = "Ký hợp đồng" }
        };

        // cbx status
        public List<Items> listItemStatus = new List<Items>()
        {
            new Items(){ID = "0", Name="Trạng thái"},
            new Items(){ID = "1", Name="Trượt vòng loại phỏng vấn"},
            new Items(){ID = "2", Name="Trượt học việc"},
            new Items(){ID = "3", Name="Trượt vòng loại hồ sơ"},
            new Items(){ID = "4", Name="Hủy phỏng vấn"},
            new Items(){ID = "5", Name="Hủy nhận việc"},
            new Items(){ID = "6", Name="Hủy học việc"},
        };

        //cbx Gender
        public List<Items> listItemsGender = new List<Items>()
        {
            new Items(){ID = "0", Name="Chọn giới tính"},
            new Items(){ID = "1", Name="Nam"},
            new Items(){ID = "2", Name="Nữ"},
            new Items(){ID = "3", Name="Khác"},
        };

        bool perAdd, perEdit, perDel; // quyền thêm, sửa, xóa
        private Action<object, MouseButtonEventArgs> clickViewDetail;

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

        public ListOfCandidatesPage(string token, string id, string type, bool perAdd, bool perEdit, bool perDel)
        {


            InitializeComponent();
            this.token = token;
            this.id = id;
            this.type = type;
            this.PerAdd = perAdd;
            this.PerEdit = perEdit;

            if(id == "3312")
            {
                perDel = false;
            }
            else
            {
                this.PerDel = perDel;
            }

            DataContext = this;
            GetAllEmployee();
            GetAllData();
            GetListRecruitmentNew();

            cbxStatus.ItemsSource = listItemStatus;
            cbxGender.ItemsSource = listItemsGender;

            MoreCandidatePopup.hidePopup += HidePopup;
            EditCandidatePopup.hidePopup += HidePopup;
            DeleteLocationPopup.hidePopup += HidePopup;
            CandidatePofilePopup.hidePopup += HidePopup;
            DeleteCandidatePopup.hidePopup += HidePopup;
            ExporFileExcelPopup.hidePopup += HidePopup3;


            ChooseTransportProcess.hidePopup += OnShowTransportPopup;
            InterviewPopup.hidePopup += HidePopup2;
            GetJobPopup.hidePopup += HidePopup2;
            ContractJobPopup.hidePopup += HidePopup2;
            FailJobPopup.hidePopup += HidePopup2;
            CancelJobPopup.hidePopup += HidePopup2;




            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy"; //for the second type
            Thread.CurrentThread.CurrentCulture = ci;
        }

        private void HidePopup3(int mode)
        {
            if (mode == 1)
            {
                GetAllData();
            }
            onShowPopup("");
        }

        private void GetAllData()
        {
            GetListCandidate();
            GetListCandidateGetJob();
            GetListCandidateFailJob();
            GetListCandidateCancelJob();
            GetListCandidateContactJob();
            GetListProcessInterview();
            GetlistProcessInterviewGetJob();
            GetlistProcessInterviewFailJob();
            GetlistProcessInterviewCancelJob();
            GetlistProcessInterviewContactJob();
        }

    
        // lấy toàn bộ nhân viên trong công ty
        private async void GetAllEmployee()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.list_all_employee;

                httpRequestMessage.RequestUri = new Uri(api);
                var form = new Dictionary<string, string>();
                form.Add("com_id", id);
                // Thiết lập Header
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);
                httpRequestMessage.Content = new FormUrlEncodedContent(form);

                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                AllEmployeeEntity result = JsonConvert.DeserializeObject<AllEmployeeEntity>(responseContent);
                listAllEmployee.Add("", "Tất cả");
                if (result.data == null) return;
                foreach (var item in result.data.items)
                {
                    listAllEmployee.Add(item.ep_id, item.ep_name + "(" + item.ep_id + ")" );
                }


                cbxUserHiring.ItemsSource = listAllEmployee;
            }
            catch
            {
                //MessageBox.Show("Lỗi đăng nhập, vui lòng thử lại!");
            }
        }




        // Danh sách tên giai đoạn sau nhận hồ sơ ứng viên tự tạo
        private async void GetListProcessInterview()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.list_process_interview;

                httpRequestMessage.RequestUri = new Uri(api);

                var parameters = new List<KeyValuePair<string, string>>();

                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                // Thiết lập Content
                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                ListProcessInterviewEntity result = JsonConvert.DeserializeObject<ListProcessInterviewEntity>(responseContent);

                if (result.success != null && result.success.data != null)
                {
                    List<DataList> list = result.success.data.data;
                    listProccess = new Dictionary<string, string>();
                    listItem_cbx4_process.Clear();
                    foreach (var item in list)
                    {
                        item.type = 0;
                        listProccess.Add(item.id, item.name);
                        Items item_cbx = new Items() { ID = item.id, Name = item.name };
                        Task<List<DataEntity>> task = GetListCandidateSchedule(item.id);
                        await task;
                        listItem_cbx4_process.Add(item_cbx);
                        item.data = task.Result;
                        if (item.data != null)
                            item.total = "(" + item.data.Count.ToString() + " ứng viên)";
                    }

                    icListProcess.Items.Refresh();
                    icListProcess.ItemsSource = list;
                }
            }
            catch
            {
                //MessageBox.Show("Lỗi đăng nhập, vui lòng thử lại!");
            }
        }


        // danh sách sau giai đoạn nhận việc
        private async void GetlistProcessInterviewGetJob()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.listProcessInterviewGetJob;

                httpRequestMessage.RequestUri = new Uri(api);

                var parameters = new List<KeyValuePair<string, string>>();

                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                // Thiết lập Content
                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();
                ListProcessInterviewEntity result = JsonConvert.DeserializeObject<ListProcessInterviewEntity>(responseContent);
                if (result.success != null && result.success.data != null)
                {
                    List<DataList> list = result.success.data.data;
                    listProccess = new Dictionary<string, string>();
                    listItem_cbx4_process.Clear();
                    foreach (var item in list)
                    {
                        item.type = 1;
                        listProccess.Add(item.id, item.name);
                        Items item_cbx = new Items() { ID = item.id, Name = item.name };

                        Task<List<DataEntity>> task = GetListCandidateSchedule(item.id);
                        await task;
                        listItem_cbx4_process.Add(item_cbx);
                        item.data = task.Result;
                        if (item.data != null)
                            item.total = "(" + item.data.Count.ToString() + " ứng viên)";
                    }
                    icListProcessNhanViec.ItemsSource = list;
                    icListProcessNhanViec.Items.Refresh();
                }
            }
            catch
            {
                //MessageBox.Show("Lỗi đăng nhập, vui lòng thử lại!");
            }
        }


        // danh sách sau giai đoạn trượt
        private async void GetlistProcessInterviewFailJob()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.listProcessInterviewFailJob;

                httpRequestMessage.RequestUri = new Uri(api);

                var parameters = new List<KeyValuePair<string, string>>();

                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);


                // Thiết lập Content
                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                ListProcessInterviewEntity result = JsonConvert.DeserializeObject<ListProcessInterviewEntity>(responseContent);


                if (result.success != null && result.success.data != null)
                {
                    List<DataList> list = result.success.data.data;
                    listProccess = new Dictionary<string, string>();
                    listItem_cbx4_process.Clear();
                    foreach (var item in list)
                    {
                        item.type = 2;
                        listProccess.Add(item.id, item.name);
                        Items item_cbx = new Items() { ID = item.id, Name = item.name };
                        Task<List<DataEntity>> task = GetListCandidateSchedule(item.id);
                        await task;
                        listItem_cbx4_process.Add(item_cbx);

                        item.data = task.Result;
                        if (item.data != null)
                            item.total = "(" + item.data.Count.ToString() + " ứng viên)";
                    }

                    icListProcessTruot.Items.Refresh();
                    icListProcessTruot.ItemsSource = list;
                }
            }
            catch
            {
                //MessageBox.Show("Lỗi đăng nhập, vui lòng thử lại!");
            }
        }


        // danh sách sau giai đoạn hủy
        private async void GetlistProcessInterviewCancelJob()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.listProcessInterviewCancelJob;

                httpRequestMessage.RequestUri = new Uri(api);

                var parameters = new List<KeyValuePair<string, string>>();

                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                // Thiết lập Content
                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                ListProcessInterviewEntity result = JsonConvert.DeserializeObject<ListProcessInterviewEntity>(responseContent);


                if (result.success != null && result.success.data != null)
                {
                    List<DataList> list = result.success.data.data;
                    listProccess = new Dictionary<string, string>();
                    listItem_cbx4_process.Clear();
                    foreach (var item in list)
                    {
                        item.type = 3;
                        listProccess.Add(item.id, item.name);
                        Items item_cbx = new Items() { ID = item.id, Name = item.name };
                        Task<List<DataEntity>> task = GetListCandidateSchedule(item.id);
                        await task;
                        listItem_cbx4_process.Add(item_cbx);

                        item.data = task.Result;
                        if (item.data != null)
                            item.total = "(" + item.data.Count.ToString() + " ứng viên)";
                    }

                    icListProcessHuy.Items.Refresh();
                    icListProcessHuy.ItemsSource = list;
                }
            }
            catch
            {
                //MessageBox.Show("Lỗi đăng nhập, vui lòng thử lại!");
            }
        }


        // danh sách sau giai đoạn ký hợp đồng
        private async void GetlistProcessInterviewContactJob()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.listProcessInterviewContactJob;

                httpRequestMessage.RequestUri = new Uri(api);

                var parameters = new List<KeyValuePair<string, string>>();

                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);


                // Thiết lập Content
                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                ListProcessInterviewEntity result = JsonConvert.DeserializeObject<ListProcessInterviewEntity>(responseContent);


                if (result.success != null && result.success.data != null)
                {
                    List<DataList> list = result.success.data.data;
                    listProccess = new Dictionary<string, string>();
                    listItem_cbx4_process.Clear();
                    foreach (var item in list)
                    {
                        item.type = 4;
                        listProccess.Add(item.id, item.name);
                        Items item_cbx = new Items() { ID = item.id, Name = item.name };
                        Task<List<DataEntity>> task = GetListCandidateSchedule(item.id);
                        await task;
                        listItem_cbx4_process.Add(item_cbx);

                        item.data = task.Result;
                        if (item.data != null)
                            item.total = "(" + item.data.Count.ToString() + " ứng viên)";
                    }

                    icListProcessKyHopDong.Items.Refresh();
                    icListProcessKyHopDong.ItemsSource = list;
                }
            }
            catch
            {
                //MessageBox.Show("Lỗi đăng nhập, vui lòng thử lại!");
            }
        }


        private async Task<List<DataEntity>> GetListCandidateSchedule(string process_id)
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.list_candidate_schedule;

                httpRequestMessage.RequestUri = new Uri(api);

                var parameters = new List<KeyValuePair<string, string>>();

                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                parameters.Add(new KeyValuePair<string, string>("process_interview_id", process_id));
                parameters.Add(new KeyValuePair<string, string>("type", type));


                //if (dp1.SelectedDate != null)
                //{
                //    parameters.Add(new KeyValuePair<string, string>("date_from", dp1.SelectedDate.Value.ToString("yyyy-MM-dd")));
                //}
                if (dp1.SelectedDate != null)
                {
                    DateTime dateTime = DateTime.ParseExact(dp1.SelectedDate.Value.ToString("dd/MM/yyyy"), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    string fromDate = dateTime.ToString("yyyy-MM-dd");
                    parameters.Add(new KeyValuePair<string, string>("fromDate", fromDate));
                }
                else
                {
                    parameters.Add(new KeyValuePair<string, string>("fromDate", ""));
                }

                if (dp2.SelectedDate != null)
                {
                    DateTime dateTime = DateTime.ParseExact(dp2.SelectedDate.Value.ToString("dd/MM/yyyy"), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    string toDate = dateTime.ToString("yyyy-MM-dd");
                    parameters.Add(new KeyValuePair<string, string>("toDate", toDate));
                }
                else
                {
                    parameters.Add(new KeyValuePair<string, string>("toDate", ""));
                }

                parameters.Add(new KeyValuePair<string, string>("name", tbName.Text));

                if (cbxRecruitment.SelectedIndex > -1)
                {
                    string new_id = cbxRecruitment.SelectedValue.ToString();
                    parameters.Add(new KeyValuePair<string, string>("recruitmentNewsId", new_id));
                }

                if (cbxUserHiring.SelectedIndex > -1)
                {
                    string can_id = cbxUserHiring.SelectedValue.ToString();
                    parameters.Add(new KeyValuePair<string, string>("userHiring", can_id));
                }

                if (cbxGender.SelectedIndex > -1)
                {
                    string gender = cbxGender.SelectedValue.ToString();
                    parameters.Add(new KeyValuePair<string, string>("gender", gender));
                }


                if (cbxStatus.SelectedIndex > -1)
                {
                    string status = cbxStatus.SelectedValue.ToString();
                    parameters.Add(new KeyValuePair<string, string>("status", status));
                }

                // Thiết lập Content
                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                ListCandidateEntity result = JsonConvert.DeserializeObject<ListCandidateEntity>(responseContent);

                if (result.success != null && result.success.data != null)
                {
                    List<DataEntity> listRecuitment = result.success.data.data;
                    foreach (var item in listRecuitment)
                    {
                        if (listAllEmployee.ContainsKey(item.user_hiring))
                        {
                            item.hr_name = listAllEmployee[item.user_hiring];
                        }
                        item.id_process_interview = process_id;
                    }
                    return listRecuitment;
                }

                return null;

            }
            catch
            {
                return null;
                //MessageBox.Show("Lỗi đăng nhập, vui lòng thử lại!");
            }
        }

        /// ==========================================================

        // danh sách ứng viên gửi hồ sơ
        private async void GetListCandidate()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.list_candidate;

                httpRequestMessage.RequestUri = new Uri(api);

                var parameters = new List<KeyValuePair<string, string>>();

                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                if (dp1.SelectedDate!= null)
                {
                    DateTime dateTime = DateTime.ParseExact(dp1.SelectedDate.Value.ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    string fromDate = dateTime.ToString("yyyy-MM-dd");
                    parameters.Add(new KeyValuePair<string, string>("fromDate", fromDate));
                }
                else
                {
                    parameters.Add(new KeyValuePair<string, string>("fromDate", ""));
                }

                if (dp2.SelectedDate != null)
                {
                    DateTime dateTime = DateTime.ParseExact(dp2.SelectedDate.Value.ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    string toDate = dateTime.ToString("yyyy-MM-dd");
                    parameters.Add(new KeyValuePair<string, string>("toDate", toDate));
                }
                else
                {
                    parameters.Add(new KeyValuePair<string, string>("toDate", ""));
                }

                parameters.Add(new KeyValuePair<string, string>("name", tbName.Text));

                if (cbxRecruitment.SelectedIndex > -1)
                {
                    string new_id = cbxRecruitment.SelectedValue.ToString();
                    parameters.Add(new KeyValuePair<string, string>("recruitmentNewsId", new_id));
                }

                if (cbxUserHiring.SelectedIndex > -1)
                {
                    string can_id = cbxUserHiring.SelectedValue.ToString();
                    parameters.Add(new KeyValuePair<string, string>("userHiring", can_id));
                }

                if (cbxGender.SelectedIndex > -1)
                {
                    string gender = cbxGender.SelectedValue.ToString();
                    parameters.Add(new KeyValuePair<string, string>("gender", gender));
                }


                if (cbxStatus.SelectedIndex > -1)
                {
                    string status = cbxStatus.SelectedValue.ToString();
                    parameters.Add(new KeyValuePair<string, string>("status", status));
                }


                // Thiết lập Content
                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                ListCandidateEntity result = JsonConvert.DeserializeObject<ListCandidateEntity>(responseContent);

                if (result.success != null && result.success.data != null)
                {
                    listProfile = result.success.data.data;
                    foreach (var item in listProfile)
                    {
                        if (listAllEmployee.ContainsKey(item.user_hiring))
                        {
                            item.hr_name = listAllEmployee[item.user_hiring];
                        }
                        item.id_process_interview = "0";
                    }

                    tbTotalCandidate.Text = "(" + listProfile.Count.ToString() + " ứng viên)";

                    icListCandidate.ItemsSource = listProfile;
                }

            }
            catch
            {
                //MessageBox.Show("Lỗi đăng nhập, vui lòng thử lại!");
            }
        }
        //danh sách UV nhận việc
        private async void GetListCandidateGetJob()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.list_candidate_get_job;

                httpRequestMessage.RequestUri = new Uri(api);
                var parameters = new List<KeyValuePair<string, string>>();

                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                if (dp1.SelectedDate != null)
                {
                    DateTime dateTime = DateTime.ParseExact(dp1.SelectedDate.Value.ToString("dd/MM/yyyy"), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    string fromDate = dateTime.ToString("yyyy-MM-dd");
                    parameters.Add(new KeyValuePair<string, string>("fromDate", fromDate));
                }
                else
                {
                    parameters.Add(new KeyValuePair<string, string>("fromDate", ""));
                }

                if (dp2.SelectedDate != null)
                {
                    DateTime dateTime = DateTime.ParseExact(dp2.SelectedDate.Value.ToString("dd/MM/yyyy"), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    string toDate = dateTime.ToString("yyyy-MM-dd");
                    parameters.Add(new KeyValuePair<string, string>("toDate", toDate));
                }
                else
                {
                    parameters.Add(new KeyValuePair<string, string>("toDate", ""));
                }

                parameters.Add(new KeyValuePair<string, string>("name", tbName.Text));

                if (cbxRecruitment.SelectedIndex > -1)
                {
                    string new_id = cbxRecruitment.SelectedValue.ToString();
                    parameters.Add(new KeyValuePair<string, string>("recruitmentNewsId", new_id));
                }

                if (cbxUserHiring.SelectedIndex > -1)
                {
                    string can_id = cbxUserHiring.SelectedValue.ToString();
                    parameters.Add(new KeyValuePair<string, string>("userHiring", can_id));
                }

                if (cbxGender.SelectedIndex > -1)
                {
                    string gender = cbxGender.SelectedValue.ToString();
                    parameters.Add(new KeyValuePair<string, string>("gender", gender));
                }


                if (cbxStatus.SelectedIndex > -1)
                {
                    string status = cbxStatus.SelectedValue.ToString();
                    parameters.Add(new KeyValuePair<string, string>("status", status));
                }

                // Thiết lập Content
                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                ListCandidateEntity result = JsonConvert.DeserializeObject<ListCandidateEntity>(responseContent);

                if (result.success != null && result.success.data != null)
                {
                    listGetJob = result.success.data.data;
                    foreach (var item in listGetJob)
                    {
                        if (listAllEmployee.ContainsKey(item.user_hiring))
                        {
                            item.hr_name = listAllEmployee[item.user_hiring];
                        }
                        item.id_process_interview = "1";

                    }

                    tbTotalCandidateGetJob.Text = "(" + listGetJob.Count.ToString() + " ứng viên)";
                    icListCandidateGetJob.ItemsSource = listGetJob;
                    //Xuất excel danh sách nhận việc
                }
            }
            catch
            {
                //MessageBox.Show("Lỗi đăng nhập, vui lòng thử lại!");
            }
        }

        //danh sách UV trượt  việc
        private async void GetListCandidateFailJob()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.list_candidate_fail_job;

                httpRequestMessage.RequestUri = new Uri(api);

                var parameters = new List<KeyValuePair<string, string>>();

                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                if (dp1.SelectedDate != null)
                {
                    DateTime dateTime = DateTime.ParseExact(dp1.SelectedDate.Value.ToString("dd/MM/yyyy"), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    string fromDate = dateTime.ToString("yyyy-MM-dd");
                    parameters.Add(new KeyValuePair<string, string>("fromDate", fromDate));
                }
                else
                {
                    parameters.Add(new KeyValuePair<string, string>("fromDate", ""));
                }

                if (dp2.SelectedDate != null)
                {
                    DateTime dateTime = DateTime.ParseExact(dp2.SelectedDate.Value.ToString("dd/MM/yyyy"), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    string toDate = dateTime.ToString("yyyy-MM-dd");
                    parameters.Add(new KeyValuePair<string, string>("toDate", toDate));
                }
                else
                {
                    parameters.Add(new KeyValuePair<string, string>("toDate", ""));
                }

                parameters.Add(new KeyValuePair<string, string>("name", tbName.Text));

                if (cbxRecruitment.SelectedIndex > -1)
                {
                    string new_id = cbxRecruitment.SelectedValue.ToString();
                    parameters.Add(new KeyValuePair<string, string>("recruitmentNewsId", new_id));
                }

                if (cbxUserHiring.SelectedIndex > -1)
                {
                    string can_id = cbxUserHiring.SelectedValue.ToString();
                    parameters.Add(new KeyValuePair<string, string>("userHiring", can_id));
                }

                if (cbxGender.SelectedIndex > -1)
                {
                    string gender = cbxGender.SelectedValue.ToString();
                    parameters.Add(new KeyValuePair<string, string>("gender", gender));
                }


                if (cbxStatus.SelectedIndex > -1)
                {
                    string status = cbxStatus.SelectedValue.ToString();
                    parameters.Add(new KeyValuePair<string, string>("status", status));
                }

                // Thiết lập Content
                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                ListCandidateEntity result = JsonConvert.DeserializeObject<ListCandidateEntity>(responseContent);

                if (result.success != null && result.success.data != null)
                {
                    listFailJob = result.success.data.data;
                    foreach (var item in listFailJob)
                    {
                        if (listAllEmployee.ContainsKey(item.user_hiring))
                        {
                            item.hr_name = listAllEmployee[item.user_hiring];
                        }
                        item.id_process_interview = "-2";
                    }

                    tbTotalCandidateFailJob.Text = "(" + listFailJob.Count.ToString() + " ứng viên)";
                    icListCandidateFailJob.ItemsSource = listFailJob;
                }
            }
            catch
            {
                //MessageBox.Show("Lỗi đăng nhập, vui lòng thử lại!");
            }
        }

        //danh sách UV hủy việc
        private async void GetListCandidateCancelJob()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.list_candidate_cancel_job;

                httpRequestMessage.RequestUri = new Uri(api);

                var parameters = new List<KeyValuePair<string, string>>();

                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                if (dp1.SelectedDate != null)
                {
                    DateTime dateTime = DateTime.ParseExact(dp1.SelectedDate.Value.ToString("dd/MM/yyyy"), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    string fromDate = dateTime.ToString("yyyy-MM-dd");
                    parameters.Add(new KeyValuePair<string, string>("fromDate", fromDate));
                }
                else
                {
                    parameters.Add(new KeyValuePair<string, string>("fromDate", ""));
                }

                if (dp2.SelectedDate != null)
                {
                    DateTime dateTime = DateTime.ParseExact(dp2.SelectedDate.Value.ToString("dd/MM/yyyy"), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    string toDate = dateTime.ToString("yyyy-MM-dd");
                    parameters.Add(new KeyValuePair<string, string>("toDate", toDate));
                }
                else
                {
                    parameters.Add(new KeyValuePair<string, string>("toDate", ""));
                }

                parameters.Add(new KeyValuePair<string, string>("name", tbName.Text));

                if (cbxRecruitment.SelectedIndex > -1)
                {
                    string new_id = cbxRecruitment.SelectedValue.ToString();
                    parameters.Add(new KeyValuePair<string, string>("recruitmentNewsId", new_id));
                }

                if (cbxUserHiring.SelectedIndex > -1)
                {
                    string can_id = cbxUserHiring.SelectedValue.ToString();
                    parameters.Add(new KeyValuePair<string, string>("userHiring", can_id));
                }

                if (cbxGender.SelectedIndex > -1)
                {
                    string gender = cbxGender.SelectedValue.ToString();
                    parameters.Add(new KeyValuePair<string, string>("gender", gender));
                }


                if (cbxStatus.SelectedIndex > -1)
                {
                    string status = cbxStatus.SelectedValue.ToString();
                    parameters.Add(new KeyValuePair<string, string>("status", status));
                }

                // Thiết lập Content
                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                ListCandidateEntity result = JsonConvert.DeserializeObject<ListCandidateEntity>(responseContent);

                if (result.success != null && result.success.data != null)
                {
                    listCancelJob = result.success.data.data;
                    int dem = 0;
                    foreach (var item in listCancelJob)
                    {
                        dem++;
                        if (listAllEmployee.ContainsKey(item.user_hiring))
                        {
                            item.hr_name = listAllEmployee[item.user_hiring];
                        }
                        item.id_process_interview = "-3";
                        if (dem > 100) break;
                    }

                    tbTotalCandidateCancelJob.Text = "(" + listCancelJob.Count.ToString() + " ứng viên)";
                    if(listCancelJob.Count >1000)
                        icListCandidateCancelJob.ItemsSource = listCancelJob.GetRange(0, 1000);
                    else
                        icListCandidateCancelJob.ItemsSource = listCancelJob;
                }

            }
            catch
            {
                //MessageBox.Show("Lỗi đăng nhập, vui lòng thử lại!");
            }
        }

        //danh sách UV kí hợp đồng
        private async void GetListCandidateContactJob()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.list_candidate_contact_job;

                httpRequestMessage.RequestUri = new Uri(api);

                var parameters = new List<KeyValuePair<string, string>>();

                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                if (dp1.SelectedDate != null)
                {
                    DateTime dateTime = DateTime.ParseExact(dp1.SelectedDate.Value.ToString("dd/MM/yyyy"), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    string fromDate = dateTime.ToString("yyyy-MM-dd");
                    parameters.Add(new KeyValuePair<string, string>("fromDate", fromDate));
                }
                else
                {
                    parameters.Add(new KeyValuePair<string, string>("fromDate", ""));
                }

                if (dp2.SelectedDate != null)
                {
                    DateTime dateTime = DateTime.ParseExact(dp2.SelectedDate.Value.ToString("dd/MM/yyyy"), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    string toDate = dateTime.ToString("yyyy-MM-dd");
                    parameters.Add(new KeyValuePair<string, string>("toDate", toDate));
                }
                else
                {
                    parameters.Add(new KeyValuePair<string, string>("toDate", ""));
                }

                parameters.Add(new KeyValuePair<string, string>("name", tbName.Text));

                if (cbxRecruitment.SelectedIndex > -1)
                {
                    string new_id = cbxRecruitment.SelectedValue.ToString();
                    parameters.Add(new KeyValuePair<string, string>("recruitmentNewsId", new_id));
                }

                if (cbxUserHiring.SelectedIndex > -1)
                {
                    string can_id = cbxUserHiring.SelectedValue.ToString();
                    parameters.Add(new KeyValuePair<string, string>("userHiring", can_id));
                }

                if (cbxGender.SelectedIndex > -1)
                {
                    string gender = cbxGender.SelectedValue.ToString();
                    parameters.Add(new KeyValuePair<string, string>("gender", gender));
                }


                if (cbxStatus.SelectedIndex > -1)
                {
                    string status = cbxStatus.SelectedValue.ToString();
                    parameters.Add(new KeyValuePair<string, string>("status", status));
                }

                // Thiết lập Content
                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                ListCandidateEntity result = JsonConvert.DeserializeObject<ListCandidateEntity>(responseContent);

                if (result.success != null && result.success.data != null)
                {
                    listContactJob = result.success.data.data;
                    foreach (var item in listContactJob)
                    {
                        if (listAllEmployee.ContainsKey(item.user_hiring))
                        {
                            item.hr_name = listAllEmployee[item.user_hiring];
                        }
                        item.id_process_interview = "-4";
                    }

                    tbTotalCandidateContactJob.Text = "(" + listContactJob.Count.ToString() + " ứng viên)";
                    icListCandidateContactJob.ItemsSource = listContactJob;
                }

            }
            catch
            {
                //MessageBox.Show("Lỗi đăng nhập, vui lòng thử lại!");
            }
        }


       /// ==========================================================

        private async void GetListRecruitmentNew()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.list_all_new;

                httpRequestMessage.RequestUri = new Uri(api);

                var parameters = new List<KeyValuePair<string, string>>();

                httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);

                // Thiết lập Content
                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                // Thực hiện Post
                var response = await httpClient.SendAsync(httpRequestMessage);

                var responseContent = await response.Content.ReadAsStringAsync();

                Entities.ManageRecuitmentEntities.PerformRecuitmentEntities.ListNewActive result = JsonConvert.DeserializeObject<Entities.ManageRecuitmentEntities.PerformRecuitmentEntities.ListNewActive>(responseContent);
                if (result.success == null || result.success.data.data.Count == 0)
                {
                    return;
                }

                //listRecruitmentNew.Add("", "Vị trí tuyển dụng");

                foreach (var item in result.success.data.data)
                {
                    listRecruitmentNew.Add(item.id, item.title);
                }
                cbxRecruitment.ItemsSource = listRecruitmentNew;
            }
            catch
            {
                //MessageBox.Show("Lỗi đăng nhập, vui lòng thử lại!");
            }
        }

        private void NavigateToWarehouse(object sender, MouseButtonEventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(HomeView))
                {
                    (window as HomeView).MainContent.Navigate(new CandidateWarehousePage(token, id, listRecruitmentNew, PerAdd, PerEdit, PerDel));
                }
            }
        }

        private void NavigateToCandidateDetail(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            string id = ((DataEntity)textBlock.DataContext).id;
            NavigateToCandidateDetail(id);
        }


        //Nhận hồ sơ
        private void NavigateToCandidateDetail(string id)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(HomeView))
                {
                    (window as HomeView).MainContent.Navigate(new CandidateDetailPage(token, id, listAllEmployee, listRecruitmentNew, PerEdit));
                }
            }
        }

        private void NavigateToCandidateProcessDetail(object sender, MouseButtonEventArgs e)
        {

            TextBlock textBlock = sender as TextBlock;
            DataEntity dataEntity = (DataEntity)textBlock.DataContext;
            string id = dataEntity.id;
            string process_id = dataEntity.id_process_interview;
            string name_process = dataEntity.name;
            NavigateToCandidateProcessDetail(id, process_id, name_process);
        }

        //xem chi tiết theo id
        private void NavigateToCandidateProcessDetail(string id, string process_id, string name_process)
        {

            List<Items> listItem = new List<Items>();
            listItem = listItem.Concat(listItem_cbx4_1).ToList();
            listItem = listItem.Concat(listItem_cbx4_process).ToList();
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(HomeView))
                {
                    (window as HomeView).MainContent.Navigate(new CandidateDetailProcessInterviewPage(token, id, listAllEmployee, process_id, name_process, listRecruitmentNew, PerEdit, listItem));
                }
            }
        }

        private void NavigateToCandidateDetailGetJob(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            string id = ((DataEntity)textBlock.DataContext).id;
            NavigateToCandidateDetailGetJob(id);
        }

        private void NavigateToCandidateDetailGetJob(string id)
        {
            List<Items> listItem = new List<Items>();
            listItem = listItem.Concat(listItem_cbx4_1).ToList();
            listItem = listItem.Concat(listItem_cbx4_process).ToList();

            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(HomeView))
                {
                    (window as HomeView).MainContent.Navigate(new CandidateDetailGetJobPage(token, id, listAllEmployee, listRecruitmentNew, PerEdit, listItem));
                }
            }
        }

        private void NavigateToCandidateDetailFailJob(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            string id = ((DataEntity)textBlock.DataContext).id;
            NavigateToCandidateDetailFailJob(id);
        }

        private void NavigateToCandidateDetailFailJob(string id)
        {
            List<Items> listItem = new List<Items>();
            listItem = listItem.Concat(listItem_cbx4_1).ToList();
            listItem = listItem.Concat(listItem_cbx4_process).ToList();
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(HomeView))
                {
                    (window as HomeView).MainContent.Navigate(new CandidateDetailFailJobPage(token, id, listAllEmployee, listRecruitmentNew, PerEdit, listItem));
                }
            }
        }

        private void NavigateToCandidateDetailCancelJob(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            string id = ((DataEntity)textBlock.DataContext).id;
            NavigateToCandidateDetailCancelJob(id);
        }

        private void NavigateToCandidateDetailCancelJob(string id)
        {
            List<Items> listItem = new List<Items>();
            listItem = listItem.Concat(listItem_cbx4_1).ToList();
            listItem = listItem.Concat(listItem_cbx4_process).ToList();
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(HomeView))
                {
                    (window as HomeView).MainContent.Navigate(new CandidateDetailCancelJobPage(token, id, listAllEmployee, listRecruitmentNew, PerEdit, listItem));
                }
            }
        }

        private void NavigateToCandidateDetailContractJob(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            string id = ((DataEntity)textBlock.DataContext).id;
            NavigateToCandidateDetailContractJob(id);
        }

        private void NavigateToCandidateDetailContractJob(string id)
        {
            List<Items> listItem = new List<Items>();
            listItem = listItem.Concat(listItem_cbx4_1).ToList();
            listItem = listItem.Concat(listItem_cbx4_process).ToList();
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(HomeView))
                {
                    (window as HomeView).MainContent.Navigate(new CandidateDetailContractJobPage(token, id, listAllEmployee, listRecruitmentNew, PerEdit, listItem));
                }
            }
        }

        //NavigateToCandidateDetailApplicationStage
        private void NavigateToCandidateDetailApplicationStage(object sender, MouseButtonEventArgs e)
        {

            TextBlock textBlock = sender as TextBlock;
            DataEntity dataEntity = (DataEntity)textBlock.DataContext;
            string id = dataEntity.id;
            string process_id = dataEntity.id_process_interview;
            NavigateToCandidateDetailApplicationStage(id, process_id);
        }

        private void NavigateToCandidateDetailApplicationStage(string id, string process_id)
        {

            List<Items> listItem = new List<Items>();
            listItem = listItem.Concat(listItem_cbx4_1).ToList();
            listItem = listItem.Concat(listItem_cbx4_process).ToList();
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(HomeView))
                {
                    (window as HomeView).MainContent.Navigate(new CandidateDetailApplicationStage(token, id, process_id, listAllEmployee, listRecruitmentNew, PerEdit, listItem));
                }
            }
        }
        //end

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

        private void ClickViewDetail(object sender, MouseButtonEventArgs e)
        {

            DataEntity dataEntity = (DataEntity)popupOption.DataContext;
            string id_process = dataEntity.id_process_interview;
            string id_candidate = dataEntity.id;
            string name_process = dataEntity.name;


            List<Items> listItem = new List<Items>();
            listItem = listItem.Concat(listItem_cbx4_1).ToList();
            listItem = listItem.Concat(listItem_cbx4_process).ToList();

            //int index1 = listItem.FindIndex(item => item.ID == id_process);


            switch (id_process)
            {
                case "0":
                    NavigateToCandidateDetail(id_candidate);
                    popupOption.Visibility = Visibility.Collapsed;
                    break;
                case "1":
                    NavigateToCandidateDetailGetJob(id_candidate);
                    popupOption.Visibility = Visibility.Collapsed;
                    break;

                case "-2":
                    NavigateToCandidateDetailFailJob(id_candidate);
                    popupOption.Visibility = Visibility.Collapsed;
                    break;
                case "-3":
                    NavigateToCandidateDetailCancelJob(id_candidate);
                    popupOption.Visibility = Visibility.Collapsed;
                    break;
                case "-4":
                    NavigateToCandidateDetailContractJob(id_candidate);
                    popupOption.Visibility = Visibility.Collapsed;
                    break;
                default:
                    NavigateToCandidateProcessDetail(id_candidate, id_process, name_process);
                    popupOption.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void ClickDelete(object sender, MouseButtonEventArgs e)
        {
            DataEntity dataEntity = (DataEntity)popupOption.DataContext;
            string id_candidate = dataEntity.id;
            string id_process = dataEntity.id_process_interview;
            string name = dataEntity.name;
            DeleteCandidatePopup deleteCandidatePopup = new DeleteCandidatePopup(token, id_candidate, name, id_process);
            onShowPopup(deleteCandidatePopup);
            //DeleteCandidate(id_candidate, id_process);
            popupOption.Visibility = Visibility.Collapsed;
        }

        private void TransportFile(object sender, MouseButtonEventArgs e)
        {
            DataEntity dataEntity = (DataEntity)popupOption.DataContext;
            string id_candidate = dataEntity.id;
            string id_process = dataEntity.id_process_interview;
            popupOption.Visibility = Visibility.Collapsed;
            List<Items> listOption = new List<Items>();
            if (Convert.ToInt32(id_process) < 0)
            {
                listOption = listItem_cbx4_12;
                listOption = listOption.Concat(listItem_cbx4_process).ToList();
                int idx = 1;
                foreach (var item in listOption)
                {
                    if (item.ID == id_process && item.ID == id_candidate)
                    {
                        idx = listOption.IndexOf(item);
                        break;
                    }
                }
            }
            else
            {
                listOption = listItem_cbx4_1;

                //listOption = listOption.Concat(listItem_cbx4_process).ToList();
                //listOption = listOption.Concat(listItem_cbx4_2).ToList();
                //listOption = listOption.Concat(listItem_cbx4_1).ToList();
                listOption = listOption.Concat(listItem_cbx4_process).ToList();
                int idx = 0;
                foreach (var item in listOption)
                {
                    if (item.ID == id_process && item.ID == id_candidate)
                    {
                        idx = listOption.IndexOf(item);
                        break;
                    }
                }
                listOption = listOption.GetRange(idx + 1, listOption.Count - idx - 1);
            }

            if (listOption.Count == 0) return;
            ChooseTransportProcess chooseTransportProcess = new ChooseTransportProcess(listOption);
            onShowPopup(chooseTransportProcess);
            popupOption.Visibility = Visibility.Collapsed;
        }

        private void ReloadProcessById(string id_process)
        {
            switch (id_process)
            {
                case "0":
                    GetListCandidate();
                    break;
                default:
                    GetAllData();
                    break;
            }
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

        private void cbx4_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //string id_process = ((Items)cbx4.SelectedItem).ID;
        }

        private void ShowPopupAddProcess(object sender, MouseButtonEventArgs e)
        {
            List<Items> listItem = new List<Items>();
            listItem = listItem.Concat(listItem_cbx4_1).ToList();
            listItem = listItem.Concat(listItem_cbx4_process).ToList();
            MoreCandidatePopup moreCandidatePopup = new MoreCandidatePopup(token, listItem);
            onShowPopup(moreCandidatePopup);
        }


        //show popup chọn giai đoạn xuất excel


        private void clickExportExcel(object sender, MouseButtonEventArgs e)
        {
            List<Items> listItem = new List<Items>();
            listItem = listItem.Concat(listItem_cbx4_1).ToList();
            listItem = listItem.Concat(listItem_cbx4_process).ToList();

            ExporFileExcelPopup moreCandidatePopup = new ExporFileExcelPopup(token, listItem, listProfile,listGetJob, listFailJob, listCancelJob, listContactJob);
            onShowPopup(moreCandidatePopup);
        }

        // mode = 0 : ko load lại, 1: load lại
        private void HidePopup(int mode, int id_process)
        {
            if (mode == 1)
            {
                ReloadProcessById(id_process.ToString());
            }
            onShowPopup("");
        }

        private void HidePopup2(int mode, int id_process_old, int id_process_new)
        {
            if (mode == 1)
            {
                ReloadProcessById(id_process_old.ToString());
                if (id_process_old != id_process_new)
                {
                    ReloadProcessById(id_process_new.ToString());
                }
            }
            onShowPopup("");
        }

        private void OnShowTransportPopup(int process_id)
        {
            onShowPopup("1"); // ẩn không hiệu ứng
            DataEntity dataEntity = (DataEntity)popupOption.DataContext;
            string id_candidate = dataEntity.id;
            string id_process_old = dataEntity.id_process_interview;
            switch (process_id)
            {
                case 0:
                    break;
                case 1:
                    GetJobPopup getJobPopup = new GetJobPopup(token, id_candidate, listAllEmployee, listRecruitmentNew, id_process_old, process_id.ToString());
                    onShowPopup(getJobPopup);
                    break;
                case 2:
                    FailJobPopup failJobPopup = new FailJobPopup(token, id_candidate, listAllEmployee, listRecruitmentNew, id_process_old, process_id.ToString());
                    onShowPopup(failJobPopup);
                    break;
                case 3:
                    CancelJobPopup cancelJobPopup = new CancelJobPopup(token, id_candidate, listAllEmployee, listRecruitmentNew, id_process_old, process_id.ToString());
                    onShowPopup(cancelJobPopup);
                    break;
                case 4:
                    ContractJobPopup contractJobPopup = new ContractJobPopup(token, id_candidate, listAllEmployee, listRecruitmentNew, id_process_old, process_id.ToString());
                    onShowPopup(contractJobPopup);
                    break;
                default:
                    InterviewPopup interviewPopup = new InterviewPopup(token, id_candidate, listAllEmployee, listRecruitmentNew, id_process_old, process_id.ToString());
                    onShowPopup(interviewPopup);
                    break;
            }
        }

        private void ShowPopupDeleteProcess(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            DataList dataEntity = (DataList)textBlock.DataContext;
            string id_process = dataEntity.id;
            string name_process = dataEntity.name;
            DeleteLocationPopup deleteLocationPopup = new DeleteLocationPopup(token, id_process, name_process);
            onShowPopup(deleteLocationPopup);
        }

        private void ShowPopupEditProcess(object sender, MouseButtonEventArgs e)
        {
            DataList dataEntity = (DataList)(sender as TextBlock).DataContext;
            List<Items> listItem = new List<Items>();
            listItem = listItem.Concat(listItem_cbx4_1).ToList();
            listItem = listItem.Concat(listItem_cbx4_process).ToList();
            EditCandidatePopup editCandidatePopup = new EditCandidatePopup(token, listItem, dataEntity.id, dataEntity.name, dataEntity.type);
            onShowPopup(editCandidatePopup);
        }

        private void tbName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                GetAllData();
            }
        }

        private void cbxUserHiring_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                cbxUserHiring.SelectedIndex = -1;
                string textSearch = cbxUserHiring.Text;
                cbxUserHiring.Items.Refresh();
                cbxUserHiring.ItemsSource = listAllEmployee.Where(t => t.Value.ToLower().Contains(textSearch.ToLower()));
            }
        }

        private void cbxUserHiring_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            cbxUserHiring.SelectedIndex = -1;
            string textSearch = cbxUserHiring.Text + e.Text;
            cbxUserHiring.IsDropDownOpen = true;
            if (textSearch == "")
            {
                cbxUserHiring.Text = "";
                cbxUserHiring.Items.Refresh();
                cbxUserHiring.ItemsSource = listAllEmployee;
                cbxUserHiring.SelectedIndex = -1;
            }
            else
            {
                cbxUserHiring.ItemsSource = "";
                cbxUserHiring.Items.Refresh();
                cbxUserHiring.ItemsSource = listAllEmployee.Where(t => t.Value.ToLower().Contains(textSearch.ToLower()));
            }
        }

        private void cbxRecruitment_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            cbxRecruitment.SelectedIndex = -1;
            string textSearch = cbxRecruitment.Text + e.Text;
            cbxRecruitment.IsDropDownOpen = true;
            if (textSearch == "")
            {
                cbxRecruitment.Text = "";
                cbxRecruitment.Items.Refresh();
                cbxRecruitment.ItemsSource = listRecruitmentNew;
                cbxRecruitment.SelectedIndex = -1;
            }
            else
            {
                cbxRecruitment.ItemsSource = "";
                cbxRecruitment.Items.Refresh();
                cbxRecruitment.ItemsSource = listRecruitmentNew.Where(t => t.Value.ToLower().Contains(textSearch.ToLower()));
            }
        }

        private void cbxRecruitment_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                cbxRecruitment.SelectedIndex = -1;
                string textSearch = cbxRecruitment.Text;
                cbxRecruitment.Items.Refresh();
                cbxRecruitment.ItemsSource = listRecruitmentNew.Where(t => t.Value.ToLower().Contains(textSearch.ToLower()));
            }
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                scroll.ScrollToVerticalOffset(scroll.VerticalOffset);
                scroll1.ScrollToHorizontalOffset(scroll1.HorizontalOffset - e.Delta);
            }
            else
                scroll.ScrollToVerticalOffset(scroll.VerticalOffset - e.Delta);
        }


        //private void Border_MouseUp(object sender, MouseButtonEventArgs e)
        //{
        //    dp1.IsDropDownOpen = true;
        //}

        //private void Border_MouseUp1(object sender, MouseButtonEventArgs e)
        //{
        //    dp2.IsDropDownOpen = true;
        //}

        private void ShowPopupAddCandidate(object sender, MouseButtonEventArgs e)
        {
            CandidatePofilePopup candidatePofilePopup = new CandidatePofilePopup(token, listAllEmployee, listRecruitmentNew);
            onShowPopup(candidatePofilePopup);
        }

        private void ClickSearch(object sender, MouseButtonEventArgs e)
        {
            GetAllData();
        }
    }

    // item combobox4
    public class Items
    {
        public string ID { get; set; }
        public string Name { get; set; }
    }
}
