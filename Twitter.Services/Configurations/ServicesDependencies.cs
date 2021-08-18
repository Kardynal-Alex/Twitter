using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Twitter.Services.Abstractions;

namespace Twitter.Services.Configurations
{
    public static class ServicesDependencies
    {
        public static IServiceCollection AddServicesDependencies(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = AuthOptions.ISSUER,
                    ValidAudience = AuthOptions.AUDIENCE,
                    IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey()
                };
            });
            services.Configure<FacebookAuthSettings>(Configuration.GetSection(nameof(FacebookAuthSettings)));
            services.Configure<GoogleAuthSettings>(Configuration.GetSection(nameof(GoogleAuthSettings)));

            services.AddTransient<ICommentService, CommentService>();
            services.AddTransient<IImagesService, ImagesService>();
            services.AddTransient<ITwitterPostService, TwitterPostService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IFriendService, FriendService>();

            return services;
        }
    }
}
