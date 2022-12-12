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

namespace TaxiSupport
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Logo_auth.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath("../../Resource/Oe0jPt9jLVU.jpg")));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(LoginBox.Text))
            {
                if (!String.IsNullOrEmpty(PasswordBox.Password))
                {
                    IQueryable<User> login_list = DataBase.GetContext().User.Where(p => p.login == LoginBox.Text && p.password == PasswordBox.Password);
                    if (login_list.Count() == 1)
                    {
                        MessageBox.Show("Успешная авторизация");
                        ViewWindow window = new ViewWindow(login_list.First());
                        window.Owner = this;
                        window.Show();
                        this.Hide();
                    }
                    //Окно при неверных данных
                    else
                    {
                        MessageBox.Show("Неверный логин или пароль");
                        this.LoginBox.Text = "";
                        this.PasswordBox.Password = "";
                    }
                }

            }
        }
    }
}
