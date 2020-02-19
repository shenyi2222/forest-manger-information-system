using System;

using System.Linq;
using FineUIPro;
using Dapper;
using System.Transactions;
using System.Collections.Generic;

namespace AppBoxPro.Dapper.admin.business
{
    public partial class message_edit : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "BizMessageEdit";
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

        private Message GetCurrentMessage(int messageID)
        {
            return DB.QuerySingleOrDefault<Message>("select * from messages where ID = @MessageID", new { MessageID = messageID });
        }

        private void LoadData()
        {
            btnClose.OnClientClick = ActiveWindow.GetHideReference();

            int id = GetQueryIntValue("id");
            Message current = GetCurrentMessage(id);
            if (current == null)
            {
                // 参数错误，首先弹出Alert对话框然后关闭弹出窗口
                Alert.Show("参数错误！", string.Empty, ActiveWindow.GetHideReference());
                return;
            }

            labRecipients.Text = current.Recipients;
            tbxContent.Text = current.Content;
            pickerDate.SelectedDate = current.Time;
            //TimeSpan time = new TimeSpan(current.Time.Hour, current.Time.Minute,current.Time.Second);
            pickerTime.SelectedDate = current.Time;

        }

        #endregion

        #region Events

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            int messageID = GetQueryIntValue("id");

            Message item = GetCurrentMessage(messageID);
            if (string.IsNullOrEmpty(labRecipients.Text))
            {
                item.Recipients = "全部";
            }
            else
            {
                item.Recipients = labRecipients.Text.Trim();
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

            using (var transactionScope = new TransactionScope())
            {
                // 更新用户
                ExecuteUpdate<Message>(DB, item);

                transactionScope.Complete();
            }

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
