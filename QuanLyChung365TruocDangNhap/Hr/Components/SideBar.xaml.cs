using QuanLyChung365TruocDangNhap.Hr.Entities.HomeEntity;
using QuanLyChung365TruocDangNhap.Hr.Entities.SettingEntities;
using QuanLyChung365TruocDangNhap.Hr.Pages.AdministrationPages.EmployeeManagerPages;
using QuanLyChung365TruocDangNhap.Hr.Pages.AdministrationPages.PersonalChangePages;
using QuanLyChung365TruocDangNhap.Hr.Pages.AdministrationPages.RegulationsPoliciesPages;
using QuanLyChung365TruocDangNhap.Hr.Pages.HomePages;
using QuanLyChung365TruocDangNhap.Hr.Pages.HRReportPages.HRReportPages;
using QuanLyChung365TruocDangNhap.Hr.Pages.ManageRecruitmentPages.ListOfCandidatesPages;
using QuanLyChung365TruocDangNhap.Hr.Pages.ManageRecruitmentPages.PerformRecruitmentPages;
using QuanLyChung365TruocDangNhap.Hr.Pages.ManageRecruitmentPages.RecruitmentProcessPages;
using QuanLyChung365TruocDangNhap.Hr.Pages.OrganizationalChart;
using QuanLyChung365TruocDangNhap.Hr.Pages.RecentDeletedData;
using QuanLyChung365TruocDangNhap.Hr.Pages.SalaryAndBenefitPages.BonusPages;
using QuanLyChung365TruocDangNhap.Hr.Pages.SalaryAndBenefitPages.ViolationsPages;
using QuanLyChung365TruocDangNhap.Hr.Pages.SettingPages;
using QuanLyChung365TruocDangNhap.Hr.Pages.TrainingAndDevelopingPages.JobPositionPages;
using QuanLyChung365TruocDangNhap.Hr.Pages.TrainingAndDevelopingPages.TrainingProcessPages;
using QuanLyChung365TruocDangNhap.Hr.View;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuanLyChung365TruocDangNhap.Hr.Components
{
    /// <summary>
    /// Interaction logic for SideBar.xaml
    /// </summary>
    public partial class SideBar : UserControl
    {
        public Dictionary<string, bool> dictionary = new Dictionary<string, bool>();

        public Dictionary<string, bool> dictionaryExtra = new Dictionary<string, bool>();
        bool isSetupRole = false;
        bool[,] arrPermission = new bool[8, 5];
        public static readonly DependencyProperty MainItemProperty =
         DependencyProperty.Register("MainItem", typeof(string), typeof(SideBar), new
            PropertyMetadata("", new PropertyChangedCallback(MainItemChanged)));

        public string MainItem
        {
            get { return (string)GetValue(MainItemProperty); }
            set { SetValue(MainItemProperty, value); }
        }

        private static void MainItemChanged(DependencyObject d,
           DependencyPropertyChangedEventArgs e)
        {
            SideBar sideBar = d as SideBar;
            sideBar.MainItemChanged(e);
        }

        private void MainItemChanged(DependencyPropertyChangedEventArgs e)
        {
            string option = e.NewValue.ToString();
            SetOption(option);
        }


        public static readonly DependencyProperty ExtraItemProperty =
         DependencyProperty.Register("ExtraItem", typeof(string), typeof(SideBar), new
            PropertyMetadata("", new PropertyChangedCallback(ExtraItemChanged)));

        public string ExtraItem
        {
            get { return (string)GetValue(ExtraItemProperty); }
            set { SetValue(ExtraItemProperty, value); }
        }

        private static void ExtraItemChanged(DependencyObject d,
           DependencyPropertyChangedEventArgs e)
        {
            SideBar sideBar = d as SideBar;
            sideBar.ExtraItemChanged(e);
        }

        private void ExtraItemChanged(DependencyPropertyChangedEventArgs e)
        {
            string option = e.NewValue.ToString();
            SetOptionExtra(option);
        }

        public static readonly DependencyProperty TokenProperty =
         DependencyProperty.Register("Token", typeof(string), typeof(SideBar));

        public string Token
        {
            get { return (string)GetValue(TokenProperty); }
            set { SetValue(TokenProperty, value); }
        }

        public static readonly DependencyProperty UserNameProperty =
         DependencyProperty.Register("UserName", typeof(string), typeof(SideBar));

        public string UserName
        {
            get { return (string)GetValue(UserNameProperty); }
            set { SetValue(UserNameProperty, value); }
        }

        public static readonly DependencyProperty IdProperty =
         DependencyProperty.Register("Id", typeof(string), typeof(SideBar));

        public string Id
        {
            get { return (string)GetValue(IdProperty); }
            set { SetValue(IdProperty, value);}
        }

        public static readonly DependencyProperty EpIdProperty =
         DependencyProperty.Register("EpId", typeof(string), typeof(SideBar));

        public string EpId
        {
            get { return (string)GetValue(EpIdProperty); }
            set { SetValue(EpIdProperty, value); }
        }

        public static readonly DependencyProperty TypeUserProperty =
         DependencyProperty.Register("TypeUser", typeof(int), typeof(SideBar));

        public int TypeUser
        {
            get { return (int)GetValue(TypeUserProperty); }
            set { SetValue(TypeUserProperty, value);}
        }
        string type;
        int typeUser; // 1:nhân viên, 2:công ty

        public SideBar()
        {
            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy"; //for the second type
            Thread.CurrentThread.CurrentCulture = ci;
            InitializeComponent();
            SetupSideBar();
            //SetupRole();
        }

        private void SetupRole(object sender, MouseButtonEventArgs e, int type)
        {
            if (isSetupRole) return;
            if(TypeUser == 2)
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        arrPermission[i, j] = true;
                    }
                }

                isSetupRole = true;


                if (type == 1)
                {
                    navigateToPage(sender, e);
                }
                else
                {
                    navigateToPageItem(sender, e);
                }
            }
            else
            {
                GetUserPermission(sender, e, type);
            }

        }

        private async void GetUserPermission(object sender, MouseButtonEventArgs e, int type)
        {
            try
            {
                var httpClient = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                string api = APIs.API.apiCheckRole;
                httpRequestMessage.RequestUri = new Uri(api);
                var parameters = new List<KeyValuePair<string, string>>();
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + Token);
                parameters.Add(new KeyValuePair<string, string>("userId", EpId));
                parameters.Add(new KeyValuePair<string, string>("winform", "1"));

                // Thiết lập Content
                var content = new FormUrlEncodedContent(parameters);
                httpRequestMessage.Content = content;
                var response = await httpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                PermissionEntity result = JsonConvert.DeserializeObject<PermissionEntity>(responseContent);
                if (result.result)
                {
                    foreach(var item in result.success.data.list_permision)
                    {
                        int i = Convert.ToInt32(item.bar_id);
                        int j = Convert.ToInt32(item.per_id);
                        arrPermission[i, j] = true;
                    }

                    isSetupRole = true;


                    if (type == 1)
                    {
                        navigateToPage(sender, e);
                    }
                    else
                    {
                        navigateToPageItem(sender, e);
                    }
                }
                else
                {
                    
                }
            }
            catch
            {
                
            }
        }

        private void SetupSideBar()
        {
            dictionary.Add("Home", false);
            dictionary.Add("ManageRecruitmentParent", false);
            dictionary.Add("salary_and_benifits", false);
            dictionary.Add("administration", false);
            dictionary.Add("training_and_developing", false);
            dictionary.Add("Chart", false);
            dictionary.Add("HRReport", false);
            dictionary.Add("Setting", false);
            dictionary.Add("digital_conversion", false);
            dictionary.Add("RecentDeletedData", false);
            dictionary[MainItem] = true;

            dictionaryExtra.Add("RecruitmentProcess", false);
            dictionaryExtra.Add("PerformRecruitment", false);
            dictionaryExtra.Add("ListOfCandidates", false);
            dictionaryExtra.Add("EmployeeManager", false);
            dictionaryExtra.Add("RegulationsPolicies", false);
            dictionaryExtra.Add("PersonalChange", false);
            dictionaryExtra.Add("JobPosition", false);
            dictionaryExtra.Add("TrainingProcess", false);
            dictionaryExtra.Add("SalaryAndBenefit", false);
            dictionaryExtra.Add("Violation", false);
            if (dictionaryExtra.ContainsKey(ExtraItem))
            {
                dictionaryExtra[ExtraItem] = true;
            }

            SetStatusSideBar();
            SetStatusSideBarExtra();
        }

        private void manage_recruitment_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy"; //for the second type
            Thread.CurrentThread.CurrentCulture = ci;
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = ManageRecruitment.Height;
            animation.To = ManageRecruitment.Height == 0 ? 132 : 0;
            animation.Duration = new Duration(TimeSpan.FromMilliseconds(300));
            ManageRecruitment.BeginAnimation(HeightProperty, animation);
        }

        private void salary_and_benifits_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy"; //for the second type
            Thread.CurrentThread.CurrentCulture = ci;
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = salary_and_benifits_expland.Height;
            animation.To = salary_and_benifits_expland.Height == 0 ? 264 : 0;
            animation.Duration = new Duration(TimeSpan.FromMilliseconds(400));
            salary_and_benifits_expland.BeginAnimation(HeightProperty, animation);
        }

        private void administration_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy"; //for the second type
            Thread.CurrentThread.CurrentCulture = ci;
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = administration_expland.Height;
            animation.To = administration_expland.Height == 0 ? 528 : 0;
            animation.Duration = new Duration(TimeSpan.FromMilliseconds(600));
            administration_expland.BeginAnimation(HeightProperty, animation);
        }

        private void training_and_developing_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy"; //for the second type
            Thread.CurrentThread.CurrentCulture = ci;
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = training_and_developing_expland.Height;
            animation.To = training_and_developing_expland.Height == 0 ? 132 : 0;
            animation.Duration = new Duration(TimeSpan.FromMilliseconds(300));
            training_and_developing_expland.BeginAnimation(HeightProperty, animation);
        }

        private void digital_conversion_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy"; //for the second type
            Thread.CurrentThread.CurrentCulture = ci;
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = digital_conversion_expland.Height;
            animation.To = digital_conversion_expland.Height == 0 ? 176 : 0;
            animation.Duration = new Duration(TimeSpan.FromMilliseconds(300));
            digital_conversion_expland.BeginAnimation(HeightProperty, animation);
        }

        private void item_main_MouseMove(object sender, MouseEventArgs e)
        {
            Grid parent = sender as Grid;
            string name = parent.Name;
            SetColorMainItem(name);

        }

        private void item_main_MouseLeave(object sender, MouseEventArgs e)
        {
            Grid parent = sender as Grid;
            string name = parent.Name;
            UnSetColorMainItem(name);
        }

        
        // hàm chọn 1 main item
        private void SetColorMainItem(string name)
        {
            if (name == "") return;
            var converter = new BrushConverter();
            Grid parent = (Grid)this.FindName(name);
            Grid gridChildren = (Grid)parent.Children[1];
            gridChildren.Background = (Brush)converter.ConvertFromString("#F1F9FC");

            var textBlock = gridChildren.Children.OfType<TextBlock>().FirstOrDefault();
            textBlock.Foreground = (Brush)converter.ConvertFromString("#4C5BD4");

            Grid grid = (Grid)gridChildren.Children[1];
            grid.Visibility = Visibility.Visible;
        }

        // hàm bỏ chọn 1 main item
        private void UnSetColorMainItem(string name)
        {
            if (dictionary[name] == true || name =="") return;
            Grid parent = (Grid)this.FindName(name);
            Grid gridChildren = (Grid)parent.Children[1];
            var textBlock = gridChildren.Children.OfType<TextBlock>().FirstOrDefault();
            var converter = new BrushConverter();
            gridChildren.Background = (Brush)converter.ConvertFromString("#FFFFFF");
            textBlock.Foreground = (Brush)converter.ConvertFromString("#474747");
            Grid grid = (Grid)gridChildren.Children[1];
            grid.Visibility = Visibility.Collapsed;
        }

        // hàm set trạng thái cho sidebar
        private void SetStatusSideBar()
        {
            foreach (var item in dictionary)
            {
                if (item.Value == true)
                {
                    SetColorMainItem(item.Key);
                }
                else
                {
                    UnSetColorMainItem(item.Key);
                }
            }
        }

        private void SetOption(string name)
        {
            foreach (var item in dictionary)
            {
                if (item.Value == true)
                {
                    dictionary[item.Key] = false;
                    break;
                }
            }
            dictionary[name] = true;
            MainItem = name;
            SetStatusSideBar();
        }

        private void item_exta_MouseMove(object sender, MouseEventArgs e)
        {
            SetColorExtraItem(sender);
        }

        private void item_exta_MouseLeave(object sender, MouseEventArgs e)
        {
            UnSetColorExtraItem(sender);
        }

        // hàm chọn 1 main item
        private void SetColorExtraItem(object sender)
        {
            var converter = new BrushConverter();
            Grid grid = sender as Grid;
            grid.Background = (Brush)converter.ConvertFromString("#FFF6E5");
            Grid gridChild = (Grid)grid.Children[1];
            Grid gridRight = (Grid)gridChild.Children[1];
            gridRight.Visibility = Visibility.Visible;
        }

        // hàm bỏ chọn 1 main item
        private void UnSetColorExtraItem(object sender)
        {
            Grid grid = sender as Grid;
            string name = grid.Name;
            if (name != "")
            {
                if (dictionaryExtra.ContainsKey(name))
                {
                    if (dictionaryExtra[name] == true) return;
                }
            }
            var converter = new BrushConverter();
            grid.Background = (Brush)converter.ConvertFromString("#FFFFFF");
            Grid gridChild = (Grid)grid.Children[1];
            Grid gridRight = (Grid)gridChild.Children[1];
            gridRight.Visibility = Visibility.Collapsed;
        }


        private void SetStatusSideBarExtra()
        {
            foreach (var item in dictionaryExtra)
            {
                string name = item.Key;
                Grid grid = (Grid)this.FindName(name);
                if (item.Value == true)
                {
                    SetColorExtraItem(grid);
                }
                else
                {
                    UnSetColorExtraItem(grid);
                }
            }
        }

        private void SetOptionExtra(string name)
        {
            foreach (var item in dictionaryExtra)
            {
                if (item.Value == true)
                {
                    dictionaryExtra[item.Key] = false;
                    break;
                }
            }

            if (dictionaryExtra.ContainsKey(name))
            {
                dictionaryExtra[name] = true;
            }
            ExtraItem = name;
            SetStatusSideBarExtra();
        }

        private void navigateToPage(object sender, MouseButtonEventArgs e)
        {
            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy"; //for the second type
            Thread.CurrentThread.CurrentCulture = ci;
            if (!isSetupRole)
            {
                SetupRole(sender, e, 1);
                return;
            }
            Grid grid = sender as Grid;
            string name = grid.Name;
            

            SetOption(name);
            SetOptionExtra("");
            switch (name)
            {
                case "Home":
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(HomeView))
                        {
                            (window as HomeView).MainContent.Navigate(new HomePage(Token,Id, UserName));
                        }
                    };
                    break;
                case "Setting":
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(HomeView))
                        {
                            (window as HomeView).MainContent.Navigate(new SettingPage(Token, TypeUser, Id, EpId));
                        }
                    };
                    break;
                case "RecentDeletedData":
                    if (!arrPermission[6, 1])
                    {
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(HomeView))
                            {
                                (window as HomeView).MainContent.Navigate(new NotAuthorizedPage());
                            }
                        };
                    }
                    else
                    {
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(HomeView))
                            {
                                (window as HomeView).MainContent.Navigate(new DeleteData3Page(Token, arrPermission[6, 2], arrPermission[6, 3], arrPermission[6, 4]));
                            }
                        };
                    }
                    break;


                case "HRReport":
                    if (!arrPermission[5, 1])
                    {
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(HomeView))
                            {
                                (window as HomeView).MainContent.Navigate(new NotAuthorizedPage());
                            }
                        };
                    }
                    else
                    {
                       
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(HomeView))
                            {
                                (window as HomeView).MainContent.Navigate(new HRReportPage(Token, Id));
                            }
                        };
                    }
                    break;
                case "Chart":
                    if (!arrPermission[5, 1])
                    {
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(HomeView))
                            {
                                (window as HomeView).MainContent.Navigate(new NotAuthorizedPage());
                            }
                        };
                    }
                    else
                    {

                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(HomeView))
                            {
                                (window as HomeView).MainContent.Navigate(new OrganisationalStructureDiagram(Token, Id,null));
                            }
                        };
                    }
                    break;
            }
        }

        private void navigateToPageItem(object sender, MouseButtonEventArgs e)
        {
            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy"; //for the second type
            Thread.CurrentThread.CurrentCulture = ci;
            if (!isSetupRole)
            {
                SetupRole(sender, e, 2);
                return;
            }
            DependencyObject obj = sender as DependencyObject;
            Grid grid = (Grid)obj;
            string name = grid.Name;
            SetOptionExtra(name);
            switch (name)
            {
                case "RecruitmentProcess":
                    if (!arrPermission[1, 1])
                    {
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(HomeView))
                            {
                                (window as HomeView).MainContent.Navigate(new NotAuthorizedPage());
                            }
                        };
                    }
                    else
                    {
                        SetOption("ManageRecruitmentParent");
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(HomeView))
                            {
                                (window as HomeView).MainContent.Navigate(new RecruitmentProcessPage(Token, arrPermission[1,2], arrPermission[1,3], arrPermission[1,4]));
                            }
                        };
                    }
                    
                    break;
                case "PerformRecruitment":
                    if (!arrPermission[1, 1])
                    {
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(HomeView))
                            {
                                (window as HomeView).MainContent.Navigate(new NotAuthorizedPage());
                            }
                        };
                    }
                    else
                    {
                        SetOption("ManageRecruitmentParent");
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(HomeView))
                            {
                                (window as HomeView).MainContent.Navigate(new PerformRecruitmentPage(Token, Id, arrPermission[1, 2], arrPermission[1, 3], arrPermission[1, 4]));
                            }
                        };
                    }
                    break;

                case "ListOfCandidates":
                    if (!arrPermission[1, 1])
                    {
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(HomeView))
                            {
                                (window as HomeView).MainContent.Navigate(new NotAuthorizedPage());
                            }
                        };
                    }
                    else
                    {
                        SetOption("ManageRecruitmentParent");
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(HomeView))
                            {
                                (window as HomeView).MainContent.Navigate(new ListOfCandidatesPage(Token, Id,type, arrPermission[1, 2], arrPermission[1, 3], arrPermission[1, 4]));
                            }
                        };
                    }
                    
                    break;

                    //quản lý nhân viên
                case "EmployeeManager":
                    if (!arrPermission[2, 1])
                    {
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(HomeView))
                            {
                                (window as HomeView).MainContent.Navigate(new NotAuthorizedPage());
                            }
                        };
                    }
                    else
                    {
                        SetOption("administration");
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(HomeView))
                            {
                                (window as HomeView).MainContent.Navigate(new ListOfEmployeePage(Token,Id, arrPermission[2, 2], arrPermission[2, 3], arrPermission[2, 4]));
                            }
                        };
                    }
                    
                    break;
                    //quy định
                case "RegulationsPolicies":
                    SetOption("administration");
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(HomeView))
                        {
                            (window as HomeView).MainContent.Navigate(new WorkingRegulationsPage(Token, arrPermission[2, 2], arrPermission[2, 3], arrPermission[2, 4], TypeUser));
                        }
                    };

                    break;

                case "PersonalChange":
                    if (!arrPermission[2, 1])
                    {
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(HomeView))
                            {
                                (window as HomeView).MainContent.Navigate(new NotAuthorizedPage());
                            }
                        };
                    }
                    else
                    {
                        if (!arrPermission[7, 1])
                        {
                            SetOption("administration");
                            foreach (Window window in Application.Current.Windows)
                            {
                                if (window.GetType() == typeof(HomeView))
                                {
                                    (window as HomeView).MainContent.Navigate(new AppointmentPage(Token,Id, TypeUser, arrPermission[2, 2], arrPermission[2, 3], arrPermission[2, 4], arrPermission[7, 1]));
                                }
                            };

                        }
                        else
                        {
                            SetOption("administration");
                            foreach (Window window in Application.Current.Windows)
                            {
                                if (window.GetType() == typeof(HomeView))
                                {
                                    (window as HomeView).MainContent.Navigate(new UpDownSalaryPage(Token, Id, TypeUser, arrPermission[2, 2], arrPermission[2, 3], arrPermission[2, 4], arrPermission[7, 1]));
                                }
                            };

                        }
                    }
                    
                    break;
                case "JobPosition":
                    if (!arrPermission[4, 1])
                    {
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(HomeView))
                            {
                                (window as HomeView).MainContent.Navigate(new NotAuthorizedPage());
                            }
                        };
                    }
                    else
                    {
                        SetOption("training_and_developing");
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(HomeView))
                            {
                                (window as HomeView).MainContent.Navigate(new JobPositionPage(Token, arrPermission[4, 2], arrPermission[4, 3], arrPermission[4, 4]));
                            }
                        };
                    }
                    
                    break;
                case "TrainingProcess":
                    if (!arrPermission[4, 1])
                    {
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(HomeView))
                            {
                                (window as HomeView).MainContent.Navigate(new NotAuthorizedPage());
                            }
                        };
                    }
                    else
                    {
                        SetOption("training_and_developing");
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(HomeView))
                            {
                                (window as HomeView).MainContent.Navigate(new TrainingProcessPage(Token, arrPermission[4, 2], arrPermission[4, 3], arrPermission[4, 4]));
                            }
                        };
                    }
                    
                    break;
                case "SalaryAndBenefit":
                    if (!arrPermission[3, 1])
                    {
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(HomeView))
                            {
                                (window as HomeView).MainContent.Navigate(new NotAuthorizedPage());
                            }
                        };
                    }
                    else
                    {
                        SetOption("salary_and_benifits");
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(HomeView))
                            {
                                (window as HomeView).MainContent.Navigate(new PersonalBonusPage(Token, Id, TypeUser, arrPermission[3, 2], arrPermission[3, 3], arrPermission[3, 4]));
                            }
                        };
                    }
                    
                    break;
                case "Violation":
                    if (!arrPermission[3, 1])
                    {
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(HomeView))
                            {
                                (window as HomeView).MainContent.Navigate(new NotAuthorizedPage());
                            }
                        };
                    }
                    else
                    {
                        SetOption("salary_and_benifits");
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(HomeView))
                            {
                                (window as HomeView).MainContent.Navigate(new PersonalViolationsPage(Token,Id, TypeUser, arrPermission[3, 2], arrPermission[3, 3], arrPermission[3, 4]));
                            }
                        };
                    }

                    break;
                case "Timekeeping":
                    if (TypeUser == 1)
                    {
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(HomeView))
                            {
                                HomeView Main = (HomeView)window;
                                Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=81b016d57ec189daa8e04dd2d59a22c3." + Main.EpId + "." + Main.pass + "&link=" + "https://chamcong.timviec365.vn/quan-ly-cong-ty.html");
                            }
                        };
                    }
                    else
                    {
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(HomeView))
                            {
                                HomeView Main = (HomeView)window;
                                Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=f30f0b61e761b8926941f232ea7cccb9." + Main.Id + "." + Main.pass + "&link=" + "https://chamcong.timviec365.vn/quan-ly-cong-ty.html");
                            }
                        };

                    }
                    e.Handled = true;
                    break;
                case "Payroll":
                    if (TypeUser == 1)
                    {
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(HomeView))
                            {
                                HomeView Main = (HomeView)window;
                                Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=81b016d57ec189daa8e04dd2d59a22c3." + Main.EpId + "." + Main.pass + "&link=" + "https://tinhluong.timviec365.vn/quan-ly-nhan-su.html");
                            }
                        };
                    }
                    else
                    {
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(HomeView))
                            {
                                HomeView Main = (HomeView)window;
                                Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=f30f0b61e761b8926941f232ea7cccb9." + Main.Id + "." + Main.pass + "&link=" + "https://tinhluong.timviec365.vn/quan-ly-nhan-su.html");
                            }
                        };

                    }
                    e.Handled = true;
                    break;
                case "KPI":
                    if (TypeUser == 1)
                    {
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(HomeView))
                            {
                                HomeView Main = (HomeView)window;
                                Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=81b016d57ec189daa8e04dd2d59a22c3." + Main.EpId + "." + Main.pass + "&link=" + "https://chamcong.timviec365.vn/quan-ly-cong-ty.html");
                            }
                        };
                    }
                    else
                    {
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(HomeView))
                            {
                                HomeView Main = (HomeView)window;
                                Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=f30f0b61e761b8926941f232ea7cccb9." + Main.Id + "." + Main.pass + "&link=" + "https://chamcong.timviec365.vn/quan-ly-cong-ty.html");
                            }
                        };

                    }
                    e.Handled = true;
                    break;
                case "Welfare":
                    if (TypeUser == 1)
                    {
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(HomeView))
                            {
                                HomeView Main = (HomeView)window;
                                Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=81b016d57ec189daa8e04dd2d59a22c3." + Main.EpId + "." + Main.pass + "&link=" + "https://tinhluong.timviec365.vn/quan-ly-phuc-loi.html");
                            }
                        };
                    }
                    else
                    {
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(HomeView))
                            {
                                HomeView Main = (HomeView)window;
                                Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=f30f0b61e761b8926941f232ea7cccb9." + Main.Id + "." + Main.pass + "&link=" + "https://tinhluong.timviec365.vn/quan-ly-phuc-loi.html");
                            }
                        };

                    }
                    e.Handled = true;
                    break;
                case "Contract":
                    if (TypeUser == 1)
                    {
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(HomeView))
                            {
                                HomeView Main = (HomeView)window;
                                Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=81b016d57ec189daa8e04dd2d59a22c3." + Main.EpId + "." + Main.pass + "&link=" + "https://chamcong.timviec365.vn/quan-ly-cong-ty.html");
                            }
                        };
                    }
                    else
                    {
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(HomeView))
                            {
                                HomeView Main = (HomeView)window;
                                Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=f30f0b61e761b8926941f232ea7cccb9." + Main.Id + "." + Main.pass + "&link=" + "https://chamcong.timviec365.vn/quan-ly-cong-ty.html");
                            }
                        };

                    }
                    e.Handled = true;
                    break;
                case "ArchivalRecords":
                    if (TypeUser == 1)
                    {
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(HomeView))
                            {
                                HomeView Main = (HomeView)window;
                                Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=81b016d57ec189daa8e04dd2d59a22c3." + Main.EpId + "." + Main.pass + "&link=" + "https://vanthu.timviec365.vn/quanly-cong-van.html");
                            }
                        };
                    }
                    else
                    {
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(HomeView))
                            {
                                HomeView Main = (HomeView)window;
                                Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=f30f0b61e761b8926941f232ea7cccb9." + Main.Id + "." + Main.pass + "&link=" + "https://vanthu.timviec365.vn/quanly-cong-van.html");
                            }
                        };

                    }
                    e.Handled = true;
                    break;
                case "Proposed":
                    if (TypeUser == 1)
                    {
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(HomeView))
                            {
                                HomeView Main = (HomeView)window;
                                Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=81b016d57ec189daa8e04dd2d59a22c3." + Main.EpId + "." + Main.pass + "&link=" + "https://vanthu.timviec365.vn/trang-quan-ly-de-xuat.html");
                            }
                        };
                    }
                    else
                    {
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(HomeView))
                            {
                                HomeView Main = (HomeView)window;
                                Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=f30f0b61e761b8926941f232ea7cccb9." + Main.Id + "." + Main.pass + "&link=" + "https://vanthu.timviec365.vn/trang-quan-ly-de-xuat.html");
                            }
                        };

                    }
                    e.Handled = true;
                    break;
                case "Proposed2":
                    if (TypeUser == 1)
                    {
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(HomeView))
                            {
                                HomeView Main = (HomeView)window;
                                Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=81b016d57ec189daa8e04dd2d59a22c3." + Main.EpId + "." + Main.pass + "&link=" + "https://vanthu.timviec365.vn/trang-quan-ly-de-xuat.html?open=1");
                            }
                        };
                    }
                    else
                    {
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(HomeView))
                            {
                                HomeView Main = (HomeView)window;
                                Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=f30f0b61e761b8926941f232ea7cccb9." + Main.Id + "." + Main.pass + "&link=" + "https://vanthu.timviec365.vn/trang-quan-ly-de-xuat.html?open=1");
                            }
                        };

                    }
                    e.Handled = true;
                    break; 
                case "InternalCommunications":
                    if (TypeUser == 1)
                    {
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(HomeView))
                            {
                                HomeView Main = (HomeView)window;
                                Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=81b016d57ec189daa8e04dd2d59a22c3." + Main.EpId + "." + Main.pass + "&link=" + "https://truyenthongnoibo.timviec365.vn/quan-ly-chung.html");
                            }
                        };
                    }
                    else
                    {
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(HomeView))
                            {
                                HomeView Main = (HomeView)window;
                                Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=f30f0b61e761b8926941f232ea7cccb9." + Main.Id + "." + Main.pass + "&link=" + "https://truyenthongnoibo.timviec365.vn/quan-ly-chung.html");
                            }
                        };

                    }
                    e.Handled = true;
                    break;
                case "AssetManagement":
                    if (TypeUser == 1)
                    {
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(HomeView))
                            {
                                HomeView Main = (HomeView)window;
                                Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=81b016d57ec189daa8e04dd2d59a22c3." + Main.EpId + "." + Main.pass + "&link=" + "https://chamcong.timviec365.vn/quan-ly-cong-ty.html");
                            }
                        };
                    }
                    else
                    {
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(HomeView))
                            {
                                HomeView Main = (HomeView)window;
                                Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=f30f0b61e761b8926941f232ea7cccb9." + Main.Id + "." + Main.pass + "&link=" + "https://chamcong.timviec365.vn/quan-ly-cong-ty.html");
                            }
                        };

                    }
                    e.Handled = true;
                    break; 
                case "InventoryManagement":
                    if (TypeUser == 1)
                    {
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(HomeView))
                            {
                                HomeView Main = (HomeView)window;
                                Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=81b016d57ec189daa8e04dd2d59a22c3." + Main.EpId + "." + Main.pass + "&link=" + "https://chamcong.timviec365.vn/quan-ly-cong-ty.html");
                            }
                        };
                    }
                    else
                    {
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(HomeView))
                            {
                                HomeView Main = (HomeView)window;
                                Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=f30f0b61e761b8926941f232ea7cccb9." + Main.Id + "." + Main.pass + "&link=" + "https://chamcong.timviec365.vn/quan-ly-cong-ty.html");
                            }
                        };

                    }
                    e.Handled = true;
                    break; 
                case "FinancialManagement":
                    if (TypeUser == 1)
                    {
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(HomeView))
                            {
                                HomeView Main = (HomeView)window;
                                Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=81b016d57ec189daa8e04dd2d59a22c3." + Main.EpId + "." + Main.pass + "&link=" + "https://chamcong.timviec365.vn/quan-ly-cong-ty.html");
                            }
                        };
                    }
                    else
                    {
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(HomeView))
                            {
                                HomeView Main = (HomeView)window;
                                Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=f30f0b61e761b8926941f232ea7cccb9." + Main.Id + "." + Main.pass + "&link=" + "https://chamcong.timviec365.vn/quan-ly-cong-ty.html");
                            }
                        };

                    }
                    e.Handled = true;
                    break;
                case "Translate":
                    if (TypeUser == 1)
                    {
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(HomeView))
                            {
                                HomeView Main = (HomeView)window;
                                Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=81b016d57ec189daa8e04dd2d59a22c3." + Main.EpId + "." + Main.pass + "&link=" + "https://chamcong.timviec365.vn/quan-ly-cong-ty.html");
                            }
                        };
                    }
                    else
                    {
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(HomeView))
                            {
                                HomeView Main = (HomeView)window;
                                Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=f30f0b61e761b8926941f232ea7cccb9." + Main.Id + "." + Main.pass + "&link=" + "https://chamcong.timviec365.vn/quan-ly-cong-ty.html");
                            }
                        };

                    }
                    e.Handled = true;
                    break;
                case "Evaluate":
                    if (TypeUser == 1)
                    {
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(HomeView))
                            {
                                HomeView Main = (HomeView)window;
                                Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=81b016d57ec189daa8e04dd2d59a22c3." + Main.EpId + "." + Main.pass + "&link=" + "https://chamcong.timviec365.vn/quan-ly-cong-ty.html");
                            }
                        };
                    }
                    else
                    {
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(HomeView))
                            {
                                HomeView Main = (HomeView)window;
                                Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=f30f0b61e761b8926941f232ea7cccb9." + Main.Id + "." + Main.pass + "&link=" + "https://chamcong.timviec365.vn/quan-ly-cong-ty.html");
                            }
                        };

                    }
                    e.Handled = true;
                    break;
                //case "Chart":
                    //Process.Start(new ProcessStartInfo("https://phanmemnhansu.timviec365.vn/co-cau-to-chuc.html"));
                    //e.Handled = true;
                    //break;
                case "Timekeeping2":
                    if (TypeUser == 1)
                    {
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(HomeView))
                            {
                                HomeView Main = (HomeView)window;
                                Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=81b016d57ec189daa8e04dd2d59a22c3." + Main.EpId + "." + Main.pass + "&link=" + "https://chamcong.timviec365.vn/quan-ly-cong-ty.html");
                            }
                        };
                    }
                    else
                    {
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(HomeView))
                            {
                                HomeView Main = (HomeView)window;
                                Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=f30f0b61e761b8926941f232ea7cccb9." + Main.Id + "." + Main.pass + "&link=" + "https://chamcong.timviec365.vn/quan-ly-cong-ty.html");
                            }
                        };

                    }
                    e.Handled = true;
                    break;
                case "Payroll2":
                    if (TypeUser == 1)
                    {
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(HomeView))
                            {
                                HomeView Main = (HomeView)window;
                                Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=81b016d57ec189daa8e04dd2d59a22c3." + Main.EpId + "." + Main.pass + "&link=" + "https://tinhluong.timviec365.vn/quan-ly-nhan-su.html");
                            }
                        };
                    }
                    else
                    {
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(HomeView))
                            {
                                HomeView Main = (HomeView)window;
                                Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=f30f0b61e761b8926941f232ea7cccb9." + Main.Id + "." + Main.pass + "&link=" + "https://tinhluong.timviec365.vn/quan-ly-nhan-su.html");
                            }
                        };

                    }
                    e.Handled = true;
                    break;
                case "ArchivalRecords2":
                    if (TypeUser == 1)
                    {
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(HomeView))
                            {
                                HomeView Main = (HomeView)window;
                                Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=81b016d57ec189daa8e04dd2d59a22c3." + Main.EpId + "." + Main.pass + "&link=" + "https://vanthu.timviec365.vn/quanly-cong-van.html");
                            }
                        };
                    }
                    else
                    {
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(HomeView))
                            {
                                HomeView Main = (HomeView)window;
                                Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=f30f0b61e761b8926941f232ea7cccb9." + Main.Id + "." + Main.pass + "&link=" + "https://vanthu.timviec365.vn/quanly-cong-van.html");
                            }
                        };

                    }
                    e.Handled = true;
                    break;
                case "CulturalCommunication":
                    if (TypeUser == 1)
                    {
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(HomeView))
                            {
                                HomeView Main = (HomeView)window;
                                Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=81b016d57ec189daa8e04dd2d59a22c3." + Main.EpId + "." + Main.pass + "&link=" + "https://truyenthongnoibo.timviec365.vn/quan-ly-chung.html");
                            }
                        };
                    }
                    else
                    {
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(HomeView))
                            {
                                HomeView Main = (HomeView)window;
                                Process.Start("https://chamcong.timviec365.vn/thong-bao.html?s=f30f0b61e761b8926941f232ea7cccb9." + Main.Id + "." + Main.pass + "&link=" + "https://truyenthongnoibo.timviec365.vn/quan-ly-chung.html");
                            }
                        };

                    }
                    e.Handled = true;
                    break;

            }
        }
    }
}
