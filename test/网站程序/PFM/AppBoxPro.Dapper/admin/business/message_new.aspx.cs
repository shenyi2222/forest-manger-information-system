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
    public partial class message_new : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "BizMessageNew";
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

            //为下拉列表绑定数据
            BindDDL();

            
            ddlVillage.SelectedValue = "0";
            //// 初始化收信方
            //InitRecipients();
        }

        private void BindDDL()
        {
            ddlVillage.DataSource = DB.Query("select distinct village from rangers where town='锦城街道'");
            ddlVillage.DataTextField = "village";           
            ddlVillage.DataBind();
            ddlVillage.Items.Add("全部", "0");
        }

        //#region InitRecipients

        //private void InitRecipients()
        //{


        //}

        //#endregion

        #endregion

        #region Events


        private void SaveItem()
        {
            Message item = new Message();
            if (string.IsNullOrEmpty(ddlVillage.SelectedText))
            {
                item.Recipients = "全体";
            }
            else
            {
                item.Recipients = ddlVillage.SelectedText.Trim();
            }
            item.Content = tbxContent.Text.Trim();
            if (cbxEnabled.Checked)
            {
                DateTime date = pickerDate.SelectedDate.GetValueOrDefault();
                DateTime time = pickerTime.SelectedDate.GetValueOrDefault();
                DateTime dateTime = new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second);
                item.Time = dateTime;
            }
            else
            {
                item.Time = DateTime.Now;
            }
            string user = User.Identity.Name;
            string name = user.Substring(user.IndexOf('_') + 1);
            item.Sender = name;

            using (var transactionScope = new TransactionScope())
            {
                // 插入信息
                var messageID = ExecuteInsert<Message>(DB, item);

                transactionScope.Complete();
            }

        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            SaveItem();

            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }

        protected void cbxEnabled_CheckedChanged(object sender, CheckedEventArgs e)
        {
            if (cbxEnabled.Checked)
            {
                pickerDate.Enabled = true;
                pickerTime.Enabled = true;
                pickerDate.Required = true;
                pickerTime.Required = true;
            }
            else
            {
                pickerDate.Enabled = false;
                pickerTime.Enabled = false;
                pickerDate.Required = false;
                pickerTime.Required = false;
            }

        }
        #endregion

    }
}
