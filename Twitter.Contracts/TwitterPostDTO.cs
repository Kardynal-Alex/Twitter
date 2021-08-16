using NJsonSchema.Annotations;
using System;
using System.Collections.Generic;

namespace Twitter.Contracts
{
    public class TwitterPostDTO
    {
        public Guid Id { get; set; }
        public string PostText { get; set; }
        [JsonSchemaDate]
        public DateTime DateCreation { get; set; }
        public int Like { get; set; }

        public string UserId { get; set; }
        public UserDTO User { get; set; }

        public ImagesDTO Images { get; set; }

        public ICollection<CommentDTO> Comments { get; set; }

    }
}
