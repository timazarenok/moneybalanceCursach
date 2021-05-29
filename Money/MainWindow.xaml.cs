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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Money
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SetWalletAndBalance();
            SetTable();
        }
        public void SetWalletAndBalance()
        {
            DataTable dt = SqlDB.Select($"select [Wallets].[name] from Settings join Wallets on Settings.wallet_id = Wallets.id where user_id = {SqlDB.UserID}");
            if(dt.Rows.Count > 0)
            {
                string wallet = dt.Rows[0]["name"].ToString();
                WalletValue.Content = "Валюта: " + wallet;
            }
            else
            {
                WalletValue.Content = "Валюта: ";
            }
            DataTable dt2 = SqlDB.Select($"select Sum([value]) as [value] from Categories_Money where user_id = {SqlDB.UserID}");
            if (dt2.Rows.Count > 0)
            {
                string balance = dt2.Rows[0]["value"].ToString();
                Balance.Content = "Общие расходы: " + balance;
            }
            else
            {
                Balance.Content = "Общие расходы: ";
            }
        }
        public void SetTable()
        {
            DataTable dt = SqlDB.Select($"select Categories.name as category, Categories_Money.value as value, Operations.name as operation from Categories_Money " +
                $"join Categories on Categories_Money.category_id = Categories.id " +
                $"join Operations on Categories.operation = Operations.id where user_id={SqlDB.UserID}");
            List<CategoryValue> categoryValues = new List<CategoryValue>();
            foreach(DataRow dr in dt.Rows)
            {
                categoryValues.Add(new CategoryValue
                {
                    Category = dr["category"].ToString(),
                    Value = dr["value"].ToString(),
                    Operation = dr["operation"].ToString()
                });
            }
            Table.ItemsSource = categoryValues;
        }

        private void CloseWin(object sender, EventArgs e)
        {
            SetTable();
            SetWalletAndBalance();
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow window = new SettingsWindow();
            window.Closed += new EventHandler(CloseWin);
            window.Show();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            AddWindow window = new AddWindow();
            window.Show();
            window.Closed += new EventHandler(CloseWin);
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            DeleteWindow window = new DeleteWindow();
            window.Show();
            window.Closed += new EventHandler(CloseWin);
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Login window = new Login();
            window.Show();
            Close();
        }
    }
}
