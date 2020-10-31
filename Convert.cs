using KeyVaultManager.Models;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;

namespace KeyVaultManager
{
    public class Convert
    {

        public static List<KeyValueModel> ConvertConfig(ExeConfigurationFileMap xml)
        {
            List<KeyValueModel> kvp = new List<KeyValueModel>();
            Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(xml, ConfigurationUserLevel.None);
            KeyValueConfigurationCollection settings = configuration.AppSettings.Settings;
            foreach (KeyValueConfigurationElement item in settings)
            {
                KeyValueModel xmlValues = new KeyValueModel();
                xmlValues.key = item.Key.ToString();
                xmlValues.value = item.Value.ToString();
                xmlValues.isSelected = false;
                kvp.Add(xmlValues);
            }
            return kvp;
        }

        public static string ConvertDictionaryToJson(List<KeyVaultModel> kvms)
        {
            try
            {
                string json = JsonConvert.SerializeObject(kvms, Newtonsoft.Json.Formatting.Indented);
                return json;


            }
            catch (Exception e)
            {
                throw;
            }
        }

        public static bool SaveJson(string json)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Title = "Json Export";
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.CheckFileExists = false;
                saveFileDialog.CheckPathExists = false;
                saveFileDialog.Filter = "All files (*.*)|*.*";
                saveFileDialog.DefaultExt = ".json";
                if (saveFileDialog.ShowDialog() == true)
                {
                    File.WriteAllText(saveFileDialog.FileName, json);
                    return true;
                }
                else return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
