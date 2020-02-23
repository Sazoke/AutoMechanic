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
using System.Windows.Shapes;
using Excel = Microsoft.Office.Interop.Excel;

namespace AutoMechanic
{
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

            var newOrder = new Order(client, number, model);
            AddToDatabase(newOrder);
            Grid.Children.Clear();
            BuildInterface();
        }

        private void AddToDatabase(Order order)
        {
            var app = new Excel.Application();
            var workBook = app.Workbooks.Open(Directory.GetCurrentDirectory() + @"\Orders.xlsx");
            var index = 0;
            var workSheet = (Excel.Worksheet)workBook.Worksheets[1];
            var cell = workSheet.Cells[++index, 1];
            while (cell.Value2 != null)
                cell = workSheet.Cells[++index, 1];
            workSheet.Cells[index, 1].Value2 = order.Client.Name;
            workSheet.Cells[index, 2].Value2 = order.Client.Surname;
            workSheet.Cells[index, 3].Value2 = order.Client.PhoneNumber;
            workSheet.Cells[index, 4].Value2 = order.ModelOfMachine;
            workSheet.Cells[index, 5].Value2 = order.NumberOfMachine;
            workBook.Save();
            workBook.Close();
            app.Quit();
        }
    }
}
