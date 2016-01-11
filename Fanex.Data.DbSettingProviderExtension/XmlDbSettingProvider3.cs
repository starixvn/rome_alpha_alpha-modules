namespace Fanex.Data.DbSettingProviderExtension
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Runtime.Caching;
    using System.Security.Permissions;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Schema;
    using Configuration;
    using Logging;

    /// <summary>
    /// Xml setting provider.
    /// </summary>
    public class XmlDbSettingProvider3 : DbSettingProviderBase, IDisposable
    {
        /// <summary>
        /// Database key name.
        /// </summary>
        protected const string DbSettingCacheKeyName = "XmlDbSettingProvider:DBSetting";

        /// <summary>
        /// Default execute time out.
        /// </summary>
        protected const int TimeOutDefault = 120;

        /// <summary>
        /// EnableWatching configure key name.
        /// </summary>
        protected const string EnableWatchingName = "enableWatching";

        /// <summary>
        /// SyncRoot to check thread safe when implementing singleton pattern.
        /// </summary>
        private static object _syncRoot = new object();

        private static volatile Dictionary<string, IDbSetting> _databaseSettingDataSource;

        /// <summary>
        /// Resource watcher to watch resource change.
        /// </summary>
        private FileSystemWatcherEx _resourceWatcher;

        /// <summary>
        /// Get setting data source from cache.
        /// </summary>
        /// <value>Dictionary setting.</value>
        public override Dictionary<string, IDbSetting> SettingDataSource
        {
            get
            {
                return _databaseSettingDataSource;
            }
        }

        #region Caching Object

        /// <summary>
        /// Should run it before any event in application start event.
        /// </summary>
        public override void Start()
        {
            this.AddDataSourceToCache(ResourcePath);
        }

        /// <summary>
        /// Insert database setting to resource.
        /// </summary>
        /// <param name="setting">Setting type of database setting.</param>
        public override void InsertSetting(IDbSetting setting)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update resource setting.
        /// </summary>
        /// <param name="setting">Database setting.</param>
        public override void UpdateSetting(IDbSetting setting)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Delete resource setting.
        /// </summary>
        /// <param name="key">String key.</param>
        public override void DeleteSetting(string key)
        {
            throw new NotImplementedException();
        }

        #endregion Caching Object

        /// <summary>
        /// Initialize resource watch.
        /// <see cref="http://msdn.microsoft.com/en-us/library/system.io.filesystemwatcher.aspx"/>
        /// </summary>
        /// <param name="name">Name of configure.</param>
        /// <param name="config">Configure name value collection.</param>
        public override void Initialize(string name, NameValueCollection config)
        {
            base.Initialize(name, config);

            // check configuration to start watch resource file.
            if (config[EnableWatchingName] != null)
            {
                if (config[EnableWatchingName] == "1"
                    || config[EnableWatchingName].Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    this.StartWatchResource(ResourcePath);
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #region protected functions

        /// <summary>
        /// Check whether connection was encrypted or not.
        /// </summary>
        /// <param name="element">XElement element.</param>
        /// <returns>Return true if connection requires encrypted.</returns>
        protected static bool IsEncryptedConnection(XElement element)
        {
            var attr = element.Attribute("Plain");
            var isPlaintConnection = attr != null
                           && (attr.Value.Equals("1") || attr.Value.Equals("true", StringComparison.OrdinalIgnoreCase));

            return !isPlaintConnection;
        }

        /// <summary>
        /// Start run file system watcher to fire event ResourceChangedEvent.
        /// Whenever the resource change.
        /// </summary>
        /// <param name="path">Resource path.</param>
        [SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Justification = "Reviewed.")]
        protected virtual void StartWatchResource(string path)
        {
            this.EnsureTargetFileAccessible(path);
            FileAttributes fileAttribute = File.GetAttributes(path);

            if ((fileAttribute & FileAttributes.Directory) == FileAttributes.Directory)
            {
                path = path.TrimEnd('\\') + "\\";
            }

            // remember to check security on the folder to watch
            _resourceWatcher = new FileSystemWatcherEx();
            _resourceWatcher.Path = Path.GetDirectoryName(path);

            // just monitor files whose extension is xml only
            _resourceWatcher.Filter = "*.xml";
            _resourceWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.Size;

            // 2s is recent change events
            _resourceWatcher.Interval = 2000;

            // Add event handler
            _resourceWatcher.Changed += new FileSystemEventHandler(this.ResourceChangedEvent);
            _resourceWatcher.Created += new FileSystemEventHandler(this.ResourceChangedEvent);
            _resourceWatcher.Deleted += new FileSystemEventHandler(this.ResourceChangedEvent);
            _resourceWatcher.IncludeSubdirectories = false;

            // Enable watching.
            _resourceWatcher.EnableRaisingEvents = true;
        }

        /// <summary>
        /// Check file is accessible.
        /// </summary>
        /// <param name="path">Valid path.</param>
        protected void EnsureTargetFileAccessible(string path)
        {
            FileIOPermission permission = new FileIOPermission(FileIOPermissionAccess.Read, path);
            permission.Demand();
        }

        /// <summary>
        /// Resource change - reload new resource and add to cache.
        /// </summary>
        /// <param name="sender">File change.</param>
        /// <param name="e">File System Event.</param>
        protected void ResourceChangedEvent(object sender, FileSystemEventArgs e)
        {
            this.AddDataSourceToCache(ResourcePath);
        }

        /// <summary>
        /// Get all database setting configuration from the data source and than add to cache.
        /// </summary>
        /// <param name="resourcePath">Path to resource file for folder.</param>
        protected void AddDataSourceToCache(string resourcePath)
        {
            if (_databaseSettingDataSource == null)
            {
                lock (_syncRoot)
                {
                    if (_databaseSettingDataSource == null)
                    {
                        string[] listOfResources = this.GetResourceFiles(resourcePath);

                        _databaseSettingDataSource = GetSettingDataSource(listOfResources);

                        LogService.Info("_dbSettingDataSource: " + _databaseSettingDataSource.Count);
                    }

                    ////this.CacheManager.Set(DbSettingCacheKeyName, settingDataSource, this.GetCachePolicy());
                }
            }
        }

        /// <summary>
        /// Gets the cache policy.
        /// </summary>
        /// <returns>Return CacheItemPolicy.</returns>
        protected CacheItemPolicy GetCachePolicy()
        {
            return null;
        }

        #endregion protected functions

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _resourceWatcher.Dispose();
            }
        }

        #region private functions

        /// <summary>
        /// Gets the setting data source.
        /// </summary>
        /// <param name="listOfResources">The list of resources.</param>
        /// <returns>Setting data sources.</returns>
        /// <exception cref="System.Xml.Schema.XmlSchemaValidationException">Validate setting file.</exception>
        private Dictionary<string, IDbSetting> GetSettingDataSource(string[] listOfResources)
        {
            Dictionary<string, IDbSetting> settingDataSource = new Dictionary<string, IDbSetting>();
            int resourceLength = listOfResources.Length;
            for (int i = 0; i < resourceLength; i++)
            {
                IXmlLineInfo line = null;
                try
                {
                    using (FileStream fileStream = new FileStream(listOfResources[i], FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        using (StreamReader streamReader = new StreamReader(fileStream))
                        {
                            var dataSetting = streamReader.ReadToEnd();
                            var document = XDocument.Parse(dataSetting, LoadOptions.SetLineInfo);
                            var functions = document.Descendants("Function");

                            foreach (var functionItem in functions)
                            {
                                var settingList = functionItem.Descendants("Setting");

                                foreach (var setting in settingList)
                                {
                                    line = ((IXmlLineInfo)setting).HasLineInfo() ? ((IXmlLineInfo)setting) : null;
                                    var databaseSetting = this.GetDbSetting(setting);
                                    var key = setting.Attribute("Key").Value.Trim();

                                    if (!string.IsNullOrWhiteSpace(key)
                                        && databaseSetting.IsValid()
                                        && !settingDataSource.ContainsKey(key))
                                    {
                                        settingDataSource.Add(key, databaseSetting);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (!ReferenceEquals(line, null))
                    {
                        throw new XmlSchemaValidationException(listOfResources[i], ex, line.LineNumber, line.LinePosition);
                    }

                    throw;
                }
            }

            return settingDataSource;
        }

        /// <summary>
        /// Gets the database setting from XElement.
        /// </summary>
        /// <param name="setting">The XElement setting.</param>
        /// <returns>Database setting object.</returns>
        private DbSetting GetDbSetting(XElement setting)
        {
            var databaseSetting = new DbSetting
            {
                SpName = setting.Attribute("SpName").Value.Trim(),
                DbName = setting.Attribute("DbName").Value.Trim(),
                ExecuteTimeOut = setting.Attribute("TimeOut") != null ? Convert.ToInt32(setting.Attribute("TimeOut").Value.Trim()) : TimeOutDefault
            };

            // Add custom column for SpName
            var customColumList = setting.Descendants("Mapping");
            Hashtable customMapping = new Hashtable();
            foreach (var item in customColumList)
            {
                string dataField = item.Attribute("DataField").Value.Trim();
                string objectProperty = item.Attribute("ObjectProperty").Value.Trim();
                string obj = item.Attribute("Class") != null ? item.Attribute("Class").Value : string.Empty;
                customMapping.Add(string.Format("{0};{1}", dataField, obj), objectProperty);
            }

            databaseSetting.CustomMappingColumns = customMapping;

            // check connection string whether require to decrypt or not
            var isEncryptedConn = IsEncryptedConnection(setting);
            databaseSetting.ConnectionString = ConnectionStringGetter.GetConnectionString(databaseSetting.DbName, isEncryptedConn);

            return databaseSetting;
        }

        /// <summary>
        /// Gets the resource files.
        /// </summary>
        /// <param name="resourcePath">The resource path.</param>
        /// <returns>Array resource file.</returns>
        private string[] GetResourceFiles(string resourcePath)
        {
            string[] listOfResouces;

            if (IsFolder(resourcePath))
            {
                // just get xml file
                listOfResouces = Directory.GetFiles(resourcePath);
                listOfResouces = listOfResouces.ToList<string>()
                     .Where(path
                         => Path.GetExtension(path).Equals(".xml", StringComparison.OrdinalIgnoreCase))
                     .Select(s => s).ToArray<string>();
            }
            else
            {
                listOfResouces = new string[] { resourcePath };
            }

            return listOfResouces;
        }

        #endregion private functions
    }
}