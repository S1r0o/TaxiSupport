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
    /// Логика взаимодействия для CreateAppealWindow.xaml
    /// </summary>
    public partial class CreateAppealWindow : Window
    {
        public Order order;
        User user;

        public CreateAppealWindow(User user)
        {
            InitializeComponent();
            this.user = user;
            TimeBox.Text = DateTime.Now.ToString();
            ImportanceComboBox.ItemsSource = DataBase.GetContext().Importance.ToList();
            TypeProblemComboBox.ItemsSource = DataBase.GetContext().Type_problem.ToList();
        }
        private void CloseAppealButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (String.IsNullOrWhiteSpace(Order.Text))
                errors.AppendLine("Выберите заказ");
            if (ImportanceComboBox.SelectedItem == null)
                errors.AppendLine("Укажите важность");
            if (TypeProblemComboBox.SelectedItem == null)
                errors.AppendLine("Укажите тип проблемы");
            if (String.IsNullOrWhiteSpace(Applicant.Text))
                errors.AppendLine("Введите Имя заявителя");
            if (String.IsNullOrWhiteSpace(Applicant_Copy.Text))
                errors.AppendLine("Введите телефон заявителя");
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            Appeal appeal;
            Applicant applicant = new Applicant() { name = Applicant.Text, phone = Applicant_Copy.Text };
            if (DataBase.GetContext().Applicant.Contains(applicant))
            {
                appeal = new Appeal() { Importance1 = ImportanceComboBox.SelectedItem as Importance, Applicant1 = DataBase.GetContext().Applicant.Where(p => p.name == Applicant.Text && p.phone == Applicant_Copy.Text).FirstOrDefault(), description = DescriptionBox.Text, date = DateTime.Now, status = 1, User = user, Type_problem = TypeProblemComboBox.SelectedItem as Type_problem, Order1 = order };
            }
            else
            {
                appeal = new Appeal() { Importance1 = ImportanceComboBox.SelectedItem as Importance, Applicant1 = applicant, description = DescriptionBox.Text, date = DateTime.Now, status = 1, User = user, Type_problem = TypeProblemComboBox.SelectedItem as Type_problem, Order1 = order };
            }
            DataBase.GetContext().Appeal.Add(appeal);
            DataBase.GetContext().SaveChanges();
            ((ViewWindow)this.Owner).UpdateData();
            this.Owner.Show();
            this.Close();
        }

        private void Order_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            OrderWindowForCreate orderWindowForCreate = new OrderWindowForCreate();
            orderWindowForCreate.Owner = this;
            orderWindowForCreate.Show();
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Owner.Show();
        }
    }
}
