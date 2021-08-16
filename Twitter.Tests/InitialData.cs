﻿using System;
using System.Collections.Generic;
using Twitter.Domain.Entities;

namespace Twitter.Tests
{
    public static class InitialData
    {
        #region data
        public static IEnumerable<TwitterPost> ExpectedTwitterPosts =>
        new[]
        {
                new TwitterPost
                {
                    Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    PostText = "TwitterPost text1",
                    DateCreation = DateTime.Now.Date,
                    Like = 0,
                    UserId = "925695ec-0e70-4e43-8514-8a0710e11d53"
                },
                new TwitterPost
                {
                    Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                    PostText = "TwitterPost text2",
                    DateCreation = DateTime.Now.Date,
                    Like = 3,
                    UserId = "925695ec-0e70-4e43-8514-8a0710e11d53"
                },
                new TwitterPost
                {
                    Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                    PostText = "TwitterPost text3",
                    DateCreation = DateTime.Now.Date,
                    Like = 0,
                    UserId = "5ae019a1-c312-4589-ab62-8b8a1fcb882c"
                },
                new TwitterPost
                {
                    Id = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                    PostText = "TwitterPost text4",
                    DateCreation = DateTime.Now.Date,
                    Like = 5,
                    UserId = "5ae019a1-c312-4589-ab62-8b8a1fcb882c"
                }
        };

        public static IEnumerable<Images> ExpectedImages =>
            new[]
            {
                new Images
                {
                    Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    Image1 = "Resources\\Images\\11-1.jpg",
                    Image2 = "Resources\\Images\\11-2.jpg",
                    Image3 = "Resources\\Images\\11-3.jpg",
                    Image4 = "Resources\\Images\\11-4.jpg"
                },
                new Images
                {
                    Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                    Image1 = "Resources\\Images\\22-1.jpg",
                    Image2 = "Resources\\Images\\22-2.jpg",
                    Image3 = "Resources\\Images\\22-3.jpg",
                    Image4 = "Resources\\Images\\22-4.jpg"
                },
                new Images
                {
                    Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                    Image1 = "Resources\\Images\\33-1.jpg",
                    Image2 = "Resources\\Images\\33-2.jpg",
                    Image3 = "Resources\\Images\\33-3.jpg",
                    Image4 = "Resources\\Images\\33-4.jpg"
                },
                new Images
                {
                    Id = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                    Image1 = "Resources\\Images\\44-1.jpg",
                    Image2 = "Resources\\Images\\44-2.jpg",
                    Image3 = "Resources\\Images\\44-3.jpg",
                    Image4 = "Resources\\Images\\44-4.jpg"
                }
            };

        public static IEnumerable<User> ExpectedUsers =>
            new[]
            {
                new User
                {
                    Id = "925695ec-0e70-4e43-8514-8a0710e11d53",
                    Name = "Oleksandr",
                    UserName = "admin@gmail.com",
                    Surname = "Kardynal",
                    Role = "admin",
                    Email = "admin@gmail.com",
                    ProfileImagePath = "Image path1",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123")
                },
                new User
                {
                    Id = "5ae019a1-c312-4589-ab62-8b8a1fcb882c",
                    Name = "Ira",
                    UserName = "irakardinal@gmail.com",
                    Surname = "Kardynal",
                    Role = "user",
                    Email = "irakardinal@gmail.com",
                    ProfileImagePath = "Image path2",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("ira123")
                }
            };

        public static IEnumerable<Comment> ExpectedComments =>
            new[]
            {
                new Comment
                {
                    Id = new Guid("1f8d4896-a7cd-1b5d-3527-0151a96d94de"),
                    Author = "Oleksandr Kardynal",
                    Text = "Comment1",
                    DateCreation = DateTime.Now.Date,
                    TwitterPostId = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    UserId = "925695ec-0e70-4e43-8514-8a0710e11d53",
                    ProfileImagePath = "Image path1"
                },
                new Comment
                {
                    Id = new Guid("8a25b419-782d-34b5-1478-43a0b2dc9736"),
                    Author = "Ira Kardynal",
                    Text = "Comment2",
                    DateCreation = DateTime.Now.Date,
                    TwitterPostId = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    UserId = "5ae019a1-c312-4589-ab62-8b8a1fcb882c",
                    ProfileImagePath = "Image path2"
                },
                new Comment
                {
                    Id = new Guid("c5a4692a-b390-114b-f6f9-3f6953e41fe5"),
                    Author = "Oleksandr Kardynal",
                    Text = "Comment3",
                    DateCreation = DateTime.Now.Date,
                    TwitterPostId = new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                    UserId = "925695ec-0e70-4e43-8514-8a0710e11d53",
                    ProfileImagePath = "Image path1"
                },
                new Comment
                {
                    Id = new Guid("c6ec7102-9af6-09ab-0eb9-5b74c8afd128"),
                    Author = "Ira Kardynal",
                    Text = "Comment4",
                    DateCreation = DateTime.Now.Date,
                    TwitterPostId = new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                    UserId = "5ae019a1-c312-4589-ab62-8b8a1fcb882c",
                    ProfileImagePath = "Image path2"
                }
            };
        #endregion
    }

}