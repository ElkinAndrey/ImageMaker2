using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.IO;
using System.Xml.Linq;


namespace ImageMaker.Start
{
    /// <summary>
    /// Логика взаимодействия для Settings.xaml
    /// </summary>
    public partial class Settings : Page
    {
        public Settings()
        {
            InitializeComponent();
        }
        private void ButtonPathClick(object sender, RoutedEventArgs e)
        {
            var x = new FolderBrowserDialog();
            x.ShowDialog();
            textbox1.Text = x.SelectedPath;
        }

        private void ButtonSaveClick(object sender, RoutedEventArgs e)
        {

            MessageBox Error;

            if (Directory.Exists(textbox1.Text))
            {
                if (RadioButtonWhite.IsChecked == true || RadioButtonBlack.IsChecked == true)
                {
                    XDocument doc = XDocument.Load("Data.xml");
                    doc.Element("database").Element("SavePath").Value = textbox1.Text;
                    if (RadioButtonWhite.IsChecked == true)
                        doc.Element("database").Element("Inversion").Value = "white";
                    else if (RadioButtonBlack.IsChecked == true)
                        doc.Element("database").Element("Inversion").Value = "black";
                    doc.Element("database").Element("StartWindow").Value = "false";
                    doc.Save("Data.xml");
                    System.Windows.Application.Current.MainWindow.Hide();
                    System.Threading.Thread.Sleep(1000);
                    Main.MainWindow win2 = new Main.MainWindow();
                    win2.Show();
                    System.Windows.Application.Current.MainWindow.Close();

                }
                else
                {
                    Error = new MessageBox("Choose a color!");
                    Error.Show();
                }
            }
            else
            {
                if (textbox1.Text == "")
                {
                    Error = new MessageBox("Choose the path!");
                    Error.Show();
                }
                else if (!Directory.Exists(textbox1.Text))
                {
                    Error = new MessageBox("Change path!");
                    Error.Show();
                }
            }
        }
        }
}
