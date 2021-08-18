using System;
using System.Collections.Generic;
using Twitter.Contracts;
using Twitter.Domain.Entities;

namespace Twitter.Tests
{
    public static class InitialData
    {
        #region model data
        public static IEnumerable<TwitterPost> ExpectedTwitterPosts =>
        new[]
        {
                new TwitterPost
                {
                    Id = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    PostText = "TwitterPost text1",
                    DateCreation = DateTime.Now.Date,
                    Like = 0,
                    UserId = "925695ec-0e70-4e43-8514-8a0710e11d53"
                },
                new TwitterPost
                {
                    Id = new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                    PostText = "TwitterPost text2",
                    DateCreation = DateTime.Now.Date,
                    Like = 3,
                    UserId = "925695ec-0e70-4e43-8514-8a0710e11d53"
                },
                new TwitterPost
                {
                    Id = new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                    PostText = "TwitterPost text3",
                    DateCreation = DateTime.Now.Date,
                    Like = 0,
                    UserId = "5ae019a1-c312-4589-ab62-8b8a1fcb882c"
                },
                new TwitterPost
                {
                    Id = new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"),
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
                    Id = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    Image1 = "Resources\\Images\\11-1.jpg",
                    Image2 = "Resources\\Images\\11-2.jpg",
                    Image3 = "Resources\\Images\\11-3.jpg",
                    Image4 = "Resources\\Images\\11-4.jpg"
                },
                new Images
                {
                    Id = new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                    Image1 = "Resources\\Images\\22-1.jpg",
                    Image2 = "Resources\\Images\\22-2.jpg",
                    Image3 = "Resources\\Images\\22-3.jpg",
                    Image4 = "Resources\\Images\\22-4.jpg"
                },
                new Images
                {
                    Id = new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                    Image1 = "Resources\\Images\\33-1.jpg",
                    Image2 = "Resources\\Images\\33-2.jpg",
                    Image3 = "Resources\\Images\\33-3.jpg",
                    Image4 = "Resources\\Images\\33-4.jpg"
                },
                new Images
                {
                    Id = new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"),
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
        #region DTO data
        public static IEnumerable<CommentDTO> ExpectedCommentDTOs =>
            new[]
            {
                 new CommentDTO
                 {
                     Id = new Guid("1f8d4896-a7cd-1b5d-3527-0151a96d94de"),
                     Author = "Oleksandr Kardynal", Text = "Comment1",
                     DateCreation = DateTime.Now.Date,
                     TwitterPostId = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                     UserId = "925695ec-0e70-4e43-8514-8a0710e11d53",
                     ProfileImagePath = "Image path1"
                 },
                new CommentDTO
                {
                    Id = new Guid("8a25b419-782d-34b5-1478-43a0b2dc9736"),
                    Author = "Ira Kardynal", Text = "Comment2",
                    DateCreation = DateTime.Now.Date,
                    TwitterPostId = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    UserId = "5ae019a1-c312-4589-ab62-8b8a1fcb882c",
                    ProfileImagePath = "Image path2"
                },
                new CommentDTO
                {
                    Id = new Guid("c5a4692a-b390-114b-f6f9-3f6953e41fe5"),
                    Author = "Oleksandr Kardynal", Text = "Comment3",
                    DateCreation = DateTime.Now.Date,
                    TwitterPostId = new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                    UserId = "925695ec-0e70-4e43-8514-8a0710e11d53",
                    ProfileImagePath = "Image path1"
                },
                new CommentDTO
                {
                    Id = new Guid("c6ec7102-9af6-09ab-0eb9-5b74c8afd128"),
                    Author = "Ira Kardynal", Text = "Comment4",
                    DateCreation = DateTime.Now.Date,
                    TwitterPostId = new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                    UserId = "5ae019a1-c312-4589-ab62-8b8a1fcb882c",
                    ProfileImagePath = "Image path2"
                }
            };

        public static IEnumerable<UserDTO> ExpectedUserDTOs =>
            new[]
            {
                new UserDTO
                {
                    Id = "925695ec-0e70-4e43-8514-8a0710e11d53",
                    Name = "Oleksandr",
                    Surname = "Kardynal",
                    Role = "admin",
                    Email = "admin@gmail.com",
                    ProfileImagePath = "Image path1"
                },
                new UserDTO
                {
                    Id = "5ae019a1-c312-4589-ab62-8b8a1fcb882c",
                    Name = "Ira",
                    Surname = "Kardynal",
                    Role = "user",
                    Email = "irakardinal@gmail.com",
                    ProfileImagePath = "Image path2",
                }
            };
        #endregion
        #region oAuth
        public static string FBAccessToken = "EAALLGbB4PQUBACTTDQxmtDkedVDpcOsuDpNO7n6ioZBX1phNY8lHOGhykwsC6ndlug6HLvoyHWKaiXZBFvzD5IiHIoftM3d3MOSbMZCX4RroJUd267fHdQqKWqMvvPJTYI6YXUVmZAyxL6SbHPHgyyWQZC2itOtSGmgCgmVInO8tZCSG3ZBxyLHZBDLVHnKAMZCIZD";

        public static string GoogleToken = "eyJhbGciOiJSUzI1NiIsImtpZCI6IjZlZjRiZDkwODU5MWY2OTdhOGE5Yjg5M2IwM2U2YTc3ZWIwNGU1MWYiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJhY2NvdW50cy5nb29nbGUuY29tIiwiYXpwIjoiNTE0OTIwMTc2NjMwLTgyMTZsdnUzZGo5MXJzaWUxNnMzOWsxYmJoc2oydGczLmFwcHMuZ29vZ2xldXNlcmNvbnRlbnQuY29tIiwiYXVkIjoiNTE0OTIwMTc2NjMwLTgyMTZsdnUzZGo5MXJzaWUxNnMzOWsxYmJoc2oydGczLmFwcHMuZ29vZ2xldXNlcmNvbnRlbnQuY29tIiwic3ViIjoiMTA5MTUwMzA2OTc0NjcwODc2NDE0IiwiZW1haWwiOiJpcmFrYXJkaW5hbEBnbWFpbC5jb20iLCJlbWFpbF92ZXJpZmllZCI6dHJ1ZSwiYXRfaGFzaCI6InVfUFFYWHR1YnVyUThUR1VLYU1XZGciLCJuYW1lIjoiSXJhIEthcmRpbmFsIiwicGljdHVyZSI6Imh0dHBzOi8vbGgzLmdvb2dsZXVzZXJjb250ZW50LmNvbS9hLS9BT2gxNEdoYzlkcWdudVpPM0FwV3dtUmc1OENxNkR3aHRWd1ozVzhZTmR2Rj1zOTYtYyIsImdpdmVuX25hbWUiOiJJcmEiLCJmYW1pbHlfbmFtZSI6IkthcmRpbmFsIiwibG9jYWxlIjoicnUiLCJpYXQiOjE2MjkyMTA3NzQsImV4cCI6MTYyOTIxNDM3NCwianRpIjoiZTI4ODYwZWM2M2QzZWI3NGY3N2EwYjMzZjllYWJhZDQ5MWZjOTU4YiJ9.OohEIi_9_ASFOaetUbyPXx0OxOMzasxDPalCGgma0a5X2wKj7TTAhVrjp-8859hww1YyM-PycF_QdWL6SEjB_HMEkMT-e6JHODwgiDXmdP41FGCqOZWZUhmNcv7ygbhodK8mowU1ZYRXYHhfcimNh-mouxQl3qeqYmYvKJeMVmdtBMMr2za1v_EIHHr6TACjTjKpMCFEtTmiI4tQuUPzPvdTuIl7fEtFzTu0MsNYR0cu0x2WaNZvgcdGGqoe4M-yoLiHd_kP31nEn_rG7R3Pu0-95yKNWvp62e1oEHA1IluuooQh8fu9gPrA0iI8zDWwABcoLlgKZFEOz32jVWAQkQ";

        public static string FBTokenAfterAuth = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6ImE1OWRmZjc2LWYxMTItNDk4OC1iNzJjLTQ0MjE4ZTRjOWEzZiIsIm5hbWUiOiJPbGVrc2FuZHIiLCJzdXJuYW1lIjoiS2FyZHluYWwiLCJlbWFpbCI6ImFsZXhhbmRya2FyZHluYWxAZ21haWwuY29tIiwicm9sZSI6InVzZXIiLCJwcm9maWxlaW1hZ2VwYXRoIjoiaHR0cHM6Ly9wbGF0Zm9ybS1sb29rYXNpZGUuZmJzYnguY29tL3BsYXRmb3JtL3Byb2ZpbGVwaWMvP2FzaWQ9MjY1MDU4ODY2ODU4MDMxMyZoZWlnaHQ9NTAmd2lkdGg9NTAmZXh0PTE2MzEzNjg5MDcmaGFzaD1BZVNCWHQtc0YxbmxFMUpfMFVBIiwibmJmIjoxNjI5MjcwNjg4LCJleHAiOjE2MjkyNzQyODgsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjQ0MzE4IiwiYXVkIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzMTgifQ.ehb0-rS6vEZwWSEcl-m7u907i6uKBKzT7WG3EmlStZ8";

        #endregion
    }
}
