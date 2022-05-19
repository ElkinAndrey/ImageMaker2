using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Drawing;
using Microsoft.Win32;
using System.Xml.Linq;
using System.Windows.Media.Animation;

namespace ImageMaker.Main
{
    public partial class MainWindow : Window
    {
        private static string pathe = ""; // Путь к картинке
        private static string SavePath; // Путь сохранения
        private XDocument doc = XDocument.Load("Data.xml"); // Файл с данными
        public MainWindow()
        {
            InitializeComponent(); // Надо
            MouseLeftButtonDown += new MouseButtonEventHandler(layoutRoot_MouseLeftButtonDown); // Чтобы можно было перемещать окно приложения, взявшись где угодно
            SavePath = doc.Element("database").Element("SavePath").Value;
            // Установить цвет
            if (doc.Element("database").Element("Inversion").Value == "white")
                RadioButton1.IsChecked = true;
            else if (doc.Element("database").Element("Inversion").Value == "black")
                RadioButton2.IsChecked = true;
            else { /*Обработать исключение при изменении файла с данными*/}
            // Установить слайдер 
            slider1.Value = int.Parse(doc.Element("database").Element("Contrast").Value);
            doc.Save("Data.xml");
        }

        // Чтобы можно было перемещать окно приложения, взявшись где угодно
        void layoutRoot_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        // Загрузить Page1 для открытия О ПРИЛОЖЕНИИ поверх всего приложения
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            App.ParentWindowRef2 = this;
            this.ParentFrame.Navigate(new Page1());
        }

        // Открыть выезжающую слева панель настроек
        private void btnopen_Click(object sender, RoutedEventArgs e)
        {

            Dark.Width = 1000;
            Dark.Height = 575;
            Storyboard sb = Resources["OpenMenu"] as Storyboard;
            sb.Begin(LeftMenu);
        }

        // Закрыть выезжающую слева панель настроек
        private void btnclose_Click(object sender, RoutedEventArgs e)
        {

            Dark.Width = 0;
            Dark.Height = 0;
            Storyboard sb = Resources["CloseMenu"] as Storyboard;
            sb.Begin(LeftMenu);
        }

        // Открыть выезжающую слева панель с изменением пути сохранения
        private void btnopensavepath_Click(object sender, RoutedEventArgs e)
        {
            savepath.Text = SavePath;
            Storyboard sb = Resources["OpenSavePath"] as Storyboard;
            sb.Begin(LeftMenuSavePath);
        }
        
        // Изменить путь сохранения
        private void SetSavePath(object sender, RoutedEventArgs e)
        {
            var x = new System.Windows.Forms.FolderBrowserDialog();
            x.ShowDialog();
            savepath.Text = x.SelectedPath;
        }

        // Закрыть выезжающую слева панель с изменением пути сохранения
        private void btnclosesavepath_Click(object sender, RoutedEventArgs e)
        {

            if (Directory.Exists(savepath.Text))
            {
                doc.Element("database").Element("SavePath").Value = savepath.Text;
                doc.Save("Data.xml");
                SavePath = savepath.Text;
                Storyboard sb = Resources["CloseSavePath"] as Storyboard;
                sb.Begin(LeftMenuSavePath);
            }
            else
            {
                MessageBox Error;
                if (savepath.Text == "")
                {
                    Error = new MessageBox("Choose the path!");
                    Error.Show();
                }
                else if (!Directory.Exists(savepath.Text))
                {
                    Error = new MessageBox("Change path!");
                    Error.Show();
                }
            }
        }

        // Открыть выезжающую слева панель с изменением цветов приложения
        private void btnopencolors_Click(object sender, RoutedEventArgs e)
        {
            Storyboard sb = Resources["OpenColors"] as Storyboard;
            sb.Begin(LeftMenuColors);
        }

        // Закрыть выезжающую слева панель с изменением цветов приложения
        private void btnclosecolors_Click(object sender, RoutedEventArgs e)
        {
            Storyboard sb = Resources["CloseColors"] as Storyboard;
            sb.Begin(LeftMenuColors);
        }

        // Открыть выезжающую слева панель с языком
        private void btnopenlanguage_Click(object sender, RoutedEventArgs e)
        {
            Storyboard sb = Resources["OpenLanguage"] as Storyboard;
            sb.Begin(LeftMenuLanguage);
        }

