using BLL;

using WebShop01.Interfaces;
using WebShop01.Services;


using Serilog;
using Serilog.Events;
using DAL_V2;
using Microsoft.EntityFrameworkCore;

namespace WebShop01
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            IConfiguration configuration = builder.Configuration;
            builder.Services.AddDbContext<EntityDatabase>();
            builder.Services.AddBusinessLogicLayer(configuration);
            builder.Services.AddAutoMapper(typeof(AutoMapperProfile));


            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages().AddRazorRuntimeCompilation();


            builder.Host.UseSerilog((ctx, lg) =>
            {
                lg.WriteTo.Console(LogEventLevel.Error);
                lg.WriteTo.Seq("http://localhost:4000", LogEventLevel.Error);
            });


            var app = builder.Build();



            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Shop}/{action=Index}/{category?}");

            app.Run();
        }
    }
}