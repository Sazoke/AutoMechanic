using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Excel = Microsoft.Office.Interop.Excel;

namespace AutoMechanic
{
    public partial class WindowForClient : Window
    {
        private Client client;
        private static Excel.Workbook workBook;
        public WindowForClient(Client client)
        {
            InitializeComponent();
            var app = new Excel.Application();
            var excelProcess = System.Diagnostics.Process.GetProcessesByName("EXCEL").Last();
            workBook = app.Workbooks.Open(Directory.GetCurrentDirectory() + @"\Orders.xlsx");
            this.client = client;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            var ground = new ImageBrush();
            ground.ImageSource = new BitmapImage(new System.Uri(Directory.GetCurrentDirectory() + @"/ClientWindow.jpg"));
            Background = ground;
            BuildInterface();
            Closing += (sender, e) =>
            {
                workBook.Save();
                workBook.Close();
                app.Quit();
                excelProcess.Kill();
            };
        }

        private void BuildInterface()
        {
            var button = new Button() { Margin = new Thickness(300, 150, 300, 150), Content = "Зарегистрировать заказ" };
            button.Click += (sender, e) => BuildFormOfOrder();
            Grid.Children.Add(button);
        }

        private void BuildFormOfOrder()
        {
            Grid.Children.Clear();
            var kitGrid = WindowMaker.GetGrid(new SolidColorBrush(Color.FromArgb(200, 240, 248, 252)), new Thickness(250, 75, 250, 75));

            var elements = kitGrid.Children;
            elements.Add(WindowMaker.GetTextBlock("Model of machine :", new Thickness(10, 45, 10, 135)));
            var machineModelBox = WindowMaker.GetTextBox(new Thickness(10, 65, 10, 175));
            elements.Add(machineModelBox);

            elements.Add(WindowMaker.GetTextBlock("Number of machine :", new Thickness(10, 105, 10, 135)));
            var machineNumberBox = WindowMaker.GetTextBox(new Thickness(10, 125, 10, 115));
            elements.Add(machineNumberBox);

            var button = new Button() { Margin = new Thickness(10, 180, 10, 40), Content = "Зарегистрировать" };
            button.Click += (sender, e) => CheckAndAddToDataBase(machineModelBox.Text, machineNumberBox.Text);
            elements.Add(button);
            Grid.Children.Add(kitGrid);
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

            var newOrder = new Order(client, model, number);
            AddToDatabase(newOrder, workBook.Sheets[1]);
            Grid.Children.Clear();
            BuildInterface();
        }

        public static void AddToDatabase(Order order, Excel.Worksheet workSheet)
        {
            var index = 0;
            var cell = workSheet.Cells[++index, 1];
            while (cell.Value2 != null)
                cell = workSheet.Cells[++index, 1];
            workSheet.Cells[index, 1].Value2 = order.Client.Name;
            workSheet.Cells[index, 2].Value2 = order.Client.Surname;
            workSheet.Cells[index, 3].Value2 = order.Client.PhoneNumber;
            workSheet.Cells[index, 4].Value2 = order.ModelOfMachine;
            workSheet.Cells[index, 5].Value2 = order.NumberOfMachine;
        }
    }
}
