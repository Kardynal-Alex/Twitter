using System;

namespace Twitter.Domain.Entities
{
    public class Friend
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string FriendId { get; set; }
    }
}
