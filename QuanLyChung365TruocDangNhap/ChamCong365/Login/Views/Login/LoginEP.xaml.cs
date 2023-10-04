using QuanLyChung365TruocDangNhap.ChamCong365.ConnectToserver;
using QuanLyChung365TruocDangNhap.ChamCong365.Login.Entities;
using Newtonsoft.Json;
using QRCoder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Device.Location;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
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

namespace QuanLyChung365TruocDangNhap.ChamCong365.Login.Views.Login
{
    /// <summary>
    /// Interaction logic for LoginCom.xaml
    /// </summary>
    public partial class LoginEP : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public LoginEP(LoginWindow login)
        {
            InitializeComponent();
            this.DataContext = this;
            WinLogin = login;
            WinLogin.Title = "Đăng nhập nhân viên";

            if (!string.IsNullOrEmpty(QuanLyChung365TruocDangNhap.Properties.Settings.Default.EpEmail) || !string.IsNullOrEmpty(QuanLyChung365TruocDangNhap.Properties.Settings.Default.EpPass))
            {
                ckSave.IsChecked = true;
                txtEmail.Text = QuanLyChung365TruocDangNhap.Properties.Settings.Default.EpEmail;
                //TypeLogin = int.Parse(QuanLyChung365TruocDangNhap.Properties.Settings.Default.EpTypePass);
                if (TypeLogin == 0)
                    txtPass.Password = QuanLyChung365TruocDangNhap.Properties.Settings.Default.EpPass;
                txtPass.GetType().GetMethod("Select", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(txtPass, new object[] { Pass.Length, 0 });
                txtPass.Focus();
            }
            else txtEmail.Focus();
            GetLocationEvent();
            connectionSocket();
        }
        //
        private int _TypeLogin = 1;

        public int TypeLogin
        {
            get { return _TypeLogin; }
            set { _TypeLogin = value; OnPropertyChanged(); }
        }

        private string _IdQR;

        public string IdQR
        {
            get { return _IdQR; }
            set { _IdQR = value; OnPropertyChanged("IdQR"); }
        }
        public double? latitude { get; set; }
        public double? longitude { get; set; }

        private string _TypeName;
        public ConnectSocket Socket { get; set; }
        public string TypeName
        {
            get { return _TypeName; }
            set { _TypeName = value; OnPropertyChanged("TypeName"); }
        }
        public LoginWindow WinLogin { get; set; }

        private string _Pass = "";
        public string Pass
        {
            get { return _Pass; }
            set { _Pass = value; OnPropertyChanged(); }
        }

