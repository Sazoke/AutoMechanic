using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Excel = Microsoft.Office.Interop.Excel;


namespace AutoMechanic
{
    public partial class WindowForMechanic : Window
    {
        private List<Order> ordersForConsideration;
        private List<Order> ordersInProgress;
        private Grid progressGid;
        private Grid considerationGrid;
        public WindowForMechanic()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            BuildInterface();
        }

        private void BuildInterface()
        {
            var tabControl = new TabControl();
            SetGridOrders(ref considerationGrid,  "ConsiderationOrders.xlsx", (sender, e) => ActionWIthConsiderationOrder((DataGrid)sender), ref ordersForConsideration);
            SetGridOrders(ref progressGid, "OrdersInProgress.xlsx", (sender, e) => ActionWithOrderInProgress((DataGrid)sender), ref ordersInProgress);
            tabControl.Items.Add(new TabItem()
            {
                Content = considerationGrid,
                Header = "Orders for consideration"
            });
            tabControl.Items.Add(new TabItem()
            {
                Content = progressGid,
                Header = "Orders in progress"
            });
            tabControl.Items.Add(new TabItem() { Content = GetGridAdmin(), Header = "Add new admin" });
            Grid.Children.Add(tabControl);
        }

        private void ActionWithOrderInProgress(DataGrid datas)
        {
            if (datas.SelectedCells.Count == 0)
                return;
            var dialogResult = MessageBox.Show("Are you complete order?", "Compliting order", MessageBoxButton.YesNo);
            if (dialogResult == MessageBoxResult.Yes)
            {
                MessageBox.Show("Call to : " + ordersInProgress[datas.SelectedIndex].Client);
                RemoveOrderAt(datas.SelectedIndex, "OrdersInProgress.xlsx");
            }
            datas.UnselectAll();
        }

        private void ActionWIthConsiderationOrder(DataGrid datas)
        {
            if (datas.SelectedCells.Count == 0)
                return;
            var dialogResult = MessageBox.Show("Add order?", "Adding order to base", MessageBoxButton.YesNoCancel);
            if (dialogResult == MessageBoxResult.Yes)
            {
                if (ordersInProgress != null)
                    ordersInProgress.Add(ordersForConsideration[datas.SelectedIndex]);
                WindowForClient.AddToDatabase(ordersForConsideration[datas.SelectedIndex], "OrdersInProgress.xlsx");
                SetGridOrders(ref progressGid, "OrdersInProgress.xlsx", (sender, e) => ActionWithOrderInProgress((DataGrid)sender), ref ordersInProgress);
                RemoveOrderAt(datas.SelectedIndex, "ConsiderationOrders.xlsx");
            }
            else if (dialogResult == MessageBoxResult.No)
                RemoveOrderAt(datas.SelectedIndex, "ConsiderationOrders.xlsx");
        }
        private void SetGridOrders(ref Grid grid,string nameOfFile, SelectedCellsChangedEventHandler selectedCellsChangedEventHandler, ref List<Order> orders)
        {
            if (grid is null)
                grid = new Grid();
            else
                grid.Children.Clear();
            if (orders is null)
                orders = GetOrders(nameOfFile);
            var datas = new DataGrid() { AutoGenerateColumns = true, ItemsSource = orders, ColumnWidth = 250, ColumnHeaderHeight = 30 };
            grid.Children.Add(datas);
            datas.SelectedCellsChanged += selectedCellsChangedEventHandler;
            var scroll = new ScrollViewer() { CanContentScroll = true, Visibility = Visibility.Visible };
            MouseWheel += (sender, e) => { if (e.Delta > 0) scroll.LineUp(); else scroll.LineDown(); };
            datas.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            scroll.Content = grid;
        }

        private List<Order> GetOrders(string nameOfFile)
        {
            var list = new List<Order>();
            var app = new Excel.Application();
            var excelProcess = System.Diagnostics.Process.GetProcessesByName("EXCEL").Last();
            var workBook = app.Workbooks.Open(Directory.GetCurrentDirectory() + @"\" + nameOfFile);
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
            excelProcess.Kill();
            return list;
        }

        private void RemoveOrderAt(int index, string nameOfFile)
        {
            if (nameOfFile == "OrdersInProgress.xlsx")
            {
                ordersInProgress.RemoveAt(index);
                SetGridOrders(ref progressGid,"OrdersInProgress.xlsx", (sender, e) => ActionWithOrderInProgress((DataGrid)sender), ref ordersInProgress);
            }
            else
            {
                ordersForConsideration.RemoveAt(index);
                SetGridOrders(ref considerationGrid, "ConsiderationOrders.xlsx", (sender, e) => ActionWIthConsiderationOrder((DataGrid)sender), ref ordersForConsideration);
            }
            index++;
            var app = new Excel.Application();
            var excelProcess = System.Diagnostics.Process.GetProcessesByName("EXCEL").Last();
            var workBook = app.Workbooks.Open(Directory.GetCurrentDirectory() + @"\" + nameOfFile);
            var workSheet = (Excel.Worksheet)workBook.Worksheets[1];
            while (workSheet.Cells[index, 1].Value2 != null)
            {
                for (int i = 1; i < 6; i++)
                    workSheet.Cells[index, i].Value2 = workSheet.Cells[index + 1, i].Value2;
                index++;
            }
            workBook.Save();
            workBook.Close();
            app.Quit();
            excelProcess.Kill();
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
