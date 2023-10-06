using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace QuanLyChung365TruocDangNhap.Hr.Popups.OrganizationalChartPopup
{
    /// <summary>
    /// Interaction logic for Paginator.xaml
    /// </summary>
    public partial class Paginator : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public Paginator()
        {
            InitializeComponent();
            this.DataContext = this;
        }
        public int TotalRecord
        {
            get { return (int)GetValue(TotalRecordProperty); }
            set
            {
                SetValue(TotalRecordProperty, value); Refresh();
            }
        }
        public static readonly DependencyProperty TotalRecordProperty =
            DependencyProperty.Register("TotalRecord", typeof(int), typeof(Paginator), new PropertyMetadata(0));

        public int SelectedPage
        {
            get { return (int)GetValue(SelectedPageProperty); }
            set { SetValue(SelectedPageProperty, value); }
        }
        public static readonly DependencyProperty SelectedPageProperty =
            DependencyProperty.Register("SelectedPage", typeof(int), typeof(Paginator), new PropertyMetadata(0));

        public int DisplayRecords
        {
            get { return (int)GetValue(DisplayRecordsProperty); }
            set { SetValue(DisplayRecordsProperty, value); }
        }
        public static readonly DependencyProperty DisplayRecordsProperty =
            DependencyProperty.Register("DisplayRecords", typeof(int), typeof(Paginator), new PropertyMetadata(0));

        public SelectionChangedEventHandler SelectionChanged
        {
            get { return (SelectionChangedEventHandler)GetValue(SelectionChangedProperty); }
            set { SetValue(SelectionChangedProperty, value); }
        }
        public static readonly DependencyProperty SelectionChangedProperty =
            DependencyProperty.Register("SelectionChanged", typeof(SelectionChangedEventHandler), typeof(Paginator));

        private List<int> _Records = new List<int>() { 10, 20, 30, 40 , 50 , 60 , 70 , 80 , 90 , 100 };
        public List<int> Records
        {
            get { return _Records; }
        }

        private List<int> _Items;
        public List<int> Items
        {
            get { return _Items; }
        }

        private List<int> _slot = new List<int>();
        public List<int> Slot
        {
            get { return _slot; }
        }

        private List<ContentControl> Display = new List<ContentControl>();


        public void Refresh()
        {
            if (cboRecords.SelectedIndex >= 0) DisplayRecords = Records[cboRecords.SelectedIndex];
            lv.SelectedIndex = 0;
            if (cboRecords.SelectedIndex >= 0 && cboRecords.SelectedIndex < Records.Count)
            {
                _Items = new List<int>();
                _slot = new List<int>();
                var page = TotalRecord / Records[cboRecords.SelectedIndex];
                if (TotalRecord % Records[cboRecords.SelectedIndex] > 0) page++;
                stplPages.Visibility = page > 1 ? Visibility.Visible : Visibility.Collapsed;

                for (int i = 1; i <= page; i++)
                {
                    _Items.Add(i);
                }
                for (int i = 1; i <= 10; i++)
                {
                    if (i <= Items.Count) _slot.Add(i);
                }
                OnPropertyChanged("Items");
                OnPropertyChanged("Slot");
                for (int i = 0; i < gridPage.Children.Count; i++)
                {
                    if (i < Slot.Count && i < Display.Count) Display[i].Tag = "0";
                    else if (i < Display.Count) Display[i].Tag = "-1";
                }
                if (Display.Count > 0 && Display[0].Tag.ToString() != "-1") Display[0].Tag = "1";
            }
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Display = new List<ContentControl>();
            for (int i = 0; i < gridPage.Children.Count; i++)
            {
                if (gridPage.Children[i] is ContentControl)
                {
                    Display.Add(gridPage.Children[i] as ContentControl);
                    Display[i].Tag = "-1";
                }
            }
            Refresh();
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Refresh();
            if (cboRecords.SelectedIndex >= 0) DisplayRecords = Records[cboRecords.SelectedIndex];
            lv.SelectedIndex = 0;
            if (SelectionChanged != null) SelectionChanged(this, e);
        }
        private void lv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectionChanged != null) SelectionChanged(this, e);
        }
        private void btnPreview_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (lv.SelectedIndex - 1 >= 0) lv.SelectedIndex--;
            var index = 0;
            for (int i = 0; i < Display.Count; i++)
            {
                if (Display[i].Tag.ToString() == "1") index = i;
                if (Display[i].Tag.ToString() != "-1") Display[i].Tag = "0";
            }
            if (index - 1 >= 0) Display[index - 1].Tag = "1";
            else Display[index].Tag = "1";

            if (index == 0)
            {

                if (lv.SelectedIndex - 1 > 0)
                {
                    for (int i = 0; i < Slot.Count; i++)
                    {
                        if (Slot[i] - 1 > 0) _slot[i] = Slot[i] - 1;
                    }
                }
                else if (lv.SelectedIndex - 1 == 0)
                {
                    for (int i = 0; i < Slot.Count; i++)
                    {
                        _slot[i] = Items[i];
                    }
                }
                OnPropertyChanged("Slot");
            }
        }
        private void btnNext_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (lv.SelectedIndex + 1 < Items.Count) lv.SelectedIndex++;
            var index = 0;
            for (int i = 0; i < Display.Count; i++)
            {
                if (Display[i].Tag.ToString() == "1") index = i;
                if (Display[i].Tag.ToString() != "-1") Display[i].Tag = "0";
            }
            if (index + 1 < Display.Count && index + 1 < Slot.Count)
            {
                if (Display[index + 1].Tag.ToString() != "-1") Display[index + 1].Tag = "1";
            }
            else if (Display[index].Tag.ToString() != "-1") Display[index].Tag = "1";
            if (index == 4)
            {
                if (lv.SelectedIndex + 1 < Items.Count)
                {
                    for (int i = 0; i < Slot.Count; i++)
                    {
                        if (Slot[i] + 1 <= Items.Count) _slot[i] = Slot[i] + 1;
                    }
                }
                else if (lv.SelectedIndex + 1 == Items.Count)
                {
                    for (int i = 0; i < Slot.Count; i++)
                    {
                        _slot[i] = Items[Items.Count - 1 - i];
                    }
                    _slot.Reverse();
                }
                OnPropertyChanged("Slot");
            }
        }
        private void Slot_MouseLelfButtonDown(object sender, MouseButtonEventArgs e)
        {
            var c = sender as ContentControl;
            var p = c.Parent as Grid;
            var z = p.Children.IndexOf(c);
            var index = 0;
            for (int i = 0; i < Display.Count; i++)
            {
                if (Display[i].Tag.ToString() == "1") index = i;
                if (Display[i].Tag.ToString() != "-1") Display[i].Tag = "0";
            }


            if (index < z)
            {
                var i = z - index;
                if (lv.SelectedIndex + i < Items.Count)
                {
                    lv.SelectedIndex = lv.SelectedIndex + i;
                }
            }
            else if (index > z)
            {
                var i = index - z;
                if (lv.SelectedIndex - i >= 0) lv.SelectedIndex = lv.SelectedIndex - i;
            }

            if (z == 4)
            {
                if (lv.SelectedIndex + 2 < Items.Count)
                {
                    for (int i = 0; i < Slot.Count; i++)
                    {
                        if (Slot[i] + 2 >= 0) _slot[i] = Slot[i] + 2;
                    }
                    Display[z - 2].Tag = "1";
                }
                else if (lv.SelectedIndex + 1 < Items.Count)
                {
                    for (int i = 0; i < Slot.Count; i++)
                    {
                        if (Slot[i] + 1 >= 0) _slot[i] = Slot[i] + 1;
                    }
                    Display[z - 1].Tag = "1";
                }
                else
                {
                    Display[z].Tag = "1";
                }
                OnPropertyChanged("Slot");
            }
            else if (z == 0)
            {
                if (lv.SelectedIndex - 2 >= 0)
                {
                    for (int i = 0; i < Slot.Count; i++)
                    {
                        if (Slot[i] - 2 > 0) _slot[i] = Slot[i] - 2;
                    }
                    Display[z + 2].Tag = "1";
                }
                else if (lv.SelectedIndex - 1 >= 0)
                {
                    for (int i = 0; i < Slot.Count; i++)
                    {
                        if (Slot[i] - 1 > 0) _slot[i] = Slot[i] - 1;
                    }
                    Display[z + 1].Tag = "1";
                }
                else
                {
                    for (int i = 0; i < Slot.Count; i++)
                    {
                        _slot[i] = Items[i];
                    }
                    Display[z].Tag = "1";
                }
                OnPropertyChanged("Slot");
            }
            else Display[z].Tag = "1";
        }
    }
}
