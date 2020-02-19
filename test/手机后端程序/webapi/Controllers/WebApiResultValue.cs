using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webapi.Controllers
{
    public class WebApiResultValue<T>
    {
        public int Status { get; set; }
        public T ReturnData { get; set; }
        public WebApiResultValue(int status, T data)
        {
            this.Status = status;
            this.ReturnData = data;
        }
    }
}