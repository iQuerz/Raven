using App;
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

namespace App
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

            #region WindowEvents
            Loaded += App_OnLoad;
            Closing += App_Closing;
            exitButton.Click += ExitButton_Click;
            #endregion

            #region AutoSave Timer Setup
            DispatcherTimer autoSaveTimer = new DispatcherTimer();
            autoSaveTimer.Interval = TimeSpan.FromMinutes(AppSettings.AutoSavePeriod);
            autoSaveTimer.Tick += AutoSaveTimer_Tick;
            autoSaveTimer.Start();
            #endregion

            #region GreetingTimer
            DispatcherTimer greetingTimer = new DispatcherTimer();
            greetingTimer.Interval = TimeSpan.FromMinutes(30);
            greetingTimer.Tick += GreetingTimer_Tick;
            greetingTimer.Start();
            #endregion

            #region TransactionsScroll
            TransactionsList.PreviewMouseWheel += TransactionsList_Scroll;
            TransactionsList.MouseWheel += TransactionsList_Scroll;
            #endregion

        }

        private void App_OnLoad(object sender, RoutedEventArgs e)
        {
            AppSettings.FirstBoot = true;
            #region FirstBoot
            if (AppSettings.FirstBoot)
            {
                // i doubt we need this here
                AppSettings.FontSize = FontSize;

                FirstBootSetup firstBoot = new FirstBootSetup(this);
                firstBoot.Owner = this;
                Hide();
                firstBoot.ShowDialog();

                AppSettings.FirstBoot = false;
            }
            #endregion

            #region Font Size stuff

            GreetingLabel.FontSize = AppSettings.FontSize * 4;
            SearchBox.FontSize = AppSettings.FontSize * 2;
            TransactionsList.FontSize = AppSettings.FontSize * 2;

            #endregion

            #region Greeting Label

            GreetingLabel.Content = GetGreetingMessage();

            _manager.loadTransactions();
            BalanceLabel.Content = _manager.BalanceString + " " + AppSettings.DefaultCurrency;

            #endregion

            #region Transactions

            foreach (Transaction t in _manager.GetTransactions(8))
            {
                TransactionsList.Items.Add(t._Description + ": " + t._Value + AppSettings.DefaultCurrency);
            }

            #endregion

        }
        private void App_Closing(object sender, EventArgs e)
        {
            // Save data before exiting
            _manager.saveTransactions();
        }

        private string GetGreetingMessage()
        {
            int h = DateTime.Now.Hour;
            if (h >= 5 && h < 12)
                return "Good morning, " + AppSettings.Username;
            if (h >= 12 && h < 18)
                return "Good afternoon, " + AppSettings.Username;
            return "Good evening, " + AppSettings.Username;
        }
    }
}