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
    public partial class dept_user : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreDeptUserView";
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
            // 权限检查
            CheckPowerWithButton("CoreDeptUserNew", btnNew);
            CheckPowerWithButton("CoreDeptUserDelete", btnDeleteSelected);


            ResolveDeleteButtonForGrid(btnDeleteSelected, Grid2, "确定要从当前部门移除选中的{0}项记录吗？");


            BindGrid1();

            // 默认选中第一个部门
            Grid1.SelectedRowIndex = 0;

            // 每页记录数
            Grid2.PageSize = ConfigHelper.PageSize;
            ddlGridPageSize.SelectedValue = ConfigHelper.PageSize.ToString();

            BindGrid2();
        }

        private void BindGrid1()
        {
            List<Dept> mys = DeptHelper.Depts;

            Grid1.DataSource = mys;
            Grid1.DataBind();
        }

        private void BindGrid2()
        {
            int deptID = GetSelectedDataKeyID(Grid1);

            if (deptID == -1)
            {
                Grid2.RecordCount = 0;

                Grid2.DataSource = null;
                Grid2.DataBind();
            }
            else
            {
                // 查询条件
                var builder = new WhereBuilder();


                string searchText = ttbSearchUser.Text.Trim();
                if (!string.IsNullOrEmpty(searchText))
                {
                    builder.AddWhere("(users.Name like @SearchText or users.ChineseName like @SearchText)");
                    builder.AddParameter("SearchText", "%" + searchText + "%");
                }

                builder.AddWhere("users.Name != 'admin'");

                // 过滤选中部门下的所有用户
                builder.AddWhere("users.DeptID = @DeptID");
                builder.AddParameter("DeptID", deptID);

                // 获取总记录数（在添加条件之后，排序和分页之前）
                Grid2.RecordCount = Count<User>(builder);

                // 排列和数据库分页
                Grid2.DataSource = SortAndPage<User>(builder, Grid2);
                Grid2.DataBind();

            }
        }


        #endregion

        #region Events

        protected void ddlGridPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Grid2.PageSize = Convert.ToInt32(ddlGridPageSize.SelectedValue);

            BindGrid2();
        }


        #endregion

        #region Grid1 Events

        protected void Grid1_RowClick(object sender, FineUIPro.GridRowClickEventArgs e)
        {
            BindGrid2();
        }

        #endregion

        #region Grid2 Events

        protected void ttbSearchUser_Trigger2Click(object sender, EventArgs e)
        {
            ttbSearchUser.ShowTrigger1 = true;
            BindGrid2();
        }

        protected void ttbSearchUser_Trigger1Click(object sender, EventArgs e)
        {
            ttbSearchUser.Text = string.Empty;
            ttbSearchUser.ShowTrigger1 = false;
            BindGrid2();
        }

        protected void Grid2_PreDataBound(object sender, EventArgs e)
        {
            // 数据绑定之前，进行权限检查
            CheckPowerWithLinkButtonField("CoreDeptUserDelete", Grid2, "deleteField");
        }

        protected void Grid2_Sort(object sender, GridSortEventArgs e)
        {
            Grid2.SortDirection = e.SortDirection;
            Grid2.SortField = e.SortField;
            BindGrid2();
        }

        protected void Grid2_PageIndexChange(object sender, GridPageEventArgs e)
        {
            Grid2.PageIndex = e.NewPageIndex;
            BindGrid2();
        }

        protected void btnDeleteSelected_Click(object sender, EventArgs e)
        {
            // 在操作之前进行权限检查
            if (!CheckPower("CoreDeptUserDelete"))
            {
                CheckPowerFailWithAlert();
                return;
            }

            // 从每个选中的行中获取ID（在Grid1中定义的DataKeyNames）
            int deptID = GetSelectedDataKeyID(Grid1);
            List<int> userIDs = GetSelectedDataKeyIDs(Grid2);


            // 执行数据库操作
            string IDs = "";

            foreach (var item in userIDs)
            {
                IDs += Convert.ToInt32(item) + ",";
            }

            IDs = IDs.Substring(0, IDs.Length - 1);

            DB.Execute("update users set DeptID = null where ID in (" + IDs + ")");


            // 重新绑定表格
            BindGrid2();
        }


        protected void Grid2_RowCommand(object sender, GridCommandEventArgs e)
        {
            object[] values = Grid2.DataKeys[e.RowIndex];
            int userID = Convert.ToInt32(values[0]);

            if (e.CommandName == "Delete")
            {
                // 在操作之前进行权限检查
                if (!CheckPower("CoreDeptUserDelete"))
                {
                    CheckPowerFailWithAlert();
                    return;
                }


                int deptID = GetSelectedDataKeyID(Grid1);

                // 执行数据库操作
                DB.Execute("update users set DeptID = null where ID = @UserID", new { UserID = userID });



                BindGrid2();

            }
        }

        protected void Window1_Close(object sender, EventArgs e)
        {
            BindGrid2();
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            int deptID = GetSelectedDataKeyID(Grid1);
            string addUrl = string.Format("~/admin/dept_user_addnew.aspx?id={0}", deptID);

            PageContext.RegisterStartupScript(Window1.GetShowReference(addUrl, "添加用户到当前部门"));
        }

        #endregion

    }
}
