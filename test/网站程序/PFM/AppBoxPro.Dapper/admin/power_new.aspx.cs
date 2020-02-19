﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;

using FineUIPro;

namespace AppBoxPro.Dapper.admin
{
    public partial class power_new : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CorePowerNew";
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

        }

        #endregion

        #region Events

        private void SaveItem()
        {
            Power item = new Power();
            item.Name = tbxName.Text.Trim();
            item.GroupName = tbxGroupName.Text.Trim();
            item.Title = tbxTitle.Text.Trim();
            item.Remark = tbxRemark.Text.Trim();

            ExecuteInsert<Power>(item);
        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            SaveItem();

            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }
        #endregion

    }
}
