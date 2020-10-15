namespace Alpha.DNAide.Web
{
    using DNAide.Core;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Threading;
    using System.Web;
    using System.Web.UI;
    using System.Xml;

    public class TplPage : Page
    {
        private string _mainTpl;

        private string _block;

        private string _blockTpl;

        private string _blockTpl2;

        private bool _blockRepeated;

        private StringBuilder _blockStr;

        private Stack<KeyValuePair<string, StringBuilder>> _blockSet = new Stack<KeyValuePair<string, StringBuilder>>();

        protected string ThemePath;

        protected string HeadTitle;

        protected string HeadKeyword;

        protected string HeadDescription;

        protected string HeadContentType;

        protected string CssGroup;

        protected string _relPath;

        protected bool IsXHtml = true;

        protected bool AutoIncludeJs = true;

        protected bool AutoRemoveComment = true;

        private StringBuilder _jsVariables = new StringBuilder(5000);

        private static string[] themeNames;

        private static Hashtable _themes;

        private static string defaultTheme;

        private static string[] _jsFiles;

        private static string[] _cssGroups;

        private static string DefHeadTitle;

        private static string DefHeadKeyword;

        private static string DefHeadDescription;

        private static string DefHeadContentType;

        public static string CCulture
        {
            get
            {
                return CastAide.AsString(HttpContext.Current.Session["Tpl_Culture"], "en-US");
            }
            set
            {
                HttpContext.Current.Session["Tpl_Culture"] = value;
            }
        }

        public static string[] CssGroups
        {
            get
            {
                return TplPage._cssGroups;
            }
        }

        protected string CssLinks
        {
            get
            {
                string str = "<link href='../{RelPath}' type='text/css' rel='stylesheet' />";
                string str1 = "\n";
                string[] item = (string[])TplPage._themes[TplPage.CTheme];
                string relativePath = this.RelativePath;
                for (int i = 0; i < (int)item.Length; i++)
                {
                    if (item[i].Contains(".css"))
                    {
                        if (string.IsNullOrEmpty(this.CssGroup))
                        {
                            str1 = string.Concat(str1, str.Replace("{RelPath}", string.Concat(relativePath, item[i])));
                        }
                        else if (item[i].IndexOf(TplPage.CssGroups[0], StringComparison.InvariantCultureIgnoreCase) > -1 || item[i].IndexOf(this.CssGroup, StringComparison.InvariantCultureIgnoreCase) > -1)
                        {
                            str1 = string.Concat(str1, str.Replace("{RelPath}", string.Concat(relativePath, item[i])));
                        }
                    }
                }
                str1 = string.Concat(str1, "\n");
                return str1;
            }
        }

        public static string CTheme
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    return null;
                }
                return CastAide.AsString(HttpContext.Current.Session["Tpl_Theme"], TplPage.defaultTheme);
            }
            set
            {
                if (TplPage.ValidateTheme(value))
                {
                    HttpContext.Current.Session["Tpl_Theme"] = value;
                }
            }
        }

        public bool IsPostBack2
        {
            get
            {
                return base.Request.Form.Count > 0;
            }
        }

        protected string JsVersion
        {
            get
            {
                return ConfigurationManager.AppSettings["JSVersion"];
            }
        }

        protected string RelativePath
        {
            get
            {
                return this._relPath;
            }
        }

        static TplPage()
        {
            TplPage._themes = new Hashtable();
            TplPage._jsFiles = null;
            TplPage._cssGroups = null;
            TplPage.themeNames = Directory.GetDirectories(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "App_Themes"));
            TplPage.defaultTheme = TplPage.themeNames[0].Substring(TplPage.themeNames[0].IndexOf("App_Themes") + 10).Replace("\\", "");
            for (int i = 0; i < (int)TplPage.themeNames.Length; i++)
            {
                string[] files = Directory.GetFiles(TplPage.themeNames[i]);
                for (int j = 0; j < (int)files.Length; j++)
                {
                    files[j] = files[j].Substring(files[j].IndexOf("App_Themes")).Replace("\\", "/");
                }
                TplPage.themeNames[i] = TplPage.themeNames[i].Substring(TplPage.themeNames[i].IndexOf("App_Themes") + 10).Replace("\\", "");
                TplPage._themes.Add(TplPage.themeNames[i], files);
            }
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["Tpl_Theme"]))
            {
                TplPage.defaultTheme = ConfigurationManager.AppSettings["Tpl_Theme"];
            }
            if (!File.Exists(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "\\Web.info")))
            {
                string empty = string.Empty;
                TplPage.DefHeadContentType = empty;
                TplPage.DefHeadDescription = empty;
                TplPage.DefHeadKeyword = empty;
                TplPage.DefHeadTitle = empty;
            }
            else
            {
                XmlDocument xmlDocument = new XmlDocument();
                try
                {
                    xmlDocument.Load(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "\\Web.info"));
                    foreach (XmlNode childNode in xmlDocument.ChildNodes[1].ChildNodes)
                    {
                        if (childNode.Attributes["name"].Value == "title")
                        {
                            TplPage.DefHeadTitle = childNode.Attributes["value"].Value;
                        }
                        else if (childNode.Attributes["name"].Value == "keywords")
                        {
                            TplPage.DefHeadKeyword = childNode.Attributes["value"].Value;
                        }
                        else if (childNode.Attributes["name"].Value != "description")
                        {
                            if (childNode.Attributes["name"].Value != "contenttype")
                            {
                                continue;
                            }
                            TplPage.DefHeadContentType = childNode.Attributes["value"].Value;
                        }
                        else
                        {
                            TplPage.DefHeadDescription = childNode.Attributes["value"].Value;
                        }
                    }
                }
                catch
                {
                    string str = string.Empty;
                    TplPage.DefHeadContentType = str;
                    TplPage.DefHeadDescription = str;
                    TplPage.DefHeadKeyword = str;
                    TplPage.DefHeadTitle = str;
                }
            }
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["JsFiles"]))
            {
                string item = ConfigurationManager.AppSettings["JsFiles"];
                char[] chrArray = new char[] { ';' };
                TplPage._jsFiles = item.Split(chrArray);
            }
            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["CssGroups"]))
            {
                TplPage._cssGroups = new string[] { "xxx" };
                return;
            }
            string item1 = ConfigurationManager.AppSettings["CssGroups"];
            char[] chrArray1 = new char[] { ';' };
            TplPage._cssGroups = item1.Split(chrArray1);
        }

        public TplPage()
        {
            string str = HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.Replace("~/", string.Empty);
            while (true)
            {
                int num = str.IndexOf("/");
                if (num == -1)
                {
                    break;
                }
                str = str.Remove(0, num + 1);
                TplPage tplPage = this;
                tplPage._relPath = string.Concat(tplPage._relPath, "../");
            }
        }

        public void AddJsRawVariable(string strName, string value)
        {
            this._jsVariables.Append(string.Concat(strName, "=", value.ToString(), ";\r\n"));
        }

        public void AddJsVariable(string strName, string value)
        {
            this._jsVariables.Append(string.Concat(strName, "='", value.ToString().Replace("'", "\\'"), "';\r\n"));
        }

        private void EndBlock()
        {
            if (!string.IsNullOrEmpty(this._block) && this._blockStr != null)
            {
                if (!this._blockRepeated)
                {
                    this._blockStr.Append(this._blockTpl2);
                    this._blockRepeated = false;
                }
                this._blockSet.Push(new KeyValuePair<string, StringBuilder>(this._block, this._blockStr));
                this._block = null;
                this._blockStr = null;
            }
        }

        protected string GetKeySessionLogged()
        {
            string str = CastAide.AsString(HttpContext.Current.Session["Tpl_Culture"], "en-US");
            return str;
        }

        protected override void InitializeCulture()
        {
            CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(TplPage.CCulture);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(TplPage.CCulture);
            Thread.CurrentThread.CurrentCulture.DateTimeFormat = cultureInfo.DateTimeFormat;
            base.InitializeCulture();
        }

        private void NewBlock(string blockName)
        {
            this._blockRepeated = false;
            if (!string.IsNullOrEmpty(this._blockTpl))
            {
                this._block = blockName;
                this._blockStr = new StringBuilder();
                return;
            }
            this._block = string.Empty;
            this._blockStr = null;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            CastAide.AsInt32(HttpContext.Current.Session["CustID"], 0);
            CastAide.AsInt32(HttpContext.Current.Session["SubAccID"], 0);
            CastAide.AsInt32(HttpContext.Current.Session["OnUserID"], 0);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!this.IsXHtml)
            {
                writer.Write("<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">\n");
            }
            else
            {
                writer.Write("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\n");
            }
            writer.Write("<html>\n");
            writer.Write("<head>\n");
            this.RenderHead(writer);
            writer.Write("</head>\n");
            writer.Write("<body>\n");
            this.EndBlock();
            while (this._blockSet.Count > 0)
            {
                KeyValuePair<string, StringBuilder> keyValuePair = this._blockSet.Pop();
                TemplateHandler.CheckInBlock(ref this._mainTpl, keyValuePair.Key, keyValuePair.Value.ToString());
            }
            if (this.AutoRemoveComment)
            {
                TemplateHandler.RemoveComment(ref this._mainTpl);
            }
            writer.Write(this._mainTpl);
            writer.Write("\n</body>\n");
            writer.Write("</html>\n");
            this.WriteJsVariables(writer);
        }

        protected void RenderHead(HtmlTextWriter writer)
        {
            writer.Write(string.Concat("<title>", (string.IsNullOrEmpty(this.HeadTitle) ? TplPage.DefHeadTitle : this.HeadTitle), "</title>\n"));
            writer.Write(string.Concat("<meta name=\"keywords\" content=\"", (string.IsNullOrEmpty(this.HeadKeyword) ? TplPage.DefHeadKeyword : this.HeadTitle), "\" />\n"));
            writer.Write(string.Concat("<meta name=\"description\" content=\"", (string.IsNullOrEmpty(this.HeadDescription) ? TplPage.DefHeadDescription : this.HeadTitle), "\" />\n"));
            writer.Write(string.Concat("<meta http-equiv=\"Content-Type\" content=\"", (string.IsNullOrEmpty(this.HeadContentType) ? TplPage.DefHeadContentType : this.HeadTitle), "\" />\n"));
            writer.Write(this.CssLinks);
            if (this.AutoIncludeJs && TplPage._jsFiles != null)
            {
                for (int i = 0; i < (int)TplPage._jsFiles.Length; i++)
                {
                    writer.Write(string.Concat("<script type='text/javascript' src='../", this.RelativePath, TplPage._jsFiles[i]));
                    if (!string.IsNullOrEmpty(this.JsVersion))
                    {
                        writer.Write(string.Concat("?", this.JsVersion));
                    }
                    writer.Write("' ></script>\n");
                }
            }
            base.Response.ContentType = "text/html";
            base.Render(writer);
        }

        protected void TplAssign(string key, object value)
        {
            TemplateHandler.Assign(ref this._mainTpl, key, value);
        }

        protected void TplAssignBlock(string key, object value)
        {
            TemplateHandler.Assign(ref this._blockTpl2, key, value);
        }

        protected void TplCheckinInnerBlock(string block, string content)
        {
            TemplateHandler.CheckInBlock(ref this._blockTpl2, block, content);
        }

        protected string TplCheckoutInnerBlock(string block, bool blockMarkup)
        {
            return TemplateHandler.CheckOutBlock(ref this._blockTpl2, block, blockMarkup);
        }

        protected void TplExecBlock(string block)
        {
            if (!string.IsNullOrEmpty(this._mainTpl))
            {
                this.EndBlock();
                string str = TemplateHandler.CheckOutBlock(ref this._mainTpl, block);
                string str1 = str;
                this._blockTpl = str;
                this._blockTpl2 = str1;
                this.NewBlock(block);
            }
        }

        protected void TplExecBlock(string block, string tpl)
        {
            this.EndBlock();
            string str = TemplateHandler.LoadTemplate(tpl);
            string str1 = str;
            this._blockTpl = str;
            this._blockTpl2 = str1;
            this.NewBlock(block);
        }

        protected void TplRemBlock(string block)
        {
            TemplateHandler.CheckOutBlock(ref this._mainTpl, block, false);
        }

        protected void TplRemInnerBlock(string block)
        {
            TemplateHandler.CheckOutBlock(ref this._blockTpl2, block, false);
            TemplateHandler.CheckOutBlock(ref this._blockTpl, block, false);
        }

        protected void TplRepeatBlock()
        {
            this._blockRepeated = true;
            if (this._blockStr != null)
            {
                this._blockStr.Append(this._blockTpl2);
            }
            this._blockTpl2 = this._blockTpl;
        }

        protected void TplSetContent(string content)
        {
            this._mainTpl = content;
        }

        protected void TplSetTemplate(string tpl)
        {
            this._mainTpl = TemplateHandler.LoadTemplate(tpl);
        }

        public static bool ValidateTheme(string themeName)
        {
            bool flag = false;
            for (int i = 0; i < (int)TplPage.themeNames.Length; i++)
            {
                if (string.Compare(themeName, TplPage.themeNames[i], true) == 0)
                {
                    flag = true;
                }
            }
            return flag;
        }

        private void WriteJsVariables(HtmlTextWriter writer)
        {
            if (this._jsVariables.Length == 0)
            {
                return;
            }
            writer.WriteLine("<script type=\"text/javascript\">\r\n");
            writer.WriteLine(this._jsVariables.ToString());
            writer.WriteLine("</script>\r\n");
        }
    }
}
