using QuanLyChung365TruocDangNhap.Hr.Entities.RecentDeletedData;
using QuanLyChung365TruocDangNhap.Hr.Popups.RecentDeletedDataPopups;
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

namespace QuanLyChung365TruocDangNhap.Hr.Pages.RecentDeletedData
{
    /// <summary>
    /// Interaction logic for DeleteData3Page.xaml
    /// </summary>
    public partial class DeleteData3Page : Page, INotifyPropertyChanged
    {
        string token;

        List<DataEntity> listRecruitment;
        List<DataEntity> listRecruitmentNew;
        List<DataEntity> listJobDescription;
        List<DataEntity> listTrainingProcess;
        List<DataEntity> listProvision;
        List<DataEntity> listPolicy;
        string list_recruitment = "";
        string list_recruitment_new = "";
        string list_job_desc = "";
        string list_training_process = "";
        string list_provision = "";
        string list_policy = "";

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
        public DeleteData3Page(string token, bool perAdd, bool perEdit, bool perDel)
        {
            InitializeComponent();
            this.token = token;
            this.PerAdd = perAdd;
            this.PerEdit = perEdit;
            this.PerDel = perDel;
            DataContext = this;
            GetData();

            DeleteNoDataPopup.hidePopup += HidePopup;
            DeleteItemPopup.hidePopup += HidePopup;
            RestoreNoDataPopup.hidePopup += HidePopup;
            RestoreInventoryPopup.hidePopup += HidePopup;
        }

        private async void GetData()
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.listDetailDelete;

                httpRequestMessage.RequestUri = new Uri(api);

                var parameters = new List<KeyValuePair<string, string>>();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                parameters.Add(new KeyValuePair<string, string>("keyword", tbSearch.Text));

                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;

                var response = await httpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                RecentDeletedDataEntity result = JsonConvert.DeserializeObject<RecentDeletedDataEntity>(responseContent);

