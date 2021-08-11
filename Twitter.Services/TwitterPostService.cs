using System;
using System.Collections.Generic;
using System.Text;
using Twitter.Domain.Repositories;
using Twitter.Services.Abstractions;

namespace Twitter.Services
{
    public class TwitterPostService : ITwitterPostService
    {
        private readonly IUnitOfWork unitOfWork;
        public TwitterPostService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
    }
}
