using System.Security.Cryptography;
using System.Text;

namespace KayordKit.Core;

public static class Util
{
    // TODO: Remove this key and add to config
    private static readonly string _key = "qG8mtop56*vDy#bk*43Z!SBcDsZBeF";

    public static string Decrypt(string stringToDecrypt)
    {
        var md5Provider = MD5.Create();
        byte[] encryptionKeyArray = md5Provider.ComputeHash(Encoding.UTF8.GetBytes(_key));
        md5Provider.Clear();

        byte[] stringToDecryptArray = Convert.FromBase64String(stringToDecrypt);

        var desProvider = TripleDES.Create();
        desProvider.Key = encryptionKeyArray;
        desProvider.Mode = CipherMode.ECB;
        desProvider.Padding = PaddingMode.PKCS7;
        ICryptoTransform cryptoTransform = desProvider.CreateDecryptor();
        byte[] resultArray = cryptoTransform.TransformFinalBlock(stringToDecryptArray, 0, stringToDecryptArray.Length);
        desProvider.Clear();
        string decryptedString = Encoding.UTF8.GetString(resultArray);
        return decryptedString;
    }

    public static string Encrypt(string stringToEncrypt)
    {
        var md5Provider = MD5.Create();
        byte[] encryptionKeyArray = md5Provider.ComputeHash(Encoding.UTF8.GetBytes(_key));
        md5Provider.Clear();

        byte[] stringToEncryptArray = UTF8Encoding.UTF8.GetBytes(stringToEncrypt);

        var desProvider = TripleDES.Create();
        desProvider.Key = encryptionKeyArray;
        desProvider.Mode = CipherMode.ECB;
        desProvider.Padding = PaddingMode.PKCS7;

        ICryptoTransform cryptoTransform = desProvider.CreateEncryptor();
        byte[] resultArray = cryptoTransform.TransformFinalBlock(stringToEncryptArray, 0, stringToEncryptArray.Length);

        desProvider.Clear();

        string encryptedString = Convert.ToBase64String(resultArray, 0, resultArray.Length);
        return encryptedString;
    }
}