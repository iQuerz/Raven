using App.Business;
using Raven;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace App
{
    /// <summary>
    /// Interaction logic for FirstBootSetup.xaml
    /// </summary>
    public partial class FirstBootSetup : Window
    {
        // TODO: Needed custom Menu bar for first boot form.
        MainWindow _mainWindow;
        public FirstBootSetup(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            InitializeComponent();

            Loaded += Window_OnLoad;
            Closing += Window_Closing;

            FontSizeInput.ValueChanged += FontSize_Changed;
            saveChangesButton.Click += SaveChanges_Click;
        }
        private void Window_OnLoad(object sender, EventArgs e)
        {
            FontSizeInput.Value = FontSize;

            LanguageInput.Items.Add("English - ENG");
            LanguageInput.Items.Add("Srpski - SRB");
            LanguageInput.Items.Add("Српски - СРБ");
            LanguageInput.SelectedIndex = 0;

            CurrencyInput.Items.Add("RSD");
            CurrencyInput.Items.Add("EUR");
            CurrencyInput.Items.Add("USD");
            CurrencyInput.SelectedIndex = 0;

            DarkModeInput.Items.Add("Yes");
            DarkModeInput.Items.Add("No");
            DarkModeInput.SelectedIndex = 0;
        }
        private void Window_Closing(object sender, EventArgs e)
        {
            _mainWindow.Show();
        }
        private void FontSize_Changed(object sender, EventArgs e)
        {
            FontSize = FontSizeInput.Value;
            GreetingLabel.FontSize = FontSize * 2;
            saveChangesButton.FontSize = FontSize * 2;
        }
        private void SaveChanges_Click(object sender, EventArgs e)
        {
            AppSettings.Username = UsernameInput.Text;

            AppSettings.DefaultLanguage = LanguageInput.Text == "English - ENG" ? "ENG" :
                LanguageInput.Text == "Srpski - SRB" ? "SRB" : "СРБ";

            AppSettings.FontSize = FontSizeInput.Value;

            AppSettings.DefaultCurrency = CurrencyInput.Text;

            AppSettings.DarkMode = DarkModeInput.Text == "Yes" ? true : false;

            Close();
        }
    }
}