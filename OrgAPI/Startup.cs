using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using OrgDAL;

namespace OrgAPI
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddDbContext<OrganizationDbContext>();
            services.AddSwaggerDocument();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseOpenApi();
            app.UseSwaggerUi3();
            app.UseExceptionHandler(
                          options =>
                          {
                              options.Run(async context =>
                              {

                                  context.Response.StatusCode = 500;//Internal Server Error
                                   context.Response.ContentType = "application/json";
                                  //await context.Response.WriteAsync("We are woirking on it");
                                  var ex = context.Features.Get<IExceptionHandlerFeature>();
                                  if (ex != null)
                                  {
                                      await context.Response.WriteAsync(ex.Error.Message);
                                  }
                              });
                          }
                          );
            app.UseMvc();
        }
    }
}
