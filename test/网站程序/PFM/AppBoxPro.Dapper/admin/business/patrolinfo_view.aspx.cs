using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FineUIPro;
using System.Text;
using System.Linq;

using Dapper;
using NpgsqlTypes;

namespace AppBoxPro.Dapper.admin.business
{
    public partial class patrolinfo_view : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "BizPatrolInfoView";
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

        public NpgsqlPoint point;

        public Patrolinfo patrolinfo;

        private void LoadData()
        {
            int id = GetQueryIntValue("id");

            NpgsqlPoint current = GetCurrentPoint(id);

            if (current == null)
            {
                // 参数错误，首先弹出Alert对话框然后关闭弹出窗口
                Alert.Show("参数错误！", string.Empty, ActiveWindow.GetHideReference());
                return;
            }

            point = current;

            patrolinfo = GetCurrentPatrolinfo(id);
        }

        private NpgsqlPoint GetCurrentPoint(int patrolinfoID)
        {
            //return DB.QueryFirst<NpgsqlPoint>("select point(point) from photos where id = @PatrolinfoID", new { PatrolinfoID = patrolinfoID });

            double point_x = DB.QueryFirst<double>("select st_x(st_transform(point, 4326)) from photos where id = @PatrolinfoID", new { PatrolinfoID = patrolinfoID });
            double point_y = DB.QueryFirst<double>("select st_y(st_transform(point, 4326)) from photos where id = @PatrolinfoID", new { PatrolinfoID = patrolinfoID });

            return new NpgsqlPoint(point_x, point_y);

        }

        private Patrolinfo GetCurrentPatrolinfo(int patrolinfoID)
        {
            return FindByID<Patrolinfo>(patrolinfoID);
        }

        #endregion

    }
}
