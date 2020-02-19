using System;
using System.Collections.Generic;
using System.Web;
using System.Linq;
using Dapper;

namespace AppBoxPro.Dapper
{
    public class ConfigHelper
    {
        #region fields & constructor

        private static IList<Config> _configs;

        public static IList<Config> Configs
        {
            get
            {
                if (_configs == null)
                {
                    InitConfigs();
                }
                return _configs;
            }
        }

        private static IList<string> changedKeys = new List<string>();

        public static void Reload()
        {
            _configs = null;
        }

        public static void InitConfigs()
        {
            _configs = PageBase.DB.Query<Config>("select * from configs").ToList();

        }

        #endregion

        #region methods

        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetValue(string key)
        {
            return Configs.Where(c => c.ConfigKey == key).Select(c => c.ConfigValue).FirstOrDefault();
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetValue(string key, string value)
        {
            Config config = Configs.Where(c => c.ConfigKey == key).FirstOrDefault();
            if (config != null)
            {
                if (config.ConfigValue != value)
                {
                    changedKeys.Add(key);
                    config.ConfigValue = value;
                }
            }
        }

        /// <summary>
        /// 保存所有更改的配置项
        /// </summary>
        public static void SaveAll()
        {
            PageBase.DB.Execute("update configs set ConfigValue = @ConfigValue where ConfigKey = @ConfigKey",
                new[] { 
                    new { ConfigKey = "Title", ConfigValue = Title }, 
                    new { ConfigKey = "PageSize", ConfigValue = PageSize.ToString() }, 
                    new { ConfigKey = "Theme", ConfigValue = Theme }, 
                    new { ConfigKey = "HelpList", ConfigValue = HelpList },
                    new { ConfigKey = "MenuType", ConfigValue = MenuType }
                });

            Reload();
        }

        #endregion

        #region properties

        /// <summary>
        /// 网站标题
        /// </summary>
        public static string Title
        {
            get
            {
                return GetValue("Title");
            }
            set
            {
                SetValue("Title", value);
            }
        }

        /// <summary>
        /// 列表每页显示的个数
        /// </summary>
        public static int PageSize
        {
            get
            {
                return Convert.ToInt32(GetValue("PageSize"));
            }
            set
            {
                SetValue("PageSize", value.ToString());
            }
        }

        /// <summary>
        /// 帮助下拉列表
        /// </summary>
        public static string HelpList
        {
            get
            {
                return GetValue("HelpList");
            }
            set
            {
                SetValue("HelpList", value);
            }
        }


        /// <summary>
        /// 菜单样式
        /// </summary>
        public static string MenuType
        {
            get
            {
                return GetValue("MenuType");
            }
            set
            {
                SetValue("MenuType", value);
            }
        }


        /// <summary>
        /// 网站主题
        /// </summary>
        public static string Theme
        {
            get
            {
                return GetValue("Theme");
            }
            set
            {
                SetValue("Theme", value);
            }
        }


        #endregion
    }
}
