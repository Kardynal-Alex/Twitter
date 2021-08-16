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
            var twitterPost = new TwitterPost { Id = id, Images = new Images { Id = id } };

            await twitterRepository.AddTwitterPostAsync(twitterPost);
            await context.SaveChangesAsync();

            Assert.That(context.TwitterPosts.Count(), Is.EqualTo(5));
            Assert.That(context.Images.Count(), Is.EqualTo(5));
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

        [TestCase("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")]
        public async Task TwitterPostRepository_GetTwitterPostsByIdWithDetails(Guid twitterPostId)
        {
            await using var context = new ApplicationContext(_context);

            var twitterRepository = new TwitterPostRepository(context);
            var twitterPost = await twitterRepository.GetTwitterPostByIdWithDetails(twitterPostId);

            var expectedTwitterPost = InitialData.ExpectedTwitterPosts.FirstOrDefault(x => x.Id == twitterPostId);
            var expectedImages = InitialData.ExpectedImages.FirstOrDefault(x => x.Id == twitterPostId);
            var expectedUser = InitialData.ExpectedUsers.ElementAt(0);
            var expectedComments = InitialData.ExpectedComments.Where(x => x.TwitterPostId == twitterPostId);

            Assert.That(twitterPost, Is.EqualTo(expectedTwitterPost)
                .Using(new TwitterPostEqualityComparer()));
            Assert.That(twitterPost.Images, Is.EqualTo(twitterPost.Images)
                .Using(new ImagesEqualityComparer()));
            Assert.That(twitterPost.Comments, Is.EqualTo(expectedComments)
                .Using(new CommentEqualityComparer()));
            Assert.That(twitterPost.User, Is.EqualTo(expectedUser)
                .Using(new UserEqualityComparer()));
        }

        [TestCase("925695ec-0e70-4e43-8514-8a0710e11d53")]
        public async Task TwitterPostRepository_GetTwitterPostsByUserIdWithDetailsExceptUser(string userId)
        {
            await using var context = new ApplicationContext(_context);

            var twitterRepository = new TwitterPostRepository(context);
            var twitterPosts = await twitterRepository.GetTwitterPostByUserIdWithDetailsExceptUserAsync(userId);

            var expectedTwitterPosts = InitialData.ExpectedTwitterPosts.Where(x => x.UserId == userId).ToList();
            var expectedImages = InitialData.ExpectedImages.Join(expectedTwitterPosts, images => images.Id, twitterPost => twitterPost.Id,
                (images, twitterPost) => images).Distinct().ToList();
            
            Assert.That(twitterPosts, Is.EqualTo(expectedTwitterPosts)
                .Using(new TwitterPostEqualityComparer()));
            Assert.That(twitterPosts.Select(x => x.Images), Is.EqualTo(expectedImages)
                .Using(new ImagesEqualityComparer()));
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

    }
}
