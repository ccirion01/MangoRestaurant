using Mango.Services.ProductAPI.Configs;
using Mango.Services.ProductAPI.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = builder.Configuration["IdentityServerUrl"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false //Why not?
        };
    });

builder.Services.AddAuthorization(options =>
{
    //We're not gonna be validating this in our requests, but it is an example of how authorization policies and scopes can be validated
    options.AddPolicy("ApiScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "mangoAdmin");
    }); //We could add the option to validate against this or other policies in the [Authorize] attribute
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Mango.Services.ProductAPI" });
    c.EnableAnnotations();
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme 
    {
        Description = @"Enter 'Bearer' [space] and your token",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    
    //This is how we handle security with openid in Swagger
    c.AddSecurityRequirement(new OpenApiSecurityRequirement 
    {
        {
            new OpenApiSecurityScheme 
            {
                Reference = new OpenApiReference 
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

//Database configuration
builder.Services.AddDbContext<ApplicationDbContext>(op =>
    op.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Automapper configuration
builder.Services.RegisterMaps();

//Services registration
builder.Services.RegisterServices();

var app = builder.Build();

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
