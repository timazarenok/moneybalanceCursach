using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for HouseWindow.xaml
    /// </summary>
    public partial class HouseWindow : Window
    {
        public HouseWindow()
        {
            InitializeComponent();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            int category_id = SqlDB.GetId("select * from Categories where name = 'Домашние расходы'");
            if(SqlDB.Command($"update Categories_Money set value += {Value.Text} where user_id = {SqlDB.UserID} and category_id = {category_id}"))
            {
                MessageBox.Show("Добавлено");
            }
        }

        private void Add_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
