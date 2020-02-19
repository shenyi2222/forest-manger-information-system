﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using FineUIPro;
using System.Linq;
using Dapper;

namespace AppBoxPro.Dapper.admin
{
    public partial class profile : PageBase
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

        }

        #endregion

        #region Events

        protected void btnSave_OnClick(object sender, EventArgs e)
        {
            // 检查当前密码是否正确
            string oldPass = tbxOldPassword.Text.Trim();
            string newPass = tbxNewPassword.Text.Trim();
            string confirmNewPass = tbxConfirmNewPassword.Text.Trim();

            if (newPass != confirmNewPass)
            {
                tbxConfirmNewPassword.MarkInvalid("确认密码和新密码不一致！");
                return;
            }


            User user = DB.QuerySingleOrDefault<User>("select * from users where Name = @UserName", new { UserName = GetIdentityName() });

            if (user != null)
            {
                if (!PasswordUtil.ComparePasswords(user.Password, oldPass))
                {
                    tbxOldPassword.MarkInvalid("当前密码不正确！");
                    return;
                }

                user.Password = PasswordUtil.CreateDbPassword(newPass);

                ExecuteUpdate<User>(user, "Password");

                Alert.ShowInTop("修改密码成功！");
            }

        }

        #endregion

    }
}
