using System;

namespace Twitter.Contracts
{
    public class FriendDTO
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string FriendId { get; set; }
    }
}
