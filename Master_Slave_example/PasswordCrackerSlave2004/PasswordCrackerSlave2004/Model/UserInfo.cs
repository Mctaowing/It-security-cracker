using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordCrackerSlave2004.Model
{
    [Serializable]
    class UserInfo
    {
        public string Username { get; set; }
        public string EntryptedPasswordBase64 { get; set; }
        public byte[] EntryptedPassword { get; set; }

        public UserInfo(string username, string entryptedPasswordBase64)
        {
            if (username == null)
            {
                throw new ArgumentNullException("username");
            }
            if (entryptedPasswordBase64 == null)
            {
                throw new ArgumentNullException("entryptedPasswordBase64");
            }
            Username = username;
            EntryptedPasswordBase64 = entryptedPasswordBase64;
            EntryptedPassword = Convert.FromBase64String(entryptedPasswordBase64);
        }

        public override string ToString()
        {
            return Username + ":" + EntryptedPasswordBase64;
        }
    }
}
