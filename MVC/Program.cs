using MVC.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSession();

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<HttpClient>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<PostService>();
builder.Services.AddScoped<LoginService>();
builder.Services.AddScoped<CommonApiService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
