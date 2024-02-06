using System.Text;
using dotnetcore7_webapi_authentication.Data;
using dotnetcore7_webapi_authentication.Services;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<ICustomerService, CustomerService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("cors1", policy =>
    {
        policy.AllowAnyHeader().AllowAnyMethod().AllowCredentials().SetIsOriginAllowed(host => true);
    });
});

builder.Services.AddDbContext<Dotnetcore7WebapiAuthenticationDbContext>();
builder.Services.AddAuthentication().AddJwtBearer(options =>
 options.TokenValidationParameters = new TokenValidationParameters
 {
     ValidateIssuerSigningKey = true,
     ValidateAudience = false,
     ValidateIssuer = false,
     ValidateLifetime = true,
     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JwtSettings:SecretKey").Value!))
 });

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("cors1");

app.UseAuthorization();

app.MapControllers();

app.Run();
