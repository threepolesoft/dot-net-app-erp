using Erp.Services;
using Erp.Services.TransferService;
using Erp.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components.Web;

var builder = WebApplication.CreateBuilder(args);
builder.Environment.EnvironmentName= builder.Configuration["Environments"];

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddAntDesign();

builder.Services.AddSignalR(); // Add SignalR
builder.Services.AddScoped<SignalRService>();

#region Add authentication services
builder.Services.AddAuthorizationCore();
builder.Services.AddAuthentication();

builder.Services.AddDistributedMemoryCache(); // Required for session
builder.Services.AddScoped<ProtectedSessionStorage>();
builder.Services.AddScoped<LocalStorageService>();
builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
#endregion Add authentication services

builder.Services.AddScoped<SettingTransferService>();
builder.Services.AddScoped<UserTransferService>();
builder.Services.AddScoped<RoleTransferService>();
builder.Services.AddScoped<MenuTransferService>();
builder.Services.AddScoped<MenuLeftTransferService>();
builder.Services.AddScoped<BrandTransferServic>();
builder.Services.AddScoped<CategoryTS>();
builder.Services.AddScoped<ColorTS>();
builder.Services.AddScoped<SizeTS>();
builder.Services.AddScoped<UnitTS>();
builder.Services.AddScoped<RestService>();

builder.Services.AddSingleton<PluginLoader>();

builder.Services.AddServerSideBlazor()
    .AddCircuitOptions(options => options.DetailedErrors = true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
