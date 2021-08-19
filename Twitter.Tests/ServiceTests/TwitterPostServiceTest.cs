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
    public class TwitterPostServiceTest
    {
        private List<TwitterPost> TwitterPosts;
        private List<TwitterPostDTO> TwitterPostDTOs;
        [SetUp]
        public void Init()
        {
            using (var context = new ApplicationContext(UnitTestHelper.GetUnitDbOptions()))
            {
                TwitterPosts = context.TwitterPosts
                    .Include(x => x.Images)
                    .Include(x => x.User)
                    .Include(x => x.Comments).ToList();
                var config = new MapperConfiguration(cfg => {
                    cfg.CreateMap<TwitterPost, TwitterPostDTO>();
                    cfg.CreateMap<User, UserDTO>();
                    cfg.CreateMap<Comment, CommentDTO>();
                    cfg.CreateMap<Images, ImagesDTO>();
                });
                var mapper = new Mapper(config);
                TwitterPostDTOs = mapper.Map<List<TwitterPostDTO>>(TwitterPosts);
            }
        }

        [Test]
        public async Task TwitterPostService_AddTwitterPost()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.TwitterPostRepository.AddTwitterPostAsync(It.IsAny<TwitterPost>()));

            var twitterPostService = new TwitterPostService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            var twitterPostDTO = new TwitterPostDTO
            {
                Id = new Guid("babaaaab-baaa-baaa-aaaa-aaaaaaaaaaaa"),
                PostText = "TwitterPost text new",
                DateCreation = DateTime.Now.Date,
                Like = 0,
                UserId = "925695ec-0e70-4e43-8514-8a0710e11d53",
                Images = new ImagesDTO
                {
                    Id = new Guid("babaaaab-baaa-baaa-aaaa-aaaaaaaaaaaa"),
                    Image1 = "Resources\\Images\\11-1.jpg",
                    Image2 = "Resources\\Images\\11-2.jpg",
                    Image3 = "Resources\\Images\\11-3.jpg",
                    Image4 = "Resources\\Images\\11-4.jpg"
                }
            };

            await twitterPostService.AddTwitterPostsAsync(twitterPostDTO);

            mockUnitOfWork.Verify(x => x.TwitterPostRepository.AddTwitterPostAsync(It.Is<TwitterPost>(x =>
                  x.Id == twitterPostDTO.Id && x.PostText == twitterPostDTO.PostText &&
                  x.DateCreation == twitterPostDTO.DateCreation && x.Like == twitterPostDTO.Like &&
                  x.UserId == twitterPostDTO.UserId && x.Images.Image1 == twitterPostDTO.Images.Image1 &&
                  x.Images.Image2 == twitterPostDTO.Images.Image2 && x.Images.Image3 == twitterPostDTO.Images.Image3 &&
                  x.Images.Image4 == twitterPostDTO.Images.Image4)));
            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Test]
        public void TwitterPostService_AddTwitterPost_ThrowTwitterExceptionIfModelIsIncorrect()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.TwitterPostRepository.AddTwitterPostAsync(It.IsAny<TwitterPost>()));

            var twitterPostService = new TwitterPostService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());
            //PostText is empty
            var twitterPostDTO = new TwitterPostDTO
            {
                Id = new Guid("babaaaab-baaa-baaa-aaaa-aaaaaaaaaaaa"),
                PostText = "",
                DateCreation = DateTime.Now.Date,
                Like = 0,
                UserId = "925695ec-0e70-4e43-8514-8a0710e11d53"
            };
            Assert.ThrowsAsync<TwitterException>(async () => await twitterPostService.AddTwitterPostsAsync(twitterPostDTO));

            //Like is negative number
            twitterPostDTO.PostText = "TwitterPost text new";
            twitterPostDTO.Like = -1;
            Assert.ThrowsAsync<TwitterException>(async () => await twitterPostService.AddTwitterPostsAsync(twitterPostDTO));

            //UserId is empty
            twitterPostDTO.Like = 0;
            twitterPostDTO.UserId = "";
            Assert.ThrowsAsync<TwitterException>(async () => await twitterPostService.AddTwitterPostsAsync(twitterPostDTO));
        }

        [TestCase("925695ec-0e70-4e43-8514-8a0710e11d53")]
        public async Task TwitterPostService_GetTwitterPostByUserIdWithDetailsExceptUser(string userId)
        {
            var twitterPosts = TwitterPosts.Where(x => x.UserId == userId).ToList();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.TwitterPostRepository
                .GetTwitterPostByUserIdWithDetailsExceptUserAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(twitterPosts));

            var twitterPostService = new TwitterPostService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());
            var actualTwitterPostDTOs = await twitterPostService.GetTwitterPostByUserIdAsync(userId);
            var expectedTwitterPostDTOs = TwitterPostDTOs.Where(x => x.UserId == userId).ToList();

            Assert.That(actualTwitterPostDTOs, Is.InstanceOf<List<TwitterPostDTO>>());
            Assert.That(actualTwitterPostDTOs, Is.EqualTo(expectedTwitterPostDTOs)
                .Using(new TwitterPostDTOEqualityComparer()));
            Assert.That(actualTwitterPostDTOs.Select(x => x.Images), Is.EqualTo(expectedTwitterPostDTOs.Select(x => x.Images))
                .Using(new ImagesDTOEqualityComparer()));
        }

        [Test]
        public async Task TwitterPostService_DeleteTwitterPostWithImages()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.TwitterPostRepository.DeleteTwitterPostById(It.IsAny<Guid>()));
            mockUnitOfWork.Setup(x => x.ImagesRepository.DeletePhysicalImages(It.IsAny<Images>()));

            var twitterPostService = new TwitterPostService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());
            var twitterPostDTO = new TwitterPostDTO
            {
                Id = new Guid("babaaaab-baaa-baaa-aaaa-aaaaaaaaaaaa"),
                PostText = "TwitterPost text new",
                DateCreation = DateTime.Now.Date,
                Like = 0,
                UserId = "925695ec-0e70-4e43-8514-8a0710e11d53",
                Images = new ImagesDTO
                {
                    Id = new Guid("babaaaab-baaa-baaa-aaaa-aaaaaaaaaaaa"),
                    Image1 = "Resources\\Images\\11-1.jpg",
                    Image2 = "Resources\\Images\\11-2.jpg",
                    Image3 = "Resources\\Images\\11-3.jpg",
                    Image4 = "Resources\\Images\\11-4.jpg"
                }
            };
            await twitterPostService.DeleteTwitterPostWithImagesAsync(twitterPostDTO);

            mockUnitOfWork.Verify(x => x.TwitterPostRepository
                .DeleteTwitterPostById(twitterPostDTO.Id), Times.Once);
            mockUnitOfWork.Verify(x => x.ImagesRepository.DeletePhysicalImages(It.Is<Images>(x =>
                  x.Image1 == twitterPostDTO.Images.Image1 && x.Image2 == twitterPostDTO.Images.Image2 &&
                  x.Image3 == twitterPostDTO.Images.Image3 && x.Image4 == twitterPostDTO.Images.Image4)));
            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        }

        [TestCase("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")]
        public async Task TwitterPostService_GetTwitterPostByIdWithDetails(Guid twitterPostId)
        {
            var twitterPost = TwitterPosts.FirstOrDefault(x => x.Id == twitterPostId);
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.TwitterPostRepository.GetTwitterPostByIdWithDetails(It.IsAny<Guid>()))
                .Returns(Task.FromResult(twitterPost));

            var twitterPostService = new TwitterPostService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());
            var actualTwitterPost = await twitterPostService.GetTwitterPostByIdWithDetails(twitterPostId);
            var expectedTwitterPost = TwitterPostDTOs.FirstOrDefault(x => x.Id == twitterPostId);

            Assert.That(actualTwitterPost, Is.EqualTo(expectedTwitterPost)
                .Using(new TwitterPostDTOEqualityComparer()));
            Assert.That(actualTwitterPost.Images, Is.EqualTo(expectedTwitterPost.Images)
                .Using(new ImagesDTOEqualityComparer()));
            Assert.That(actualTwitterPost.User, Is.EqualTo(expectedTwitterPost.User)
                .Using(new UserDTOEqualityComparer()));
            Assert.That(actualTwitterPost.Comments, Is.EqualTo(expectedTwitterPost.Comments)
                .Using(new CommentDTOEqualityComparer()));
        }

        [TestCase("925695ec-0e70-4e43-8514-8a0710e11d53")]
        public async Task TwitterPostService_GetTwitterPostsByUserIdWithImagesAndUsers(string userId)
        {
            var twitterPosts = TwitterPosts.Where(x => x.UserId == userId).ToList();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.TwitterPostRepository
                .GetTwitterPostsByUserIdWithImagesAndUsers(It.IsAny<string>()))
                .Returns(Task.FromResult(twitterPosts));

            var twitterPostService = new TwitterPostService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());
            var actualTwitterPostDTOs = await twitterPostService.GetTwitterPostsByUserIdWithImagesAndUsers(userId);
            var expectedTwitterPostDTOs = TwitterPostDTOs.Where(x => x.UserId == userId).ToList();

            Assert.That(actualTwitterPostDTOs, Is.InstanceOf<List<TwitterPostDTO>>());
            Assert.That(actualTwitterPostDTOs, Is.EqualTo(expectedTwitterPostDTOs)
                .Using(new TwitterPostDTOEqualityComparer()));
            Assert.That(actualTwitterPostDTOs.Select(x => x.Images), Is.EqualTo(expectedTwitterPostDTOs.Select(x => x.Images))
                .Using(new ImagesDTOEqualityComparer()));
            Assert.That(actualTwitterPostDTOs[0].User, Is.EqualTo(expectedTwitterPostDTOs[0].User)
                .Using(new UserDTOEqualityComparer()));
        }

        [Test]
        public async Task TwitterPostService_UpdateTwitterPostWithImagesAsync()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork.Setup(x => x.TwitterPostRepository.UpdateTwitterPost(It.IsAny<TwitterPost>()));
            mockUnitOfWork.Setup(x => x.ImagesRepository.UpdateImages(It.IsAny<Images>()));
            var twitterPostService = new TwitterPostService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            var twitterPostDTO = new TwitterPostDTO
            {
                Id = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                PostText = "update TwitterPost text1",
                DateCreation = DateTime.Now.Date,
                Like = 0,
                UserId = "925695ec-0e70-4e43-8514-8a0710e11d53",
                Images = new ImagesDTO
                {
                    Id = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    Image1 = "Resources\\Images\\111-1.jpg",
                    Image2 = "Resources\\Images\\111-2.jpg",
                    Image3 = "Resources\\Images\\111-3.jpg",
                    Image4 = "Resources\\Images\\111-4.jpg"
                }
            };
            await twitterPostService.UpdateTwitterPostWithImagesAsync(twitterPostDTO);

            mockUnitOfWork.Verify(x => x.TwitterPostRepository.UpdateTwitterPost(It.Is<TwitterPost>(x =>
                  x.Id == twitterPostDTO.Id && x.PostText == twitterPostDTO.PostText &&
                  x.DateCreation.Date == twitterPostDTO.DateCreation.Date && x.Like == twitterPostDTO.Like &&
                  x.UserId == twitterPostDTO.UserId)), Times.Once);

            mockUnitOfWork.Verify(x => x.ImagesRepository.UpdateImages(It.Is<Images>(x =>
                  x.Id == twitterPostDTO.Images.Id && x.Image1 == twitterPostDTO.Images.Image1 &&
                  x.Image2 == twitterPostDTO.Images.Image2 && x.Image3 == twitterPostDTO.Images.Image3 &&
                  x.Image4 == twitterPostDTO.Images.Image4)), Times.Once);

            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        }

        [TestCase("925695ec-0e70-4e43-8514-8a0710e11d53")]
        public async Task TwitterPostService_GetFriendsTweetsByUserId(string userId)
        {
            var twitterPosts = TwitterPosts.ToList();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.TwitterPostRepository.GetFriendsTweetsByUserIdAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(twitterPosts));

            var twitterPostService = new TwitterPostService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());
            var actual = await twitterPostService.GetFriendsTweetsByUserIdAsync(userId);
            var expectedTwitterPostDTOs = TwitterPostDTOs.OrderBy(x => x.Id);
            var expectedImagesDTOs = expectedTwitterPostDTOs.Select(x => x.Images).OrderBy(x => x.Id);
            var expectedUserDTOs = expectedTwitterPostDTOs.Select(x => x.User).OrderBy(x => x.Id);
            var actualUserDTOs = actual.Select(x => x.User).OrderBy(x => x.Id);

            Assert.That(actual.OrderBy(x => x.Id), Is.EqualTo(expectedTwitterPostDTOs)
                .Using(new TwitterPostDTOEqualityComparer()));
            Assert.That(actual.Select(x => x.Images).OrderBy(x => x.Id), Is.EqualTo(expectedImagesDTOs)
                .Using(new ImagesDTOEqualityComparer()));
            Assert.That(actualUserDTOs, Is.EqualTo(expectedUserDTOs)
                .Using(new UserDTOEqualityComparer()));
        }

    }
}
