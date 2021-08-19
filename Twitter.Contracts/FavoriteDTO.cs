using System;

namespace Twitter.Contracts
{
    public class FavoriteDTO
    {
        public Guid Id { get; set; }
        public Guid TwitterPostId { get; set; }
        public string UserId { get; set; }
    }
}
