using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using Dapper;
using System.Collections.Generic;

namespace AppBoxPro.Dapper
{
    public class WhereBuilder
    {
        private DynamicParameters _parameters = new DynamicParameters();

        public DynamicParameters Parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }

        private List<string> _wheres = new List<string>();

        public List<string> Wheres
        {
            get { return _wheres; }
            set { _wheres = value; }
        }


        private string _fromSql = string.Empty;

        public string FromSql
        {
            get { return _fromSql; }
            set { _fromSql = value; }
        }


        /// <summary>
        /// 添加检索条件
        /// </summary>
        /// <param name="item"></param>
        public void AddWhere(string item)
        {
            _wheres.Add(item);
        }


        /// <summary>
        /// 添加条件参数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void AddParameter(string name, object value)
        {
            _parameters.Add(name, value);
        }


    }
}
