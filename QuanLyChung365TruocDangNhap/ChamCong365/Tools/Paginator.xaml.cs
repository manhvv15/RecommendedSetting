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

namespace QuanLyChung365TruocDangNhap.ChamCong365.Tools
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



        public int TotalRecords
        {
            get { return (int)GetValue(TotalRecordsProperty); }
            set
            {
                SetValue(TotalRecordsProperty, value);
                refresh();
            }
        }
        public static readonly DependencyProperty TotalRecordsProperty =
            DependencyProperty.Register("TotalRecords", typeof(int), typeof(Paginator));

        public int DisplayNumber
        {
            get { return (int)GetValue(DisplayNumberProperty); }
            set { SetValue(DisplayNumberProperty, value); }
        }
        public static readonly DependencyProperty DisplayNumberProperty =
            DependencyProperty.Register("DisplayNumber", typeof(int), typeof(Paginator));


        public int Pages
        {
            get { return (int)GetValue(PagesProperty); }
            set { SetValue(PagesProperty, value); }
        }
        public static readonly DependencyProperty PagesProperty =
            DependencyProperty.Register("Pages", typeof(int), typeof(Paginator));


        public int SelectedPage
        {
            get { return (int)GetValue(SelectedPageProperty); }
            set { SetValue(SelectedPageProperty, value); }
        }
        public static readonly DependencyProperty SelectedPageProperty =
            DependencyProperty.Register("SelectedPage", typeof(int), typeof(Paginator));



        public SelectionChangedEventHandler SelectionChange
        {
            get { return (SelectionChangedEventHandler)GetValue(SelectionChangeProperty); }
            set { SetValue(SelectionChangeProperty, value); }
        }
        public static readonly DependencyProperty SelectionChangeProperty =
            DependencyProperty.Register("SelectionChange", typeof(SelectionChangedEventHandler), typeof(Paginator));



        private List<string> _Items;
        public List<string> Items
        {
            get { return _Items; }
        }

        private List<string> _Slots;
        public List<string> Slots
        {
            get { return _Slots; }
        }

        private List<Border> listBor = new List<Border>();

        public void refresh()
        {
            if (DisplayNumber > 0)
            {
                _Items = new List<string>();
                _Slots = new List<string>();

                Pages = TotalRecords / DisplayNumber;
                if (TotalRecords % DisplayNumber > 0) Pages++;

                for (int i = 1; i <= Pages; i++)
                {
                    _Items.Add(i.ToString());
                }


                for (int i = 0; i < listBor.Count; i++)
                {
                    listBor[i].Tag = "-1";
                    if (i < Pages) listBor[i].Tag = "0";
                    _Slots.Add((i + 1).ToString());
                }

                OnPropertyChanged("Items");
                OnPropertyChanged("Slots");

               
                if (Pages >= 1)
                {
                    SelectedPage = 1;
                    if(listBor.Count>0) listBor[0].Tag = "1";
                    borderPre.Tag = SelectedPage > 1 ? "0" : "-1";
                    borderNext.Tag = SelectedPage < Items.Count ? "0" : "-1";
                    borderFirst.Tag = SelectedPage > 1 ? "0" : "-1";
                    borderLast.Tag = SelectedPage < Items.Count ? "0" : "-1";
                }
                else borderPre.Tag = borderNext.Tag = borderFirst.Tag = borderLast.Tag = "-1";
            }
        }

        private void setHightLight(int index)
        {
            if (index >= listBor.Count) index = listBor.Count - 1;
            listBor.ForEach(i =>
            {
                if (i.Tag.ToString() != "-1") i.Tag = "0";
            });
            listBor[index].Tag = "1";
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            listBor = new List<Border> { border1, border2, border3, border4, border5 };
            listBor.ForEach(i => i.Tag = "-1");
            refresh();
        }

        private void border_Click(object sender, MouseButtonEventArgs e)
        {
            var bor = listBor.Find(x => x == (sender as Border));
            if (bor != null)
            {
                var index = listBor.IndexOf(bor);
                var txt = (bor.Child as TextBlock).Text;
                lv.SelectedIndex = Items.FindIndex(i => i == txt);
                setHightLight(index);
                if (index == 4)
                {
                    if (lv.SelectedIndex + 2 < Items.Count && lv.SelectedIndex - 2 >= 0)
                    {
                        var list = new List<string>();
                        for (int i = lv.SelectedIndex - 2; i <= lv.SelectedIndex + 2; i++)
                        {
                            if (i >= 0 && i < Items.Count) list.Add(Items[i]);
                        }
                        for (int i = 0; i < list.Count; i++)
                        {
                            _Slots[i] = list[i];
                        }
                        setHightLight(index - 2);
                    }
                    else if (lv.SelectedIndex + 1 < Items.Count && lv.SelectedIndex - 1 >= 0)
                    {
                        var list = new List<string>();
                        for (int i = lv.SelectedIndex - 1; i <= lv.SelectedIndex + 1; i++)
                        {
                            if (i >= 0 && i < Items.Count) list.Add(Items[i]);
                        }
                        for (int i = 0; i < list.Count; i++)
                        {
                            _Slots[i] = list[i];
                        }
                        setHightLight(index - 1);
                    }
                    else
                    {
                        setHightLight(index);
                    }
                    OnPropertyChanged("Slots");
                }
                if (index == 0)
                {
                    if (lv.SelectedIndex - 2 >= 0 && lv.SelectedIndex + 2 < Items.Count)
                    {
                        var list = new List<string>();
                        for (int i = lv.SelectedIndex - 2; i <= lv.SelectedIndex + 2; i++)
                        {
                            if (i >= 0 && i < Items.Count) list.Add(Items[i]);
                        }
                        for (int i = 0; i < list.Count; i++)
                        {
                            _Slots[i] = list[i];
                        }
                        setHightLight(index + 2);
                    }
                    else if (lv.SelectedIndex - 1 >= 0 && lv.SelectedIndex + 1 < Items.Count)
                    {
                        var list = new List<string>();
                        for (int i = lv.SelectedIndex - 1; i <= lv.SelectedIndex + 1; i++)
                        {
                            if (i >= 0 && i < Items.Count) list.Add(Items[i]);
                        }
                        for (int i = 0; i < list.Count; i++)
                        {
                            _Slots[i] = list[i];
                        }
                        setHightLight(index + 1);
                    }
                    else
                    {
                        setHightLight(index);
                    }
                    OnPropertyChanged("Slots");
                }
            }
        }

        private void lv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          if(Pages > 1)
          {
                borderPre.Tag = SelectedPage > 1 ? "0" : "-1";
                borderNext.Tag = SelectedPage < Items.Count ? "0" : "-1";
                borderFirst.Tag = SelectedPage > 1 ? "0" : "-1";
                borderLast.Tag = SelectedPage < Items.Count ? "0" : "-1";
          }
          else borderPre.Tag = borderNext.Tag = borderFirst.Tag = borderLast.Tag = "-1";
            if (SelectionChange != null) SelectionChange(this, e);
        }

        private void PrePage(object sender, MouseButtonEventArgs e)
        {
            if (lv.SelectedIndex - 1 >= 0) lv.SelectedIndex--;
            var index = listBor.FindIndex(x => x.Tag.ToString() == "1");
            if (index >= 0)
            {
                if (index - 1 >= 0) setHightLight(index - 1);
                if (index - 1 == 0)
                {
                    if (lv.SelectedIndex - 2 >= 0)
                    {
                        var list = new List<string>();
                        for (int i = lv.SelectedIndex - 2; i <= lv.SelectedIndex + 2; i++)
                        {
                            if (i >= 0 && i < Items.Count) list.Add(Items[i]);
                        }
                        for (int i = 0; i < list.Count; i++)
                        {
                            _Slots[i] = list[i];
                        }
                        setHightLight(index + 1);
                    }
                    else if (lv.SelectedIndex - 1 >= 0)
                    {
                        var list = new List<string>();
                        for (int i = lv.SelectedIndex - 1; i <= lv.SelectedIndex + 1; i++)
                        {
                            if (i >= 0 && i < Items.Count) list.Add(Items[i]);
                        }
                        for (int i = 0; i < list.Count; i++)
                        {
                            _Slots[i] = list[i];
                        }
                        setHightLight(index);
                    }
                    else
                    {
                        setHightLight(index - 1);
                    }
                    OnPropertyChanged("Slots");
                }
            }
        }

        private void NextPage(object sender, MouseButtonEventArgs e)
        {
            if (lv.SelectedIndex + 1 >= 0) lv.SelectedIndex++;
            var index = listBor.FindIndex(x => x.Tag.ToString() == "1");
            if (index >= 0)
            {
                if (index + 1 >= 0) setHightLight(index + 1);
                if (index + 1 == 4)
                {
                    if (lv.SelectedIndex + 2 < Items.Count && lv.SelectedIndex - 2 >= 0)
                    {
                        var list = new List<string>();
                        for (int i = lv.SelectedIndex - 2; i <= lv.SelectedIndex + 2; i++)
                        {
                            if (i >= 0 && i < Items.Count) list.Add(Items[i]);
                        }
                        for (int i = 0; i < list.Count; i++)
                        {
                            _Slots[i] = list[i];
                        }
                        setHightLight(index - 1);
                    }
                    else if (lv.SelectedIndex + 1 < Items.Count && lv.SelectedIndex - 3 >= 0)
                    {
                        var list = new List<string>();
                        for (int i = lv.SelectedIndex - 3; i <= lv.SelectedIndex + 1; i++)
                        {
                            if (i >= 0 && i < Items.Count) list.Add(Items[i]);
                        }
                        for (int i = 0; i < list.Count; i++)
                        {
                            _Slots[i] = list[i];
                        }
                        setHightLight(index);
                    }
                    else
                    {
                        setHightLight(index + 1);
                    }
                    OnPropertyChanged("Slots");
                }
            }
        }

        private void FirstPage(object sender, MouseButtonEventArgs e)
        {
            lv.SelectedIndex = 0;
            setHightLight(0);
            for (int i = 0; i < Slots.Count; i++)
            {
                _Slots[i] = (i + 1).ToString();
            }
            OnPropertyChanged("Slots");
        }

        private void LastPage(object sender, MouseButtonEventArgs e)
        {
            lv.SelectedIndex = Items.Count - 1;
            for (int i = 0; i < listBor.Count; i++)
            {
                if (listBor[listBor.Count-1-i].Tag.ToString()!="-1")
                {
                    setHightLight(listBor.Count-1 - i);
                    break;
                }
            }
            var list = new List<string>();
            for (int i = lv.SelectedIndex - listBor.Where(k => k.Tag.ToString() == "0").ToList().Count; i < Items.Count; i++)
            {
                if (i >= 0) list.Add(Items[i]);
            }
            for (int i = 0; i < list.Count; i++)
            {
                _Slots[i] = list[i];
            }
            OnPropertyChanged("Slots");
        }
    }
}
