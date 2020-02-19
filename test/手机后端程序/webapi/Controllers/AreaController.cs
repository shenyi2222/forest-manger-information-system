﻿using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using webapi.Models.Business;
using webapi.Models.Parameter;

namespace webapi.Controllers
{
    public class AreaController : ApiController
    {
        [HttpGet]
        public WebApiResultValue<List<ProjectAreaModel>> GetProjectAreaById([FromUri]CKpoint model)

        {

            var id = model.id;
            List<ProjectAreaModel> data = new List<ProjectAreaModel>();
            string constr = "PORT=5432;DATABASE=PFM;HOST=39.106.41.205;PASSWORD=123456;USER ID=user";
            NpgsqlConnection mycon = new NpgsqlConnection(constr);
            mycon.Open();
            //MySqlCommand commn = new MySqlCommand("set names gb2312", mycon);

            //commn.ExecuteNonQuery();


             string sql1 = "select ST_AsGEOJSON(geom) FROM  units where id IN (SELECT unitid FROM unitrangers where rangerid = '" + id + "')";


            NpgsqlDataAdapter mda = new NpgsqlDataAdapter(sql1, mycon);
            DataSet ds = new DataSet();
            mda.Fill(ds, "table");
            //ds.Tables["table1"].Rows[0][1].ToString();
            //就是要读取第一行第二列空间数据字段的内容;
            var resultcount = ds.Tables["table"].Rows.Count;
            int i = 0;
            for (i = 0; i < resultcount; i++)
            {
                ProjectAreaModel info = new ProjectAreaModel();

                //info.patrolpoint = ds.Tables["table"].Rows[i][1];
                info.Area= ds.Tables["table"].Rows[i][0];
                data.Add(info);

            }


            mycon.Close();
            if (data.Count == 0)
            {
                return new WebApiResultValue<List<ProjectAreaModel>>(2, data);
            }
            else
            {
                return new WebApiResultValue<List<ProjectAreaModel>>(1, data);
            }
        }
    }
}
