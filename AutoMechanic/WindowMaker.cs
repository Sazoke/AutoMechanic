using System;
using System.Windows;
using System.Windows.Controls;

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
    }
}
