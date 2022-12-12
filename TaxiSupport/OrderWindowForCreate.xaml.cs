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
    /// Логика взаимодействия для OrderWindowForCreate.xaml
    /// </summary>
    public partial class OrderWindowForCreate : Window
    {
        IEnumerable<Order> OrderList;
        public OrderWindowForCreate()
        {
            InitializeComponent();
            Logo_auth.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath("../../Resource/Oe0jPt9jLVU.jpg")));
            DataContext = DataBase.GetContext().Order.ToList();
            UpdateData();
        }
        public void UpdateData()
        {
            DataBase.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
            OrderList = DataBase.GetContext().Order.ToList();
            if (!String.IsNullOrWhiteSpace(SearchBox.Text))
            {
                switch (sortComboBox.SelectedIndex)
                {
                    case 0:
                        OrderList = OrderList.Where(p => p.price.ToString().Contains(SearchBox.Text)).ToList();
                        break;
                    case 1:
                        OrderList = OrderList.Where(p => p.Driver1.car_brand_and_model.Contains(SearchBox.Text)).ToList();
                        break;
                    case 2:
                        OrderList = OrderList.Where(p => p.Driver1.name.Contains(SearchBox.Text)).ToList();
                        break;
                    case 3:
                        OrderList = OrderList.Where(p => p.description.Contains(SearchBox.Text)).ToList();
                        break;
                    case 4:
                        OrderList = OrderList.Where(p => p.number.Contains(SearchBox.Text)).ToList();
                        break;
                }
            }
            listBox.ItemsSource = OrderList;
        }

        private void ViewButton_Click(object sender, RoutedEventArgs e)
        {
            OrderWindow orderWindow = new OrderWindow(listBox.SelectedItem as Order);
            orderWindow.Owner = this;
            orderWindow.Show();
        }

        private void ChoseButton_Click(object sender, RoutedEventArgs e)
        {
            ((CreateAppealWindow)this.Owner).order = listBox.SelectedItem as Order;
            ((CreateAppealWindow)this.Owner).Order.Text = (listBox.SelectedItem as Order).number;
            this.Close();
        }
        private void SearchBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            UpdateData();
        }

        private void listBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            ViewButton.IsEnabled = true;
            ChoseButton.IsEnabled = true;
        }

        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateData();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Owner.Focus();
        }
    }
}
