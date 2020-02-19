using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;

using FineUIPro;
using System.Transactions;
using Dapper;


namespace AppBoxPro.Dapper.admin
{
    public partial class user_new : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreUserNew";
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

            // 初始化用户所属角色
            InitUserRole();

            // 初始化用户所属部门
            InitUserDept();

            // 初始化用户所属职称
            InitUserTitle();
        }

        #region InitUserRole

        private void InitUserDept()
        {


        }

        #endregion

        #region InitUserRole

        private void InitUserRole()
        {


        }
        #endregion

        #region InitUserJobTitle

        private void InitUserTitle()
        {


        }
        #endregion

        #endregion

        #region Events


        private void SaveItem()
        {
            User item = new User();
            item.Name = tbxName.Text.Trim();
            item.Password = PasswordUtil.CreateDbPassword(tbxPassword.Text.Trim());
            item.ChineseName = tbxRealName.Text.Trim();
            item.Gender = ddlGender.SelectedValue;
            item.Email = tbxEmail.Text.Trim();
            item.OfficePhone = tbxOfficePhone.Text.Trim();
            item.CellPhone = tbxCellPhone.Text.Trim();
            item.Address = tbxAddress.Text.Trim();
            item.Remark = tbxRemark.Text.Trim();
            item.Enabled = cbxEnabled.Checked;
            item.CreateTime = DateTime.Now;

            if (string.IsNullOrEmpty(hfSelectedDept.Text))
            {
                item.DeptID = null;
            }
            else
            {
                item.DeptID = Convert.ToInt32(hfSelectedDept.Text);
            }

            using (var transactionScope = new TransactionScope())
            {
                // 插入用户
                var userID = ExecuteInsert<User>(DB, item);

                // 更新用户所属角色
                int[] roleIDs = StringUtil.GetIntArrayFromString(hfSelectedRole.Text);
                DB.Execute("delete from roleusers where UserID = @UserID", new { UserID = userID });
                DB.Execute("insert into roleusers (UserID, RoleID) values (@UserID, @RoleID)", roleIDs.Select(u => new { UserID = userID, RoleID = u }).ToList());

                // 更新用户所属职务
                int[] titleIDs = StringUtil.GetIntArrayFromString(hfSelectedTitle.Text);
                DB.Execute("delete from titleusers where UserID = @UserID", new { UserID = userID });
                DB.Execute("insert into titleusers (UserID, TitleID) values (@UserID, @TitleID)", titleIDs.Select(u => new { UserID = userID, TitleID = u }).ToList());


                transactionScope.Complete();
            }

        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            string inputUserName = tbxName.Text.Trim();

            Object user = DB.QuerySingleOrDefault("select * from users where Name = @UserName", new { UserName = inputUserName });

            if (user != null)
            {
                Alert.Show("用户 " + inputUserName + " 已经存在！");
                return;
            }


            SaveItem();

            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }
        #endregion

    }
}
