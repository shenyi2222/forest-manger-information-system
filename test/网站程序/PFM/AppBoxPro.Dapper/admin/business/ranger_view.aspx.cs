using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FineUIPro;
using System.Text;
using System.Linq;

using Dapper;

namespace AppBoxPro.Dapper.admin.business
{
    public partial class ranger_view : PageBase
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

            int id = GetQueryIntValue("id");

            Ranger current = FindByID<Ranger>(id);

            if (current == null)
            {
                // 参数错误，首先弹出Alert对话框然后关闭弹出窗口
                Alert.Show("参数错误！", string.Empty, ActiveWindow.GetHideReference());
                return;
            }

            labID.Text = current.ID.ToString();
            labName.Text = current.Name;
            labTel.Text = current.Tel;
            labTown.Text = current.Town;
            LabVillage.Text = current.Village;

        }

        #endregion

    }
}
