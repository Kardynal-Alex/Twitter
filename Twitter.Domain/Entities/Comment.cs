using NJsonSchema.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Twitter.Domain.Entities
{
    public class Comment
    {
        [Key]
        public Guid Id { get; set; }
        public string Author { get; set; }
        public string Text { get; set; }
        [JsonSchemaDate]
        public DateTime DateCreation { get; set; }
        public Guid TwitterPostId { get; set; }
        public string UserId { get; set; }
    }
}
