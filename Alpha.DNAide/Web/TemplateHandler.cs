namespace Alpha.DNAide.Web
{
    using DNAide.Core;
    using Fanex.Caching;
    using Fanex.Caching.Memory;
    using ILogger;
    using System;
    using System.Configuration;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Threading;

    public static class TemplateHandler
    {
        private static readonly bool useCache;
        private static readonly string _templateFolder;
        private readonly static ICacheService cache;
        private readonly static Regex regexComment;

        static TemplateHandler()
        {
            TemplateHandler._templateFolder = (string.IsNullOrEmpty(ConfigurationManager.AppSettings["template_folder"]) ? "Templates" : ConfigurationManager.AppSettings["template_folder"]);
            TemplateHandler.useCache = ConfigurationManager.AppSettings["LocalTest"] == "0";
            TemplateHandler.cache = new CacheService(new MemoryCache(new MemoryCacheOptions()));
            TemplateHandler.regexComment = new Regex("<!--[^>]*-->", RegexOptions.Compiled);
        }

        public static string TemplateFolder
        {
            get
            {
                return TemplateHandler._templateFolder;
            }
        }

        public static void Assign(ref string template, string tag, object value)
        {
            string str = CastAide.AsString(value, string.Empty);
            if (!string.IsNullOrEmpty(template) && !string.IsNullOrEmpty(tag))
            {
                if (tag[0] != '{' && tag[tag.Length - 1] != '}')
                {
                    tag = string.Concat("{", tag, "}");
                }
                template = template.Replace(tag, str);
            }
        }

        public static string LoadTemplate(string templateName)
        {
            return TemplateHandler.LoadTemplate(templateName, true);
        }

        public static string LoadTemplate(string templateName, bool hasShrinkHtml)
        {
            return TemplateHandler.LoadTemplate(templateName, hasShrinkHtml, TemplateHandler.TemplateFolder);
        }

        public static string LoadTemplate(string templateName, bool hasShrinkHtml, string templateFolder)
        {
            if (string.IsNullOrEmpty(templateName) || string.IsNullOrEmpty(templateFolder))
            {
                throw new InvalidDataException("Template name and template folder cannot be null or empty");
            }
            string empty = string.Empty;
            string lower = string.Format("{0}_{1}_{2}", templateFolder, templateName, Thread.CurrentThread.CurrentCulture.Name);
            lower = lower.ToLower();
            if (TemplateHandler.useCache && TemplateHandler.cache.Contain(lower))
            {
                empty = TemplateHandler.cache.Get<string>(lower);
                if (!string.IsNullOrEmpty(empty))
                {
                    return empty;
                }
            }
            string str = Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, templateFolder), templateName);
            try
            {
                if (File.Exists(str))
                {
                    using (StreamReader streamReader = File.OpenText(str))
                    {
                        empty = streamReader.ReadToEnd();
                        streamReader.Close();
                    }
                }
            }
            catch (Exception exception)
            {
                Service.WriteLog(exception.Message, LoggingType.Error);
                empty = string.Empty;
            }
            if (hasShrinkHtml && !string.IsNullOrEmpty(empty))
            {
                TemplateHandler.ShrinkHtml(ref empty);
            }
            if (!string.IsNullOrEmpty(empty))
            {
                TemplateHandler.cache.Set<string>(lower, empty, new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(120)));
            }
            return empty;
        }

        public static void ShrinkHtml(ref string content)
        {
            if (!string.IsNullOrEmpty(content))
            {
                content = Regex.Replace(content, "\\s*(<[^>]+>)\\s*", "$1", RegexOptions.Singleline);
            }
        }

        public static void RemoveBlock(ref string str, string tag)
        {
            if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(tag))
            {
                return;
            }
            string str1 = string.Concat("<!--", tag, "-->");
            while (str.IndexOf(str1) >= 0)
            {
                int num = str.IndexOf(str1);
                if (num == -1)
                {
                    return;
                }
                int num1 = str.IndexOf(string.Concat("<!--/", tag, "-->"), num) - str.IndexOf(str1) + str1.Length + 1;
                if (num1 <= 0)
                {
                    return;
                }
                str = str.Remove(num, num1);
            }
        }

        public static string ReplaceBlock(ref string str, string tag, string newStr)
        {
            if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(tag))
            {
                return string.Empty;
            }
            if (object.ReferenceEquals(newStr, null))
            {
                newStr = string.Empty;
            }
            string str1 = string.Concat("<!--", tag, "-->");
            int num = str.IndexOf(str1);
            if (num == -1)
            {
                return string.Empty;
            }
            int num1 = str.IndexOf(string.Concat("<!--/", tag, "-->"), num) - str.IndexOf(str1) - str1.Length;
            if (num1 <= 0)
            {
                return string.Empty;
            }
            string str2 = str.Substring(num + str1.Length, num1);
            str = str.Replace(str2, newStr);
            return str2;
        }

        public static string CopyBlock(string str, string tag)
        {
            if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(tag))
            {
                return string.Empty;
            }
            string str1 = string.Concat("<!--", tag, "-->");
            int num = str.IndexOf(str1);
            if (num == -1)
            {
                return string.Empty;
            }
            int num1 = str.IndexOf(string.Concat("<!--/", tag, "-->"), num) - str.IndexOf(str1) - str1.Length;
            if (num1 <= 0)
            {
                return string.Empty;
            }
            return str.Substring(num + str1.Length, num1);
        }

        public static void CheckInBlock(ref string str, string tag, string content)
        {
            if (!string.IsNullOrEmpty(str) && !string.IsNullOrEmpty(tag) && !string.IsNullOrEmpty(content))
            {
                str = str.Replace(string.Concat("<!--", tag, "/-->"), content);
            }
        }

        public static void RemoveComment(ref string content)
        {
            if (!string.IsNullOrEmpty(content))
            {
                content = TemplateHandler.regexComment.Replace(content, string.Empty);
            }
        }

        public static string CheckOutBlock(ref string str, string tag)
        {
            return TemplateHandler.CheckOutBlock(ref str, tag, true);
        }

        public static string CheckOutBlock(ref string str, string tag, bool blockMarkup)
        {
            if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(tag))
            {
                return str;
            }
            string str1 = string.Concat("<!--", tag, "-->");
            int num = str.IndexOf(str1);
            if (num == -1)
            {
                return string.Empty;
            }
            int num1 = str.IndexOf(string.Concat("<!--/", tag, "-->"), num) - num - str1.Length;
            if (num1 <= 0)
            {
                return string.Empty;
            }
            string str2 = str.Substring(num, num1 + 2 * str1.Length + 1);
            str = (blockMarkup ? str.Replace(str2, string.Concat("<!--", tag, "/-->")) : str.Replace(str2, string.Empty));
            str2 = str2.Substring(str1.Length, num1);
            return str2;
        }
    }
}
