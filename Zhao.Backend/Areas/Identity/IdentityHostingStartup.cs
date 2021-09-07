using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zhao.Backend.Areas.Identity.Data;

[assembly: HostingStartup(typeof(Zhao.Backend.Areas.Identity.IdentityHostingStartup))]
namespace Zhao.Backend.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddDbContext<TpIdentityDataContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("DefaultConnection")));

                services.AddIdentity<IdentityUser, IdentityRole>()
                    .AddDefaultTokenProviders()
                    .AddErrorDescriber<ErreurIdentityEnFr>()
                    .AddDefaultUI()
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<TpIdentityDataContext>();
            });
        }
    }
}