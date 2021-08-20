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
    public class FavoriteIntegrationTest
    {
        private CustomWebApplicationFactory _factory;
        private readonly string requestUri = "api/favorite/";
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
        public async Task FavoriteIntegration_AddFavorite()
        {
            var favoriteDTO = new FavoriteDTO
            {
                Id = new Guid("94d1b908-ff65-4c74-b836-44a4ca840ce8"),
                TwitterPostId = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                UserId = "925695ec-0e70-4e43-8514-8a0710e11d53"
            };
            var content = new StringContent(JsonConvert.SerializeObject(favoriteDTO), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync(requestUri + "addFavorite", content);

            httpResponse.EnsureSuccessStatusCode();
            using (var test = _factory.Services.CreateScope())
            {
                var context = test.ServiceProvider.GetService<ApplicationContext>();
                var favorite = await context.Favorites.FirstOrDefaultAsync(x => x.Id == favoriteDTO.Id);
                var expected = new AutoMapperHelper<Favorite, FavoriteDTO>().MapToType(favorite);

                Assert.That(favoriteDTO, Is.EqualTo(expected)
                    .Using(new FavoriteDTOEqualityComparer()));
            }
        }

        [Test]
        public async Task FavoriteIntegration_AddFavorite_ThrowExceptionIfModelIsIncorrect()
        {
            //UserId is empty
            var favoriteDTO = new FavoriteDTO
            {
                Id = new Guid("94d1b908-ff65-4c74-b836-44a4ca840ce8"),
                TwitterPostId = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                UserId = ""
            };
            await CheckExceptionWhileAddNewFavorite(favoriteDTO);
        }

        private async Task CheckExceptionWhileAddNewFavorite(FavoriteDTO model)
        {
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync(requestUri + "addFavorite", content);

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [TestCase("74d1b908-ff65-4c74-b836-44a4ca840ce8")]
        public async Task FavoriteIntegration_DeleteFavoriteById(Guid id)
        {
            var httpResponse = await _client.DeleteAsync(requestUri + "deleteFavoriteById/" + id);

            httpResponse.EnsureSuccessStatusCode();
            using (var test = _factory.Services.CreateScope())
            {
                var context = test.ServiceProvider.GetService<ApplicationContext>();
                Assert.AreEqual(2, context.Favorites.Count());
            }
        }

        [TestCase("925695ec-0e70-4e43-8514-8a0710e11d53")]
        public async Task FavoriteIntegration_GetFavoritesByUserId(string id)
        {
            var httpResponse = await _client.GetAsync(requestUri + "getFavoritesByUserId/" + id);

            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<IEnumerable<FavoriteDTO>>(stringResponse).ToList();
            var expected = InitialData.ExpectedFavoriteDTOs.Where(x => x.UserId == id);

            Assert.That(actual, Is.EqualTo(expected)
                .Using(new FavoriteDTOEqualityComparer()));
        }

        [Test]
        public async Task FavoriteIntegration_DeleteFavoriteByTwitterPostAndUserId()
        {
            var favoriteDTO = new FavoriteDTO
            {
                TwitterPostId = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                UserId = "925695ec-0e70-4e43-8514-8a0710e11d53"
            };

            var content = new StringContent(JsonConvert.SerializeObject(favoriteDTO), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync(requestUri + "deleteFavoriteByTwitterPostAndUserId", content);

            httpResponse.EnsureSuccessStatusCode();
            using (var test = _factory.Services.CreateScope())
            {
                var context = test.ServiceProvider.GetService<ApplicationContext>();
                Assert.AreEqual(2, context.Favorites.Count());
            }
        }
    }
}
