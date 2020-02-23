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

namespace AutoMechanic
{
    /// <summary>
    /// Логика взаимодействия для WindowForGuest.xaml
    /// </summary>
    public partial class WindowForClient : Window
    {
        private Client client;
        public WindowForClient(Client client)
        {
            InitializeComponent();
            this.client = client;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            BuildInterface();
        }

        private void BuildInterface()
        {
            var button = new Button() { Margin = new Thickness(300, 150, 300, 150), Content = "Зарегистрировать заказ" };
            button.Click += (sender, e) => BuildFormOfOrder();
            Grid.Children.Add(button);
        }

        private void BuildFormOfOrder()
        {
            var elements = Grid.Children;
            elements.Clear();

            elements.Add(WindowMaker.GetTextBlock("Model of machine :", new Thickness(80, 150, 500, 200)));
            var machineModelBox = WindowMaker.GetTextBox(new Thickness(80, 200, 500, 180));
            elements.Add(machineModelBox);

            elements.Add(WindowMaker.GetTextBlock("Number of machine :", new Thickness(320, 150, 200, 200)));
            var machineNumberBox = WindowMaker.GetTextBox(new Thickness(320, 200, 200, 180));
            elements.Add(machineNumberBox);

            var button = new Button() { Margin = new Thickness(600, 150, 50, 150), Content = "Зарегистрировать" };
            button.Click += (sender, e) => CheckAndAddToDataBase(machineModelBox.Text, machineNumberBox.Text);
            elements.Add(button);
        }

        private void CheckAndAddToDataBase(string model, string number)
        {
            if(number.Length != 6)
            {
                MessageBox.Show("Wrong Number");
                return;
            }
            var arrayOfNumber = number.ToArray();
            int gostNumber;
            if(!char.IsLetter(arrayOfNumber[0]) ||
               !int.TryParse(number.Substring(1, 3), out gostNumber) ||
               !char.IsLetter(arrayOfNumber[4]) ||
               !char.IsLetter(arrayOfNumber[5]))
            {
                MessageBox.Show("Wrong Number");
                return;
            }

            //TODO отправка в базу данных

            Grid.Children.Clear();
        }
    }
}
