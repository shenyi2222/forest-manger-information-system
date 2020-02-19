using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NpgsqlTypes;

namespace AppBoxPro.Dapper
{
    public class Patrolinfo : IKeyID
    {
        [Key]
        public int ID { get; set; }

        [StringLength(50)]
        public string Town { get; set; }

        [StringLength(50)]
        public string Village { get; set; }

        [Required, StringLength(20)]
        public string Name { get; set; }

        [Required, StringLength(30)]
        public string Tel { get; set; }

        [StringLength(100)]
        public string Path { get; set; }

        [Required, StringLength(50)]
        public DateTime Time { get; set; }

        [NotMapped]
        public string Point { get; set; }

    }
}