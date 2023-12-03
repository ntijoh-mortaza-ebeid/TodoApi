using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TodoApi.Models;

var builder = WebApplication.CreateBuilder(args);

{
    builder.Services.AddControllers();
    builder.Services.AddDbContext<TodoContext>(opt =>
    {
        opt.UseNpgsql("Host=localhost;Database=pg-docker;Username=postgres;Password=docker");
    });
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.EnableAnnotations();
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "Todo API",
            Description = "An ASP.NET Core Web API for managing Todo items",
            Contact = new OpenApiContact
            {
                Name = "GITHUB page for Todo API",
                Url = new Uri("https://github.com/ntijoh-mortaza-ebeid/TodoApi")
            }
        });
    });
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

{
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}