        Regex regex = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", RegexOptions.CultureInvariant | RegexOptions.Singleline);
        Regex regexSDT = new Regex(@"^((09|03|07|08|05|04)+([0-9]{8})\b)$", RegexOptions.CultureInvariant | RegexOptions.Singleline);
        //
        private void SignIn(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://hungha365.com/dang-ky-nhan-vien.html");
        }
        private void GoBack(object sender, MouseButtonEventArgs e)
        {
            WinLogin.Close();

            WinLogin.WinMain.IsFull = WinLogin.IsFull;
            WinLogin.WinMain.Width = WinLogin.ActualWidth;
            WinLogin.WinMain.Height = WinLogin.ActualHeight;
            WinLogin.WinMain.Left = WinLogin.Left;
            WinLogin.WinMain.Top = WinLogin.Top;

            WinLogin.WinMain.Show();
        }
        private void txtPass_PasswordChanged(object sender, RoutedEventArgs e)
        {
            Pass = txtPass.Password;
            tblValidatePass.Text = "";
            if (string.IsNullOrEmpty(Pass))
            {
                tblValidatePass.Text = "Không được để trống";
            }

        }
        private async void runLogin()
        {
            bool allow = true;
            tblValidateEmail.Text = tblValidatePass.Text = "";
            if (string.IsNullOrEmpty(txtEmail.Text))
            {
                allow = false;
                tblValidateEmail.Text = "Không được để trống";
            }
            else if (!regex.IsMatch(txtEmail.Text) && !regexSDT.IsMatch(txtEmail.Text))
            {
                allow = false;
                tblValidateEmail.Text = "Nhập không đúng định dạng";
            }
            if (string.IsNullOrEmpty(Pass))
            {
                allow = false;
                tblValidatePass.Text = "Không được để trống";
            }
            if (allow)
            {
                if (ckSave.IsChecked == true)
                {
                    QuanLyChung365TruocDangNhap.Properties.Settings.Default.EpEmail = txtEmail.Text;
                    QuanLyChung365TruocDangNhap.Properties.Settings.Default.Save();
                }
                try
                {
                    Dictionary<string, string> form = new Dictionary<string, string>();
                    form.Add("account", txtEmail.Text);
                    form.Add("password", Pass);
                    form.Add("type", "2");
                    if(TypeLogin == 1)
                    {
                        form.Add("passtype", "1");
                    }
                    HttpClient httpClient = new HttpClient();
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                    var respon = await httpClient.PostAsync("http://210.245.108.202:3000/api/qlc/employee/login", new FormUrlEncodedContent(form));
                    QuanLyChung365TruocDangNhap.ChamCong365.Entities.Staff.API_Login_Staff api = JsonConvert.DeserializeObject<QuanLyChung365TruocDangNhap.ChamCong365.Entities.Staff.API_Login_Staff>(respon.Content.ReadAsStringAsync().Result);
                    if (api.data != null && api.data.result)
                    {
                        if(api.data.data.type == "2")
                        {
                            if (ckSave.IsChecked == true)
                            {
                                QuanLyChung365TruocDangNhap.Properties.Settings.Default.EpPass = Pass;
                                QuanLyChung365TruocDangNhap.Properties.Settings.Default.EpTypePass = TypeLogin.ToString();
                                QuanLyChung365TruocDangNhap.Properties.Settings.Default.Save();
                            }

                            var main = new QuanLyChung365TruocDangNhap.ChamCong365.MainWindow(api);

                            main.IsFull = WinLogin.IsFull;
                            main.Width = WinLogin.Width;
                            main.Height = WinLogin.Height;
                            main.Left = WinLogin.Left;
                            main.Top = WinLogin.Top;

                            main.LogOut = () =>
                            {
                                main.Close();
                                WinLogin.LogOut(main.IsFull, main);

                            };

                            WinLogin.Hide();
                            main.ShowDialog();
                        }
                        else
                        {
                            runloginCom();
                        }
                    }
                    else
                    {
                        var n = new Notify();
                        n.Type = Notify.NotifyType.Error;
                        if (api.error != null && api.error.message != null) n.setMessage(api.error.message);
                        else n.setMessage("");
                        WinLogin.ShowPopup(n);
                    }
                }
                catch (Exception ex)
                {
                    var n = new Notify();
                    n.Type = Notify.NotifyType.Error;
                    n.setMessage(ex.Message);
                    WinLogin.ShowPopup(n);
                }
            }
        }
        private void Login(object sender, MouseButtonEventArgs e)
        {
            runLogin();
        }
        private async void runloginCom()
        {
            try
            {
                Dictionary<string, string> form = new Dictionary<string, string>();
                form.Add("account", txtEmail.Text);
                form.Add("password", Pass);
                form.Add("type", "1");
                if (TypeLogin == 1)
                {
                    form.Add("passtype", "1");
                }
                HttpClient httpClient = new HttpClient();
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                var respon = await httpClient.PostAsync("http://210.245.108.202:3000/api/qlc/employee/login", new FormUrlEncodedContent(form));
                QuanLyChung365TruocDangNhap.ChamCong365.Entities.Company.API_Login_Company api = JsonConvert.DeserializeObject<QuanLyChung365TruocDangNhap.ChamCong365.Entities.Company.API_Login_Company>(respon.Content.ReadAsStringAsync().Result);
                if (api.data != null && api.data.result)
                {
                    if (ckSave.IsChecked == true)
                    {
                        QuanLyChung365TruocDangNhap.Properties.Settings.Default.ComPass = Pass;
                        QuanLyChung365TruocDangNhap.Properties.Settings.Default.ComTypePass = TypeLogin.ToString();
                        QuanLyChung365TruocDangNhap.Properties.Settings.Default.Save();
                    }

                    var main = new QuanLyChung365TruocDangNhap.ChamCong365.MainWindow(api);

                    main.IsFull = WinLogin.IsFull;
                    main.Width = WinLogin.Width;
                    main.Height = WinLogin.Height;
                    main.Left = WinLogin.Left;
                    main.Top = WinLogin.Top;

                    main.LogOut = () =>
                    {
                        main.Close();
                        WinLogin.LogOut(main.IsFull, main);
                    };

                    WinLogin.Hide();
                    main.ShowDialog();
                }
                else
                {
                    var n = new Notify();
                    n.Type = Notify.NotifyType.Error;
                    if (api.error != null && api.error.message != null) n.setMessage(api.error.message);
                    else n.setMessage("");
                    WinLogin.ShowPopup(n);
                }
            }
            catch (Exception ex)
            {
                var n = new Notify();
                n.Type = Notify.NotifyType.Error;
                n.setMessage(ex.Message);
                WinLogin.ShowPopup(n);
            }
        }
        private void txtPass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                runLogin();
            }
        }

        private void txtEmail_LostFocus(object sender, RoutedEventArgs e)
        {
            tblValidateEmail.Text = "";
            if (string.IsNullOrEmpty(txtEmail.Text))
            {
                tblValidateEmail.Text = "Không được để trống";
            }
            else if (!regex.IsMatch(txtEmail.Text) && !regexSDT.IsMatch(txtEmail.Text))
            {
                tblValidateEmail.Text = "Nhập không đúng định dạng";
            }
        }

        private void txtPass_LostFocus(object sender, RoutedEventArgs e)
        {
            tblValidatePass.Text = "";
            if (string.IsNullOrEmpty(Pass))
            {
                tblValidatePass.Text = "Không được để trống";
            }
        }

        private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            tblValidateEmail.Text = "";
            if (string.IsNullOrEmpty(txtEmail.Text))
            {
                tblValidateEmail.Text = "Không được để trống";
            }
            else if (!regex.IsMatch(txtEmail.Text) && !regexSDT.IsMatch(txtEmail.Text))
            {
                tblValidateEmail.Text = "Nhập không đúng định dạng";
            }
        }

        private void ckSave_Unchecked(object sender, RoutedEventArgs e)
        {
            QuanLyChung365TruocDangNhap.Properties.Settings.Default.EpEmail = "";
            QuanLyChung365TruocDangNhap.Properties.Settings.Default.EpPass = "";
            QuanLyChung365TruocDangNhap.Properties.Settings.Default.Save();
        }

        private void ForgotPass(object sender, MouseButtonEventArgs e)
        {
            /*var z = new ForgotPasswordWindow(WinLogin);
            z.Type = 1;

            z.IsFull = WinLogin.IsFull;
            z.Width = WinLogin.ActualWidth;
            z.Height = WinLogin.ActualHeight;
            z.Left = WinLogin.Left;
            z.Top = WinLogin.Top;

            WinLogin.Hide();
            z.Show();*/
            Process.Start("https://hungha365.com/quen-mat-khau.html?type=2");
        }
        private void SelectedTypeLogin(object sender, MouseButtonEventArgs e)
        {
            Border b = sender as Border;
            if (b.Name == "QR" && TypeLogin == 1 || b.Name == "TaiKhoan" && TypeLogin == 0) { }
            else
                TypeLogin = TypeLogin == 1 ? 0 : 1;
            if (TypeLogin == 1)
            {
                spQRCode.Visibility = Visibility.Visible;
                Login_Account.Visibility = Visibility.Collapsed;
            }
            else
            {
                Login_Account.Visibility = Visibility.Visible;
                spQRCode.Visibility = Visibility.Collapsed;
            }
        }
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        private ImageSource BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                return bitmapImage;
            }
        }
        public class infoQR
        {
            public infoQR(string qRType, string idQR, string idComputer, string nameComputer, double? latitude, double? longitude, string time)
            {
                QRType = qRType;
                this.idQR = idQR;
                IdComputer = idComputer;
                NameComputer = nameComputer;
                this.latitude = latitude;
                this.longitude = longitude;
                Time = time;
            }

            public string QRType { get; set; }
            public string idQR { get; set; }
            public string IdComputer { get; set; }
            public string NameComputer { get; set; }
            public double? latitude { get; set; }
            public double? longitude { get; set; }
            public string Time { get; set; }

        }
        private void CreateQRCode(double? lat, double? lng)
        {

            try
            {
                string logo = Environment.GetEnvironmentVariable("APPDATA") + @"\Chat365\chat365_logo.png";
                IdQR = RandomString(8);
                string x = Base64Encode(IdQR);
                string IdDevice = $"{RandomString(8)}-{RandomString(4)}-{RandomString(4)}-{RandomString(4)}-{RandomString(12)}";
                string info = JsonConvert.SerializeObject(new infoQR("QRLoginPc", (x + "++"), IdDevice, $"{Environment.MachineName}-Windows", lat, lng, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")));
                QRCodeGenerator qRCodeGenerator = new QRCodeGenerator();
                QRCodeData qRCodeData = qRCodeGenerator.CreateQrCode(info, QRCodeGenerator.ECCLevel.Q);
                QRCode qRCode = new QRCode(qRCodeData);
                Bitmap qRCodeImage = qRCode.GetGraphic(20);
                //QRCodeWriter.CreateQrCode("Welcome to HungHa", 500, QRCodeWriter.QrErrorCorrectionLevel.Medium);
                //var MyQRWithLogo = QRCodeWriter.CreateQrCodeWithLogo(info, logo, 500);

                this.Dispatcher.Invoke(() =>
                {
                    //QrCodeImage.ImageSource = BitmapToImageSource(MyQRWithLogo.ToBitmap());
                    QrCodeImage.ImageSource = BitmapToImageSource(qRCodeImage);
                });
            }
            catch (Exception ex)
            {
            }
        }
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
        private void connectionSocket()
        {
            try
            {
                Socket = new ConnectSocket();
                Socket.WIO.On("QRLogin", response =>
                {
                    try
                    {
                        string QrId = response.GetValue<string>(0);
                        string Email = response.GetValue<string>(1).Replace("+", "");
                        Email = Base64Decode(Email);
                        string Password = response.GetValue<string>(2).Replace("+", "");
                        Password = Base64Decode(Password);

                        if (QrId == IdQR)
                        {
                            this.Dispatcher.Invoke(() =>
                            {
                                Pass = Password;
                                txtEmail.Text = Email;
                                Properties.Settings.Default.EpEmail = Email;
                                Properties.Settings.Default.EpPass = "";
                                Properties.Settings.Default.Save();
                                runLogin();
                            });
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                });
            }
            catch (Exception ex)
            {

            }
        }

        GeoCoordinateWatcher watcher;
        public void GetLocationEvent()
        {
            Task t = new Task(() =>
            {
                CreateQRCode(null, null);
                this.watcher = new GeoCoordinateWatcher();
                if (this.watcher.Status == GeoPositionStatus.NoData || this.watcher.Status == GeoPositionStatus.Ready)
                {
                    if (watcher.TryStart(false, TimeSpan.FromMilliseconds(20000)))
                    {
                        bool ck = true;
                        while (ck)
                        {
                            latitude = this.watcher.Position.Location.Latitude;
                            longitude = this.watcher.Position.Location.Longitude;
                            if (latitude != double.NaN && longitude != double.NaN && latitude > 0 && longitude > 0)
                            {
                                CreateQRCode(latitude.Value, longitude.Value);
                                ck = false;
                            }
                        }

                    }

                }
                else
                {
                    latitude = null;
                    longitude = null;
                }
            });
            t.ContinueWith((p) =>
            {

                t.Dispose();
                this.Dispatcher.Invoke(() =>
                {
                    //loadding.Visibility = Visibility.Collapsed;
                });

            });
            t.Start();
        }

        private void GuideQrCode_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WinLogin.LoginSelectionPage.NavigationService.Navigate(new PageGuideQRCode(WinLogin));
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start("https://quanlychung.timviec365.vn/");
        }
        private void Open_TuyChonKhac(object sender, MouseButtonEventArgs e)
        {
            popup.Visibility = Visibility.Visible;
        }

        private void Closepopup(object sender, MouseButtonEventArgs e)
        {
            popup.Visibility = Visibility.Hidden;
        }

        private void LoginWithPhone(object sender, MouseButtonEventArgs e)
        {
            try
            {
                //bd_login.Visibility = Visibility.Hidden;
                this.FrameLogin.NavigationService.Navigate(new LoginWithPhone(this));
            }
            catch
            {

            }
        }
    }
}
