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
            if (uri != "")
            {
                try
                {
                    List<DataGridModel> dataGridValues = KeyVault.LoadFile(uri, txtUri);
                    dataGridConfigValues.ItemsSource = dataGridValues;
                    lblStatusMessage.Foreground = new SolidColorBrush(Colors.Green);
                    lblStatusMessage.Content = "Loaded Values Successfully.";
                }
                catch (Exception ex)
                {
                    lblStatusMessage.Content = ex.Message;
                }
            }
        }

        private void btnExportJson_Click(object sender, RoutedEventArgs e)
        {
            List<KeyVaultModel> keyVaults = new List<KeyVaultModel>();

            try
            {
                if (dataGridConfigValues.ItemsSource != null)
                {
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
                    if (keyVaults.Count > 0)
                    {
                        KeyVault.Export(keyVaults);
                        lblStatusMessage.Foreground = new SolidColorBrush(Colors.Green);
                        lblStatusMessage.Content = "Exported Successfully.";
                    }
                    else
                    {
                        lblStatusMessage.Content = "Must select at least one value pair to export.";
                    }
                }
                else
                {
                    lblStatusMessage.Content = "No values to export.";
                }
            }
            catch (Exception ex)
            {
                lblStatusMessage.Content = ex.Message;
            }
        }

        private void btnReplace_Click(object sender, RoutedEventArgs e)
        {
            string findValue = txtFind.Text;
            string replaceValue = txtReplace.Text;
            List<DataGridModel> newValues = new List<DataGridModel>();

            try
            {
                foreach (DataGridModel dgm in dataGridConfigValues.ItemsSource)
                {
                    if (dgm.isSelected == true)
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
                lblStatusMessage.Content = "";
            }
            catch (Exception ex)
            {
                lblStatusMessage.Content = ex.Message;
            }

        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            string uri = KeyVault.BrowseFile();
            List<DataGridModel> dataGridValues = KeyVault.LoadFile(uri, txtUri);
            dataGridConfigValues.ItemsSource = dataGridValues;
            lblStatusMessage.Content = "";
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
            lblStatusMessage.Content = "";
            dataGridConfigValues.ItemsSource = null;
            dataGridConfigValues.Items.Refresh();
        }

        private void chkSelectAll_Checked(object sender, RoutedEventArgs e)
        {
            foreach(DataGridModel item in dataGridConfigValues.ItemsSource)
            {
                if(item.isSelected == false)
                {
                    item.isSelected = true;
                }
                dataGridConfigValues.Items.Refresh();
            }
        }

        private void chkSelectAll_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (DataGridModel item in dataGridConfigValues.ItemsSource)
            {
                if (item.isSelected == true)
                {
                    item.isSelected = false;
                }
                dataGridConfigValues.Items.Refresh();
            }
        }
    }
}
