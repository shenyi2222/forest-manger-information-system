using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

using Newtonsoft.Json.Linq;
using FineUIPro;
using System.Linq;



namespace AppBoxPro.Dapper
{
    public partial class main : PageBase
    {
        #region Page_Init

        protected void Page_Init(object sender, EventArgs e)
        {
            // 工具栏上的帮助菜单
            JArray ja = JArray.Parse(ConfigHelper.HelpList);
            foreach (JObject jo in ja)
            {
                MenuButton menuItem = new MenuButton();
                menuItem.EnablePostBack = false;
                menuItem.Text = jo.Value<string>("Text");
                menuItem.Icon = IconHelper.String2Icon(jo.Value<string>("Icon"), true);
                menuItem.OnClientClick = string.Format("addExampleTab('{0}','{1}','{2}')", jo.Value<string>("ID"), ResolveUrl(jo.Value<string>("URL")), jo.Value<string>("Text"));

                btnHelp.Menu.Items.Add(menuItem);
            }

            // 用户可见的菜单列表
            List<Menu> menus = ResolveUserMenuList();
            if (menus.Count == 0)
            {
                Response.Write("系统管理员尚未给你配置菜单！");
                Response.End();

                return;
            }

            if (ConfigHelper.MenuType == "accordion")
            {
                InitAccordionMenu(menus);
            }
            else
            {
                InitTreeMenu(menus);
            }
        }

        #region InitAccordionMenu

        /// <summary>
        /// 创建手风琴菜单
        /// </summary>
        /// <param name="menus"></param>
        /// <returns></returns>
        private Accordion InitAccordionMenu(List<Menu> menus)
        {
            Accordion accordionMenu = new Accordion();
            accordionMenu.ID = "accordionMenu";
            accordionMenu.EnableFill = true;
            accordionMenu.ShowBorder = false;
            accordionMenu.ShowHeader = false;
            leftPanel.Items.Add(accordionMenu);


            foreach (var menu in menus.Where(m => m.ParentID == null))
            {
                AccordionPane accordionPane = new AccordionPane();
                accordionPane.Title = menu.Name;
                accordionPane.Layout = LayoutType.Fit;
                accordionPane.ShowBorder = false;
                accordionPane.BodyPadding = "2px 0 0 0";
               
                Tree innerTree = new Tree();
                innerTree.ShowBorder = false;
                innerTree.ShowHeader = false;
                innerTree.EnableIcons = true;
                innerTree.AutoScroll = true;
                
                // 生成树
                int nodeCount = ResolveMenuTree(menus, menu.ID, innerTree.Nodes);
                if (nodeCount > 0)
                {
                    accordionPane.Items.Add(innerTree);
                    accordionMenu.Items.Add(accordionPane);
                }

            }

            return accordionMenu;
        }

        #endregion

        #region InitTreeMenu

        /// <summary>
        /// 创建树菜单
        /// </summary>
        /// <param name="menus"></param>
        /// <returns></returns>
        private Tree InitTreeMenu(List<Menu> menus)
        {
            Tree treeMenu = new Tree();
            treeMenu.ID = "treeMenu";
            treeMenu.ShowBorder = false;
            treeMenu.ShowHeader = false;
            treeMenu.EnableIcons = true;
            treeMenu.AutoScroll = true;
            leftPanel.Items.Add(treeMenu);

            // 生成树
            ResolveMenuTree(menus, null, treeMenu.Nodes);

            // 展开第一个树节点
            treeMenu.Nodes[0].Expanded = true;

            return treeMenu;
        }

        /// <summary>
        /// 生成菜单树
        /// </summary>
        /// <param name="menus"></param>
        /// <param name="parentMenuId"></param>
        /// <param name="nodes"></param>
        private int ResolveMenuTree(List<Menu> menus, int? parentMenuID, FineUIPro.TreeNodeCollection nodes)
        {
            int count = 0;
            foreach (var menu in menus.Where(m => m.ParentID == parentMenuID))
            {
                FineUIPro.TreeNode node = new FineUIPro.TreeNode();
                nodes.Add(node);
                count++;

                node.Text = menu.Name;
                node.IconUrl = menu.ImageUrl;
                if (!string.IsNullOrEmpty(menu.NavigateUrl))
                {
                    node.EnableClickEvent = false;
                    node.NavigateUrl = ResolveUrl(menu.NavigateUrl);
                }

                if (menu.IsTreeLeaf)
                {
                    node.Leaf = true;

                    // 如果是叶子节点，但不是超链接，则是空目录，删除
                    if (string.IsNullOrEmpty(menu.NavigateUrl))
                    {
                        nodes.Remove(node);
                        count--;
                    }
                }
                else
                {
                    int childCount = ResolveMenuTree(menus, menu.ID, node.Nodes);

                    // 如果是目录，但是计算的子节点数为0，可能目录里面的都是空目录，则要删除此父目录
                    if (childCount == 0 && string.IsNullOrEmpty(menu.NavigateUrl))
                    {
                        nodes.Remove(node);
                        count--;
                    }
                }

            }

            return count;
        }

        #endregion

        #region ResolveUserMenuList

        // 获取用户可用的菜单列表
        private List<Menu> ResolveUserMenuList()
        {
            // 当前登陆用户的权限列表
            List<string> rolePowerNames = GetRolePowerNames();

            // 当前用户所属角色可用的菜单列表
            List<Menu> menus = new List<Menu>();

            foreach (var menu in MenuHelper.Menus)
            {
                // 如果此菜单不属于任何模块，或者此用户所属角色拥有对此模块的权限
                if (menu.ViewPowerID == null || rolePowerNames.Contains(menu.ViewPowerName))
                {
                    menus.Add(menu);
                }
            }

            return menus;
        } 

        #endregion

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
            System.Web.UI.WebControls.HyperLink link = topPanel.FindControl("linkSystemTitle") as System.Web.UI.WebControls.HyperLink;
            if (link != null)
            {
                //link.Text = string.Format("AppBoxPro.Dapper v{0}", GetProductVersion());
                link.Text = string.Format("专业森林管家系统");
            }

            btnUserName.Text = GetIdentityName();
        }


        #endregion

        #region Events

        protected void btnExit_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Session.Abandon();

            FormsAuthentication.RedirectToLoginPage();
        }

        #endregion
    }
}
