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
    /// Логика взаимодействия для AppealViewWindow.xaml
    /// </summary>
    public partial class AppealViewWindow : Window
    {
        Appeal appeal;
        User user;
        public AppealViewWindow(Appeal appeal, User user)
        {
            InitializeComponent();
            this.appeal = appeal;
            this.user = user;
            DataContext = appeal;
            TypeProblemComboBox.ItemsSource = DataBase.GetContext().Type_problem.ToList();
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            DataBase.GetContext().SaveChanges();
            ((ViewWindow)this.Owner).UpdateData();
        }

        private void CloseAppealButton_Click(object sender, RoutedEventArgs e)
        {
            CloseAppealWindow closeAppealWindow = new CloseAppealWindow(appeal, user, 1);
            closeAppealWindow.Owner = this;
            closeAppealWindow.Show();
        }

        private void JudgmentButton_Click(object sender, RoutedEventArgs e)
        {
            CloseAppealWindow closeAppealWindow = new CloseAppealWindow(appeal, user, 3);
            closeAppealWindow.Owner = this;
            closeAppealWindow.Show();
        }

        private void Order_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            OrderWindow orderWindow = new OrderWindow(appeal.Order1);
            orderWindow.Owner = this;
            orderWindow.Show();
        }

        private void BackButton_Copy_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            this.Owner.Show();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Owner.Show();
        }

        private void FaqButton_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            FAQWindow fAQWindow = new FAQWindow();
            fAQWindow.Owner = this;
            fAQWindow.Show();
        }
    }
}
