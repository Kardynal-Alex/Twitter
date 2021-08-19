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
    public class FriendIntegrationTest
    {
        private CustomWebApplicationFactory _factory;
        private readonly string requestUri = "api/friend/";
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
        public async Task FriendController_AddFriend()
        {
            var friendDTO = new FriendDTO
            {
                Id = new Guid("9b4eca73-b8e9-7717-06aa-35e69d52c43f"),
                UserId = "925695ec-0e70-4e43-8514-8a0710e11d53",
                FriendId = "5ae019a1-c312-4589-ab62-8b8a1fcb882c"
            };
            var content = new StringContent(JsonConvert.SerializeObject(friendDTO), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync(requestUri + "addFriend", content);

            httpResponse.EnsureSuccessStatusCode();
            using (var test = _factory.Services.CreateScope())
            {
                var context = test.ServiceProvider.GetService<ApplicationContext>();
                var friend = await context.Friends.FirstOrDefaultAsync(x => x.Id == friendDTO.Id);
                var expected = new AutoMapperHelper<Friend, FriendDTO>().MapToType(friend);

                Assert.That(friendDTO, Is.EqualTo(expected)
                    .Using(new FriendDTOEqualityComparer()));
            }
        }

        [Test]
        public async Task FriendController_AddFriend_ThrowExceptionIfModelIsIncorrect()
        {
            //UserId is empty
            var friendDTO = new FriendDTO
            {
                Id = new Guid("9b4eca73-b8e9-7717-06aa-35e69d52c43f"),
                UserId = "",
                FriendId = "5ae019a1-c312-4589-ab62-8b8a1fcb882c"
            };
            await CheckExceptionWhileAddNewFriend(friendDTO);

            //FriendId is empty
            friendDTO.UserId = "925695ec-0e70-4e43-8514-8a0710e11d53";
            friendDTO.FriendId = "";
            await CheckExceptionWhileAddNewFriend(friendDTO);
        }

        private async Task CheckExceptionWhileAddNewFriend(FriendDTO model)
        {
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync(requestUri + "addFriend", content);

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [TestCase("b4edd1e5-c05d-ee7d-ed93-4603de11d462")]
        public async Task FriendController_DeleteFriendById(Guid id)
        {
            var httpResponse = await _client.DeleteAsync(requestUri + "deleteFriendById/" + id);

            httpResponse.EnsureSuccessStatusCode();
            using (var test = _factory.Services.CreateScope())
            {
                var context = test.ServiceProvider.GetService<ApplicationContext>();
                Assert.AreEqual(1, context.Friends.Count());
            }
        }

        [TestCase("b4edd1e5-c05d-ee7d-ed93-4603de11d462")]
        public async Task FriendController_GetFriendById(Guid id)
        {
            var httpResponse = await _client.GetAsync(requestUri + "getFriendById/" + id);

            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<FriendDTO>(stringResponse);

            Assert.That(actual, Is.EqualTo(InitialData.ExpectedFriendDTOs.ElementAt(0))
                .Using(new FriendDTOEqualityComparer()));
        }

        [Test]
        public async Task FriendController_GetFriendByUserAndFriendId()
        {
            var friendDTO = new FriendDTO
            {
                UserId = "925695ec-0e70-4e43-8514-8a0710e11d53",
                FriendId = "5ae019a1-c312-4589-ab62-8b8a1fcb882c"
            };
            var content = new StringContent(JsonConvert.SerializeObject(friendDTO), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync(requestUri + "getFriendByUserAndFriendId", content);

            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<FriendDTO>(stringResponse);

            Assert.That(actual, Is.EqualTo(InitialData.ExpectedFriendDTOs.ElementAt(0))
                .Using(new FriendDTOEqualityComparer()));
        }

        [TestCase("925695ec-0e70-4e43-8514-8a0710e11d53")]
        public async Task FriendController_GetFriendsByUserId(string id)
        {
            var httpResponse = await _client.GetAsync(requestUri + "getFriendsByUserId/" + id);

            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<List<FriendDTO>>(stringResponse);
            var expected = InitialData.ExpectedFriendDTOs.Where(x => x.UserId == id);

            Assert.That(actual, Is.EqualTo(expected)
                .Using(new FriendDTOEqualityComparer()));
        }
    }
}
