namespace Sunplus.Agent.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;
    using Microsoft.VisualBasic;

    public class Encryption
    {
        private const string cfsKey = "&％#@?,:*!~^.";

        public const string HEX_DIGIT = "0123456789abcdef";

        public static string PearsonEncode(string key)
        {
            if (string.IsNullOrEmpty(key) || key.Length == 1) return key;

            var h = 0L;
            var c = key.Length;
            var keys = key.ToCharArray();

            for (var i = 0; i < c; i++)
            {
                h += keys[i].GetHashCode();
            }

            return h.ToString();
        }

        private static string CfsCode(string nWord)
        {
            var number = "";
            for (var i = 1; i <= nWord.Length; i++)
            {
                number = number + Strings.Asc(Strings.Mid(nWord, i, 1));
            }
            return Conversion.Hex(number);
        }

        /// <summary>
        ///     One-way encryption
        /// </summary>
        /// <param name="codeStr"></param>
        /// <returns></returns>
        public static string CfsEnCode(string codeStr)
        {
            var num = 30;
            var num2 = num - codeStr.Length;
            if (num2 > 1)
            {
                for (var m = 1; m <= num2; m++)
                {
                    codeStr = codeStr + Strings.Chr(0x15);
                }
            }
            var num3 = 1.0;
            for (var i = 1; i <= num; i++)
            {
                var num5 = num + Strings.Asc(Strings.Mid(codeStr, i, 1))*i;
                num3 *= num5;
            }
            codeStr = num3.ToString();
            var str = "";
            for (var j = 1; j <= codeStr.Length; j++)
            {
                str = str + CfsCode(Strings.Mid(codeStr, j, 3));
            }
            var str2 = "";
            for (var k = 20; k <= str.Length - 0x12; k += 2)
            {
                str2 = str2 + Strings.Mid(str, k, 1);
            }
            return str2;
        }

        /// <summary>
        ///     One-way encryption
        /// </summary>
        /// <param name="codeStr"></param>
        /// <returns></returns>
        public static string CfsEnCode(string codeStr, int number)
        {
            var num = number < 6 ? 6 : number;
            var num2 = num - codeStr.Length;
            if (num2 > 1)
            {
                for (var m = 1; m <= num2; m++)
                {
                    codeStr = codeStr + Strings.Chr(0x15);
                }
            }
            var num3 = 1.0;
            for (var i = 1; i <= num; i++)
            {
                var num5 = num + Strings.Asc(Strings.Mid(codeStr, i, 1))*i;
                num3 *= num5;
            }
            codeStr = num3.ToString();
            var str = "";
            for (var j = 1; j <= codeStr.Length; j++)
            {
                str = str + CfsCode(Strings.Mid(codeStr, j, 3));
            }
            var str2 = "";
            for (var k = 20; k <= str.Length - 0x12; k += 2)
            {
                str2 = str2 + Strings.Mid(str, k, 1);
            }
            return str2;
        }

        public static string CfsDeCrypt(string strText)
        {
            try
            {
                var rgbIV = new byte[0];
                byte[] rgbKey = {0x21, 0x63, 0x54, 0x5a, 0x91, 0xac, 110, 0xcc};
                var buffer = new byte[strText.Length];
                rgbIV = Encoding.UTF8.GetBytes(cfsKey);
                var provider = new DESCryptoServiceProvider();
                buffer = Convert.FromBase64String(strText);
                var stream = new MemoryStream();
                var stream2 = new CryptoStream(stream, provider.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                stream2.Write(buffer, 0, buffer.Length);
                stream2.FlushFinalBlock();
                return Encoding.UTF8.GetString(stream.ToArray());
            }
            catch (Exception exception)
            {
                return exception.Message;
            }
        }

        public static string CfsEnCrypt(string strText)
        {
            try
            {
                var rgbIV = new byte[0];
                byte[] rgbKey = {0x21, 0x63, 0x54, 0x5a, 0x91, 0xac, 110, 0xcc};
                rgbIV = Encoding.UTF8.GetBytes(cfsKey);
                var provider = new DESCryptoServiceProvider();
                var bytes = Encoding.UTF8.GetBytes(strText);
                var stream = new MemoryStream();
                var stream2 = new CryptoStream(stream, provider.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                stream2.Write(bytes, 0, bytes.Length);
                stream2.FlushFinalBlock();
                return Convert.ToBase64String(stream.ToArray());
            }
            catch (Exception exception)
            {
                return exception.Message;
            }
        }

        public static string RemoveSpecialChars(string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;
            return str.Replace("\n", "\\n").Replace("\"", "\\\"").Replace("'", "\\'");
        }

        /// <summary>
        ///     Generate a random string
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string RandomString(int length)
        {
            return RandomString(length, false);
        }

        public static string RandomString(int length, bool noZeroO)
        {
            string chars;
            if (noZeroO)
                chars = "qwertyuipasdfghjklzxcvbnm123456789";
            else
                chars = "qwertyuiopasdfghjklzxcvbnm0123456789";
            var rndStr = string.Empty;
            var rnd = new Random(DateTime.Now.Second + DateTime.Now.Millisecond);

            for (var i = 0; i < length; ++i)
            {
                rndStr += chars[rnd.Next()%chars.Length];
            }
            return rndStr;
        }

        public static string RandomNumberString(int length)
        {
            var chars = "0123456789";
            var rndStr = string.Empty;
            var rnd = new Random();

            for (var i = 0; i < length; ++i)
            {
                rndStr += chars[rnd.Next()%chars.Length];
            }
            return rndStr;
        }

        public static string HexEncode(string data)
        {
            var b16_digits = HEX_DIGIT;
            var b16_map = new string[256];
            for (var i = 0; i < 256; i++)
            {
                b16_map[i] = string.Concat(b16_digits[i >> 4], b16_digits[i & 15]);
            }

            var c = data.Length;
            var result = new List<string>();

            for (var i = 0; i < c; i++)
            {
                result.Add(b16_map[Convert.ToInt32(data[i])]);
            }

            return string.Join(null, result);
        }

        public static string HexDecode(string data)
        {
            var b16_digits = HEX_DIGIT;
            var b16_map = new char[256];
            for (var i = 0; i < 256; i++)
            {
                b16_map[Convert.ToInt32(string.Concat(b16_digits[i >> 4], b16_digits[i & 15]), 16)] = Convert.ToChar(i);
            }
            var c = data.Length;
            if (c%2 != 0) data = '0' + data;
            var result = new List<char>();


            for (var i = 0; i < c; i += 2)
            {
                result.Add(b16_map[Convert.ToInt32(data.Substring(i, 2), 16)]);
            }
            return new string(result.ToArray());
        }

        public static string RC4(string key, string value)
        {
            var ks = key.ToCharArray();
            var vs = value.ToCharArray();
            var c = key.Length;
            var d = value.Length;
            var s = new int[256];
            var i = 0;
            for (i = 0; i < 256; i++)
            {
                s[i] = i;
            }
            var j = 0;
            int x;
            for (i = 0; i < 256; i++)
            {
                j = (j + s[i] + ks[i%c])%256;
                x = s[i];
                s[i] = s[j];
                s[j] = x;
            }
            i = 0;
            j = 0;
            var ct = string.Empty;
            for (var y = 0; y < d; y++)
            {
                i = (i + 1)%256;
                j = (j + s[i])%256;
                x = s[i];
                s[i] = s[j];
                s[j] = x;
                ct += Convert.ToChar(vs[y] ^ s[(s[i] + s[j])%256]);
            }
            return ct;
        }
    }
}