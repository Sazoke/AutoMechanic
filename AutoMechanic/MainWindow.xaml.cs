using System;
using System.Collections.Generic;
using System.IO;
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


namespace AutoMechanic
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            BuildInterface();
        }

        private void BuildInterface()
        {
            Grid.Children.Add(WindowMaker.GetTextBlock("Login :", new Thickness(300, 150, 300, 220), "Login"));
            var loginBox = WindowMaker.GetTextBox(new Thickness(300, 180, 300, 210), "LoginBox");
            Grid.Children.Add(loginBox);
            Grid.Children.Add(WindowMaker.GetTextBlock("Password :", new Thickness(300, 210, 300, 100), "Password"));
            var passwordBox = WindowMaker.GetTextBox(new Thickness(300, 240, 300, 150), "PasswordBox");
            Grid.Children.Add(passwordBox);
            var button = new Button() { Margin = new Thickness(300, 300, 300, 50), Content = "Login" };
            button.Click += (sender, e) => CheckAndLogin(loginBox.Text, passwordBox.Text);
            Grid.Children.Add(button);
        }

        private void CheckAndLogin(string login, string password)
        {
            if(login == "" || password == "" || login.Contains(' ') || password.Contains(' '))
            {
                MessageBox.Show("Wrong login and password");
                return;
            }
            var directory = Directory.GetCurrentDirectory() + @"\Logins.txt";
            var reader = new StreamReader(directory);
            var dataOfUser = "";
            while (dataOfUser != null)
            {
                dataOfUser = reader.ReadLine();
                var datas = dataOfUser.Split(' ');
                if (datas[0].Equals(login))
                {
                    if (!int.Parse(datas[1]).Equals(password.GetHashCode()))
                        break;
                    
                }
            }
            MessageBox.Show("Wrong login and password");
            return;
        }
    }
}
