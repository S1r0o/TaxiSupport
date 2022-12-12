using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Логика взаимодействия для PriceCalculatorWindow.xaml
    /// </summary>
    public partial class PriceCalculatorWindow : Window
    {
        Order order;
        public PriceCalculatorWindow(Order order)
        {
            InitializeComponent();
            this.order = order;
            DataContext = order;
            if (order.mileage < 12)
                textBox_Copy4.Text = " 15 рублей";
            else
            {
                textBox_Copy4.Text = " 20 рублей";
            }
            Calculated();
            
        }

        async void Calculated()
        {
            await Task.Run(() => Timer());
            decimal compensacion = Decimal.Parse(textBox_Copy2.Text) - order.price;
            if (compensacion < 0)
                textBox_Copy6.Text = "Нет";
            else
            {
                textBox_Copy6.Text = textBox_Copy2.Text + " - " + order.price + " = " + compensacion.ToString();
            }
            int priceMileage;
            int tariffPrice;
            int.TryParse(string.Join("", textBox_Copy1.Text.Where(c => char.IsDigit(c))), out tariffPrice);
            int.TryParse(string.Join("", textBox_Copy4.Text.Where(c => char.IsDigit(c))), out priceMileage);
            textBox_Copy5.Text = tariffPrice.ToString() + " + " + textBox.Text + " * " + priceMileage.ToString() + " + " + String.Join("", TimeSpan.FromSeconds(Double.Parse(order.travel_time)).TotalMinutes) + " * " + "10" + " + " + textBox_Copy3.Text + " = " + order.price.ToString().TrimEnd('0').TrimEnd(',');
        }

        void Timer()
        {
            Thread.Sleep(500);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Owner.Focus();
        }
    }
}
