using NJsonSchema.Annotations;
using System;

namespace Twitter.Contracts
{
    public class CommentDTO
    {
        public Guid Id { get; set; }
        public string Author { get; set; }
        public string Text { get; set; }
        [JsonSchemaDate]
        public DateTime DateCreation { get; set; }
        public Guid TwitterPostId { get; set; }
        public string UserId { get; set; }
        public string ProfileImagePath { get; set; }
    }
}
