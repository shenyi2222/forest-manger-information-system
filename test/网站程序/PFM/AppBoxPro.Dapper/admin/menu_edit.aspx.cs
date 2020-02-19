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
    public partial class menu_edit : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreMenuEdit";
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

        private Menu GetCurrentMenu(int menuID)
        {
            return DB.QuerySingleOrDefault<Menu>("select menus.*, powers.Name ViewPowerName from menus left join powers on menus.ViewPowerID = powers.ID where menus.ID = @MenuID", new { MenuID = menuID });
        }

        private void LoadData()
        {
            btnClose.OnClientClick = ActiveWindow.GetHideReference();

            int id = GetQueryIntValue("id");

            Menu current = GetCurrentMenu(id);
            if (current == null)
            {
                // 参数错误，首先弹出Alert对话框然后关闭弹出窗口
                Alert.Show("参数错误！", string.Empty, ActiveWindow.GetHideReference());
                return;
            }

            tbxName.Text = current.Name;
            tbxUrl.Text = current.NavigateUrl;
            tbxSortIndex.Text = current.SortIndex.ToString();
            tbxIcon.Text = current.ImageUrl;
            tbxRemark.Text = current.Remark;
            if (current.ViewPowerName != null)
            {
                tbxViewPower.Text = current.ViewPowerName;
            }


            // 绑定上级菜单下拉列表
            BindDDL(current);

            // 预置图标列表
            InitIconList(iconList);

            if (!string.IsNullOrEmpty(current.ImageUrl))
            {
                iconList.SelectedValue = current.ImageUrl;
            }

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

        private void BindDDL(Menu current)
        {
            List<Menu> mys = ResolveDDL<Menu>(MenuHelper.Menus, current.ID);

            // 绑定到下拉列表（启用模拟树功能和不可选择项功能）
            ddlParent.EnableSimulateTree = true;
            ddlParent.DataTextField = "Name";
            ddlParent.DataValueField = "ID";
            ddlParent.DataSimulateTreeLevelField = "TreeLevel";
            ddlParent.DataEnableSelectField = "Enabled";
            ddlParent.DataSource = mys;
            ddlParent.DataBind();

            if (current.ParentID != null)
            {
                // 选中当前节点的父节点
                ddlParent.SelectedValue = current.ParentID.ToString();
            }
        }

        #endregion

        #region Events

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            int menuID = GetQueryIntValue("id");

            Menu item = GetCurrentMenu(menuID);
            item.Name = tbxName.Text.Trim();
            item.NavigateUrl = tbxUrl.Text.Trim();
            item.SortIndex = Convert.ToInt32(tbxSortIndex.Text.Trim());
            item.ImageUrl = tbxIcon.Text;
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

            //ExecuteUpdate(item, "menus", "Name", "NavigateUrl", "SortIndex", "ImageUrl", "Remark", "ParentID", "ViewPowerID");
            ExecuteUpdate<Menu>(item);


            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }

        #endregion

    }
}
