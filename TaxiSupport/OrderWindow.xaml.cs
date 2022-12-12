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
    /// Логика взаимодействия для OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        Order order;
        public OrderWindow(Order order)
        {
            InitializeComponent();
            Logo_auth.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath("../../Resource/Oe0jPt9jLVU.jpg")));
            this.order = order;
            DataContext = order;

        }
        private void DriverTextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DriverWindow driverWindow = new DriverWindow(order.Driver1);
            driverWindow.Owner = this;
            driverWindow.Show();
        }

        private void ChatButton_Click(object sender, RoutedEventArgs e)
        {
            ChatWindow chatWindow = new ChatWindow(order);
            chatWindow.Owner = this;
            chatWindow.Show();
        }

        private void label1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            PriceCalculatorWindow priceCalculatorWindow = new PriceCalculatorWindow(order);
            priceCalculatorWindow.Owner = this;
            priceCalculatorWindow.Show();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Owner.Focus();
        }

        private void label_Copy1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.Hide();
        }
    }
}
