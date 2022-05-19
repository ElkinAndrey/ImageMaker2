using System;
using System.Windows;
using System.Xml.Linq;

namespace ImageMaker
{
    public partial class App : Application
    {

        // Главный метод
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            XDocument doc = XDocument.Load("Data.xml"); // Открытие .xml файла с данными
            string StartWindow = doc.Element("database").Element("StartWindow").Value; // Приложение открывается первый раз?
            if (StartWindow == "true")
                StartupUri = new Uri("Start/MainWindow.xaml", UriKind.Relative); // Если приложение открывается в первый раз
            else if (StartWindow == "false")
                StartupUri = new Uri("Main/MainWindow.xaml", UriKind.Relative); // Если приложение открывается не в первый раз
        }

        public static Start.MainWindow ParentWindowRef; // Для создания Page при открытии в первый раз

        public static Main.MainWindow ParentWindowRef2;// Для создания Page при открытии не в первый раз
    }
}