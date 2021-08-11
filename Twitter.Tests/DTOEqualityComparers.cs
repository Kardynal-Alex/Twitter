using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Twitter.Contracts;

namespace Twitter.Tests
{
    public class TwitterPostDTOEqualityComparer : IEqualityComparer<TwitterPostDTO>
    {
        public bool Equals([AllowNull] TwitterPostDTO x, [AllowNull] TwitterPostDTO y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id && x.PostText == y.PostText && x.DateCreation == y.DateCreation
                && x.Like == y.Like && x.UserId == y.UserId;
        }

        public int GetHashCode([DisallowNull] TwitterPostDTO obj)
        {
            return obj.GetHashCode();
        }
    }

    public class ImagesDTOEqualityComparer : IEqualityComparer<ImagesDTO>
    {
        public bool Equals([AllowNull] ImagesDTO x, [AllowNull] ImagesDTO y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id && x.Image1 == y.Image1 && x.Image2 == y.Image2
                && x.Image3 == y.Image3 && x.Image4 == y.Image4;
        }

        public int GetHashCode([DisallowNull] ImagesDTO obj)
        {
            return obj.GetHashCode();
        }
    }

    public class CommentDTOEqualityComparer : IEqualityComparer<CommentDTO>
    {
        public bool Equals([AllowNull] CommentDTO x, [AllowNull] CommentDTO y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id && x.Author == y.Author && x.Text == y.Text
                && x.DateCreation == y.DateCreation && x.TwitterPostId == y.TwitterPostId
                && x.UserId == y.UserId;
        }

        public int GetHashCode([DisallowNull] CommentDTO obj)
        {
            return obj.GetHashCode();
        }
    }

    public class UserDTOEqualityComparer : IEqualityComparer<UserDTO>
    {
        public bool Equals([AllowNull] UserDTO x, [AllowNull] UserDTO y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id && x.Name == y.Surname && x.Surname == y.Surname
                && x.Email == y.Email && x.Role == y.Role;
        }

        public int GetHashCode([DisallowNull] UserDTO obj)
        {
            return obj.GetHashCode();
        }
    }
}
