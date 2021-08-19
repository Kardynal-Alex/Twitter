USE [Twitter]
GO

SELECT u.Id,u.Name,u.Surname,u.Role,u.ProfileImagePath,u.UserName,u.NormalizedUserName,u.Email,u.NormalizedEmail,
u.EmailConfirmed,u.PasswordHash,u.SecurityStamp,u.ConcurrencyStamp,u.PhoneNumber,u.PhoneNumberConfirmed,
u.TwoFactorEnabled,u.LockoutEnd,u.LockoutEnabled,u.AccessFailedCount
FROM dbo.AspNetUsers as u
INNER JOIN dbo.Friends as f
ON f.UserId='26243b74-a75f-4d7a-812c-ba407790593f' AND u.Id=f.FriendId