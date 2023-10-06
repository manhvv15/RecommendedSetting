using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace QuanLyChung365TruocDangNhap.ChamCong365.Tools
{
    public partial class Combobox : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public Combobox()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public bool IsDropDownOpen
        {
            get { return (bool)GetValue(IsDropDownOpenProperty); }
            set
            {
                SetValue(IsDropDownOpenProperty, value);
            }
        }
        // Using a DependencyProperty as the backing store for IsDropDownOpen.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsDropDownOpenProperty =
            DependencyProperty.Register("IsDropDownOpen", typeof(bool), typeof(Combobox), new PropertyMetadata(false));

        public int CornerRadius
        {
            get { return (int)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(int), typeof(Combobox));

        public IEnumerable<object> ItemsSource
        {
            get { return (IEnumerable<object>)GetValue(ItemsSourceProperty); }
            set
            {
                SetValue(ItemsSourceProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for ItemsSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable<object>), typeof(Combobox), new PropertyMetadata(new List<object>()));


        private List<string> _Test = new List<string>();
        public List<string> Test
        {
            get { return _Test; }
        }

        public string DisplayMemberPath
        {
            get { return (string)GetValue(DisplayMemberPathProperty); }
            set { SetValue(DisplayMemberPathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DisplayMemberPath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DisplayMemberPathProperty =
            DependencyProperty.Register("DisplayMemberPath", typeof(string), typeof(Combobox));



        public List<String> FilterMemberPaths
        {
            get { return (List<String>)GetValue(FilterMemberPathsProperty); }
            set { SetValue(FilterMemberPathsProperty, value); }
        }
        // Using a DependencyProperty as the backing store for DisplayMemberPaths.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FilterMemberPathsProperty =
            DependencyProperty.Register("FilterMemberPaths", typeof(List<String>), typeof(Combobox), new PropertyMetadata(new List<string>()));

        public Style ItemContainerStyle
        {
            get { return (Style)GetValue(ItemContainerStyleProperty); }
            set { SetValue(ItemContainerStyleProperty, value); }
        }
        // Using a DependencyProperty as the backing store for ItemContainerStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemContainerStyleProperty =
            DependencyProperty.Register("ItemContainerStyle", typeof(Style), typeof(Combobox));



        public String PlaceHolder
        {
            get { return (String)GetValue(PlaceHolderProperty); }
            set { SetValue(PlaceHolderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PlaceHolder.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlaceHolderProperty =
            DependencyProperty.Register("PlaceHolder", typeof(String), typeof(Combobox));

        public Object SelectedItem
        {
            get { return (Object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(Object), typeof(Combobox),new PropertyMetadata(null));



        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { 
                SetValue(SelectedIndexProperty, value);
                var index = (int)GetValue(SelectedIndexProperty);
                if (index >= 0) Refresh();
            }
        }
        public static readonly DependencyProperty SelectedIndexProperty =
            DependencyProperty.Register("SelectedIndex", typeof(int), typeof(Combobox), new PropertyMetadata(-1));


        private string _Text;
        public string Text
        {
            get { return _Text; }
            set { _Text = value;OnPropertyChanged(); }
        }

        public SelectionChangedEventHandler SelectionChanged
        {
            get { return (SelectionChangedEventHandler)GetValue(SelectionChangedProperty); }
            set { SetValue(SelectionChangedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectionChanged.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectionChangedProperty =
            DependencyProperty.Register("SelectionChanged", typeof(SelectionChangedEventHandler), typeof(Combobox));

        public char DisplayChar
        {
            get { return (char)GetValue(DisplayCharProperty); }
            set { SetValue(DisplayCharProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DisplayChar.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DisplayCharProperty =
            DependencyProperty.Register("DisplayChar", typeof(char), typeof(Combobox));



        public bool StayOpenPlaceHolder
        {
            get { return (bool)GetValue(StayOpenPlaceHolderProperty); }
            set { SetValue(StayOpenPlaceHolderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StayOpenPlaceHolder.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StayOpenPlaceHolderProperty =
            DependencyProperty.Register("StayOpenPlaceHolder", typeof(bool), typeof(Combobox), new PropertyMetadata(false));

        private double _PopupMaxWith;
        public double PopupMaxWith
        {
            get { return _PopupMaxWith; }
        }



        public Action ScrollToEnd
        {
            get { return (Action)GetValue(ScrollToEndProperty); }
            set { SetValue(ScrollToEndProperty, value); }
        }
        public static readonly DependencyProperty ScrollToEndProperty =
            DependencyProperty.Register("ScrollToEnd", typeof(Action), typeof(Combobox));



        public TextChangedEventHandler SearchIsNull
        {
            get { return (TextChangedEventHandler)GetValue(SearchIsNullProperty); }
            set { SetValue(SearchIsNullProperty, value); }
        }
        public static readonly DependencyProperty SearchIsNullProperty =
            DependencyProperty.Register("SearchIsNull", typeof(TextChangedEventHandler), typeof(Combobox));



        private Type itemType;
        public string RemoveUnicode(string text)
        {
            string[] arr1 = new string[] { "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ",
                "đ",
                "é","è","ẻ","ẽ","ẹ","ê","ế","ề","ể","ễ","ệ",
                "í","ì","ỉ","ĩ","ị",
                "ó","ò","ỏ","õ","ọ","ô","ố","ồ","ổ","ỗ","ộ","ơ","ớ","ờ","ở","ỡ","ợ",
                "ú","ù","ủ","ũ","ụ","ư","ứ","ừ","ử","ữ","ự",
                "ý","ỳ","ỷ","ỹ","ỵ",
            };
            string[] arr2 = new string[] { "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a",
                "d",
                "e","e","e","e","e","e","e","e","e","e","e",
                "i","i","i","i","i",
                "o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o",
                "u","u","u","u","u","u","u","u","u","u","u",
                "y","y","y","y","y",
            };
            for (int i = 0; i < arr1.Length; i++)
            {
                text = text.Replace(arr1[i], arr2[i]);
                text = text.Replace(arr1[i].ToUpper(), arr2[i].ToUpper());
            }
            return text;
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ItemsSource!=null)
            {
                var textbox = sender as TextBox;
                if (itemType == typeof(string))
                {
                    var list = new List<string>();
                    ItemsSource.ToList().ForEach(i =>
                    {
                        var txt = i as string;
                        if (RemoveUnicode(txt).ToLower().Contains(RemoveUnicode(textbox.Text).ToLower()))
                        {
                            list.Add(txt);
                        }
                    });
                    _Test = list;
                    OnPropertyChanged("Test");
                    return;
                }
                else
                {
                    if (!string.IsNullOrEmpty(DisplayMemberPath))
                    {
                        var list = new List<string>();
                        if (DisplayMemberPath.Contains(","))
                        {
                            var dis = DisplayMemberPath.Split(',').ToList();
                            ItemsSource.ToList().ForEach(item =>
                            {
                                var txt = new List<string>();
                                foreach (var p in item.GetType().GetProperties())
                                {
                                    if (dis.Contains(p.Name))
                                    {
                                        txt.Add(p.GetValue(item, null).ToString());
                                    }
                                }
                                var temp = "";
                                if (!string.IsNullOrEmpty(DisplayChar.ToString())) temp = (string.Join(" " + DisplayChar.ToString() + " ", txt));
                                else temp = string.Join(" ", txt);
                                if (RemoveUnicode(temp).ToLower().Contains(RemoveUnicode(textbox.Text.ToLower()))) list.Add(temp);
                            });
                        }
                        else
                        {
                            ItemsSource.ToList().ForEach(item =>
                            {
                                var txt = new List<string>();
                                foreach (var p in item.GetType().GetProperties())
                                {
                                    if (p.Name == DisplayMemberPath)
                                    {
                                        txt.Add(p.GetValue(item, null).ToString());
                                    }
                                }
                                var temp = string.Join(" ", txt);
                                if (RemoveUnicode(temp).ToLower().Contains(RemoveUnicode(textbox.Text.ToLower()))) list.Add(temp);
                            });
                        }
                        _Test = list;
                        OnPropertyChanged("Test");
                    }
                }
            }
            if (Test.Count <= 0 || Test == null || string.IsNullOrEmpty((sender as TextBox).Text)) if (SearchIsNull != null) SearchIsNull(sender, e);
        }
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IsDropDownOpen = false;
            var index = (sender as ListView).SelectedIndex;
            SelectedItem = null;
            Text = "";
            if (index >= 0 && index < ItemsSource.ToList().Count)
            {
                SelectedItem = ItemsSource.ToList()[index];
                Text = Test.ToList()[index] as string;
                if (SelectionChanged != null) SelectionChanged(this, e);
            }
        }
        public void Refresh() {
            if (ItemsSource != null)
            {
                itemType = ItemsSource.GetType().GetGenericArguments().Single();
                if (itemType == typeof(string))
                {
                    var list = new List<string>();
                    ItemsSource.ToList().ForEach(i =>
                    {
                        list.Add(i as string);
                    });
                    _Test = list;
                    OnPropertyChanged("Test");
                    return;
                }
                if (!string.IsNullOrEmpty(DisplayMemberPath))
                {
                    var list = new List<string>();
                    if (DisplayMemberPath.Contains(","))
                    {
                        var dis = DisplayMemberPath.Split(',').ToList();
                        ItemsSource.ToList().ForEach(item =>
                        {
                            var txt = new List<string>();
                            foreach (var p in item.GetType().GetProperties())
                            {
                                if (dis.Contains(p.Name))
                                {
                                    if(p.GetValue(item, null)!=null) txt.Add(p.GetValue(item, null).ToString());
                                }
                            }
                            if (!string.IsNullOrEmpty(DisplayChar.ToString())) list.Add(string.Join(" " + DisplayChar.ToString() + " ", txt));
                            else list.Add(string.Join(" ", txt));
                        });
                    }
                    else
                    {
                        ItemsSource.ToList().ForEach(item =>
                        {
                            var txt = new List<string>();
                            foreach (var p in item.GetType().GetProperties())
                            {
                                if (p.Name == DisplayMemberPath)
                                {
                                    var ten = p.GetValue(item, null);
                                    if(ten!=null)txt.Add(ten.ToString());
                                }
                            }
                            list.Add(string.Join(" ", txt));
                        });
                    }
                    _Test = list;
                    OnPropertyChanged("Test");
                }
                else
                {
                    var list = new List<string>();
                    ItemsSource.ToList().ForEach(i =>
                    {
                        list.Add(itemType.FullName);
                    });
                    _Test = list;
                    OnPropertyChanged("Test");
                }
            }
        }
        private void Border_Loaded(object sender, RoutedEventArgs e)
        {
            Refresh();
        }
        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            _PopupMaxWith = this.ActualWidth;
            foreach (var item in Test)
            {
                var formattedText = new FormattedText(
                    item,
                    CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    new Typeface(this.FontFamily, FontStyles.Normal, FontWeights.Regular, FontStretches.Normal)
                    , this.FontSize, Brushes.Black, new NumberSubstitution(), 1
                );
                if (formattedText.Width > _PopupMaxWith)
                {
                    _PopupMaxWith = formattedText.Width;
                    OnPropertyChanged("PopupMaxWith");
                }
            }
            Refresh();
        }
        private void SearchbarLoaded(object sender, RoutedEventArgs e)
        {
            var txt = (sender as Border).Child as TextBox;
            txt.Focus();
            txt.Text = "";
        }
        private T GetFirstChildOfType<T>(DependencyObject dependencyObject) where T : DependencyObject
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
        private void ListView_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var lv = sender as ListView;
            var scroll = GetFirstChildOfType<ScrollViewer>(lv);
            if (scroll != null)
            {
                if (scroll.ScrollableHeight>0 && scroll.ScrollableHeight <= scroll.VerticalOffset)
                {
                    if (ScrollToEnd != null) ScrollToEnd();
                }
            }
        }
    }
}
