using System.Net;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using com.primebird.portal.core.infrastructure;
using FluentNHibernate.Cfg;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Util;
using primebird.portal.airtime;


namespace nhibernatespatialtest
{
    /*
        Addons need to be added with AddApplicationPart and in ConfigureServices
    */
    public class Program
    {
        private static AppSessionFactory? appSessionFactory;
        public static void Main(string[] args)
        {
            appSessionFactory = new AppSessionFactory();
            RDBMS rdbms = new RDBMS();
            rdbms.dbType = RDBMS.POSTGRESQL;
            StringBuilder ConnStrBuilder = new StringBuilder();
            ConnStrBuilder.Append("User Id=").Append(Environment.GetEnvironmentVariable("DATABASE_USER")).Append(";");
            ConnStrBuilder.Append("Password=").Append(Environment.GetEnvironmentVariable("DATABASE_USER_PASSWORD")).Append(";");
            ConnStrBuilder.Append("Host=").Append(Environment.GetEnvironmentVariable("DATABASE_HOST")).Append(";");
            ConnStrBuilder.Append("Port=").Append(Environment.GetEnvironmentVariable("DATABASE_PORT")).Append(";");
            ConnStrBuilder.Append("Database=").Append(Environment.GetEnvironmentVariable("DATABASE_NAME"));
            rdbms.connectionstring = ConnStrBuilder.ToString();
            appSessionFactory.SetupRDBMS(rdbms,new List<Assembly>() { typeof(Position).Assembly });

            int port = 5001;

            var builder = WebApplication.CreateBuilder(args);

            
            builder.WebHost.ConfigureKestrel(serverOptions => {
                serverOptions.Listen(IPAddress.Loopback, port);
            });

            IMvcBuilder mvcBuilder = builder.Services
                .AddControllersWithViews();
            
            
            builder.Services
                .AddEndpointsApiExplorer()
                .AddSwaggerGen();


            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<NHibernate.ISession, NHibernate.ISession>(sp => appSessionFactory.OpenSession());

            var app = builder.Build();

            app.UseHsts();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            
            app.UseRouting();
            app.UseCors();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}