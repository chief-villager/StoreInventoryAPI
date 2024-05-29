using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using storeInventoryApi;
using storeInventoryApi.Models;
using storeInventoryApi.Repository;
using storeInventoryApi.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();
var connection = builder.Configuration.GetConnectionString("MyConnection");

builder.Services.AddDbContext<ApplicationDbContext>();

builder.Services.AddHttpContextAccessor();


builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddScoped<IProductService, ProductService>();


builder.Services.AddScoped<IUserService, UserService>();





builder.Services.AddIdentity<ApplicationUser, IdentityRole<string>>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole<string>>()
    .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();



builder.Services.Configure<IdentityOptions>(options =>
{

    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;


    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;


    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(options =>
{
    // Configure cookie options
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
    options.LoginPath = "/User/Login"; // Specify the login page URL
    options.AccessDeniedPath = "/User/Login"; // Specify the access denied page URL
    options.SlidingExpiration = true;
});


builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(10);

    options.LoginPath = "/User/Login";
    options.AccessDeniedPath = "/User/Login";
    options.SlidingExpiration = true;
});





var app = builder.Build();

// using (var scope = app.Services.CreateScope())
// {
//     var services = scope.ServiceProvider;
//     var context = services.GetRequiredService<ApplicationDbContext>();
//     try
//     {
//         context.Database.Migrate(); // Apply pending migrations
//         await SeedData.InitializeAsync(services); // Seed the roles
//     }
//     catch (Exception ex)
//     {
//         throw new Exception(ex.Message);
//     }
// }
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