        // Закрыть выезжающую слева панель с языком
        private void btncloselanguage_Click(object sender, RoutedEventArgs e)
        {
            Storyboard sb = Resources["CloseLanguage"] as Storyboard;
            sb.Begin(LeftMenuLanguage);
        }

        // Открыть выезжающую слева панель с дополнительными настройками
        private void btnopensettings_Click(object sender, RoutedEventArgs e)
        {
            Storyboard sb = Resources["OpenSettings"] as Storyboard;
            sb.Begin(LeftMenuSettings);
        }

        // Закрыть выезжающую слева панель с дополнительными настройками
        private void btnclosesettings_Click(object sender, RoutedEventArgs e)
        {
            Storyboard sb = Resources["CloseSettings"] as Storyboard;
            sb.Begin(LeftMenuSettings);
        }

        // Открыть окно переустановки приложения
        private void btnopenreset_Click(object sender, RoutedEventArgs e)
        {
            Main.ResetApp s = new Main.ResetApp();
            s.Show();
        }

        // Открыть вкладку о приложении
        private void btnopenabout_Click(object sender, RoutedEventArgs e)
        {
            App.ParentWindowRef2 = this;
            this.ParentFrame.Navigate(new Information());
        }

        // При переносе картинки в приложение
        private void text_PreviewDrop(object sender, DragEventArgs e)
        {

            try
            {
                string[] filenames = (string[])e.Data.GetData(DataFormats.FileDrop, true);
                Uri uri = new Uri(filenames[0], UriKind.Absolute);
                ImageSource imgSource = new BitmapImage(uri);
                Bitmap img = new Bitmap(filenames[0]);
                size1.Text = ((int)(img.Width)).ToString();
                size2.Text = ((int)(img.Height)).ToString();
                text.Source = imgSource;
                e.Handled = true;

                path.Text = filenames[0];
                pathe = filenames[0];
            }
            catch
            {
                MessageBox Error = new MessageBox("Choose the path to the picture!");
                Error.Show();
            }
        }

        // Создать текст из картинки
        private void Collect_Click(object sender, RoutedEventArgs e)
        {

            if (File.Exists(pathe))
            {
                Collect.Width = 0; // Скрыть кнопку collect
                Path.Width = 0; // Скрыть кнопку Path
                PBbar.Width = 472; // Показать загруску картинки
                fun(); // Запустить создание картинки
            }
            else
            {
                MessageBox Error;
                if (pathe == "")
                {
                    Error = new MessageBox("Choose the path!");
                    Error.Show();
                }
                else
                {
                    Error = new MessageBox("Change path!");
                    Error.Show();
                }
            }
        }

        // Запуск создания картинки
        private async void fun()
        {
            PBbar.Value = 0; // Установить загрузку в начальное состояние
            var progress = new Progress<int>(x => PBbar.Value = x);

            // Сгенерировать путь сохранения
            string savePath = "txt";
            char f = '.';
            int i = 1;
            while (f != '\\')
            {
                savePath = f + savePath;
                f = pathe[pathe.Length - i];
                i++;
            }
            savePath = SavePath + "\\" + savePath;

            // Указание белый или черный цвет
            string mode = "";
            if (RadioButton1.IsChecked == true)
                mode = "white";
            else if (RadioButton2.IsChecked == true)
                mode = "black";
            else { /*Обработать ошибку, если не указан цвет inversion*/ mode = "white"; }

            // Генерация строки с картинкой
            ImageMakerEngine IME = new ImageMakerEngine(pathe, mode, (int)slider1.Value, int.Parse(size1.Text), int.Parse(size2.Text), progress);
            await Task.Run(() => IME.Run());
            string s = IME.image;
            IME = null;

            // Создание текстового файла
            StreamWriter sw = new StreamWriter(savePath);
            sw.WriteLine(s);
            sw.Close();
            GC.Collect();

            PBbar.Value = 100;

            // Показать кнопки и скрыть загрузку
            Collect.Width = 185;
            Path.Width = 185;
            PBbar.Width = 0;
        }

