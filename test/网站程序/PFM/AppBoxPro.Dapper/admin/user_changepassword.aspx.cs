﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Linq;
using FineUIPro;
using Dapper;

namespace AppBoxPro.Dapper.admin
{
    public partial class user_changepassword : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreUserChangePassword";
            }
        }

        #endregion

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
            btnClose.OnClientClick = ActiveWindow.GetHideReference();

            int id = GetQueryIntValue("id");
            User current = FindByID<User>(id);

            if (current == null)
            {
                // 参数错误，首先弹出Alert对话框然后关闭弹出窗口
                Alert.Show("参数错误！", string.Empty, ActiveWindow.GetHideReference());
                return;
            }

            if (current.Name == "admin" && GetIdentityName() != "admin")
            {
                Alert.Show("你无权编辑超级管理员！", string.Empty, ActiveWindow.GetHideReference());
                return;
            }

            labUserName.Text = current.Name;
            labUserRealName.Text = current.ChineseName;
        }

        #endregion

        #region Events

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            int id = GetQueryIntValue("id");

            string newPass = tbxNewPassword.Text.Trim();
            string confirmNewPass = tbxConfirmPassword.Text.Trim();

            if (newPass != confirmNewPass)
            {
                tbxConfirmPassword.MarkInvalid("确认密码和新密码不一致！");
                return;
            }


            User current = FindByID<User>(id);

            if (current != null)
            {
                current.Password = PasswordUtil.CreateDbPassword(tbxNewPassword.Text.Trim());

                ExecuteUpdate<User>(current, "Password");
                //DB.Execute("update users set Password = @Password where ID = @ID", current);
            }


            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }
        #endregion

    }
}
