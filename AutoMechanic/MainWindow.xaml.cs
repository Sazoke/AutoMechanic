using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AutoMechanic
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            var Grid = WindowMaker.GetGrid("Start.jpg", new Thickness(0));
            AddChild(Grid);

            var kitGrid = WindowMaker.GetGrid(new SolidColorBrush(Color.FromArgb(200, 240, 248, 252)), new Thickness(250, 75, 250, 75));
            kitGrid.Background = new SolidColorBrush(Color.FromArgb(200, 240, 248, 252));
            Grid.Children.Add(kitGrid);

            BuildInterface(kitGrid);
        }

        private void BuildInterface(Grid grid)
        {
            var elements = grid.Children;
            elements.Add(WindowMaker.GetTextBlock("Login :", new Thickness(10,20,10,210)));
            var loginBox = WindowMaker.GetTextBox(new Thickness(10, 40, 10, 200));
            elements.Add(loginBox);
            elements.Add(WindowMaker.GetTextBlock("Password :", new Thickness(10, 80, 10, 160)));
            var passwordBox = new PasswordBox() { Margin = new Thickness(10, 100, 10, 140) };
            elements.Add(passwordBox);
            elements.Add(WindowMaker.GetButton("Login", new Thickness(10, 140, 10, 80),
                (sender, e) => CheckAndLogin(loginBox.Text, passwordBox.Password)));
            elements.Add(WindowMaker.GetButton("Register", new Thickness(10, 200, 10, 20),
                (sender, e) => RegisterGuest()));
        }

        private void CheckAndLogin(string login, string password)
        {
            if(login == "" || password == "")
            {
                MessageBox.Show("Wrong login or password");
                return;
            }
            var directory = Directory.GetCurrentDirectory() + @"\Logins.txt";
            var reader = new StreamReader(directory);
            var dataOfUser = "";
            while (true)
            {
                dataOfUser = reader.ReadLine();
                if (dataOfUser is null)
                    break;
                var datas = dataOfUser.Split(' ');
                if (datas[0].Equals(login))
                {
                    if (!int.Parse(datas[1]).Equals(password.GetHashCode()))
                        continue;
                    Window window;
                    if (datas.Last() == "client")
                        window = new WindowForClient(new Client(datas));
                    else
                        window = new WindowForMechanic();
                    window.Show();
                    Close();
                    return;
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
