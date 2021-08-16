using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Twitter.Domain.Entities;

namespace Twitter.Tests
{
    public class TwitterPostEqualityComparer : IEqualityComparer<TwitterPost>
    {
        public bool Equals([AllowNull] TwitterPost x, [AllowNull] TwitterPost y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id && x.PostText == y.PostText && x.DateCreation == y.DateCreation
                && x.Like == y.Like && x.UserId == y.UserId;
        }

        public int GetHashCode([DisallowNull] TwitterPost obj)
        {
            return obj.GetHashCode();
        }
    }

    public class ImagesEqualityComparer : IEqualityComparer<Images>
    {
        public bool Equals([AllowNull] Images x, [AllowNull] Images y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id && x.Image1 == y.Image1 && x.Image2 == y.Image2
                && x.Image3 == y.Image3 && x.Image4 == y.Image4;
        }

        public int GetHashCode([DisallowNull] Images obj)
        {
            return obj.GetHashCode();
        }
    }

    public class CommentEqualityComparer : IEqualityComparer<Comment>
    {
        public bool Equals([AllowNull] Comment x, [AllowNull] Comment y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id && x.Author == y.Author && x.Text == y.Text
                && x.DateCreation == y.DateCreation && x.TwitterPostId == y.TwitterPostId
                && x.UserId == y.UserId;
        }

        public int GetHashCode([DisallowNull] Comment obj)
        {
            return obj.GetHashCode();
        }
    }

    public class UserEqualityComparer : IEqualityComparer<User>
    {
        public bool Equals([AllowNull] User x, [AllowNull] User y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id && x.Name == y.Name && x.Surname == y.Surname
                && x.Email == y.Email && x.Role == y.Role && x.ProfileImagePath == y.ProfileImagePath;
        }

        public int GetHashCode([DisallowNull] User obj)
        {
            return obj.GetHashCode();
        }
    }

}
