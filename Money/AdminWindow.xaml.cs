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

namespace Money
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
        }

        private void Category_Click(object sender, RoutedEventArgs e)
        {
            AddCategory window = new AddCategory();
            window.Show();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Login window = new Login();
            window.Show();
            Close();
        }
    }
}
