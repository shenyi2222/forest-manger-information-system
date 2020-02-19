using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using webapi.Models.Parameter;

namespace webapi.Controllers
{
    public class tkpointController : ApiController
    {
        [HttpPost]
        public string PostProjecttkpoint(tkpoint model)
        {
            string s;
            var message = model.message;
            //判断发过来的数据块中有几组数据需要处理
            int count = message.Count(ch => ch == '#');

            string[] sArray2 = message.Split(new char[1] { '#' });
            for (int i = 0; i <= count; i++)
            {
                string[] sArray3 = sArray2[i].Split(new char[1] { '|' });
                var rid = Convert.ToInt32(sArray3[0]);
                var longitude = sArray3[1];
                var latitude = sArray3[2];
                var ti = sArray3[3];
                var time = System.Convert.ToDateTime(ti);
                //将数据库中的甄别所需的数据储存在内存中（即datatable）
                string constr = "PORT=5432;DATABASE=PFM;HOST=39.106.41.205;PASSWORD=123456;USER ID=user";
                Npgsql.NpgsqlConnection mycon = new Npgsql.NpgsqlConnection(constr);
                mycon.Open();
                string sql2 = "insert into points (time,rangerid,point) values('" + time + "'," + rid + ",st_GeometryFromText('Point(" + longitude + "    " + latitude + ")',4326))";
                Debug.WriteLine(sql2);
                //var str = string.Format(sql2, tel, longitude, latitude, time);
                NpgsqlCommand mycmd = new NpgsqlCommand(sql2, mycon);
                if (mycmd.ExecuteNonQuery() > 0)
                {
                    Console.WriteLine("数据插入成功！");
                    //判断与数据库连接是否关闭

                    mycon.Close();
                   s = "在线";
                    return s;
                }
                else
                {
                   
                    mycon.Close();
                    s="不在线";
                    return s;

                }  

            }
            s = "不在线";
            return s;

        }
    }
}

