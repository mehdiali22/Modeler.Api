using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Modeler.Api.Persistence;

namespace Modeler.Api;

public sealed class Startup
{
    private readonly IConfiguration _cfg;

    public Startup(IConfiguration cfg)
    {
        _cfg = cfg;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        services.AddDbContext<ModelerDbContext>(opt =>
            opt.UseSqlServer(_cfg.GetConnectionString("Default")));

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddCors(opt =>
        {
            opt.AddPolicy("ui", p => p.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
        });
    }

    public void Configure(IApplicationBuilder app, IHostEnvironment env)
    {
        app.UseCors("ui");

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseRouting();
        app.UseAuthorization();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}
