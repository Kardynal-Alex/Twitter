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

            return x.Id == y.Id && x.PostText == y.PostText && x.DateCreation.Date == y.DateCreation.Date
                && x.Like == y.Like && x.UserId == y.UserId && x.NComments == y.NComments;
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
                && x.DateCreation.Date == y.DateCreation.Date && x.TwitterPostId == y.TwitterPostId
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

            return x.Id == y.Id && x.Name == y.Name && x.Surname == y.Surname
                && x.Email == y.Email && x.Role == y.Role && x.ProfileImagePath == y.ProfileImagePath;
        }

        public int GetHashCode([DisallowNull] UserDTO obj)
        {
            return obj.GetHashCode();
        }
    }

    public class FriendDTOEqualityComparer : IEqualityComparer<FriendDTO>
    {
        public bool Equals([AllowNull] FriendDTO x, [AllowNull] FriendDTO y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id && x.UserId == y.UserId && x.FriendId == y.FriendId;
        }

        public int GetHashCode([DisallowNull] FriendDTO obj)
        {
            return obj.GetHashCode();
        }
    }

    public class FavoriteDTOEqualityComparer : IEqualityComparer<FavoriteDTO>
    {
        public bool Equals([AllowNull] FavoriteDTO x, [AllowNull] FavoriteDTO y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id && x.UserId == y.UserId && x.TwitterPostId == y.TwitterPostId;
        }

        public int GetHashCode([DisallowNull] FavoriteDTO obj)
        {
            return obj.GetHashCode();
        }
    }

    public class LikeDTOEqualityComparer : IEqualityComparer<LikeDTO>
    {
        public bool Equals([AllowNull] LikeDTO x, [AllowNull] LikeDTO y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id && x.UserId == y.UserId && x.TwitterPostId == y.TwitterPostId;
        }

        public int GetHashCode([DisallowNull] LikeDTO obj)
        {
            return obj.GetHashCode();
        }
    }
}
