using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;

namespace WpfApplication1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public SQLiteConnection sqlCon = new SQLiteConnection("Data Source=DBspending.db; Version=3");
        public string iz;
        public MainWindow()
        {
            InitializeComponent();
            const string sql = "SELECT IdCtgrs, NameCtgrs FROM t_categories";
            SQLiteDataAdapter sqlDA = new SQLiteDataAdapter(sql, sqlCon);
            DataTable datMain = new DataTable();
            try
            {
                sqlDA.Fill(datMain);
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }

            List<MyCategories> ctgrList = new List<MyCategories>();
            ctgrList.Add(new MyCategories() { NameCtgrs = "" });
            for (int i = 0; i < datMain.Rows.Count; i++)
            {
                MyCategories temp = new MyCategories
                {
                    IdCtgrs = Convert.ToInt32(datMain.Rows[i]["IdCtgrs"]),
                    NameCtgrs = datMain.Rows[i]["NameCtgrs"].ToString()
                };
                ctgrList.Add(temp);
            }
            cbCtgrs.ItemsSource = ctgrList;

            SEARCH_Click(null, null);
        }
        private void SEARCH_Click(object sender, RoutedEventArgs e)
        {
            string sqlDB = @"SELECT t_spending.IdSpending, t_spending.Id_ctgrsS, t_spending.NameSpen, t_spending.DateSpen, t_spending.SumSpen, t_categories.IdCtgrs, t_categories.NameCtgrs 
                            FROM    t_spending, t_categories 
                            WHERE   t_spending.Id_ctgrsS = t_categories.IdCtgrs ";
            //загрузка данных
            SQLiteDataAdapter sqlDA = new SQLiteDataAdapter(sqlDB, sqlCon);
            DataTable datMain = new DataTable();
            try
            {
                sqlDA.Fill(datMain);
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
            //загрузка данных по критериям
            if (txtNameS.Text!="")
            {
                string nameSp = Convert.ToString(txtNameS.Text);
                sqlDB += string.Format("AND t_spending.NameSpen='{0}'",nameSp);
            }
            if (cbCtgrs.SelectedIndex > 0)
            {
                int cbox1 = ((MyCategories)cbCtgrs.SelectedItem).IdCtgrs;
                sqlDB += " AND t_categories.IdCtgrs=" + cbox1;
            }
            if (calen.SelectedDate != null)
            {
                sqlDB += string.Format("AND t_spending.DateSpen='{0}'",
                    calen.SelectedDate.GetValueOrDefault().ToString("yyyy-MM-dd"));
            }
            sqlDA = new SQLiteDataAdapter(sqlDB, sqlCon);
            datMain = new DataTable();
            try
            {
                sqlDA.Fill(datMain);
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
            List<YourSpend> listSpends = new List<YourSpend>();
            for (int i = 0; i < datMain.Rows.Count; i++)
            {
                YourSpend temp = new YourSpend();
                temp.IdSpending = Convert.ToInt32(datMain.Rows[i]["IdSpending"]);
                temp.NameSpen = datMain.Rows[i]["NameSpen"].ToString();
                temp.SumSpen = Convert.ToInt32(datMain.Rows[i]["SumSpen"]);
                temp.DateSpen = Convert.ToDateTime(datMain.Rows[i]["DateSpen"]);
                temp.MCtgrs = new MyCategories();
                temp.MCtgrs.IdCtgrs = Convert.ToInt32(datMain.Rows[i]["IdCtgrs"]);
                temp.MCtgrs.NameCtgrs = datMain.Rows[i]["NameCtgrs"].ToString();
                listSpends.Add(temp);
            }
            //заполняем DataGrid данными
            DataCenter.ItemsSource = null;
            DataCenter.ItemsSource = listSpends;
        }
        private void AddWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            ADDSpending form = new ADDSpending(this);
            form.ShowDialog();
            SEARCH_Click(null, null);
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            string k = ((Button)sender).Tag.ToString();
            SQLiteCommand del = new SQLiteCommand();
            del.CommandType = CommandType.Text;
            del.CommandText = "DELETE FROM t_spending WHERE IdSpending =  " + k;
            del.Connection = sqlCon;

            sqlCon.Open();
            int m = del.ExecuteNonQuery();
            sqlCon.Close();

            SEARCH_Click(null, null);
        }
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            iz = ((Button)sender).Tag.ToString();
            for (int i = 0; i < DataCenter.Items.Count; i++)
            {
                var uuu = (YourSpend)DataCenter.Items[i];
                if (uuu.IdSpending.ToString() == iz)
                {
                    ADDSpending form = new ADDSpending(this, uuu);
                    form.ShowDialog();
                    SEARCH_Click(null, null);
                    break;
                }
            }
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            txtNameS.Text = "";
            cbCtgrs.SelectedIndex = -1;
            calen.SelectedDate = null;
            SEARCH_Click(null, null);
        }
    }

    public class MyCategories
    {
        public int IdCtgrs { get; set; }
        public string NameCtgrs { get; set; }
    }

    public class YourSpend
    {
        public int IdSpending { get; set; }
        public string NameSpen { get; set; }
        public MyCategories MCtgrs { get; set; }
        public DateTime DateSpen { get; set; }
        public int SumSpen { get; set; }
        
    }
}
