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
using Excel = Microsoft.Office.Interop.Excel;

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
            int opeartion_id = SqlDB.GetId("select * from Operations where [name]='Расход'");
            DataTable dt2 = SqlDB.Select($"select Sum([value]) as [value] from Categories_Money " +
                $"join Categories on Categories_Money.category_id = Categories.id " +
                $"where user_id = {SqlDB.UserID} and operation={opeartion_id}");
            if (dt2.Rows.Count > 0)
            {
                string balance = dt2.Rows[0]["value"].ToString();
                Balance.Content = "Общие расходы: " + balance;
            }
            else
            {
                Balance.Content = "Общие расходы: ";
            }
            int opeartion_id1 = SqlDB.GetId("select * from Operations where [name]='Доход'");
            DataTable dt3 = SqlDB.Select($"select Sum([value]) as [value] from Categories_Money " +
                $"join Categories on Categories_Money.category_id = Categories.id " +
                $"where user_id = {SqlDB.UserID} and operation={opeartion_id1}");
            if (dt3.Rows.Count > 0)
            {
                string balance = dt3.Rows[0]["value"].ToString();
                Plus.Content = "Общие доходы: " + balance;
            }
            else
            {
                Plus.Content = "Общие доходы: ";
            }
        }
        public void SetTable()
        {
            DataTable dt = SqlDB.Select($"select Categories_Money.id, Categories.name as category, Categories_Money.value as value, Operations.name as operation, Categories_Money.date from Categories_Money " +
                $"join Categories on Categories_Money.category_id = Categories.id " +
                $"join Operations on Categories.operation = Operations.id where user_id={SqlDB.UserID}");
            List<CategoryValue> categoryValues = new List<CategoryValue>();
            foreach(DataRow dr in dt.Rows)
            {
                categoryValues.Add(new CategoryValue
                {
                    ID = dr["id"].ToString(),
                    Category = dr["category"].ToString(),
                    Value = dr["value"].ToString(),
                    Operation = dr["operation"].ToString(),
                    Date = dr["date"].ToString().Substring(0, 10)
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

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if(DeleteID.Text.Length > 0)
            {
                if (SqlDB.Command($"delete from Categories_Money where id = {DeleteID.Text}"))
                {
                    MessageBox.Show("Успешно удалено");
                    SetWalletAndBalance();
                    SetTable();
                }
            }
            else
            {
                MessageBox.Show("Введите ID");
            }
        }

        private void Excel_Click(object sender, RoutedEventArgs e)
        {
            Excel.Application ExcelApp = new Excel.Application();
            ExcelApp.Application.Workbooks.Add(Type.Missing);
            ExcelApp.Columns.ColumnWidth = 15;

            ExcelApp.Cells[1, 1] = "Категория";
            ExcelApp.Cells[1, 2] = "Значение";
            ExcelApp.Cells[1, 3] = "Операция";

            var list = Table.Items.OfType<CategoryValue>().ToList();

            for (int j = 0; j < list.Count; j++)
            {
                ExcelApp.Cells[j + 2, 1] = list[j].Category;
                ExcelApp.Cells[j + 2, 2] = list[j].Value;
                ExcelApp.Cells[j + 2, 3] = list[j].Operation;
            }
            ExcelApp.Visible = true;
        }
    }
}
