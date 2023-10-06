using QuanLyChung365TruocDangNhap.Hr.Login.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

namespace QuanLyChung365TruocDangNhap.Hr.Login.Views.Login
{
    /// <summary>
    /// Interaction logic for ForgotPassword_Email.xaml
    /// </summary>
    public partial class ForgotPassword_OTP : Page
    {
        public ForgotPassword_OTP(ForgotPasswordWindow win)
        {
            InitializeComponent();
            this.DataContext = this;
            WinForgot = win;
        }
        //
        public ForgotPasswordWindow WinForgot { get; set; }
        private List<TextBox> listTxt = new List<TextBox>();
        //
        private async void Run()
        {
            var allow = true;
            tblValidate.Text = "";
            var ck = listTxt.Find(i => string.IsNullOrEmpty(i.Text));
            if (ck != null)
            {
                allow = false;
                tblValidate.Text = "Mã OTP gồm 6 số";
            }
            if (allow)
            {
                string txt = "";
                listTxt.ForEach(i => txt += i.Text);
                try
                {
                    Dictionary<string, string> form = new Dictionary<string, string>();
                    form.Add("email", WinForgot.ForgotItem.Email);
                    form.Add("type_user", WinForgot.Type.ToString());
                    form.Add("otp_code", txt);

                    HttpClient httpClient = new HttpClient();
                    var respon = await httpClient.PostAsync("https://chamcong.24hpay.vn/service/verify_otp.php", new FormUrlEncodedContent(form));
                    API_Response api = JsonConvert.DeserializeObject<API_Response>(respon.Content.ReadAsStringAsync().Result);
                    if (api.data != null && api.data.result)
                    {
                        WinForgot.ForgotItem.OTP = txt;
                        var z = new Views.Login.ForgotPassword_NewPass(WinForgot);
                        WinForgot.SelectionPage.NavigationService.Navigate(z);
                    }
                    else
                    {
                        var n = new Notify();
                        n.Type = Notify.NotifyType.Error;
                        if (api.error != null && api.error.message != null) n.setMessage(api.error.message);
                        else n.setMessage("");
                        WinForgot.ShowPopup(n);
                    }
                }
                catch (Exception ex)
                {
                    var n = new Notify();
                    n.Type = Notify.NotifyType.Error;
                    n.setMessage(ex.Message);
                    WinForgot.ShowPopup(n);
                }
            }
        }
        //
        private void Continue(object sender, MouseButtonEventArgs e)
        {
            Run();
        }
        private void GoBack(object sender, MouseButtonEventArgs e)
        {
            WinForgot.SelectionPage.GoBack();
        }
        private void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            listTxt = new List<TextBox>() { txt1, txt2, txt3, txt4, txt5, txt6 };
                listTxt[0].Focus();

        }
        private void txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            var txt = sender as TextBox;
            if (txt != null)
            {
                foreach (TextBox item in listTxt)
                {
                    if (item == txt)
                    {
                        if (txt != listTxt[listTxt.Count - 1] && !string.IsNullOrEmpty(txt.Text)) listTxt[listTxt.IndexOf(item) + 1].Focus();
                        break;
                    }
                }
            }
        }
        private void txt_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var txt = sender as TextBox;
            if (txt != null)
            {
                foreach (TextBox item in listTxt)
                {
                    if (item == txt)
                    {
                        if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.Key == Key.V)
                        {
                            int so;
                            string otp = Clipboard.GetText();

                            if (!int.TryParse(otp, out so)) e.Handled = true;


                            List<bool> ck = new List<bool>();
                            for (int i = 0; i < otp.Length; i++)
                            {
                                ck.Add(int.TryParse(otp[i].ToString(), out so));
                            }
                            if (!ck.Contains(false))
                            {
                                for (int i = 0; i < listTxt.Count; i++)
                                {
                                    listTxt[i].Text = otp[i].ToString();
                                    if (i == listTxt.Count - 1)
                                    {
                                        listTxt[i].SelectionStart = listTxt[i].Text.Length;
                                    };
                                }
                            }
                        }
                        else if (txt == listTxt[5] && e.Key == Key.Return) Run();
                        else if (txt != listTxt[0] && e.Key == Key.Back && string.IsNullOrEmpty(txt.Text))
                        {
                            listTxt[listTxt.IndexOf(item) - 1].Focus();
                            listTxt[listTxt.IndexOf(item) - 1].SelectionStart = listTxt[listTxt.IndexOf(item) - 1].Text.Length;
                        }
                        break;
                    }
                }
            }
        }
        private async void SendOTP(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Dictionary<string, string> form = new Dictionary<string, string>();
                form.Add("email", WinForgot.ForgotItem.Email);
                form.Add("type_user", WinForgot.Type.ToString());

                HttpClient httpClient = new HttpClient();
                var respon = await httpClient.PostAsync("https://chamcong.24hpay.vn/service/send_otp.php", new FormUrlEncodedContent(form));
                API_Response api = JsonConvert.DeserializeObject<API_Response>(respon.Content.ReadAsStringAsync().Result);
                if (api.data != null && api.data.result)
                {
                    var n = new Notify();
                    n.Type = Notify.NotifyType.Success;
                    if (api.data.message != null) n.setMessage(api.data.message);
                    else n.setMessage("");
                    WinForgot.ShowPopup(n);
                }
                else
                {
                    var n = new Notify();
                    n.Type = Notify.NotifyType.Error;
                    if (api.error != null && api.error.message != null) n.setMessage(api.error.message);
                    else n.setMessage("");
                    WinForgot.ShowPopup(n);
                }
            }
            catch (Exception ex)
            {
                var n = new Notify();
                n.Type = Notify.NotifyType.Error;
                n.setMessage(ex.Message);
                WinForgot.ShowPopup(n);
            }
        }
    }
}
