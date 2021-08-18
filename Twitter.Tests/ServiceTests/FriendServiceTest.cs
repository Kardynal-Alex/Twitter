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
    public class FriendServiceTest
    {
        [Test]
        public async Task FriendService_AddFriend()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.FriendRepository.AddFriendAsync(It.IsAny<Friend>()));

            var friendService = new FriendService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());
            var friendDTO = new FriendDTO
            {
                Id = new Guid("be196775-47b5-a4ad-d520-60bb02fefe01"),
                UserId = "925695ec-0e70-4e43-8514-8a0710e11d53",
                FriendId = "5ae019a1-c312-4589-ab62-8b8a1fcb882c"
            };

            await friendService.AddFriendAsync(friendDTO);

            mockUnitOfWork.Verify(x => x.FriendRepository.AddFriendAsync(It.Is<Friend>(x =>
                  x.Id == friendDTO.Id && x.UserId == friendDTO.UserId && x.FriendId == friendDTO.FriendId)), Times.Once);

            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Test]
        public void FriendService_AddFriend_ThrowTwitterExceptionIfModelIsIncorrect()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.FriendRepository.AddFriendAsync(It.IsAny<Friend>()));

            var friendService = new FriendService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());
            //UserId is incorect
            var friendDTO = new FriendDTO
            {
                Id = new Guid("be196775-47b5-a4ad-d520-60bb02fefe01"),
                UserId = "",
                FriendId = "5ae019a1-c312-4589-ab62-8b8a1fcb882c"
            };
            Assert.ThrowsAsync<TwitterException>(async () => await friendService.AddFriendAsync(friendDTO));

            //FriendId is incorect
            friendDTO.UserId = "925695ec-0e70-4e43-8514-8a0710e11d53";
            friendDTO.FriendId = "";
            Assert.ThrowsAsync<TwitterException>(async () => await friendService.AddFriendAsync(friendDTO));
        }

        [TestCase("b4edd1e5-c05d-ee7d-ed93-4603de11d462")]
        public async Task FriendService_DeleteFriendById(Guid id)
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.FriendRepository.AddFriendAsync(It.IsAny<Friend>()));

            var friendService = new FriendService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            await friendService.DeleteFriendByIdAsync(id);

            mockUnitOfWork.Verify(x => x.FriendRepository.DeleteFriendById(id), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        }

        [TestCase("b4edd1e5-c05d-ee7d-ed93-4603de11d462")]
        public async Task FriendService_GetFriendByIdById(Guid id)
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.FriendRepository.GetFriendByIdAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(InitialData.ExpectedFriends.ElementAt(0)));

            var friendService = new FriendService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            var friendDTO = await friendService.GetFriendByIdAsync(id);

            Assert.That(friendDTO, Is.EqualTo(InitialData.ExpectedFriendDTOs.ElementAt(0))
                .Using(new FriendDTOEqualityComparer()));
        }
    }
}
