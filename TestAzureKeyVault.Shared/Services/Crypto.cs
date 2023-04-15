using System.Text;

namespace TestAzureKeyVault.Shared.Services;
public class Crypto : ICrypto
{
    private string InternalEncrypt(string text, string key)
    {
        string text2 = XORCipher(text, key);
        byte[] bytes = Encoding.UTF8.GetBytes(new string(text2));
        return Convert.ToBase64String(bytes);
    }

    private string InternalDecrypt(string text, string key)
    {
        byte[] xorBytes = Convert.FromBase64String(text);
        string xor = Encoding.UTF8.GetString(xorBytes);
        return XORCipher(xor, key);
    }

    private string XORCipher(string data, string key)
    {
        int length = data.Length;
        int length2 = key.Length;
        char[] array = new char[length];
        for (int i = 0; i < length; i++)
        {
            array[i] = (char)(data[i] ^ key[i % length2]);
        }

        return new string(array);
    }

    public string EncryptByKeyInternal(string text, string key)
    {
        if (string.IsNullOrEmpty(text))
            return null;

        var v = InternalEncrypt(text, key);
        return v;
    }

    public string DecryptByKeyInternal(string text, string key)
    {
        if (string.IsNullOrEmpty(text))
            return null;

        string result = InternalDecrypt(text, key);

        return result;
    }
}
public interface ICrypto
{
    string EncryptByKeyInternal(string text, string key);
    string DecryptByKeyInternal(string text, string key);
}
