using KeyVaultManager.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
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

namespace KeyVaultManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            string uri = txtUri.Text.ToLower();
            List<DataGridModel> dataGridValues = KeyVault.LoadFile(uri, txtUri);
            dataGridConfigValues.ItemsSource = dataGridValues;
        }

        private void btnExportJson_Click(object sender, RoutedEventArgs e)
        {
            List<KeyVaultModel> keyVaults = new List<KeyVaultModel>();
            foreach (DataGridModel keyPair in dataGridConfigValues.ItemsSource)
            {
                if (keyPair.isSelected == true)
                {
                    KeyVaultModel keyVault = new KeyVaultModel();
                    keyVault.secretName = keyPair.key;
                    keyVault.secretValue = keyPair.value;
                    keyVaults.Add(keyVault);
                }
            }

            KeyVault.Export(keyVaults);
        }

        private void btnReplace_Click(object sender, RoutedEventArgs e)
        {
            string findValue = txtFind.Text;
            string replaceValue = txtReplace.Text;
            List<DataGridModel> newValues = new List<DataGridModel>();
            foreach(DataGridModel dgm in dataGridConfigValues.ItemsSource)
            {
                if(dgm.isSelected == true)
                {
                    if (dgm.value.Contains(findValue))
                    {
                        dgm.value = dgm.value.Replace(findValue, replaceValue);
                        DataGridModel newDataGridModel = new DataGridModel();
                        newDataGridModel = dgm;
                        newValues.Add(newDataGridModel);
                    }
                    else
                    {
                        newValues.Add(dgm);
                    }
                }
                else
                {
                    newValues.Add(dgm);
                }
            }
            dataGridConfigValues.ItemsSource = newValues;
            dataGridConfigValues.Items.Refresh();
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            string uri = KeyVault.BrowseFile();
            List<DataGridModel> dataGridValues = KeyVault.LoadFile(uri, txtUri);
            dataGridConfigValues.ItemsSource = dataGridValues;
        }

        private void FindTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.Text = string.Empty;
            tb.GotFocus -= FindTextBox_GotFocus;
        }

        private void ReplaceTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.Text = string.Empty;
            tb.GotFocus -= ReplaceTextBox_GotFocus;
        }

        private void FindTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text == "")
            {
                tb.Text = "Find:";
            }
        }

        private void ReplaceTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text == "")
            {
                tb.Text = "Replace:";
            }
        }

        private void btnClearAll_Click(object sender, RoutedEventArgs e)
        {
            txtUri.Text = "";
            txtFind.Text = "";
            txtReplace.Text = "";
            dataGridConfigValues.ItemsSource = null;
            dataGridConfigValues.Items.Refresh();
        }
    }
}
