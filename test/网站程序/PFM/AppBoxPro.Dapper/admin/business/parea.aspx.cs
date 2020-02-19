using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;

using FineUIPro;
using System.Transactions;
using Dapper;
using System.Diagnostics;

namespace AppBoxPro.Dapper.admin.business
{
    public partial class parea : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindTown();
                BindVillage();
                BindRanger();

            }

        }
        private void BindTown()
        {
            ddlTown.DataSource = DB.Query("select distinct town from rangers");
            ddlTown.DataBind();
            ddlTown.DataTextField = "town";
            ddlTown.DataValueField = "town";
            ddlTown.Items.Insert(0, new FineUIPro.ListItem("选择乡镇", "-1"));
            ddlTown.SelectedValue = "-1";
        }
        private void BindVillage()
        {
            string sheng = ddlTown.SelectedValue;

            if (sheng != "-1")
            {   
                ddlVillage.DataSource = DB.Query("select distinct village from rangers where town ='" + sheng+"'");
                ddlVillage.DataBind();
                ddlVillage.DataTextField = "village";
                ddlVillage.DataValueField = "village";
            }

            ddlVillage.Items.Insert(0, new FineUIPro.ListItem("选择村", "-1"));
            ddlVillage.SelectedValue = "-1";

            // 是否禁用
            ddlVillage.Enabled = !(ddlVillage.Items.Count == 1);
        }
        private void BindRanger()
        {
            string shi = ddlVillage.SelectedValue;
            string sheng = ddlTown.SelectedValue;
            if (shi != "-1")
            {    //配置数据源
                ddlRanger.DataSource = DB.Query("select distinct name from rangers where village ='" + shi + "'" + "and town='" + sheng + "'");
                //绑定字段属性（ps：这个字段属性必须和数据库里的字段名一致）
                ddlRanger.DataTextField = "name";
                ddlRanger.DataValueField = "name";
                //绑定到控件
                ddlRanger.DataBind();
            }

            ddlRanger.Items.Insert(0, new FineUIPro.ListItem("选择护林人员", "-1"));
            ddlRanger.SelectedValue = "-1";

            // 是否禁用
            ddlRanger.Enabled = !(ddlRanger.Items.Count == 1);
        }
        protected void ddlTown_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlVillage.Items.Clear();
            BindVillage();
            ddlRanger.Items.Clear();
            BindRanger();


        }

        protected void ddlVillage_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlRanger.Items.Clear();
            BindRanger();


        }
        protected void ddlRanger_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}