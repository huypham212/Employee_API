using Microsoft.AspNetCore.Builder;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppS3.Middleware
{
    public static class ApplicationBuilderMiddleware
    {
        public static IApplicationBuilder AddMiddlewareInProduction(this IApplicationBuilder app)
        {
            return app;
        }

        public static IApplicationBuilder AddMiddlewareInStaging(this IApplicationBuilder app)
        {
            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseSerilogRequestLogging();

            app.UseSwagger();

            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "AppS3 V1");
            });

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(end =>
            {
                end.MapControllers();
            });

            return app;
        }

        public static IApplicationBuilder AddMiddewaresInDevelopment(this IApplicationBuilder app)
        {
            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseSerilogRequestLogging();

            app.UseSwagger();

            app.UseSwaggerUI(swagger =>
            {
                swagger.SwaggerEndpoint("/swagger/v1/swagger.json", "AppS3 V1");
            });

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(end =>
            {
                end.MapControllers();
            });

            return app;
        }
    }
}
