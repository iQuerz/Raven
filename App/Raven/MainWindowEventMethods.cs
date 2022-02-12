using System;
using System.Windows;
using System.Windows.Input;

namespace App
{
    public partial class MainWindow : Window
    {

        #region Timers

        private void AutoSaveTimer_Tick(object sender, EventArgs e)
        {
            _manager.saveTransactions();
        }

        private void GreetingTimer_Tick(object sender, EventArgs e)
        {
            GreetingLabel.Content = GetGreetingMessage();
        }

        #endregion

        #region Buttons

        private void ExitButton_Click(object sender, EventArgs e)
        {
            _manager.saveTransactions();
            Close();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.Show();
        }

        private void newTransactionButton_Click(object sender, RoutedEventArgs e)
        {
            NewTransactionWindow newTransactionWindow = new NewTransactionWindow();
            newTransactionWindow.Show();
        }

        #endregion

        #region Other

        private void TransactionsList_Scroll(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer.ScrollToVerticalOffset(ScrollViewer.VerticalOffset - e.Delta);
        }

        #endregion

    }
}
