using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GraphicClient
{
    internal class ConfigModel
    {
        public string Username { get; set; }
        public SecureString Password { internal get; set; }
        public string Ip { get; set; }
        [JsonConstructor]
        internal ConfigModel(string username, string password, string Ip)
        {
            this.Username = username;
            this.Password = String.IsNullOrEmpty(password) ? null : PasswordHelper.Decrypt(password);
            this.Ip = Ip;
        }
        internal ConfigModel()
        {
            this.Username = "";
            this.Password = new SecureString();
            this.Ip = "";
        }

        internal void SaveConfig(string path)
        {
            string json = JsonConvert.SerializeObject(this);
            File.WriteAllText(path, json);
        }

        internal static ConfigModel LoadConfig(string path)
        {
            if (!File.Exists(path))
            {
                return null;
            }
            string config = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<ConfigModel>(config);
        }
    }
}
