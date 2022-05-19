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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ImageMaker.Start
{
    /// <summary>
    /// Логика взаимодействия для Begin.xaml
    /// </summary>
    public partial class Begin : Page
    {
        public Begin()
        {
            InitializeComponent();
        }
        private void ButtonBegin_Click(object sender, RoutedEventArgs e)
        {
            App.ParentWindowRef.ParentFrame.Navigate(new Settings());
        }
        private void ButtonAbout_Click(object sender, RoutedEventArgs e)
        {
            App.ParentWindowRef.ParentFrame.Navigate(new About());
        }
    }
}
