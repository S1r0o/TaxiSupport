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
    /// Логика взаимодействия для DriverWindow.xaml
    /// </summary>
    public partial class DriverWindow : Window
    {
        Driver driver;
        public DriverWindow(Driver driver)
        {
            InitializeComponent();
            this.driver = driver;
            DataContext = driver;
            StatusComboBox.ItemsSource = DataBase.GetContext().Status_of_the_driver_s_account.ToList();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messegeBoxResult = MessageBox.Show("Сохранить изменения?", "Сохранение изменений", MessageBoxButton.YesNo);
            if (messegeBoxResult == MessageBoxResult.Yes)
            {
                DataBase.GetContext().SaveChanges();
            }
        }

        private void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            DriverOrderWindow driverOrderWindow = new DriverOrderWindow(driver);
            driverOrderWindow.Owner = this;
            driverOrderWindow.Show();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Owner.Focus();
        }
    }
}
