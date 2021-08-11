using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace Twitter.Services.Configurations
{
    public class AuthOptions
    {
        public const string ISSUER = "https://localhost:44318";
        public const string AUDIENCE = "https://localhost:44318";
        private const string KEY = "superSecretKey@345";
        public const int LIFETIME = 60;
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
