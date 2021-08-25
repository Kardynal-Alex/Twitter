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
    public class LikeRepositoryTest
    {
        private DbContextOptions<ApplicationContext> _context;
        [SetUp]
        public void Setup()
        {
            _context = UnitTestHelper.GetUnitDbOptions();
        }

        [Test]
        public async Task LikeRepository_AddLike()
        {
            await using var context = new ApplicationContext(_context);
            var likeRepository = new LikeRepository(context);

            var like = new Like { Id = new Guid() };

            await likeRepository.AddLikeAsync(like);
            await context.SaveChangesAsync();

            Assert.That(context.Likes.Count(), Is.EqualTo(4));
        }

        [TestCase("74d1b908-ff65-4c74-b836-44a4ca840ce8")]
        public async Task LikeRepository_DeleteLikeById(Guid id)
        {
            await using var context = new ApplicationContext(_context);
            var likeRepository = new LikeRepository(context);

            likeRepository.DeleteLikeById(id);
            await context.SaveChangesAsync();

            Assert.That(context.Likes.Count(), Is.EqualTo(2));
        }

        [TestCase("925695ec-0e70-4e43-8514-8a0710e11d53")]
        public async Task LikeRepository_GetLikesByUserId(string userId)
        {
            await using var context = new ApplicationContext(_context);
            var likeRepository = new LikeRepository(context);

            var likes = await likeRepository.GetLikesByUserIdAsync(userId);

            Assert.That(likes, Is.EqualTo(InitialData.ExpectedLikes)
                .Using(new LikeEqualityComparer()));
        }

        [Test]
        public async Task LikeRepository_GetLikeByUserAndTwitterPostId()
        {
            await using var context = new ApplicationContext(_context);
            var likeRepository = new LikeRepository(context);
            var like = new Like
            {
                UserId = "925695ec-0e70-4e43-8514-8a0710e11d53",
                TwitterPostId = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")
            };

            var actual = await likeRepository.GetLikeByUserAndTwitterPostIdAsync(like);
            var expected = InitialData.ExpectedLikes.ElementAt(0);

            Assert.NotNull(actual);
            Assert.NotNull(expected.Id);
            Assert.AreEqual(actual.TwitterPostId, expected.TwitterPostId);
            Assert.AreEqual(actual.UserId, expected.UserId);
        }
    }
}
