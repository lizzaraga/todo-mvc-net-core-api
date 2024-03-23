using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Todo_API.Data;
using Todo_API.Models.Utils;
using Todo_API.Services;
using Todo_API.Services.Abstracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
} );

builder.Logging.AddConsole();
builder.Services.Configure<AppJwtInfo>(builder.Configuration.GetSection("Jwt"));


builder.Services.AddIdentity<IdentityUser, IdentityRole>(
        options =>
        {
            options.User.RequireUniqueEmail = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireDigit = true;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 3;

            options.SignIn.RequireConfirmedEmail = true;
       
        }
    ).AddEntityFrameworkStores<TodoDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(op =>
{
    op.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    var configuration = builder.Configuration;
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        ValidAudience = configuration["Jwt:Audience"],
        ValidIssuer = configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]))
    };
});
builder.Services.AddControllers(); 
builder.Services.AddDbContext<TodoDbContext>(optionsBuilder =>
{
    optionsBuilder.UseNpgsql(builder.Configuration.GetConnectionString("DBConnectionString"));
});


builder.Services.AddAuthorization();

builder.Services.AddScoped<ITodoService, TodoService>();

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
app.UseStaticFiles();
app.Run();

