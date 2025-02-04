using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using WebApi.Data;
using WebApi.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Add Swagger + config
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});

// Database connection
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlite($"Data Source=.\\Data/SqliteTestDb.db"));

// Authorization
builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    .AddEntityFrameworkStores<DataContext>();

// Services

builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ICommonRepository, CommonRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        options.RoutePrefix = string.Empty;
    });
}

MapsterConfig.RegisterMappings();

// Add Identity API
app.MapIdentityApi<IdentityUser>();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
