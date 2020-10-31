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

            if(uri.Contains(".config"))
            {
                ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap();
                configFileMap.ExeConfigFilename = txtUri.Text;
                List<DataGridModel> kvps = Convert.ConvertConfig(configFileMap);
                dataGridConfigValues.ItemsSource = kvps;
            }
            else if(uri.Contains(".json"))
            {
                using (StreamReader r = new StreamReader(txtUri.Text))
                {
                    string json = r.ReadToEnd();
                    List<KeyVaultModel> keyVaultValues = JsonConvert.DeserializeObject<List<KeyVaultModel>>(json);
                    List<DataGridModel> keyValues = new List<DataGridModel>();
                    foreach(KeyVaultModel item in keyVaultValues)
                    {
                        DataGridModel kvm = new DataGridModel();
                        kvm.key = item.secretName;
                        kvm.value = item.secretValue;
                        keyValues.Add(kvm);
                    }
                    dataGridConfigValues.ItemsSource = keyValues;
                }
            }
            else
            {

            }
        }

        private void btnExportJson_Click(object sender, RoutedEventArgs e)
        {
            List<KeyVaultModel> keyVaults = new List<KeyVaultModel>();
            foreach(DataGridModel keyPair in dataGridConfigValues.ItemsSource)
            {
                if(keyPair.isSelected == true)
                {
                    KeyVaultModel keyVault = new KeyVaultModel();
                    keyVault.secretName = keyPair.key;
                    keyVault.secretValue = keyPair.value;
                    keyVaults.Add(keyVault);
                }
            }

            if(keyVaults.Count > 0)
            {
                string json = Convert.ConvertDictionaryToJson(keyVaults);
                bool saveSuccessfully = Convert.SaveJson(json);
            }
            else
            {
                //TODO: handle no values being selected/checked.
            }
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
    }
}
