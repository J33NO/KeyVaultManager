using KeyVaultManager.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        List<DataGridModel> _dataGridValues;

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
                    _dataGridValues = new List<DataGridModel>();
                    _dataGridValues = KeyVault.LoadFile(uri, txtUri);
                    dataGridConfigValues.ItemsSource = _dataGridValues;
                    lblStatusMessage.Foreground = new SolidColorBrush(Colors.LightGreen);
                    lblStatusMessage.Content = "Loaded Values Successfully.";
                }
                catch (Exception ex)
                {
                    lblStatusMessage.Foreground = new SolidColorBrush(Colors.Red);
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
                        lblStatusMessage.Foreground = new SolidColorBrush(Colors.LightGreen);
                        lblStatusMessage.Content = "Exported Successfully.";
                    }
                    else
                    {
                        lblStatusMessage.Foreground = new SolidColorBrush(Colors.Red);
                        lblStatusMessage.Content = "Must select at least one value pair to export.";
                    }
                }
                else
                {
                    lblStatusMessage.Foreground = new SolidColorBrush(Colors.Red);
                    lblStatusMessage.Content = "No values to export.";
                }
            }
            catch (Exception ex)
            {
                lblStatusMessage.Foreground = new SolidColorBrush(Colors.Red);
                lblStatusMessage.Content = ex.Message;
            }
        }

        private void btnReplace_Click(object sender, RoutedEventArgs e)
        {
            int valuesReplaced = 0;
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
                            valuesReplaced += 1;
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
                lblStatusMessage.Foreground = new SolidColorBrush(Colors.LightGreen);
                lblStatusMessage.Content = valuesReplaced + " values replaced.";
            }
            catch (Exception ex)
            {
                lblStatusMessage.Foreground = new SolidColorBrush(Colors.Red);
                lblStatusMessage.Content = ex.Message;
            }

        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            string uri = KeyVault.BrowseFile();
            if(uri != "")
            {
                try
                {
                    List<DataGridModel> dataGridValues = KeyVault.LoadFile(uri, txtUri);
                    dataGridConfigValues.ItemsSource = dataGridValues;
                    lblStatusMessage.Content = "";
                }
                catch (Exception ex)
                {
                    lblStatusMessage.Foreground = new SolidColorBrush(Colors.Red);
                    lblStatusMessage.Content = ex.Message;
                }
            }
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
            txtFind.Text = "Find:";
            txtReplace.Text = "Replace:";
            lblStatusMessage.Content = "";
            chkSelectAll.IsChecked = false;
            dataGridConfigValues.ItemsSource = null;
            dataGridConfigValues.Items.Refresh();
        }

        private void chkSelectAll_Checked(object sender, RoutedEventArgs e)
        {
            if(dataGridConfigValues.Items.Count > 0)
            {
                foreach (DataGridModel item in dataGridConfigValues.ItemsSource)
                {
                    if (item.isSelected == false)
                    {
                        item.isSelected = true;
                    }
                    dataGridConfigValues.Items.Refresh();
                }
            }
        }

        private void chkSelectAll_Unchecked(object sender, RoutedEventArgs e)
        {
            if (dataGridConfigValues.Items.Count > 0)
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

        private void btnCopyJson_Click(object sender, RoutedEventArgs e)
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

                        KeyVault.QuickCopy(keyVaults);
                        lblStatusMessage.Foreground = new SolidColorBrush(Colors.LightGreen);
                        lblStatusMessage.Content = "Selected values copied to clipboard.";
                    }
                    else
                    {
                        lblStatusMessage.Foreground = new SolidColorBrush(Colors.Red);
                        lblStatusMessage.Content = "Must select at least one value pair to copy.";
                    }
                }
                else
                {
                    lblStatusMessage.Foreground = new SolidColorBrush(Colors.Red);
                    lblStatusMessage.Content = "No values to copy.";
                }
            }
            catch (Exception ex)
            {
                lblStatusMessage.Foreground = new SolidColorBrush(Colors.Red);
                lblStatusMessage.Content = ex.Message;
            }
        }

        private void txtFind_KeyUp(object sender, KeyEventArgs e)
        {
            //FindDataGridRow(dataGridConfigValues);
        }

        public void FindDataGridRow(DependencyObject obj)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DataGridRow lv = obj as DataGridRow;
                if (lv != null)
                {
                    HighlightText(lv);
                }
                FindDataGridRow(VisualTreeHelper.GetChild(obj as DependencyObject, i));
            }
        }

        private void HighlightText(Object itx)
        {
            if (itx != null)
            {
                if (itx is TextBlock)
                {
                    Regex regex = new Regex("(" + txtFind.Text + ")", RegexOptions.IgnoreCase);
                    TextBlock tb = itx as TextBlock;
                    if (txtFind.Text.Length == 0)
                    {
                        string str = tb.Text;
                        tb.Inlines.Clear();
                        tb.Inlines.Add(str);
                        return;
                    }
                    string[] substrings = regex.Split(tb.Text);
                    tb.Inlines.Clear();
                    foreach (var item in substrings)
                    {
                        if (regex.Match(item).Success)
                        {
                            Run runx = new Run(item);
                            runx.Background = Brushes.Yellow;
                            tb.Inlines.Add(runx);
                        }
                        else
                        {
                            tb.Inlines.Add(item);
                        }
                    }
                    return;
                }
                else
                {
                    for (int i = 0; i < VisualTreeHelper.GetChildrenCount(itx as DependencyObject); i++)
                    {
                        HighlightText(VisualTreeHelper.GetChild(itx as DependencyObject, i));
                    }
                }
            }
        }
    }
}
