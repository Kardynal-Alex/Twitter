using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twitter.Contracts;
using Twitter.Domain.Entities;
using Twitter.Domain.Exceptions;
using Twitter.Domain.Repositories;
using Twitter.Persistence;
using Twitter.Services;

namespace Twitter.Tests.ServiceTests
{
    [TestFixture]
    public class FavoriteServiceTest
    {
        [Test]
        public async Task FavoriteService_AddFavorite()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.FavoriteRepository.AddFavoriteAsync(It.IsAny<Favorite>()));

            var favoriteService = new FavoriteService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            var favoriteDTO = new FavoriteDTO
            {
                Id = new Guid("94d1b908-ff65-4c74-b836-44a4ca840ce8"),
                TwitterPostId = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                UserId = "925695ec-0e70-4e43-8514-8a0710e11d53"
            };
            await favoriteService.AddFavoriteAsync(favoriteDTO);

            mockUnitOfWork.Verify(x => x.FavoriteRepository.AddFavoriteAsync(It.Is<Favorite>(x =>
                  x.Id == favoriteDTO.Id && x.UserId == favoriteDTO.UserId && x.TwitterPostId == favoriteDTO.TwitterPostId)), Times.Once);

            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Test]
        public void FavoriteService_AddFavorite_ThrowTwitterExceptionIfModelIsIncorrect()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.FavoriteRepository.AddFavoriteAsync(It.IsAny<Favorite>()));

            var favoriteService = new FavoriteService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());
            //UserId is empty
            var favoriteDTO = new FavoriteDTO
            {
                Id = new Guid("94d1b908-ff65-4c74-b836-44a4ca840ce8"),
                TwitterPostId = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                UserId = ""
            };
            Assert.ThrowsAsync<TwitterException>(async () => await favoriteService.AddFavoriteAsync(favoriteDTO));
        }

        [TestCase("74d1b908-ff65-4c74-b836-44a4ca840ce8")]
        public async Task FavoriteService_DeleteFavoriteById(Guid id)
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.FavoriteRepository.DeleteFavoriteById(It.IsAny<Guid>()));

            var favoriteService = new FavoriteService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            await favoriteService.DeleteFavoriteByIdAsync(id);

            mockUnitOfWork.Verify(x => x.FavoriteRepository.DeleteFavoriteById(id), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        }

        [TestCase("925695ec-0e70-4e43-8514-8a0710e11d53")]
        public async Task FavoriteService_GetFavoritesByUserId(string userId)
        {
            var favorites = InitialData.ExpectedFavorites.Where(x => x.UserId == userId);
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.FavoriteRepository.GetFavoritesByUserIdAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(favorites.ToList()));

            var favoriteService = new FavoriteService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());
            var actual = await favoriteService.GetFavoritesByUserIdAsync(userId);
            var expected = InitialData.ExpectedFavoriteDTOs.Where(x => x.UserId == userId);

            Assert.That(actual, Is.InstanceOf<List<FavoriteDTO>>());
            Assert.That(actual,Is.EqualTo(expected)
                .Using(new FavoriteDTOEqualityComparer()));
        }
    }
}
