using System.Text;
using dotnetcore7_webapi_authentication.Data;
using dotnetcore7_webapi_authentication.Services;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<ICustomerService, CustomerService>();

builder.Services.AddDbContext<Dotnetcore7WebapiAuthenticationDbContext>();
builder.Services.AddAuthentication().AddJwtBearer(options =>
 options.TokenValidationParameters = new TokenValidationParameters
 {
     ValidateIssuerSigningKey = true,
     ValidateAudience = false,
     ValidateIssuer = false,
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

app.UseAuthorization();

app.MapControllers();

app.Run();
