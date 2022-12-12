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
    /// Логика взаимодействия для CloseAppealWindow.xaml
    /// </summary>
    public partial class CloseAppealWindow : Window
    {
        int id_judgment;
        User user;
        Appeal appeal;
        public CloseAppealWindow(Appeal appeal, User user, int id)
        {
            InitializeComponent();
            this.appeal = appeal;
            this.user = user;
            this.id_judgment = id;
            if (id == 1)
                label.Content = "Причина закрытия";
            else
            {
                label.Content = "Решение по обращению";
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messegeBoxResult = MessageBox.Show("Подтвердить действие?", "Подтверждение действия", MessageBoxButton.YesNo);
            if (messegeBoxResult == MessageBoxResult.Yes)
            {
                DataBase.GetContext().Response_to_the_appeal.Add(new Response_to_the_appeal() { date = DateTime.Now, id_appeal = appeal.ID, id_user = user.ID, Appeal = appeal, User = user, responce = DescriptionBox.Text, id_judgment = id_judgment });
                DataBase.GetContext().Appeal.Where(x => x.ID == appeal.ID).FirstOrDefault().status = 2;
                DataBase.GetContext().SaveChanges();
                ((ViewWindow)this.Owner.Owner).UpdateData();
                this.Owner.Close();
                this.Close();
            }
                
        }
    }
}
