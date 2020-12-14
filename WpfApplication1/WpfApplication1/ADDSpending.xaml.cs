using System.Windows;

namespace WpfApplication1
{
    /// <summary>
    /// Логика взаимодействия для ADDSpending.xaml
    /// </summary>
    public partial class ADDSpending : Window
    {
        public ADDSpending(MainWindow mainWindow)
        {
            InitializeComponent();
        }
        private void ADD_Click(object sender, RoutedEventArgs e)
        {
            //int s = Convert.ToInt32(TEXTbox1.Text);
            //int roubles = s / 100;
            //int cents = s - roubles * 100;
            //MessageBox.Show(string.Format(@"{0} руб. {1} коп.", roubles, cents));
        }
    }
}
