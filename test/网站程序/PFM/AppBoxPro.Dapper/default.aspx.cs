﻿using System;
using System.Web;
using System.Web.Security;

using FineUIPro;
using System.Text;
using System.Linq;
using Dapper;
using System.Collections.Generic;


namespace AppBoxPro.Dapper
{
    public partial class _default : PageBase
    {
        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        private void LoadData()
        {
            // 如果用户已经登录，则重定向到管理首页
            if (User.Identity.IsAuthenticated)
            {
                Response.Redirect(FormsAuthentication.DefaultUrl);
            }

            //Window1.Title = string.Format("AppBoxPro.Dapper v{0}", GetProductVersion());
            Window1.Title = string.Format("专业森林管家系统");


        }

        #endregion

        #region Events

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string userName = tbxUserName.Text.Trim();
            string password = tbxPassword.Text.Trim();

            User user = DB.QueryFirstOrDefault<User>("select * from users where Name = @Name", new { Name = userName });

            if (user != null)
            {
                if (PasswordUtil.ComparePasswords(user.Password, password))
                {
                    if (!user.Enabled)
                    {
                        Alert.Show("用户未启用，请联系管理员！");
                    }
                    else
                    {
                        // 登录成功
                        LoginSuccess(user);

                        return;
                    }
                }
                else
                {
                    Alert.Show("用户名或密码错误！");
                    return;
                }

            }
            else
            {
                Alert.Show("用户名或密码错误！");
                return;
            }

        }


        private void LoginSuccess(User user)
        {
            RegisterOnlineUser(user);

            // 用户所属的角色字符串，以逗号分隔
            string roleIDs = string.Empty;

            var roleIdList = DB.Query<int>("select RoleID from roleusers where UserID = @UserID", new { UserID = user.ID });
            if (roleIdList.Count() > 0)
            {
                roleIDs = string.Join(",", roleIdList);
            }



            bool isPersistent = false;
            DateTime expiration = DateTime.Now.AddMinutes(120);
            CreateFormsAuthenticationTicket(user.ID, user.Name, roleIDs, isPersistent, expiration);

            // 重定向到登陆后首页
            Response.Redirect(FormsAuthentication.DefaultUrl);
        }


        #endregion
    }
}
