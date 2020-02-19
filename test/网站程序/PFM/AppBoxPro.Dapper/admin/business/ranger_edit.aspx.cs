using System;

using System.Linq;
using FineUIPro;
using Dapper;
using System.Transactions;
using System.Collections.Generic;

namespace AppBoxPro.Dapper.admin.business
{
    public partial class ranger_edit : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "BizRangerEdit";
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

        private Ranger GetCurrentRanger(int rangerID)
        {
            return DB.QuerySingleOrDefault<Ranger>("select * from rangers where ID = @RangerID", new { RangerID = rangerID });
        }

        private void LoadData()
        {
            btnClose.OnClientClick = ActiveWindow.GetHideReference();

            int id = GetQueryIntValue("id");
            Ranger current = GetCurrentRanger(id);
            if (current == null)
            {
                // 参数错误，首先弹出Alert对话框然后关闭弹出窗口
                Alert.Show("参数错误！", string.Empty, ActiveWindow.GetHideReference());
                return;
            }

            if (current.Name == "admin" && GetIdentityName() != "admin")
            {
                Alert.Show("你无权编辑超级管理员！", string.Empty, ActiveWindow.GetHideReference());
                return;
            }

            labID.Text = current.ID.ToString();
            tbxName.Text = current.Name;
            tbxTel.Text = current.Tel;
            tbxTown.Text = current.Town;
            tbxVillage.Text = current.Village;

        }

        #endregion

        #region Events

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            int rangerID = GetQueryIntValue("id");

            Ranger item = GetCurrentRanger(rangerID);
            item.Name = tbxName.Text.Trim();
            item.Tel = tbxTel.Text.Trim();
            item.Town = tbxTown.Text.Trim();
            item.Village = tbxVillage.Text.Trim();
            


            //if (string.IsNullOrEmpty(hfSelectedDept.Text))
            //{
            //    item.DeptID = null;
            //}
            //else
            //{
            //    item.DeptID = Convert.ToInt32(hfSelectedDept.Text);
            //}


            using (var transactionScope = new TransactionScope())
            {
                // 更新用户
                ExecuteUpdate<Ranger>(DB, item);

                transactionScope.Complete();
            }

            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }

        #endregion

    }
}
