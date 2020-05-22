using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using CRM.BLAZOR.Services;
using CRM.BLL.Interfaces;
using CRM.BLL.Services;
using CRM.DAL.EF;
using CRM.DAL.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace CRM.BLAZOR
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
            services.AddBlazoredLocalStorage();
            services.AddAuthorizationCore();
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddOptions();
            //services.AddHttpClient();
            services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped(s =>
            {
                // Creating the URI helper needs to wait until the JS Runtime is initialized, so defer it.
                //var uriHelper = s.GetRequiredService<NavigationManager>();
                return new HttpClient
                {
                    //BaseAddress = new Uri(uriHelper.BaseUri)
                    BaseAddress = new Uri("https://localhost:3333")
                };
            });
            services.AddControllersWithViews();
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddIdentity<User, IdentityRole>()
                 .AddEntityFrameworkStores<ApiContext>();
            services.AddIdentityCore<User>()
                .AddEntityFrameworkStores<ApiContext>();
            services.AddControllersWithViews();
            services.AddDbContext<ApiContext>(options =>
                options.UseSqlServer(connection));
            services.AddControllersWithViews().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddTransient(typeof(IUserRegistrationService), typeof(UserRegistrationService));
            services.AddTransient(typeof(ICountryService), typeof(CountryService));
            services.AddTransient(typeof(ICompanyService), typeof(CompanyService));
            services.AddTransient(typeof(IMailFindService), typeof(MailFindService));
            services.AddTransient(typeof(ILogService), typeof(LogService));
            services.AddTransient(typeof(ICsvService), typeof(CsvService));
            services.AddTransient(typeof(ILemlistIntegrationService), typeof(LemlistIntegrationService));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = Configuration["ISSUER"],
                ValidAudience = Configuration["AUDIENCE"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Key"]))
            };
        });

            services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
                options.HttpsPort = 3333;
            });
            services.AddScoped(typeof(ITempService), typeof(TempService));

            //services.AddHttpClient<IAuthService, AuthService>();
            services.AddSignalR();
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
