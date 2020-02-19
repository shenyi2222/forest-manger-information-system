using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;

using FineUIPro;
using System.Transactions;
using Dapper;


namespace AppBoxPro.Dapper.admin.business
{
    public partial class ranger_new : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "BizRangerNew";
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
            Ranger item = new Ranger();
            item.Name = tbxName.Text.Trim();
            item.Password = tbxPassword.Text.Trim();
            //item.Password = PasswordUtil.CreateDbPassword(tbxPassword.Text.Trim());
            item.Tel = tbxTel.Text.Trim();
            item.Character = ddlCharacter.SelectedValue;
            item.Town = tbxTown.Text.Trim();
            item.Village = tbxVillage.Text.Trim();

            using (var transactionScope = new TransactionScope())
            {
                // 插入用户
                var rangerID = ExecuteInsert<Ranger>(DB, item);

                transactionScope.Complete();
            }

        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            string inputRangerName = tbxName.Text.Trim();

            Object ranger = DB.QuerySingleOrDefault("select * from rangers where Name = @RangerName", new { RangerName = inputRangerName });

            if (ranger != null)
            {
                Alert.Show("用户 " + inputRangerName + " 已经存在！");
                return;
            }

            SaveItem();

            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }
        #endregion

    }
}
