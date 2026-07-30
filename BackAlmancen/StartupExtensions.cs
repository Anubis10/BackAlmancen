using BackAlmancen.Application;
using BackAlmancen.Filters;
using BackAlmancen.Persistence;

namespace BackAlmancen
{
    public static class StartupExtensions
    {
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddApplicationServices();
            builder.Services.AddPersistenceServices(builder.Configuration);
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();


            var info = new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                Version = "v1"
            };
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", info);
            });
            builder.Services.AddMvc(
                                   options =>
                                   {

                                       options.Filters.Add<ValidationFilter>();
                                   }
               );
            #region CORS

            var originesPermitidos = builder.Configuration.GetValue<string>("OrigenesPermitidos")!.Split(",");

            builder.Services.AddCors(opciones =>
            {
                opciones.AddDefaultPolicy(politica =>
                {

                    politica.WithOrigins(originesPermitidos)
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                });
            });
            #endregion

            return builder.Build();
        }

        public static WebApplication ConfigurePipeline(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BackAlmancen BackEnd"));
                });
            }


            app.UseAuthorization();

            app.MapControllers();

            app.MapGet("/", () => "Running...");


            return app;
        }

    }
}
