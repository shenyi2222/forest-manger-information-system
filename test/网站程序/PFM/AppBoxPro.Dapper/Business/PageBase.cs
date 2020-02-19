using System;
using System.Web.Security;
using System.Web.UI;
using System.Reflection;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Linq;
using System.Web;


using FineUIPro;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;

using Dapper;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using Npgsql;

namespace AppBoxPro.Dapper
{
    public class PageBase : System.Web.UI.Page
    {
        #region 只读静态变量

        // Session key
        private static readonly string SK_ONLINE_UPDATE_TIME = "OnlineUpdateTime";
        //private static readonly string SK_USER_ROLE_ID = "UserRoleId";

        private static readonly string CHECK_POWER_FAIL_PAGE_MESSAGE = "您无权访问此页面！";
        private static readonly string CHECK_POWER_FAIL_ACTION_MESSAGE = "您无权进行此操作！";


        #endregion

        #region 浏览权限

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public virtual string ViewPower
        {
            get
            {
                return string.Empty;
            }
        }

        #endregion

        #region 页面初始化

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // 此用户是否有访问此页面的权限
            if (!CheckPowerView())
            {
                CheckPowerFailWithPage();
                return;
            }

            // 设置主题
            if (PageManager.Instance != null)
            {
                var pm = PageManager.Instance;
                var themeValue = ConfigHelper.Theme;
                // 是否为内置主题
                if (IsSystemTheme(themeValue))
                {
                    pm.CustomTheme = string.Empty;
                    pm.Theme = (Theme)Enum.Parse(typeof(Theme), themeValue, true);
                }
                else
                {
                    pm.CustomTheme = themeValue;
                }
            }

            UpdateOnlineUser(GetIdentityID());

            // 设置页面标题
            Page.Title = ConfigHelper.Title;

            // 禁用表单的自动完成功能
            Form.Attributes["autocomplete"] = "off";
        }

