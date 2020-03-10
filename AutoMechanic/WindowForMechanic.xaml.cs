using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Media;
using System.Text;

namespace AutoMechanic
{
    public partial class WindowForMechanic : Window
    {
        private List<Order> ordersForConsideration;
        private List<Order> ordersInProgress;
        private Grid progressGid;
        private Grid considerationGrid;
        private Excel.Workbook workBook;
        public WindowForMechanic()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            var application = new Excel.Application();
            var excelProcess = System.Diagnostics.Process.GetProcessesByName("EXCEL").Last();
            workBook =  application.Workbooks.Open(Directory.GetCurrentDirectory() + @"\Orders.xlsx");
            BuildInterface();
            Closing += (sender, e) =>
            {
                workBook.Save();
                workBook.Close();
                application.Quit();
                excelProcess.Kill();
            };
        }

        private void BuildInterface()
        {
            var tabControl = new TabControl();
            SetGridOrders(ref considerationGrid, workBook.Sheets[1], (SelectedCellsChangedEventHandler)ActionWithConsiderationOrder, ref ordersForConsideration);
            SetGridOrders(ref progressGid, workBook.Sheets[2], (SelectedCellsChangedEventHandler)ActionWithOrderInProgress, ref ordersInProgress);
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
            tabControl.Items.Add(new TabItem()
            {
                Content = GetHelpGrid(),
                Header = "Help"
            });
        }

        private void ActionWithOrderInProgress(object sender, SelectedCellsChangedEventArgs e)
        {
            var datas = (DataGrid)sender;
            if (datas.SelectedCells.Count == 0)
                return;
            var dialogResult = MessageBox.Show("Are you complete order?", "Compliting order", MessageBoxButton.YesNo);
            if (dialogResult == MessageBoxResult.Yes)
            {
                MessageBox.Show("Call to : " + ordersInProgress[datas.SelectedIndex].Client);
                WindowForClient.AddToDatabase(ordersInProgress[datas.SelectedIndex], workBook.Sheets[3]);
                RemoveOrderAt(datas.SelectedIndex, workBook.Sheets[2]);
            }
            datas.UnselectAll();
            workBook.Save();
        }

        private Grid GetHelpGrid()
        {
            var result = WindowMaker.GetGrid(new Thickness(0));
            result.Children.Add(new TextBlock() 
            { 
                Text = "1. If you want to agree order, then click on order and press \"Yes\", if you disagree then press \"No\".\n2. If you to complete the order, then click on order and press \"Yes\". \n3. Register new admin" 
            });
            return result;
        }

        private void ActionWithConsiderationOrder(object sender,  SelectedCellsChangedEventArgs e)
        {
            var datas = (DataGrid)sender;
            if (datas.SelectedCells.Count == 0)
                return;
            var dialogResult = MessageBox.Show("Add order?", "Adding order to base", MessageBoxButton.YesNoCancel);
            if (dialogResult == MessageBoxResult.Yes)
            {
                if (ordersInProgress != null)
                    ordersInProgress.Add(ordersForConsideration[datas.SelectedIndex]);
                WindowForClient.AddToDatabase(ordersForConsideration[datas.SelectedIndex], workBook.Sheets[2]);
                SetGridOrders(ref progressGid, workBook.Sheets[2], (SelectedCellsChangedEventHandler)ActionWithOrderInProgress, ref ordersInProgress);
                RemoveOrderAt(datas.SelectedIndex, workBook.Sheets[1]);
            }
            else if (dialogResult == MessageBoxResult.No)
                RemoveOrderAt(datas.SelectedIndex, workBook.Sheets[1]);
            workBook.Save();
        }


        private void SetGridOrders(ref Grid grid, Excel.Worksheet workSheet, SelectedCellsChangedEventHandler selectedCellsChangedEventHandler, ref List<Order> orders)
        {
            if (grid is null)
                grid = new Grid();
            else
                grid.Children.Clear();
            if (orders is null)
                orders = GetOrders(workSheet);
            var datas = new DataGrid() { AutoGenerateColumns = true, ItemsSource = orders, ColumnWidth = 250, ColumnHeaderHeight = 30 };
            grid.Children.Add(datas);
            datas.SelectedCellsChanged += selectedCellsChangedEventHandler;
            var scroll = new ScrollViewer() { CanContentScroll = true, Visibility = Visibility.Visible };
            MouseWheel += (sender, e) => { if (e.Delta > 0) scroll.LineUp(); else scroll.LineDown(); };
            datas.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            scroll.Content = grid;
            workBook.Save();
        }

        private List<Order> GetOrders(Excel.Worksheet workSheet)
        {
            var list = new List<Order>();
            var index = 1;
            while (workSheet.Cells[index, 1].Value2 != null)
                list.Add(new Order(new Client((string)workSheet.Cells[index, 1].Value2, 
                    (string)workSheet.Cells[index, 2].Value2, 
                    ((long)workSheet.Cells[index, 3].Value2).ToString()),
                    (string)workSheet.Cells[index, 4].Value2,
                    (string)workSheet.Cells[index++, 5].Value2));
            return list;
        }

        private void RemoveOrderAt(int index, Excel.Worksheet workSheet)
        {
            if (workSheet.Index == 2)
            {
                ordersInProgress.RemoveAt(index);
                SetGridOrders(ref progressGid,workSheet , ActionWithOrderInProgress, ref ordersInProgress);
            }
            else
            {
                ordersForConsideration.RemoveAt(index);
                SetGridOrders(ref considerationGrid, workSheet, ActionWithConsiderationOrder, ref ordersForConsideration);
            }
            index++;
            while (workSheet.Cells[index, 1].Value2 != null)
            {
                for (int i = 1; i < 6; i++)
                    workSheet.Cells[index, i].Value2 = workSheet.Cells[index + 1, i].Value2;
                index++;
            }
            workBook.Save();
        }

        private Grid GetGridAdmin()
        {
            var result = WindowMaker.GetGrid("Mechanic.jpg", new Thickness(0));
            var kitGrid = WindowMaker.GetGrid(new SolidColorBrush(Color.FromArgb(200, 240, 248, 252)), new Thickness(250, 75, 250, 75));
            kitGrid.Children.Add(WindowMaker.GetTextBlock("Login :", new Thickness(10, 10, 10, 200)));
            var login = WindowMaker.GetTextBox(new Thickness(10, 40, 10, 170));
            kitGrid.Children.Add(login);
            kitGrid.Children.Add(WindowMaker.GetTextBlock("Password :", new Thickness(10, 70, 10, 140)));
            var password = WindowMaker.GetTextBox(new Thickness(10, 100, 10, 110));
            kitGrid.Children.Add(password);
            kitGrid.Children.Add(WindowMaker.GetButton("Register", new Thickness(10, 140, 10, 50),
                (sender, e) => { AddAdmin(login.Text, password.Text.GetHashCode()); login.Text = ""; password.Text = ""; }));
            result.Children.Add(kitGrid);
            return result;
        }

        private void AddAdmin(string login, int passwordHash)
        {
            var path = Directory.GetCurrentDirectory() + @"\Logins.txt";
            var txt = login + ' ' + passwordHash + ' ' + "admin";
            byte[] bytes = Encoding.ASCII.GetBytes(txt);
            using (var streamWriter = new StreamWriter(path, true))
            {
                for (var i = 0; i < bytes.Length - 1; i++)
                    streamWriter.Write(bytes[i].ToString() + ' ');
                streamWriter.Write(bytes.Last());
                streamWriter.WriteLine();
            }
        }
    }
}
