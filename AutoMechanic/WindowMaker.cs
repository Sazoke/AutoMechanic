using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AutoMechanic
{
    public static class WindowMaker
    {
        public static TextBlock GetTextBlock(string text, Thickness thickness) =>
            new TextBlock() { Text = text, Margin = thickness };

        public static TextBox GetTextBox(Thickness thickness) =>
            new TextBox() { Margin = thickness };

        public static Button GetButton(string text, Thickness thickness, RoutedEventHandler action)
        {
            var button = new Button() { Content = text, Margin = thickness };
            button.Click += action;
            return button;
        }

        public static Grid GetGrid(ImageSource image, Thickness thickness)
        {
            var grid = GetGrid(thickness);
            grid.Margin = thickness;
            var ground = new ImageBrush();
            ground.ImageSource = image;
            grid.Background = ground;
            return grid;
        }

        public static Grid GetGrid(SolidColorBrush color, Thickness thickness)
        {
            var grid = GetGrid(thickness);
            grid.Background = color;
            return grid;
        }

        public static Grid GetGrid(Thickness thickness) => new Grid() { Margin = thickness };
        public static Grid GetGrid(string nameOfImage, Thickness thickness) => GetGrid(new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"/" + nameOfImage)), thickness);
    }
}
