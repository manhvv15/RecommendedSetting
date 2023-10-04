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

namespace QuanLyChung365TruocDangNhap.Hr.Popups
{
    /// <summary>
    /// Interaction logic for ConfirmBox.xaml
    /// </summary>
    public partial class ConfirmBox : UserControl
    {
        public ConfirmBox()
        {
            InitializeComponent();
            this.DataContext = this;
        }
        public string ConfirmTitle
        {
            get { return (string)GetValue(ConfirmTitleProperty); }
            set { SetValue(ConfirmTitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ConfirmTitle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ConfirmTitleProperty =
            DependencyProperty.Register("ConfirmTitle", typeof(string), typeof(ConfirmBox));



        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    if (value.Contains("<br>") || value.Contains("<b>"))
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
                        ContentConfirm = mess;
                    }
                if (!string.IsNullOrEmpty(value))
                    if (value.Contains("{br}") || value.Contains("{b}"))
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
                                    else
                                    {
                                        if (!string.IsNullOrEmpty(bold[i])) mess.Inlines.Add(new Run() { Text = string.Format(" {0} ", bold[i]), FontWeight = FontWeights.SemiBold });
                                        else mess.Inlines.Add(new Run() { Text = " " });
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
                        ContentConfirm = mess;
                    }
                    else
                        SetValue(MessageProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for Message.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(ConfirmBox));



        public object ContentConfirm
        {
            get { return (object)GetValue(ContentConfirmProperty); }
            set { SetValue(ContentConfirmProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ContentConfirm.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentConfirmProperty =
            DependencyProperty.Register("ContentConfirm", typeof(object), typeof(ConfirmBox));




        public Action CanCel
        {
            get { return (Action)GetValue(CanCelProperty); }
            set { SetValue(CanCelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CanCel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CanCelProperty =
            DependencyProperty.Register("CanCel", typeof(Action), typeof(ConfirmBox));



        public Action Accept
        {
            get { return (Action)GetValue(AcceptProperty); }
            set { SetValue(AcceptProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Accept.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AcceptProperty =
            DependencyProperty.Register("Accept", typeof(Action), typeof(ConfirmBox));

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

        private void Close()
        {
            var p = this.Parent as Panel;
            if (p != null)
            {
                p.Visibility = Visibility.Collapsed;
            }
        }

        private void CloseBox(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void CancelBox(object sender, MouseButtonEventArgs e)
        {
            Close();
            if (CanCel != null) CanCel();
        }

        private void AcceptBox(object sender, MouseButtonEventArgs e)
        {
            Close();
            if (Accept != null) Accept();
        }
    }
}
