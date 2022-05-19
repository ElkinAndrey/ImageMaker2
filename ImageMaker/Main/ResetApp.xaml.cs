using System;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;

namespace ImageMaker.Main
{
    public partial class ResetApp : Window
    {
        public ResetApp()
        {
            InitializeComponent();
            MouseLeftButtonDown += new MouseButtonEventHandler(layoutRoot_MouseLeftButtonDown);
        }

        void layoutRoot_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            XDocument doc = XDocument.Load("Data.xml");
            MouseLeftButtonDown += new MouseButtonEventHandler(layoutRoot_MouseLeftButtonDown);

            doc.Element("database").Element("Contrast").Value = "256";
            doc.Element("database").Element("StartWindow").Value = "true";
            doc.Element("database").Element("SavePath").Value = "";
            doc.Element("database").Element("Inversion").Value = "";

            doc.Save("Data.xml");

            Environment.Exit(0);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

