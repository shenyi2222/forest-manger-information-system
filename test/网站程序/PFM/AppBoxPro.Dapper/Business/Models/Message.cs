using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppBoxPro.Dapper
{
    public class Message : IKeyID
    {
        [Key]
        public int ID { get; set; }

        [StringLength(255)]
        public string Recipients { get; set; }

        [Required, StringLength(255)]
        public string Content { get; set; }

        [Required, StringLength(255)]
        public string Sender { get; set; }

        [Required]
        public DateTime Time { get; set; }

    }
}