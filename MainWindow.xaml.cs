using KeyVaultManager.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
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

        private void btnConfigUri_Click(object sender, RoutedEventArgs e)
        {
            ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap();
            configFileMap.ExeConfigFilename = txtConfigUri.Text;
            List<KeyVaultModel> kvps = Convert.ConvertXmlToDictionary(configFileMap);
            dataGridConfigValues.ItemsSource = kvps;
        }

        private void btnExportJson_Click(object sender, RoutedEventArgs e)
        {
            List<KeyVaultModel> kvps = new List<KeyVaultModel>();
            foreach(KeyVaultModel kvm in dataGridConfigValues.ItemsSource)
            {
                if(kvm.isSelected == true)
                {
                    kvps.Add(kvm);
                }
            }

            string keyVaultJson = Convert.ConvertDictionaryToJson(kvps);
        }
    }
}
