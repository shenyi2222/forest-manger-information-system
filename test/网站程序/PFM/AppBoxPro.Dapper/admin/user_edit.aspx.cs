using System;

using System.Linq;
using FineUIPro;
using Dapper;
using System.Transactions;
using System.Collections.Generic;

namespace AppBoxPro.Dapper.admin
{
    public partial class user_edit : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreUserEdit";
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

        private User GetCurrentUser(int userID)
        {
            return DB.QuerySingleOrDefault<User>("select users.*, depts.Name UserDeptName from users left join depts on users.DeptID = depts.ID where users.ID = @UserID", new { UserID = userID });
        }

        private void LoadData()
        {
            btnClose.OnClientClick = ActiveWindow.GetHideReference();

            int id = GetQueryIntValue("id");
            User current = GetCurrentUser(id);
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

            labName.Text = current.Name;
            tbxRealName.Text = current.ChineseName;
            tbxEmail.Text = current.Email;
            tbxCellPhone.Text = current.CellPhone;
            tbxOfficePhone.Text = current.OfficePhone;
            tbxAddress.Text = current.Address;
            tbxRemark.Text = current.Remark;
            cbxEnabled.Checked = current.Enabled;
            ddlGender.SelectedValue = current.Gender;

            // 初始化用户所属角色
            InitUserRole(current);

            // 初始化用户所属部门
            InitUserDept(current);

            // 初始化用户所属职称
            InitUserTitle(current);
        }

        #region InitUserDept

        private void InitUserDept(User current)
        {
            if (current.DeptID != null)
            {
                tbSelectedDept.Text = current.UserDeptName;
                hfSelectedDept.Text = current.DeptID.ToString();
            }
        }

        #endregion

        #region InitUserRole

        private void InitUserRole(User current)
        {
            var roles = DB.Query<Role>("select * from roles inner join roleusers on roles.ID = roleusers.RoleID where roleusers.UserID = @UserID", new { UserID = current.ID });

            tbSelectedRole.Text = string.Join(",", roles.Select(u => u.Name).ToArray());
            hfSelectedRole.Text = string.Join(",", roles.Select(u => u.ID).ToArray());

        }
        #endregion

        #region InitUserTitle

        private void InitUserTitle(User current)
        {
            var titles = DB.Query<Title>("select * from titles inner join titleusers on titles.ID = titleusers.TitleID where titleusers.UserID = @UserID", new { UserID = current.ID });

            tbSelectedTitle.Text = string.Join(",", titles.Select(u => u.Name).ToArray()); ;
            hfSelectedTitle.Text = string.Join(",", titles.Select(u => u.ID).ToArray()); ;

        }
        #endregion


        #endregion

        #region Events

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            int userID = GetQueryIntValue("id");

            User item = GetCurrentUser(userID);
            item.ChineseName = tbxRealName.Text.Trim();
            item.Gender = ddlGender.SelectedValue;
            item.Email = tbxEmail.Text.Trim();
            item.CellPhone = tbxCellPhone.Text.Trim();
            item.OfficePhone = tbxOfficePhone.Text.Trim();
            item.Address = tbxAddress.Text.Trim();
            item.Remark = tbxRemark.Text.Trim();
            item.Enabled = cbxEnabled.Checked;


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
                // 更新用户
                ExecuteUpdate<User>(DB, item);

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

            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }

        #endregion

    }
}
