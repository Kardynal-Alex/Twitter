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
using Twitter.Services;

namespace Twitter.Tests.ServiceTests
{
    [TestFixture]
    public class CommentServiceTest
    {
        [Test]
        public async Task CommentService_AddComment()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.CommentRepository.AddCommentAsync(It.IsAny<Comment>()));
            var commentSevice = new CommentService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            var commentDTO = new CommentDTO
            {
                Id = new Guid("328d4896-a7cd-1b5d-3527-0151a96d94de"),
                Author = "Oleksandr Kardynal",
                Text = "new Comment",
                DateCreation = DateTime.Now.Date,
                TwitterPostId = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                UserId = "925695ec-0e70-4e43-8514-8a0710e11d53",
                ProfileImagePath = "Image path1"
            };

            await commentSevice.AddCommentAsync(commentDTO);

            mockUnitOfWork.Verify(x => x.CommentRepository.AddCommentAsync(It.Is<Comment>(x =>
                  x.Id == commentDTO.Id && x.Author == commentDTO.Author && x.Text == commentDTO.Text &&
                  x.DateCreation == commentDTO.DateCreation && x.TwitterPostId == commentDTO.TwitterPostId &&
                  x.UserId == commentDTO.UserId && x.ProfileImagePath == commentDTO.ProfileImagePath)), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Test]
        public void CommentService_AddComment_ThrowsTwitterExceptionIfModelIsIncorrect()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.CommentRepository.AddCommentAsync(It.IsAny<Comment>()));
            var commentSevice = new CommentService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            //Author is empty
            var commentDTO = new CommentDTO
            {
                Id = new Guid("328d4896-a7cd-1b5d-3527-0151a96d94de"),
                Author = "",
                Text = "new Comment",
                DateCreation = DateTime.Now.Date,
                TwitterPostId = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                UserId = "925695ec-0e70-4e43-8514-8a0710e11d53",
                ProfileImagePath = "Image path1"
            };
            Assert.ThrowsAsync<TwitterException>(async () => await commentSevice.AddCommentAsync(commentDTO));

            //Text is empty
            commentDTO.Author = "Oleksandr Kardynal";
            commentDTO.Text = "";
            Assert.ThrowsAsync<TwitterException>(async () => await commentSevice.AddCommentAsync(commentDTO));

            //UserId is empty
            commentDTO.Text = "new Comment";
            commentDTO.UserId = "";
            Assert.ThrowsAsync<TwitterException>(async () => await commentSevice.AddCommentAsync(commentDTO));
        }

        [TestCase("1f8d4896-a7cd-1b5d-3527-0151a96d94de")]
        public async Task CommentService_DeleteCommentById(Guid id)
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.CommentRepository.DeleteCommentById(It.IsAny<Guid>()));
            var commentSevice = new CommentService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            await commentSevice.DeleteCommentByIdAsync(id);

            mockUnitOfWork.Verify(x => x.CommentRepository.DeleteCommentById(id), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        }

        [TestCase("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")]
        public async Task CommentService_GetCommentsByTwitterPostId(Guid twitterPostId)
        {
            var comments = InitialData.ExpectedComments.Where(x => x.TwitterPostId == twitterPostId).ToList();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.CommentRepository.GetCommentsByTwitterPostIdAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(comments));
            var commentSevice = new CommentService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            var actualCommentDTOs = await commentSevice.GetCommentsByTwitterPostIdAsync(twitterPostId);
            var expectedCommentDTOs = InitialData.ExpectedCommentDTOs.Where(x => x.TwitterPostId == twitterPostId);

            Assert.That(actualCommentDTOs, Is.InstanceOf<List<CommentDTO>>());
            Assert.AreEqual(actualCommentDTOs.Count, expectedCommentDTOs.Count());
            Assert.That(actualCommentDTOs, Is.EqualTo(expectedCommentDTOs)
                .Using(new CommentDTOEqualityComparer()));
        }

        [Test]
        public async Task CommentService_UpdateComment()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.CommentRepository.UpdateComment(It.IsAny<Comment>()));
            var commentSevice = new CommentService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            var commentDTO = new CommentDTO
            {
                Id = new Guid("1f8d4896-a7cd-1b5d-3527-0151a96d94de"),
                Author = "Oleksandr Kardynal",
                Text = "new text Comment1",
                DateCreation = DateTime.Now.Date,
                TwitterPostId = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                UserId = "925695ec-0e70-4e43-8514-8a0710e11d53",
                ProfileImagePath = "Image path1"
            };
            await commentSevice.UpdateCommentAsync(commentDTO);

            mockUnitOfWork.Verify(x => x.CommentRepository.UpdateComment(It.Is<Comment>(x =>
                  x.Id == commentDTO.Id && x.Author == commentDTO.Author && x.Text == commentDTO.Text &&
                  x.DateCreation.Date == commentDTO.DateCreation.Date && x.TwitterPostId == commentDTO.TwitterPostId &&
                  x.UserId == commentDTO.UserId && x.ProfileImagePath == commentDTO.ProfileImagePath)), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        }
    }
}
