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
    public class FavoriteRepositoryTest
    {
        private DbContextOptions<ApplicationContext> _context;
        [SetUp]
        public void Setup()
        {
            _context = UnitTestHelper.GetUnitDbOptions();
        }

        [Test]
        public async Task FavoriteRepository_AddFavorite()
        {
            await using var context = new ApplicationContext(_context);

            var favoriteRepository = new FavoriteRepository(context);
            var favorite = new Favorite { Id = new Guid() };

            await favoriteRepository.AddFavoriteAsync(favorite);
            await context.SaveChangesAsync();

            Assert.That(context.Favorites.Count(), Is.EqualTo(4));
        }

        [TestCase("74d1b908-ff65-4c74-b836-44a4ca840ce8")]
        public async Task FavoriteRepository_DeleteFavoriteById(Guid id)
        {
            await using var context = new ApplicationContext(_context);

            var favoriteRepository = new FavoriteRepository(context);

            favoriteRepository.DeleteFavoriteById(id);
            await context.SaveChangesAsync();

            Assert.That(context.Favorites.Count(), Is.EqualTo(2));
        }

        [TestCase("925695ec-0e70-4e43-8514-8a0710e11d53")]
        public async Task FavoriteRepository_GetFavoritesByUserId(string userId)
        {
            await using var context = new ApplicationContext(_context);

            var favoriteRepository = new FavoriteRepository(context);
            var favorites = await favoriteRepository.GetFavoritesByUserIdAsync(userId);
            var expected = InitialData.ExpectedFavorites.Where(x => x.UserId == userId);

            Assert.That(favorites, Is.EqualTo(expected)
                .Using(new FavoriteEqualityComparer()));
        }
    }
}
