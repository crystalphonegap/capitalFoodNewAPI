using Microsoft.AspNetCore.Builder;
using ICICI_BANK_INTERFACE.Context;
using Microsoft.AspNetCore.Hosting;
using ICICI_BANK_INTERFACE.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICICI_BANK_INTERFACE.Interface;
using ICICI_BANK_INTERFACE.Services;
using Microsoft.AspNetCore.Authentication;

namespace ICICI_BANK_INTERFACE
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
            services.AddRazorPages();
            services.AddMvc();
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            //services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));

            services.Configure<FormOptions>(o =>
            {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });
            //JwtSettings settings = GetJwtSettings();
            //services.AddSingleton<JwtSettings>(settings);
            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = "JwtBearer";
            //    options.DefaultChallengeScheme = "JwtBearer";
            //}).AddJwtBearer("JwtBearer", jwtOptions =>
            //{
            //    jwtOptions.TokenValidationParameters = new TokenValidationParameters()
            //    {
            //        ValidateIssuerSigningKey = true,
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Key)),
            //        ValidateIssuer = true,
            //        ValidIssuer = settings.Issuer,
            //        ValidateAudience = true,
            //        ValidAudience = settings.Audience,
            //        ValidateLifetime = true,
            //        ClockSkew = TimeSpan.FromMinutes(settings.MinToExpiration)
            //    };
            //});

            services.AddControllers();
    //        services.AddAuthentication("BasicAuthentication")
    //.AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
            // services.AddDbContext<DataContext>(options =>
            //options.UseSqlServer(Configuration.GetConnectionString("DatabaseContext")));

            //For all to Access WebApi

            //Commented By SUman on 20-11-2020
            services.AddCors(options =>
            {
                options.AddPolicy("EnableCORS", builder =>
                    builder.SetIsOriginAllowed(_ => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });
           

            services.AddControllers()
         .AddNewtonsoftJson(options =>
         {
             options.UseMemberCasing();
         });
            services.AddScoped<IHomeService, HomeService>();
            services.AddScoped<IHelperClass, HelperClass>();
            services.AddScoped<IIQCService, IQCService>();
            //services.AddScoped<IUserMasterService, UserMasterService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            //app.UseStaticFiles();

            app.UseRouting();

            app.UseCors("EnableCORS");
            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        //public JwtSettings GetJwtSettings()
        //{
        //    JwtSettings settings = new JwtSettings();
        //    settings.Key = Configuration["JwtSettings:key"];
        //    settings.Issuer = Configuration["JwtSettings:issuer"];
        //    settings.Audience = Configuration["JwtSettings:audience"];
        //    settings.MinToExpiration = Convert.ToInt32(Configuration["JwtSettings:minToExpiration"]);
        //    return settings;
        //}
    }
}
