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
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            SetWallets();
        }
        public void SetWallets()
        {
            DataTable dt = SqlDB.Select("select * from Wallets");
            List<string> wallets = new List<string>();
            foreach(DataRow dr in dt.Rows)
            {
                wallets.Add(dr["name"].ToString());
            }
            Wallets.ItemsSource = wallets;
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            string wallet = (string)Wallets.SelectedItem;
            int wallet_id = SqlDB.GetId($"select * from Wallets where name='{wallet}'");
            DataTable dt = SqlDB.Select($"select * from Settings where user_id = {SqlDB.UserID}");
            if(dt.Rows.Count > 0)
            {
                if (SqlDB.Command($"update Settings set wallet_id = {wallet_id} where user_id = {SqlDB.UserID}"))
                {
                    MessageBox.Show("Успешно обновлено");
                }
            }
            else
            {
                if (SqlDB.Command($"insert into Settings values ({SqlDB.UserID}, {wallet_id})"))
                {
                    MessageBox.Show("Успешно добавлено");
                }
            }
        }
    }
}