        private bool IsSystemTheme(string themeName)
        {
            themeName = themeName.ToLower();
            string[] themes = Enum.GetNames(typeof(Theme));
            foreach (string theme in themes)
            {
                if (theme.ToLower() == themeName)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region 请求参数

        /// <summary>
        /// 获取查询字符串中的参数值
        /// </summary>
        protected string GetQueryValue(string queryKey)
        {
            return Request.QueryString[queryKey];
        }


        /// <summary>
        /// 获取查询字符串中的参数值
        /// </summary>
        protected int GetQueryIntValue(string queryKey)
        {
            int queryIntValue = -1;
            try
            {
                queryIntValue = Convert.ToInt32(Request.QueryString[queryKey]);
            }
            catch (Exception)
            {
                // TODO
            }

            return queryIntValue;
        }

        #endregion

        #region 表格相关

        protected int GetSelectedDataKeyID(Grid grid)
        {
            int id = -1;
            int rowIndex = grid.SelectedRowIndex;
            if (rowIndex >= 0)
            {
                id = Convert.ToInt32(grid.DataKeys[rowIndex][0]);
            }
            return id;
        }

        protected string GetSelectedDataKey(Grid grid, int dataIndex)
        {
            string data = string.Empty;
            int rowIndex = grid.SelectedRowIndex;
            if (rowIndex >= 0)
            {
                data = grid.DataKeys[rowIndex][dataIndex].ToString();
            }
            return data;
        }

        /// <summary>
        /// 获取表格选中项DataKeys的第一个值，并转化为整型列表
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        protected List<int> GetSelectedDataKeyIDs(Grid grid)
        {
            List<int> ids = new List<int>();
            foreach (int rowIndex in grid.SelectedRowIndexArray)
            {
                ids.Add(Convert.ToInt32(grid.DataKeys[rowIndex][0]));
            }

            return ids;
        }

        #endregion

        #region 在线用户相关

        protected void UpdateOnlineUser(int? userID)
        {
            if (userID == null)
            {
                return;
            }

            DateTime now = DateTime.Now;
            object lastUpdateTime = Session[SK_ONLINE_UPDATE_TIME];
            if (lastUpdateTime == null || (Convert.ToDateTime(lastUpdateTime).Subtract(now).TotalMinutes > 5))
            {
                // 记录本次更新时间
                Session[SK_ONLINE_UPDATE_TIME] = now;

                Online online = DB.QueryFirstOrDefault<Online>("select * from onlines where UserID = @UserID", new { UserID = userID });

                if (online != null)
                {
                    DB.Execute("update onlines set UpdateTime = @UpdateTime where UserID = @UserID", new { UpdateTime = now, UserID = userID });
                }

            }
        }

        protected void RegisterOnlineUser(User user)
        {
            DateTime now = DateTime.Now;

            Online online = DB.QueryFirstOrDefault<Online>("select * from onlines where UserID = @UserID", new { UserID = user.ID });

            // 如果不存在，就创建一条新的记录
            var isNew = false;
            if (online == null)
            {
                isNew = true;
                online = new Online();
            }
            online.UserID = user.ID;
            online.IPAdddress = Request.UserHostAddress;
            online.LoginTime = now;
            online.UpdateTime = now;


            if (isNew)
            {
                ExecuteInsert<Online>(online, "UserID", "IPAdddress", "LoginTime", "UpdateTime");
                //DB.Execute("insert onlines (UserID, IPAdddress, LoginTime, UpdateTime) values (@UserID, @IPAdddress, @LoginTime, @UpdateTime)", online);
            }
            else
            {
                ExecuteUpdate<Online>(online, "UserID", "IPAdddress", "LoginTime", "UpdateTime");
                //DB.Execute("update onlines set IPAdddress = @IPAdddress, LoginTime = @LoginTime, UpdateTime = @UpdateTime where UserID = @UserID", online);
            }


            // 记录本次更新时间
            Session[SK_ONLINE_UPDATE_TIME] = now;

        }

        /// <summary>
        /// 在线人数
        /// </summary>
        /// <returns></returns>
        protected int GetOnlineCount()
        {
            DateTime lastM = DateTime.Now.AddMinutes(-15);

            return DB.Execute("select count(*) from onlines where UpdateTime > @LastUpdateTime", new { LastUpdateTime = lastM });

        }

        #endregion

        #region 当前登录用户信息

        // http://blog.163.com/zjlovety@126/blog/static/224186242010070024282/
        // http://www.cnblogs.com/gaoshuai/articles/1863231.html
        /// <summary>
        /// 当前登录用户的角色列表
        /// </summary>
        /// <returns></returns>
        protected List<int> GetIdentityRoleIDs()
        {
            List<int> roleIDs = new List<int>();

            if (User.Identity.IsAuthenticated)
            {
                FormsAuthenticationTicket ticket = ((FormsIdentity)User.Identity).Ticket;
                string userData = ticket.UserData;

                foreach (string roleID in userData.Split(','))
                {
                    if (!string.IsNullOrEmpty(roleID))
                    {
                        roleIDs.Add(Convert.ToInt32(roleID));
                    }
                }
            }

            return roleIDs;
        }

        /// <summary>
        /// 当前登录用户名
        /// </summary>
        /// <returns></returns>
        protected string GetIdentityName()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return null;
            }

            var identityName = User.Identity.Name;
            var firstUnderlineIndex = identityName.IndexOf('_');
            return identityName.Substring(firstUnderlineIndex + 1);
        }

