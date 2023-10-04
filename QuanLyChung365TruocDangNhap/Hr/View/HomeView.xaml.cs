using QuanLyChung365TruocDangNhap.Hr.Components;
using QuanLyChung365TruocDangNhap.Hr.Entities;
using QuanLyChung365TruocDangNhap.Hr.Entities.LoginEntity;
using QuanLyChung365TruocDangNhap.Hr.Pages.AdministrationPages.EmployeeManagerPages;
using QuanLyChung365TruocDangNhap.Hr.Pages.AdministrationPages.PersonalChangePages;
using QuanLyChung365TruocDangNhap.Hr.Pages.AdministrationPages.RegulationsPoliciesPages;
using QuanLyChung365TruocDangNhap.Hr.Pages.HomePages;
using QuanLyChung365TruocDangNhap.Hr.Pages.ManageRecruitmentPages.ListOfCandidatesPages;
using QuanLyChung365TruocDangNhap.Hr.Pages.ManageRecruitmentPages.PerformRecruitmentPages;
using QuanLyChung365TruocDangNhap.Hr.Pages.ManageRecruitmentPages.RecruitmentProcessPages;
using QuanLyChung365TruocDangNhap.Hr.Pages.OrganizationalChart;
using QuanLyChung365TruocDangNhap.Hr.Pages.RecentDeletedData;
using QuanLyChung365TruocDangNhap.Hr.Pages.SalaryAndBenefitPages.BonusPages;
using QuanLyChung365TruocDangNhap.Hr.Pages.SalaryAndBenefitPages.ViolationsPages;
using QuanLyChung365TruocDangNhap.Hr.Pages.TrainingAndDevelopingPages.JobPositionPages;
using QuanLyChung365TruocDangNhap.Hr.Pages.TrainingAndDevelopingPages.TrainingProcessPages;
using QuanLyChung365TruocDangNhap.Hr.Popups.OverviewPopups;
using QuanLyChung365TruocDangNhap.Hr.Popups.RecruitmentManagementPopups.ListOfCandidatesPopups.TransportPopups;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuanLyChung365TruocDangNhap.Hr.View
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : Window, INotifyPropertyChanged
    {
        Thickness margin;
        LoginEmployeeEntity loginEmployeeEntity;
        LoginCompanyEntity loginCompanyEntity;
        private string mainItem;
        public string MainItem
        {
            get { return mainItem; }
            set
            {
                mainItem = value;
                OnPropertyChanged("MainItem");
            }
        }

        private string extraItem;

        public string ExtraItem
        {
            get { return extraItem; }
            set
            {
                extraItem = value;
                OnPropertyChanged("ExtraItem");
            }
        }

        public string token;
        public string Token
        {
            get { return token; }
            set
            {
                token = value;
                OnPropertyChanged("Token");
            }
        }

        public string userName;
        public string UserName
        {
            get { return userName; }
            set
            {
                userName = value;
                OnPropertyChanged("UserName");
            }
        }

        // id com
        public string id;
        public string Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }
        public string _pass;
        public string pass
        {
            get { return _pass; }
            set
            {
                _pass = value;
                OnPropertyChanged("pass");
            }
        }

        // id_ep
        public string ep_id;
        public string EpId
        {
            get { return ep_id; }
            set
            {
                ep_id = value;
                OnPropertyChanged("EpId");
            }
        }

        public int typeUser;
        public int TypeUser
        {
            get { return typeUser; }
            set
            {
                typeUser = value;
                OnPropertyChanged("TypeUser");
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
        public HomeView()
        {
            InitializeComponent();
            //Setup();
        }
        //nhân viên
        public HomeView(LoginEmployeeEntity loginEmployeeEntity)
        {
            InitializeComponent();
            this.loginEmployeeEntity = loginEmployeeEntity;
            Header header = new Header(loginEmployeeEntity);
            gridHeader.Children.Add(header);
            this.Token = loginEmployeeEntity.data.data.access_token;
            this.UserName = loginEmployeeEntity.data.data.user_info.ep_name;
            this.Id = loginEmployeeEntity.data.data.user_info.com_id;
            this.EpId = loginEmployeeEntity.data.data.user_info.ep_id;
            this.pass = loginEmployeeEntity.data.data.user_info.ep_pass;
            TypeUser = 1;
            DataContext = this;
            Setup();

            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy"; //for the second type
            Thread.CurrentThread.CurrentCulture = ci;
        }
        //công ty
        public HomeView(LoginCompanyEntity loginCompanyEntity)
        {
            InitializeComponent();
            this.loginCompanyEntity = loginCompanyEntity;
            Header header = new Header(loginCompanyEntity);
            gridHeader.Children.Add(header);
            this.Token = loginCompanyEntity.data.data.access_token;
            this.UserName = loginCompanyEntity.data.data.user_info.com_name;
            this.Id = loginCompanyEntity.data.data.user_info.com_id;
            this.EpId = "";
            this.pass = loginCompanyEntity.data.data.user_info.com_pass;
            TypeUser = 2;
            DataContext = this;
            Setup();

            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy"; //for the second type
            Thread.CurrentThread.CurrentCulture = ci;
        }

        private void Setup()
        {
            
            MainContent.NavigationService.Navigate(new HomePage(Token,Id, UserName));
            MainItem = "Home";
            ExtraItem = "";
            DataContext = this;

            MenuPopups.onShowPopup += ShowLogoutPopup;
            ListOfCandidatesPage.onShowPopup += ShowPopup;
            RecruitmentProcessPage.onShowPopup += ShowPopup;
            CandidateWarehousePage.onShowPopup += ShowPopup;
            RecruitmentProcessDetailPage.onShowPopup += ShowPopup;
            CandidateDetailPage.onShowPopup += ShowPopup;
            PersonalBonusPage.onShowPopup += ShowPopup;
            ListViolationsPage.onShowPopup += ShowPopup;
            GroupBonusPage.onShowPopup += ShowPopup;
            CandidateDetailProcessInterviewPage.onShowPopup += ShowPopup;
            CandidateDetailGetJobPage.onShowPopup += ShowPopup;
            CandidateDetailFailJobPage.onShowPopup += ShowPopup;
            CandidateDetailCancelJobPage.onShowPopup += ShowPopup;
            CandidateDetailContractJobPage.onShowPopup += ShowPopup;
            ListOfEmployeePage.onShowPopup += ShowPopup;
            WorkingRegulationsPage.onShowPopup += ShowPopup;
            EmployeePolicyPage.onShowPopup += ShowPopup;
            PersonalViolationsPage.onShowPopup += ShowPopup;
            GroupViolationsPage.onShowPopup += ShowPopup;
            ListAchievementsPage.onShowPopup += ShowPopup;
            JobPositionPage.onShowPopup += ShowPopup;
            TrainingProcessPage.onShowPopup += ShowPopup;
            TrainingProcessDetailPage.onShowPopup += ShowPopup;
            DeleteData3Page.onShowPopup += ShowPopup;
            RecruitmentInformationPage.onShowPopup += ShowPopup;
            AppointmentPage.onShowPopup += ShowPopup;
            WorkingRotationPage.onShowPopup += ShowPopup;
            DownsizingPage.onShowPopup += ShowPopup;
            RightToUse.onShowPopup += ShowPopup;
            OrganisationalStructureDiagram.onShowPopup += ShowPopup;
            BreakTheRules.onShowPopup += ShowPopup;
            //PositionChart.onShowPopup += ShowPopup;

            //ChooseTransportProcess.hidePopup1 += ShowPopup;
        }

        private void Minimimize_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void NomarlSize_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IsFull == 0) IsFull = 1;
            else IsFull = 0;
            NomarlSize.Visibility = Visibility.Collapsed;
            Maximimize.Visibility = Visibility.Visible;
            logoInTitle.Margin = new Thickness(0, 0, 0, 0);
            pageTitle.Height = 40;
            //pageDisplay.Margin = new Thickness(0, 25, 0, 0);
        }
        private int _IsFull = 0;

        public int IsFull
        {
            get { return _IsFull; }
            set
            {
                _IsFull = value;
                var workingArea = System.Windows.SystemParameters.WorkArea;
                switch (IsFull)
                {
                    case 0:
                        this.WindowState = WindowState.Normal;
                        Width = workingArea.Right - 180;
                        Height = workingArea.Bottom - 100;
                        Left = (workingArea.Right / 2) - (this.ActualWidth / 2);
                        Top = (workingArea.Bottom / 2) - (this.ActualHeight / 2);
                        this.ResizeMode = ResizeMode.CanResize;
                        break;
                    case 1:
                        this.WindowState = WindowState.Normal;
                        Left = workingArea.TopLeft.X;
                        Top = workingArea.TopLeft.Y;
                        Width = workingArea.Width;
                        Height = workingArea.Height;
                        this.ResizeMode = ResizeMode.NoResize;
                        break;
                    default:
                        break;
                }
            }
        }
        private void Maximimize_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IsFull == 0) IsFull = 1;
            else IsFull = 0;
            NomarlSize.Visibility = Visibility.Visible;
            Maximimize.Visibility = Visibility.Collapsed;
            pageTitle.Height = 45;
            logoInTitle.Margin = new Thickness(0, 5, 0, 0);
            //pageDisplay.Margin = new Thickness(0, 30, 0, 0);
        }

        private void CloseWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void pageTitle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        public void ShowPopup(object obj)
        {
            if (obj.ToString() == "")
            {
                HidePopup();
            }
            else if (obj.ToString() == "1") // ẩn không hiệu ứng
            {
                if (svPopup.Children.Count > 1)
                {
                    int count = svPopup.Children.Count;
                    for (int i = count - 1; i > 0; i--)
                    {
                        svPopup.Children.RemoveAt(i);
                    }
                    gridPopup.Visibility = Visibility.Collapsed;
                }
            } 
            else
            {
                svPopup.Children.Add((UserControl)obj);
                gridPopup.Visibility = Visibility.Visible;

                Storyboard storyboard = new Storyboard();
                TimeSpan duration = TimeSpan.FromSeconds(0.3);
                DoubleAnimation animation = new DoubleAnimation();
                animation.From = 0.0;
                animation.To = 1.0;
                animation.Duration = new Duration(duration);
                Storyboard.SetTargetName(animation, "scPopup");
                Storyboard.SetTargetProperty(animation, new PropertyPath(Control.OpacityProperty));
                storyboard.Children.Add(animation);
                storyboard.Begin(this);

                var sb = new Storyboard();
                var ta = new ThicknessAnimation();
                ta.BeginTime = new TimeSpan(0);
                ta.SetValue(Storyboard.TargetNameProperty, "scPopup");
                Storyboard.SetTargetProperty(ta, new PropertyPath(MarginProperty));

                ta.From = new Thickness(0, -300, 0, 300);
                ta.To = new Thickness(0, 20, 0, 0);
                ta.Duration = new Duration(TimeSpan.FromSeconds(0.25));
                sb.Children.Add(ta);
                sb.Begin(this);


            }
        }

        public void HidePopup()
        {
            if(gridPopup.Visibility == Visibility.Visible)
            {
                EventHandler onComplete = null;
                onComplete = (s, e) =>
                {
                    if (svPopup.Children.Count > 1)
                    {
                        int count = svPopup.Children.Count;
                        for (int i = count - 1; i > 0; i--)
                        {
                            svPopup.Children.RemoveAt(i);
                        }
                    }
                    //svPopup.Children.Clear();
                    //svPopup.Children.RemoveAt(1);
                    gridPopup.Visibility = Visibility.Collapsed;
                };
                Storyboard storyboard = new Storyboard();
                TimeSpan duration = TimeSpan.FromSeconds(0.3);
                DoubleAnimation animation = new DoubleAnimation();
                animation.From = 1.0;
                animation.To = 0.0;
                animation.Duration = new Duration(duration);
                Storyboard.SetTargetName(animation, "scPopup");
                Storyboard.SetTargetProperty(animation, new PropertyPath(Control.OpacityProperty));
                storyboard.Children.Add(animation);
                storyboard.Begin(this);

                var sb = new Storyboard();
                var ta = new ThicknessAnimation();
                ta.BeginTime = new TimeSpan(0);
                ta.SetValue(Storyboard.TargetNameProperty, "scPopup");
                Storyboard.SetTargetProperty(ta, new PropertyPath(MarginProperty));

                ta.From = new Thickness(0, 20, 0, 0);
                ta.To = new Thickness(0, -300, 0, 300);
                ta.Duration = new Duration(TimeSpan.FromSeconds(0.25));
                sb.Children.Add(ta);
                sb.Completed += onComplete;
                sb.Begin(this);
                //if (svPopup.Children.Count > 1)
                //    svPopup.Children.RemoveAt(1);
                //gridPopup.Visibility = Visibility.Collapsed;
            }

            
        }



        private void HomeViewSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (homeView.ActualWidth < 1366)
            {
                margin = new Thickness(60 - (1366 - homeView.ActualWidth) / 600 * 40, 0, 60 - (1366 - homeView.ActualWidth) / 600 * 40, 0);
            }
            else
            {
                margin = new Thickness(60, 0, 60, 0);
            }
            gridContent.Margin = margin;

            if (homeView.ActualWidth < 1058)
            {
                sideBar.Visibility = Visibility.Collapsed;
                iconShowSideBar.Visibility = Visibility.Visible;
            }
            else
            {
                sideBar.Visibility = Visibility.Visible;
                iconShowSideBar.Visibility = Visibility.Collapsed;
                CloseSideBar();
            }
        }

        private void CloseSideBar(object sender, MouseButtonEventArgs e)
        {
            CloseSideBar();
        }

        private void ShowSideBar(object sender, MouseButtonEventArgs e)
        {
            ShowSideBar();
        }

        private void CloseSideBar()
        {
            containerSideBar.Visibility = Visibility.Collapsed;
        }

        private void ShowSideBar()
        {
            containerSideBar.Visibility = Visibility.Visible;
        }

        private void gridPopup_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            HidePopup();
        }

        private void gridPopup2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            gridLogoutPopup.Visibility = Visibility.Collapsed;
        }

        private void ShowLogoutPopup()
        {
            gridLogoutPopup.Visibility = Visibility.Visible;
        }

        private void Cancel(object sender, MouseButtonEventArgs e)
        {
            gridLogoutPopup.Visibility = Visibility.Collapsed;
        }

        private void LogOut(object sender, MouseButtonEventArgs e)
        {
            //gridLogoutPopup.Visibility = Visibility.Collapsed;
            //svPopup.Children.Clear();
            //gridPopup.Children.Clear();
            //container.Children.Clear();
            //LoginView loginView = new LoginView();
            //loginView.Show();
            //this.Close();
            Application.Current.Shutdown();
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
        }
    }
}
