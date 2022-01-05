using App.Business;
using App.Models.Base;
using System;
using System.Collections.Generic;
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
using System.Windows.Threading;

namespace Raven
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Manager _manager;
        public MainWindow()
        {
            InitializeComponent();
            _manager = new Manager();
            Loaded += App_OnLoad;

            #region AutoSave Timer Setup
            DispatcherTimer autoSaveTimer = new DispatcherTimer();
            autoSaveTimer.Interval = TimeSpan.FromMinutes(AppSettings.AutoSavePeriod);
            autoSaveTimer.Tick += AutoSaveTimer_Tick;
            autoSaveTimer.Start();
            #endregion
        }

        private void App_OnLoad(object sender, RoutedEventArgs e)
        {
            if (AppSettings.FirstBoot)
            {
                AppSettings.FontSize = FontSize;
                //open first boot menu
                AppSettings.FirstBoot = false;
            }
            GreetingLabel.Content = "Good evening, " + AppSettings.Username;

            _manager.loadTransactions();
            BalanceLabel.Content = _manager.BalanceString + " " + AppSettings.DefaultCurrency;

            foreach(Transaction t in _manager.GetTransactions(3))
            {
                TransactionsList.Items.Add(t._Description + ": " + t._Value + AppSettings.DefaultCurrency);
            }
        }
        private void AutoSaveTimer_Tick(object sender, EventArgs e)
        {
            _manager.saveTransactions();
        }
    }
}