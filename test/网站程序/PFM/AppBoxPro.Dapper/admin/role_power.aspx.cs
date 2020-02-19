using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;

using FineUIPro;
using System.Data;
using Newtonsoft.Json.Linq;
using AspNet = System.Web.UI.WebControls;
using Dapper;
using System.Transactions;


namespace AppBoxPro.Dapper.admin
{
    public partial class role_power : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreRolePowerView";
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
            // 权限检查
            CheckPowerWithButton("CoreRolePowerEdit", btnGroupUpdate);



            // 每页记录数
            Grid1.PageSize = ConfigHelper.PageSize;
            BindGrid();

            // 默认选中第一个角色
            Grid1.SelectedRowIndex = 0;

            // 每页记录数
            Grid2.PageSize = ConfigHelper.PageSize;
            BindGrid2();
        }

        private void BindGrid()
        {
            // 全部的角色列表
            Grid1.DataSource = Sort<Role>(Grid1);
            Grid1.DataBind();
        }

        private Dictionary<string, bool> _currentRolePowers = new Dictionary<string, bool>();

        private void BindGrid2()
        {
            int roleID = GetSelectedDataKeyID(Grid1);
            if (roleID == -1)
            {
                Grid2.DataSource = null;
                Grid2.DataBind();
            }
            else
            {
                // 当前选中角色拥有的权限列表
                _currentRolePowers.Clear();

                var powerNames = DB.Query<string>("select Name from powers inner join rolepowers on powers.ID = rolepowers.PowerID where rolepowers.RoleID = @RoleID", new { RoleID = roleID });

                foreach (var power in powerNames)
                {
                    if (!_currentRolePowers.ContainsKey(power))
                    {
                        _currentRolePowers.Add(power, true);
                    }
                }

                var sql = "select * from powers";
                if (Grid2.SortField == "GroupName")
                {
                    sql += " order by GroupName " + (Grid2.SortDirection == "ASC" ? "asc" : "desc");
                }

                var powerList = new List<string>();
                var powerDictionary = new Dictionary<string, List<Power>>();
                foreach (var p in DB.Query<Power>(sql).ToList())
                {
                    List<Power> powers;
                    if (!powerDictionary.TryGetValue(p.GroupName, out powers))
                    {
                        powerList.Add(p.GroupName);

                        powers = new List<Power>();
                        powerDictionary[p.GroupName] = powers;
                    }

                    powers.Add(p);
                }

                var powerData = powerList.Select(u => new GroupPowerViewModel
                {
                    GroupName = u,
                    Powers = powerDictionary[u]
                });

                Grid2.DataSource = powerData;
                Grid2.DataBind();


            }

        }



        #endregion

        #region Grid1 Events

        protected void Grid1_Sort(object sender, GridSortEventArgs e)
        {
            Grid1.SortDirection = e.SortDirection;
            Grid1.SortField = e.SortField;
            BindGrid();

            // 默认选中第一个角色
            Grid1.SelectedRowIndex = 0;

            BindGrid2();
        }

        protected void Grid1_RowClick(object sender, FineUIPro.GridRowClickEventArgs e)
        {
            BindGrid2();
        }

        #endregion

        #region Grid2 Events

        protected void Grid2_RowDataBound(object sender, FineUIPro.GridRowEventArgs e)
        {
            AspNet.CheckBoxList ddlPowers = (AspNet.CheckBoxList)Grid2.Rows[e.RowIndex].FindControl("ddlPowers");

            var powers = e.DataItem as GroupPowerViewModel;

            foreach (Power power in powers.Powers)
            {
                AspNet.ListItem item = new AspNet.ListItem();
                item.Value = power.ID.ToString();
                item.Text = power.Title;
                item.Attributes["data-qtip"] = power.Name;

                if (_currentRolePowers.ContainsKey(power.Name))
                {
                    item.Selected = true;
                }
                else
                {
                    item.Selected = false;
                }

                ddlPowers.Items.Add(item);
            }
        }



        protected void Grid2_Sort(object sender, GridSortEventArgs e)
        {
            Grid2.SortDirection = e.SortDirection;
            Grid2.SortField = e.SortField;
            BindGrid2();
        }

        protected void btnGroupUpdate_Click(object sender, EventArgs e)
        {
            // 在操作之前进行权限检查
            if (!CheckPower("CoreRolePowerEdit"))
            {
                CheckPowerFailWithAlert();
                return;
            }

            int roleID = GetSelectedDataKeyID(Grid1);
            if (roleID == -1)
            {
                return;
            }

            // 当前角色新的权限列表
            List<int> newPowerIDs = new List<int>();
            for (int i = 0; i < Grid2.Rows.Count; i++)
            {
                AspNet.CheckBoxList ddlPowers = (AspNet.CheckBoxList)Grid2.Rows[i].FindControl("ddlPowers");
                foreach (AspNet.ListItem item in ddlPowers.Items)
                {
                    if (item.Selected)
                    {
                        newPowerIDs.Add(Convert.ToInt32(item.Value));
                    }
                }
            }


            using (var transactionScope = new TransactionScope())
            {
                DB.Execute("delete from rolepowers where RoleID = @RoleID", new { RoleID = roleID });
                DB.Execute("insert into rolepowers (RoleID, PowerID) values (@RoleID, @PowerID)", newPowerIDs.Select(u => new { PowerID = u, RoleID = roleID }).ToList());

                transactionScope.Complete();
            }


            Alert.ShowInTop("当前角色的权限更新成功！");
        }


        #endregion

    }

}
