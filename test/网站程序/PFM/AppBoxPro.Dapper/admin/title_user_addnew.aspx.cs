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
    public partial class title_user_addnew : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreTitleUserNew";
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
            Title current = FindByID<Title>(id);
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
            int titleID = GetQueryIntValue("id");

            // 查询条件
            var builder = new WhereBuilder();
            
            string searchText = ttbSearchMessage.Text.Trim();
            if (!string.IsNullOrEmpty(searchText))
            {
                builder.AddWhere("(users.Name like @SearchText or users.ChineseName like @SearchText or users.EnglishName like @SearchText)");
                builder.AddParameter("SearchText", "%" + searchText + "%");
            }

            builder.AddWhere("users.Name != 'admin'");

            // 排除已经属于本职称的用户
            builder.AddWhere("users.ID not in (select ID from users inner join titleusers on users.ID = titleusers.UserID where titleusers.TitleID = @TitleID)");
            builder.AddParameter("TitleID", titleID);


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
            int titleID = GetQueryIntValue("id");

            // 跨页保持选中行
            string[] ids = Grid1.SelectedRowIDArray;

            int[] newids = new int[ids.Length];

            for (int i = 0; i < ids.Length; i++)
            {
                newids[i] = Convert.ToInt32(ids[i]);
            }

            DB.Execute("insert into titleusers (UserID, TitleID) values (@UserID, @TitleID)", newids.Select(u => new { UserID = u, TitleID = titleID }).ToList());
            
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
