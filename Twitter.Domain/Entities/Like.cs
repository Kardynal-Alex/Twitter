using System;
using System.ComponentModel.DataAnnotations;

namespace Twitter.Domain.Entities
{
    public class Like
    {
        [Key]
        public Guid Id { get; set; }
        public Guid TwitterPostId { get; set; }
        public string UserId { get; set; }
    }
}
