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
        private int GetPreviousWallet()
        {
            DataTable setting = SqlDB.Select($"select * from Settings where user_id = {SqlDB.UserID}");
            return Convert.ToInt32(setting.Rows[0]["wallet_id"]);
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            string wallet = (string)Wallets.SelectedItem;
            int wallet_id = SqlDB.GetId($"select * from Wallets where name='{wallet}'");
            DataTable dt = SqlDB.Select($"select * from Settings where user_id = {SqlDB.UserID}");
            if(dt.Rows.Count > 0)
            {
                int previous_id = GetPreviousWallet();
                DataTable value = SqlDB.Select($"select value from Converter where first={previous_id} and second={wallet_id}");
                double coef = Convert.ToDouble(value.Rows[0]["value"]);
                DataTable newrows = SqlDB.Select($"select * from Categories_Money where user_id = {SqlDB.UserID}");
                foreach(DataRow dr in newrows.Rows)
                {
                    SqlDB.Command($"Update Categories_Money set [value] = {dr["value"]} * {coef} where id = {dr["id"]}");
                }
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
