using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Course_Project.Data;
using Course_Project.Models;
using Course_Project.Services;
using Microsoft.Extensions.Logging;
using Course_Project.Loggs;
using System.IO;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;


namespace Course_Project
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
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(opts => {
                opts.Password.RequiredLength = 6;   
                opts.Password.RequireNonAlphanumeric = false;   
                opts.Password.RequireLowercase = true; 
                opts.Password.RequireUppercase = false; 
                opts.Password.RequireDigit = true; 
                opts.User.RequireUniqueEmail = true; 
            })

            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
            services.AddAuthentication().AddTwitter(twitterOptions =>
            {
                twitterOptions.ConsumerKey = Configuration["Authentication:Twitter:ConsumerKey"];
                twitterOptions.ConsumerSecret = Configuration["Authentication:Twitter:ConsumerSecret"];
            })
            .AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId = Configuration["Authentication:Facebook:AppId"];
                facebookOptions.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
            })
            .AddOAuth("VKontakte", vk =>
                {
                    vk.ClientId = Configuration["Authentication:VKontakte:AppId"];
                    vk.ClientSecret = Configuration["Authentication:VKontakte:AppSecret"];
                    vk.Scope.Add("notify");
                    vk.Scope.Add("offline");
                    vk.Scope.Add("photos");
                    vk.CallbackPath = "/signin-vkontakte";
                    vk.AuthorizationEndpoint = "https://oauth.vk.com/authorize";
                    vk.TokenEndpoint = "https://api.vk.com/method/account.getinfo";

                });
            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddMvc()
                    .AddDataAnnotationsLocalization()
                    .AddViewLocalization();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                  
                    new CultureInfo("be"),
                    new CultureInfo("ru")
                };

                options.DefaultRequestCulture = new RequestCulture("ru");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            loggerFactory.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt"));
            var logger = loggerFactory.CreateLogger("FileLogger");

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locOptions.Value);

            app.UseStaticFiles();
         
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                logger.LogInformation("Processing request {0}");
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
