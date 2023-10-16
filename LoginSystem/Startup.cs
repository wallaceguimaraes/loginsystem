using System.Text;
using LoginSystem.Extensions.Filters;
using LoginSystem.Extensions.Infrastructure.Extensions;

namespace api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureApp(services);
        }

        public void ConfigureApp(IServiceCollection services)
        {
            services.AddTransientServices();
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IOTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(typeof(CustomAuthorizationAttribute));
            });
            services.AddRazorPages();
        }

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
            app.UseSession();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllerRoute(
                                          name: "default",
                                          pattern: "{controller=Login}/{action=Index}/{id?}"));
            app.UseAuthentication();
        }
    }
}

