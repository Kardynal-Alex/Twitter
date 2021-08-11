using System;
using System.Collections.Generic;
using Twitter.Domain.Repositories;

namespace Twitter.Persistence.Repositories
{
    public class ImagesRepository : IImagesRepository
    {
        private readonly ApplicationContext context;
        public ImagesRepository(ApplicationContext context)
        {
            this.context = context;
        }
    }
}
