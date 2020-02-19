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
    public partial class ranger : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "BizRangerView";
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
            CheckPowerWithButton("BizRangerDelete", btnDeleteSelected);
            CheckPowerWithButton("BizRangerNew", btnNew);



            ResolveDeleteButtonForGrid(btnDeleteSelected, Grid1);

            btnNew.OnClientClick = Window1.GetShowReference("~/admin/business/ranger_new.aspx", "新增用户");

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
                builder.AddWhere("(rangers.Name like @SearchText or rangers.Town like @SearchText or rangers.Village like @SearchText)");
                builder.AddParameter("SearchText", "%" + searchText + "%");
            }

            if (GetIdentityName() != "admin")
            {
                builder.AddWhere("rangers.Name != 'admin'");
            }

            // 获取总记录数（在添加条件之后，排序和分页之前）
            Grid1.RecordCount = Count<Ranger>(builder);

            // 排列和数据库分页
            Grid1.DataSource = SortAndPage<Ranger>(builder, Grid1);
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
            CheckPowerWithWindowField("BizRangerEdit", Grid1, "editField");
            CheckPowerWithLinkButtonField("BizRangerDelete", Grid1, "deleteField");
            CheckPowerWithWindowField("BizRangerChangePassword", Grid1, "changePasswordField");

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

        protected void btnDeleteSelected_Click(object sender, EventArgs e)
        {
            // 在操作之前进行权限检查
            if (!CheckPower("BizRangerDelete"))
            {
                CheckPowerFailWithAlert();
                return;
            }

            // 从每个选中的行中获取ID（在Grid1中定义的DataKeyNames）
            List<int> ids = GetSelectedDataKeyIDs(Grid1);

            string IDs = "";

            foreach (var item in ids)
            {
                IDs += item + ",";
            }

            IDs = IDs.Substring(0, IDs.Length - 1);

            // 执行数据库操作
            DB.Execute("delete from rangers where ID in (" + IDs + ")");


            // 重新绑定表格
            BindGrid();
        }

        //protected void btnEnableRangers_Click(object sender, EventArgs e)
        //{
        //    SetSelectedRangersEnableStatus(true);
        //}

        //protected void btnDisableRangers_Click(object sender, EventArgs e)
        //{
        //    SetSelectedRangersEnableStatus(false);
        //}


        //private void SetSelectedRangersEnableStatus(bool enabled)
        //{
        //    // 在操作之前进行权限检查
        //    if (!CheckPower("BizRangerEdit"))
        //    {
        //        CheckPowerFailWithAlert();
        //        return;
        //    }

        //    // 从每个选中的行中获取ID（在Grid1中定义的DataKeyNames）
        //    List<int> ids = GetSelectedDataKeyIDs(Grid1);

        //    string IDs = "";

        //    foreach (var item in ids)
        //    {
        //        IDs += item + ",";
        //    }

        //    IDs = IDs.Substring(0, IDs.Length - 1);

        //    // 执行数据库操作
        //    DB.Execute("update rangers set Enabled = '" + (enabled ? 1 : 0) + "' where ID in (" + IDs + ")");  //对bool数值添加引号，List数组改为拼接字符串


        //    // 重新绑定表格
        //    BindGrid();
        //}

        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
        {
            int rangerID = GetSelectedDataKeyID(Grid1);
            //string rangerName = GetSelectedDataKey(Grid1, 1);

            if (e.CommandName == "Delete")
            {
                // 在操作之前进行权限检查
                if (!CheckPower("BizRangerDelete"))
                {
                    CheckPowerFailWithAlert();
                    return;
                }

                //if (rangerName == "admin")
                //{
                //    Alert.ShowInTop("不能删除默认的系统管理员（admin）！");
                //}
                else
                {
                    DB.Execute("delete from rangers where ID = @ID", new { ID = rangerID });


                    BindGrid();
                }
            }
        }

        protected void Window1_Close(object sender, EventArgs e)
        {
            BindGrid();
        }

        //protected void rblEnableStatus_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    BindGrid();
        //}


        protected void ddlGridPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Grid1.PageSize = Convert.ToInt32(ddlGridPageSize.SelectedValue);

            BindGrid();
        }

        #endregion

    }
}
