using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RealEstate.Data;
using RealEstate.Infrastructure.Repositories;
using RealEstate.Models;
using RealEstate.Repositories;

using RealEstate.Services;
using RealEStateProject.Repositories;
using RealEStateProject.Repositories.Implementation;
using RealEStateProject.Repositories.Interfaces;
using RealEStateProject.Services;
using RealEStateProject.Services.Implementation;
using RealEStateProject.Services.Interfaces;

using RealEStateProject.Repositories;
using RealEStateProject.Repositories.Implementation;
using RealEStateProject.Repositories.Interfaces;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Facebook;
using AspNet.Security.OAuth.Apple;


namespace RealEStateProject
{
    public class Program

    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Configure database context
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Register repositories
            builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            //builder.Services.AddScoped<IPropertyRepository, PropertyRepository>();

            builder.Services.AddScoped<IPropertyRepository, PropertyRepository>();
            //builder.Services.AddScoped<IAgentRepository, AgentRepository>()
            //builder.Services.AddScoped<IPropertyRepository, PropertyRepository>();
            builder.Services.AddScoped<IAgentRepository, AgentRepository>();

            //builder.Services.AddScoped<IPropertyImageRepository, PropertyImageRepository>();
            builder.Services.AddScoped<IFavoriteRepository, FavoriteRepository>();
            //builder.Services.AddScoped<IPropertyRequestRepository, PropertyRequestRepository>();

            

            // Register services
            builder.Services.AddScoped(typeof(IBaseService<,>), typeof(BaseService<,>));
            builder.Services.AddScoped<IPropertyService, PropertyService>();
            //builder.Services.AddScoped<IAgentService, AgentService>();
            //builder.Services.AddScoped<IPropertyImageService, PropertyImageService>();
            builder.Services.AddScoped<IFavoriteService, FavoriteService>();
            //builder.Services.AddScoped<IPropertyRequestService, PropertyRequestService>();

            // Add AutoMapper
            builder.Services.AddAutoMapper(typeof(Program));

            // Configure authentication with cookies
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.AccessDeniedPath = "/Account/AccessDenied";
                    options.LogoutPath = "/Account/Logout";
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                    options.SlidingExpiration = true;
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.Cookie.SameSite = SameSiteMode.Strict;
                });

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
                .AddGoogle(options =>
                {
                    options.ClientId = "YOUR_GOOGLE_CLIENT_ID";
                    options.ClientSecret = "YOUR_GOOGLE_CLIENT_SECRET";
                })
                .AddFacebook(options =>
                {
                    options.AppId = "YOUR_FACEBOOK_APP_ID";
                    options.AppSecret = "YOUR_FACEBOOK_APP_SECRET";
                })
                //.AddApple(options =>
                //{
                //    options.ClientId = "YOUR_APPLE_CLIENT_ID";
                //    options.KeyId = "YOUR_KEY_ID";
                //    options.TeamId = "YOUR_TEAM_ID";
                //    options.UsePrivateKey(keyId => builder.Configuration["ApplePrivateKey"]);
                //})
                ;

            // Add Identity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                options.Password.RequiredLength = 8)                
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/Home/Error/{0}");

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
