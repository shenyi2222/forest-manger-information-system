﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Linq;
using FineUIPro;
using System.Transactions;
using System.Text;
using Dapper;

namespace AppBoxPro.Dapper.admin.business
{
    public partial class message_select_recipients : PageBase
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
            btnClose.OnClientClick = ActiveWindow.GetHideReference();


            _recipientID = GetQueryIntValue("ids");

            // 绑定列表
            BindGrid();

            // 初始化选中项，放在表格数据绑定之后
            if (_selectedRowIndex != -1)
            {
                Grid1.SelectedRowIndex = _selectedRowIndex;
            }
        }

        // 当前部门ID
        private int _recipientID;
        // 用来在表格渲染时记录选中的行索引
        private int _selectedRowIndex = -1;

        private void BindGrid()
        {
            Grid1.DataSource = DB.Query<Ranger>("select * from rangers where town='锦城街道'");
            Grid1.DataBind();

        }

        #endregion

        #region Events

        protected void Grid1_RowDataBound(object sender, FineUIPro.GridRowEventArgs e)
        {
            // 行绑定后，确定应该选择哪一行
            Ranger ranger = e.DataItem as Ranger;
            if (ranger != null && _recipientID == ranger.ID)
            {
                _selectedRowIndex = e.RowIndex;
            }
        }

        #endregion

    }
}
