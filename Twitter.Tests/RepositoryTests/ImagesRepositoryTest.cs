using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twitter.Domain.Entities;
using Twitter.Persistence;
using Twitter.Persistence.Repositories;

namespace Twitter.Tests.RepositoryTests
{
    [TestFixture]
    public class ImagesRepositoryTest
    {
        private DbContextOptions<ApplicationContext> _context;
        [SetUp]
        public void Setup()
        {
            _context = UnitTestHelper.GetUnitDbOptions();
        }

        [Test]
        public async Task ImagesRepository_UpdateImages()
        {
            await using var context = new ApplicationContext(_context);
            var imagesRepository = new ImagesRepository(context);

            var images = new Images
            {
                Id = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                Image1 = "Resources\\Images\\111-1.jpg",
                Image2 = "Resources\\Images\\111-2.jpg",
                Image3 = "Resources\\Images\\111-3.jpg",
                Image4 = "Resources\\Images\\111-4.jpg"
            };

            imagesRepository.UpdateImages(images);
            await context.SaveChangesAsync();

            var expectedImages = await context.Images.FirstOrDefaultAsync(x => x.Id == images.Id);
            Assert.That(images, Is.EqualTo(expectedImages)
                .Using(new ImagesEqualityComparer()));
        }
    }
}
