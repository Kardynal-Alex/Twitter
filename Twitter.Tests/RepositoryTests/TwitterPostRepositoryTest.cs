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
    public class TwitterPostRepositoryTest
    {
        private DbContextOptions<ApplicationContext> _context;
        [SetUp]
        public void Setup()
        {
            _context = UnitTestHelper.GetUnitDbOptions();
        }

        [TestCase("445c79d9-faae-470f-a670-bc5155b1cc19")]
        public async Task TwitterPostRepository_AddLot(Guid id)
        {
            await using var context = new ApplicationContext(_context);

            var twitterRepository = new TwitterPostRepository(context);
            var twitterPost = new TwitterPost { Id = id };

            await twitterRepository.AddTwitterPostAsync(twitterPost);
            await context.SaveChangesAsync();

            Assert.That(context.TwitterPosts.Count(), Is.EqualTo(5));
        }

        [TestCase("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")]
        public async Task TwitterPostRepository_DeleteLotById(Guid id)
        {
            await using var context = new ApplicationContext(_context);

            var twitterRepository = new TwitterPostRepository(context);

            twitterRepository.DeleteTwitterPostById(id);
            await context.SaveChangesAsync();

            Assert.That(context.TwitterPosts.Count(), Is.EqualTo(3));
        }

        [Test]
        public async Task TwitterPostRepository_UpdateLot()
        {
            await using var context = new ApplicationContext(_context);

            var twitterRepository = new TwitterPostRepository(context);
            var twitterPost = new TwitterPost
            {
                Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                PostText = "TwitterPost text1 text1",
                DateCreation = DateTime.Now.Date,
                Like = 1,
                UserId = "925695ec-0e70-4e43-8514-8a0710e11d53"
            };

            twitterRepository.UpdateTwitterPost(twitterPost);
            await context.SaveChangesAsync();

            Assert.That(twitterPost, Is.EqualTo(new TwitterPost
            {
                Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                PostText = "TwitterPost text1 text1",
                DateCreation = DateTime.Now.Date,
                Like = 1,
                UserId = "925695ec-0e70-4e43-8514-8a0710e11d53"
            }).Using(new TwitterPostEqualityComparer()));
        }

    }
}
