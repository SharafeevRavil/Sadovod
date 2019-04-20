using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SadovodBack.Models;

[assembly: HostingStartup(typeof(SadovodBack.Areas.Identity.IdentityHostingStartup))]
namespace SadovodBack.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<SadovodBackContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("SadovodBackContextConnection")));

                services.AddDefaultIdentity<IdentityUser>()
                    .AddEntityFrameworkStores<SadovodBackContext>();
            });
        }
    }
}