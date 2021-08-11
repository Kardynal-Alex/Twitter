using System;
using System.Collections.Generic;
using Twitter.Domain.Repositories;

namespace Twitter.Persistence.Repositories
{
    public class TwitterPostRepository : ITwitterPostRepository
    {
        private readonly ApplicationContext context;
        public TwitterPostRepository(ApplicationContext context)
        {
            this.context = context;
        }
    }
}
