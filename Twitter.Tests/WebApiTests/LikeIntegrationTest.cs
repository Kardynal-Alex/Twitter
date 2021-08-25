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
    public class LikeIntegrationTest
    {
        private CustomWebApplicationFactory _factory;
        private readonly string requestUri = "api/like/";
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

        [Test]
        public async Task LikeController_AddLike()
        {
            var likeDTO = new LikeDTO
            {
                Id = new Guid("94d1b908-ff65-4c74-b836-44a4ca840ce8"),
                UserId = "5ae019a1-c312-4589-ab62-8b8a1fcb882c",
                TwitterPostId = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")
            };
            var content = new StringContent(JsonConvert.SerializeObject(likeDTO), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync(requestUri + "addLike", content);

            httpResponse.EnsureSuccessStatusCode();
            using (var test = _factory.Services.CreateScope()) 
            {
                var context = test.ServiceProvider.GetService<ApplicationContext>();
                var like = await context.Likes.FirstOrDefaultAsync(x => x.Id == likeDTO.Id);
                var expected = new AutoMapperHelper<Like, LikeDTO>().MapToType(like);

                Assert.That(likeDTO, Is.EqualTo(expected)
                    .Using(new LikeDTOEqualityComparer()));
            }
        }

        [Test]
        public async Task LikeController_AddLike_ThrowExceptionIfModelIsIncorrect()
        {
            //UserId is empty
            var likeDTO = new LikeDTO
            {
                Id = new Guid("94d1b908-ff65-4c74-b836-44a4ca840ce8"),
                UserId = "",
                TwitterPostId = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")
            };
            await CheckExceptionWhileAddNewLike(likeDTO);
        }

        private async Task CheckExceptionWhileAddNewLike(LikeDTO model)
        {
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync(requestUri + "addLike", content);

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [TestCase("74d1b908-ff65-4c74-b836-44a4ca840ce8")]
        public async Task LikeController_DeleteLikeById(Guid id)
        {
            var httpResponse = await _client.DeleteAsync(requestUri + "deleteLikeById/" + id);

            httpResponse.EnsureSuccessStatusCode();
            using (var test = _factory.Services.CreateScope())
            {
                var context = test.ServiceProvider.GetService<ApplicationContext>();
                Assert.AreEqual(context.Likes.Count(), 2);
            }
        }

        [TestCase("925695ec-0e70-4e43-8514-8a0710e11d53")]
        public async Task LikeController_GetLikesByUserId(string id)
        {
            var httpResponse = await _client.GetAsync(requestUri + "getLikesByUserId/" + id);

            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<IEnumerable<LikeDTO>>(stringResponse).ToList();
            var expected = InitialData.ExpectedLikeDTOs;

            Assert.That(actual, Is.EqualTo(expected)
                .Using(new LikeDTOEqualityComparer()));
        }

    }
}
