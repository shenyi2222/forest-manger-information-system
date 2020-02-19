using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using FineUIPro;


using Dapper;
using System.Data;


namespace AppBoxPro.Dapper.admin
{
    public partial class online : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreOnlineView";
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
                builder.AddWhere("users.Name like @SearchText");
                builder.AddParameter("SearchText", "%" + searchText + "%");
            }

            DateTime twoHoursBefore = DateTime.Now.AddHours(-2);
            builder.AddWhere("onlines.UpdateTime > @TwoHoursBefore");
            builder.AddParameter("TwoHoursBefore", twoHoursBefore);


            // 获取总记录数（在添加条件之后，排序和分页之前）
            Grid1.RecordCount = Count<Online>(builder);

            // 排列和数据库分页
            builder.FromSql = "select onlines.*, users.Name UserName, users.ChineseName UserChineseName from onlines inner join users on users.ID = onlines.UserID";
            Grid1.DataSource = SortAndPage<Online>(builder, Grid1);
            Grid1.DataBind();
        }



        #endregion

        #region Events

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
