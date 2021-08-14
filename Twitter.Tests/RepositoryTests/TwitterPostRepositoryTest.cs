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
        public async Task TwitterPostRepository_AddTwitterPost(Guid id)
        {
            await using var context = new ApplicationContext(_context);

            var twitterRepository = new TwitterPostRepository(context);
            var twitterPost = new TwitterPost { Id = id };

            await twitterRepository.AddTwitterPostAsync(twitterPost);
            await context.SaveChangesAsync();

            Assert.That(context.TwitterPosts.Count(), Is.EqualTo(5));
        }

        [TestCase("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")]
        public async Task TwitterPostRepository_DeleteTwitterPostById(Guid id)
        {
            await using var context = new ApplicationContext(_context);

            var twitterRepository = new TwitterPostRepository(context);

            twitterRepository.DeleteTwitterPostById(id);
            await context.SaveChangesAsync();

            Assert.That(context.TwitterPosts.Count(), Is.EqualTo(3));
        }

        [Test]
        public async Task TwitterPostRepository_UpdateTwitterPost()
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

        [TestCase("925695ec-0e70-4e43-8514-8a0710e11d53")]
        public async Task TwitterPostRepository_GetTwitterPostsByUserIdWithDetailsExceptUser(string userId)
        {
            await using var context = new ApplicationContext(_context);

            var twitterRepository = new TwitterPostRepository(context);
            var twitterPosts = await twitterRepository.GetTwitterPostByUserIdWithDetailsExceptUserAsync(userId);

            var expectedTwitterPosts = ExpectedTwitterPosts.Where(x => x.UserId == userId).ToList();
            var expectedImages = ExpectedImages.Join(expectedTwitterPosts, images => images.Id, twitterPost => twitterPost.Id,
                (images, twitterPost) => images).Distinct().ToList();
            
            Assert.That(twitterPosts, Is.EqualTo(expectedTwitterPosts)
                .Using(new TwitterPostEqualityComparer()));
            Assert.That(twitterPosts.Select(x => x.Images), Is.EqualTo(expectedImages)
                .Using(new ImagesEqualityComparer()));
        }

        #region data
        private static IEnumerable<TwitterPost> ExpectedTwitterPosts =>
            new[]
            {
                new TwitterPost
                {
                    Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    PostText = "TwitterPost text1",
                    DateCreation = DateTime.Now.Date,Like = 0,UserId = "925695ec-0e70-4e43-8514-8a0710e11d53"
                },
                new TwitterPost
                {
                    Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                    PostText = "TwitterPost text2",
                    DateCreation = DateTime.Now.Date,Like = 3,UserId = "925695ec-0e70-4e43-8514-8a0710e11d53"
                },
                new TwitterPost
                {
                    Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                    PostText = "TwitterPost text3",
                    DateCreation = DateTime.Now.Date,Like = 0,UserId = "5ae019a1-c312-4589-ab62-8b8a1fcb882c"
                },
                new TwitterPost
                {
                    Id = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                    PostText = "TwitterPost text4",
                    DateCreation = DateTime.Now.Date,Like = 5,UserId = "5ae019a1-c312-4589-ab62-8b8a1fcb882c"
                }
            };

        private static IEnumerable<Images> ExpectedImages =>
            new[]
            {
                new Images
                {
                    Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    Image1 = "Resources\\Images\\11-1.jpg",Image2 = "Resources\\Images\\11-2.jpg",
                    Image3 = "Resources\\Images\\11-3.jpg",Image4 = "Resources\\Images\\11-4.jpg"
                },
                new Images
                {
                    Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                    Image1 = "Resources\\Images\\22-1.jpg",Image2 = "Resources\\Images\\22-2.jpg",
                    Image3 = "Resources\\Images\\22-3.jpg",Image4 = "Resources\\Images\\22-4.jpg",
                },
                new Images
                {
                    Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                    Image1 = "Resources\\Images\\33-1.jpg",Image2 = "Resources\\Images\\33-2.jpg",
                    Image3 = "Resources\\Images\\33-3.jpg",Image4 = "Resources\\Images\\33-4.jpg"
                },
                new Images
                {
                    Id = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                    Image1 = "Resources\\Images\\44-1.jpg",Image2 = "Resources\\Images\\44-2.jpg",
                    Image3 = "Resources\\Images\\44-3.jpg",Image4 = "Resources\\Images\\44-4.jpg",
                }
            };

        private static IEnumerable<User> ExpectedUsers =>
            new[]
            {
                new User
                {
                    Id = "925695ec-0e70-4e43-8514-8a0710e11d53",
                    Name = "Oleksandr",UserName = "admin@gmail.com",
                    Surname = "Kardynal",Role = "admin",Email = "admin@gmail.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123")
                },
                new User
                {
                    Id = "5ae019a1-c312-4589-ab62-8b8a1fcb882c",
                    Name = "Ira",UserName = "irakardinal@gmail.com",
                    Surname = "Kardynal",Role = "user",Email = "irakardinal@gmail.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("ira123")
                }
            };
        #endregion
    }
}
