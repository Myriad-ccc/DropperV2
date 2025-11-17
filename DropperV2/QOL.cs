using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace DropperV2
{
    public static class QOL
    {
        private static readonly Random random = new Random();

        public static SolidColorBrush RandomColor() => new (Color.FromArgb(255, (byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256)));

        public static void WriteOut(object obj) => MessageBox.Show($"{obj}");
    }
}
