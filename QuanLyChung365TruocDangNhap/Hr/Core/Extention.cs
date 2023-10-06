using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace QuanLyChung365TruocDangNhap.Hr.Core
{
    static class Extention
    {
        public static void AutoReponsiveColumn(this DataGrid table, int column, string displayMemberPath = null)
        {
            double margin = 25;

            double minwidthz = 0;
            if (!string.IsNullOrEmpty(displayMemberPath) && table.ItemsSource != null)
                foreach (var item in table.ItemsSource)
                {
                    foreach (var prop in item.GetType().GetProperties())
                    {
                        if (prop.Name == displayMemberPath)
                        {
                            var val = prop.GetValue(item).ToString();
                            if (!string.IsNullOrEmpty(val))
                            {
                                var formattedText = new FormattedText(
                                    val,
                                    CultureInfo.CurrentCulture,
                                    FlowDirection.LeftToRight,
                                    new Typeface(table.FontFamily, FontStyles.Normal, FontWeights.Regular, FontStretches.Normal)
                                    , 16, Brushes.Black, new NumberSubstitution(), (TextFormattingMode)1
                                );

                                if (minwidthz < formattedText.Width + margin) minwidthz = formattedText.Width + margin;
                            }
                        }
                    }
                }
            else if (table.ItemsSource != null && table.ItemsSource.GetType().GetGenericArguments().Single() == typeof(string))
            {
                foreach (var item in table.ItemsSource)
                {
                    var contentText = new FormattedText(
                           item.ToString(),
                           CultureInfo.CurrentCulture,
                           FlowDirection.LeftToRight,
                           new Typeface(table.FontFamily, FontStyles.Normal, FontWeights.Regular, FontStretches.Normal)
                           , 16, Brushes.Black, new NumberSubstitution(), (TextFormattingMode)1
                       );

                    if (minwidthz < contentText.Width + margin) minwidthz = contentText.Width + margin;
                }
            }

            if (table.Columns[column].Header != null)
            {
                var headerText = new FormattedText(
                    table.Columns[column].Header.ToString(),
                    CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    new Typeface(table.FontFamily, FontStyles.Normal, FontWeights.Regular, FontStretches.Normal)
                    , 16, Brushes.Black, new NumberSubstitution(), (TextFormattingMode)1
                );
                if (minwidthz < headerText.Width * 2) minwidthz = headerText.Width * 2;
            }

            table.Columns[column].MinWidth = minwidthz;

            double ww = 0;
            table.Columns.ToList().ForEach(c =>
            {
                if (table.Columns.IndexOf(c) != column) ww += c.ActualWidth;
            });
            table.Columns[column].Width = table.ActualWidth - ww;

            table.Loaded += (s, e) =>
            {
                double minwidth = 0;
                if (!string.IsNullOrEmpty(displayMemberPath) && table.ItemsSource != null)
                    foreach (var item in table.ItemsSource)
                    {
                        foreach (var prop in item.GetType().GetProperties())
                        {
                            if (prop.Name == displayMemberPath)
                            {
                                var val = prop.GetValue(item).ToString();
                                if (!string.IsNullOrEmpty(val))
                                {
                                    var formattedText = new FormattedText(
                                        val,
                                        CultureInfo.CurrentCulture,
                                        FlowDirection.LeftToRight,
                                        new Typeface(table.FontFamily, FontStyles.Normal, FontWeights.Regular, FontStretches.Normal)
                                        , 16, Brushes.Black, new NumberSubstitution(), (TextFormattingMode)1
                                    );

                                    if (minwidth < formattedText.Width + margin) minwidth = formattedText.Width + margin;
                                }
                            }
                        }
                    }
                else if (table.ItemsSource != null && table.ItemsSource.GetType().GetGenericArguments().Single() == typeof(string))
                {
                    foreach (var item in table.ItemsSource)
                    {
                        var contentText = new FormattedText(
                               item.ToString(),
                               CultureInfo.CurrentCulture,
                               FlowDirection.LeftToRight,
                               new Typeface(table.FontFamily, FontStyles.Normal, FontWeights.Regular, FontStretches.Normal)
                               , 16, Brushes.Black, new NumberSubstitution(), (TextFormattingMode)1
                           );

                        if (minwidth < contentText.Width + margin) minwidth = contentText.Width + margin;
                    }
                }

                if (table.Columns[column].Header != null)
                {
                    var headerText = new FormattedText(
                        table.Columns[column].Header.ToString(),
                        CultureInfo.CurrentCulture,
                        FlowDirection.LeftToRight,
                        new Typeface(table.FontFamily, FontStyles.Normal, FontWeights.Regular, FontStretches.Normal)
                        , 16, Brushes.Black, new NumberSubstitution(), (TextFormattingMode)1
                    );
                    if (minwidth < headerText.Width * 2) minwidth = headerText.Width * 2;
                }


                table.Columns[column].MinWidth = minwidth;

                double w = 0;
                table.Columns.ToList().ForEach(c =>
                {
                    if (table.Columns.IndexOf(c) != column) w += c.ActualWidth;
                });
                table.Columns[column].Width = table.ActualWidth - w;
            };
            table.SizeChanged += (s, e) =>
            {
                double minwidthx = 0;
                if (!string.IsNullOrEmpty(displayMemberPath) && table.ItemsSource != null)
                    foreach (var item in table.ItemsSource)
                    {
                        foreach (var prop in item.GetType().GetProperties())
                        {
                            if (prop.Name == displayMemberPath)
                            {
                                var val = prop.GetValue(item).ToString();
                                if (!string.IsNullOrEmpty(val))
                                {
                                    var formattedText = new FormattedText(
                                        val,
                                        CultureInfo.CurrentCulture,
                                        FlowDirection.LeftToRight,
                                        new Typeface(table.FontFamily, FontStyles.Normal, FontWeights.Regular, FontStretches.Normal)
                                        , 16, Brushes.Black, new NumberSubstitution(), (TextFormattingMode)1
                                    );

                                    if (minwidthx < formattedText.Width + margin) minwidthx = formattedText.Width + margin;
                                }
                            }
                        }
                    }
                else if (table.ItemsSource != null && table.ItemsSource.GetType().GetGenericArguments().Single() == typeof(string))
                {
                    foreach (var item in table.ItemsSource)
                    {
                        var contentText = new FormattedText(
                               item.ToString(),
                               CultureInfo.CurrentCulture,
                               FlowDirection.LeftToRight,
                               new Typeface(table.FontFamily, FontStyles.Normal, FontWeights.Regular, FontStretches.Normal)
                               , 16, Brushes.Black, new NumberSubstitution(), (TextFormattingMode)1
                           );

                        if (minwidthx < contentText.Width + margin) minwidthx = contentText.Width + margin;
                    }
                }

                if (table.Columns[column].Header != null)
                {
                    var headerText = new FormattedText(
                        table.Columns[column].Header.ToString(),
                        CultureInfo.CurrentCulture,
                        FlowDirection.LeftToRight,
                        new Typeface(table.FontFamily, FontStyles.Normal, FontWeights.Regular, FontStretches.Normal)
                        , 16, Brushes.Black, new NumberSubstitution(), (TextFormattingMode)1
                    );
                    if (minwidthx < headerText.Width * 2) minwidthx = headerText.Width * 2;
                }


                table.Columns[column].MinWidth = minwidthx;

                double wd = 0;
                table.Columns.ToList().ForEach(c =>
                {
                    if (table.Columns.IndexOf(c) != column) wd += c.ActualWidth;
                });
                table.Columns[column].Width = 0;
                table.Columns[column].Width = table.ActualWidth - wd;
            };
        }
        public static void AutoFitColumn(this DataGrid table, int column, string displayMemberPath = null)
        {
            double margin = 25;

            double minwidthz = 0;
            if (!string.IsNullOrEmpty(displayMemberPath) && table.ItemsSource != null)
                foreach (var item in table.ItemsSource)
                {
                    foreach (var prop in item.GetType().GetProperties())
                    {
                        if (prop.Name == displayMemberPath)
                        {
                            var val = prop.GetValue(item).ToString();
                            if (!string.IsNullOrEmpty(val))
                            {
                                var formattedText = new FormattedText(
                                    val,
                                    CultureInfo.CurrentCulture,
                                    FlowDirection.LeftToRight,
                                    new Typeface(table.FontFamily, FontStyles.Normal, FontWeights.Regular, FontStretches.Normal)
                                    , 16, Brushes.Black, new NumberSubstitution(), (TextFormattingMode)1
                                );

                                if (minwidthz < formattedText.Width + margin) minwidthz = formattedText.Width + margin;
                            }
                        }
                    }
                }
            else if (table.ItemsSource != null && table.ItemsSource.GetType().GetGenericArguments().Single() == typeof(string))
            {
                foreach (var item in table.ItemsSource)
                {
                    var contentText = new FormattedText(
                           item.ToString(),
                           CultureInfo.CurrentCulture,
                           FlowDirection.LeftToRight,
                           new Typeface(table.FontFamily, FontStyles.Normal, FontWeights.Regular, FontStretches.Normal)
                           , 16, Brushes.Black, new NumberSubstitution(), (TextFormattingMode)1
                       );

                    if (minwidthz < contentText.Width + margin) minwidthz = contentText.Width + margin;
                }
            }

            if (table.Columns[column].Header != null)
            {
                var headerText = new FormattedText(
                    table.Columns[column].Header.ToString(),
                    CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    new Typeface(table.FontFamily, FontStyles.Normal, FontWeights.Regular, FontStretches.Normal)
                    , 16, Brushes.Black, new NumberSubstitution(), (TextFormattingMode)1
                );
                if (minwidthz < headerText.Width * 2) minwidthz = headerText.Width * 2;
            }

            table.Columns[column].MinWidth = minwidthz;

            double ww = 0;
            table.Columns.ToList().ForEach(c =>
            {
                if (table.Columns.IndexOf(c) != column) ww += c.ActualWidth;
            });
            table.Columns[column].Width = table.ActualWidth - ww;
        }
        public static T GetFirstChildOfType<T>(this DependencyObject dependencyObject) where T : DependencyObject
        {
            if (dependencyObject == null)
            {
                return null;
            }

            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(dependencyObject); i++)
            {
                var child = VisualTreeHelper.GetChild(dependencyObject, i);

                var result = (child as T) ?? GetFirstChildOfType<T>(child);

                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }
        public static void OnPreviewMouseWheel(this ScrollViewer MainScroll, object sender, MouseWheelEventArgs e)
        {
            if (sender is DataGrid)
            {
                var datagrid = sender as DataGrid;
                if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                {
                    var scroll = datagrid.GetFirstChildOfType<ScrollViewer>();
                    scroll.ScrollToHorizontalOffset(scroll.HorizontalOffset - e.Delta);
                }
                else MainScroll.ScrollToVerticalOffset(MainScroll.VerticalOffset - e.Delta);
            }
            if (sender is ListView)
            {
                var listView = sender as ListView;
                if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                {
                    var scroll = listView.GetFirstChildOfType<ScrollViewer>();
                    scroll.ScrollToHorizontalOffset(scroll.HorizontalOffset - e.Delta);
                }
                else MainScroll.ScrollToVerticalOffset(MainScroll.VerticalOffset - e.Delta);
            }
        }
        public static BitmapImage GetThumbnail(this string path, int width)
        {
            BitmapImage image = new BitmapImage();
            using (var stream = File.OpenRead(path))
            {
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = stream;
                image.DecodePixelWidth = width;
                //image.DecodePixelHeight = height;
                image.EndInit();
            }
            return image;
        }
        public static BitmapImage GetThumbnail(this Uri link, int width)
        {
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.UriSource = link;
            image.DecodePixelWidth = width;
            image.EndInit();
            return image;
        }
        public static string RemoveUnicode(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return "";
            }
            else
            {
                string[] arr1 = new string[] { "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ", "đ", "é", "è", "ẻ", "ẽ", "ẹ", "ê", "ế", "ề", "ể", "ễ", "ệ", "í", "ì", "ỉ", "ĩ", "ị", "ó", "ò", "ỏ", "õ", "ọ", "ô", "ố", "ồ", "ổ", "ỗ", "ộ", "ơ", "ớ", "ờ", "ở", "ỡ", "ợ", "ú", "ù", "ủ", "ũ", "ụ", "ư", "ứ", "ừ", "ử", "ữ", "ự", "ý", "ỳ", "ỷ", "ỹ", "ỵ", };
                string[] arr2 = new string[] { "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "d", "e", "e", "e", "e", "e", "e", "e", "e", "e", "e", "e", "i", "i", "i", "i", "i", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "u", "u", "u", "u", "u", "u", "u", "u", "u", "u", "u", "y", "y", "y", "y", "y", };
                for (int i = 0; i < arr1.Length; i++)
                {
                    text = text.Replace(arr1[i], arr2[i]);
                    text = text.Replace(arr1[i].ToUpper(), arr2[i].ToUpper());
                }
                return text;
            }
        }
    }
}
