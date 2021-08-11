using System;
using System.Collections.Generic;
using Twitter.Domain.Repositories;

namespace Twitter.Persistence.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationContext context;
        public CommentRepository(ApplicationContext context)
        {
            this.context = context;
        }
    }
}
