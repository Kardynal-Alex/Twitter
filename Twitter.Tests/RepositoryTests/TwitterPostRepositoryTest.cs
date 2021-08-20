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

        [TestCase("925695ec-0e70-4e43-8514-8a0710e11d53")]
        public async Task TwitterPostRepository_GetTwitterPostsByUserIdWithImagesAndUsers(string userId)
        {
            await using var context = new ApplicationContext(_context);

            var twitterRepository = new TwitterPostRepository(context);
            var twitterPosts = await twitterRepository.GetTwitterPostsByUserIdWithImagesAndUsers(userId);

            var expectedUser = InitialData.ExpectedUsers.ElementAt(0);
            var expectedTwitterPosts = InitialData.ExpectedTwitterPosts.Where(x => x.UserId == userId).ToList();
            var expectedImages = InitialData.ExpectedImages.Join(expectedTwitterPosts, images => images.Id, twitterPost => twitterPost.Id,
                (images, twitterPost) => images).Distinct().ToList();

            Assert.That(twitterPosts, Is.EqualTo(expectedTwitterPosts)
                .Using(new TwitterPostEqualityComparer()));
            Assert.That(twitterPosts.Select(x => x.Images), Is.EqualTo(expectedImages)
                .Using(new ImagesEqualityComparer()));
            Assert.That(twitterPosts.ElementAt(0).User, Is.EqualTo(expectedUser)
                .Using(new UserEqualityComparer()));
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
        public async Task TwitterPostRepository_GetFriendsTweetsByUserId(string userId)
        {
            await using var context = new ApplicationContext(_context);

            var twitterRepository = new TwitterPostRepository(context);
            var actual = await twitterRepository.GetFriendsTweetsByUserIdAsync(userId);
            var expectedTwitterPosts = InitialData.ExpectedTwitterPosts.OrderBy(x => x.Id);
            var expectedImages = InitialData.ExpectedImages.OrderBy(x => x.Id);
            var expectedUsers = new List<User>();
            expectedUsers.AddRange(InitialData.ExpectedUsers);
            expectedUsers.AddRange(InitialData.ExpectedUsers);
            var actualUsers = actual.Select(x => x.User).OrderBy(x => x.Id);

            Assert.That(actual.OrderBy(x => x.Id), Is.EqualTo(expectedTwitterPosts)
                .Using(new TwitterPostEqualityComparer()));
            Assert.That(actual.Select(x => x.Images).OrderBy(x => x.Id), Is.EqualTo(expectedImages)
                .Using(new ImagesEqualityComparer()));
            Assert.That(actualUsers, Is.EqualTo(expectedUsers.OrderBy(x => x.Id))
                .Using(new UserEqualityComparer()));
        }

        [TestCase("925695ec-0e70-4e43-8514-8a0710e11d53")]
        public async Task TwitterPostRepository_GetFavoriteUserTwitterPostsByUserId(string userId)
        {
            await using var context = new ApplicationContext(_context);

            var twitterRepository = new TwitterPostRepository(context);
            var favoriteTweets = await twitterRepository.GetFavoriteUserTwitterPostsByUserIdAsync(userId);

            var expectedTweets = new List<TwitterPost>(new[]
            {
                InitialData.ExpectedTwitterPosts.ElementAt(0),
                InitialData.ExpectedTwitterPosts.ElementAt(1),
                InitialData.ExpectedTwitterPosts.ElementAt(3)
            });
            var expectedImages = new List<Images>(new[]
            {
                InitialData.ExpectedImages.ElementAt(0),
                InitialData.ExpectedImages.ElementAt(1),
                InitialData.ExpectedImages.ElementAt(3)
            });
            var expectedUsers = new List<User>(new[]
            {
                InitialData.ExpectedUsers.ElementAt(0),
                InitialData.ExpectedUsers.ElementAt(0),
                InitialData.ExpectedUsers.ElementAt(1)
            });

            Assert.That(favoriteTweets, Is.EqualTo(expectedTweets)
                .Using(new TwitterPostEqualityComparer()));
            Assert.That(favoriteTweets.Select(x => x.Images), Is.EqualTo(expectedImages)
                .Using(new ImagesEqualityComparer()));
            Assert.That(favoriteTweets.Select(x => x.User), Is.EqualTo(expectedUsers)
                .Using(new UserEqualityComparer()));
        }
    }
}
