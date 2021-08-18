﻿using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using Twitter.Domain.Entities;
using Twitter.Persistence;
using Twitter.Persistence.Repositories;

namespace Twitter.Tests.RepositoryTests
{
    [TestFixture]
    public class UserRepositoryTest
    {
        private DbContextOptions<ApplicationContext> _context;
        [SetUp]
        public void Setup()
        {
            _context = UnitTestHelper.GetUnitDbOptions();
        }

        [Test]
        public async Task UserRepository_SearchUsersByNameAndSurname()
        {
            string search = "kardynal";

            await using var context = new ApplicationContext(_context);
            var userRepository = new UserRepository(context);

            var users = await userRepository.SearchUserByNameAndSurname(search);

            Assert.AreEqual(users.Count, InitialData.ExpectedUsers.Count());
            Assert.That(users, Is.EqualTo(InitialData.ExpectedUsers)
                .Using(new UserEqualityComparer()));
        }
    }
}
