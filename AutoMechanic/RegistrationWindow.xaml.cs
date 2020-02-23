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
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        private List<string> ContentOfBoxes = new List<string>();
        public RegistrationWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Closing += (sender, e) => { var window = new MainWindow(); window.Show(); };
            var names = new List<string>() { "Login", "Password", "Name", "Surname", "Phone number" };
            BuildInterface(names);
        }

        private void BuildInterface(List<string> names)
        {
            var elements = Grid.Children;
            var thicknessOfBLock = new Thickness(100, 50, 500, 340);
            var thicknessOfBox = new Thickness(300, 50, 300, 340);
            foreach (var name in names)
            {
                elements.Add(WindowMaker.GetTextBlock(name, thicknessOfBLock));
                var box = WindowMaker.GetTextBox(thicknessOfBox);
                ContentOfBoxes.Add(box.Text);
                elements.Add(box);
                thicknessOfBLock = ChangeThickness(thicknessOfBLock);
                thicknessOfBox = ChangeThickness(thicknessOfBox);
            }
            elements.Add(WindowMaker.GetButton("Register", new Thickness(550, 370, 50, 10),
                (sender, e) => { AddUser(); Close(); }));
        }

        private void AddUser()
        {
            //TODO:Добавление в базу
        }

        private Thickness ChangeThickness(Thickness thickness) =>
            new Thickness(thickness.Left, thickness.Top + 80, thickness.Right, thickness.Bottom - 80);
    }
}
