using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using webapi.Models.Business;
using webapi.Models.Parameter;

namespace webapi.Controllers
{
    public class LoginController : ApiController
    {
        [HttpPost]
        public WebApiResultValue<List<ProjectInfoListModel>> PostProjectInfoById(Login model)
        {
            var tel = model.tel;
            var password = model.password;
            List<ProjectInfoListModel> data = new List<ProjectInfoListModel>();

            string constr = "PORT=5432;DATABASE=PFM;HOST=39.106.41.205;PASSWORD=123456;USER ID=user";
            NpgsqlConnection mycon = new NpgsqlConnection(constr);
            mycon.Open();
            string sql = "select id, tel,name,character,town,village  FROM  rangers  where tel='" + tel + "' and password ='" + password + "'";
            NpgsqlDataAdapter mda = new NpgsqlDataAdapter(sql, mycon);
            DataSet ds = new DataSet();
            mda.Fill(ds, "table");
            //ds.Tables["table1"].Rows[0][1].ToString();
            //就是要读取第一行第二列空间数据字段的内容;
            var resultcount = ds.Tables["table"].Rows.Count;
            int i = 0;
            for (i = 0; i < resultcount; i++)
            {
                ProjectInfoListModel info = new ProjectInfoListModel();

                //info.patrolpoint = ds.Tables["table"].Rows[i][1];
                info.id = ds.Tables["table"].Rows[i][0];
                info.tel = ds.Tables["table"].Rows[i][1];
                info.name = ds.Tables["table"].Rows[i][2];
                info.character = ds.Tables["table"].Rows[i][3];
                info.town = ds.Tables["table"].Rows[i][4];
                info.village = ds.Tables["table"].Rows[i][5];
                data.Add(info);

            }
            mycon.Close();
            if (data.Count == 0)
            { return new WebApiResultValue<List<ProjectInfoListModel>>(2, data); }
            else
            {
                return new WebApiResultValue<List<ProjectInfoListModel>>(1, data);
            }
        }
    }
}