                if (result.result)
                {
                    // danh sách quy trình tuyển dụng
                    listRecruitment = result.success.data.list_delete_recuitment.data;
                    if(listRecruitment.Count > 0)
                    {
                        grNoDataRecruitment.Visibility = Visibility.Collapsed;
                        foreach (var item in listRecruitment)
                        {
                            item.full_id = "QTTĐ" + item.id;
                        }
                    }
                    else
                    {
                        grNoDataRecruitment.Visibility = Visibility.Visible;
                    }
                    icListRecruitment.Items.Refresh();
                    icListRecruitment.ItemsSource = listRecruitment;


                    // danh sách tin tuyển dụng
                    listRecruitmentNew = result.success.data.list_delete_recuitment_new.data;
                    if(listRecruitmentNew.Count > 0)
                    {
                        grNoDataRecruitmentNew.Visibility = Visibility.Collapsed;
                        foreach (var item in listRecruitmentNew)
                        {
                            item.full_id = "TTD" + item.id;
                        }
                    }
                    else
                    {
                        grNoDataRecruitmentNew.Visibility = Visibility.Visible;
                    }
                    icListRecruitmentNew.Items.Refresh();
                    icListRecruitmentNew.ItemsSource = listRecruitmentNew;


                    // danh sách vị trí công việc
                    listJobDescription = result.success.data.list_delete_job_description.data;
                    if(listJobDescription.Count > 0)
                    {
                        grNoDataJobDescription.Visibility = Visibility.Collapsed;
                        foreach (var item in listJobDescription)
                        {
                            item.full_id = "VT" + item.id;
                        }
                    }
                    else
                    {
                        grNoDataJobDescription.Visibility = Visibility.Visible;
                    }
                    icListJobDescription.Items.Refresh();
                    icListJobDescription.ItemsSource = listJobDescription;


                    // danh sách quy trình đào tạo
                    listTrainingProcess = result.success.data.list_delete_training_process.data;
                    if (listTrainingProcess.Count > 0)
                    {
                        grNoDataTrainingProcess.Visibility = Visibility.Collapsed;
                        foreach (var item in listTrainingProcess)
                        {
                            item.full_id = "QTĐT" + item.id;
                        }
                    }
                    else
                    {
                        grNoDataTrainingProcess.Visibility = Visibility.Visible;
                    }
                    icListTrainingProcess.Items.Refresh();
                    icListTrainingProcess.ItemsSource = listTrainingProcess;


                    // danh sách quy định làm việc
                    listProvision = result.success.data.list_delete_provision.data;
                    if(listProvision.Count > 0)
                    {
                        grNoDataProvision.Visibility = Visibility.Collapsed;
                        foreach (var item in listProvision)
                        {
                            item.full_id = "QĐ" + item.id;
                        }
                    }
                    else
                    {
                        grNoDataProvision.Visibility = Visibility.Visible;
                    }
                    icListProvision.Items.Refresh();
                    icListProvision.ItemsSource = listProvision;


                    // danh sách chính sách nhân viên
                    listPolicy = result.success.data.list_delete_employe_policy.data;
                    if(listPolicy.Count > 0)
                    {
                        grNoDataPolicy.Visibility = Visibility.Collapsed;
                        foreach (var item in listPolicy)
                        {
                            item.full_id = "QĐ" + item.id;
                        }
                    }
                    else
                    {
                        grNoDataPolicy.Visibility = Visibility.Visible;
                    }
                    icListPolicy.Items.Refresh();
                    icListPolicy.ItemsSource = listPolicy;

                }
            }
            catch (Exception)
            {
                //MessageBox.Show("hihiih");
            }
        }

        private void CheckAllRecruitmentChanged(object sender, RoutedEventArgs e)
        {
            if (cbRecruitment.IsChecked == true)
            {
                foreach(var item in listRecruitment)
                {
                    item.isCheck = true;
                }
            }
            else
            {
                foreach (var item in listRecruitment)
                {
                    item.isCheck = false;
                }
            }

            icListRecruitment.Items.Refresh();
            icListRecruitment.ItemsSource = listRecruitment;
        }

        private void ShowHideListRecruitment(object sender, MouseButtonEventArgs e)
        {
            if(icListRecruitment.Visibility == Visibility.Visible)
            {
                icListRecruitment.Visibility = Visibility.Collapsed;
            }
            else
            {
                icListRecruitment.Visibility = Visibility.Visible;
            }
        }

        private void CheckAllRecruitmentNewChanged(object sender, RoutedEventArgs e)
        {
            if (cbRecruitmentNew.IsChecked == true)
            {
                foreach (var item in listRecruitmentNew)
                {
                    item.isCheck = true;
                }
            }
            else
            {
                foreach (var item in listRecruitmentNew)
                {
                    item.isCheck = false;
                }
            }

            icListRecruitmentNew.Items.Refresh();
            icListRecruitmentNew.ItemsSource = listRecruitmentNew;
        }

        private void ShowHideListRecruitmentNew(object sender, MouseButtonEventArgs e)
        {
            if (icListRecruitmentNew.Visibility == Visibility.Visible)
            {
                icListRecruitmentNew.Visibility = Visibility.Collapsed;
            }
            else
            {
                icListRecruitmentNew.Visibility = Visibility.Visible;
            }
        }

        private void CheckAllJobDescriptionChanged(object sender, RoutedEventArgs e)
        {
            if (cbJobDescription.IsChecked == true)
            {
                foreach (var item in listJobDescription)
                {
                    item.isCheck = true;
                }
            }
            else
            {
                foreach (var item in listJobDescription)
                {
                    item.isCheck = false;
                }
            }

            icListJobDescription.Items.Refresh();
            icListJobDescription.ItemsSource = listJobDescription;
        }

        private void ShowHideListJobDescription(object sender, MouseButtonEventArgs e)
        {
            if (icListJobDescription.Visibility == Visibility.Visible)
            {
                icListJobDescription.Visibility = Visibility.Collapsed;
            }
            else
            {
                icListJobDescription.Visibility = Visibility.Visible;
            }
        }

        private void CheckAllTrainingProcessChanged(object sender, RoutedEventArgs e)
        {
            if (cbTrainingProcess.IsChecked == true)
            {
                foreach (var item in listTrainingProcess)
                {
                    item.isCheck = true;
                }
            }
            else
            {
                foreach (var item in listTrainingProcess)
                {
                    item.isCheck = false;
                }
            }

            icListTrainingProcess.Items.Refresh();
            icListTrainingProcess.ItemsSource = listTrainingProcess;
        }

        private void ShowHideListTrainingProcess(object sender, MouseButtonEventArgs e)
        {
            if (icListTrainingProcess.Visibility == Visibility.Visible)
            {
                icListTrainingProcess.Visibility = Visibility.Collapsed;
            }
            else
            {
                icListTrainingProcess.Visibility = Visibility.Visible;
            }
        }

        private void CheckAllProvisionChanged(object sender, RoutedEventArgs e)
        {
            if (cbProvision.IsChecked == true)
            {
                foreach (var item in listProvision)
                {
                    item.isCheck = true;
                }
            }
            else
            {
                foreach (var item in listProvision)
                {
                    item.isCheck = false;
                }
            }

            icListProvision.Items.Refresh();
            icListProvision.ItemsSource = listProvision;
        }

        private void ShowHideListProvision(object sender, MouseButtonEventArgs e)
        {
            if (icListProvision.Visibility == Visibility.Visible)
            {
                icListProvision.Visibility = Visibility.Collapsed;
            }
            else
            {
                icListProvision.Visibility = Visibility.Visible;
            }
        }

        private void CheckAllPolicyChanged(object sender, RoutedEventArgs e)
        {
            if (cbPolicy.IsChecked == true)
            {
                foreach (var item in listPolicy)
                {
                    item.isCheck = true;
                }
            }
            else
            {
                foreach (var item in listPolicy)
                {
                    item.isCheck = false;
                }
            }

            icListPolicy.Items.Refresh();
            icListPolicy.ItemsSource = listPolicy;
        }

        private void ShowHideListPolicy(object sender, MouseButtonEventArgs e)
        {
            if (icListPolicy.Visibility == Visibility.Visible)
            {
                icListPolicy.Visibility = Visibility.Collapsed;
            }
            else
            {
                icListPolicy.Visibility = Visibility.Visible;
            }
        }

        private void tbSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                GetData();
            }
        }

        private void ClickSearch(object sender, MouseButtonEventArgs e)
        {
            GetData();
        }

        private void HidePopup(int mode)
        {
            if(mode == 1)
            {
                GetData();
            }

            onShowPopup("");
        }
        private void ClickDeleteData(object sender, MouseButtonEventArgs e)
        {
            bool isChecked = CheckDataIsCheck();
            if (isChecked)
            {
                DeleteItemPopup deleteItemPopup = new DeleteItemPopup(token, list_recruitment, list_recruitment_new, list_job_desc, list_training_process, list_provision, list_policy);
                onShowPopup(deleteItemPopup);
            }
            else
            {
                DeleteNoDataPopup deleteNoDataPopup = new DeleteNoDataPopup();
                onShowPopup(deleteNoDataPopup);
            }
        }

        private bool CheckDataIsCheck()
        {
            bool isChecked = false;
            list_recruitment = "";
            list_recruitment_new = "";
            list_job_desc = "";
            list_training_process = "";
            list_provision = "";
            list_policy = "";
            foreach (var item in listRecruitment)
            {
                if (item.isCheck == true)
                {
                    list_recruitment += item.id + ", ";
                    isChecked = true;
                }
            }

            foreach (var item in listRecruitmentNew)
            {
                if (item.isCheck == true)
                {
                    list_recruitment_new += item.id + ", ";
                    isChecked = true;
                }
            }

            foreach (var item in listJobDescription)
            {
                if (item.isCheck == true)
                {
                    list_job_desc += item.id + ", ";
                    isChecked = true;
                }
            }

            foreach (var item in listTrainingProcess)
            {
                if (item.isCheck == true)
                {
                    list_training_process += item.id + ", ";
                    isChecked = true;
                }
            }

            foreach (var item in listProvision)
            {
                if (item.isCheck == true)
                {
                    list_provision += item.id + ",";
                    isChecked = true;
                }
            }

            foreach (var item in listPolicy)
            {
                if (item.isCheck == true)
                {
                    list_policy += item.id + ", ";
                    isChecked = true;
                }
            }

            return isChecked;
        }

        private void ClickRestore(object sender, MouseButtonEventArgs e)
        {
            bool isChecked = CheckDataIsCheck();
            if (isChecked)
            {
                RestoreInventoryPopup restoreInventoryPopup = new RestoreInventoryPopup(token, list_recruitment, list_recruitment_new, list_job_desc, list_training_process, list_provision, list_policy);
                onShowPopup(restoreInventoryPopup);
            }
            else
            {
                RestoreNoDataPopup restoreNoDataPopup = new RestoreNoDataPopup();
                onShowPopup(restoreNoDataPopup);
            }
        }
    }
}
