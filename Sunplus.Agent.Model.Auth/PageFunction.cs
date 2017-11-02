namespace Sunplus.Agent.Model.BL.Authorization
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Xml;
    using Fanex.Logging.Extensions.Web;

    [Obsolete("Should rename to Sunplus.Agent.Model.Auth")]
    public class RolePermissionPair
    {
        public byte Role { get; set; }

        /// <summary>
        /// [A], [B], [C], [D], [E]...
        /// </summary>
        public string Permission { get; set; }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed.")]
    public class PageFunction
    {
        /// <summary>
        /// Id of the function.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Related url.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Name of the function.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Name of the sub-function.
        /// </summary>
        public string SubName { get; set; }

        /// <summary>
        /// Array of 2-[A], 3-[D]...
        /// </summary>
        public RolePermissionPair[] RoleRules { get; set; }
    }

    public class PageFunctionCollection
    {
        /// <summary>
        /// Get collection from a file
        /// </summary>
        static PageFunctionCollection()
        {
            PageFunctions = new Dictionary<string, PageFunction>(100);

            // Load description from xml file
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.Load(AppDomain.CurrentDomain.BaseDirectory + "App_Data\\PagesAccess.xml");

            XmlNodeList pageList = xmlDoc.GetElementsByTagName("add");
            string[] rolePermissions;
            foreach (XmlNode node in pageList)
            {
                try
                {
                    var function = new PageFunction
                    {
                        Url = node.Attributes["url"].Value.ToLower(),
                        Id = int.Parse(node.Attributes["id"].Value),
                        Name = node.Attributes["fun"].Value,
                        SubName = node.Attributes["subfun"].Value
                    };

                    if (!string.IsNullOrEmpty(node.Attributes["role"].Value))
                    {
                        // Format: 2-[A],3-[D]...
                        rolePermissions = node.Attributes["role"].Value.Split(',');
                        var rolePairs = new RolePermissionPair[rolePermissions.Length];

                        for (int i = 0; i < rolePermissions.Length; ++i)
                        {
                            rolePairs[i] = new RolePermissionPair
                            {
                                Role = byte.Parse(rolePermissions[i].Substring(0, 1)),
                                Permission = rolePermissions[i].Substring(2)
                            };
                        }

                        function.RoleRules = rolePairs;
                    }
                    else
                    {
                        function.RoleRules = null;
                    }

                    // Add to collection
                    PageFunctions.Add(function.Url, function);
                }
                catch (Exception e)
                {
                    ExceptionHandler.Handle(e);
                }
            }
        }

        protected static Dictionary<string, PageFunction> PageFunctions { get; set; }

        public static PageFunction GetPageFunctionByUrl(string url)
        {
            url = url.ToLower();
            return !PageFunctions.ContainsKey(url) ? null : PageFunctions[url];
        }
    }
}