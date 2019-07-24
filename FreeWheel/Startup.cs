using System.IdentityModel.Tokens.Jwt;
using FreeWheel.DataAccess;
using FreeWheel.Logic.DataAccess;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace FreeWheel
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
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt => {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = "local",
                        ValidAudience = "local",
                        RequireSignedTokens = false,
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateLifetime = false,
                        ValidateIssuerSigningKey = false,
                        RequireExpirationTime = false,
                        SignatureValidator = delegate (string token, TokenValidationParameters parameters)
                        {
                            var jwt = new JwtSecurityToken(token);
                            return jwt;
                        },

                    };
                });

            services.AddDbContext<MovieContext>(opt => opt.UseInMemoryDatabase());
            services.AddTransient<IMoviesRepository, EFMovieRepository>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseMvc();

            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedTestData(serviceScope.ServiceProvider.GetService<MovieContext>());
            }
        }

        private void SeedTestData(MovieContext db) {
            db.Movies.Add(new Logic.Models.Movie() {
                Id= 1,
                Title = "Test1",
                Genre = "Action",
                YearOfRelease = 2000
            });

            db.Movies.Add(new Logic.Models.Movie()
            {
                Id = 2,
                Title = "Test2",
                Genre = "Romance",
                YearOfRelease = 2000
            });

            db.Movies.Add(new Logic.Models.Movie()
            {
                Id = 3,
                Title = "Test3",
                Genre = "Drama",
                YearOfRelease = 2001
            });

            db.Movies.Add(new Logic.Models.Movie()
            {
                Id = 4,
                Title = "Test4",
                Genre = "Drama",
                YearOfRelease = 2001
            });

            db.Ratings.Add(new Logic.Models.Rating {
                MovieId = 2,
                UserId = 2,
                Rate = 5
            });

            db.Ratings.Add(new Logic.Models.Rating
            {
                MovieId = 1,
                UserId = 1,
                Rate = 5
            });

            db.Ratings.Add(new Logic.Models.Rating
            {
                MovieId = 2,
                UserId = 1,
                Rate = 5
            });

            db.Ratings.Add(new Logic.Models.Rating
            {
                MovieId = 3,
                UserId = 1,
                Rate = 3
            });

            db.Ratings.Add(new Logic.Models.Rating
            {
                MovieId = 3,
                UserId = 2,
                Rate = 4
            });

            db.SaveChanges();
        }
    }
}
