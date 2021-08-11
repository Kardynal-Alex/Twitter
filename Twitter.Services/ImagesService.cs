using System;
using System.Collections.Generic;
using System.Text;
using Twitter.Domain.Repositories;
using Twitter.Services.Abstractions;

namespace Twitter.Services
{
    public class ImagesService : IImagesService
    {
        private readonly IUnitOfWork unitOfWork;
        public ImagesService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
    }
}
