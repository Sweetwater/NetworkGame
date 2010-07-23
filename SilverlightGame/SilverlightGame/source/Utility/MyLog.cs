using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Diagnostics;

namespace SilverlightGame.Utility
{
    public static class MyLog
    {
        public static TextBlock LogTextBlock;
        public static int OutputLevel = 0;

        public static void WriteLine(string text)
        {
            MyLog.WriteLine10(text);
        }
        public static void WriteLineError(string text)
        {
            MyLog.WriteLine(100, text);
        }

        public static void WriteLine0(string text)
        {
            MyLog.WriteLine(0, text);
        }
        public static void WriteLine10(string text)
        {
            MyLog.WriteLine(10, text);
        }
        public static void WriteLine20(string text)
        {
            MyLog.WriteLine(20, text);
        }
        public static void WriteLine30(string text)
        {
            MyLog.WriteLine(30, text);
        }

        public static void WriteLine(int level, string text)
        {
            if (MyLog.OutputLevel > level) return;

            Console.WriteLine(text);
            Debug.WriteLine(text);
            LogTextBlock.Text += text + "\n";

            var window = System.Windows.Browser.HtmlPage.Window;
            window.Eval("console.log('" + text + "');");
        }

        public static void WriteTextBlock(string text)
        {
            LogTextBlock.Text = text;
        }
    }
}
