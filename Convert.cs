using KeyVaultManager.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace KeyVaultManager
{
    public class Convert
    {

        public static List<KeyVaultModel> ConvertXmlToDictionary(ExeConfigurationFileMap xml)
        {
            List<KeyVaultModel> kvp = new List<KeyVaultModel>();
            Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(xml, ConfigurationUserLevel.None);
            KeyValueConfigurationCollection settings = configuration.AppSettings.Settings;
            foreach (KeyValueConfigurationElement item in settings)
            {
                KeyVaultModel xmlValues = new KeyVaultModel();
                xmlValues.secretName = item.Key.ToString();
                xmlValues.secretValue = item.Value.ToString();
                xmlValues.isSelected = false;
                kvp.Add(xmlValues);
            }
            return kvp;
        }

        public static string ConvertDictionaryToJson(List<KeyVaultModel> kvps)
        {
            //List<KeyVaultModel> keyVaultValues = new List<KeyVaultModel>();
            //foreach (KeyVaultModel kvm in kvps)
            //{
            //    KeyVaultModel kvp = new KeyVaultModel();
            //    kvp.secretName = kvm.secretName;
            //    kvp.secretValue = kvm.secretValue;
            //    keyVaultValues.Add(kvp);
            //}
            string json = JsonConvert.SerializeObject(kvps, Newtonsoft.Json.Formatting.Indented);
            return json;
        }
    }
}
