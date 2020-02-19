using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webapi.Models.Business
{
    public class ProjectInfoListModel
    {
        public object id { get; set; }
        public object tel { get; set; }
        public object name { get; set; }
        public object town { get; set; }
        public object village { get; set; }
        public object character { get; set; }
    }
    public class ProjectPointModel
    {
        public object ckpoint { get; set; }
    }
    public class ProjectAreaModel
    {
        public object Area { get; set; }
    }
    public class ProjecttkPointModel
    {
        public object message { get; set; }
    }

}