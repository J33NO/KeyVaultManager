using Hangfire.Annotations;
using KeyVaultManager.Models;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace KeyVaultManager
{
    public class KeyVault
    {
        public static List<DataGridModel> ConvertConfig(ExeConfigurationFileMap xml)
        {
            List<DataGridModel> keyValues = new List<DataGridModel>();
            Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(xml, ConfigurationUserLevel.None);
            KeyValueConfigurationCollection settings = configuration.AppSettings.Settings;
            foreach (KeyValueConfigurationElement item in settings)
            {
                DataGridModel xmlValues = new DataGridModel();
                xmlValues.key = item.Key.ToString();
                xmlValues.value = item.Value.ToString();
                xmlValues.isSelected = false;
                keyValues.Add(xmlValues);
            }
            return keyValues;
        }

        public static string ConvertDictionaryToJson(List<KeyVaultModel> kvms)
        {
            try
            {
                string json = JsonConvert.SerializeObject(kvms, Newtonsoft.Json.Formatting.Indented);
                return json;
            }
            catch (Exception ex)
            {
                throw ex;
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

        public static bool IsValidJson(string strInput)
        {
            if (string.IsNullOrWhiteSpace(strInput)) { return false; }
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
                (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = JToken.Parse(strInput);
                    return true;
                }
                catch (JsonReaderException jex)
                {
                    //Exception in parsing json
                    Console.WriteLine(jex.Message);
                    return false;
                }
                catch (Exception ex) //some other exception
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static string BrowseFile()
        {
            string filename = "";
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "All files (*.*)|*.*";
            dlg.DefaultExt = ".json";
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                filename = dlg.FileName;
            }
            return filename;
        }

        public static List<DataGridModel> LoadFile(string uri, TextBox txtUri)
        {
            try
            {
                if (uri.Contains(".config"))
                {
                    ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap();
                    configFileMap.ExeConfigFilename = txtUri.Text;
                    List<DataGridModel> keyValues = KeyVault.ConvertConfig(configFileMap);
                    return keyValues;
                }
                else if (uri.Contains(".json") || uri.Contains(".txt"))
                {
                    using (StreamReader r = new StreamReader(uri))
                    {
                        List<DataGridModel> keyValues = new List<DataGridModel>();
                        string json = r.ReadToEnd();
                        bool validJson = KeyVault.IsValidJson(txtUri.Text);
                        if (validJson)
                        {
                            List<KeyVaultModel> keyVaultValues = JsonConvert.DeserializeObject<List<KeyVaultModel>>(json);
                            foreach (KeyVaultModel item in keyVaultValues)
                            {
                                DataGridModel kvm = new DataGridModel();
                                kvm.key = item.secretName;
                                kvm.value = item.secretValue;
                                keyValues.Add(kvm);
                            }
                        }
                        else
                        {
                            throw new Exception("Invalid format");
                        }
                        return keyValues;
                    }
                }
                else //if not .config file, check to see if uri pasted is json.
                {
                    List<DataGridModel> keyValues = new List<DataGridModel>();
                    bool validJson = KeyVault.IsValidJson(txtUri.Text);
                    if (validJson)
                    {
                        List<KeyVaultModel> keyVaultValues = JsonConvert.DeserializeObject<List<KeyVaultModel>>(txtUri.Text);
                        foreach (KeyVaultModel item in keyVaultValues)
                        {
                            DataGridModel kvm = new DataGridModel();
                            kvm.key = item.secretName;
                            kvm.value = item.secretValue;
                            keyValues.Add(kvm);
                        }
                    }
                    else
                    {
                        throw new Exception("Invalid format");
                    }
                    return keyValues;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Export(List<KeyVaultModel> keyVaults)
        {
            try
            {
                string json = KeyVault.ConvertDictionaryToJson(keyVaults);
                bool saveSuccessfully = KeyVault.SaveJson(json);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void QuickCopy(List<KeyVaultModel> keyVaults)
        {
            try
            {
                string json = KeyVault.ConvertDictionaryToJson(keyVaults);
                Clipboard.SetText(json);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
