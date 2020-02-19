using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;

using FineUIPro;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Dapper;


namespace AppBoxPro.Dapper.admin
{
    public partial class dept_user_addnew : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreDeptUserNew";
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
            Dept current = FindByID<Dept>(id);
            if (current == null)
            {
                // 参数错误，首先弹出Alert对话框然后关闭弹出窗口
                Alert.Show("参数错误！", string.Empty, ActiveWindow.GetHideReference());
                return;
            }

            // 每页记录数
            Grid1.PageSize = ConfigHelper.PageSize;
            ddlGridPageSize.SelectedValue = ConfigHelper.PageSize.ToString();


            BindGrid();
        }


        private void BindGrid()
        {
            // 查询条件
            var builder = new WhereBuilder();

            string searchText = ttbSearchMessage.Text.Trim();
            if (!string.IsNullOrEmpty(searchText))
            {
                builder.AddWhere("(users.Name like @SearchText or users.ChineseName like @SearchText or users.EnglishName like @SearchText)");
                builder.AddParameter("SearchText", "%" + searchText + "%");
            }

            builder.AddWhere("users.Name != 'admin'");

            // 排除所有已经属于某个部门的用户
            builder.AddWhere("users.DeptID is null");

            // 获取总记录数（在添加条件之后，排序和分页之前）
            Grid1.RecordCount = Count<User>(builder);

            // 排列和数据库分页
            Grid1.DataSource = SortAndPage<User>(builder, Grid1);
            Grid1.DataBind();
        }

        #endregion

        #region Events

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            int deptID = GetQueryIntValue("id");

            // 跨页保持选中行
            string[] ids = Grid1.SelectedRowIDArray;

            string IDs = "";

            foreach (var item in ids)
            {
                IDs += Convert.ToInt32(item) + ",";
            }

            IDs = IDs.Substring(0, IDs.Length - 1);

            DB.Execute("update users set DeptID = " + deptID + " where ID in (" + IDs + ")");
            
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }



        protected void ttbSearchMessage_Trigger2Click(object sender, EventArgs e)
        {
            ttbSearchMessage.ShowTrigger1 = true;
            BindGrid();
        }

        protected void ttbSearchMessage_Trigger1Click(object sender, EventArgs e)
        {
            ttbSearchMessage.Text = string.Empty;
            ttbSearchMessage.ShowTrigger1 = false;
            BindGrid();
        }

        protected void Grid1_Sort(object sender, GridSortEventArgs e)
        {
            Grid1.SortDirection = e.SortDirection;
            Grid1.SortField = e.SortField;
            BindGrid();
        }

        protected void Grid1_PageIndexChange(object sender, GridPageEventArgs e)
        {
            Grid1.PageIndex = e.NewPageIndex;
            BindGrid();
        }


        protected void ddlGridPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Grid1.PageSize = Convert.ToInt32(ddlGridPageSize.SelectedValue);

            BindGrid();
        }

        #endregion


    }
}
