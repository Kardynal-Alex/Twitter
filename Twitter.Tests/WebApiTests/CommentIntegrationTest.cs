using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Twitter.Contracts;
using Twitter.Domain.Entities;
using Twitter.Persistence;

namespace Twitter.Tests.WebApiTests
{
    [TestFixture]
    public class CommentIntegrationTest
    {
        private CustomWebApplicationFactory _factory;
        private readonly string requestUri = "api/comment/";
        private HttpClient _client;
        [SetUp]
        public void Init()
        {
            _factory = new CustomWebApplicationFactory();
            _client = _factory.CreateClient();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _factory.Dispose();
            _client.Dispose();
        }

        [TestCase("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")]
        public async Task CommentController_GetCommentsByTwitterId(Guid id)
        {
            var expectedCommentDTOs = InitialData.ExpectedCommentDTOs.Where(x => x.TwitterPostId == id);
            var httpResponse = await _client.GetAsync(requestUri + "getCommentsByTweetId/" + id);

            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<IEnumerable<CommentDTO>>(stringResponse).ToList();

            Assert.AreEqual(actual.Count, expectedCommentDTOs.Count());
            Assert.That(actual, Is.InstanceOf<List<CommentDTO>>());
            Assert.That(actual, Is.EqualTo(expectedCommentDTOs)
                .Using(new CommentDTOEqualityComparer()));
        }

        [Test]
        public async Task CommentController_AddComment()
        {
            var commentDTO = new CommentDTO
            {
                Id = new Guid("baaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                Author = "Oleksandr Kardynal",
                Text = "new Comment",
                DateCreation = DateTime.Now.Date,
                TwitterPostId = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                UserId = "925695ec-0e70-4e43-8514-8a0710e11d53",
                ProfileImagePath = "new Image path"
            };

            var content = new StringContent(JsonConvert.SerializeObject(commentDTO), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync(requestUri + "addComment", content);

            httpResponse.EnsureSuccessStatusCode();
            using (var test = _factory.Services.CreateScope())
            {
                var context = test.ServiceProvider.GetService<ApplicationContext>();
                var comment = await context.Comments.FirstOrDefaultAsync(x => x.Id == commentDTO.Id);

                var expectedCommentDTO = new AutoMapperHelper<Comment, CommentDTO>().MapToType(comment);

                Assert.That(commentDTO, Is.EqualTo(expectedCommentDTO)
                    .Using(new CommentDTOEqualityComparer()));
            }
        }

        [Test]
        public async Task CommentController_AddComment_ThrowExceptionIfModelIsIncorrect()
        {
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
            await CheckExceptionWhileAddNewComment(commentDTO);

            //Text is empty
            commentDTO.Author = "Oleksandr Kardynal";
            commentDTO.Text = "";
            await CheckExceptionWhileAddNewComment(commentDTO);

            //UserId is empty
            commentDTO.Text = "new Comment";
            commentDTO.UserId = "";
            await CheckExceptionWhileAddNewComment(commentDTO);
        }

        private async Task CheckExceptionWhileAddNewComment(CommentDTO model)
        {
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync(requestUri + "addComment", content);

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [TestCase("1f8d4896-a7cd-1b5d-3527-0151a96d94de")]
        public async Task CommentController_DeleteCommentById(Guid id)
        {
            var httpResponse = await _client.DeleteAsync(requestUri + "deleteCommentById/" + id);

            httpResponse.EnsureSuccessStatusCode();
            using (var test = _factory.Services.CreateScope())
            {
                var context = test.ServiceProvider.GetService<ApplicationContext>();
                Assert.AreEqual(3, context.Comments.Count());
            }
        }
    }
}
