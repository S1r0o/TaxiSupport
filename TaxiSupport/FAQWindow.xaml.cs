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
using System.Windows.Shapes;

namespace TaxiSupport
{
    /// <summary>
    /// Логика взаимодействия для FAQWindow.xaml
    /// </summary>
    public partial class FAQWindow : Window
    {
        IEnumerable<FAQ> OrderList;
        public FAQWindow()
        {
            InitializeComponent();
            Logo_auth.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath("../../Resource/Oe0jPt9jLVU.jpg")));
            DataContext = DataBase.GetContext().FAQ.ToList();
            UpdateData();
        }

        public void UpdateData()
        {
            DataBase.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
            OrderList = DataBase.GetContext().FAQ.ToList();
            if (!String.IsNullOrWhiteSpace(SearchBox.Text))
            {
                OrderList = OrderList.Where(p => p.title.Contains(SearchBox.Text) || p.description.Contains(SearchBox.Text)).ToList();
            }
            listBox.ItemsSource = OrderList;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ViewingApostWindow orderWindow = new ViewingApostWindow(listBox.SelectedItem as FAQ);
            orderWindow.Owner = this;
            orderWindow.Show();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateData();
        }
        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewButton.IsEnabled = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Owner.Focus();
        }
    }
}
