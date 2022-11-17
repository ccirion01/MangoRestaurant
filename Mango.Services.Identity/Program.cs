using Duende.IdentityServer.Services;
using Mango.Services.Identity;
using Mango.Services.Identity.DbContexts;
using Mango.Services.Identity.Initializer;
using Mango.Services.Identity.Models;
using Mango.Services.Identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Database configuration
builder.Services.AddDbContext<ApplicationDbContext>(op =>
    op.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Add aspnet identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

//Add identity provider
var identityBuilder = builder.Services.AddIdentityServer(options =>
{
    options.Events.RaiseErrorEvents = true;
    options.Events.RaiseInformationEvents = true;
    options.Events.RaiseFailureEvents = true;
    options.Events.RaiseSuccessEvents = true;
    options.EmitStaticAudienceClaim = true; //This will generate an aud claim in the issuer_name/resources format
})
    .AddInMemoryIdentityResources(SD.IdentityResources)
    .AddInMemoryApiScopes(SD.ApiScopes)
    .AddInMemoryClients(SD.Clients)
    .AddAspNetIdentity<ApplicationUser>(); //We want it to use the asp net identity that we previously configured

identityBuilder.AddDeveloperSigningCredential();

builder.Services.AddScoped<IDbInitializer, DbInitializer>();
builder.Services.AddScoped<IProfileService, ProfileService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) 
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer();

app.UseAuthorization();

Seed();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


void Seed()
{
    using var scope = app.Services.CreateScope();
    var initializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
    initializer.Initialize();
}