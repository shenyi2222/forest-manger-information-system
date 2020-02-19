using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;

using FineUIPro;
using System.Transactions;
using System.Text;
using Dapper;

namespace AppBoxPro.Dapper.admin
{
    public partial class user_select_title : PageBase
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
            btnClose.OnClientClick = ActiveWindow.GetHideReference();

            string ids = GetQueryValue("ids");

            // 绑定角色复选框列表
            BindDDLRole();

            // 初始化角色复选框列表的选择项
            cblJobTitle.SelectedValueArray = ids.Split(',');
        }

        private void BindDDLRole()
        {
            cblJobTitle.DataTextField = "Name";
            cblJobTitle.DataValueField = "ID";
            cblJobTitle.DataSource = DB.Query<Title>("select * from titles");
            cblJobTitle.DataBind();

        }

        #endregion

        #region Events


        #endregion

    }
}
