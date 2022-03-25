using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace integracionesTecni
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional:true, reloadOnChange: true);

            Configuration = builder.Build();

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var Server = Configuration.GetSection("ConnectionStrings:Server");
            var UserName = Configuration.GetSection("ConnectionStrings:UserName");
            var Password = Configuration.GetSection("ConnectionStrings:Password");
            var CompanyDB = Configuration.GetSection("ConnectionStrings:CompanyDB");
            var SQL  = Configuration.GetSection("ConnectionStrings:SQL");
            var DbUserName = Configuration.GetSection("ConnectionStrings:DbUserName");
            var DbPassword = Configuration.GetSection("ConnectionStrings:DbPassword");
            Modelos.Conexion cnn = new Modelos.Conexion();
            cnn.Server = Server.Value;
            cnn.UserName = UserName.Value;
            cnn.Password = Password.Value;
            cnn.CompanyDB = CompanyDB.Value;
            cnn.SQL = SQL.Value;
            cnn.DbUserName = DbUserName.Value;
            cnn.DbPassword = DbPassword.Value;
            services.AddControllers().AddXmlSerializerFormatters();
            //services.AddControllers();
            services.AddSingleton<SAP.Interfaces.IConexionService>(sp => new SAP.ConexionService(cnn));
            services.AddScoped<SAP.Interfaces.IClienteService, SAP.ClienteService>();
            services.AddScoped<SAP.Interfaces.IOrdenService, SAP.OrdenService>();
            services.AddScoped<SAP.Interfaces.IPagoService, SAP.PagoService>();
            services.AddScoped<SAP.Interfaces.IFacturaService, SAP.FacturaService>();
            services.AddScoped<SAP.Interfaces.IContactoService, SAP.ContactoService>();
            services.AddScoped<SAP.Interfaces.ISalidaService, SAP.SalidaService>();
            services.AddScoped<SAP.Interfaces.IFacturaItemsService, SAP.FacturaItemsService>();
            services.AddScoped<SAP.Interfaces.IArticuloService, SAP.ArticuloService>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "integracionesTecni", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment() || env.IsProduction())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "integracionesTecni v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
