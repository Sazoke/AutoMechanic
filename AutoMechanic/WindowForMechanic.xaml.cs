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
    /// <summary>
    /// Логика взаимодействия для WindowForMechanic.xaml
    /// </summary>
    public partial class WindowForMechanic : Window
    {
        private List<Order> orders;
        public WindowForMechanic()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Width = 780;
            var tabControl = new TabControl();
            tabControl.Items.Add(new TabItem() { Content = GetGridOrders(), Header = "Orders" });
            tabControl.Items.Add(new TabItem() { Content = GetGridAdmin(), Header = "Add new admin" });
            Grid.Children.Add(tabControl);
        }

        private Grid GetGridOrders()
        {
            var result = new Grid();
            if (orders is null)
                orders = GetOrders();
            var datas = new DataGrid() { AutoGenerateColumns = true, ItemsSource = orders, ColumnWidth = 250 };
            result.Children.Add(datas);
            var scroll = new ScrollViewer() { CanContentScroll = true, Visibility = Visibility.Visible };
            MouseWheel += (sender, e) => { if (e.Delta > 0) scroll.LineUp(); else scroll.LineDown(); };
            datas.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            scroll.Content = result;
            return result;
        }

        private List<Order> GetOrders()
        {
            var list = new List<Order>();
            var app = new Excel.Application();
            var workBook = app.Workbooks.Open(Directory.GetCurrentDirectory() + @"\Orders.xlsx");
            var index = 1;
            var workSheet = (Excel.Worksheet)workBook.Worksheets[1];
            while (workSheet.Cells[index, 1].Value2 != null)
                list.Add(new Order(new Client((string)workSheet.Cells[index, 1].Value2, 
                    (string)workSheet.Cells[index, 2].Value2, 
                    ((long)workSheet.Cells[index, 3].Value2).ToString()),
                    (string)workSheet.Cells[index, 4].Value2,
                    (string)workSheet.Cells[index++, 5].Value2));
            workBook.Close();
            app.Quit();
            return list;
        }

        private Grid GetGridAdmin()
        {
            var result = new Grid() { Margin = new Thickness(0) };
            result.Children.Add(WindowMaker.GetTextBlock("Login :", new Thickness(100, 0, 400, 360)));
            var login = WindowMaker.GetTextBox(new Thickness(100, 20, 400, 340));
            result.Children.Add(login);
            result.Children.Add(WindowMaker.GetTextBlock("Password :", new Thickness(100, 50, 400, 310)));
            var password = WindowMaker.GetTextBox(new Thickness(100, 70, 400, 290));
            result.Children.Add(password);
            result.Children.Add(WindowMaker.GetButton("Register", new Thickness(100, 200, 400, 100),
                (sender, e) => { AddAdmin(login.Text, password.Text.GetHashCode()); login.Text = "";password.Text = ""; }));
            return result;
        }

        private void AddAdmin(string login, int passwordHash)
        {
            var path = Directory.GetCurrentDirectory() + @"\Logins.txt";
            var txt = login + ' ' + passwordHash + ' ' + "admin";
            using (var streamWriter = new StreamWriter(path, true))
                streamWriter.WriteLine(txt);
        }
    }
}
