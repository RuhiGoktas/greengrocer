using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using greengrocer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using greengrocer.Repositories;
using greengrocer.Services;
using Microsoft.Extensions.Configuration;
using greengrocer.Models;

namespace greengrocer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
      .AddCookie(options =>
      {
          options.LoginPath = "/Login";
          options.LogoutPath = "/Logout";
      })
      .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
      {
          var key = Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]);

          options.TokenValidationParameters = new TokenValidationParameters
          {
              ValidateIssuer = true,
              ValidateAudience = true,
              ValidateLifetime = true,
              ValidateIssuerSigningKey = true,
              ValidIssuer = Configuration["Jwt:Issuer"],
              ValidAudience = Configuration["Jwt:Audience"],
              IssuerSigningKey = new SymmetricSecurityKey(key)
          };
      });


            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;

                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });
            services.Configure<LoginOptions>(Configuration.GetSection("LoginOptions"));
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IUserService, UserService>();

        }


        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // app.UseHsts();
            }


            using (var scope = app.ApplicationServices.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();


                db.Database.EnsureCreated();


                db.Database.ExecuteSqlRaw(@"
            CREATE TABLE IF NOT EXISTS Users (
                Id        INTEGER PRIMARY KEY AUTOINCREMENT,
                UserName  TEXT NOT NULL,
                Password  TEXT NOT NULL,
                FullName  TEXT NULL,
                IsActive  INTEGER NOT NULL DEFAULT 1,
                CreatedAt TEXT NULL
            );

            CREATE UNIQUE INDEX IF NOT EXISTS IX_Users_UserName
                ON Users(UserName);
        ");


                if (!db.Users.Any())
                {
                    db.Users.Add(new User
                    {
                        UserName = "admin",
                        Password = "123456",
                        FullName = "System Admin",
                        IsActive = true
                    });

                    db.SaveChanges();
                }


                app.UseStaticFiles();

                app.UseAuthentication();

                app.UseMvc();
            }
        }



    }
}