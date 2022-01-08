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
        public FirstBootSetup()
        {
            InitializeComponent();
            Loaded += Window_OnLoad;
            FontSizeInput.ValueChanged += FontSize_Changed;
        }
        private void Window_OnLoad(object sender, EventArgs e)
        {
            
        }
        private void FontSize_Changed(object sender, EventArgs e)
        {

        }
    }
}
