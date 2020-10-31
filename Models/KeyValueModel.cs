using System;
using System.Collections.Generic;
using System.Text;

namespace KeyVaultManager.Models
{
    public class KeyValueModel
    {
        public string key { get; set; }
        public string value { get; set; }
        public  bool isSelected { get; set; }
    }

}
