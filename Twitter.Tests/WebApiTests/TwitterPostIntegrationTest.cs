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
    public class TwitterPostIntegrationTest
    {
        private CustomWebApplicationFactory _factory;
        private readonly string requestUri = "api/twitterpost/";
        private HttpClient _client;
        private static List<TwitterPostDTO> TwitterPostDTOs;
        [SetUp]
        public void Init()
        {
            _factory = new CustomWebApplicationFactory();
            _client = _factory.CreateClient();
            using (var test = _factory.Services.CreateScope())
            {
                var context = test.ServiceProvider.GetService<ApplicationContext>();
                var config = new MapperConfiguration(cfg => { 
                    cfg.CreateMap<TwitterPost, TwitterPostDTO>();
                    cfg.CreateMap<Images, ImagesDTO>();
                    cfg.CreateMap<Comment, CommentDTO>();
                    cfg.CreateMap<User, UserDTO>();
                });
                var mapper = new Mapper(config);
                TwitterPostDTOs = mapper.Map<List<TwitterPostDTO>>(context.TwitterPosts
                    .Include(x => x.Images)
                    .Include(x => x.User)
                    .Include(x => x.Comments)
                    .ToList());
            }
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _factory.Dispose();
            _client.Dispose();
        }

        [Test]
        public async Task TwitterPostController_AddTwitterPost()
        {
            var twitterPostDTO = new TwitterPostDTO
            {
                Id = new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                PostText = "new TwitterPost",
                DateCreation = DateTime.Now.Date,
                Like = 0,
                UserId = "925695ec-0e70-4e43-8514-8a0710e11d53",
                Images = new ImagesDTO
                {
                    Id = new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                    Image1 = "Resources\\Images\\11-11.jpg",
                    Image2 = "Resources\\Images\\11-22.jpg",
                    Image3 = "Resources\\Images\\11-33.jpg",
                    Image4 = "Resources\\Images\\11-44.jpg"
                }
            };

            var content = new StringContent(JsonConvert.SerializeObject(twitterPostDTO), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync(requestUri + "addTwitterPost", content);

            httpResponse.EnsureSuccessStatusCode();
            using (var test = _factory.Services.CreateScope())
            {
                var context = test.ServiceProvider.GetService<ApplicationContext>();
                var twitterPosts = await context.TwitterPosts.Include(x => x.Images)
                    .FirstOrDefaultAsync(x => x.Id == twitterPostDTO.Id);

                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<TwitterPost, TwitterPostDTO>();
                    cfg.CreateMap<Images, ImagesDTO>();
                });
                var mapper = new Mapper(config);
                var actualTwitterPostDTO = mapper.Map<TwitterPost, TwitterPostDTO>(twitterPosts);

                Assert.That(actualTwitterPostDTO, Is.EqualTo(twitterPostDTO)
                    .Using(new TwitterPostDTOEqualityComparer()));
                Assert.That(actualTwitterPostDTO.Images, Is.EqualTo(twitterPostDTO.Images)
                    .Using(new ImagesDTOEqualityComparer()));
            }
        }

        [Test]
        public async Task TwitterPostController_AddTwitterPost_ThrowExceptionIfModelIsIncorrect()
        {
            //PostText is empty
            var twitterPostDTO = new TwitterPostDTO
            {
                Id = new Guid("babaaaab-baaa-baaa-aaaa-aaaaaaaaaaaa"),
                PostText = "",
                DateCreation = DateTime.Now.Date,
                Like = 0,
                UserId = "925695ec-0e70-4e43-8514-8a0710e11d53"
            };
            await CheckExceptionWhileAddNewTwitterPost(twitterPostDTO);

            //Like is negative number
            twitterPostDTO.PostText = "TwitterPost text new";
            twitterPostDTO.Like = -1;
            await CheckExceptionWhileAddNewTwitterPost(twitterPostDTO);

            //UserId is empty
            twitterPostDTO.Like = 0;
            twitterPostDTO.UserId = "";
            await CheckExceptionWhileAddNewTwitterPost (twitterPostDTO);
        }

        private async Task CheckExceptionWhileAddNewTwitterPost(TwitterPostDTO model)
        {
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync(requestUri + "addTwitterPost", content);

            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [TestCase("925695ec-0e70-4e43-8514-8a0710e11d53")]
        public async Task TwitterPostController_GetTwitterPostsByUserId(string id)
        {
            var expectedTwitterPostDTOs = TwitterPostDTOs.Where(x => x.UserId == id);
            var httpResponse = await _client.GetAsync(requestUri + "getUserTwitterPosts/" + id);

            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<IEnumerable<TwitterPostDTO>>(stringResponse).ToList();

            Assert.AreEqual(actual.Count, expectedTwitterPostDTOs.Count());
            Assert.That(actual, Is.EqualTo(expectedTwitterPostDTOs)
                .Using(new TwitterPostDTOEqualityComparer()));
            Assert.That(actual.Select(x=>x.Images), Is.EqualTo(expectedTwitterPostDTOs.Select(x=>x.Images))
                .Using(new ImagesDTOEqualityComparer()));
        }

        [Test]
        public async Task TwitterPostController_DeleteTwitterPost()
        {
            var twitterPostDTO = new TwitterPostDTO
            {
                Id = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                PostText = "TwitterPost text1",
                DateCreation = DateTime.Now.Date,
                Like = 0,
                UserId = "925695ec-0e70-4e43-8514-8a0710e11d53",
                Images = new ImagesDTO
                {
                    Id = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    Image1 = "Resources\\Images\\11-1.jpg",
                    Image2 = "Resources\\Images\\11-2.jpg",
                    Image3 = "Resources\\Images\\11-3.jpg",
                    Image4 = "Resources\\Images\\11-4.jpg"
                }
            };

            var content = new StringContent(JsonConvert.SerializeObject(twitterPostDTO), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync(requestUri + "deleteTwitterPost", content);

            httpResponse.EnsureSuccessStatusCode();
            using (var test = _factory.Services.CreateScope())
            {
                var context = test.ServiceProvider.GetService<ApplicationContext>();
                Assert.AreEqual(3, context.TwitterPosts.Count());
            }
        }

        [TestCase("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")]
        public async Task TwitterPostController_GetTwitterPostByIdWithDetails(Guid id)
        {
            var expectedTwitterPostDTOs = TwitterPostDTOs.FirstOrDefault(x => x.Id == id);
            var httpResponse = await _client.GetAsync(requestUri + "getTweetByIdWithDetails/" + id);

            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<TwitterPostDTO>(stringResponse);

            Assert.That(actual, Is.EqualTo(expectedTwitterPostDTOs)
                .Using(new TwitterPostDTOEqualityComparer()));
            Assert.That(actual.Images, Is.EqualTo(expectedTwitterPostDTOs.Images)
                .Using(new ImagesDTOEqualityComparer()));
            Assert.That(actual.User, Is.EqualTo(expectedTwitterPostDTOs.User)
                .Using(new UserDTOEqualityComparer()));
            Assert.That(actual.Comments, Is.EqualTo(expectedTwitterPostDTOs.Comments)
                .Using(new CommentDTOEqualityComparer()));
        }

        [TestCase("925695ec-0e70-4e43-8514-8a0710e11d53")]
        public async Task TwitterPostController_GetTwitterPostsByUserIdWithImagesAndUsers(string id)
        {
            var expectedTwitterPostDTOs = TwitterPostDTOs.Where(x => x.UserId == id);
            var httpResponse = await _client.GetAsync(requestUri + "getTweetByUserIdWithImagesAndUsers/" + id);

            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<IEnumerable<TwitterPostDTO>>(stringResponse).ToList();

            Assert.AreEqual(actual.Count, expectedTwitterPostDTOs.Count());
            Assert.That(actual, Is.EqualTo(expectedTwitterPostDTOs)
                .Using(new TwitterPostDTOEqualityComparer()));
            Assert.That(actual.Select(x => x.Images), Is.EqualTo(expectedTwitterPostDTOs.Select(x => x.Images))
                .Using(new ImagesDTOEqualityComparer()));
            Assert.That(actual[0].User, Is.EqualTo(expectedTwitterPostDTOs.ElementAt(0).User)
               .Using(new UserDTOEqualityComparer()));
        }

        public async Task TwitterPostController_UpdateTwitterPostWithImages()
        {
            var twitterPostDTO = new TwitterPostDTO
            {
                Id = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                PostText = "update TwitterPost text1",
                DateCreation = DateTime.Now.Date,
                Like = 0,
                UserId = "925695ec-0e70-4e43-8514-8a0710e11d53",
                Images = new ImagesDTO
                {
                    Id = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    Image1 = "Resources\\Images\\111-1.jpg",
                    Image2 = "Resources\\Images\\111-2.jpg",
                    Image3 = "Resources\\Images\\111-3.jpg",
                    Image4 = "Resources\\Images\\111-4.jpg"
                }
            };
            var content = new StringContent(JsonConvert.SerializeObject(twitterPostDTO), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PutAsync(requestUri + "updateTwitterPost", content);

            httpResponse.EnsureSuccessStatusCode();
            using (var test = _factory.Services.CreateScope())
            {
                var context = test.ServiceProvider.GetService<ApplicationContext>();
                var updatedTwitterPost = await context.TwitterPosts.Include(x => x.Images)
                    .FirstOrDefaultAsync(x => x.Id == twitterPostDTO.Id);
                var expected = new AutoMapperHelper<TwitterPost, TwitterPostDTO>().MapToType(updatedTwitterPost);

                Assert.That(twitterPostDTO, Is.EqualTo(expected)
                    .Using(new TwitterPostDTOEqualityComparer()));
                Assert.That(twitterPostDTO.Images, Is.EqualTo(expected.Images)
                    .Using(new ImagesDTOEqualityComparer()));
            }
        }

        [TestCase("925695ec-0e70-4e43-8514-8a0710e11d53")]
        public async Task TwitterPostController_GetFriendsTweetsByUserId(string id)
        {
            var httpResponse = await _client.GetAsync(requestUri + "getFriendsTweetsByUserId/" + id);

            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<IEnumerable<TwitterPostDTO>>(stringResponse).ToList();

            var expectedTwitterPostDTOs = TwitterPostDTOs.OrderBy(x => x.Id);
            var expectedImagesDTOs = expectedTwitterPostDTOs.Select(x => x.Images).OrderBy(x => x.Id);
            var expectedUserDTOs = expectedTwitterPostDTOs.Select(x => x.User).OrderBy(x => x.Id);
            var actualUserDTOs = actual.Select(x => x.User).OrderBy(x => x.Id);

            Assert.That(actual.OrderBy(x => x.Id), Is.EqualTo(expectedTwitterPostDTOs)
                .Using(new TwitterPostDTOEqualityComparer()));
            Assert.That(actual.Select(x => x.Images).OrderBy(x => x.Id), Is.EqualTo(expectedImagesDTOs)
                .Using(new ImagesDTOEqualityComparer()));
            Assert.That(actualUserDTOs, Is.EqualTo(expectedUserDTOs)
                .Using(new UserDTOEqualityComparer()));
        }

        [TestCase("925695ec-0e70-4e43-8514-8a0710e11d53")]
        public async Task TwitterPostController_GetFavoriteUserTwitterPostsByUserId(string id)
        {
            var httpResponse = await _client.GetAsync(requestUri + "getFavoriteTwitterPostsByUserId/" + id);

            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<IEnumerable<TwitterPostDTO>>(stringResponse).ToList();
            var expected = new List<TwitterPostDTO>(new[] { TwitterPostDTOs[0], TwitterPostDTOs[1], TwitterPostDTOs[3] });

            Assert.That(actual, Is.EqualTo(expected)
              .Using(new TwitterPostDTOEqualityComparer()));
            Assert.That(actual.Select(x => x.Images), Is.EqualTo(expected.Select(x => x.Images))
                .Using(new ImagesDTOEqualityComparer()));
            Assert.That(actual.Select(x => x.User), Is.EqualTo(expected.Select(x => x.User))
                .Using(new UserDTOEqualityComparer()));
        }

        [Test]
        public async Task TwitterPostController_SearchTwitterPostsByHeshTag()
        {
            string search = "test";
            var httpResponse = await _client.GetAsync(requestUri + "searchTwitterPostsByHeshTag/?search=" + search);

            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<IEnumerable<TwitterPostDTO>>(stringResponse).ToList();
            var expectedTweets = new List<TwitterPostDTO>(new[]
            {
                TwitterPostDTOs[1],
                TwitterPostDTOs[2]
            });
            var expectedImages = new List<ImagesDTO>(new[]
            {
                TwitterPostDTOs[1].Images,
                TwitterPostDTOs[2].Images
            });
            var expectedUsers = new List<UserDTO>(new[]
            {
                InitialData.ExpectedUserDTOs.ElementAt(0),
                InitialData.ExpectedUserDTOs.ElementAt(1)
            });

            Assert.That(actual, Is.EqualTo(expectedTweets)
               .Using(new TwitterPostDTOEqualityComparer()));
            Assert.That(actual.Select(x => x.Images), Is.EqualTo(expectedImages)
                .Using(new ImagesDTOEqualityComparer()));
            Assert.That(actual.Select(x => x.User), Is.EqualTo(expectedUsers)
                .Using(new UserDTOEqualityComparer()));
        }

    }
}
