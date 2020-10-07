using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Audit.Net.Models
{
    public class Audit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long EntityId { get; set; }
        public string EntityType { get; set; }
        public DateTime DateTime { get; set; }
        public string Operation { get; set; }
        public long UserId { get; set; }
        public string Data { get; set; }
    }
}
