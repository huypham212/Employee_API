using AppS3.Data;
using AppS3.Services.EmployeeService;
using AppS3.Services.PositionService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AppS3.Profiles;
using AppS3.Services.AuthenticationService;
using Microsoft.AspNetCore.Identity;
using AppS3.Models;
using AppS3.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AppS3.Services;
using Microsoft.AspNetCore.Http;
using AppS3.Controllers;
using AppS3.Middleware;

namespace AppS3
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
            services.AddDbContext<AppS3DbContext>(db => db.UseMySQL(Configuration.GetConnectionString("DatabaseConnection")));
            services.AddMvc();
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<AppS3DbContext>();

            services.AddControllers();

            //AutoMapper
            var mapperConfig = new MapperConfiguration(mp =>
            {
                mp.AddProfile(new AppS3Profile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            //DI
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IPositionService, PositionService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<GenerateTokenService>();
            services.AddDistributedMemoryCache();

            services.Configure<IdentityOptions>(opt =>
            {
                opt.Password.RequireDigit = true;
                //opt.Password.RequiredLength = 4;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireLowercase = true;
            });

            var jwtSection = Configuration.GetSection("JwtOptions");
            services.Configure<JwtOptions>(jwtSection);
            var jwtOptionsSetttings = jwtSection.Get<JwtOptions>();
            var key = Encoding.ASCII.GetBytes(jwtOptionsSetttings.SecretKey);

            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(bearer =>
            {
                bearer.SaveToken = true;
                bearer.RequireHttpsMetadata = false;
                bearer.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidAudience = jwtOptionsSetttings.Audience,
                    ValidIssuer = jwtOptionsSetttings.Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AppS3", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"Example Token: ",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },

                            Scheme = "oath2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },

                        new List<string>()
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.AddMiddewaresInDevelopment();
            }

            if (env.IsStaging())
            {
                app.AddMiddlewareInStaging();
            }
        }
    }
}
