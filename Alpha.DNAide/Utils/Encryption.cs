namespace Alpha.DNAide.Utils
{
    using Microsoft.VisualBasic;
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    public static class Encryption
    {
        public static string GetDeCryptConnectionString(string connStr)
        {
            string[] decryptPWD = Encryption.GetDecryptPWD(connStr, "=");
            string str = Encryption.CfsDeCrypt(decryptPWD[1].Trim());
            return string.Concat(decryptPWD[0].Trim(), str, ";");
        }

        private static string CfsCode(string nWord)
        {
            string str = "";
            for (int i = 1; i <= nWord.Length; i++)
            {
                str = string.Concat(str, Strings.Asc(Strings.Mid(nWord, i, 1)));
            }
            return Conversion.Hex(str);
        }

        public static string CfsDeCrypt(string strText)
        {
            string str;
            try
            {
                byte[] bytes = new byte[0];
                byte[] numArray = new byte[] { 33, 99, 84, 90, 145, 172, 110, 204 };
                byte[] numArray1 = new byte[strText.Length];
                bytes = Encoding.UTF8.GetBytes("&％#@?,:*!~^.");
                DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
                numArray1 = Convert.FromBase64String(strText);
                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateDecryptor(numArray, bytes), CryptoStreamMode.Write);
                cryptoStream.Write(numArray1, 0, (int)numArray1.Length);
                cryptoStream.FlushFinalBlock();
                str = Encoding.UTF8.GetString(memoryStream.ToArray());
            }
            catch (Exception exception)
            {
                str = exception.Message;
            }
            return str;
        }

        public static string CfsEnCode(string codeStr)
        {
            int num = 30;
            int length = num - codeStr.Length;
            if (length > 1)
            {
                for (int i = 1; i <= length; i++)
                {
                    codeStr = string.Concat(codeStr, Strings.Chr(21));
                }
            }
            double num1 = 1;
            for (int j = 1; j <= num; j++)
            {
                int num2 = num + Strings.Asc(Strings.Mid(codeStr, j, 1)) * j;
                num1 *= (double)num2;
            }
            codeStr = num1.ToString();
            string str = "";
            for (int k = 1; k <= codeStr.Length; k++)
            {
                str = string.Concat(str, Encryption.CfsCode(Strings.Mid(codeStr, k, 3)));
            }
            string str1 = "";
            for (int l = 20; l <= str.ToString().Length - 18; l += 2)
            {
                str1 = string.Concat(str1, Strings.Mid(str, l, 1));
            }
            return str1;
        }

        private static string[] GetDecryptPWD(string SourceString, string SearchString)
        {
            int num = 1;
            string str = "";
            string str1 = "";
            string str2 = "";
            string str3 = "";
            for (int i = SourceString.Length - 1; i >= 0; i--)
            {
                str = string.Concat(str, SourceString.Substring(i, 1));
            }
            for (int j = SearchString.Length - 1; j >= 0; j--)
            {
                str1 = string.Concat(str1, SearchString.Substring(j, 1));
            }
            if (str.Substring(1, 2) == "==")
            {
                num += 2;
                for (int k = 3; k <= str.Length && !(str.Substring(k, 1) == "="); k++)
                {
                    num++;
                }
                str3 = str.Substring(0, num);
                for (int l = str3.Length; l > 1; l--)
                {
                    str2 = string.Concat(str2, str3.Substring(l - 1, 1));
                }
            }
            else if (str.Substring(1, 1) == "=")
            {
                num++;
                int num1 = 2;
                while (true)
                {
                    if (num1 <= str.Length)
                    {
                        if (str.Substring(num1, 1) == "=")
                        {
                            break;
                        }
                        num++;
                        num1++;
                    }
                    else
                    {
                        break;
                    }
                }
                str3 = str.Substring(0, num);
                for (int m = str3.Length; m > 1; m--)
                {
                    str2 = string.Concat(str2, str3.Substring(m - 1, 1));
                }
            }
            else
            {
                for (int n = 1; n <= str.Length && !(str.Substring(n, 1) == "="); n++)
                {
                    num++;
                }
                str3 = str.Substring(0, num);
                for (int o = str3.Length; o > 1; o--)
                {
                    str2 = string.Concat(str2, str3.Substring(o - 1, 1));
                }
            }
            string[] strArrays = new string[] { SourceString.Substring(0, SourceString.Length - num), str2 };
            return strArrays;
        }
    }
}
