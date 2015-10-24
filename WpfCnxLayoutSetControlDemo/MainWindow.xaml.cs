using System;
using System.Collections.Generic;
using System.Linq;
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
using WpfCnxLayoutSetControlDemo.Models;
using WpfCnxLayoutSetControlDemo.ViewModel;

namespace WpfCnxLayoutSetControlDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private LayoutViewModel viewmodel;
        public MainWindow()
        {
            InitializeComponent();
            viewmodel  = new LayoutViewModel();
            this.DataContext = viewmodel;
        }
        private void ButtonBase_OnClick (object sender, RoutedEventArgs e)
        {
            var t = viewmodel.Devices[1];
             viewmodel.Devices.Remove(t);
            var tt = t as LcLaneAssign;
            if(tt != null)
            {
                foreach(var item in tt.Devices)
                    viewmodel.Devices.Remove(item);
            }
        }
        private void ButtonAdd_OnClick (object sender, RoutedEventArgs e)
        {
            var t = new LcOnlineTestInstrument(){ Label="ok"};
            var n = viewmodel.Devices.First(a => a is LcLaneAssign) as LcLaneAssign;
            n.Devices.Add(t);
            viewmodel.Devices.Add(t);

        }
        private void ButtonRefresh_OnClick (object sender, RoutedEventArgs e)
        {

            this.ListBox.Width = this.ListBox.Width -1;
        }
        private void ButtonReset_OnClick (object sender, RoutedEventArgs e)
        {
            if(this.ListBox.SelectedItem == null)
                return;
           var d =  this.ListBox.SelectedItem as LcNonLaneDevice;
            if(d == null) 
                return;
          var aw =   viewmodel.Devices.IndexOf(d);

            var l = viewmodel.Devices.OfType<LcLaneAssign>().FirstOrDefault(a => a.Devices.Contains(d));
            var newd = new LcOnlineTestInstrument(){ Label = "dfdf", IsReverse = d.IsReverse };
            viewmodel.Devices[aw] = newd;
            l.Devices.Remove(d);
            l.Devices.Add(newd);
        }
        private void OnContainerFocused (object sender, KeyboardFocusChangedEventArgs e)
        {
            (sender as ListBoxItem).IsSelected = true;
        }
    }
}
