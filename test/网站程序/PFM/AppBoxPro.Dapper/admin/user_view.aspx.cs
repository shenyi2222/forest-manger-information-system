using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FineUIPro;
using System.Text;
using System.Linq;

using Dapper;

namespace AppBoxPro.Dapper.admin
{
    public partial class user_view : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreUserView";
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

            labName.Text = current.Name;
            labRealName.Text = current.ChineseName;
            labEmail.Text = current.Email;
            labCellPhone.Text = current.CellPhone;
            labOfficePhone.Text = current.OfficePhone;
            labAddress.Text = current.Address;
            labRemark.Text = current.Remark;
            labEnabled.Text = current.Enabled ? "启用" : "禁用";
            labGender.Text = current.Gender;

            // 用户所属角色
            var roleNames = DB.Query<string>("select roles.Name from roleusers left join roles on roleusers.RoleID = roles.ID where roleusers.UserID = @UserID", new { UserID = id });
            labRole.Text = string.Join(",", roleNames.ToArray());


            // 用户的职称列表
            var titleNames = DB.Query<string>("select titles.Name from titleusers left join titles on titleusers.TitleID = titles.ID where titleusers.UserID = @UserID", new { UserID = id });
            labTitle.Text = string.Join(",", titleNames.ToArray());


            // 用户所属的部门
            if (current.DeptID != null)
            {
                var deptName = DB.QuerySingleOrDefault<string>("select depts.Name from depts where depts.ID = @DeptID", new { DeptID = current.DeptID });
                labDept.Text = deptName;
            }

        }

        #endregion

    }
}
