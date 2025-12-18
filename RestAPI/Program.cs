using AppBLL.InventoryServices;
using AppBLL.Services;
using AppDAL.Db;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.WebSockets;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestAPI.Hubs;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
string MyAllowSpecificOrigins = "AllowSpecificOrigins";
//string MyAllowSpecificOrigins = "AllowAll";
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddResponseCompression();

// Register the DbContext
builder.Services.AddDbContext<PifErpDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("pifErpDbConnection")));


builder.Services.AddControllers().AddJsonOptions(options =>
{
    // Use original property casing (PascalCase)
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});

// Add WebSocket support
builder.Services.AddSignalR();  // Add SignalR services

#region Add authentication services
var key = Encoding.ASCII.GetBytes(builder.Configuration["SigningKey"].ToString());  // Replace with your secret key
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = "melabari.com",  // Replace with your issuer
                        ValidAudience = "melabari.com",  // Replace with your audience
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                    };
                });

string[] origins = builder.Configuration["Origins"].Split(',');

builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins,
         all =>
         {
             all.AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .WithOrigins(origins) // Add your client domain here
                .SetIsOriginAllowedToAllowWildcardSubdomains();
         });
});
#endregion Add authentication services

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<SettingService>();
builder.Services.AddScoped<SettingUserService>();
builder.Services.AddScoped<SettingDeviceService>();
builder.Services.AddScoped<RoleService>();
builder.Services.AddScoped<RoleUserService>();
builder.Services.AddScoped<ShareService>();
builder.Services.AddScoped<MenuService>();
builder.Services.AddScoped<MenuDetailService>();
builder.Services.AddScoped<RoleMenuService>();
builder.Services.AddScoped<ApplicationUserService>();
builder.Services.AddScoped<UsersDevicesService>();

#region inventory services
builder.Services.AddScoped<BrandService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<ColorService>();
builder.Services.AddScoped<SizeService>();
builder.Services.AddScoped<UnitService>();
#endregion inventory services

var app = builder.Build();
app.UseCors(MyAllowSpecificOrigins);
// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseRouting();  // Enable routing

app.UseAuthentication();
app.UseAuthorization();
app.UseResponseCompression();
app.UseStaticFiles();
app.UseWebSockets();  // Enable WebSocket support

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<UserHub>("/user");  // Client WebSocket-like hub
});
app.MapControllers();

app.Run();
