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
    public class FriendRepositoryTests
    {
        private DbContextOptions<ApplicationContext> _context;
        [SetUp]
        public void Setup()
        {
            _context = UnitTestHelper.GetUnitDbOptions();
        }

        [Test]
        public async Task FriendRepository_AddFriend()
        {
            await using var context = new ApplicationContext(_context);
            var friendRepository = new FriendRepository(context);

            var friend = new Friend { Id = new Guid() };

            await friendRepository.AddFriendAsync(friend);
            await context.SaveChangesAsync();

            Assert.That(context.Friends.Count, Is.EqualTo(3));
        }

        [TestCase("b4edd1e5-c05d-ee7d-ed93-4603de11d462")]
        public async Task FriendRepository_DeleteFriendById(Guid id)
        {
            await using var context = new ApplicationContext(_context);
            var friendRepository = new FriendRepository(context);

            friendRepository.DeleteFriendById(id);
            await context.SaveChangesAsync();

            Assert.That(context.Friends.Count, Is.EqualTo(1));
        }

        [TestCase("b4edd1e5-c05d-ee7d-ed93-4603de11d462")]
        public async Task FriendRepository_GetFriendById(Guid id)
        {
            await using var context = new ApplicationContext(_context);
            var friendRepository = new FriendRepository(context);

            var friend = await friendRepository.GetFriendByIdAsync(id);

            Assert.That(friend, Is.EqualTo(InitialData.ExpectedFriends.ElementAt(0))
                .Using(new FriendEqualityComparer()));
        }
    }
}
