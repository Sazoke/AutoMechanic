﻿using System;
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
            var elements = Grid.Children;
            elements.Add(WindowMaker.GetTextBlock("Login :", new Thickness(300, 150, 300, 220)));
            var loginBox = WindowMaker.GetTextBox(new Thickness(300, 180, 300, 210));
            elements.Add(loginBox);
            elements.Add(WindowMaker.GetTextBlock("Password :", new Thickness(300, 210, 300, 100)));
            var passwordBox = WindowMaker.GetTextBox(new Thickness(300, 240, 300, 150));
            elements.Add(passwordBox);
            elements.Add(WindowMaker.GetButton("Login", new Thickness(300, 290, 300, 80),
                (sender, e) => CheckAndLogin(loginBox.Text, passwordBox.Text)));
            elements.Add(WindowMaker.GetButton("Register", new Thickness(300, 350, 300, 20),
                (sender, e) => RegisterGuest()));
        }

        private void CheckAndLogin(string login, string password)
        {
            if(login == "" || password == "" || login.Contains(' ') || password.Contains(' '))
            {
                MessageBox.Show("Wrong login or password");
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
                    //TODO: логирование
                }
            }
            MessageBox.Show("Wrong login or password");
            return;
        }

        private void RegisterGuest()
        {
            var registrationWindow = new RegistrationWindow();
            registrationWindow.Show();
            Close();
        }
    }
}