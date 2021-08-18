using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Twitter.Contracts;
using Twitter.Domain.Entities;
using Twitter.Domain.Exceptions;
using Twitter.Domain.Repositories;
using Twitter.Persistence;
using Twitter.Services;
using Twitter.Services.Abstractions;
using Twitter.Services.Configurations;

namespace Twitter.Tests.ServiceTests
{
    [TestFixture]
    public class UserServiceTest
    {
        [TestCase("925695ec-0e70-4e43-8514-8a0710e11d53")]
        public async Task UserService_GetUserByUserId(string userId)
        {
            var mockUserManager = GetUserManagerMock<User>();
            mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(InitialData.ExpectedUsers.ElementAt(0)));

            var mockTokenService = new Mock<ITokenService>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.UserManager).Returns(mockUserManager.Object);

            var userService = new UserService(mockUnitOfWork.Object,
                                            mockTokenService.Object,
                                            UnitTestHelper.CreateMapperProfile(),
                                            new Mock<IOptions<FacebookAuthSettings>>().Object,
                                            new Mock<IOptions<GoogleAuthSettings>>().Object);

            var userDTO = await userService.GetUserByUserIdAsync(userId);

            Assert.That(userDTO, Is.EqualTo(InitialData.ExpectedUserDTOs.ElementAt(0))
                .Using(new UserDTOEqualityComparer()));
        }

        private IOptions<FacebookAuthSettings> facebookAuthSettings;
        private IOptions<GoogleAuthSettings> googleAuthSetting;
        [OneTimeSetUp]
        public void GlobalPrepare()
        {
            var configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", false)
               .Build();

            facebookAuthSettings = Options.Create(configuration.GetSection(nameof(FacebookAuthSettings))
                .Get<FacebookAuthSettings>());
            googleAuthSetting = Options.Create(configuration.GetSection(nameof(GoogleAuthSettings))
                .Get<GoogleAuthSettings>());
        }


        [Test]
        public async Task UserService_GoogleLogin()
        {
            var user = new User
            {
                Id = "5ae019a1-c312-4589-ab62-8b8a1fcb882c",
                Email = "alexandrkardynal@gmail.com",
                Name = "Oleksandr",
                Surname = "Kardynal",
                Role = "user",
                ProfileImagePath = "Path"
            };
            var mockTokenService = new Mock<ITokenService>();
            mockTokenService.Setup(x => x.GetClaims(It.IsAny<string>())).Returns(Task.FromResult(new List<Claim>
            {
                new Claim("id", user.Id),
                new Claim("name", user.Name),
                new Claim("surname", user.Surname),
                new Claim("email", user.Email),
                new Claim("role", user.Role),
                new Claim("profileimagepath", user.ProfileImagePath)
            }));
            mockTokenService.Setup(x => x.GenerateToken(It.IsAny<List<Claim>>())).Returns(InitialData.FBTokenAfterAuth);

            var mockUserManager = GetUserManagerMock<User>();
            mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(InitialData.ExpectedUsers.ElementAt(1)));
            mockUserManager.Setup(x => x.CreateAsync(It.IsAny<User>()));
            mockUserManager.Setup(x => x.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()));
            mockUserManager.Setup(x => x.AddLoginAsync(It.IsAny<User>(), It.IsAny<UserLoginInfo>()));

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.UserManager).Returns(mockUserManager.Object);
            var userService = new UserService(mockUnitOfWork.Object,
                                            mockTokenService.Object,
                                            UnitTestHelper.CreateMapperProfile(),
                                            facebookAuthSettings,
                                            googleAuthSetting);

            var token = await userService.FacebookLoginAsync(InitialData.FBAccessToken);
            Assert.IsTrue(token.Length > 50);
        }

        [Test]
        public async Task UserService_SearchUsersByNameAndSurname()
        {
            string search = "kardynal";
            var mockUserManager = GetUserManagerMock<User>();
            mockUserManager.Setup(x => x.Users)
                .Returns(InitialData.ExpectedUsers.AsQueryable());

            var mockTokenService = new Mock<ITokenService>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.UserRepository.SearchUserByNameAndSurname(It.IsAny<string>()))
                .Returns(Task.FromResult(InitialData.ExpectedUsers.ToList()));

            var userService = new UserService(mockUnitOfWork.Object,
                                            mockTokenService.Object,
                                            UnitTestHelper.CreateMapperProfile(),
                                            new Mock<IOptions<FacebookAuthSettings>>().Object,
                                            new Mock<IOptions<GoogleAuthSettings>>().Object);
            var users = await userService.SearchUsersByNameAndSurnameAsync(search);

            Assert.AreEqual(users.Count, InitialData.ExpectedUserDTOs.Count());
            Assert.That(users, Is.EqualTo(InitialData.ExpectedUserDTOs)
                .Using(new UserDTOEqualityComparer()));
        }

        Mock<UserManager<TIDentityUser>> GetUserManagerMock<TIDentityUser>() where TIDentityUser : IdentityUser
        {
            return new Mock<UserManager<TIDentityUser>>(
                    new Mock<IUserStore<TIDentityUser>>().Object,
                    new Mock<IOptions<IdentityOptions>>().Object,
                    new Mock<IPasswordHasher<TIDentityUser>>().Object,
                    new IUserValidator<TIDentityUser>[0],
                    new IPasswordValidator<TIDentityUser>[0],
                    new Mock<ILookupNormalizer>().Object,
                    new Mock<IdentityErrorDescriber>().Object,
                    new Mock<IServiceProvider>().Object,
                    null);
        }

        Mock<RoleManager<TIdentityRole>> GetRoleManagerMock<TIdentityRole>() where TIdentityRole : IdentityRole
        {
            return new Mock<RoleManager<TIdentityRole>>(
                    new Mock<IRoleStore<TIdentityRole>>().Object,
                    new IRoleValidator<TIdentityRole>[0],
                    new Mock<ILookupNormalizer>().Object,
                    new Mock<IdentityErrorDescriber>().Object,
                    null);
        }

    }
}
