USE [Twitter]
GO

SELECT u.* FROM dbo.AspNetUsers as u
INNER JOIN dbo.Friends as f on u.Id=f.UserId 
WHERE f.FriendId='26243b74-a75f-4d7a-812c-ba407790593f' AND u.Id!='26243b74-a75f-4d7a-812c-ba407790593f'

/*
SELECT u.* FROM dbo.AspNetUsers as u,dbo.Friends as f
WHERE f.FriendId='26243b74-a75f-4d7a-812c-ba407790593f' AND u.Id=f.UserId AND u.Id!='26243b74-a75f-4d7a-812c-ba407790593f'
*/