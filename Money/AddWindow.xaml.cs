using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        public AddWindow()
        {
            InitializeComponent();
            SetCategories();
        }
        private void SetCategories()
        {
            int operation_id = SqlDB.GetId("select * from Operations where [name]='Доход'");
            DataTable dt = SqlDB.Select($"select * from Categories where operation = {operation_id}");
            List<string> categories = new List<string>();
            foreach(DataRow dr in dt.Rows)
            {
                categories.Add(dr["name"].ToString());
            }
            Categories.ItemsSource = categories;
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if(Value.Text.Length > 0)
            {
                int category_id = SqlDB.GetId($"select * from Categories where [name]='{Categories.SelectedItem}'");
                if (SqlDB.Command($"insert into Categories_Money values({SqlDB.UserID}, {category_id}, {Value.Text})"))
                {
                    MessageBox.Show("Добавлено");
                }
            }
            else
            {
                MessageBox.Show("Введите значение");
            }
        }
    }
}
