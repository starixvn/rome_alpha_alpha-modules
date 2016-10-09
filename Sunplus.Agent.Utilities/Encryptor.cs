namespace Sunplus.Agent.Utilities
{
    public interface Encryptor
    {
        string CfsDeCrypt(string strText);

        string CfsEnCode(string codeStr);

        string CfsEnCode(string codeStr, int number);

        string CfsEnCrypt(string strText);

        string HexDecode(string data);

        string HexEncode(string data);

        string PearsonEncode(string key);

        string RandomNumberString(int length);

        string RandomString(int length);

        string RandomString(int length, bool noZeroO);

        string RC4(string key, string value);

        string RemoveSpecialChars(string str);

        string ComputeHash(string hashAlgorithm, string plainText, string salt = null);
    }
}