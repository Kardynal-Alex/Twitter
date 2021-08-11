using NJsonSchema.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Twitter.Domain.Entities
{
    public class TwitterPost
    {
        [Key]
        public Guid Id { get; set; }
        public string PostText { get; set; }
        [JsonSchemaDate]
        public DateTime DateCreation { get; set; }
        public int Like { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public Images Images { get; set; }
    }
}
