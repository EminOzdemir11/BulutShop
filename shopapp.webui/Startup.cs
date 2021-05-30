using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using shopapp.business.Abstract;
using shopapp.business.Concrete;
using shopapp.data.Abstract;
using shopapp.data.Concrete.EfCore;
using shopapp.webui.EmailServices;
using shopapp.webui.Identity;

namespace shopapp.webui
{
    public class Startup
    {
        private IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>(options=> options.UseSqlite("Data Source=shopDb"));
            services.AddIdentity<User,IdentityRole>().AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();
            
            services.Configure<IdentityOptions>(options=> {
                //password
                options.Password.RequireDigit = false; //sayı olmak zorunda
                options.Password.RequireLowercase = false; // küçük harf olmak zorunda
                options.Password.RequireUppercase = false; // büyük harf olmak zorunda
                options.Password.RequiredLength = 3; // minimum 6 karakter olmalı
                options.Password.RequireNonAlphanumeric = false; // alfanümerik bir işaret olmak zorunda değil

                //lockout
                options.Lockout.MaxFailedAccessAttempts = 50; // en fazla 5 defa yanlış şifre girilebilir
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // 5 dakika sonra tekrar şifre denenebilir
                options.Lockout.AllowedForNewUsers = false; // kilitleme aktif edilmez

                //options.User.AllowedUserNameCharacters = "";
                options.User.RequireUniqueEmail = true; // bir mail adresi ile tek bir hesap açılabilir
                options.SignIn.RequireConfirmedEmail = true; // mail adresinin onaylanmasını gerektirmez
                options.SignIn.RequireConfirmedPhoneNumber = false; // telefon numarası onayı gerektirmez

            });

            services.ConfigureApplicationCookie(options=> {
                options.LoginPath = "/account/login";
                options.LogoutPath = "/account/logout";
                options.AccessDeniedPath = "/account/accessdenied";
                options.SlidingExpiration = true; // sayfada işlem yapmak için verilen süre izni(varsayılan: 20 dk)
                options.ExpireTimeSpan = TimeSpan.FromDays(30); // bu süre 30 gün olarak ayarlandı
                options.Cookie = new CookieBuilder
                {
                    HttpOnly = true,
                    Name = ".ShopApp.Security.Cookie",
                    SameSite = SameSiteMode.Strict
                };

            });

            services.AddScoped<ICategoryRepository,EfCoreCategoryRepository>();
            services.AddScoped<IProductRepository,EfCoreProductRepository>();

            services.AddScoped<ICategoryService,CategoryManager>();
            services.AddScoped<IProductService,ProductManager>();

            services.AddScoped<IEmailSender,SmtpEmailSender>(i=>
                new SmtpEmailSender(
                    _configuration["IEmailSender:Host"],
                    _configuration.GetValue<int>("IEmailSender:Port"),
                    _configuration.GetValue<bool>("IEmailSender:EnableSSL"),
                    _configuration["IEmailSender:UserName"],
                    _configuration["IEmailSender:Password"]
                )
            );

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "node_modules")),
                    RequestPath = "/modules"
            });

            if (env.IsDevelopment())
            {
                SeedDatabase.Seed();
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "adminproducts",
                    pattern: "admin/products",
                    defaults: new {controller="Admin",action="ProductList"}
                );

                endpoints.MapControllerRoute(
                    name: "adminproductcreate",
                    pattern: "admin/products/create",
                    defaults: new {controller="Admin",action="ProductCreate"}
                );

                endpoints.MapControllerRoute(
                    name: "adminproductedit",
                    pattern: "admin/products/{id?}",
                    defaults: new {controller="Admin",action="ProductEdit"}
                );

                endpoints.MapControllerRoute(
                    name: "admincategories",
                    pattern: "admin/categories",
                    defaults: new {controller="Admin",action="CategoryList"}
                );

                endpoints.MapControllerRoute(
                    name: "admincategoriescreate",
                    pattern: "admin/categories/create",
                    defaults: new {controller="Admin",action="CategoryCreate"}
                );

                endpoints.MapControllerRoute(
                    name: "admincategoryedit",
                    pattern: "admin/categories/{id?}",
                    defaults: new {controller="Admin",action="CategoryEdit"}
                );

                //localhost/about
                endpoints.MapControllerRoute(
                    name: "search",
                    pattern: "search",
                    defaults: new {controller="Shop",action="search"}
                );

                endpoints.MapControllerRoute(
                    name: "productdetails",
                    pattern: "{url}",
                    defaults: new {controller="Shop",action="details"}
                );

                endpoints.MapControllerRoute(
                    name: "products",
                    pattern: "products/{category?}",
                    defaults: new {controller="Shop",action="list"}
                );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                );
            });
        }
    }
}
