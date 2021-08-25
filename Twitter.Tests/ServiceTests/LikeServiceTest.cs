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
    public class LikeServiceTest
    {
        [Test]
        public async Task LikeService_AddLike()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.LikeRepository.AddLikeAsync(It.IsAny<Like>()));

            var likeService = new LikeService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            var likeDTO = new LikeDTO
            {
                Id = new Guid("94d1b908-ff65-4c74-b836-44a4ca840ce8"),
                UserId = "5ae019a1-c312-4589-ab62-8b8a1fcb882c",
                TwitterPostId = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")
            };
            await likeService.AddLikeAsync(likeDTO);

            mockUnitOfWork.Verify(x => x.LikeRepository.AddLikeAsync(It.Is<Like>(x =>
                  x.Id == likeDTO.Id && x.UserId == likeDTO.UserId && x.TwitterPostId == likeDTO.TwitterPostId)), Times.Once);

            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Test]
        public void LikeService_AddLike_ThrowTwitterExceptionIfModelIsIncorrect()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.LikeRepository.AddLikeAsync(It.IsAny<Like>()));

            var likeService = new LikeService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            //User Id is empty
            var likeDTO = new LikeDTO
            {
                Id = new Guid("94d1b908-ff65-4c74-b836-44a4ca840ce8"),
                UserId = "",
                TwitterPostId = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")
            };
            Assert.ThrowsAsync<TwitterException>(async () => await likeService.AddLikeAsync(likeDTO));
        }

        [TestCase("74d1b908-ff65-4c74-b836-44a4ca840ce8")]
        public async Task LikeService_DeleteLikeById(Guid id)
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.LikeRepository.DeleteLikeById(It.IsAny<Guid>()));

            var likeService = new LikeService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            await likeService.DeleteLikeByIdAsync(id);

            mockUnitOfWork.Verify(x => x.LikeRepository.DeleteLikeById(id), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        }

        [TestCase("925695ec-0e70-4e43-8514-8a0710e11d53")]
        public async Task LikeService_GetLikesByUserId(string userId)
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.LikeRepository.GetLikesByUserIdAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(InitialData.ExpectedLikes.ToList()));

            var likeService = new LikeService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            var actual = await likeService.GetLikesByUserIdAsync(userId);

            Assert.That(actual, Is.EqualTo(InitialData.ExpectedLikeDTOs)
                .Using(new LikeDTOEqualityComparer()));
        }
    }
}
