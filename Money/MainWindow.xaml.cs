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
            CreateOrSetTable();
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
        public void CreateOrSetTable()
        {
            DataTable dt2 = SqlDB.Select($"select * from Categories_Money where user_id = {SqlDB.UserID}");
            if (dt2.Rows.Count > 0)
            {
               SetTable();
            }
            else
            {
                DataTable dt = SqlDB.Select($"select * from Categories");
                foreach (DataRow dr in dt.Rows)
                {
                    int category_id = SqlDB.GetId($"select * from Categories where name = '{dr["name"]}'");
                    SqlDB.Command($"insert into Categories_Money values({SqlDB.UserID}, {category_id}, 0)");
                }
                SetTable();
            }
        }
        public void SetTable()
        {
            DataTable dt = SqlDB.Select($"select Categories.name as category, Categories_Money.value as value from Categories_Money join Categories on Categories_Money.category_id = Categories.id where user_id={SqlDB.UserID}");
            List<CategoryValue> categoryValues = new List<CategoryValue>();
            foreach(DataRow dr in dt.Rows)
            {
                categoryValues.Add(new CategoryValue { Category = dr["category"].ToString(), Value = dr["value"].ToString() });
            }
            Table.ItemsSource = categoryValues;
        }

        private void House_Click(object sender, RoutedEventArgs e)
        {
            HouseWindow window = new HouseWindow();
            window.Closed += new EventHandler(CloseWin);
            window.Show();
        }

        private void CloseWin(object sender, EventArgs e)
        {
            SetTable();
            SetWalletAndBalance();
        }

        private void Party_Click(object sender, RoutedEventArgs e)
        {
            PartyWindow window = new PartyWindow();
            window.Closed += new EventHandler(CloseWin);
            window.Show();
        }

        private void Transport_Click(object sender, RoutedEventArgs e)
        {
            TransportWindow window = new TransportWindow();
            window.Closed += new EventHandler(CloseWin);
            window.Show();
        }

        private void Education_Click(object sender, RoutedEventArgs e)
        {
            EducationWindow window = new EducationWindow();
            window.Closed += new EventHandler(CloseWin);
            window.Show();
        }

        private void Products_Click(object sender, RoutedEventArgs e)
        {
            ProductsWindow window = new ProductsWindow();
            window.Closed += new EventHandler(CloseWin);
            window.Show();
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow window = new SettingsWindow();
            window.Closed += new EventHandler(CloseWin);
            window.Show();
        }
    }
}
