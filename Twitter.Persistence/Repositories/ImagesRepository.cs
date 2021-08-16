using System;
using System.Collections.Generic;
using System.Reflection;
using Twitter.Domain.Entities;
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

        public void DeletePhysicalImages(Images images)
        {
            Type t = typeof(Images);
            PropertyInfo[] props = t.GetProperties();
            for (int i = 1; i < props.Length - 1; i++) 
            {
                var path = props[i].GetValue(images).ToString();
                if (path != "" && System.IO.File.Exists(path)) 
                {
                    System.IO.File.Delete(path);
                }
            }
        }
    }
}
