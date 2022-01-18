using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GraphicClient
{
    internal static class PasswordHelper
    {
        private readonly static byte[] entropy = {1, 0, 0, 0, 1};
        internal static string Encrypt(string plaintext)
        {
            byte[] ciphertext = ProtectedData.Protect(Encoding.UTF8.GetBytes(plaintext), entropy,
                DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(ciphertext);
        }

        internal static SecureString Decrypt(string ciphertext)
        {
            byte[] plaintext = ProtectedData.Unprotect(Convert.FromBase64String(ciphertext), entropy,
                DataProtectionScope.CurrentUser);
            string password = Encoding.UTF8.GetString(plaintext);
            SecureString ss = new SecureString();
            foreach (var character in password)
            {
               ss.AppendChar(character);
            }
            return ss;
        }
    }
}
