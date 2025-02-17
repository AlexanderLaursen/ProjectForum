using MVC.Repositories;
using MVC.Repositories.Interfaces;
using MVC.Services;
using MVC.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSession();

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<HttpClient>();
builder.Services.AddScoped<PostService>();
builder.Services.AddScoped<CommentServiceOld>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<CommonApiService>();
builder.Services.AddScoped<CommentHistoryApiService>();
builder.Services.AddScoped<PostHistoryApiService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<LikeService>();
builder.Services.AddScoped<SearchService>();
builder.Services.AddScoped<ApiRepository>();

builder.Services.AddScoped<IApiRepository, ApiRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();

builder.Services.AddScoped<IPostHistoryApiService, PostHistoryApiService>();
builder.Services.AddScoped<ICommentHistoryApiService, CommentHistoryApiService>();
builder.Services.AddScoped<ICategoryApiService, CategoryApiService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<HttpContextService>();

builder.Services.AddServerSideBlazor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapBlazorHub();

app.Run();
