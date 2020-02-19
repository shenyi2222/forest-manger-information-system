using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;

using FineUIPro;

using Dapper;

namespace AppBoxPro.Dapper.admin
{
    public partial class title : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreTitleView";
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
            CheckPowerWithButton("CoreTitleNew", btnNew);
            //CheckPowerDeleteWithButton(btnDeleteSelected);

            //ResolveDeleteButtonForGrid(btnDeleteSelected, Grid1);

            btnNew.OnClientClick = Window1.GetShowReference("~/admin/title_new.aspx", "新增职务");

            // 每页记录数
            Grid1.PageSize = ConfigHelper.PageSize;

            BindGrid();
        }

        private void BindGrid()
        {
            // 查询条件
            var builder = new WhereBuilder();
            

            string searchText = ttbSearchMessage.Text.Trim();
            if (!string.IsNullOrEmpty(searchText))
            {
                builder.AddWhere("titles.Name like @SearchText");
                builder.AddParameter("SearchText", "%" + searchText + "%");
            }

            // 获取总记录数（在添加条件之后，排序和分页之前）
            Grid1.RecordCount = Count<Title>(builder);

            // 排列和数据库分页
            Grid1.DataSource = SortAndPage<Title>(builder, Grid1);
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

        protected void Grid1_PreDataBound(object sender, EventArgs e)
        {
            // 数据绑定之前，进行权限检查
            CheckPowerWithWindowField("CoreTitleEdit", Grid1, "editField");
            CheckPowerWithLinkButtonField("CoreTitleDelete", Grid1, "deleteField");
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

        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
        {
            int titleID = GetSelectedDataKeyID(Grid1);

            if (e.CommandName == "Delete")
            {
                // 在操作之前进行权限检查
                if (!CheckPower("CoreTitleDelete"))
                {
                    CheckPowerFailWithAlert();
                    return;
                }

                int userCount = DB.QuerySingle<int>("select count(*) from titleusers where TitleID = @TitleID", new { TitleID = titleID });
                if (userCount > 0)
                {
                    Alert.ShowInTop("删除失败！需要先清空拥有此职务的用户！");
                    return;
                }

                DB.Execute("delete from titles where ID = @TitleID", new { TitleID = titleID });



                BindGrid();
            }
        }

        protected void Window1_Close(object sender, EventArgs e)
        {
            BindGrid();
        }

        #endregion

    }
}
