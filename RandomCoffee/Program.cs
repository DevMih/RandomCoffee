using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RandomCoffee.Data;
using RandomCoffee.Entities;
using RandomCoffee.Helpers;
using RandomCoffee.Interfaces;
using RandomCoffee.Services;
using System.Configuration;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// Add services to the container.

builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPhotoService, PhotoService>();
builder.Services.AddSingleton<SmtpService>();
builder.Services.AddSingleton<TimerService>();
builder.Services.AddSingleton<MatchingService>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(connectionString);
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
builder.Services.AddIdentityCore<AppUser>(opt =>
{
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequireDigit = false;
    opt.Password.RequireUppercase = false;
    opt.Password.RequiredLength = 4;
})
    .AddRoles<AppRole>()
    .AddRoleManager<RoleManager<AppRole>>()
    .AddSignInManager<SignInManager<AppUser>>()
    .AddRoleValidator<RoleValidator<AppRole>>()
    .AddEntityFrameworkStores<DataContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenKey"])),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DataContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
    await context.Database.MigrateAsync();
    await Seed.SeedUsers(userManager, roleManager);
    var timerService = scope.ServiceProvider.GetRequiredService<TimerService>();
    timerService.StartAsync(CancellationToken.None);
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().WithOrigins("http://localhost:4200"));

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