        /// <summary>
        /// 当前登录用户标识符
        /// </summary>
        /// <returns></returns>
        protected int? GetIdentityID()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return null;
            }

            var identityName = User.Identity.Name;
            var firstUnderlineIndex = identityName.IndexOf('_');
            if (firstUnderlineIndex > 0)
            {
                return Convert.ToInt32(identityName.Substring(0, firstUnderlineIndex));
            }

            return null;
        }


        /// <summary>
        /// 创建表单验证的票证并存储在客户端Cookie中
        /// </summary>
        /// <param name="userID">当前登录用户标识符</param>
        /// <param name="userName">当前登录用户名</param>
        /// <param name="roleIDs">当前登录用户的角色标识符列表</param>
        /// <param name="isPersistent">是否跨浏览器会话保存票证</param>
        /// <param name="expiration">过期时间</param>
        protected void CreateFormsAuthenticationTicket(int userID, string userName, string roleIDs, bool isPersistent, DateTime expiration)
        {
            // 创建Forms身份验证票据
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                userID + "_" + userName,        // 与票证关联的用户
                DateTime.Now,                   // 票证发出时间
                expiration,                     // 票证过期时间
                isPersistent,                   // 如果票证将存储在持久性 Cookie 中（跨浏览器会话保存），则为 true；否则为 false。
                roleIDs                         // 存储在票证中的用户特定的数据
             );

            // 对Forms身份验证票据进行加密，然后保存到客户端Cookie中
            string hashTicket = FormsAuthentication.Encrypt(ticket);
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hashTicket);
            cookie.HttpOnly = true;
            // 1. 关闭浏览器即删除（Session Cookie）：DateTime.MinValue
            // 2. 指定时间后删除：大于 DateTime.Now 的某个值
            // 3. 删除Cookie：小于 DateTime.Now 的某个值
            if (isPersistent)
            {
                cookie.Expires = expiration;
            }
            else
            {
                cookie.Expires = DateTime.MinValue;
            }
            Response.Cookies.Add(cookie);
        }

        #endregion

        #region 权限检查

        /// <summary>
        /// 检查当前用户是否拥有当前页面的浏览权限
        /// 页面需要先定义ViewPower属性，以确定页面与某个浏览权限的对应关系
        /// </summary>
        /// <returns></returns>
        protected bool CheckPowerView()
        {
            return CheckPower(ViewPower);
        }

        /// <summary>
        /// 检查当前用户是否拥有某个权限
        /// </summary>
        /// <param name="powerType"></param>
        /// <returns></returns>
        protected bool CheckPower(string powerName)
        {
            // 如果权限名为空，则放行
            if (string.IsNullOrEmpty(powerName))
            {
                return true;
            }

            // 当前登陆用户的权限列表
            List<string> rolePowerNames = GetRolePowerNames();
            if (rolePowerNames.Contains(powerName))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 获取当前登录用户拥有的全部权限列表
        /// </summary>
        /// <param name="roleIDs"></param>
        /// <returns></returns>
        protected List<string> GetRolePowerNames()
        {
            // 将用户拥有的权限列表保存在Session中，这样就避免每个请求多次查询数据库
            if (Session["UserPowerList"] == null)
            {
                List<string> rolePowerNames = new List<string>();

                // 超级管理员拥有所有权限
                if (GetIdentityName() == "admin")
                {
                    rolePowerNames = DB.Query<string>("select Name from powers").ToList();

                }
                else
                {
                    List<int> roleIDs = GetIdentityRoleIDs();

                    string IDs = "";

                    if (roleIDs.Count > 0)
                    {
                        foreach (var item in roleIDs)
                        {
                            IDs += item.ToString() + ",";
                        }

                        IDs = IDs.Substring(0, IDs.Length - 1);

                        rolePowerNames = DB.Query<string>("select distinct powers.Name as PowerName from powers inner join rolepowers On powers.ID = rolepowers.PowerID and rolepowers.RoleID in (" + IDs + ")").ToList();
                    }
                    else
                    {
                        Alert.Show("当前用户为定义角色！", "错误警告", MessageBoxIcon.Error, "确定");
                    }
                }

                Session["UserPowerList"] = rolePowerNames;
            }
            return (List<string>)Session["UserPowerList"];
        }

        #endregion

        #region 权限相关

        protected void CheckPowerFailWithPage()
        {
            Response.Write(CHECK_POWER_FAIL_PAGE_MESSAGE);
            Response.End();
        }

        protected void CheckPowerFailWithButton(FineUIPro.Button btn)
        {
            btn.Enabled = false;
            btn.ToolTip = CHECK_POWER_FAIL_ACTION_MESSAGE;
        }

        protected void CheckPowerFailWithLinkButtonField(FineUIPro.Grid grid, string columnID)
        {
            FineUIPro.LinkButtonField btn = grid.FindColumn(columnID) as FineUIPro.LinkButtonField;
            btn.Enabled = false;
            btn.ToolTip = CHECK_POWER_FAIL_ACTION_MESSAGE;
        }

        protected void CheckPowerFailWithWindowField(FineUIPro.Grid grid, string columnID)
        {
            FineUIPro.WindowField btn = grid.FindColumn(columnID) as FineUIPro.WindowField;
            btn.Enabled = false;
            btn.ToolTip = CHECK_POWER_FAIL_ACTION_MESSAGE;
        }

        protected void CheckPowerFailWithAlert()
        {
            PageContext.RegisterStartupScript(Alert.GetShowInTopReference(CHECK_POWER_FAIL_ACTION_MESSAGE));
        }

        protected void CheckPowerWithButton(string powerName, FineUIPro.Button btn)
        {
            if (!CheckPower(powerName))
            {
                CheckPowerFailWithButton(btn);
            }
        }

        protected void CheckPowerWithLinkButtonField(string powerName, FineUIPro.Grid grid, string columnID)
        {
            if (!CheckPower(powerName))
            {
                CheckPowerFailWithLinkButtonField(grid, columnID);
            }
        }

        protected void CheckPowerWithWindowField(string powerName, FineUIPro.Grid grid, string columnID)
        {
            if (!CheckPower(powerName))
            {
                CheckPowerFailWithWindowField(grid, columnID);
            }
        }

        /// <summary>
        /// 为删除Grid中选中项的按钮添加提示信息
        /// </summary>
        /// <param name="btn"></param>
        /// <param name="grid"></param>
        protected void ResolveDeleteButtonForGrid(FineUIPro.Button btn, Grid grid)
        {
            ResolveDeleteButtonForGrid(btn, grid, "确定要删除选中的{0}项记录吗？");
        }

        protected void ResolveDeleteButtonForGrid(FineUIPro.Button btn, Grid grid, string confirmTemplate)
        {
            ResolveDeleteButtonForGrid(btn, grid, "请至少应该选择一项记录！", confirmTemplate);
        }

        protected void ResolveDeleteButtonForGrid(FineUIPro.Button btn, Grid grid, string noSelectionMessage, string confirmTemplate)
        {
            // 点击删除按钮时，至少选中一项
            btn.OnClientClick = grid.GetNoSelectionAlertInParentReference(noSelectionMessage);
            btn.ConfirmText = string.Format(confirmTemplate, "&nbsp;<span class=\"highlight\"><script>" + grid.GetSelectedCountReference() + "</script></span>&nbsp;");
            btn.ConfirmTarget = Target.Top;
        }

        #endregion

        #region 产品版本

        public string GetProductVersion()
        {
            Version v = Assembly.GetExecutingAssembly().GetName().Version;
            return string.Format("{0}.{1}.{2}", v.Major, v.Minor, v.Build);
        }

        #endregion

        #region 模拟树的下拉列表

        protected List<T> ResolveDDL<T>(List<T> mys) where T : ICustomTree, ICloneable, IKeyID, new()
        {
            return ResolveDDL<T>(mys, -1, true);
        }

        protected List<T> ResolveDDL<T>(List<T> mys, int currentId) where T : ICustomTree, ICloneable, IKeyID, new()
        {
            return ResolveDDL<T>(mys, currentId, true);
        }


        // 将一个树型结构放在一个下列列表中可供选择
        protected List<T> ResolveDDL<T>(List<T> source, int currentID, bool addRootNode) where T : ICustomTree, ICloneable, IKeyID, new()
        {
            List<T> result = new List<T>();

            if (addRootNode)
            {
                // 添加根节点
                T root = new T();
                root.Name = "--根节点--";
                root.ID = -1;
                root.TreeLevel = 0;
                root.Enabled = true;
                result.Add(root);
            }

            foreach (T item in source)
            {
                T newT = (T)item.Clone();
                result.Add(newT);

                // 所有节点的TreeLevel加一
                if (addRootNode)
                {
                    newT.TreeLevel++;
                }
            }

            // currentId==-1表示当前节点不存在
            if (currentID != -1)
            {
                // 本节点不可点击（也就是说当前节点不可能是当前节点的父节点）
                // 并且本节点的所有子节点也不可点击，你想如果当前节点跑到子节点的子节点，那么这些子节点就从树上消失了
                bool startChileNode = false;
                int startTreeLevel = 0;
                foreach (T my in result)
                {
                    if (my.ID == currentID)
                    {
                        startTreeLevel = my.TreeLevel;
                        my.Enabled = false;
                        startChileNode = true;
                    }
                    else
                    {
                        if (startChileNode)
                        {
                            if (my.TreeLevel > startTreeLevel)
                            {
                                my.Enabled = false;
                            }
                            else
                            {
                                startChileNode = false;
                            }
                        }
                    }
                }
            }

            return result;
        }

        #endregion

        #region 日志记录

        protected void LogInfo(string message)
        {
            var log = new Log
            {
                Level = "Info",
                Message = message,
                LogTime = DateTime.Now
            };

            ExecuteInsert<Log>(log);
        }

        #endregion

        #region Dapper

        /// <summary>
        /// 每个请求共享一个数据库连接实例
        /// </summary>
        public static IDbConnection DB
        {
            get
            {
                // http://stackoverflow.com/questions/6334592/one-dbcontext-per-request-in-asp-net-mvc-without-ioc-container
                if (!HttpContext.Current.Items.Contains("__AppBoxProContext"))
                {
                    HttpContext.Current.Items["__AppBoxProContext"] = GetDbConnection();
                }
                return HttpContext.Current.Items["__AppBoxProContext"] as IDbConnection;
            }
        }


        /// <summary>
        /// 数据库连接实例
        /// </summary>
        /// <returns></returns>
        public static IDbConnection GetDbConnection()
        {
            var database = ConfigurationManager.AppSettings["Database"];

            var connectionStringSection = ConfigurationManager.ConnectionStrings[database];
            var connectionString = connectionStringSection.ToString();

            IDbConnection connection;
            if (connectionStringSection.ProviderName.StartsWith("MySql"))
            {
                connection = new MySqlConnection(connectionString);
            }
            if (connectionStringSection.ProviderName.StartsWith("Npgsql"))
            {
                connection = new NpgsqlConnection(connectionString);
            }
            else
            {
                connection = new SqlConnection(connectionString);
            }

            // 打开数据库连接
            connection.Open();

            return connection;
        }

        /// <summary>
        /// 获取实例的属性名称列表
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        private string[] GetReflectionProperties(object instance)
        {
            var result = new List<string>();
            foreach (PropertyInfo property in instance.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                var propertyName = property.Name;
                // NotMapped特性
                var notMappedAttr = property.GetCustomAttribute<NotMappedAttribute>(false);
                if (notMappedAttr == null && propertyName != "ID")
                {
                    result.Add(propertyName);
                }
            }
            return result.ToArray();
        }


        /// <summary>
        /// 执行数据库更新操作
        /// </summary>
        /// <param name="instance">模型实例</param>
        /// <param name="fields">更新的表字段</param>
        /// <returns></returns>
        protected int ExecuteUpdate<T>(object instance, params string[] fields)
        {
            return ExecuteUpdate<T>(DB, instance, fields);
        }

        /// <summary>
        /// 执行数据库更新操作
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="instance"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        protected int ExecuteUpdate<T>(IDbConnection conn, object instance, params string[] fields)
        {
            // 约定：类型 User 对应的数据库表名 users
            string tableName = typeof(T).Name.ToLower() + "s";

            if (fields.Length == 0)
            {
                fields = GetReflectionProperties(instance);
            }

            var fieldsSql = string.Join(",", fields.Select(field => field + " = @" + field));

            var sql = string.Format("update {0} set {1} where ID = @ID", tableName, fieldsSql);

            return conn.Execute(sql, instance);
        }


        protected int ExecuteInsert<T>(object instance, params string[] fields)
        {
            return ExecuteInsert<T>(DB, instance, fields);
        }

        /// <summary>
        /// 执行数据库插入操作
        /// </summary>
        /// <param name="instance">模型实例</param>
        /// <param name="tableName">模型对应的表名</param>
        /// <param name="fields">插入的表字段</param>
        /// <returns>新插入的行ID</returns>
        protected int ExecuteInsert<T>(IDbConnection conn, object instance, params string[] fields)
        {
            // 约定：类型 User 对应的数据库表名 users
            string tableName = typeof(T).Name.ToLower() + "s";

            if (fields.Length == 0)
            {
                fields = GetReflectionProperties(instance);
            }

            var fieldsSql1 = string.Join(",", fields);
            var fieldsSql2 = string.Join(",", fields.Select(field => "@" + field));

            var sql = "";

            if (conn is NpgsqlConnection)
            {
                sql = string.Format("insert into {0} ({1}) values ({2})", tableName, fieldsSql1, fieldsSql2); ///更改为ISO标准SQL，去掉末尾引号已支持returning id
            }
            else
            {
                sql = string.Format("insert into {0} ({1}) values ({2});", tableName, fieldsSql1, fieldsSql2);
            }

            if (conn is MySqlConnection)
            {
                sql += "select last_insert_id();";
            }
            if (conn is NpgsqlConnection)
            {
                sql += "returning id";
            }
            else
            {
                sql += "SELECT @@IDENTITY;";
            }

            return conn.QuerySingle<int>(sql, instance);
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="grid"></param>
        /// <returns></returns>
        protected IEnumerable<T> Sort<T>(FineUIPro.Grid grid)
        {
            return Sort<T>(DB, new WhereBuilder(), grid);
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <param name="grid"></param>
        /// <returns></returns>
        protected IEnumerable<T> Sort<T>(WhereBuilder builder, FineUIPro.Grid grid)
        {
            return Sort<T>(DB, builder, grid);
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="builder"></param>
        /// <param name="grid"></param>
        /// <returns></returns>
        protected IEnumerable<T> Sort<T>(IDbConnection conn, WhereBuilder builder, FineUIPro.Grid grid)
        {
            // sql: users
            // sql: select * from users
            // sql: select onlines.*, users.Name UserName from onlines inner join users on users.ID = onlines.UserID
            var sql = builder.FromSql;
            if (string.IsNullOrEmpty(sql))
            {
                // 约定：类型 User 对应的数据库表名 users
                sql = typeof(T).Name.ToLower() + "s";
            }

            if (!sql.StartsWith("select"))
            {
                sql = "select * from " + sql;
            }

            if (builder.Wheres.Count > 0)
            {
                sql += " where " + string.Join(" and ", builder.Wheres);
            }

            sql += " order by " + grid.SortField + " " + grid.SortDirection;

            return conn.Query<T>(sql, builder.Parameters);
        }


        /// <summary>
        /// 排序和分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <param name="grid"></param>
        /// <returns></returns>
        protected IEnumerable<T> SortAndPage<T>(WhereBuilder builder, FineUIPro.Grid grid)
        {
            return SortAndPage<T>(DB, builder, grid);
        }

        /// <summary>
        /// 排序和分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="builder"></param>
        /// <param name="grid"></param>
        /// <returns></returns>
        protected IEnumerable<T> SortAndPage<T>(IDbConnection conn, WhereBuilder builder, FineUIPro.Grid grid)
        {
            // sql: users
            // sql: select * from users
            // sql: select onlines.*, users.Name UserName from onlines inner join users on users.ID = onlines.UserID

            var sql = builder.FromSql;
            if (string.IsNullOrEmpty(sql))
            {
                // 约定：类型 User 对应的数据库表名 users
                sql = typeof(T).Name.ToLower() + "s";
            }

            if (!sql.StartsWith("select"))
            {
                sql = "select * from " + sql;
            }

            if (builder.Wheres.Count > 0)
            {
                sql += " where " + string.Join(" and ", builder.Wheres);
            }

            sql += " order by " + grid.SortField + " " + grid.SortDirection;

            // 分页
            if (conn is MySqlConnection)
            {
                sql += " limit @PageStartIndex, @PageSize";
            }
            else
            {
                sql += " OFFSET @PageStartIndex ROWS FETCH NEXT @PageSize ROWS ONLY";
            }

            builder.Parameters.Add("PageSize", grid.PageSize);
            builder.Parameters.Add("PageStartIndex", grid.PageSize * grid.PageIndex);


            return conn.Query<T>(sql, builder.Parameters);
        }

        /// <summary>
        /// 获取总记录数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        protected int Count<T>(WhereBuilder builder)
        {
            return Count<T>(DB, builder);
        }

        /// <summary>
        /// 获取总记录数
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        protected int Count<T>(IDbConnection conn, WhereBuilder builder)
        {
            var sql = builder.FromSql;
            if (string.IsNullOrEmpty(sql))
            {
                // 约定：类型 User 对应的数据库表名 users
                sql = typeof(T).Name.ToLower() + "s";
            }

            sql = "select count(*) from " + sql;

            if (builder.Wheres.Count > 0)
            {
                sql += " where " + string.Join(" and ", builder.Wheres);
            }

            return conn.QuerySingleOrDefault<int>(sql, builder.Parameters);
        }

        /// <summary>
        /// 检索对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="paramID"></param>
        /// <returns></returns>
        protected T FindByID<T>(int paramID)
        {
            return FindByID<T>(DB, paramID);
        }

        /// <summary>
        /// 检索对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="paramID"></param>
        /// <returns></returns>
        protected T FindByID<T>(IDbConnection conn, int paramID)
        {
            // 约定：类型 User 对应的数据库表名 users
            var tableName = typeof(T).Name.ToLower() + "s";

            return conn.QuerySingleOrDefault<T>("select * from " + tableName + " where ID = @ParamID", new { ParamID = paramID });
        }

        #endregion

    }

}
