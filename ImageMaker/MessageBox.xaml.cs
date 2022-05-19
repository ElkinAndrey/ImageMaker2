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
using System.Windows.Shapes;

namespace ImageMaker
{
    // Для отображения ошибок
    public partial class MessageBox : Window
    {
        public MessageBox(string s)
        {
            InitializeComponent();
            TextMessageBox.Text = s; // Сообщение, которое нужно вывести
            BorderMessageBox.MouseLeftButtonDown += new MouseButtonEventHandler(layoutRoot_MouseLeftButtonDown); // Для того, чтобы можно было переносить окно, взявшись мышкой где угодно
        }

        // Для того, чтобы можно было переносить окно, взявшись мышкой где угодно
        void layoutRoot_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) 
        { 
            this.DragMove(); 
        }

        // При нажатии на кнопку "OK" окно с ошибкой закроется
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
