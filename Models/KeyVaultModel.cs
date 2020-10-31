using System;
using System.Collections.Generic;
using System.Text;

namespace KeyVaultManager.Models
{
    public class KeyVaultModel
    {
        public string secretName { get; set; }
        public string secretValue { get; set; }
        public  bool isSelected { get; set; }
    }

}