        // Открыть путь к картинке
        private void Path_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                try
                {
                    string s = dlg.FileName;
                    Uri uri = new Uri(s, UriKind.Absolute);
                    ImageSource imgSource = new BitmapImage(uri);
                    Bitmap img = new Bitmap(s);
                    size1.Text = ((int)(img.Width)).ToString();
                    size2.Text = ((int)(img.Height)).ToString();
                    text.Source = imgSource;

                    pathe = s;
                    path.Text = s;
                }
                catch
                {
                    MessageBox Error = new MessageBox("Choose the path to the picture!");
                    Error.Show();
                }

            }
        }

        // При нажатии на переключатель на белый
        private void RadioButton1_Checked(object sender, RoutedEventArgs e)
        {
            XDocument doc = XDocument.Load("Data.xml");
            doc.Element("database").Element("Inversion").Value = "white";
            doc.Save("Data.xml");
        }

        // При нажатии на переключатель на черный
        private void RadioButton2_Checked(object sender, RoutedEventArgs e)
        {
            XDocument doc = XDocument.Load("Data.xml");
            doc.Element("database").Element("Inversion").Value = "black";
            doc.Save("Data.xml");
        }

        private void contrast_TextChange(object sender, TextChangedEventArgs e)
        {
            // Чтобы можно было вводить только цифры
            int caretPostion = contrast.CaretIndex; // Позиция каретки
            string s = ""; // Строка, которую нужно вставить
            string[] mas = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            flag2 = true;
            for (int i = 0; i < contrast.Text.Length; i++)
            {
                flag = true;
                foreach (string ma in mas)
                {
                    if ((contrast.Text[i]).ToString() == ma)
                    {
                        s += ma;
                        flag = false;
                        break;
                    }
                }
                if (flag && flag2)
                {
                    caretPostion = i;
                    flag2 = false;
                }
            }
            if (s != "")
            {
                double g = Convert.ToDouble(s);
                if (g > 512)
                {
                    slider1.Value = 512;
                    contrast.Text = "512";
                    contrast.CaretIndex = 3;
                }
                else
                {
                    slider1.Value = g;
                    contrast.Text = Convert.ToString((int)g);
                    contrast.CaretIndex = caretPostion;
                }
            }
            else
            {
                slider1.Value = 0;
                contrast.Text = "0";
                contrast.CaretIndex = 1;
            }
        }


        // При изменении контрасности при помощи слайдера
        private void slider1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            XDocument doc = XDocument.Load("Data.xml");
            doc.Element("database").Element("Contrast").Value = (slider1.Value).ToString();
            doc.Save("Data.xml");
        }

        private bool flag = true, flag2 = true;
        // При вводе ширины картинки

        private void size1_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Чтобы можно было вводить только цифры
            int caretPostion = size1.CaretIndex; // Позиция каретки
            string s = ""; // Строка, которую нужно вставить
            string[] mas = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            flag2 = true;
            for (int i = 0; i < size1.Text.Length; i++)
            {
                flag = true;
                foreach (string ma in mas)
                {
                    if ((size1.Text[i]).ToString() == ma)
                    {
                        s += ma;
                        flag = false;
                        break;
                    }
                }
                if (flag && flag2)
                {
                    caretPostion = i;
                    flag2 = false;
                }
            }
            size1.Text = s;
            size1.CaretIndex = caretPostion;
        }

        // При вводе высоты картинки
        private void size2_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Можно вводить только цифры
            int caretPostion = size2.CaretIndex;
            string s = "";
            string[] mas = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            flag2 = true;
            for (int i = 0; i < size2.Text.Length; i++)
            {
                flag = true;
                foreach (string ma in mas)
                {
                    if ((size2.Text[i]).ToString() == ma)
                    {
                        s += ma;
                        flag = false;
                        break;
                    }
                }
                if (flag && flag2)
                {
                    caretPostion = i;
                    flag2 = false;
                }
            }
            size2.Text = s;
            size2.CaretIndex = caretPostion;
        }

        // При нажатии копки закрытия приложения
        private void Button_Click(object sender, RoutedEventArgs e) { Environment.Exit(0); }

        // При нажатии кнопки сварачивания приложения
        private void Button_Click2(object sender, RoutedEventArgs e) { MyWindow.WindowState = WindowState.Minimized; }
    }
}