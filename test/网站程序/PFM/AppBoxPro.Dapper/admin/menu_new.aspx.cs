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
    public partial class menu_new : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreMenuNew";
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

            //// 模块名称列表
            //ddlModules.DataSource = ModuleTypeHelper.GetAppModules();
            //ddlModules.DataBind();

            //ddlModules.SelectedValue = ModuleTypeHelper.Module2String(ModuleType.None);

            BindDDL();

            InitIconList(iconList);
        }

        public void InitIconList(FineUIPro.RadioButtonList iconList)
        {
            string[] icons = new string[] { "tag_yellow", "tag_red", "tag_purple", "tag_pink", "tag_orange", "tag_green", "tag_blue" };
            foreach (string icon in icons)
            {
                string value = string.Format("~/res/icon/{0}.png", icon);
                string text = string.Format("<img style=\"vertical-align:bottom;\" src=\"{0}\" />&nbsp;{1}", ResolveUrl(value), icon);

                iconList.Items.Add(new RadioItem(text, value));
            }
        }

        private void BindDDL()
        {
            List<Menu> menus = ResolveDDL<Menu>(MenuHelper.Menus);

            // 绑定到下拉列表（启用模拟树功能）
            ddlParent.EnableSimulateTree = true;
            ddlParent.DataTextField = "Name";
            ddlParent.DataValueField = "ID";
            ddlParent.DataSimulateTreeLevelField = "TreeLevel";
            ddlParent.DataSource = menus;
            ddlParent.DataBind();

            // 选中根节点
            ddlParent.SelectedValue = "0";
        }

        #endregion

        #region Events

        private void SaveItem()
        {
            Menu item = new Menu();
            item.Name = tbxName.Text.Trim();
            item.NavigateUrl = tbxUrl.Text.Trim();
            item.SortIndex = Convert.ToInt32(tbxSortIndex.Text.Trim());
            item.Remark = tbxRemark.Text.Trim();

            int parentID = Convert.ToInt32(ddlParent.SelectedValue);
            item.ParentID = parentID == -1 ? null : (int?)parentID;

            string viewPowerName = tbxViewPower.Text.Trim();
            if (string.IsNullOrEmpty(viewPowerName))
            {
                item.ViewPowerID = null;
            }
            else
            {
                item.ViewPowerID = DB.QuerySingleOrDefault<int?>("select powers.ID from powers where powers.Name = @ViewPowerName", new { ViewPowerName = viewPowerName });

            }

            //ExecuteInsert(item, "menus", "Name", "NavigateUrl", "SortIndex", "ImageUrl", "Remark", "ParentID", "ViewPowerID");
            ExecuteInsert<Menu>(item);
        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            SaveItem();

            //Alert.Show("添加成功！", string.Empty, ActiveWindow.GetHidePostBackReference());
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }

        #endregion

    }
}
