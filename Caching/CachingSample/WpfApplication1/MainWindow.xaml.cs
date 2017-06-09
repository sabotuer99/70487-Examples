using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Caching;
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

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ViewModel vm = new ViewModel();

        public MainWindow()
        {
            InitializeComponent();
            
            DataContext = vm;
            
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            int? count = (int?)MemoryCache.Default.Get("employee_count");
            string message = "";
            if(count != null)
            {
                message = string.Format("Cached Employee count: {0}", count);
            } else
            {
                message = "No data found in cache";
            }

            vm.listSource.Add(message);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            vm.UpdateCache();
        }
    }
}
