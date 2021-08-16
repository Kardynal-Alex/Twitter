using System;
using System.Collections.Generic;
using Twitter.Domain.Entities;

namespace Twitter.Domain.Repositories
{
    public interface IImagesRepository
    {
        void DeletePhysicalImages(Images images);
    }
}
