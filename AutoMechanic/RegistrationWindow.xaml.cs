using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AutoMechanic
{
    public partial class RegistrationWindow : Window
    {
        private List<TextBox> boxes = new List<TextBox>();
        private List<string> ContentOfBlocks = new List<string>() { "Login", "Password", "Name", "Surname", "Phone number" };
        public RegistrationWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            Closing += (sender, e) => { var window = new MainWindow(); window.Show(); };
            BuildInterface();
        }

        private void BuildInterface()
        {
            var Grid = WindowMaker.GetGrid("Start.jpg", new Thickness(0));
            AddChild(Grid);
            var kitGrid = WindowMaker.GetGrid(new SolidColorBrush(Color.FromArgb(200, 240, 248, 252)), new Thickness(200, 25, 200, 25));
            var elements = kitGrid.Children;
            var thicknessOfBLock = new Thickness(10, 10, 10, 300);
            var thicknessOfBox = new Thickness(10, 40, 10, 300);
            foreach (var name in ContentOfBlocks)
            {
                elements.Add(WindowMaker.GetTextBlock(name, thicknessOfBLock));
                var box = WindowMaker.GetTextBox(thicknessOfBox);
                boxes.Add(box);
                elements.Add(box);
                thicknessOfBLock = ChangeThickness(thicknessOfBLock);
                thicknessOfBox = ChangeThickness(thicknessOfBox);
            }
            elements.Add(WindowMaker.GetButton("Register", new Thickness(10, 320, 10, 10),
                (sender, e) => CheckAndAdd()));
            Grid.Children.Add(kitGrid);
        }

        private void CheckAndAdd()
        {
            for (int i = 0; i < boxes.Count; i++)
            {
                var name = boxes[i].Text;
                long x;
                if (name.Contains(' ') || name == "" || 
                    (i == 1 && name.Length < 4) || 
                    (i == 4 && (name.Length != 11 || !long.TryParse(name,out x))))
                {
                    MessageBox.Show("Wrong " + ContentOfBlocks[i]);
                    return;
                }
            }
            AddUser();
            Close();
        }

        private void AddUser()
        {
            var path = Directory.GetCurrentDirectory() + @"\Logins.txt";
            
            var txt = "";
            for (int i = 0; i < boxes.Count; i++)
            {
                if(i == 1)
                {
                    txt += boxes[i].Text.GetHashCode();
                    txt += " ";
                    continue;
                }
                txt += boxes[i].Text + ' ';
            }
            txt += "client";
            using(var streamWriter = new StreamWriter(path, true))
                streamWriter.WriteLine(txt);
        }

        private Thickness ChangeThickness(Thickness thickness) =>
            new Thickness(thickness.Left, thickness.Top + 60, thickness.Right, thickness.Bottom - 60);
    }
}
