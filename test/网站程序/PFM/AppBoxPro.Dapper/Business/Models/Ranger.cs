using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppBoxPro.Dapper
{
    public class Ranger : IKeyID
    {
        [Key]
        public int ID { get; set; }

        [Required, StringLength(20)]
        public string Name { get; set; }

        [StringLength(20)]
        public string Password { get; set; }

        [StringLength(20)]
        public string Character { get; set; }

        [Required, StringLength(30)]
        public string Tel { get; set; }

        [StringLength(50)]
        public string Town { get; set; }

        [StringLength(50)]
        public string Village { get; set; }
    }
}