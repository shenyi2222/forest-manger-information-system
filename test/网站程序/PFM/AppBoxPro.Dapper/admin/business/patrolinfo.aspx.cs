using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FineUIPro;
using System.Linq;


using Dapper;

namespace AppBoxPro.Dapper.admin.business
{
    public partial class patrolinfo : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "BizPatrolInfoView";
            }
        }

        #endregion

        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {
            if (etDate.SelectedDate == null)
            {
                etDate.SelectedDate = DateTime.Now;
            }

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
                if (!string.IsNullOrEmpty(stDate.Text))
                {
                    if (!string.IsNullOrEmpty(etDate.Text))
                    {
                        builder.AddWhere("(patrolinfos.Name like @SearchText or patrolinfos.Town like @SearchText or patrolinfos.Village like @SearchText and patrolinfos.time between @ST and @ET)");

                        builder.AddParameter("SearchText", "%" + searchText + "%");

                        builder.AddParameter("ST", stDate.SelectedDate.GetValueOrDefault());

                        builder.AddParameter("ET", etDate.SelectedDate.GetValueOrDefault().AddDays(1));
                    }
                    else
                    {
                        builder.AddWhere("(patrolinfos.Name like @SearchText or patrolinfos.Town like @SearchText or patrolinfos.Village like @SearchText and patrolinfos.time >= @ST)");

                        builder.AddParameter("SearchText", "%" + searchText + "%");

                        builder.AddParameter("ST", stDate.SelectedDate.GetValueOrDefault());
                    }
                }
                else if (!string.IsNullOrEmpty(etDate.Text))
                {
                    builder.AddWhere("(patrolinfos.Name like @SearchText or patrolinfos.Town like @SearchText or patrolinfos.Village like @SearchText and patrolinfos.time <= @ET)");

                    builder.AddParameter("SearchText", "%" + searchText + "%");

                    builder.AddParameter("ET", stDate.SelectedDate.GetValueOrDefault().AddDays(1));
                }
                else
                {
                    builder.AddWhere("(patrolinfos.Name like @SearchText or patrolinfos.Town like @SearchText or patrolinfos.Village like @SearchText and patrolinfos.time >= @ST)");

                    builder.AddParameter("SearchText", "%" + searchText + "%");

                    builder.AddParameter("ST", stDate.SelectedDate.GetValueOrDefault());
                }

            }
            else
            {
                if (!string.IsNullOrEmpty(stDate.Text))
                {
                    if (!string.IsNullOrEmpty(etDate.Text))
                    {
                        builder.AddWhere("(patrolinfos.time between @ST and @ET)");

                        builder.AddParameter("ST", stDate.SelectedDate.GetValueOrDefault());

                        builder.AddParameter("ET", etDate.SelectedDate.GetValueOrDefault().AddDays(1));
                    }
                    else
                    {
                        builder.AddWhere("(patrolinfos.time >= @ST)");

                        builder.AddParameter("ST", stDate.SelectedDate.GetValueOrDefault());
                    }
                }
                else if (!string.IsNullOrEmpty(etDate.Text))
                {
                    builder.AddWhere("(patrolinfos.time <= @ET)");

                    builder.AddParameter("ET", etDate.SelectedDate.GetValueOrDefault().AddDays(1));
                }
            }

            // 获取总记录数（在添加条件之后，排序和分页之前）
            Grid1.RecordCount = Count<Patrolinfo>(builder);

            // 排列和数据库分页
            Grid1.DataSource = SortAndPage<Patrolinfo>(builder, Grid1);
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

        protected void Window1_Close(object sender, EventArgs e)
        {
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
