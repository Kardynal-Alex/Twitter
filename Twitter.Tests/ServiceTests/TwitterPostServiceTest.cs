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
        public async Task TwitterPost_AddTwitterPost()
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
        public void TwitterPost_AddTwitterPost_ThrowTwitterExceptionIfModelIsIncorrect()
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
        public async Task TwitterPost_GetTwitterPostByUserIdWithDetailsExceptUser(string userId)
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
        public async Task TwitterPost_DeleteTwitterPostWithImages()
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
        public async Task TwitterPost_GetTwitterPostByIdWithDetails(Guid twitterPostId)
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

    }
}
