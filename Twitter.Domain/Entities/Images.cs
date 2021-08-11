using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Twitter.Domain.Entities
{
    public class Images
    {
        [Key]
        [ForeignKey("TwitterPost")]
        public Guid Id { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public string Image4 { get; set; }

        public TwitterPost TwitterPost { get; set; }
    }
}
