using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthProvider.AESEncryption
{
    public interface ICustomAESEncryption
    {
        string Decrypt(byte[] cipherText);
        byte[] Encrypt(string plainText);

    }
}
