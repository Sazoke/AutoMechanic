using System.Windows;
using System.Windows.Controls;

namespace AutoMechanic
{
    public static class WindowMaker
    {
        public static TextBlock GetTextBlock(string text, Thickness thickness, string name) =>
            new TextBlock() { Text = text, Margin = thickness, Name = name };

        public static TextBox GetTextBox(Thickness thickness, string name) =>
            new TextBox() { Margin = thickness, Name = name };
    }
}
