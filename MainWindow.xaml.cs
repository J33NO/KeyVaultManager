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
                List<KeyValueModel> kvps = Convert.ConvertConfig(configFileMap);
                dataGridConfigValues.ItemsSource = kvps;
            }
            else if(uri.Contains(".json"))
            {
                using (StreamReader r = new StreamReader(txtUri.Text))
                {
                    string json = r.ReadToEnd();
                    List<KeyVaultModel> keyVaultValues = JsonConvert.DeserializeObject<List<KeyVaultModel>>(json);
                    List<KeyValueModel> keyValues = new List<KeyValueModel>();
                    foreach(KeyVaultModel item in keyVaultValues)
                    {
                        KeyValueModel kvm = new KeyValueModel();
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
            foreach(KeyValueModel keyPair in dataGridConfigValues.ItemsSource)
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
    }
}
