using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Twitter.Contracts;
using Twitter.Domain.Entities;
using Twitter.Persistence;

namespace Twitter.Tests.WebApiTests
{
    [TestFixture]
    public class AccountIntegrationTest
    {
        private CustomWebApplicationFactory _factory;
        private readonly string requestUri = "api/account/";
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
        public async Task AccountController_LoginWithFacebook()
        {
            var facebookToken = new FacebookAuthDTO
            {
                AccessToken = InitialData.FBAccessToken
            };
            var content = new StringContent(JsonConvert.SerializeObject(facebookToken), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync(requestUri + "facebook", content);

            httpResponse.EnsureSuccessStatusCode();
        }

        [Test]
        public async Task AccountController_LoginWithGoogle()
        {
            var googleAuthDTO = new GoogleAuthDTO
            {
                IdToken = InitialData.GoogleToken,
                Provider = "GOOGLE"
            };
            var content = new StringContent(JsonConvert.SerializeObject(googleAuthDTO), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync(requestUri + "google", content);

            httpResponse.EnsureSuccessStatusCode();
        }

        [TestCase("925695ec-0e70-4e43-8514-8a0710e11d53")]
        public async Task AccountController_GetUserByUserId(string id)
        {
            var httpResponse = await _client.GetAsync(requestUri + "getUserById/" + id);

            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<UserDTO>(stringResponse);

            Assert.That(actual, Is.EqualTo(InitialData.ExpectedUserDTOs.ElementAt(0))
                .Using(new UserDTOEqualityComparer()));
        }

        [Test]
        public async Task AccountController_SearchUsersByNameAndSurname()
        {
            string search = "kardynal";
            var httpResponse = await _client.GetAsync(requestUri + "SearchUsers/?search=" + search);

            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<List<UserDTO>>(stringResponse);

            Assert.That(actual, Is.EqualTo(InitialData.ExpectedUserDTOs)
                .Using(new UserDTOEqualityComparer()));
        }

        [TestCase("925695ec-0e70-4e43-8514-8a0710e11d53")]
        public async Task AccountController_GetUserFriendsByUserIdAsync(string id)
        {
            var httpResponse = await _client.GetAsync(requestUri + "getUserFriendsByUserId/" + id);

            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<List<UserDTO>>(stringResponse);

            Assert.That(actual, Is.EqualTo(InitialData.ExpectedUserDTOs)
               .Using(new UserDTOEqualityComparer()));
        }

        [Test]
        public async Task AccountController_UpdateUserProfile()
        {
            var userDTO = new UserDTO
            {
                Id = "925695ec-0e70-4e43-8514-8a0710e11d53",
                Name = "Oleksandr",
                Surname = "Kardynal",
                Role = "admin",
                Email = "admin@gmail.com",
                ProfileImagePath = "new Image path1"
            };
            var content = new StringContent(JsonConvert.SerializeObject(userDTO), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PutAsync(requestUri + "updateUser", content);

            httpResponse.EnsureSuccessStatusCode();
            using (var test = _factory.Services.CreateScope())
            {
                var context = test.ServiceProvider.GetService<ApplicationContext>();
                var user = await context.Users.FirstOrDefaultAsync(x => x.Id == userDTO.Id);
                var expected = new AutoMapperHelper<User, UserDTO>().MapToType(user);

                Assert.That(userDTO, Is.EqualTo(expected)
                    .Using(new UserDTOEqualityComparer()));
            }
        }
    }
}
