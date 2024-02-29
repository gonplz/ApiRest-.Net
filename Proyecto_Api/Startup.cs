using Proyecto_Api.HandlerException;

namespace Proyecto_Api
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {

        }
        //Invesigar más
        [Obsolete]
        public void Configure (IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

           app.UseMiddleware<LoggingMiddleware>();

            app.UseRouting();

            app.UseEndpoints(endpoints => { 
            
            endpoints.MapControllers();
            
            });
        }
    }
}
