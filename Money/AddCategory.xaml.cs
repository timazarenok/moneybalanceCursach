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
    /// Interaction logic for AddCategory.xaml
    /// </summary>
    public partial class AddCategory : Window
    {
        public AddCategory()
        {
            InitializeComponent();
            SetOperations();
        }
        private void SetOperations()
        {
            DataTable dt = SqlDB.Select($"select * from Operations");
            List<string> operations = new List<string>();
            foreach(DataRow dr in dt.Rows)
            {
                operations.Add(dr["name"].ToString());
            }
            Operations.ItemsSource = operations;
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if(Name.Text.Length > 0)
            {
                int operation_id = SqlDB.GetId($"select * from Operations where [name]='{Operations.SelectedItem}'");
                if (SqlDB.Command($"insert into Categories values ({operation_id}, '{Name.Text}')"))
                {
                    MessageBox.Show("Успешно добавлено");
                }
            }
            else
            {
                MessageBox.Show("Введите название");
            }
        }
    }
}
