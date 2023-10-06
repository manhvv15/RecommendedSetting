using System;
using System.Collections.Generic;
using System.Linq;
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

namespace QuanLyChung365TruocDangNhap.Hr.Login.Views
{
    /// <summary>
    /// Interaction logic for Notify.xaml
    /// </summary>
    public partial class Notify : UserControl
    {
        public Notify()
        {
            InitializeComponent();
            this.DataContext = this;
            Type = NotifyType.None;

            this.Focus();
            btn.Focus();
        }
        //
        public enum NotifyType{
            None,
            Success,
            Error
        }

        public NotifyType Type
        {
            get { return (NotifyType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }
        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(NotifyType), typeof(Notify));

        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set 
            {
                if (!string.IsNullOrEmpty(value)) {
                    if (value.Contains("`"))
                    {
                        var mess = new TextBlock() { FontSize = 16, Foreground = App.Current.Resources["#474747"] as SolidColorBrush, TextAlignment = TextAlignment.Center, TextWrapping = TextWrapping.Wrap };
                        mess.Inlines.Add(new Run() { Text=value.Substring(0,value.IndexOf("`")) + "," });
                        mess.Inlines.Add(new LineBreak());
                        mess.Inlines.Add(new Run() { Text=value.Substring(value.IndexOf("`")+1) });
                        ContentNoti = mess;
                    }
                    else if (value.Contains("<br>") || value.Contains("<b>"))
                    {
                        var mess = new TextBlock() { FontSize = 16, Foreground = App.Current.Resources["#474747"] as SolidColorBrush, TextAlignment = TextAlignment.Center, TextWrapping = TextWrapping.Wrap };
                        var lines = value.Split(new string[] { "<br>" }, StringSplitOptions.None).ToList();
                        lines.ForEach(line => {
                            if (line.Contains("<b>"))
                            {
                                var bold = line.Split(new string[] { "<b>" }, StringSplitOptions.None).ToList();
                                for (int i = 0; i < bold.Count; i++)
                                {
                                    if (i == 0 || i % 2 == 0)
                                    {
                                        if (!string.IsNullOrEmpty(bold[i]))
                                            mess.Inlines.Add(new Run() { Text = bold[i] });
                                    }
                                    else mess.Inlines.Add(new Run() { Text = string.Format(" {0} ", bold[i]), FontWeight = FontWeights.SemiBold });
                                }
                                mess.Inlines.Add(new LineBreak());
                            }
                            else
                            {
                                mess.Inlines.Add(new Run() { Text = line });
                                mess.Inlines.Add(new LineBreak());
                            }
                        });
                        ContentNoti = mess;
                    }
                    else if (value.Contains("{br}") || value.Contains("{b}"))
                    {
                        var mess = new TextBlock() { FontSize = 16, Foreground = App.Current.Resources["#474747"] as SolidColorBrush, TextAlignment = TextAlignment.Center, TextWrapping = TextWrapping.Wrap };
                        var lines = value.Split(new string[] { "{br}" }, StringSplitOptions.None).ToList();
                        lines.ForEach(line => {
                            if (line.Contains("{b}"))
                            {
                                var bold = line.Split(new string[] { "{b}" }, StringSplitOptions.None).ToList();
                                for (int i = 0; i < bold.Count; i++)
                                {
                                    if (i == 0 || i % 2 == 0)
                                    {
                                        if (!string.IsNullOrEmpty(bold[i])) mess.Inlines.Add(new Run() { Text = bold[i].Trim() });
                                    }
                                    else {
                                        if (!string.IsNullOrEmpty(bold[i])) mess.Inlines.Add(new Run() { Text =string.Format(" {0} ", bold[i]), FontWeight = FontWeights.SemiBold });
                                        else mess.Inlines.Add(new Run() { Text =" "});
                                    }
                                }
                                mess.Inlines.Add(new LineBreak());
                            }
                            else
                            {
                                mess.Inlines.Add(new Run() { Text = line });
                                mess.Inlines.Add(new LineBreak());
                            }
                        });
                        ContentNoti = mess;
                    }
                    else SetValue(MessageProperty, value);
                }
            }
        }
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(Notify));

        public object ContentNoti
        {
            get { return (object)GetValue(ContentNotiProperty); }
            set { SetValue(ContentNotiProperty, value); }
        }
        public static readonly DependencyProperty ContentNotiProperty =
            DependencyProperty.Register("ContentNoti", typeof(object), typeof(Notify));



        public Action NotiClosed
        {
            get { return (Action)GetValue(NotiClosedProperty); }
            set { SetValue(NotiClosedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Closing.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NotiClosedProperty =
            DependencyProperty.Register("NotiClosed", typeof(Action), typeof(Notify));


        //
        private void Close()
        {
            var p = this.Parent as Panel;
            if (p != null)
            {
                p.Visibility = Visibility.Collapsed;
            }
        }
        public void setMessage(string mess)
        {
            if (!string.IsNullOrEmpty(mess))
            if (mess.Contains("{") && mess.Contains("}"))
            {
                var dem = 0;
                foreach (var t in mess)
                {
                    if (t == '{')
                    {
                        dem++;
                    }
                }
                for (int i = 0; i < dem; i++)
                {
                    var z = "{" + i.ToString() + "}";
                    if (mess.Contains(z))
                    {
                        mess = mess.Replace(z, null);
                    }
                }
                Message = Regex.Replace(mess, @"\s+", " ");
                return;
            }
            else Message = mess;
        }
        public void setMessage(string mess, int index, string replacetText)
        {
            if (!string.IsNullOrEmpty(mess))
            if (mess.Contains("{") && mess.Contains("}"))
            {
                var dem = 0;
                foreach (var t in mess)
                {
                    if (t == '{')
                    {
                        dem++;
                    }
                }
                for (int i = 0; i < dem; i++)
                {
                    var z = "{" + i.ToString() + "}";
                    if (mess.Contains(z))
                    {
                        if (i == index) mess = mess.Replace(z, "{b}" + replacetText + "{b}");
                        else mess = mess.Replace(z, null);
                    }
                }
                Message = Regex.Replace(mess, @"\s+", " ");
                return;
            }
            else Message = mess;
        }
        private void CloseNotify(object sender, MouseButtonEventArgs e)
        {
            if (NotiClosed != null) NotiClosed();
            Close();
        }

        private void btn_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key==Key.Enter)
            {
                Close();
            }
        }
    }
}
