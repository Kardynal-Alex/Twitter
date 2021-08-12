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
    public class CommentRepositoryTest
    {
        private DbContextOptions<ApplicationContext> _context;
        [SetUp]
        public void Setup()
        {
            _context = UnitTestHelper.GetUnitDbOptions();
        }

        [Test]
        public async Task CommentRepository_AddComment()
        {
            await using var context = new ApplicationContext(_context);

            var commentRepository = new CommentRepository(context);
            var comment = new Comment { Id = new Guid() };

            await commentRepository.AddCommentAsync(comment);
            await context.SaveChangesAsync();

            Assert.That(context.Comments.Count(), Is.EqualTo(5));
        }

        [TestCase("c6ec7102-9af6-09ab-0eb9-5b74c8afd128")]
        public async Task CommentRepository_DeleteCommentById(Guid id)
        {
            await using var context = new ApplicationContext(_context);

            var commentRepository = new CommentRepository(context);

            commentRepository.DeleteCommentById(id);
            await context.SaveChangesAsync();

            Assert.That(context.Comments.Count(), Is.EqualTo(3));
        }

        [Test]
        public async Task CommentRepository_UpdateComment()
        {
            await using var context = new ApplicationContext(_context);

            var commentRepository = new CommentRepository(context);
            var comment = new Comment
            {
                Id = new Guid("c6ec7102-9af6-09ab-0eb9-5b74c8afd128"),
                Author = "Ira Kardynal",
                Text = "Update Comment5",
                DateCreation = DateTime.Now.Date,
                TwitterPostId = new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                UserId = "5ae019a1-c312-4589-ab62-8b8a1fcb882c"
            };

            commentRepository.UpdateComment(comment);
            await context.SaveChangesAsync();

            Assert.That(comment, Is.EqualTo(new Comment
            {
                Id = new Guid("c6ec7102-9af6-09ab-0eb9-5b74c8afd128"),
                Author = "Ira Kardynal",
                Text = "Update Comment5",
                DateCreation = DateTime.Now.Date,
                TwitterPostId = new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                UserId = "5ae019a1-c312-4589-ab62-8b8a1fcb882c"
            }).Using(new CommentEqualityComparer()));
        }
    }
}
