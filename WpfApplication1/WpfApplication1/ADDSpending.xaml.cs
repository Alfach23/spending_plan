using System;
using System.Data;
using System.Data.SQLite;
using System.Windows;

namespace WpfApplication1
{
    /// <summary>
    /// Логика взаимодействия для ADDSpending.xaml
    /// </summary>
    public partial class ADDSpending : Window
    {
        public MainWindow per;
        public YourSpend sp;
        
        public ADDSpending(MainWindow _per)
        {
            InitializeComponent();
            per = _per;
            cbCtgrs.ItemsSource = per.cbCtgrs.ItemsSource;
            calen.SelectedDate = DateTime.Now;
            txtSumRub.Text = "0";
            txtSumKop.Text = "00";
            btmResult.Content = "Добавить";
        }
        public ADDSpending(MainWindow _per, YourSpend _ys)
        {
            InitializeComponent();
            per = _per;
            cbCtgrs.ItemsSource = per.cbCtgrs.ItemsSource;
            NameSpend.Text = _ys.NameSpen;
            calen.SelectedDate = _ys.DateSpen;

            int sumRK = _ys.SumSpen;
            int roubles = sumRK / 100;
            int cents = sumRK - roubles * 100;
            txtSumRub.Text = Convert.ToString(roubles);
            txtSumKop.Text = Convert.ToString(cents);

            sp = _ys;
            for (int i = 0; i < cbCtgrs.Items.Count; i++)
            {
                var uuu = (MyCategories)cbCtgrs.Items[i];
                if (uuu.IdCtgrs == sp.MCtgrs.IdCtgrs)
                {
                    cbCtgrs.SelectedIndex = i;
                    break;
                }
            }
            btmResult.Content = "Редактировать";
        }
        private void ADD_Click(object sender, RoutedEventArgs e)
        {
            string strNamesp =Convert.ToString(NameSpend.Text);

            string strSum1 = Convert.ToString(txtSumRub.Text + txtSumKop.Text);
            int intSum2 = Convert.ToInt32(strSum1);

            SQLiteCommand dob = new SQLiteCommand();
            dob.CommandType = CommandType.Text;
            if (sp == null)
            {
                dob.CommandText =
                    "Insert into t_spending (Id_ctgrsS, NameSpen, SumSpen, DateSpen)  values ('" +
                    ((MyCategories)cbCtgrs.SelectedItem).IdCtgrs + "', '" +
                    strNamesp + "', '" +
                    intSum2 + "', '" +
                    calen.SelectedDate.GetValueOrDefault().ToString("yyyy-MM-dd") + "')";
                //MessageBox.Show(dob.CommandText);
            }
            else
            {
                dob.CommandText = "Update t_spending set Id_ctgrsS = '" + ((MyCategories)cbCtgrs.SelectedItem).IdCtgrs +
                                  "', NameSpen = '" + strNamesp + "', SumSpen = '" +
                                  intSum2 + "',  DateSpen = '" +
                                  calen.SelectedDate.GetValueOrDefault().ToString("yyyy-MM-dd") + "' where IdSpending =  '" + per.iz + "'";
                //MessageBox.Show(dob.CommandText);
            }
            dob.Connection = per.sqlCon;

            per.sqlCon.Open();
            int m = dob.ExecuteNonQuery();
            per.sqlCon.Close();
            this.Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.Close();
            Window form = new MainWindow();
            form.Show();
        }
    }
}
