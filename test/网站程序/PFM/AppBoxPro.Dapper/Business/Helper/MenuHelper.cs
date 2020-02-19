using System;
using System.Collections.Generic;
using System.Web;

using System.Linq;


using Dapper;

namespace AppBoxPro.Dapper
{
    public class MenuHelper
    {
        private static List<Menu> _menus;

        public static List<Menu> Menus
        {
            get
            {
                if (_menus == null)
                {
                    InitMenus();
                }
                return _menus;
            }
        }

        public static void Reload()
        {
            _menus = null;
        }

        private static void InitMenus()
        {
            _menus = new List<Menu>();

            List<Menu> dbMenus = PageBase.DB.Query<Menu>("select menus.*, powers.Name ViewPowerName from menus left join powers on menus.ViewPowerID = powers.ID order by SortIndex ASC").ToList();


            ResolveMenuCollection(dbMenus, null, 0);
        }


        private static int ResolveMenuCollection(List<Menu> dbMenus, int? parentMenuID, int level)
        {
            int count = 0;

            foreach (var menu in dbMenus.Where(m => m.ParentID == parentMenuID))
            {
                count++;

                _menus.Add(menu);
                menu.TreeLevel = level;
                menu.IsTreeLeaf = true;
                menu.Enabled = true;

                level++;
                int childCount = ResolveMenuCollection(dbMenus, menu.ID, level);
                if (childCount != 0)
                {
                    menu.IsTreeLeaf = false;
                }
                level--;
            }

            return count;
        }

    }
}
