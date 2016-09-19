namespace Sunplus.Agent.Utilities
{
    public class EncryptorImpl : Encryptor
    {
        public string CfsDeCrypt(string strText)
        {
            return Encryption.CfsDeCrypt(strText);
        }

        public string CfsEnCode(string codeStr)
        {
            return Encryption.CfsEnCode(codeStr);
        }

        public string CfsEnCode(string codeStr, int number)
        {
            return Encryption.CfsEnCode(codeStr, number);
        }

        public string CfsEnCrypt(string strText)
        {
            return Encryption.CfsEnCrypt(strText);
        }

        public string HexDecode(string data)
        {
            return Encryption.HexDecode(data);
        }

        public string HexEncode(string data)
        {
            return Encryption.HexEncode(data);
        }

        public string PearsonEncode(string key)
        {
            return Encryption.PearsonEncode(key);
        }

        public string RandomNumberString(int length)
        {
            return Encryption.RandomNumberString(length);
        }

        public string RandomString(int length)
        {
            return Encryption.RandomString(length);
        }

        public string RandomString(int length, bool noZeroO)
        {
            return Encryption.RandomString(length);
        }

        public string RC4(string key, string value)
        {
            return Encryption.RC4(key, value);
        }

        public string RemoveSpecialChars(string str)
        {
            return Encryption.RemoveSpecialChars(str);
        }
    }
}